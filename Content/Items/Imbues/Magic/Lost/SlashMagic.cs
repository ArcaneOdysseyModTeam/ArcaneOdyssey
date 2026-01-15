using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Normal;
using ArcaneOdyssey.VFX.Dusts;
using Microsoft.Xna.Framework;
using ArcaneOdyssey.Content.Buffs.DOT;
using Terraria;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Lost
{
	public class SlashMagic : AOMagic
	{
		public override AOImbuableTier ImbuableTier => AOImbuableTier.Lost;
		public override float AOImbueDamage => 1.2f;
		public override float AOImbueSpeed => 1.1f;
		public override float AOImbueSize => .8f;
		public override Color ImbueColour => Color.White;
		public override AODebuffRequirement[] ImbueDebuffs => [new(ModContent.BuffType<AOBleed>(), 60 * 10)];
		public override SoundStyle? ImbueSound => SoundID.Item71;

		public override void LingeringEffects(Entity entity)
		{
			Dust.NewDust(entity.position, entity.width, entity.height, ModContent.DustType<SlashDust>());
		}

		public override void ExplosionEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + projectile.width / 2f, projectile.position.Y + projectile.height / 2f), 0, 0, ModContent.DustType<SlashDust>(), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize))];
				spawnedDust.noGravity = true;
			}
		}

		public override void KillEffects(Entity entity)
		{
			for (float i = 0; i < 70; i++)
			{
				var centre = (MathHelper.TwoPi / 25 * i).ToRotationVector2() * ((entity.width + entity.height) / 2);
				if (i % 2 == 0)
					AOUtils.NewDustImperfect(entity.Center, ModContent.DustType<SlashDust>(), centre / (8 + (Main.rand.NextFloat() * 2)), scale: .7f).noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, entity.Center, null);
		}

		public override void SpawningEffects(Entity entity)
		{
			for (float i = 0; i < 5, i++)
			{
				Dust.NewDust(entity.position, entity.width, entity.height, ModContent.DustType<SlashDust>(), entity.velocity.X / 2, entity.velocity.Y / 2, Scale: .5f)
			}
		}

		public override void AddRecipes()
		{
			CreateLostRecipe(typeof(MetalMagic), typeof(GlassMagic), typeof(WoodMagic));
		}
	}
}
