using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Lost;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Normal
{
	public class GlassMagic : AOMagic
	{
		public override void RegisterMutations()
		{
			RegisterMutation<PrismMagic>();
			RegisterMutation<SoundMagic>();
			RegisterMutation<SlashMagic>();
		}
		public override bool Special => true;
		public override float? DashResist => 1.05f;
		public override float ItemInvisibility => .5f;
		public override SoundStyle? ImbueSound => SoundID.Shatter;
		public override Color ImbueColour => new(255, 255, 255);
		public override float AOImbueSpeed => 1f;
		public override float AOImbueSize => 1.053f;
		public override float AOImbueDamage => 1f;
		public override float AOScrollSpeed => 1f;
		public override float AOScrollSize => 1.1f;
		public override float AOScrollDamage => 0.9f;
		public override AODebuffRequirement[] ImbueDebuffs => [new(ModContent.BuffType<AOBleed>(), 60 * 10)];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
			
			],
			[
				new(BuffID.Venom,1.05f),
				new(ModContent.BuffType<Crystallized>(),0.92f),
				new(ModContent.BuffType<FreezingEffect>(),1.075f),
				new(ModContent.BuffType<SandyEffect>(),1.1f),
				new(BuffID.OnFire3,1.05f)
			]
			);
		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Glass, direction.X * 0.4f, direction.Y * 0.4f, Scale: area.RelativeScale());
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.SilverFlame, Scale: area.RelativeScale())];
			spawnedDust.noGravity = true;
			spawnedDust.noLight = true;
		}
		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust.NewDust(position, 0, 0, DustID.Glass, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), Scale: intensity);
			}
		}
		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 30; n++)
			{
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Glass, 2f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 2f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: area.RelativeScale());
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}
	}
}