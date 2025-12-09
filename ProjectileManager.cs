using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Imbues;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Lost;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Normal;
using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
		public override bool PreDraw(Projectile projectile, ref Color lightColor)
		{
			if ((projectile.GetOwner()?.ArcaneOdyssey()?.Imbue is PoisonMagic or PoisonLightningMagic || projectile.GetOwner()?.PlayerItem()?.Imbue() is PoisonMagic or PoisonLightningMagic || projectile.GetOwner()?.ArcaneOdyssey()?.Imbue?.SecondImbue is PoisonMagic or PoisonLightningMagic || projectile.GetOwner()?.PlayerItem()?.Imbue()?.SecondImbue is PoisonMagic or PoisonLightningMagic) && (projectile.type == ProjectileID.SporeGas || projectile.type == ProjectileID.SporeGas2 || projectile.type == ProjectileID.SporeGas3))
			{
				lightColor = projectile.GetAlpha(Color.DarkRed);
			}

			if ((projectile.GetOwner()?.ArcaneOdyssey()?.Imbue is AshMagic || projectile.GetOwner()?.PlayerItem()?.Imbue() is AshMagic || projectile.GetOwner()?.ArcaneOdyssey()?.Imbue?.SecondImbue is AshMagic || projectile.GetOwner()?.PlayerItem()?.Imbue()?.SecondImbue is AshMagic) && projectile.type == ProjectileID.SporeCloud)
			{
				lightColor = projectile.GetAlpha(Color.DarkRed);
			}
			return true; 
		}
	}

	public class AOProjectile : GlobalProjectile, IImbuable
	{
		public override void ModifyDamageHitbox(Projectile projectile, ref Rectangle hitbox)
		{
			thisProjectile = projectile;
			if (projectile.hostile || projectile.npcProj || projectile.owner == 255 || projectile.damage <= 0 || (!CanBeAffected) || (!ArcaneOdysseyConfig.Instance.ProjectileSizes))
				return;
			Player player = Main.player[projectile.owner];
			Vector2 dim = projectile.ArcaneOdyssey().OriginalDimensions.GetValueOrDefault(projectile.Size);
			float mult = projectile.ArcaneOdyssey().BaseScale.GetValueOrDefault(1f);
			if (Imbue is not null)
			{
				mult += (projectile.ModProjectile is MagicSpell or StrengthTechnique or SpiritProjectile ? Imbue.AOScrollSize : Imbue.AOImbueSize).MultiToPercent();
				if (projectile.CanHaveSecondImbue(Imbue, out var second))
				{
					mult += second.AOImbueSize;
				}
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

		public override void SetDefaults(Projectile projectile)
		{
			thisProjectile = projectile;
			if (ArcaneOdysseyMod.excludedProjectiles.Contains(projectile.type))
			{
				CanBeAffected = false;
			}
		}

		public override bool InstancePerEntity => true;

		private float? _basescale = null;
		public float? BaseScale
		{
			get
			{
				if (ArcaneOdysseyConfig.Instance.ProjectileSizes)
					return _basescale.GetValueOrDefault(1f);
				else
					return thisProjectile.scale;
			}
			set
			{
				if (ArcaneOdysseyConfig.Instance.ProjectileSizes)
					_basescale = value;
				else
					thisProjectile.scale = value.GetValueOrDefault(1f);
			}
		}
		public Vector2? OriginalDimensions = null;
		public Imbuable Imbue { get; set; }
		public Projectile thisProjectile = null;

		private bool _canImbue = true;
		public bool CanBeAffected { get
			{
				if (thisProjectile is not null && thisProjectile.ModProjectile is AOPlayerProjectile proj)
				{
					return proj.CanHaveImbue;
				}
				return _canImbue;
			} set => _canImbue = value;
		}


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

		public override bool PreKill(Projectile projectile, int timeLeft)
		{
			thisProjectile = projectile;
			if (CanBeAffected && !Main.dedServ)
			{
				if (Imbue is not null && Imbue.PreEffects(projectile))
				{
					if (projectile.ModProjectile is not ExplosionSpell && projectile.ModProjectile is not ExplosionTracker)
					{
						Imbue.KillEffects(projectile);
						if (projectile.CanHaveSecondImbue(Imbue, out var second))
							second.KillEffects(projectile);
					}
				}
			}
			return true;
		}

		public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
		{
			thisProjectile = projectile;
			if (!CanBeAffected)
				return;

			if (projectile.TryGetOwner(out var player))
			{
				if (player.meleeEnchant != 0 && (projectile.DamageType.CountsAsClass(DamageClass.Melee) || projectile.DamageType == DamageClass.SummonMeleeSpeed))
				{
					// apply early for synergies and stuff, no way to do it for modded imbues
					foreach (var buff in player.buffType)
					{
						if (Main.meleeBuff[buff])
						{
							switch (player.meleeEnchant)
							{
								case 1:
									target.AddBuff(BuffID.Venom, 60 * Main.rand.Next(5, 10));
									break;
								case 2:
									target.AddBuff(BuffID.CursedInferno, 60 * Main.rand.Next(3, 7));
									break;
								case 3:
									target.AddBuff(BuffID.OnFire, 60 * Main.rand.Next(3, 7));
									break;
								case 4:
									target.AddBuff(BuffID.Midas, 120);
									break;
								case 5:
									target.AddBuff(BuffID.Ichor, 60 * Main.rand.Next(10, 20));
									break;
								case 6:
									target.AddBuff(BuffID.Confused, 60 * Main.rand.Next(1, 4));
									break;
								case 8:
									target.AddBuff(BuffID.Poisoned, 60 * Main.rand.Next(5, 10));
									break;
								default:
									if (player.ArcaneOdyssey().gel.HasValue)
										target.AddBuff(player.ArcaneOdyssey().gel.Value, 60 * Main.rand.Next(5, 10));
									break;
							}
						}
					}
				}
			}
			CalculateImbueDamage(Imbue, target, ref modifiers);
			if (projectile.CanHaveSecondImbue(Imbue, out var second))
				CalculateImbueDamage(second, target, ref modifiers);

		}

		public override void OnSpawn(Projectile projectile, IEntitySource source)
		{
			thisProjectile = projectile;
			if (!CanBeAffected)
				return;
			OriginalDimensions ??= projectile.Size;
			BaseScale ??= projectile.scale;

			if (projectile.ModProjectile is AOPlayerProjectile proj1 && !projectile.DamageType.CountsAsClass<MeleeNoSpeedDamageClass>())
			{
				projectile.velocity *= proj1.AOSpeed;
			}

			if (ImbueClassCheck(projectile))
			{
				if (source is EntitySource_Parent { Entity: Projectile proj })
				{
					Imbue ??= proj.ArcaneOdyssey().Imbue;
					if (projectile.CanHaveSecondImbue(Imbue, out var second))
						Imbue.SecondImbue ??= second;
					Cold ??= proj.ArcaneOdyssey().Cold;
				}
				else if (source is EntitySource_ItemUse { Item: Item item })
				{
					if (item.ModItem is RelicWeapon relic)
					{
						Imbue ??= relic;
						Imbue.SecondImbue ??= relic.SecondImbue;
					}
					else if (item.TryGetGlobalItem<AOItem>(out var aOItem))
					{
						Imbue ??= aOItem.Imbue;
						if (projectile.CanHaveSecondImbue(Imbue, out var second))
							Imbue.SecondImbue ??= second;
						Cold ??= aOItem.Cold;
					}
				}
				else if (source is EntitySource_Parent { Entity: Player player })
				{
					Imbue ??= player.ArcaneOdyssey().Imbue;
					if (projectile.CanHaveSecondImbue(Imbue, out var second))
						Imbue.SecondImbue ??= second;
				}

				if (Imbue is not null && Cold.HasValue && Imbue.Cold.HasValue && (Cold.Value != Imbue.Cold.Value))
				{
					Imbue = SteamImbue.Create(Imbue);
				}

				if (Imbue is not null && Imbue.SecondImbue is not null && Imbue.Cold.HasValue && Imbue.SecondImbue.Cold.HasValue && (Imbue.Cold.Value != Imbue.SecondImbue.Cold.Value))
				{
					Imbue.SecondImbue = SteamImbue.Create(Imbue);
				}

				if (Imbue is not null && Imbue.PreEffects(projectile))
				{
					if (projectile.ModProjectile is not ExplosionSpell && projectile.ModProjectile is not ExplosionTracker)
					{
						Imbue.SpawningEffects(projectile);
						if (projectile.CanHaveSecondImbue(Imbue, out var second))
							second.SpawningEffects(projectile);
					}
				}
				projectile.DamageType = projectile.DamageType.Imbued(Imbue);
			}
		}

		public override bool PreAI(Projectile projectile)
		{
			thisProjectile = projectile;
			if (CanBeAffected)
			{
				OriginalDimensions ??= projectile.Size;
				BaseScale ??= projectile.scale;
			}
			return true;
		}

		public override void AI(Projectile projectile)
		{
			thisProjectile = projectile;
			if (!CanBeAffected)
				return;
			if (Imbue is not null && Imbue.PreEffects(projectile))
			{
				Imbue.LingeringEffects(projectile);
				if (projectile.CanHaveSecondImbue(Imbue, out var second))
					second.LingeringEffects(projectile);
			}
		}
	}
}
