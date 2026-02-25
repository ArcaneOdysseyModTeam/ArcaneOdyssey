using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Buffs.Stuns;
using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Lost
{
	[AutoloadEquip(EquipType.Wings)]
	public class PhoenixMagic : AOMagic
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(180, 8f, 2f, true, 12f, 12f);
		}

		public override void UpdateEquip(Player player)
		{
			base.UpdateEquip(player);
			player.noFallDmg = true;
		}

		

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.accessory = true;
		}

		public static float AscentWhenFalling => 0.75f;
		public static float AscentWhenRising => 0.15f;
		public static float MaxCanAscendMultiplier => 1f;
		public static float MaxAscentMultiplier => 1.805f;
		public static float ConstantAscend => 0.125f;

		public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
		{
			ascentWhenFalling = AscentWhenFalling;
			ascentWhenRising = AscentWhenRising;
			maxCanAscendMultiplier = MaxCanAscendMultiplier;
			maxAscentMultiplier = MaxAscentMultiplier;
			constantAscend = ConstantAscend;

			if (player.TryingToHoverDown && player.controlJump && player.wingTime > 0f && !player.merman)
			{
				player.wingTime += 0.5f;
				player.velocity.Y *= 0.8f;
				if (player.velocity.Y > -2f && player.velocity.Y < 1f)
					player.velocity.Y = 0.00001f;
				ascentWhenFalling *= 0f;
				ascentWhenRising *= 0f;
				constantAscend *= 0f;
			}
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			base.UpdateAccessory(player, hideVisual);
			if (!hideVisual)
			{
				Vector2 spawnPos = player.MountedCenter + new Vector2(-25 * player.direction, 0);
				Lighting.AddLight(spawnPos, ImbueColour.ToVector3() * 1.5f);
			}
		}

		public override bool Special => true;
		public override float DashSpeed => 1.2f; // burst
		public override bool? Cold => false;
		public override bool CanBeWet => false;
		public override SoundStyle? ImbueSound => SoundID.Item20;
		public override Color ImbueColour => new(0, 204, 255); // lerp between yellow and blue later
		public override AOImbuableTier ImbuableTier => AOImbuableTier.Lost;
		public override float AOScrollDamage => .95f;
		public override float AOScrollSpeed => 1.2f;
		public override float AOScrollSize => 1.3f;
		public override Debuff[] ImbueDebuffs => [new(ModContent.BuffType<PhoenixHealing>(), 60 * 10),];

		public override Combo[] CombinedDebuffs => [new(ModContent.BuffType<CharredEffect>(), ModContent.BuffType<Petrified>())];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				ClearBuff.Create<AOBleed>(),
				ClearBuff.Create < FreezingEffect >(),
				ClearBuff.Create < SnowyEffect >(),
				new(BuffID.Wet),
				ClearBuff.Create < CharredEffect >(),
				new(BuffID.Slimed)
			],
			[
				new(ModContent.BuffType<AOBleed>(),1.15f),
				new(ModContent.BuffType<CharredEffect>(),1.01f),
				new(BuffID.Venom,1.05f),
				Synergy.Create<Corroding>(1.05f),
				new(ModContent.BuffType<Crystallized>(),0.85f),
				new(ModContent.BuffType<FreezingEffect>(),0.99f),
				new(ModContent.BuffType<SnowyEffect>(),0.99f),
				new(BuffID.Wet,0.99f),
				new(BuffID.OnFire3,1.05f),
				Synergy.Create<Melting>(1.05f),
				new(BuffID.Poisoned,1.05f),
				Synergy.Create<AOPoisoned>(1.05f),
				new(BuffID.ShadowFlame,1.1f),
				new(BuffID.Slimed,1.075f),
				new(BuffID.Oiled,1.075f),
				new(ModContent.BuffType<SandyEffect>(),0.98f),
				new(ModContent.BuffType<Scalding>(),1.1f),
				new(ModContent.BuffType<SearedEffect>(),1.1f)

			]
			);

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			if (!Main.dedServ)
			{
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.BlueTorch, Scale: 1.5f * area.RelativeScale());
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.YellowTorch, Scale: 1.5f * area.RelativeScale());
			}
		}
		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.BlueTorch, direction.X * 2f, direction.Y * 2f, Scale: 4f * area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.YellowTorch, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 4f * area.RelativeScale())];
				spawnedDust2.noGravity = true;
			}
		}
		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.BlueFairy, (Main.rand.NextFloat() - 0.5f) * (15f * intensity * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * intensity * AOScrollSize), Scale: 2f * intensity)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(position, 0, 0, DustID.YellowStarDust, (Main.rand.NextFloat() - 0.5f) * (15f * intensity * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * intensity * AOScrollSize), Scale: 2f * intensity)];
				spawnedDust2.noGravity = true;
			}
		}
		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.BlueTorch, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 5.5f * area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.YellowTorch, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 5.5f * area.RelativeScale())];
				spawnedDust2.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}
	}
}
