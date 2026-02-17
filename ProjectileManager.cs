using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Imbues;
using ArcaneOdyssey.Content.Items.Imbues.FightingStyles.Normal;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Ancient;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Lost;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Normal;
using ArcaneOdyssey.Content.Items.Imbues.Relics;
using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey
{
	public class ProjectileManager : GlobalProjectile
	{
		public override bool PreDraw(Projectile projectile, ref Color lightColor)
		{
			if ((projectile.GetOwner()?.ArcaneOdyssey()?.Imbue is PoisonMagic or PoisonLightningMagic || projectile.GetOwner()?.PlayerItem()?.Imbue() is PoisonMagic or PoisonLightningMagic || projectile.GetOwner()?.ArcaneOdyssey()?.Imbue?.Imbue is PoisonMagic or PoisonLightningMagic || projectile.GetOwner()?.PlayerItem()?.Imbue()?.Imbue is PoisonMagic or PoisonLightningMagic) && (projectile.type == ProjectileID.SporeGas || projectile.type == ProjectileID.SporeGas2 || projectile.type == ProjectileID.SporeGas3))
			{
				lightColor = projectile.GetAlpha(Color.Purple);
			}

			if ((projectile.GetOwner()?.ArcaneOdyssey()?.Imbue is AshMagic || projectile.GetOwner()?.PlayerItem()?.Imbue() is AshMagic || projectile.GetOwner()?.ArcaneOdyssey()?.Imbue?.Imbue is AshMagic || projectile.GetOwner()?.PlayerItem()?.Imbue()?.Imbue is AshMagic) && projectile.type == ProjectileID.SporeCloud)
			{
				lightColor = projectile.GetAlpha(Color.DarkRed);
			}
			return true;
		}
	}

	public class AOProjectile : GlobalProjectile, IImbuable
	{
		public override void SendExtraAI(Projectile projectile, BitWriter bitWriter, BinaryWriter binaryWriter)
		{
			binaryWriter.Write(Imbue?.Type ?? 0);
			binaryWriter.Write(SecondImbue?.Type ?? 0);
		}

		public override void ReceiveExtraAI(Projectile projectile, BitReader bitReader, BinaryReader binaryReader)
		{
			int read = binaryReader.Read();
			if (read != 0)
			{
				Imbue = (Imbuable)ModContent.GetModItem(read);
			}
			else
			{
				Imbue = null;
			}	

			int read2 = binaryReader.Read();
			if (read2 != 0)
			{
				SecondImbue = (Imbuable)ModContent.GetModItem(read2);
			}
			else
			{
				SecondImbue = null;
			}
		}

		public float ApplyScrollSpeed(float value, bool flipfloat = false)
		{
			if (Imbue is not null)
			{
				if (!flipfloat)
				{
					value *= Imbue.AOScrollSpeed;
					if (SecondImbue is not null)
						value *= SecondImbue.AOScrollSpeed;
				}
				else
				{
					value *= Imbue.AOScrollSpeed.FlipFloat();
					if (SecondImbue is not null)
						value *= SecondImbue.AOScrollSpeed.FlipFloat();
				}
			}
			return value;
		}

		public float ApplyImbueSpeed(float value, bool flipfloat = false)
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
			return value;
		}

		public bool? BenifitsFromScrollStats
		{
			get
			{
				if (OriginWeaponType == WeaponType.Artisinal)
					return null;
				if (OriginWeaponType != WeaponType.Normal)
					return true;
				else
					return false;
				if (thisProjectile is not null)
				{
					return thisProjectile.ModProjectile is StrengthTechnique or MagicSpell or SpiritProjectile;
				}
				else
				{
					if (thisProjectile.ModProjectile is null or AOPlayerProjectile || ArcaneOdysseyConfig.Instance.AffectsOtherMods)
					{
						return false;
					}
				}
				return null;
			}
		}

		public override void ModifyDamageHitbox(Projectile projectile, ref Rectangle hitbox)
		{
			thisProjectile = projectile;
			if (projectile.hostile || projectile.npcProj || projectile.owner == 255 || (projectile.damage <= 0 && projectile.ModProjectile is not AOPlayerProjectile) || (!CanBeAffected) || (!ArcaneOdysseyConfig.Instance.ProjectileSizes))
				return;
			Player player = Main.player[projectile.owner];
			float mult = BaseScale.GetValueOrDefault(projectile.scale);
			if (Imbue is not null)
			{
				if (BenifitsFromScrollStats.HasValue)
				{
					if (BenifitsFromScrollStats.Value)
					{
						mult *= Imbue.AOScrollSize;
						if (SecondImbue is not null)
						{
							mult *= SecondImbue.AOScrollSize;
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
			mult *= player.ArcaneOdyssey().SizeMulti;
			if (projectile.ModProjectile is null or AOPlayerProjectile || ArcaneOdysseyConfig.Instance.AffectsOtherMods)
			{
				ScaleRectangle(ref hitbox, mult);
				projectile.scale = mult;
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

			if (projectile.TryGetOwner(out Player player))
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
									if (player.ArcaneOdyssey().gel != 0)
										target.AddBuff(player.ArcaneOdyssey().gel, 60 * Main.rand.Next(5, 10));
									break;
							}
						}
					}
				}

				if (player.ArcaneOdyssey().BloodDisease != 0)
				{
					target.AddBuff(player.ArcaneOdyssey().BloodDisease, 60 * Main.rand.Next(5, 10));
				}
			}
			modifiers = CalculateImbueDamage(Imbue, target, modifiers);
			modifiers = CalculateImbueDamage(SecondImbue, target, modifiers);
		}

		public override void OnSpawn(Projectile projectile, IEntitySource source)
		{
			thisProjectile = projectile;
			if (!CanBeAffected)
				return;

			if (projectile.owner == Main.myPlayer)
			{
				projectile.netUpdate = true;
				projectile.netSpam = 0;
			}

			BaseScale ??= projectile.scale;

			if (projectile.ModProjectile is AOPlayerProjectile proj1 && !projectile.DamageType.CountsAsClass<MeleeNoSpeedDamageClass>())
			{
				projectile.velocity *= proj1.AOSpeed;
			}

			if (ImbueClassCheck(projectile))
			{
				if (source is EntitySource_Parent { Entity: Projectile proj })
				{
					Imbue ??= proj.ArcaneOdyssey()?.Imbue;
					SecondImbue ??= proj.ArcaneOdyssey()?.SecondImbue;
					Cold ??= proj.ArcaneOdyssey().Cold;
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
				projectile.DamageType = projectile.DamageType.Imbued(Imbue);
			}
		}

		public override bool PreAI(Projectile projectile)
		{
			thisProjectile = projectile;
			if (CanBeAffected)
			{
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
				Imbue.LingeringEffects(projectile.Hitbox, projectile.velocity, projectile);
			}
			if (SecondImbue is not null && SecondImbue.PreEffects(projectile))
				SecondImbue.LingeringEffects(projectile.Hitbox, projectile.velocity, projectile);
		}

		public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
		{
			thisProjectile = projectile;
			if (!CanBeAffected)
				return;
			if (Imbue is VanishingStyle && hit.Crit)
				projectile.CritChance = projectile.OriginalCritChance;
			if (Imbue is SpiritEnergy && projectile.TryGetOwner(out var owner))
			{
				owner.ArcaneOdyssey()?.TrySpiritLifesteal(Math.Min(projectile.originalDamage, projectile.damage), projectile.ModProjectile is not SpiritProjectile);
			}
			if (Main.netMode == NetmodeID.SinglePlayer && Imbue is DeathMagic && (target.lifeMax < Main.player[projectile.owner].statLifeMax2))
			{
				target.StrikeInstantKill();
			}
		}
	}
}
