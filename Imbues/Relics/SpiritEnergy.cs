using ArcaneOdyssey.Dusts;
using ArcaneOdyssey.GodSouls;
using ArcaneOdyssey.Imbues.Base;
using System.IO;
using System.Linq;
using Terraria.Audio;
using Terraria.ModLoader.IO;

namespace ArcaneOdyssey.Imbues.Relics
{
	public class SpiritEnergy : Imbuable
	{
		public override void Load()
		{
			base.Load();
			ModTypeLookup<SpiritEnergy>.Register(this);
		}

		public sealed override ImbuableTiers ImbuableTier
		{
			get
			{
				if (Soul.Type > 0)
				{
					if (Stability.HasValue)
					{
						if (Stability.Value)
						{
							return ImbuableTiers.Ancient; // deific
						}
						else
						{
							return ImbuableTiers.Mythical; // unstable
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

		private GodSoul soul = null;
		public GodSoul Soul { get => soul ?? GodSoul.None; set => soul = value; }

		private int soulindex = 0;

		/// <summary>
		/// The <seealso cref="AOUtils.GodSoulType{T}"/> of each
		/// </summary>
		public virtual byte[] SoulSynergies => [];

		/// <inheritdoc cref="SoulSynergies"/>
		public virtual byte[] UnstableSouls => [];

		/// <summary>
		/// true is stable, false is unstable
		/// </summary>
		public bool? Stability
		{
			get
			{
				if (SoulSynergies.Contains(Soul.Type))
					return true;
				if (UnstableSouls.Contains(Soul.Type))
					return false;
				return null;
			}
		}

		public override void NetSend(BinaryWriter writer)
		{
			base.NetSend(writer);
			writer.Write(Soul.Type);
		}

		public override void NetReceive(BinaryReader reader)
		{
			base.NetReceive(reader);
			Soul = GodSoul.GetSoul(reader.ReadByte());
		}

		public override void Update(ref float gravity, ref float maxFallSpeed)
		{
			base.Update(ref gravity, ref maxFallSpeed);
			Soul = GodSoul.None;
			if (Type == ModContent.ItemType<SpiritEnergy>())
			{
				Item.color = SpiritColor;
			}
		}

		public static Asset<Texture2D> synergyAsset;
		public static Asset<Texture2D> unstableAsset;

		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			if (Stability.HasValue)
			{
				Asset<Texture2D> image;
				float indscale = 1f;
				if (Stability.Value)
				{
					image = AOUtils.Request(ArcaneOdysseyMod.InternalName + "/Assets/GodSoulSynergy", ref synergyAsset, AssetRequestMode.ImmediateLoad);
					indscale = 1.1f;
				}
				else
				{
					image = AOUtils.Request(ArcaneOdysseyMod.InternalName + "/Assets/GodSoulUnstable", ref unstableAsset, AssetRequestMode.ImmediateLoad);
				}

				spriteBatch.Draw(image.Value, position, null, Item.GetAlpha(Color.White * Main.inventoryScale), 0f, image.Size() / 2f, Main.inventoryScale * indscale, SpriteEffects.None, 1f);
			}

			if (Type == ModContent.ItemType<SpiritEnergy>())
			{
				if (itemColor == Color.Transparent)
				{
					spriteBatch.Draw(Sprite, position, frame, Item.GetAlpha(SpiritColor), 0f, origin, scale, SpriteEffects.None, 1f);
					return false;
				}
			}

			return true;
		}

		public override void UpdateInventory(Player player)
		{
			base.UpdateInventory(player);

			if (Type == ModContent.ItemType<SpiritEnergy>())
			{
				Item.color = SpiritColor;
			}

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
						Main.NewText(ArcaneOdysseyMod.Instance.CustomLocalization("ImbueStuff.SpecificImbue", DisplayName.Value, Soul.DisplayName.Value, colour));
					}
				}
			}
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			if (Type == ModContent.ItemType<SpiritEnergy>())
			{
				ItemID.Sets.ItemNoGravity[Type] = true;

				ItemID.Sets.ItemIconPulse[Type] = ArcaneOdysseyClientConfig.Instance.PulsingImbueIcons;
				ArcaneOdysseyMod.Sets.toggleablePulse[Type] = true;

				if (!ModContent.RequestIfExists(GetType().FullName.Replace('.', '/').Replace(Name, AttackPrefix + "Ray"), out ArcaneOdysseyMod.Sets.Assets.raySprites[Type]) & ArcaneOdysseyMod.DevMode)
				{
					ArcaneOdysseyMod.NoticeQueue.Add(Name + " is missing ray sprite");
				}

				if (!ModContent.RequestIfExists(GetType().FullName.Replace('.', '/').Replace(Name, AttackPrefix + "RayEnd"), out ArcaneOdysseyMod.Sets.Assets.rayEndSprites[Type]) & ArcaneOdysseyMod.DevMode)
				{
					ArcaneOdysseyMod.NoticeQueue.Add(Name + " is missing ray end sprite");
				}

				if (!ModContent.RequestIfExists(GetType().FullName.Replace('.', '/').Replace(Name, AttackPrefix + "RayStart"), out ArcaneOdysseyMod.Sets.Assets.rayStartSprites[Type]) & ArcaneOdysseyMod.DevMode)
				{
					ArcaneOdysseyMod.NoticeQueue.Add(Name + " is missing ray start sprite");
				}
			}
		}

		public override Color ImbueColour => SpiritColor;

		public Color SpiritColor
		{
			get
			{
				if (!EliusSpareSystem.spared)
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
					return new(0, 183, 255);
				}
			}
		}

