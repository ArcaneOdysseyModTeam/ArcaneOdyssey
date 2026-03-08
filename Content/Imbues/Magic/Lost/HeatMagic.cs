using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Buffs.Stuns;
using ArcaneOdyssey.Content.Imbues.Magic.Ancient;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.VFX.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Imbues.Magic.Lost
{
	public class HeatMagic : AOMagic
	{
		public override float Aura => .6f;
		public override void RegisterMutations()
		{
			RegisterMutation<IonMagic>();
		}
		public override float DashSpeed => 1.2f; // burst
		public override Color ImbueColour => new(255, 0, 0);
		public override bool? Cold => false;
		public override bool CanBeWet => false;
		public override float AOScrollSpeed => 1.3f;
		public override float AOScrollSize => 1.2f;
		public override float AOScrollDamage => .85f;
		public override AOImbuableTier ImbuableTier => AOImbuableTier.Lost;
		public override Debuff[] ImbueDebuffs => [Debuff.Create<SearedEffect>()];

		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				ClearBuff.Create<Soaked>(),
				ClearBuff.Create<Flammable>(),
				ClearBuff.Create<FreezingEffect>(),
				ClearBuff.Create<SnowyEffect>(),
				ClearBuff.Create<AOFrozen>()
			],
			[
				Synergy.Create<AOBleed>(1.15f),
				Synergy.Create<SnowyEffect>(0.99f),
				Synergy.Create<AOPoisoned>(1.05f),
				Synergy.Create<Flammable>(1.075f),
				Synergy.Create<Corroding>(1.075f),
				Synergy.Create<Melting>(1.075f),
				Synergy.Create<CharredEffect>(1.1f),
				Synergy.Create<FreezingEffect>(0.95f),
				Synergy.Create<AOBurning>(1.125f),
				Synergy.Create<Crystallized>(1.075f),
				Synergy.Create<Scorched>(1.15f),
				Synergy.Create<Scalding>(1.125f),
				Synergy.Create<Soaked>(0.9f),
			]
			);

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 2; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, ModContent.DustType<HeatDust>(), direction.X * 2f, direction.Y * 2f, Alpha: (255 * .75f).Round(), Scale: area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			for (int n = 0; n < 2; n++)
			{
				var spawnedDust = Dust.NewDustDirect(area.TopLeft(), area.Width, area.Height, ModContent.DustType<HeatDust>(), Alpha: (255 * .75f).Round(), Scale: area.RelativeScale());
				spawnedDust.noGravity = true;
			}
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 6; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, ModContent.DustType<HeatDust>(), (Main.rand.NextFloat() - 0.5f) * (30f * intensity * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (30f * intensity * AOScrollSize), Alpha: (255 * .75f).Round(), Scale: intensity)];
				spawnedDust.noGravity = true;
			}
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 20; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, ModContent.DustType<HeatDust>(), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Alpha: (255 * .75f).Round(), Scale: area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}
	}
}