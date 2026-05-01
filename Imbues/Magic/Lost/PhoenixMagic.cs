using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Buffs.Stuns;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Magic.Normal;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Imbues.Magic.Lost
{
	[AutoloadEquip(EquipType.Wings)]
	public class PhoenixMagic : MagicType
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

		public override void UpdateVanity(Player player)
		{
			base.UpdateEquip(player);
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
		public override Color ImbueColour => new(0, 115, 255);
		public override Color ImbueColour2 => Color.Yellow;
		public override ColourTransitionStyle TransitionStyle => ColourTransitionStyle.Tangent;
		public override ImbuableTiers ImbuableTier => ImbuableTiers.Lost;
		public override float ScrollDamage => .95f;
		public override float ScrollSpeed => 1.2f;
		public override float ScrollSize => 1.3f;
		public override Debuff[] ImbueDebuffs => [Debuff.Create<PhoenixHealing>(),];

		public override Combo[] CombinedDebuffs => [Combo.Create<CharredEffect, Petrified>()];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				ClearBuff.Create<Bleeding>(),
				ClearBuff.Create<FreezingEffect>(),
				ClearBuff.Create<SnowyEffect>(),
				ClearBuff.Create<Soaked>(),
				ClearBuff.Create<CharredEffect>(),
				ClearBuff.Create<Flammable>()
			],
			[
				Synergy.Create<Bleeding>(1.15f),
				Synergy.Create<CharredEffect>(1.01f),
				Synergy.Create<Corroding>(1.05f),
				Synergy.Create<Crystallized>(0.85f),
				Synergy.Create<FreezingEffect>(0.99f),
				Synergy.Create<SnowyEffect>(0.99f),
				Synergy.Create<Soaked>(0.99f),
				Synergy.Create<Melting>(1.05f),
				Synergy.Create<Poisoned>(1.05f),
				Synergy.Create<Scorched>(1.1f),
				Synergy.Create<Flammable>(1.075f),
				Synergy.Create<SandyEffect>(0.98f),
				Synergy.Create<Scalding>(1.1f),
				Synergy.Create<SearedEffect>(1.1f)

			]
			);

		public override int BlastFrames => 4;

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
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.BlueFairy, (Main.rand.NextFloat() - 0.5f) * (20f * intensity), (Main.rand.NextFloat() - 0.5f) * (20f * intensity), Scale: 2f * intensity)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(position, 0, 0, DustID.YellowStarDust, (Main.rand.NextFloat() - 0.5f) * (20f * intensity), (Main.rand.NextFloat() - 0.5f) * (20f * intensity), Scale: 2f * intensity)];
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

		public override void RegisterMutations()
		{
			RegisterDefaultMagic<FireMagic>();
		}
	}
}
