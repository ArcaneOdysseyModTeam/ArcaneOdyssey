using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Imbues;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Lost;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Normal;
using ArcaneOdyssey.Content.Projectiles;
using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey
{
	public class ProjectileManager : GlobalProjectile
	{
		public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
		{
			if (projectile.TryGetImbue(out Imbuable imbue))
			{
				var spell = projectile.ModProjectile is MagicSpell;
				modifiers.FinalDamage += (!spell ? imbue.AOImbueDamage : imbue.AOScrollDamage).MultiToPercent();
				if (imbue is CrystalMagic && target.HasBuff<Crystallized>() && GetAOBuffStack(target, target.FindBuffIndex(ModContent.BuffType<Crystallized>())) == 4)
				{
					modifiers.FinalDamage += .3f;
				}


				foreach (var debuff in imbue.ImbueDebuffs)
				{
					if ((debuff.debuffPercent == 0) || modifiers.GetDamage(projectile.damage, true) > (target.lifeMax / debuff.debuffPercent))
					{
						target.AddBuff(debuff.debuffID, debuff.debuffDuration);
					}
				}

				if (imbue.CombinedDebuffs is not null)
				{
					foreach (CombinedDebuff buffkeys in imbue.CombinedDebuffs)
					{
						if (target.HasBuff(buffkeys.requirement) || (buffkeys.requirement == BuffID.Wet && target.wet))
						{
							target.AddBuff(buffkeys.result, buffkeys.duration);
						}
					}
				}

				foreach (MagicBuffMultiplier multiplier in imbue.Effects.magicBuffMultipliers)
				{
					if (target.HasBuff(multiplier.buffID) || (multiplier.buffID == BuffID.Wet && target.wet))
					{
						modifiers.FinalDamage += multiplier.multiplier.MultiToPercent();
					}
				}

				if (Main.netMode == NetmodeID.SinglePlayer) // things would get chaotic in multiplayer if everyone kept clearing eachothers debuffs
				{
					foreach (int buffid in imbue.Effects.clearBuffs)
					{
						if (target.HasBuff(buffid))
						{
							target.DelBuff(target.FindBuffIndex(buffid));
						}
					}
				}
			}
		}
		

		public override void ModifyDamageHitbox(Projectile projectile, ref Rectangle hitbox)
		{
			if (projectile.hostile || projectile.npcProj || projectile.owner == 255 || projectile.damage <= 0)
				return;
			Player player = Main.player[projectile.owner];
			Vector2 dim = projectile.ArcaneOdyssey().OriginalDimensions.GetValueOrDefault(projectile.Size);
			float mult = projectile.ArcaneOdyssey().BaseScale.GetValueOrDefault(1f);
			if (projectile.ModProjectile is AOPlayerProjectile proj)
				mult += proj.AOSize.MultiToPercent();
			if (projectile.TryGetImbue(out Imbuable imbue))
			{
				mult += (projectile.ModProjectile is MagicSpell ? imbue.AOScrollSize : imbue.AOImbueSize).MultiToPercent();
			}
			mult += player.ArcaneOdyssey().SizeMulti;
			if (projectile.ModProjectile is null or AOPlayerProjectile || ArcaneOdysseyConfig.Instance.AffectsOtherMods)
			{
				hitbox.Width = (int)(dim.X * mult);
				hitbox.Height = (int)(dim.Y * mult);
				projectile.scale = mult;
				if (projectile.ModProjectile is BaseStaffProjectile)
				{
					hitbox.Width = (int)(dim.X * mult * 1.5f);
					hitbox.Height = (int)(dim.Y * mult * 1.5f);
					hitbox.X -= hitbox.Width / 3;
					hitbox.Y -= hitbox.Height / 3;
				}
			}
		}

		public override bool PreKill(Projectile projectile, int timeLeft)
		{
			if (!Main.dedServ)
			{
				if (projectile.TryGetImbue(out Imbuable imbue) && imbue.PreEffects(projectile))
				{
					if (projectile.ModProjectile is not ExplosionSpell && projectile.ModProjectile is not ExplosionTracker)
						imbue.KillEffects(projectile);
				}
			}
			return base.PreKill(projectile, timeLeft);
		}

		public override bool PreDraw(Projectile projectile, ref Color lightColor)
		{
			bool returntype = true;
			if ((Main.player[projectile.owner].ArcaneOdyssey().Imbue is PoisonMagic || Main.player[projectile.owner].ArcaneOdyssey().Imbue is PoisonLightningMagic) && (projectile.type == ProjectileID.SporeGas || projectile.type == ProjectileID.SporeGas2 || projectile.type == ProjectileID.SporeGas3))
			{
				Main.instance.LoadProjectile(projectile.type);
				var asset = TextureAssets.Projectile[projectile.type];
				Main.EntitySpriteDraw(asset.Value, projectile.Center - Main.screenPosition, null, Color.DarkViolet, projectile.rotation, projectile.GetDrawOriginCentre(), projectile.scale * 1.12f, SpriteEffects.None);
				returntype = false;
			}

			else if (Main.player[projectile.owner].ArcaneOdyssey().Imbue is AshMagic && projectile.type == ProjectileID.SporeCloud)
			{
				Main.instance.LoadProjectile(projectile.type);
				var asset = TextureAssets.Projectile[projectile.type];
				Main.EntitySpriteDraw(asset.Value, projectile.Center - Main.screenPosition, new(0, 30 * projectile.frame, 28, 30), Color.DarkRed, projectile.rotation, projectile.GetDrawOriginCentre(), projectile.scale, SpriteEffects.None);
				returntype = false;
			}

			return returntype; 
		}
	}
	public class AOProjectile : GlobalProjectile, IImbuableEntity
	{
		public override bool InstancePerEntity => true;
		public float? BaseScale = null;
		public Vector2? OriginalDimensions = null;
		public Imbuable Imbue { get; set; }
		public Projectile thisProjectile = null;

		private bool? _cold = null;
		public bool? Cold { get 
			{
				if (thisProjectile is not null && thisProjectile.ModProjectile is AOPlayerProjectile proj && proj.Cold.HasValue)
				{
					return proj.Cold.Value;
				}
				return _cold;
			} set => _cold = value;
        }

        public override void OnSpawn(Projectile projectile, IEntitySource source)
		{
			thisProjectile = projectile;
			OriginalDimensions ??= projectile.Size;
			BaseScale ??= projectile.scale;
			if (ImbueClassCheck(projectile))
			{
				if (source is EntitySource_Parent { Entity: Projectile proj })
				{
					Imbue ??= proj.ArcaneOdyssey().Imbue;
					Cold ??= proj.ArcaneOdyssey().Cold;
				}
				else if (source is EntitySource_ItemUse { Item: Item item })
				{
					if (item.TryGetGlobalItem<AOItem>(out var aOItem))
					{
						Imbue ??= aOItem.Imbue;
						Cold ??= aOItem.Cold;
					}
				}
				else if (source is EntitySource_Parent { Entity: Player player })
				{
					Imbue ??= player.ArcaneOdyssey().Imbue;
				}

				if (Imbue is not null && Cold.HasValue && Imbue.Cold.HasValue && (Cold.Value != Imbue.Cold.Value))
				{
					Imbue = SteamImbue.Create(Imbue);
				}

                if (Imbue is not null && Imbue.PreEffects(projectile))
                {
                    if (projectile.DamageType != DamageClass.MeleeNoSpeed)
                        projectile.velocity *= projectile.ModProjectile is MagicSpell ? Imbue.AOScrollSpeed : Imbue.AOImbueSpeed;
                    if (projectile.ModProjectile is not ExplosionSpell && projectile.ModProjectile is not ExplosionTracker)
                        Imbue.SpawningEffects(projectile);
                }
            }
		}

		public override bool PreAI(Projectile projectile)
		{
			thisProjectile = projectile;
			if (projectile.numUpdates < 1 && Main.netMode == NetmodeID.MultiplayerClient)
			{
				OriginalDimensions ??= projectile.Size;
				BaseScale ??= projectile.scale;
				if (ImbueClassCheck(projectile))
					Imbue ??= Main.player[projectile.owner].ArcaneOdyssey().Imbue;
			}
            projectile.coldDamage = Cold.GetValueOrDefault(false) || (Imbue is not null && Imbue.Cold.GetValueOrDefault(false));
			return true;
		}

        public override void AI(Projectile projectile)
        {
            if (projectile.owner == Main.myPlayer && (projectile.numUpdates % (projectile.extraUpdates + 1) == 0))
            {
                if (Imbue is not null && Imbue.PreEffects(projectile))
                {
                    Imbue.LingeringEffects(projectile);
                }
            }
        }
	}
}
