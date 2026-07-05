using ArcaneOdyssey.Imbues.Magic.Normal;
using ArcaneOdyssey.Projectiles;
using ArcaneOdyssey.Projectiles.Magic;
using ArcaneOdyssey.Skills.Base;
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
		public override void Load()
		{
			base.Load();
			ModTypeLookup<MagicType>.Register(this);
		}

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
			ArcaneOdysseyMod.Sets.Mutations[Type] = [.. ArcaneOdysseyMod.Sets.Mutations[Type].OrderBy(e => ModContent.GetModItem(e).DisplayName.Value)];
			ItemID.Sets.ItemNoGravity[Type] = true;
			ArcaneOdysseyMod.Sets.BlastMaxFrames[Type] = BlastFrames;

			ItemID.Sets.ItemIconPulse[Type] = ArcaneOdysseyClientConfig.Instance.PulsingImbueIcons;
			ArcaneOdysseyMod.Sets.toggleablePulse[Type] = true;

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

		public static int DefaultOriginalImbue => WindMagic.ID;

		private Imbuable _og = null;
		public Imbuable OriginalImbue { get => _og ?? AOUtils.Safe<Imbuable>(ModContent.GetModItem(ArcaneOdysseyMod.Sets.baseImbues[Type] ?? DefaultOriginalImbue)); set => _og = value; }
		private string cachedUnloadedBase = null;

		public override void SaveData(TagCompound tag)
		{
			base.SaveData(tag);
			if (_og is not null || cachedUnloadedBase is not null)
				tag.Add("baseimbue", _og?.FullName ?? cachedUnloadedBase);
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
		}

		public override void NetSend(BinaryWriter writer)
		{
			base.NetSend(writer);
			writer.Write(OriginalImbue.Type);
			writer.Write(cachedUnloadedBase ?? "");
		}

		public override void NetReceive(BinaryReader reader)
		{
			base.NetReceive(reader);
			OriginalImbue = AOUtils.Safe<Imbuable>(ModContent.GetModItem(reader.ReadInt32()));
			cachedUnloadedBase = reader.ReadString();
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

		public abstract int BlastFrames { get; }
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