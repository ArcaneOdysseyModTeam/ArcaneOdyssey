using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Normal;
using ArcaneOdyssey.VFX.Dusts;
using Microsoft.Xna.Framework;
using ArcaneOdyssey.Content.Buffs.DOT;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Lost
{
	public class SlashMagic : AOMagic
	{
		public override AOImbuableTier ImbuableTier => AOImbuableTier.Lost;
		public override float AOImbueDamage => 1.1f;
		public override float AOImbueSpeed => 1.1f;
		public override float AOImbueSize => .8f;
		public override Color ImbueColour => Color.White;
		public override AODebuffRequirement[] ImbueDebuffs => [new(ModContent.BuffType<AOBleed>(), 60 * 10)];

		public override void LingeringEffects(Entity entity)
		{
			Dust.NewDust(entity.position, entity.width, entity.height, ModContent.DustType<SlashDust>());
		}

		public override void ExplosionEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + projectile.width / 2f, projectile.position.Y + projectile.height / 2f), 0, 0, ModContent.DustType<SlashDust>(), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 1f)];
				spawnedDust.noGravity = true;
			}
		}

		public override void AddRecipes()
		{
			CreateLostRecipe(typeof(MetalMagic), typeof(GlassMagic), typeof(WoodMagic));
		}
	}
}
