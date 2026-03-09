using ArcaneOdyssey.Content.Imbues.Magic.Normal;
using ArcaneOdyssey.AOPlayers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Imbues.Relics
{
	public class TidestoneBand : SpiritEnergy
	{
		public override int AOValue => 500;
		public override SoundStyle? ImbueSound => SoundID.Splash;

		public override SynergyEffects Effects => AOUtils.CopyDamageSynergiesFromImbue<WaterMagic>();

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = Item.height = 56;
			Item.damage = (20 * AOImbueDamage).Round();
		}
		public override bool? Cold => true;
		public override Color ImbueColour => Color.Blue;

		public override void UseAnimation(Player player)
		{
			base.UseAnimation(player);
			if (!player.AltUse())
			{
				if (!player.ArcaneOdyssey().OnCooldown<ThakrousiCooldown>())
				{
					player.ArcaneOdyssey().StartDash(new Thakrousi(Item), imbue: this);
				}
			}
		}
	}

	public class Thakrousi(Entity source) : DashSystem(source)
	{
		public override DamageClass DamageType => DamageClass.Summon;
		public override bool Immune => true;
		public override float DashSpeed => 120;
		public override int DashMax => 2;
		public override bool LocksPlayer => true;
		public override int Cooldown => 60 * 3;

		public override bool OnHit(Player player, Entity target) => true;

		public override void OnEnd(Player player)
		{
			AOUtils.SimulateAOE(150, Damage, player.MountedCenter, Knockback, source, DamageType);
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
		public override string ExtraIconTexture => AOUtils.GetTexture<TidestoneBand>();
	}
}
