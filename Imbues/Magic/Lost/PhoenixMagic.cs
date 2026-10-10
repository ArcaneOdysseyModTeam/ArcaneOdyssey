using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Buffs.Stuns;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Magic.Ancient;
using ArcaneOdyssey.Imbues.Magic.Normal;
using ArcaneOdyssey.Items.Accessories.Helpers;
using ArcaneOdyssey.MagicVariants.Base;
using ArcaneOdyssey.MagicVariants.Phoenix;
using ArcaneOdyssey.Skills.Base;
using Terraria.Audio;

namespace ArcaneOdyssey.Imbues.Magic.Lost
{
	public class PhoenixMagic : MagicType
	{
		public static PhoenixMagic Instance;

		public override void Load()
		{
			base.Load();
			Instance = this;
		}

		public override void Unload()
		{
			base.Unload();
			Instance = null;
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ArcaneOdysseyMod.Sets.cold[Type] = false;
			ImbueID.Sets.Special[ID] = true;
			ImbueID.Sets.Bright[ID] = true;
			ImbueID.Sets.DefaultVariant[ID] = MagicVariant.GetID<WarPhoenixVariant>();
		}

		public override MagicCircleTypes CircleType => MagicCircleTypes.Imperial;

		public static float AscentWhenFalling => 0.75f;
		public static float AscentWhenRising => 0.15f;
		public static float MaxCanAscendMultiplier => 1f;
		public static float MaxAscentMultiplier => 1.805f;
		public static float ConstantAscend => 0.125f;

		public override float DashSpeed => 1.2f; // burst
		public override bool CanBeWet => false;
		public override SoundStyle? ImbueSound => SoundID.Item20;
		public override Color ImbueColour => new(0, 115, 255);
		public override Color? DefaultAnimatedColour => Color.Yellow;

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



		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			if (!Main.dedServ)
			{
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.WhiteTorch, newColor: Colour, Scale: 1.5f * area.RelativeScale());
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.WhiteTorch, newColor: Colour, Scale: 1.5f * area.RelativeScale());
			}
		}
		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.WhiteTorch, direction.X * 2f, direction.Y * 2f, newColor: Colour, Scale: 4f * area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.WhiteTorch, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), newColor: Colour, Scale: 4f * area.RelativeScale())];
				spawnedDust2.noGravity = true;
			}
		}
		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.Snow, (Main.rand.NextFloat() - 0.5f) * (20f * intensity), (Main.rand.NextFloat() - 0.5f) * (20f * intensity), newColor: Colour, Scale: 2f * intensity)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(position, 0, 0, DustID.SilverFlame, (Main.rand.NextFloat() - 0.5f) * (20f * intensity), (Main.rand.NextFloat() - 0.5f) * (20f * intensity), newColor: Colour, Scale: 2f * intensity)];
				spawnedDust2.noGravity = true;
			}
		}
		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.WhiteTorch, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), newColor: Colour, Scale: 5.5f * area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.WhiteTorch, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), newColor: Colour, Scale: 5.5f * area.RelativeScale())];
				spawnedDust2.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}

		public override void RegisterMutations()
		{
			RegisterDefaultMagic<FireMagic>();
			RegisterMutation<InfernoMagic>();
		}

		public override ModSkill DefaultMobility => ModContent.GetInstance<PhoenixFlight>();
	}

	public class PhoenixFlight : ModSkill
	{
		public override SkillType SkillSlot => SkillType.Mobility;

		public override int Scroll => 0;

		public override void Activate(Player player, Imbuable imbue)
		{
			var item = new Item(ModContent.ItemType<PhoenixWings>())
			{
				active = false,
			};
			(item.ModItem as PhoenixWings).Variant = (imbue as PhoenixMagic).magicVariant;
			player.ApplyEquipFunctional(item, false);
		}
	}
}
