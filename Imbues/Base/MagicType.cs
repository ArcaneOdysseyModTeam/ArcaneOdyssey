using ArcaneOdyssey.Imbues.Magic.Normal;
using ArcaneOdyssey.Projectiles;
using ArcaneOdyssey.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ArcaneOdyssey.Imbues.Base
{
	public abstract class MagicType : Imbuable
	{
		public abstract MagicCircleTypes CircleType { get; }

		public class MagicCircle
		{
			public override string ToString()
			{
				if (!ArcaneOdysseyClientConfig.Instance.UniqueMagicCircles)
				{
					return $"{ArcaneOdysseyMod.InternalName}/Effects/MagicCircles/Familiar";
				}
				return $"{ArcaneOdysseyMod.InternalName}/Effects/MagicCircles/{Type}_{Tier}";
			}

			public MagicCircleTypes Type;

			public ImbuableTiers Tier;

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

		public MagicCircle Circle 
		{
			get
			{
				return new MagicCircle { Tier = ImbuableTier, Type = CircleType };
			}
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ArcaneOdysseyMod.Sets.Mutations[Type] = [];
			RegisterMutations();
			ArcaneOdysseyMod.Sets.Mutations[Type] = [.. ArcaneOdysseyMod.Sets.Mutations[Type].OrderBy(e => ModContent.GetModItem(e).DisplayName.Value)];
			ItemID.Sets.ItemNoGravity[Type] = true;
			ArcaneOdysseyMod.Sets.BlastMaxFrames[Type] = BlastFrames;
			ItemID.Sets.ItemIconPulse[Type] = ArcaneOdysseyClientConfig.Instance.PulsingImbueIcons;

			if (!ModContent.RequestIfExists(GetType().FullName.Replace('.', '/').Replace(Name, AttackPrefix + "Annihilation"), out ArcaneOdysseyMod.Sets.Assets.annihilationSprites[Type]) & ArcaneOdysseyMod.DevMode)
			{
				ArcaneOdysseyMod.NoticeQueue.Add(Name + " is missing annihilation sprite");
			}

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

			if (!ModContent.RequestIfExists(GetType().FullName.Replace('.', '/').Replace(Name, AttackPrefix + "Blast"), out ArcaneOdysseyMod.Sets.Assets.blasts[Type]) & ArcaneOdysseyMod.DevMode) 
			{
				ArcaneOdysseyMod.NoticeQueue.Add(Name + " is missing blast sprite");
			}
		}

		public const string DefaultOriginalImbue = ArcaneOdysseyMod.InternalName + "." + nameof(WindMagic);

		public string BaseImbue
		{
			get
			{
				var type = ArcaneOdysseyMod.Sets.baseImbues[Type];
				if (type != -1)
				{
					var item = ModContent.GetModItem(type);
					if (item != null)
					{
						return item.Mod.Name + "." + item.Name;
					}
				}
				return DefaultOriginalImbue;
			}
		}

		public string OriginalImbue = DefaultOriginalImbue;

		public override void SaveData(TagCompound tag)
		{
			base.SaveData(tag);
			if (OriginalImbue != DefaultOriginalImbue)
				tag.Add("original", OriginalImbue);
		}

		public override void LoadData(TagCompound tag)
		{
			base.LoadData(tag);
			var str = tag.GetString("original");
			if (!string.IsNullOrEmpty(str))
				OriginalImbue = str;
			else
				OriginalImbue = BaseImbue;
		}

		public override void NetSend(BinaryWriter writer)
		{
			base.NetSend(writer);
			writer.Write(OriginalImbue);
		}

		public override void NetReceive(BinaryReader reader)
		{
			base.NetReceive(reader);
			OriginalImbue = reader.ReadString();
		}

		/// <summary>
		/// Will be useful for using hecate essense with lost/ancient magic later
		/// </summary>
		/// <returns></returns>
		public MagicType GetBaseImbue()
		{
			if (ModLoader.TryGetMod(OriginalImbue.Split('.')[0], out var mod))
			{
				if (mod.TryFind<ModItem>(OriginalImbue.Split('.')[1], out var item))
				{
					if (item is MagicType)
					{
						return item as MagicType;
					}
				}
			}
			return ModContent.GetInstance<WindMagic>();
		}

		public abstract void RegisterMutations();

		public void RegisterMutation<T>() where T : MagicType
		{
			ArcaneOdysseyMod.Sets.Mutations[Type].Add(ModContent.ItemType<T>());
		}

		public void RegisterDefaultMagic<T>() where T : MagicType
		{
			ArcaneOdysseyMod.Sets.baseImbues[Type] = ModContent.ItemType<T>();
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.mana = 5 * ((int)ImbuableTier + 1);
			Item.DamageType = DamageClass.Magic;
			Item.shoot = ModContent.ProjectileType<BlastSpell>();
			Item.autoReuse = true;
			Item.damage = 15 + (100 * (int)ImbuableTier);
			Item.shootSpeed = 7f * ScrollSpeed;
		}

		public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
		{
			if (player.AltUse())
				mult *= 0;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (!player.AltUse())
			{
				CreateMagicCircle(Item, player, MagicCircleMode.Basic, true, type);
			}
			else
			{
				CreateMagicCircle(Item, player, MagicCircleMode.Rotating, true);
			}
			return false;
		}

		public abstract int BlastFrames { get; }
	}
}