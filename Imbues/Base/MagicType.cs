using ArcaneOdyssey.Projectiles;
using ArcaneOdyssey.Projectiles.Magic;
using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Imbues.Base
{
	public abstract class MagicType : Imbuable
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ArcaneOdysseyMod.Sets.Mutations[Type] = [];
			RegisterMutations();
			ArcaneOdysseyMod.Sets.Mutations[Type] = [.. ArcaneOdysseyMod.Sets.Mutations[Type].OrderBy(e => ModContent.GetModItem(e).DisplayName.Value)];
			ItemID.Sets.ItemNoGravity[Type] = true;
			ArcaneOdysseyMod.Sets.BlastMaxFrames[Type] = BlastFrames;
		}

		public override void AutoStaticDefaults()
		{
			base.AutoStaticDefaults();

			if (!ModContent.RequestIfExists(GetType().FullName.Replace('.', '/').Replace(Name, AttackPrefix + "Annihilation"), out ArcaneOdysseyMod.Sets.annihilationSprites[Type]) & ArcaneOdysseyMod.DevMode)
			{
				ArcaneOdysseyMod.NoticeQueue.Add(Name + " is missing annihilation sprite");
			}

			if (!ModContent.RequestIfExists(GetType().FullName.Replace('.', '/').Replace(Name, AttackPrefix + "Ray"), out ArcaneOdysseyMod.Sets.raySprites[Type]) & ArcaneOdysseyMod.DevMode) 
			{
				ArcaneOdysseyMod.NoticeQueue.Add(Name + " is missing ray sprite");
			}

			if (!ModContent.RequestIfExists(GetType().FullName.Replace('.', '/').Replace(Name, AttackPrefix + "RayEnd"), out ArcaneOdysseyMod.Sets.rayEndSprites[Type]) & ArcaneOdysseyMod.DevMode) 
			{
				ArcaneOdysseyMod.NoticeQueue.Add(Name + " is missing ray end sprite");
			}

			if (!ModContent.RequestIfExists(GetType().FullName.Replace('.', '/').Replace(Name, AttackPrefix + "RayStart"), out ArcaneOdysseyMod.Sets.rayStartSprites[Type]) & ArcaneOdysseyMod.DevMode) 
			{
				ArcaneOdysseyMod.NoticeQueue.Add(Name + " is missing ray start sprite");
			}

			if (!ModContent.RequestIfExists(GetType().FullName.Replace('.', '/').Replace(Name, AttackPrefix + "Blast"), out ArcaneOdysseyMod.Sets.blasts[Type]) & ArcaneOdysseyMod.DevMode) 
			{
				ArcaneOdysseyMod.NoticeQueue.Add(Name + " is missing blast sprite");
			}
		}

		public virtual void RegisterMutations() { }

		public void RegisterMutation<T>() where T : MagicType
		{
			ArcaneOdysseyMod.Sets.Mutations[Type].Add(ModContent.ItemType<T>());
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.mana = (10 * ScrollSpeed.FlipFloat()).Round();
			Item.DamageType = DamageClass.Magic;
			Item.shoot = ModContent.ProjectileType<BlastSpell>();
			Item.autoReuse = true;
			Item.damage = 10 + (100 * (int)ImbuableTier);
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
				CreateMagicCircle(Item, player, MagicCircleMode.Basic, true, DashSpeed >= 1.4f ? ModContent.ProjectileType<LesserBeam>() : type);
			}
			else
			{
				CreateMagicCircle(Item, player, MagicCircleMode.Rotating, true);
			}
			return false;
		}

		public virtual int BlastFrames => 7;
	}
}