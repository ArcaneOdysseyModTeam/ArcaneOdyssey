using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Buffs.Stuns;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Magic.Developer;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Imbues
{
	public class SteamImbue : Imbuable
	{
		public static SteamImbue Create(Imbuable imbue)
		{
			SteamImbue steam = ModContent.GetInstance<SteamImbue>();
			steam.Imbue = imbue;
			if (imbue is null)
				steam.Imbue = ModContent.GetInstance<JerminusMagic>();
			return steam;
		}

		public override Imbuable Imbue { get => Item.ArcaneOdyssey()?.Imbue ?? ModContent.GetInstance<JerminusMagic>(); set => Item.ArcaneOdyssey().Imbue = value; }

		public override float ScrollDamage => .85f;
		public override float ImbueDamage => .925f;
		public override float ScrollSize => 1.15f;
		public override float ImbueSize => 1.1f;
		public override float ImbueSpeed => 1;
		public override Debuff[] ImbueDebuffs => [Debuff.Create<Scalding>()];
		public override Combo[] CombinedDebuffs => [Combo.Create<CharredEffect, Petrified>()];

		public override Color ImbueColour => Color.LightGray;
		public override SoundStyle? ImbueSound => SoundID.LiquidsWaterLava;

		public override ImbuableTiers ImbuableTier => ImbuableTiers.Developer;

		public override SynergyEffects Effects => new([],
			[
				Synergy.Create<Bleeding>(1.15f),
				Synergy.Create<Petrified>(1.1f),
				Synergy.Create<Burning>(1.1f),
				Synergy.Create<CharredEffect>(1.1f),
				Synergy.Create<Corroding>(1.05f),
				Synergy.Create<FreezingEffect>(.9f),
				Synergy.Create<Soaked>(.9f),
				Synergy.Create<Frozen>(.9f),
				Synergy.Create<Crystallized>(.85f),
				Synergy.Create<SandyEffect>(.8f),
			]
		);



		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 30; n++)
			{
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Smoke, 5f * area.RelativeScale() * Main.rand.NextFloat() - 0.5f, 5f * area.RelativeScale() * Main.rand.NextFloat() - 0.5f, (255 * .75f).Round(), default, 3f * area.RelativeScale());
			}
		}

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 30; n++)
			{
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Smoke, 5f * area.RelativeScale() * Main.rand.NextFloat() - 0.5f, 5f * area.RelativeScale() * Main.rand.NextFloat() - 0.5f, (255 * .75f).Round(), default, 3f * area.RelativeScale());
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			for (int n = 0; n < 2; n++)
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Smoke, 0f, 0f, (255 * .75f).Round(), default, 2f * area.RelativeScale());
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ItemID.Sets.ItemNoGravity[Type] = true;

			ItemID.Sets.ItemIconPulse[Type] = ArcaneOdysseyClientConfig.Instance.PulsingImbueIcons;
			ArcaneOdysseyMod.Sets.toggleablePulse.Add(Type);
		}
	}
}