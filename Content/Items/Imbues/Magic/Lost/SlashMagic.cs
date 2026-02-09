using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Normal;
using ArcaneOdyssey.VFX.Dusts;
using Microsoft.Xna.Framework;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
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
		public override float AOScrollDamage => 1.2f;
		public override float AOScrollSpeed => 1.1f;
		public override float AOScrollSize => .8f;
		public override Color ImbueColour => Color.White;
		public override AODebuffRequirement[] ImbueDebuffs => [new(ModContent.BuffType<AOBleed>(), 60 * 10)];
		public override SoundStyle? ImbueSound => SoundID.Item71;
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				ModContent.BuffType<FreezingEffect>()
			],
			[
				new(BuffID.Venom,1.05f),
				new(ModContent.BuffType<Crystallized>(),1.05f),
				new(ModContent.BuffType<FreezingEffect>(),1.02f),
				new(BuffID.OnFire3,1.05f),
				new(ModContent.BuffType<SandyEffect>(),1.1f)

			]
			);

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, ModContent.DustType<SlashDust>(), Alpha: 60, Scale: area.RelativeScale());
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, ModContent.DustType<SlashDust>(), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), Alpha: 60, Scale: intensity)];
				spawnedDust.noGravity = true;
			}
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (float i = 0; i < 70; i++)
			{
				var centre = (MathHelper.TwoPi / 25 * i).ToRotationVector2() * ((area.Width + area.Height) / 2);
				if (i % 2 == 0)
					AOUtils.NewDustImperfect(area.Center(), ModContent.DustType<SlashDust>(), centre / (8 + (Main.rand.NextFloat() * 2)), Alpha: 60, Scale: .7f * area.RelativeScale()).noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (float i = 0; i < 5; i++)
			{
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, ModContent.DustType<SlashDust>(), direction.X / 2f, direction.Y / 2f, Alpha: 60, Scale: .5f * area.RelativeScale());
			}
		}

		public override void AddRecipes()
		{
			CreateLostRecipe(typeof(MetalMagic), typeof(GlassMagic), typeof(WoodMagic),typeof(WindMagic));
		}
	}
}
