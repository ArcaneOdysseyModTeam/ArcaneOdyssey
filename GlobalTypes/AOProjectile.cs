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
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


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
				if (thisProjectile is not null)
				{
					if (OriginWeaponType == WeaponType.Artisinal)
						return null;
					if (thisProjectile.ModProjectile is null or BaseProjectile || ArcaneOdysseyConfig.Instance.AffectsOtherMods)
						return thisProjectile.ModProjectile is StrengthTechnique or MagicSpell or SpiritProjectile or Circle || OriginWeaponType != WeaponType.Normal;
				}
				return null;
			}
		}

		public override void SetDefaults(Projectile projectile)
		{
			thisProjectile = projectile;
			if (projectile.aiStyle == ProjAIStyleID.GraveMarker)
			{
				ArcaneOdysseyMod.Sets.tombstone[projectile.type] = true;
			}
			if (ArcaneOdysseyMod.Sets.excludedProjectile[projectile.type])
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

		public override void SendExtraAI(Projectile projectile, BitWriter bitWriter, BinaryWriter binaryWriter)
		{
			binaryWriter.Write(Imbue?.Type ?? ItemID.None);
			binaryWriter.Write(SecondImbue?.Type ?? ItemID.None);
			if (ArcaneOdysseyConfig.Instance.SyncProjectileSizes)
			{
				binaryWriter.Write(projectile.scale);
			}
		}

		public override void ReceiveExtraAI(Projectile projectile, BitReader bitReader, BinaryReader binaryReader)
		{
			Imbue = AOUtils.SafeImbuable(ModContent.GetModItem(binaryReader.ReadInt32()));
			SecondImbue = AOUtils.SafeImbuable(ModContent.GetModItem(binaryReader.ReadInt32()));
			if (ArcaneOdysseyConfig.Instance.SyncProjectileSizes)
			{
				projectile.scale = binaryReader.ReadSingle();
			}
		}

		public bool? Cold;

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
					if (proj.ArcaneOdyssey() is not null)
					{
						Imbue ??= proj.ArcaneOdyssey().Imbue;
						SecondImbue ??= proj.ArcaneOdyssey().SecondImbue;
						Cold ??= proj.ArcaneOdyssey().Cold;
						OriginWeaponType = proj.ArcaneOdyssey().OriginWeaponType;
					}
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
						Imbue ??= aOItem.Imbue;
						SecondImbue ??= aOItem.SecondImbue;
					}
					OriginWeaponType = ArcaneOdysseyMod.Sets.weaponType[item.type];
					Cold ??= ArcaneOdysseyMod.Sets.cold[item.type];
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
	}

	public class AntiArenaCheese : GlobalProjectile
	{
		public override void Load()
		{
			On_Projectile.CanExplodeTile += EliusTileCheck;
			On_Projectile.ShouldWallExplode += EliusWallCheck;
			On_Player.DropTombstone += EliusArenaNoTombstones;
		}

		private void EliusArenaNoTombstones(On_Player.orig_DropTombstone orig, Player self, long coinsOwned, Terraria.Localization.NetworkText deathText, int hitDirection)
		{
			if (self.Hitbox.Intersects(EliusArenaLoader.eliusArena.ToWorldRect()))
			{
				return;
			}

			orig(self, coinsOwned, deathText, hitDirection);
		}

		private bool EliusWallCheck(On_Projectile.orig_ShouldWallExplode orig, Projectile self, Microsoft.Xna.Framework.Vector2 compareSpot, int radius, int minI, int maxI, int minJ, int maxJ)
		{
			if (orig(self, compareSpot, radius, minI, maxI, minJ, maxJ) && !(EliusArenaLoader.eliusArena.Intersects(Utils.CenteredRectangle(compareSpot.ToTileCoordinates().ToVector2(), new(radius))) || ExternalModSupport.InAOSubworld))
			{
				return true;
			}
			return false;
		}

		private bool EliusTileCheck(On_Projectile.orig_CanExplodeTile orig, Projectile self, int x, int y)
		{
			if (orig(self, x, y) && !(EliusArenaLoader.eliusArena.Contains(x, y) || ExternalModSupport.InAOSubworld))
			{
				return true;
			}
			return false;
		}

		public override void Unload()
		{
			On_Projectile.CanExplodeTile -= EliusTileCheck;
			On_Projectile.ShouldWallExplode -= EliusWallCheck;
			On_Player.DropTombstone -= EliusArenaNoTombstones;
		}
	}
}
