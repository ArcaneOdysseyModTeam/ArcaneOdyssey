using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Buffs.Stuns;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Ancient;
using ArcaneOdyssey.VFX.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Lost
{
	public class HeatMagic : AOMagic
	{
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
		public override AODebuffRequirement[] ImbueDebuffs => [new(ModContent.BuffType<SearedEffect>(), 60 * 10)];

		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				BuffID.Wet,
				BuffID.Slimed,
				ModContent.BuffType<FreezingEffect>(),
				ModContent.BuffType<SnowyEffect>(),
				ModContent.BuffType<AOFrozen>()
			],
			[
				new(ModContent.BuffType<AOBleed>(),1.15f),
				new(ModContent.BuffType<SnowyEffect>(),0.99f),
				new(BuffID.Poisoned,1.05f),
				new(BuffID.Slimed,1.075f),
				new(BuffID.Oiled,1.075f),
				new(BuffID.Venom,1.075f),
				new(BuffID.OnFire3,1.075f),
				new(ModContent.BuffType<CharredEffect>(),1.1f),
				new(ModContent.BuffType<FreezingEffect>(),0.95f),
				new(BuffID.OnFire,1.125f),
				new(ModContent.BuffType<Crystallized>(),1.075f),
				new(BuffID.ShadowFlame,1.15f),
				new(ModContent.BuffType<AOScalding>(),1.125f),
				new(BuffID.Wet,0.9f),
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