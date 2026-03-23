using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Magic.Lost;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace ArcaneOdyssey.Imbues.Magic.Normal
{
	public class EarthMagic : AOMagic
	{
		public override float Aura => 1.3f;
		public override void RegisterMutations()
		{
			RegisterMutation<DiamondMagic>();
			RegisterMutation<GravityMagic>();
			RegisterMutation<OilMagic>();
			RegisterMutation<PlantMagic>();
		}
		public override float? DashResist => 1.4f;
		public override Color ImbueColour => new(69, 42, 1);
		public override float ImbueSpeed => 0.85f;
		public override float ImbueSize => 1.26f;
		public override float ImbueDamage => 1.075f;
		public override float ScrollSpeed => 0.7f;
		public override float ScrollSize => 1.3f;
		public override float ScrollDamage => 1f;
		public override SoundStyle? ImbueSound => SoundID.Item110;
		public override Debuff[] ImbueDebuffs => [Debuff.Create<AOBleed>()];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				ClearBuff.Create<FreezingEffect>()
			],
			[
				Synergy.Create<AOBleed>(1.1f),
				
				Synergy.Create<Corroding>(1.075f),
				Synergy.Create<Crystallized>(1.075f),
				Synergy.Create<FreezingEffect>(1.02f),
				
				Synergy.Create<Melting>(1.075f),
				Synergy.Create<SandyEffect>(1.1f)
			]
			);



		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Dirt, direction.X * 2f, direction.Y * 2f, Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
		}
		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Dirt, Scale: area.RelativeScale());
		}
		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust.NewDust(position, 0, 0, DustID.Dirt, (Main.rand.NextFloat() - 0.5f) * (15f * ScrollSize * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * ScrollSize * intensity), Scale: 3f * intensity);
			}
		}
		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Dirt, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}
	}
}