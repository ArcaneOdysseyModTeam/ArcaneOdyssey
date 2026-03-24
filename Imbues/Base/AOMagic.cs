using ArcaneOdyssey.Projectiles;
using ArcaneOdyssey.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Imbues.Base
{
	public abstract class AOMagic : Imbuable
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ArrayCollections.Mutations[Type] = [];
			RegisterMutations();
			ArrayCollections.Mutations[Type] = [.. ArrayCollections.Mutations[Type].OrderBy(e => ModContent.GetModItem(e).DisplayName.Value)];
			ItemID.Sets.ItemNoGravity[Type] = true;
		}

		public override void AutoStaticDefaults()
		{
			base.AutoStaticDefaults();

			if (ModContent.RequestIfExists<Texture2D>(AOUtils.GetTexture<AnnihilationSpell>().Replace(nameof(AnnihilationSpell), $"Annihilations/{ImbuableTier}/{AttackPrefix}Annihilation"), out var annihilation))
			{
				ArrayCollections.annihilationSprites[Type] = annihilation;
			}

			if (ModContent.RequestIfExists<Texture2D>(AOUtils.GetTexture<MagicRay>().Replace(nameof(MagicRay), $"Rays/{ImbuableTier}/{AttackPrefix}Ray"), out var ray))
			{
				ArrayCollections.raySprites[Type] = ray;
			}

			if (ModContent.RequestIfExists<Texture2D>(AOUtils.GetTexture<MagicRay>().Replace(nameof(MagicRay), $"Rays/{ImbuableTier}/{AttackPrefix}RayEnd"), out var rayend))
			{
				ArrayCollections.rayEndSprites[Type] = rayend;
			}

			if (ModContent.RequestIfExists<Texture2D>(AOUtils.GetTexture<MagicRay>().Replace(nameof(MagicRay), $"Rays/{ImbuableTier}/{AttackPrefix}RayStart"), out var raystart))
			{
				ArrayCollections.rayStartSprites[Type] = raystart;
			}
		}

		public virtual void RegisterMutations() { }

		public void RegisterMutation<T>() where T : AOMagic
		{
			ArrayCollections.Mutations[Type].Add(ModContent.ItemType<T>());
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.mana = (10 * ScrollSpeed.FlipFloat()).Round();
			Item.DamageType = DamageClass.Magic;
			Item.shoot = GetSkill("Blast");
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
	}
}