		public override string ImbueUISprite
		{
			get
			{
				if (Type == ModContent.ItemType<SpiritEnergy>())
				{
					if (!EliusSpareSystem.spared)
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
				return base.ImbueUISprite;
			}
		}

		public static SpiritEnergy Instance => ModContent.GetInstance<SpiritEnergy>();

		public override SoundStyle? ImbueSound => SoundID.NPCDeath6;

		public override float ImbueSpeed => 1f;
		public override float ImbueDamage => 1f;
		public override float ImbueSize => 1f;

		public sealed override float ScrollSpeed
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

		public sealed override float ScrollDamage
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

		public sealed override float ScrollSize
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

		public sealed override int Drawback
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

		public sealed override float? DashResist => 1.2f;

		public sealed override string AttackPrefix => "Spirit";

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.DamageType = DamageClass.Summon;
			Item.useStyle = ItemUseStyleID.HiddenAnimation;

			if (Type == ModContent.ItemType<SpiritEnergy>())
			{
				Item.color = SpiritColor;
			}
		}

		public virtual int DustType => ModContent.DustType<SpiritDust>();

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

		public override void ConeEffects(Vector2 coneCenter, float coneLength, float coneRotation, float maximumAngle = 0)
		{
			AOUtils.NewDustImperfect(coneCenter, ModContent.DustType<SpiritDust>(), (coneRotation + Main.rand.NextFloat(-maximumAngle, maximumAngle)).ToRotationVector2() * (coneLength / 15f), newColor: SpiritColor, Scale: .1f * (coneLength / 25f), Alpha: 255 / 4);
			AOUtils.NewDustImperfect(coneCenter, DustType, (coneRotation + Main.rand.NextFloat(-maximumAngle, maximumAngle)).ToRotationVector2() * (coneLength / 15f), newColor: ImbueColour, Scale: .1f * (coneLength / 25f), Alpha: 255 / 4);
		}

		public override void SaveData(TagCompound tag)
		{
			base.SaveData(tag);
			if (Soul.Type > 0)
			{
				tag.Add("godsoul", Soul.FullName);
			}
		}

		public override void LoadData(TagCompound tag)
		{
			base.LoadData(tag);
			if (ModContent.TryFind<GodSoul>(tag.GetString("godsoul"), out var soul))
			{
				Soul = soul;
			}
		}
	}

	public class EliusSpareSystem : ModSystem
	{
		public static bool spared = true;

		public override void NetSend(BinaryWriter writer)
		{
			writer.Write(spared);
		}

		public override void NetReceive(BinaryReader reader)
		{
			spared = reader.ReadBoolean();
		}

		public override void SaveWorldData(TagCompound tag)
		{
			tag.Add("spared", spared);
		}

		public override void LoadWorldData(TagCompound tag)
		{
			spared = tag.GetBool("spared");
		}

		public override void OnWorldLoad()
		{
			spared = true;
		}

		public override void OnWorldUnload()
		{
			spared = true;
		}
	}
}
