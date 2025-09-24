using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Items;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Magic;
using ArcaneOdyssey.Content.Projectiles;
using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic;
using ArcaneOdyssey.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using rail;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.UI.ModBrowser;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey
{
	public class ProjectileManager : GlobalProjectile
	{
		public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
		{
			if (projectile.owner == Main.myPlayer && (ArcaneOdysseyConfig.Instance.IgnoredProjectiles is null || !ArcaneOdysseyConfig.Instance.IgnoredProjectiles.Contains(projectile.Name)))
			{
				if (projectile.TryGetImbue(out Imbuable imbue))
				{
					var spell = projectile.ModProjectile is MagicSpell;
					if (spell)
						modifiers.FinalDamage += ((projectile.damage + (BossesKilled * 2f)) / projectile.damage) - 1;
					modifiers.FinalDamage += (!spell ? imbue.AOImbueDamage : imbue.AOScrollDamage).MultiToPercent();
					if (imbue is CrystalMagic && target.HasBuff<Crystallized>() && GetAOBuffStack(target, target.FindBuffIndex(ModContent.BuffType<Crystallized>())) == 4)
					{
						modifiers.FinalDamage += .3f;
					}

					if ((imbue.ImbueDebuff is not null) && (imbue.ImbueDebuff.DebuffPercent != 0f))
					{
						if (imbue.ImbueDebuff.DebuffPercent is null || modifiers.GetDamage(projectile.damage, true) > (target.lifeMax / imbue.ImbueDebuff.DebuffPercent))
						{
							target.AddBuff(imbue.ImbueDebuff.debuffID, imbue.ImbueDebuff.debuffDuration);
						}
					}

					if ((imbue.ImbueDebuff2 is not null) && (imbue.ImbueDebuff2.DebuffPercent != 0f))
					{
						if (imbue.ImbueDebuff2.DebuffPercent is null || modifiers.GetDamage(projectile.damage, true) > (target.lifeMax / imbue.ImbueDebuff2.DebuffPercent))
						{
							target.AddBuff(imbue.ImbueDebuff2.debuffID, imbue.ImbueDebuff2.debuffDuration);
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
		}

		public override void ModifyDamageHitbox(Projectile projectile, ref Rectangle hitbox)
		{
			Player player = Main.player[projectile.owner];
			Vector2 dim = projectile.ArcaneOdyssey().OriginalDimensions.GetValueOrDefault(projectile.Size);
			float mult = projectile.ArcaneOdyssey().BaseScale.GetValueOrDefault(1f);
			if (projectile.ModProjectile is AOPlayerProjectile proj)
				mult += proj.AOSize.MultiToPercent();
			if (projectile.TryGetImbue(out Imbuable imbue))
			{
				mult += (projectile.ModProjectile is MagicSpell ? imbue.AOScrollSize : imbue.AOImbueSize).MultiToPercent();
			}
			mult += player.ArcaneOdyssey().GetSizeMulti(projectile).MultiToPercent();
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

		public override void OnSpawn(Projectile projectile, IEntitySource source)
		{
			if (projectile.TryGetImbue(out Imbuable imbue) && imbue.PreEffects(projectile) && source is not EntitySource_Parent { Entity: NPC })
			{
				if (projectile.DamageType != DamageClass.MeleeNoSpeed)
					projectile.velocity *= projectile.ModProjectile is MagicSpell ? imbue.AOScrollSpeed : imbue.AOImbueSpeed;
				if (projectile.ModProjectile is not ExplosionSpell && projectile.ModProjectile is not ExplosionTracker)
					imbue.SpawningEffects(projectile);
			}
		}

		public override void AI(Projectile projectile)
		{
			if (projectile.owner == Main.myPlayer)
			{
				if (projectile.TryGetImbue(out Imbuable imbue) && imbue.PreEffects(projectile))
				{
					if (projectile.ModProjectile is not ExplosionSpell && projectile.ModProjectile is not ExplosionTracker)
						imbue.LingeringEffects(projectile);
				}
			}
		}

		public override void OnKill(Projectile projectile, int timeLeft)
		{
			if (projectile.owner == Main.myPlayer)
			{
				if (projectile.TryGetImbue(out Imbuable imbue) && imbue.PreEffects(projectile))
				{
					if (projectile.ModProjectile is not ExplosionSpell && projectile.ModProjectile is not ExplosionTracker)
					imbue.KillEffects(projectile);
				}
			}
		}

		public override bool PreDraw(Projectile projectile, ref Color lightColor)
		{
			bool returntype = true;
			if (Main.player[projectile.owner].ArcaneOdyssey().imbue is PoisonMagic && (projectile.type == ProjectileID.SporeGas || projectile.type == ProjectileID.SporeGas2 || projectile.type == ProjectileID.SporeGas3))
			{
				Main.instance.LoadProjectile(projectile.type);
				var asset = TextureAssets.Projectile[projectile.type];
				Main.EntitySpriteDraw(asset.Value, projectile.Center - Main.screenPosition, null, Color.DarkViolet, projectile.rotation, new Vector2(projectile.height / 2, projectile.height / 2), projectile.scale * 1.12f, SpriteEffects.None);
				returntype = false;
			}

			else if (Main.player[projectile.owner].ArcaneOdyssey().imbue is AshMagic && projectile.type == ProjectileID.SporeCloud)
			{
				Main.instance.LoadProjectile(projectile.type);
				var asset = TextureAssets.Projectile[projectile.type];
				Main.EntitySpriteDraw(asset.Value, projectile.Center - Main.screenPosition, new(0, 30 * projectile.frame, 28, 30), Color.DarkRed, projectile.rotation, new Vector2(projectile.height / 2, projectile.height / 2), projectile.scale, SpriteEffects.None);
				returntype = false;
			}

			return returntype; 
		}
	}
	public class AOProjectile : GlobalProjectile
	{
		public override bool InstancePerEntity => true;
		public float? BaseScale = null;
		public Vector2? OriginalDimensions = null;
		public int FramesAlive = 0;
		public Imbuable imbue;

		public override void OnSpawn(Projectile projectile, IEntitySource source)
		{
			OriginalDimensions ??= projectile.Size;
			BaseScale ??= projectile.scale;

			if (source is EntitySource_Parent { Entity: Projectile proj })
			{
				imbue ??= proj.ArcaneOdyssey().imbue;
			}
			else if (source is EntitySource_ItemUse source1)
			{
				imbue ??= source1.Item.ArcaneOdyssey().imbue;
			}
			else if (source is EntitySource_Parent { Entity: Player player })
			{
				imbue ??= player.HeldItem.ArcaneOdyssey().imbue;
			}

			if ((ImbueClassCheck(projectile) || projectile.ModProjectile is MagicCircle1 or MagicCircle2 or ExplosionTracker) && source is not EntitySource_Parent { Entity: NPC })
			{
				imbue ??= Main.player[projectile.owner].ArcaneOdyssey().imbue;
			}

			if ((projectile.ModProjectile is AOPlayerProjectile weapon && imbue is not null) && (weapon.Cold.HasValue && imbue.Cold.HasValue) && (weapon.Cold.Value != imbue.Cold.Value))
			{
				var imbueitem = new Item(ModContent.ItemType<SteamImbue>());
				((SteamImbue)imbueitem.ModItem).originalImbue = imbue;
				imbue = (SteamImbue)imbueitem.ModItem;
			}
		}

		public override void PostAI(Projectile projectile)
		{
			FramesAlive++;
		}

		public override bool PreAI(Projectile projectile)
		{
			if (FramesAlive < 1 && Main.netMode == NetmodeID.MultiplayerClient)
			{
				OriginalDimensions ??= projectile.Size;
				BaseScale ??= projectile.scale;
				if (ImbueClassCheck(projectile) || projectile.ModProjectile is MagicCircle1 or MagicCircle2 or ExplosionTracker)
					imbue ??= Main.player[projectile.owner].ArcaneOdyssey().imbue;
			}
			return true;
		}

		public override void SetDefaults(Projectile entity)
		{
			FramesAlive = 0;
		}
	}
}
