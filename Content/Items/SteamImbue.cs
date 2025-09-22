using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.Stuns;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ID;
using static ArcaneOdyssey.AOUtils;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Projectiles.Base;
using Terraria;
using Terraria.Audio;
using ArcaneOdyssey.Content.Items.Base;

namespace ArcaneOdyssey.Content.Items
{
	public class SteamImbue : Imbuable
	{
		public override float AOScrollDamage => .85f;
		public override float AOImbueDamage => .925f;
		public override float AOScrollSize => 1.15f;
		public override float AOImbueSize => 1.1f;
		public override float AOImbueSpeed => 1;
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

		public override void KillEffects(Projectile projectile)
		{
			for (int n = 0; n < 30; n++)
			{
				Dust.NewDust(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 0, 0, DustID.Smoke, 5f * Main.rand.NextFloat() - 0.5f, 5f * Main.rand.NextFloat() - 0.5f, 0, default, 3f);
			}
		}
		public override void SpawningEffects(Projectile projectile)
		{
			KillEffects(projectile);
			SoundEngine.PlaySound(SoundID.LiquidsWaterLava, projectile.position);
		}
		public override void LingeringEffects(Projectile projectile)
		{
			for (int n = 0; n < 2; n++)
				Dust.NewDust(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 1, 1, DustID.Smoke, 0f, 0f, 0, default, 2f);
		}

		public override ModItem Clone(Item newEntity)
		{
			var clone = (SteamImbue)base.Clone(newEntity);
			clone.originalImbue = originalImbue;
			return clone;
		}
	}
}