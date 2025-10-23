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
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Imbues
{
	public class SteamImbue : Imbuable
	{
		public static SteamImbue Create(Imbuable imbue)
		{
			SteamImbue steam = (SteamImbue)new Item(ModContent.ItemType<SteamImbue>()).ModItem;
			steam.originalImbue = imbue;
			if (imbue is null)
				steam.originalImbue = (Imbuable)new Item(ModContent.ItemType<WindMagic>()).ModItem;
			return steam;
		}

		public override float AOScrollDamage => .85f;
		public override float AOImbueDamage => .925f;
		public override float AOScrollSize => 1.15f;
		public override float AOImbueSize => 1.1f;
		public override float AOImbueSpeed => 1;
		public override AODebuffRequirement[] ImbueDebuffs => [new(ModContent.BuffType<AOScalding>(), 60*10)];
		public override CombinedDebuff[] CombinedDebuffs => [new(ModContent.BuffType<CharredEffect>(), ModContent.BuffType<AOPetrified>())];

		public Imbuable originalImbue;

		public override Color ImbueColour => Color.LightGray;

		public override AOImbuableTier ImbuableTier => AOImbuableTier.Unobtainable;
		public override SynergyEffects Effects => new([], [
			new(ModContent.BuffType<AOBleed>(), 1.15f),
			new(ModContent.BuffType<AOPetrified>(), 1.1f),
			new(BuffID.OnFire, 1.1f),
			new(ModContent.BuffType<CharredEffect>(), 1.1f),
			new(BuffID.Venom, 1.05f),
			new(ModContent.BuffType<FreezingEffect>(), .9f),
			new(BuffID.Wet, .9f),
			new(ModContent.BuffType<AOFrozen>(), .9f),
			new(ModContent.BuffType<Crystallized>(), .85f),
			new(ModContent.BuffType<SandyEffect>(), .8f),
			]);

		public override void KillEffects(Entity projectile)
		{
			for (int n = 0; n < 30; n++)
			{
				Dust.NewDust(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 0, 0, DustID.Smoke, 5f * Main.rand.NextFloat() - 0.5f, 5f * Main.rand.NextFloat() - 0.5f, 0, default, 3f);
			}
		}
		public override void SpawningEffects(Entity projectile)
		{
			KillEffects(projectile);
			SoundEngine.PlaySound(SoundID.LiquidsWaterLava, projectile.position);
		}
		public override void LingeringEffects(Entity projectile)
		{
			for (int n = 0; n < 2; n++)
				Dust.NewDust(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 1, 1, DustID.Smoke, 0f, 0f, 0, default, 2f);
		}

		public override ModItem Clone(Item newEntity)
		{
			var clone = (SteamImbue)base.Clone(newEntity);
			clone.originalImbue = originalImbue;
			if (originalImbue is null)
				clone.originalImbue = (Imbuable)new Item(ModContent.ItemType<WindMagic>()).ModItem;
			return clone;
		}
	}
}