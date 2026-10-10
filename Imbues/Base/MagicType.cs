using ArcaneOdyssey.Imbues.Magic.Normal;
using ArcaneOdyssey.MagicVariants.Base;
using ArcaneOdyssey.Projectiles;
using ArcaneOdyssey.Projectiles.Magic;
using ArcaneOdyssey.Skills.Base;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader.IO;

namespace ArcaneOdyssey.Imbues.Base
{
	public abstract class MagicType : Imbuable
	{
		public override void Load()
		{
			base.Load();
			ModTypeLookup<MagicType>.Register(this);
		}

		public static MagicType GenerateMagicType(int id, MagicVariant variant)
		{
			var imbue = AOUtils.Safe<MagicType>(GetImbuable(id));
			if (imbue is not null && variant is not null)
			{
				imbue.magicVariant = variant;
				imbue.VariantColour = variant.Colour;
			}
			return imbue;
		}
		public static MagicType GenerateMagicType(int id, int variant)
		{
			var imbue = AOUtils.Safe<MagicType>(GetImbuable(id));
			if (variant >= 0)
				if (imbue is not null)
				{
					imbue.magicVariant = MagicVariant.GetFromID(variant);
					imbue.VariantColour = MagicVariant.GetFromID(variant).Colour;
				}
			return imbue;
		}

		public IEnumerable<MagicVariant> Variants => ModContent.GetContent<MagicVariant>().Where(e => e.Imbue == ID);

		public static T GenerateMagicType<T>(MagicVariant variant) where T : MagicType
		{
			var imbue = ModContent.GetInstance<T>();
			imbue.magicVariant = variant;
			imbue._colour = variant?.Colour;
			return imbue;
		}

		public static MagicType GenerateMagicType(int id, Color variant, string name)
		{
			var imbue = AOUtils.Safe<MagicType>(GetImbuable(id));
			if (imbue is not null)
			{
				imbue.VariantColour = variant;
				imbue.magicVariant = null;
				imbue.unloadedMagicVariant = name;
			}
			return imbue;
		}

		public void SetMagicVariant(MagicVariant variant = null)
		{
			if (variant is not null)
			{
				VariantColour = variant.Colour;
				magicVariant = variant;
				unloadedMagicVariant = "";
			}
			else
			{
				_colour = null;
				magicVariant = null;
				unloadedMagicVariant = "";
			}
		}

		public Color VariantColour { get => _colour ?? ImbueColour; private set => _colour = value; }
		private Color? _colour;
		public MagicVariant magicVariant = null;
		public string unloadedMagicVariant = "";

		public override AttackSkill DefaultAttack => ModContent.GetInstance<MagicBlastSkill>();

		public sealed override float ImbueDamage => base.ImbueDamage;
		public sealed override float ImbueSize => base.ImbueSize;
		public sealed override float ImbueSpeed => base.ImbueSpeed;

		public abstract MagicCircleTypes CircleType { get; }

		public class MagicCircle(ImbuableTiers tier, MagicCircleTypes type)
		{
			public override string ToString()
			{
				if (!ArcaneOdysseyClientConfig.Instance.UniqueMagicCircles)
				{
					return $"{ArcaneOdysseyMod.InternalName}/Effects/MagicCircles/Familiar";
				}
				return $"{ArcaneOdysseyMod.InternalName}/Effects/MagicCircles/{Type}_{Tier.ToString().Replace("Mythical", "Dragon")}";
			}

			public MagicCircleTypes Type = type;

			public ImbuableTiers Tier = tier;

			public Asset<Texture2D> Texture
			{
				get
				{
					if (ArcaneOdysseyMod.Sets.Assets.MagicCircles.TryGetValue(ToString(), out var tex))
					{
						return tex;
					}
					else
					{
						tex = ModContent.Request<Texture2D>(ToString());
						ArcaneOdysseyMod.Sets.Assets.MagicCircles[ToString()] = tex;
						return tex;
					}
				}
			}
		}

		public MagicCircle Circle => new(ImbuableTier, CircleType);

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			RegisterMutations();
			ImbueID.Sets.Mutations[ID] = [.. ImbueID.Sets.Mutations[ID].OrderBy(e => ImbueID.Search.GetName(e))];
			ItemID.Sets.ItemNoGravity[Type] = true;

			ItemID.Sets.ItemIconPulse[Type] = ArcaneOdysseyClientConfig.Instance.PulsingImbueIcons;
			ArcaneOdysseyMod.Sets.toggleablePulse[Type] = true;

			ModContent.RequestIfExists(GetType().FullName.Replace('.', '/').Replace(Name, AttackPrefix + "Annihilation"), out ImbueID.Sets.Assets.annihilationSprites[ID], AssetRequestMode.ImmediateLoad);

