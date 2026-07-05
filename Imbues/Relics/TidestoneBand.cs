using ArcaneOdyssey.AOPlayers;
using ArcaneOdyssey.GodSouls;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Magic.Normal;
using ArcaneOdyssey.Skills.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Imbues.Relics
{
	public class TidestoneBand : SpiritEnergy
	{
		public override float UnstableSpeed => 1.2f;
		public override int UnstableDrawback => 2;

		public override float SynergySize => 1.25f;

		public override float SynergySpeed => .8f;

		public override int Value => 500;
		public override SoundStyle? ImbueSound => SoundID.Splash;

		public override SynergyEffects Effects => AOUtils.CopyDamageSynergiesFromImbue<WaterMagic>();

		public override byte[] SoulSynergies => [AOUtils.GodSoulType<PoseidonSoul>()];
		public override byte[] UnstableSouls => [AOUtils.GodSoulType<AthenaSoul>()];

		public override DashSkill DefaultDash => ModContent.GetInstance<ThakrousiSkill>();

		public override void SetStaticDefaults()
		{ 
			base.SetStaticDefaults();
			ArcaneOdysseyMod.Sets.cold[Type] = true;
		}
		public override Color ImbueColour => new(0, 30, 255);
	}

	public class Thakrousi(Imbuable imbuesource) : ModDash(imbuesource.Item)
	{
		public override DamageClass DamageType => DamageClass.Summon;
		public override bool Immune => true;
		public override float DashSpeed => 120;
		public override int DashMax => 2;
		public override bool LocksPlayer => true;
		public override int Cooldown => (60 * 3 * imbuesource.ScrollSpeed.FlipFloat()).Round();

		public override bool OnHit(Player player, NPC target) => true;

		public override void OnEnd(Player player)
		{
			imbuesource.ActivateAbility(player, false);
			AOUtils.SimulateAOE(150, Damage, player.MountedCenter, Knockback, Source, DamageType);
			player.velocity *= .01f;
			SoundEngine.PlaySound(SoundID.Splash);
			for (int i = 0; i < 20; i++)
			{
				Imbue?.ExplosionEffects(player.MountedCenter, 2f);
				SecondImbue?.ExplosionEffects(player.MountedCenter, 1.5f);
			}
		}

		public override int DisplayedCooldownID => ModContent.BuffType<ThakrousiCooldown>();
	}


	public class ThakrousiCooldown : DisplayedCooldown
	{
		public override string Texture => AOUtils.GetTexture<TidestoneBand>();
	}

	public class ThakrousiSkill : DashSkill
	{
		public override int Damage => 20;

		public override float Knockback => 6.25f;

		public override int Scroll => 0;

		public override void Activate(Player player, Imbuable imbue)
		{
			player.ArcaneOdyssey()?.SetDash(new Thakrousi(imbue));
		}
	}
}
