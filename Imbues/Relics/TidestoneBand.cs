using ArcaneOdyssey.AOPlayers;
using ArcaneOdyssey.GodSouls;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Magic.Normal;
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

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = Item.height = 56;
			Item.damage = 20;
			Item.knockBack = 6.25f;
		}

		public override void SetStaticDefaults() { base.SetStaticDefaults(); ArcaneOdysseyMod.Sets.cold[Type] = true; }
		public override Color ImbueColour => new(0, 30, 255);

		public override void UseAnimation(Player player)
		{
			base.UseAnimation(player);
			if (!player.AltUse())
			{
				if (!player.ArcaneOdyssey().OnCooldown<ThakrousiCooldown>())
				{
					player.ArcaneOdyssey().StartDash(new Thakrousi(this), imbue: this);
				}
			}
		}
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
}