			ModContent.RequestIfExists(GetType().FullName.Replace('.', '/').Replace(Name, AttackPrefix + "Ray"), out ImbueID.Sets.Assets.raySprites[ID], AssetRequestMode.ImmediateLoad);

			ModContent.RequestIfExists(GetType().FullName.Replace('.', '/').Replace(Name, AttackPrefix + "RayEnd"), out ImbueID.Sets.Assets.rayEndSprites[ID], AssetRequestMode.ImmediateLoad);

			ModContent.RequestIfExists(GetType().FullName.Replace('.', '/').Replace(Name, AttackPrefix + "RayStart"), out ImbueID.Sets.Assets.rayStartSprites[ID], AssetRequestMode.ImmediateLoad);

			if (ModContent.RequestIfExists(GetType().FullName.Replace('.', '/').Replace(Name, AttackPrefix + "Blast"), out ImbueID.Sets.Assets.blasts[ID], AssetRequestMode.ImmediateLoad))
			{
				if (ImbueID.Sets.Assets.blasts[ID].IsLoaded)
					ImbueID.Sets.BlastFrames[ID] = (ImbueID.Sets.Assets.blasts[ID].Height() / ((float)ImbueID.Sets.Assets.blasts[ID].Width())).Round();
			}
		}

		public override Asset<Texture2D> Annihilation
		{
			get
			{
				return magicVariant?.Annihilation ?? ImbueID.Sets.Assets.annihilationSprites[ID];
			}
			set
			{
				ImbueID.Sets.Assets.annihilationSprites[ID] = value;
			}
		}

		public override Asset<Texture2D> Ray
		{
			get
			{
				return magicVariant?.Ray ?? ImbueID.Sets.Assets.raySprites[ID];
			}
			set
			{
				ImbueID.Sets.Assets.raySprites[ID] = value;
			}
		}

		public override Asset<Texture2D> RayEnd
		{
			get
			{
				return magicVariant?.RayEnd ?? ImbueID.Sets.Assets.rayEndSprites[ID];
			}
			set
			{
				ImbueID.Sets.Assets.rayEndSprites[ID] = value;
			}
		}

		public override Asset<Texture2D> RayStart
		{
			get
			{
				return magicVariant?.RayStart ?? ImbueID.Sets.Assets.rayStartSprites[ID];
			}
			set
			{
				ImbueID.Sets.Assets.rayStartSprites[ID] = value;
			}
		}

		public override Asset<Texture2D> Blast
		{
			get
			{
				return magicVariant?.Blast ?? ImbueID.Sets.Assets.blasts[ID];
			}
			set
			{
				ImbueID.Sets.Assets.blasts[ID] = value;
			}
		}

		public override string Texture => ModContent.HasAsset(base.Texture) ? base.Texture : (ImbueID.Sets.DefaultVariant[ID] >= 0 ? MagicVariant.GetFromID(ImbueID.Sets.DefaultVariant[ID]).Texture : AOUtils.BlankTexture);

		public override Texture2D Sprite => magicVariant?.Icon?.Value ?? ((Texture != $"{Mod.Name}/{TextureAssets.Item[Type]?.Name.Replace("\\", "/") ?? Texture}" ? AOUtils.Request(Texture, ref TextureAssets.Item[Type]) : TextureAssets.Item[Type])?.Value);

		public static int DefaultOriginalImbue => AOUtils.ImbuableID<WindMagic>();

		private Imbuable _og = null;
		public virtual Imbuable OriginalImbue
		{
			get
			{
				return _og ?? GenerateMagicType(ImbueID.Sets.baseImbues[ID] ?? DefaultOriginalImbue, ImbueID.Sets.DefaultVariant[ImbueID.Sets.baseImbues[ID] ?? DefaultOriginalImbue]);
			}

			set
			{
				if (value is MagicType magic)
				{
					_og = GenerateMagicType(value.ID, magic.magicVariant);
				}
				_og = AOUtils.Safe<Imbuable>(ModContent.Find<Imbuable>(value.FullName));
			}
		}

		private string cachedUnloadedBase = null;

		public override void SaveData(TagCompound tag)
		{
			base.SaveData(tag);
			if (_og is not null || cachedUnloadedBase is not null)
				tag.Add("baseimbue", _og?.FullName ?? cachedUnloadedBase);

			if (!unloadedMagicVariant.IsNullOrWhiteSpace())
			{
				tag.Add("variantname", unloadedMagicVariant);
				tag.Add("variantcolourr", VariantColour.R);
				tag.Add("variantcolourg", VariantColour.G);
				tag.Add("variantcolourb", VariantColour.B);
			}
			else if (magicVariant is not null)
			{
				tag.Add("variantname", magicVariant.FullName);
			}
		}

