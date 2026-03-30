using ArcaneOdyssey.AOPlayers;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.VFX.Dusts;
using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Imbues.Relics
{
	public class SpiritEnergy : Imbuable
	{
		public override ImbuableTiers ImbuableTier
		{
			get
			{
				if (Soul != GodSoulID.None)
				{
					if (Stability.HasValue)
					{
						if (Stability.Value)
						{
							return ImbuableTiers.Ancient; // deific
						}
						else
						{
							return ImbuableTiers.Developer; // unstable
						}
					}
					else
					{
						return ImbuableTiers.Lost; // inhabited
					}
				}
				else
				{
					return ImbuableTiers.Normal; // normal
				}
			}
		}

		public GodSoulID Soul = GodSoulID.None;
		private int soulindex = 0;

		public virtual GodSoulID[] SoulSynergies => [];
		public virtual GodSoulID[] UnstableSouls => [];

		public bool? Stability
		{
			get
			{
				if (SoulSynergies.Contains(Soul))
					return true;
				if (UnstableSouls.Contains(Soul))
					return false;
				return null;
			}
		}

		public override void Update(ref float gravity, ref float maxFallSpeed)
		{
			base.Update(ref gravity, ref maxFallSpeed);
			Soul = GodSoulID.None;
		}

		public override void UpdateInventory(Player player)
		{
			base.UpdateInventory(player);
			if (Main.myPlayer == player.whoAmI && player.PlayerItem() == Item)
			{
				if (AOKeybinds.CycleGodSoul.JustPressed)
				{
					var souls = player.ArcaneOdyssey().Souls;
					if (souls.Count > 1)
					{
						if (++soulindex >= souls.Count)
						{
							soulindex = 0;
						}
						Soul = souls[soulindex];
						Color colour = Color.White;
						if (Stability.HasValue)
						{
							if (Stability.Value)
								colour = Color.Green;
							else
								colour = Color.Red;
						}
						Main.NewText(Mod.CustomLocalization("GodSouls.Changed", DisplayName.Value, Mod.CustomLocalization($"GodSouls.Soul{(int)Soul}").Value), colour);
					}
				}
			}
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			if (Type == ModContent.ItemType<SpiritEnergy>())
				ItemID.Sets.ItemNoGravity[Type] = true;
		}

		public override Color ImbueColour => SpiritColor;

		public Color SpiritColor
		{
			get
			{
				if (!Main.dedServ)
				{
					if (Main.LocalPlayer?.ArcaneOdyssey()?.evil == true)
					{
						if (Imbue is MagicType)
						{
							return Color.Red;
						}
						return Color.Purple;
					}
					else
					{
						if (Imbue is MagicType)
						{
							return Color.Gold;
						}
					}
				}
				return new(0, 183, 255);
			}
		}

		public override string ImbueUISprite
		{
			get
			{
				if (Type == ModContent.ItemType<SpiritEnergy>())
				{
					if (!Main.dedServ && Main.LocalPlayer.active)
					{
						if (Main.LocalPlayer?.ArcaneOdyssey()?.evil == true)
						{
							if (Imbue is MagicType)
							{
								return Texture + "_Evil_Magic";
							}
							return Texture + "_Evil_Normal";
						}
						else
						{
							if (Imbue is MagicType)
							{
								return Texture + "_Good_Magic";
							}
							return Texture + "_Good_Normal";
						}
					}
				}
				return base.ImbueUISprite;
			}
		}

		public static SpiritEnergy Instance => ModContent.GetInstance<SpiritEnergy>();

		public override SoundStyle? ImbueSound => SoundID.NPCDeath6;

		public override float ImbueSpeed => 1f;
		public override float ImbueDamage => 1f;
		public override float ImbueSize => 1f;

		public override float ScrollSpeed
		{
			get
			{
				if (Stability.HasValue)
				{
					if (Stability.Value)
					{
						return SynergySpeed;
					}
					else
					{
						return UnstableSpeed;
					}
				}
				return ImbueSpeed;
			}
		}

		public override float ScrollDamage
		{
			get
			{
				if (Stability.HasValue)
				{
					if (Stability.Value)
					{
						return SynergyDamage;
					}
					else
					{
						return UnstableDamage;
					}
				}
				return ImbueDamage;
			}
		}

		public override float ScrollSize
		{
			get
			{
				if (Stability.HasValue)
				{
					if (Stability.Value)
					{
						return SynergySize;
					}
					else
					{
						return UnstableSize;
					}
				}
				return ImbueSize;
			}
		}

		public override int Drawback
		{
			get
			{
				if (Stability.HasValue)
				{
					if (Stability.Value)
					{
						return SynergyDrawback;
					}
					else
					{
						return UnstableDrawback;
					}
				}
				return RelicDrawback;
			}
		}
		
		public virtual int RelicDrawback => 0;

		public virtual float SynergySpeed => 1f;
		public virtual float SynergyDamage => 1f;
		public virtual float SynergySize => 1f;
		public virtual int SynergyDrawback => 0;

		public virtual float UnstableSpeed => 1f;
		public virtual float UnstableDamage => 1f;
		public virtual float UnstableSize => 1f;
		public virtual int UnstableDrawback => 0;

		public override float? DashResist => 1.2f;

		public override string AttackPrefix => "Spirit";

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.DamageType = DamageClass.Summon;
		}

		public virtual int DustType => ModContent.DustType<SpiritDust>();

		public override bool CanShoot(Player player) => player.ownedProjectileCounts[Item.shoot] < 1 && !player.AltUse();

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			for (float i = 0; i < 5; i++)
			{
				Dust.NewDustDirect(area.TopLeft(), area.Width, area.Height, ModContent.DustType<SpiritDust>(), direction.GetValueOrDefault().X / 2, direction.GetValueOrDefault().Y / 2, Scale: area.RelativeScale(), Alpha: 255 / 4, newColor: SpiritColor).noGravity = true;
			}
			Dust.NewDustDirect(area.TopLeft(), area.Width, area.Height, DustType, direction.GetValueOrDefault().X / 2, direction.GetValueOrDefault().Y / 2, Alpha: 255 / 4, newColor: ImbueColour, Scale: area.RelativeScale()).noGravity = true;
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			int amount = 25 * 3;
			for (float i = 0; i < amount; i++)
			{
				var centre = (MathHelper.TwoPi / amount * i).ToRotationVector2() * 20 * area.RelativeScale();
				AOUtils.NewDustImperfect(area.Center(), ModContent.DustType<SpiritDust>(), centre * area.RelativeScale() / (13 + (Main.rand.NextFloat() * 2)), Scale: area.RelativeScale(), Alpha: 255 / 4, newColor: SpiritColor).noGravity = true;
			}
			amount = 12 * 2;
			for (float i = 0; i < amount; i++)
			{
				var centre = (MathHelper.TwoPi / amount * i).ToRotationVector2() * 20 * area.RelativeScale();
				AOUtils.NewDustImperfect(area.Center(), DustType, centre * area.RelativeScale() / (13 + (Main.rand.NextFloat() * 2)), newColor: ImbueColour, Alpha: 255 / 4, Scale: area.RelativeScale()).noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, ModContent.DustType<SpiritDust>(), direction.X * 0.5f, direction.Y * 0.5f, Scale: area.RelativeScale(), Alpha: 255 / 4, newColor: SpiritColor)];
				spawnedDust.noGravity = true;
			}
			for (int n = 0; n < 2; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustType, direction.X * 0.5f, direction.Y * 0.5f, Alpha: 255 / 4, newColor: ImbueColour, Scale: area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, ModContent.DustType<SpiritDust>(), (Main.rand.NextFloat() - 0.5f) * (15f * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * intensity), Scale: intensity, Alpha: 255 / 4, newColor: SpiritColor)];
				spawnedDust.noGravity = true;
			}
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustType, (Main.rand.NextFloat() - 0.5f) * (15f * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * intensity), Alpha: 255 / 4, newColor: ImbueColour, Scale: intensity)];
				spawnedDust.noGravity = true;
			}
		}
	}
}
