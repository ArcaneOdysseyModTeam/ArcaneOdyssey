using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Magic.Ancient;
using ArcaneOdyssey.Imbues.Magic.Normal;
using ArcaneOdyssey.MagicVariants.Base;
using ArcaneOdyssey.MagicVariants.Lost.Darkness;
using Terraria.Audio;

namespace ArcaneOdyssey.Imbues.Magic.Lost
{
	public class DarknessMagic : MagicType
	{
		public override void RegisterMutations()
		{
			RegisterMutation<DeathMagic>();
			RegisterDefaultMagic<ShadowMagic>();
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ImbueID.Sets.AuraPower[ID] = 1f;
			ImbueID.Sets.Bright[ID] = true; // how ironic
			ImbueID.Sets.DefaultVariant[ID] = MagicVariant.GetID<DevouringVariant>();
		}

		public override ImbuableTiers ImbuableTier => ImbuableTiers.Lost;
		public override float DashSpeed => 1.2f; // burst
		public override SoundStyle? ImbueSound => SoundID.Item8;
		public override Color ImbueColour => Color.Black;
		public override Color? DefaultAnimatedColour => Color.DarkRed;
		
		public override float ScrollSpeed => 1.2f;
		public override float ScrollSize => 1.3f;
		public override float ScrollDamage => .85f;
		public override Debuff[] ImbueDebuffs => [Debuff.Create<DrainedEffect>((60 * 7.5f).Round())];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
			
			],
			[
				Synergy.Create<Crystallized>(0.7f),
				Synergy.Create<BlindedEffect>(0.7f),
			]
			);

		public override MagicCircleTypes CircleType => MagicCircleTypes.Singularity;

		

		protected override void BaseSpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 2; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Wraith, direction.X * 2f, direction.Y * 2f, Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
		}

		protected override void BaseLingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			if (Main.GameUpdateCount % 2 == 0)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Wraith, Scale: 2f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
		}

		protected override void BaseExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 2; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.Wraith, (Main.rand.NextFloat() - 0.5f) * (15f * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * intensity), Scale: 3f * intensity)];
				spawnedDust.noGravity = true;
			}
		}

		protected override void BaseKillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 6; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Wraith, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}
	}
}