		public override void LoadData(TagCompound tag)
		{
			base.LoadData(tag);

			var imbuename = tag.GetString("baseimbue");
			if (ModContent.TryFind<Imbuable>(imbuename, out var value))
			{
				OriginalImbue = value;
			}
			else
			{
				cachedUnloadedBase = imbuename;
			}

			var variantname = tag.GetString("variantname");
			if (variantname.IsNullOrWhiteSpace())
			{
				if (ImbueID.Sets.DefaultVariant[ID] >= 0)
				{
					SetMagicVariant(MagicVariant.GetFromID(ImbueID.Sets.DefaultVariant[ID]));
				}
				else
				{
					SetMagicVariant();
				}
			}
			else
			{
				if (ModContent.TryFind<MagicVariant>(variantname, out var variant))
				{
					SetMagicVariant(variant);
				}
				else
				{
					SetMagicVariant();
					unloadedMagicVariant = variantname;
					VariantColour = new(tag.GetByte("variantcolourr"), tag.GetByte("variantcolourg"), tag.GetByte("variantcolourb"));
				}
			}
		}

		public override void NetSend(BinaryWriter writer)
		{
			base.NetSend(writer);
			writer.Write(OriginalImbue.Type);
			writer.Write(cachedUnloadedBase ?? "");
			writer.Write(unloadedMagicVariant);
			if (!unloadedMagicVariant.IsNullOrWhiteSpace())
			{
				writer.WriteRGB(VariantColour);
			}
			writer.Write(magicVariant?.Type);
		}

		public override void NetReceive(BinaryReader reader)
		{
			base.NetReceive(reader);
			OriginalImbue = AOUtils.Safe<Imbuable>(ModContent.GetModItem(reader.ReadInt32()));
			cachedUnloadedBase = reader.ReadString();
			unloadedMagicVariant = reader.ReadString();
			if (!unloadedMagicVariant.IsNullOrWhiteSpace())
			{
				VariantColour = reader.ReadRGB();
			}
			magicVariant = MagicVariant.GetFromID(reader.ReadNullableInt32().GetValueOrDefault(-1));
		}

		public abstract void RegisterMutations();

		public void RegisterMutation<T>() where T : MagicType
		{
			ImbueID.Sets.Mutations[ID].Add(AOUtils.ImbuableID<T>());
		}

		public static void RegisterMutation<TMutate, TResult>() where TMutate : MagicType where TResult : MagicType
		{
			ModContent.GetInstance<TMutate>().RegisterMutation<TResult>();
		}

		public void RegisterMutationFrom<T>() where T : MagicType
		{
			ImbueID.Sets.Mutations[AOUtils.ImbuableID<T>()].Add(ID);
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.DamageType = DamageClass.Magic;
		}

		public void RegisterDefaultMagic<T>() where T : MagicType
		{
			ImbueID.Sets.baseImbues[ID] = AOUtils.ImbuableID<T>();
		}

		public sealed override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.AltUse())
			{
				CreateMagicCircle(Item, player, MagicCircleMode.Rotating, true);
			}
			else
			{
				return base.Shoot(player, source, position, velocity, type, damage, knockback);
			}
			return false;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			base.ModifyTooltips(tooltips);

			if (magicVariant is not null)
			{
				var line = new TooltipLine(Mod, "MagicVariant", magicVariant.DisplayName.Value)
				{
					OverrideColor = Colour
				};
				tooltips.Insert(1, line);
			}
			else if (!unloadedMagicVariant.IsNullOrWhiteSpace())
			{
				var line = new TooltipLine(Mod, "MagicVariant", VariantColour.Hex3())
				{
					OverrideColor = Colour
				};
				tooltips.Insert(1, line);
			}
		}

		public override void Update(ref float gravity, ref float maxFallSpeed)
		{
			base.Update(ref gravity, ref maxFallSpeed);
			if (ImbueID.Sets.HasVariants[ID] && !ImbueID.Sets.VariantsUseSprites[ID])
				Item.color = Colour;
		}

		public override void UpdateInventory(Player player)
		{
			base.UpdateInventory(player);
			if (ImbueID.Sets.HasVariants[ID] && !ImbueID.Sets.VariantsUseSprites[ID])
				Item.color = Colour;
		}
	}

	public abstract class MagicType<T> : MagicType where T : ImbueGimmick
	{
		public sealed override ImbueGimmick Gimmick => ModContent.GetInstance<T>();
	}

	public class MagicBlastSkill : AttackSkill
	{
		public override int Damage => 15;

		public override int Shoot => ModContent.ProjectileType<BlastSpell>();

		public override int Scroll => 0;

		public override int ManaCost => 5;

		public override float Speed => 7f;

		public override bool Attack(Player player, Imbuable imbue, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockback)
		{
			imbue.CreateMagicCircle(player, MagicCircleMode.Basic, true, Shoot);
			return false;
		}
	}
}