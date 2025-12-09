using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Buffs.Stuns;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Relics;
using ArcaneOdyssey.VFX.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Relics
{
	public class EaglePatrimony : RelicWeapon
	{
		public override Color ImbueColour => Color.LightBlue;
		public override AORarities AORarity => AORarities.Special;
		public override CombinedDebuff[] CombinedDebuffs => [new(BuffID.Wet, ModContent.BuffType<AOParalyzed>())];

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
			ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = Item.height = 40;
			Item.shoot = ModContent.ProjectileType<SpiritBlast>();
			Item.shootSpeed = 15;
			Item.UseSound = SoundID.Item84 with { Pitch = .75f };
			Item.damage = 13;
			Item.useAnimation = Item.useTime = 30;
			Item.knockBack = 3.75f;
		}

		public override void LingeringEffects(Entity entity)
		{
			Dust.NewDustDirect(entity.position, entity.width, entity.height, ModContent.DustType<SpiritTentacle>()).noGravity = true;
		}

		public override void KillEffects(Entity entity)
		{
			if (!Main.dedServ)
			{
				for (float i = 0; i < SpiritBlast.DustCount; i++)
				{
					var centre = (MathHelper.TwoPi / SpiritBlast.DustCount * i).ToRotationVector2() * (entity.width + entity.height);
					var dust = Dust.NewDustPerfect(entity.Center, ModContent.DustType<SpiritTentacle>(), centre / (13 + (Main.rand.NextFloat() * 2)));
					dust.noGravity = true;
					centre = (MathHelper.TwoPi / SpiritBlast.DustCount * i).ToRotationVector2() * (entity.width + entity.height);
					dust = Dust.NewDustPerfect(entity.Center, ModContent.DustType<SpiritTentacle>(), centre / (14 + (Main.rand.NextFloat() * 2)));
					dust.noGravity = true;
					centre = (MathHelper.TwoPi / SpiritBlast.DustCount * i).ToRotationVector2() * (entity.width + entity.height);
					dust = Dust.NewDustPerfect(entity.Center, ModContent.DustType<SpiritTentacle>(), centre / (15 + (Main.rand.NextFloat() * 2)));
					dust.noGravity = true;
				}
			}
		}

		public override void SpawningEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, ModContent.DustType<SpiritTentacle>(), projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f)];
				spawnedDust.noGravity = true;
			}
		}

		public override void ExplosionEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + projectile.width / 2f, projectile.position.Y + projectile.height / 2f), 0, 0, ModContent.DustType<SpiritTentacle>(), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize))];
				spawnedDust.noGravity = true;
			}
		}
	}
}
