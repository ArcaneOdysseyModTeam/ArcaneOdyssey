using ArcaneOdyssey.Biomes;
using ArcaneOdyssey.Buffs.Base;
using ArcaneOdyssey.Imbues;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.FightingStyles.Normal;
using ArcaneOdyssey.Imbues.Magic.Ancient;
using ArcaneOdyssey.Imbues.Relics;
using ArcaneOdyssey.Projectiles;
using ArcaneOdyssey.Projectiles.Base;
using ArcaneOdyssey.Projectiles.Magic;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;


namespace ArcaneOdyssey.GlobalTypes
{
	public partial class AOProjectile : GlobalProjectile, IImbuable
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
							value *= Imbue.ScrollSpeed;
							if (SecondImbue is not null)
								value *= SecondImbue.ImbueSpeed;
						}
						else
						{
							value *= Imbue.ScrollSpeed.FlipFloat();
							if (SecondImbue is not null)
								value *= SecondImbue.ImbueSpeed.FlipFloat();
						}
					}
				}
				else
				{
					if (Imbue is not null)
					{
						if (!flipfloat)
						{
							value *= Imbue.ImbueSpeed;
							if (SecondImbue is not null)
								value *= SecondImbue.ImbueSpeed;
						}
						else
						{
							value *= Imbue.ImbueSpeed.FlipFloat();
							if (SecondImbue is not null)
								value *= SecondImbue.ImbueSpeed.FlipFloat();
						}
					}
				}
			}
			return value;
		}

		public float ApplySize(float value, bool flipfloat = false)
		{
			value *= Main.player[thisProjectile?.owner ?? 255]?.ArcaneOdyssey()?.SizeMulti ?? 1f;
			if (BenifitsFromScrollStats.HasValue)
			{
				if (BenifitsFromScrollStats.Value)
				{
					if (Imbue is not null)
					{
						if (!flipfloat)
						{
							value *= Imbue.ScrollSize;
							if (SecondImbue is not null)
								value *= SecondImbue.ImbueSize;
						}
						else
						{
							value *= Imbue.ScrollSize.FlipFloat();
							if (SecondImbue is not null)
								value *= SecondImbue.ImbueSize.FlipFloat();
						}
					}
				}
				else
				{
					if (Imbue is not null)
					{
						if (!flipfloat)
						{
							value *= Imbue.ImbueSize;
							if (SecondImbue is not null)
								value *= SecondImbue.ImbueSize;
						}
						else
						{
							value *= Imbue.ImbueSize.FlipFloat();
							if (SecondImbue is not null)
								value *= SecondImbue.ImbueSize.FlipFloat();
						}
					}
				}
			}
			return value;
		}

		public float ApplyKnockback(float value, bool flipfloat = false)
		{
			if (BenifitsFromScrollStats.HasValue)
			{
				if (!flipfloat)
				{
					if (Imbue is not null)
					{
						value *= Imbue.KBMulti;
						if (SecondImbue is not null)
							value *= SecondImbue.KBMulti;
					}
				}
				else
				{
					if (Imbue is not null)
					{
						value *= 1f / Imbue.KBMulti;
						if (SecondImbue is not null)
							value *= 1f / SecondImbue.KBMulti;
					}
				}
				if (BenifitsFromScrollStats.Value)
				{
					if (Imbue is not null)
					{
						if (!flipfloat)
						{
							value *= Imbue.ScrollSize * Imbue.ScrollSize;
							if (SecondImbue is not null)
								value *= SecondImbue.ImbueSize * SecondImbue.ImbueSize;
						}
						else
						{
							value *= Imbue.ScrollSize.FlipFloat() * Imbue.ScrollSize.FlipFloat();
							if (SecondImbue is not null)
								value *= SecondImbue.ImbueSize.FlipFloat() * SecondImbue.ImbueSize.FlipFloat();
						}
					}
				}
				else
				{
					if (Imbue is not null)
					{
						if (!flipfloat)
						{
							value *= Imbue.ImbueSize * Imbue.ImbueSize;
							if (SecondImbue is not null)
								value *= SecondImbue.ImbueSize * SecondImbue.ImbueSize;
						}
						else
						{
							value *= Imbue.ImbueSize.FlipFloat();
							if (SecondImbue is not null)
								value *= SecondImbue.ImbueSize.FlipFloat() * SecondImbue.ImbueSize.FlipFloat();
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
					if (thisProjectile.ModProjectile is StrengthTechnique or MagicSpell or SpiritProjectile or Circle)
					{
						return true;
					}
					else if (thisProjectile.ModProjectile is null or BaseProjectile || ArcaneOdysseyConfig.Instance.AffectsOtherMods)
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
				if (thisProjectile is not null && thisProjectile.ModProjectile is PlayerProjectile proj)
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
				if (thisProjectile is not null && thisProjectile.ModProjectile is PlayerProjectile proj && proj.Cold.HasValue)
				{
					return proj.Cold.Value;
				}
				return _cold;
			}
			set => _cold = value;
		}

		public override void OnKill(Projectile projectile, int timeLeft)
		{
			thisProjectile = projectile;
			Death(projectile, timeLeft);
			if (CanBeAffected && !Main.dedServ)
			{
				if (Imbue is not null && Imbue.PreEffects(projectile))
				{
					Imbue.KillEffects(projectile.Hitbox, projectile);
				}
				if (SecondImbue is not null && SecondImbue.PreEffects(projectile))
				{
					SecondImbue.KillEffects(projectile.Hitbox, projectile);
				}
			}
		}

		public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
		{
			thisProjectile = projectile;
			if (!CanBeAffected)
				return;

			if (projectile.ModProjectile is PlayerProjectile proj)
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
			Spawn(projectile, source);
			if (!CanBeAffected || projectile.hostile || projectile.owner == 255 || !projectile.active || projectile.npcProj || projectile.trap)
				return;

			if (projectile.ModProjectile is PlayerProjectile proj1)
			{
				projectile.velocity *= proj1.Speed;
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

			var mult = ApplySize(1f);
			if (projectile.ModProjectile is null or BaseProjectile || ArcaneOdysseyConfig.Instance.AffectsOtherMods)
			{
				projectile.Hitbox = projectile.Hitbox.Scaled(mult);
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
			Update(projectile);
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
			{
				SecondImbue.LingeringEffects(projectile.Hitbox, projectile.velocity, projectile);
			}
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

			if (Main.netMode == NetmodeID.SinglePlayer && (Imbue is DeathMagic || SecondImbue is DeathMagic) && (target.lifeMax < (Main.player[projectile.owner].statLifeMax2 * 2)))
			{
				target.StrikeInstantKill();
			}
		}
		public override void PrepareBombToBlow(Projectile projectile)
		{
			if (AOUtils.TryGetOwner(projectile, out Player owner))
			{
				if (owner.InModBiome<EliusArena>()) // add subworlds later
				{
					projectile.Kill();
				}
			}
			if (projectile.Hitbox.Intersects(EliusArenaLoader.eliusArena.ToWorldRect()))
			{
				projectile.Kill();
			}
		}
	}
}
