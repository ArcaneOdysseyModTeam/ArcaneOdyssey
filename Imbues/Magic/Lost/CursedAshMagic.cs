using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Buffs.Stuns;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Magic.Ancient;
using ArcaneOdyssey.Imbues.Gimmicks.Magic;
using ArcaneOdyssey.Imbues.Magic.Normal;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Imbues.Magic.Lost
{
	public class CursedAshMagic : MagicType
	{
		public override float DashSpeed => 1.2f; // burst
		public override Color ImbueColour => Color.Violet;
		public override Color ImbueColour2 => Color.PaleVioletRed;
		public override bool AnimatedColours => true;
		public override ImbuableTiers ImbuableTier => ImbuableTiers.Lost;
		public override ImbueGimmick Gimmick => ModContent.GetInstance<AshClouds>();
		public override void SetStaticDefaults() { base.SetStaticDefaults(); ArcaneOdysseyMod.Sets.cold[Type] = false; }
		public override float ScrollDamage => 0.9f;

		public override float ScrollSpeed => 1f;

		public override float ScrollSize => 1.3f;

		public override Debuff[] ImbueDebuffs => [Debuff.Create<CursedAshes>(60 * 5), Debuff.Create<Petrified>(60, 25)];

		public override SynergyEffects Effects => AOUtils.CopySynergiesFromImbue<AshMagic>();

		public override Combo[] CombinedDebuffs => AOUtils.CopyCombosFromImbue<AshMagic>();

		public override int BlastFrames => 7;

		public override void RegisterMutations()
		{
			RegisterMutation<DeathMagic>();
			RegisterDefaultMagic<AshMagic>();
		}

		public override MagicCircleTypes CircleType => MagicCircleTypes.Demonic;

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Ash, direction.X * 2f, direction.Y * 2f, Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.CorruptTorch, direction.X * 2f, direction.Y * 2f, Scale: 2f * area.RelativeScale())];
				spawnedDust2.noGravity = true;
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.CorruptTorch, Scale: 1f * area.RelativeScale());
			Dust spawnedDust = Dust.NewDustDirect(area.TopLeft(), area.Width, area.Height, DustID.Ash, Scale: 2f * area.RelativeScale());
			spawnedDust.noGravity = true;
		}
		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust.NewDust(position, 0, 0, DustID.CorruptSpray, (Main.rand.NextFloat() - 0.5f) * (2f * intensity), (Main.rand.NextFloat() - 0.5f) * (2f * intensity), Scale: intensity);
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.Ash, (Main.rand.NextFloat() - 0.5f) * (15f * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * intensity), Scale: 2f * intensity)];
				spawnedDust.noGravity = true;
			}
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Dust.NewDustDirect(area.TopLeft(), area.Width, area.Height, DustID.Ash, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale());
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Dust.NewDustDirect(area.TopLeft(), area.Width, area.Height, DustID.CorruptTorch, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 2f * area.RelativeScale());
				spawnedDust2.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}
	}
}
