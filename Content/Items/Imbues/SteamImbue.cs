using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Buffs.Stuns;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Normal;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Imbues
{
	public class SteamImbue : Imbuable
	{
		public static SteamImbue Create(Imbuable imbue)
		{
			SteamImbue steam = (SteamImbue)new Item(ModContent.ItemType<SteamImbue>()).ModItem;
			steam.Imbue = imbue;
			if (imbue is null)
				steam.Imbue = (Imbuable)new Item(ModContent.ItemType<WindMagic>()).ModItem;
			return steam;
		}

		public override float AOScrollDamage => .85f;
		public override float AOImbueDamage => .925f;
		public override float AOScrollSize => 1.15f;
		public override float AOImbueSize => 1.1f;
		public override float AOImbueSpeed => 1;
		public override Debuff[] ImbueDebuffs => [Debuff.Create<Scalding>()];
		public override Combo[] CombinedDebuffs => [Combo.Create<CharredEffect, Petrified>()];

		public override Color ImbueColour => Color.LightGray;
		public override SoundStyle? ImbueSound => SoundID.LiquidsWaterLava;

		public override AOImbuableTier ImbuableTier => AOImbuableTier.Developer;

		public override SynergyEffects Effects => new([], 
			[
				Synergy.Create<AOBleed>(1.15f),
				Synergy.Create<Petrified>(1.1f),
				Synergy.Create<AOBurning>(1.1f),
				Synergy.Create<CharredEffect>(1.1f),
				Synergy.Create<Corroding>(1.05f),
				Synergy.Create<FreezingEffect>(.9f),
				Synergy.Create<Soaked>(.9f),
				Synergy.Create<AOFrozen>(.9f),
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

		public override ModItem Clone(Item newEntity)
		{
			var clone = (SteamImbue)base.Clone(newEntity);
			Imbue ??= (Imbuable)new Item(ModContent.ItemType<WindMagic>()).ModItem;
			clone.Imbue = Imbue;
			return clone;
		}
	}
}