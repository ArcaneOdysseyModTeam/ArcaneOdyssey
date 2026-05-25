using ArcaneOdyssey.Imbues.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace ArcaneOdyssey.Imbues.Magic.Dragon
{
    public class DeliriumMagic : MagicType
    {
        public override Color ImbueColour => Color.White;
		public override Color ImbueColour2 => Color.Black;
		public override ColourTransitionStyle TransitionStyle => ColourTransitionStyle.Tangent;
        public override float ScrollDamage => 0.5f;
        public override float ScrollSize => 5f;
        public override float ScrollSpeed => 2.3f;
        public override ImbuableTiers ImbuableTier => ImbuableTiers.Dragon;
        public override Debuff[] ImbueDebuffs => [new(BuffID.Confused, 60)];

		public override MagicCircleTypes CircleType => MagicCircleTypes.Demonic;

		public override int BlastFrames => 7;

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust.NewDustDirect(area.TopLeft(), area.Width, area.Height, DustID.ShimmerSpark, newColor: ImbueColour, Scale: area.RelativeScale());
			Dust.NewDustDirect(area.TopLeft(), area.Width, area.Height, DustID.ShimmerSpark, newColor: ImbueColour2, Scale: area.RelativeScale());
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust.NewDust(position, 0, 0, DustID.ShimmerSpark, (Main.rand.NextFloat() - 0.5f) * (5f * intensity), (Main.rand.NextFloat() - 0.5f) * (5f * intensity), 0, ImbueColour, intensity);
				Dust.NewDust(position, 0, 0, DustID.ShimmerSpark, (Main.rand.NextFloat() - 0.5f) * (5f * intensity), (Main.rand.NextFloat() - 0.5f) * (5f * intensity), 0, ImbueColour2, intensity);
			}
		}

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			Dust.NewDustDirect(area.TopLeft(), area.Width, area.Height, DustID.ShimmerSpark, direction.X * 2f, direction.Y * 2f, 0, ImbueColour, 2f * area.RelativeScale()).noGravity = true;
			Dust.NewDustDirect(area.TopLeft(), area.Width, area.Height, DustID.ShimmerSpark, direction.X * 2f, direction.Y * 2f, 0, ImbueColour2, 2f * area.RelativeScale()).noGravity = true;
			Dust.NewDustDirect(area.TopLeft(), area.Width, area.Height, DustID.ShimmerSpark, direction.X * 2f, direction.Y * 2f, 0, Colour, 2f * area.RelativeScale()).noGravity = true;
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 5; n++)
			{
				Dust.NewDustDirect(area.TopLeft(), area.Width, area.Height, DustID.ShimmerSpark, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 0, ImbueColour, 2f * area.RelativeScale()).noGravity = true;
				Dust.NewDustDirect(area.TopLeft(), area.Width, area.Height, DustID.ShimmerSpark, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 0, ImbueColour2, 2f * area.RelativeScale()).noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}

		public override void RegisterMutations()
		{

		}
	}
}
