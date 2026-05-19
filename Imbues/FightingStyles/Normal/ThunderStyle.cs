using ArcaneOdyssey.AOPlayers;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Magic.Normal;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Imbues.FightingStyles.Normal
{
	public class ThunderStyle : FightingStyleBarred
	{
		public override bool SaveBar => true;
		public override float MaxImbueSpeed => 1.3f;
		public override float MaxImbueDamage => .85f;
		public override float MaxImbueSize => .833f;

		public override float MinImbueSpeed => 1f;
		public override float MinImbueDamage => .85f;
		public override float MinImbueSize => .833f;

		public override float MaxScrollSpeed => 1.3f;
		public override float MaxScrollDamage => .75f;
		public override float MaxScrollSize => .8f;

		public override float MinScrollSpeed => 1f;
		public override float MinScrollDamage => .75f;
		public override float MinScrollSize => .8f;

		public override Color DisplayColor => Color.OrangeRed;

		public override float BarValueMulti => 1f;

		public override Color ImbueColour => new(255, 140, 255);

		public override float DashSpeed => BarValue > (BarMax / 2) ? 1.4f : 1f; // instant?

		public override SynergyEffects Effects => AOUtils.CopyDamageSynergiesFromImbue<LightningMagic>();

		public override Combo[] CombinedDebuffs => AOUtils.CopyCombosFromImbue<LightningMagic>();

		public override SoundStyle? ImbueSound => SoundID.DD2_LightningBugZap with { Volume = 2.25f };

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.WitherLightning, direction.X * 0.2f, direction.Y * 0.2f, Scale: 1.2f * area.RelativeScale());
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			var updates = (float)Main.GameUpdateCount;
			if (source is Projectile projectile && projectile.extraUpdates > 0)
			{
				updates += projectile.numUpdates;
			}
			float waveVal = 10f * MathF.Abs(updates % 5f % 10f - 2.5f) - 12.5f;
			Vector2 baseVec = new(0f, waveVal);
			Dust spawnedDust = Dust.NewDustPerfect(area.Center() + baseVec.RotatedBy(direction.GetValueOrDefault(Vector2.One).ToRotation()), DustID.CrystalPulse, Vector2.Zero, Scale: 1.2f);
			spawnedDust.noGravity = true;

			Lighting.AddLight(area.Center(), 2, 1, 2);
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.WitherLightning, Scale: 0.4f * area.RelativeScale());
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust.NewDustDirect(position, 0, 0, DustID.WitherLightning, (Main.rand.NextFloat() - 0.5f) * (15f * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * intensity), Scale: 1.2f * intensity).noGravity = true;
			}
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.WitherLightning, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 1.2f * area.RelativeScale());
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}
	}

	public class ThunderBars : GlobalItem
	{
		public override void UseAnimation(Item item, Player player)
		{
			if (item.Imbue() is ThunderStyle thunder)
			{
				thunder.BarValue -= FightingStyleBarred.BarMax / 20f;
				player.ArcaneOdyssey().SetCooldown(new Cooldown(thunder.Name, thunder.DisplayName, 60));
			}
		}
	}

	public class ThunderGrow : ModPlayer
	{
		public const float BarMax = FightingStyleBarred.BarMax;
		public const float BarMin = FightingStyleBarred.BarMin;
		public bool resetBar = false;

		public override void PostUpdateRunSpeeds()
		{
			if ((Player.controlLeft || Player.controlRight) && Player.ArcaneOdyssey().grounded && !Player.CCed)
			{
				if (Player.HasTypeInInventory<ThunderStyle>(out var thunder))
					if (!Player.ArcaneOdyssey().OnCooldown(thunder.Name))
						thunder.BarValue += BarMax / (BarMax * .6f * (BarMax / 10f));
			}
		}
	}
}
