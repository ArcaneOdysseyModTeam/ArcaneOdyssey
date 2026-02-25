using ArcaneOdyssey.Content.Buffs.Stuns;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Normal;
using ArcaneOdyssey.Content.Projectiles.Relics;
using ArcaneOdyssey.VFX.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Imbues.Relics
{
	public class EaglePatrimony : SpiritEnergy
	{
		public override AORarities AORarity => AORarities.Special;
		public override SoundStyle? ImbueSound => SoundID.DD2_LightningBugZap with { Volume = 2.25f };

		public override Combo[] CombinedDebuffs => [new(BuffID.Wet, ModContent.BuffType<Paralyzed>())];

		public override Debuff[] ImbueDebuffs => [new(ModContent.BuffType<Paralyzed>(), 60, 33)];

		

		public override SynergyEffects Effects => AOUtils.CopyDamageSynergiesFromImbue<LightningMagic>();

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = Item.height = 40;
			Item.shoot = ModContent.ProjectileType<Astrapikis>();
			Item.shootSpeed = .9f;
			Item.damage = 20;
			Item.knockBack = 3.75f;
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			base.LingeringEffects(area, direction, source);
			if (Main.GameUpdateCount % 2 == 0)
				Dust.NewDustDirect(area.TopLeft(), area.Width, area.Height, ModContent.DustType<SpiritTentacle>()).noGravity = true;
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			base.KillEffects(area, source);
			for (float i = 0; i < 25; i++)
			{
				var centre = (MathHelper.TwoPi / 25 * i).ToRotationVector2() * 15 * area.RelativeScale();
				if (i % 2 == 0)
					AOUtils.NewDustImperfect(area.Center(), ModContent.DustType<SpiritTentacle>(), centre * area.RelativeScale() / (8 + (Main.rand.NextFloat() * 2)), Scale: .75f * area.RelativeScale()).noGravity = true;
			}
		}

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			base.SpawningEffects(area, direction);
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, ModContent.DustType<SpiritTentacle>(), direction.X * 0.5f, direction.Y * 0.5f, Scale: area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			base.ExplosionEffects(position, intensity);
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, ModContent.DustType<SpiritTentacle>(), (Main.rand.NextFloat() - 0.5f) * (30f * AOScrollSize * intensity), (Main.rand.NextFloat() - 0.5f) * (30f * AOScrollSize * intensity), Scale: intensity)];
				spawnedDust.noGravity = true;
			}
		}
	}
}
