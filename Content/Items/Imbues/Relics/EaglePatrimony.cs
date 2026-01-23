using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Buffs.Stuns;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Relics;
using ArcaneOdyssey.VFX.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Imbues.Relics
{
	public class EaglePatrimony : RelicImbue
	{
		public override Color ImbueColour => new(0, 183, 255);
		public override AORarities AORarity => AORarities.Special;
		public override SoundStyle? ImbueSound => SoundID.DD2_LightningBugZap with { Volume = 2.25f };

		public override CombinedDebuff[] CombinedDebuffs => [new(BuffID.Wet, ModContent.BuffType<AOParalyzed>())];

		public override WeaponAbility? Ability => new(Mod, "Astrapikis", "Release a slash of spirit energy", ImbueColour);

		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				//ModContent.BuffType<AOPetrified>(), // petrified
				//ModContent.BuffType<CharredEffect>(),
				//ModContent.BuffType<SandyEffect>(),
				//ModContent.BuffType<AOBleed>(),
				//ModContent.BuffType<AOFrozen>()
			],
			[
				new(BuffID.Chilled, 1.2f), // frozen
				new(ModContent.BuffType<AOBleed>(), 1.2f), // bleeding
				new(BuffID.Burning, 1.15f), // scalding
				new(BuffID.OnFire3, 1.075f), // melting/hellfire
				new(BuffID.Venom, 1.075f), // venom acid
				new(BuffID.Wet, 1.05f), // 
				new(BuffID.ShadowFlame,1.15f),
				new(BuffID.Oiled,0.96f),
				new(ModContent.BuffType<Crystallized>(),1.075f),
				new(ModContent.BuffType<SearedEffect>(),1.15f)
			]
			);

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = Item.height = 40;
			Item.shoot = ModContent.ProjectileType<Astrapikis>();
			Item.shootSpeed = 1f;
			Item.damage = 20;
			Item.knockBack = 3.75f;
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			for (float i = 0; i < 5; i++)
			{
				Dust.NewDustDirect(area.TopLeft(), area.Width, area.Height, DustID.IcyMerman, direction.GetValueOrDefault().X / 2, direction.GetValueOrDefault().Y / 2, Scale: area.RelativeScale()).noGravity = true;
			}
			if (Main.GameUpdateCount % 2 == 0)
				Dust.NewDustDirect(area.TopLeft(), area.Width, area.Height, ModContent.DustType<SpiritTentacle>()).noGravity = true;
		}

		public const int DustCount = 50;

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (float i = 0; i < DustCount; i++)
			{
				var centre = (MathHelper.TwoPi / DustCount * i).ToRotationVector2() * ((area.Width + area.Height) / 2);
				if (i % 2 == 0)
					Dust.NewDustPerfect(area.Center(), ModContent.DustType<SpiritTentacle>(), centre * area.RelativeScale() / (8 + (Main.rand.NextFloat() * 2)), Scale: area.RelativeScale()).noGravity = true;
				Dust.NewDustPerfect(area.Center(), DustID.IcyMerman, centre * area.RelativeScale() / (13 + (Main.rand.NextFloat() * 2)), Scale: area.RelativeScale()).noGravity = true;
				Dust.NewDustPerfect(area.Center(), DustID.IcyMerman, centre * area.RelativeScale() / (14 + (Main.rand.NextFloat() * 2)), Scale: area.RelativeScale()).noGravity = true;
				Dust.NewDustPerfect(area.Center(), DustID.IcyMerman, centre * area.RelativeScale() / (15 + (Main.rand.NextFloat() * 2)), Scale: area.RelativeScale()).noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, ModContent.DustType<SpiritTentacle>(), direction.X * 0.5f, direction.Y * 0.5f, Scale: area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, ModContent.DustType<SpiritTentacle>(), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), Scale: intensity)];
				spawnedDust.noGravity = true;
			}
		}
	}
}
