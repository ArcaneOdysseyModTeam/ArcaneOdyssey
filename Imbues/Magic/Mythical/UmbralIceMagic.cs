using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Buffs.Stuns;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Gimmicks.Magic;
using ArcaneOdyssey.Imbues.Magic.Lost;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Imbues.Magic.Mythical
{
	public class UmbralIceMagic : MagicType
	{
		public override ImbueGimmick Gimmick => ModContent.GetInstance<FrostShards>();
		public override MagicCircleTypes CircleType => MagicCircleTypes.Collision;

		public override int BlastFrames => 4;

		public override Color ImbueColour => new(30, 200, 255);
		public override Color ImbueColour2 => Color.Black;
		public override SoundStyle? ImbueSound => SoundID.Item27;


		public override ImbuableTiers ImbuableTier => ImbuableTiers.Mythical;

		public override bool AnimatedColours => true;
		public override float? DashResist => 1.3f;
		public override void SetStaticDefaults() { base.SetStaticDefaults(); ArcaneOdysseyMod.Sets.cold[Type] = true; }
		public override float Aura => 1.1f;
		public override float ScrollSize => 1.2f;
		public override float ScrollDamage => 3f;
		public override float ScrollSpeed => 2f;
		public override Debuff[] ImbueDebuffs => [Debuff.Create<FreezingEffect>(), Debuff.Create<Frozen>(60, 33), Debuff.Create<DrainedEffect>(60 * 6)];
		public override Combo[] CombinedDebuffs => [Combo.Create<Soaked, Frozen>()];

		public override SynergyEffects Effects => AOUtils.CopySynergiesFromImbue<DarknessMagic>() + AOUtils.CopySynergiesFromImbue<FrostmetalMagic>();

		public override void RegisterMutations()
		{

		}

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 2; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.SnowflakeIce, direction.X * 0.5f, direction.Y * 0.5f, Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Ice, direction.X * 0.5f, direction.Y * 0.5f, Scale: 2f * area.RelativeScale());
				Dust spawnedDust3 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Wraith, direction.X * 2f, direction.Y * 2f, Scale: 3f * area.RelativeScale())];
				spawnedDust3.noGravity = true;
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Ice, Scale: area.RelativeScale());
			Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Wraith, Scale: 2f * area.RelativeScale())];
			spawnedDust.noGravity = true;
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 2; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.SnowflakeIce, (Main.rand.NextFloat() - 0.5f) * (15f * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * intensity), Scale: 3f * intensity)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(position, 0, 0, DustID.Ice, (Main.rand.NextFloat() - 0.5f) * (15f * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * intensity), Scale: 2f * intensity)];
				spawnedDust2.noGravity = true;
				Dust spawnedDust3 = Main.dust[Dust.NewDust(position, 0, 0, DustID.Wraith, (Main.rand.NextFloat() - 0.5f) * (15f * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * intensity), Scale: 3f * intensity)];
				spawnedDust3.noGravity = true;
			}
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 7; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.SnowflakeIce, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Ice, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 2f * area.RelativeScale());
				Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Wraith, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale())];
				spawnedDust2.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}
	}
}
