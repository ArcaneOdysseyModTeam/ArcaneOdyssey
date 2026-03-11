using ArcaneOdyssey.Content.Buffs.Base;
using ArcaneOdyssey.Content.Imbues;
using ArcaneOdyssey.Content.Imbues.FightingStyles.Normal;
using ArcaneOdyssey.Content.Imbues.Magic.Ancient;
using ArcaneOdyssey.Content.Imbues.Relics;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;


namespace ArcaneOdyssey.GlobalTypes
{
	public class AOProjectile : GlobalProjectile, IImbuable
	{
		public float ApplySpeed(float value, bool flipfloat = false)
		{
			if (BenifitsFromScrollStats.HasValue)
			{
				if (BenifitsFromScrollStats.Value)
				{
					if (Imbue is not null)
					{
						if (!flipfloat)
						{
							value *= Imbue.AOScrollSpeed;
							if (SecondImbue is not null)
								value *= SecondImbue.AOImbueSpeed;
						}
						else
						{
							value *= Imbue.AOScrollSpeed.FlipFloat();
							if (SecondImbue is not null)
								value *= SecondImbue.AOImbueSpeed.FlipFloat();
						}
					}
				}
				else
				{
					if (Imbue is not null)
					{
						if (!flipfloat)
						{
							value *= Imbue.AOImbueSpeed;
							if (SecondImbue is not null)
								value *= SecondImbue.AOImbueSpeed;
						}
						else
						{
							value *= Imbue.AOImbueSpeed.FlipFloat();
							if (SecondImbue is not null)
								value *= SecondImbue.AOImbueSpeed.FlipFloat();
						}
					}
				}
			}
			return value;
		}

		public float ApplySize(float value, bool flipfloat = false)
		{
			if (BenifitsFromScrollStats.HasValue)
			{
				if (BenifitsFromScrollStats.Value)
				{
					if (Imbue is not null)
					{
						if (!flipfloat)
						{
							value *= Imbue.AOScrollSize;
							if (SecondImbue is not null)
								value *= SecondImbue.AOImbueSize;
						}
						else
						{
							value *= Imbue.AOScrollSize.FlipFloat();
							if (SecondImbue is not null)
								value *= SecondImbue.AOImbueSize.FlipFloat();
						}
					}
				}
				else
				{
					if (Imbue is not null)
					{
						if (!flipfloat)
						{
							value *= Imbue.AOImbueSize;
							if (SecondImbue is not null)
								value *= SecondImbue.AOImbueSize;
						}
						else
						{
							value *= Imbue.AOImbueSize.FlipFloat();
							if (SecondImbue is not null)
								value *= SecondImbue.AOImbueSize.FlipFloat();
						}
					}
				}
			}
			return value;
		}

