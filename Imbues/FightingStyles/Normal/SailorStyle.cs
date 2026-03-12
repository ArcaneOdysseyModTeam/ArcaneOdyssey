using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Buffs.Stuns;
using ArcaneOdyssey.Imbues.Base;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Imbues.FightingStyles.Normal
{
	public class SailorStyle : FightingStyleBarred
	{
		public override float Aura => .875f;
		public override float DashSpeed => BarValue > (BarMax / 2) ? 1.2f : 1f; // burst?
		public override bool? Cold => true;
		public override Color ImbueColour => Color.CornflowerBlue;
		public override SoundStyle? ImbueSound => SoundID.Splash;

		public override float BarValueMulti => 1.25f;
		public override float MaxImbueSpeed => 1f;
		public override float MaxImbueDamage => .925f;
		public override float MaxImbueSize => 1.278f;
		public override float MinImbueSpeed => 1f;
		public override float MinImbueDamage => .85f;
		public override float MinImbueSize => .833f;
		public override float MaxScrollSpeed => 1f;
		public override float MaxScrollDamage => .85f;
		public override float MaxScrollSize => 1.2f;
		public override float MinScrollSpeed => 1f;
		public override float MinScrollDamage => .775f;
		public override float MinScrollSize => .8f;
		public override Color DisplayColor => Color.PaleVioletRed;
		public override Debuff[] ImbueDebuffs => [Debuff.Create<Soaked>()];
		public override SynergyEffects Effects => new(
			[
				ClearBuff.Create<SearedEffect>(),
				ClearBuff.Create<CharredEffect>(),
				ClearBuff.Create<AOBurning>(),
				ClearBuff.Create<Melting>(),
				ClearBuff.Create<Corroding>(),
				ClearBuff.Create<Scorched>(),
				ClearBuff.Create<Singed>(),
				ClearBuff.Create<Scalding>(),
				ClearBuff.Create<Petrified>()
			],
			[
				Synergy.Create<Crystallized>(1.1f),
				Synergy.Create<SnowyEffect>(1.1f),
				Synergy.Create<FreezingEffect>(1.075f),
				Synergy.Create<AOBleed>(1.05f),
				Synergy.Create<CharredEffect>(0.9f),
				Synergy.Create<Melting>(.9f),
				Synergy.Create<Singed>(0.8f),
				Synergy.Create<Corroding>(.9f),
				Synergy.Create<SearedEffect>(0.85f),
				Synergy.Create<Scorched>(0.85f),
				Synergy.Create<SandyEffect>(0.8f),
				Synergy.Create<AOBurning>(.8f),
			]
		);

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			BarValue -= BarMax / 100f;
			for (int n = 0; n < (int)Math.Max(Math.Round((float)BarValue / (BarMax / 3)), 1); n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Water, direction.X * 2f, direction.Y * 2f, Scale: LerpValue * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			for (int n = 0; n < (int)Math.Max(Math.Round((float)BarValue / (BarMax / 3 * 2)), 1); n++)
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Water, Scale: LerpValue * 2.2f * area.RelativeScale());
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < (int)Math.Max(Math.Round((float)BarValue / (BarMax / 3)), 1); n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.Water, (Main.rand.NextFloat() - 0.5f) * (35f * AOScrollSize * intensity), (Main.rand.NextFloat() - 0.5f) * (35f * AOScrollSize * intensity), Scale: LerpValue * intensity)];
				spawnedDust.noGravity = true;
			}
		}
		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Water, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: LerpValue * 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}

		public override void AddRecipes()
		{
			var rec = CreateRecipe().AddIngredient<BasicCombat>();
			bool usecoral = true;
			if (ExternalModSupport.HasCalamity)
			{
				usecoral = false;
				rec.AddIngredient(ExternalModSupport.Calamity.Find<ModItem>("SeaRemains"), 5);
			}
			if (ExternalModSupport.HasThorium)
			{
				usecoral = false;
				rec.AddIngredient(ExternalModSupport.Thorium.Find<ModItem>("DepthScale"), 5);
			}
			if (usecoral)
			{
				rec.AddIngredient(ItemID.Coral, 15);
			}
			rec.Register();
		}

		public override void UpdateInventory(Player player)
		{
			if (player.wet && !player.honeyWet && !player.lavaWet)
			{
				BarValue += BarMax / (BarMax * .6f * 2.5f);
			}
			base.UpdateInventory(player);
		}
	}

	public class SailorBars : GlobalItem
	{
		public override void UseAnimation(Item item, Player player)
		{
			if (item.Imbue() is SailorStyle imbue)
			{
				imbue.BarValue -= FightingStyleBarred.BarMax / 100f;
			}
		}

		public override void OnConsumeItem(Item item, Player player)
		{
			if (item.potion)
			{
				if (player.Imbue() is SailorStyle imbue)
				{
					imbue.BarValue = FightingStyleBarred.BarMax;
				}
				if (player.PlayerItem()?.Imbue() is SailorStyle imbue2)
				{
					imbue2.BarValue = FightingStyleBarred.BarMax;
				}

			}
		}
	}
}