		public bool? BenifitsFromScrollStats
		{
			get
			{
				if (OriginWeaponType == WeaponType.Artisinal)
					return null;
				if (thisProjectile is not null)
				{
					if (thisProjectile.ModProjectile is StrengthTechnique or MagicSpell or SpiritProjectile or BaseMagicCircle)
					{
						return true;
					}
					else if (thisProjectile.ModProjectile is null or AOBaseProjectile || ArcaneOdysseyConfig.Instance.AffectsOtherMods)
					{
						return false;
					}
				}
				if (OriginWeaponType != WeaponType.Normal)
					return true;
				else
					return false;
				return null;
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

		public Imbuable Imbue { get; set; }
		public Imbuable SecondImbue { get; set; }
		public Projectile thisProjectile = null;

		public WeaponType OriginWeaponType;

		private bool _canImbue = true;
		public bool CanBeAffected
		{
			get
			{
				if (thisProjectile is not null && thisProjectile.ModProjectile is AOPlayerProjectile proj)
				{
					return proj.CanHaveImbue;
				}
				return _canImbue;
			}
			set => _canImbue = value;
		}


		private bool? _cold = null;
		public bool? Cold
		{
			get
			{
				if (thisProjectile is not null && thisProjectile.ModProjectile is AOPlayerProjectile proj && proj.Cold.HasValue)
				{
					return proj.Cold.Value;
				}
				return _cold;
			}
			set => _cold = value;
		}

		public override bool PreKill(Projectile projectile, int timeLeft)
		{
			thisProjectile = projectile;
			if (CanBeAffected && !Main.dedServ)
			{
				if (projectile.ModProjectile is not ExplosionSpell)
				{
					if (Imbue is not null && Imbue.PreEffects(projectile))
					{
						Imbue.KillEffects(projectile.Hitbox, projectile);
					}
					if (SecondImbue is not null && SecondImbue.PreEffects(projectile))
						SecondImbue.KillEffects(projectile.Hitbox, projectile);
				}
			}
			return true;
		}

		public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
		{
			thisProjectile = projectile;
			if (!CanBeAffected)
				return;

			if (projectile.ModProjectile is AOPlayerProjectile proj)
			{
				if (proj.ProjectileDebuff.HasValue) // is done here instead of under AOPlayerProjectile to have damage calculation done in the correct order
				{
					target.AddBuff(proj.ProjectileDebuff.Value.debuffID, proj.ProjectileDebuff.Value.debuffDuration);
				}

				if (proj.HitSound.HasValue)
				{
					SoundEngine.PlaySound(proj.HitSound.Value, target.position);
				}
			}

			modifiers = AOUtils.CalculateImbueDamage(Imbue, target, modifiers);
			modifiers = AOUtils.CalculateImbueDamage(SecondImbue, target, modifiers);
		}

		public override void OnSpawn(Projectile projectile, IEntitySource source)
		{
			thisProjectile = projectile;
			if (!CanBeAffected || projectile.hostile || projectile.owner == 255 || !projectile.active || projectile.npcProj || projectile.trap)
				return;

			if (projectile.ModProjectile is AOPlayerProjectile proj1)
			{
				projectile.velocity *= proj1.AOSpeed;
			}

			if (AOUtils.ImbueClassCheck(projectile))
			{
				if (source is EntitySource_Parent { Entity: Projectile proj })
				{
					Imbue ??= proj.ArcaneOdyssey()?.Imbue;
					SecondImbue ??= proj.ArcaneOdyssey()?.SecondImbue;
					Cold ??= proj.ArcaneOdyssey()?.Cold;
				}
				else if (source is EntitySource_ItemUse { Item: Item item })
				{
					if (item.ModItem is Imbuable relic)
					{
						Imbue ??= relic;
						SecondImbue ??= relic.Imbue;
						Cold = relic.Cold;
					}
					else if (item.TryGetGlobalItem<AOItem>(out var aOItem))
					{
						OriginWeaponType = aOItem.WeaponsType;
						Imbue ??= aOItem.Imbue;
						SecondImbue ??= aOItem.SecondImbue;
						Cold ??= aOItem.Cold;
					}
				}
				else if (source is EntitySource_Parent { Entity: Player player })
				{
					Imbue ??= player.ArcaneOdyssey().Imbue;
					if (player.TryGetSecondImbue(Imbue, out var second))
						SecondImbue ??= second;
				}

				if (Imbue is not null && Cold.HasValue && Imbue.Cold.HasValue && (Cold.Value != Imbue.Cold.Value))
				{
					Imbue = SteamImbue.Create(Imbue);
				}

				if (Imbue is not null && Imbue.Imbue is not null && Imbue.Cold.HasValue && Imbue.Imbue.Cold.HasValue && (Imbue.Cold.Value != Imbue.Imbue.Cold.Value))
				{
					Imbue.Imbue = SteamImbue.Create(Imbue);
				}

				if (projectile.ModProjectile is not ExplosionSpell)
				{
					if (Imbue is not null && Imbue.PreEffects(projectile))
					{
						Imbue.SpawningEffects(projectile.Hitbox, projectile.velocity);
					}
					if (SecondImbue is not null && SecondImbue.PreEffects(projectile))
						SecondImbue.SpawningEffects(projectile.Hitbox, projectile.velocity);
				}
			}

			float mult = 1f;

			if (Imbue is not null)
			{
				if (BenifitsFromScrollStats.HasValue)
				{
					if (BenifitsFromScrollStats.Value)
					{
						mult *= Imbue.AOScrollSize;
						if (SecondImbue is not null)
						{
							mult *= SecondImbue.AOImbueSize;
						}
					}
					else
					{
						mult *= Imbue.AOImbueSize;
						if (SecondImbue is not null)
						{
							mult *= SecondImbue.AOImbueSize;
						}
					}
				}
			}
			mult *= Main.player[projectile.owner]?.ArcaneOdyssey()?.SizeMulti ?? 1f;
			if (projectile.ModProjectile is null or AOBaseProjectile || ArcaneOdysseyConfig.Instance.AffectsOtherMods)
			{
				projectile.Hitbox = AOUtils.ScaleRectangleNotRef(projectile.Hitbox, mult);
				projectile.scale *= mult;
			}
		}

		public override bool PreAI(Projectile projectile)
		{
			thisProjectile = projectile;
			return true;
		}

		public override void AI(Projectile projectile)
		{
			thisProjectile = projectile;
			if (!Main.dedServ && projectile.TryGetOwner(out var player) && player.meleeEnchant == GelBuff.meleeEnchantID && (projectile.DamageType.CountsAsClass(DamageClass.Melee) || projectile.DamageType == DamageClass.SummonMeleeSpeed))
			{
				player.ArcaneOdyssey()?.Gel?.Effects(projectile.Hitbox);
			}
			if (!CanBeAffected)
				return;
			if (Imbue is not null && Imbue.PreEffects(projectile))
			{
				Imbue.LingeringEffects(projectile.Hitbox, projectile.velocity, projectile);
			}
			if (SecondImbue is not null && SecondImbue.PreEffects(projectile))
				SecondImbue.LingeringEffects(projectile.Hitbox, projectile.velocity, projectile);
		}

		public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
		{
			thisProjectile = projectile;

			if (projectile.TryGetOwner(out var player))
			{
				if (player.meleeEnchant == GelBuff.meleeEnchantID && (projectile.DamageType.CountsAsClass(DamageClass.Melee) || projectile.DamageType == DamageClass.SummonMeleeSpeed))
				{
					if (player.ArcaneOdyssey().GelDebuff != 0)
						target.AddBuff(player.ArcaneOdyssey().GelDebuff, 60 * Main.rand.Next(5, 10));
				}

				if (player.ArcaneOdyssey().BloodDisease != 0)
				{
					target.AddBuff(player.ArcaneOdyssey().BloodDisease, 60 * Main.rand.Next(4, 10));
				}
			}

			if (!CanBeAffected)
				return;

			if (Imbue is VanishingStyle && hit.Crit)
				projectile.CritChance = projectile.OriginalCritChance;

			if (projectile.TryGetOwner(out var owner))
			{
				if (Imbue is SpiritEnergy)
				{
					if (!target.immortal)
						owner.ArcaneOdyssey()?.TrySpiritLifesteal(Math.Min(projectile.originalDamage, projectile.damage), projectile.ModProjectile is not SpiritProjectile);
				}
			}

			if (Main.netMode == NetmodeID.SinglePlayer && Imbue is DeathMagic && (target.lifeMax < (Main.player[projectile.owner].statLifeMax2 * 2)))
			{
				target.StrikeInstantKill();
			}
		}
	}
}
