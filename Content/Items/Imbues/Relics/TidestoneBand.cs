using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Normal;
using ArcaneOdyssey.PlayerClasses;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Imbues.Relics
{
	public class TidestoneBand : RelicImbue
	{
		public override int AOValue => 500;
		public override float AOScrollDamage => 1f;
		public override float AOScrollSize => 1f;
		public override float AOScrollSpeed => 1f;
		public override SoundStyle? ImbueSound => SoundID.Splash;

		public override SynergyEffects Effects => AOUtils.CopyDamageSynergiesFromImbue<WaterMagic>();

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = Item.height = 56;
		}
		public override bool? Cold => true;
		public override Color ImbueColour => new(0, 30, 255);

		public override bool AltFunctionUse(Player player)
		{
			if (!player.ArcaneOdyssey().OnCooldown(ModContent.BuffType<ThakrousiCooldown>()))
			{
				player.ArcaneOdyssey().StartDash(new Thakrousi(Item), imbue: this);
			}
			return true;
		}
		public override WeaponAbility? Ability => new(Mod, "Thakrousi", "Surround yourself in spirit energy and leap forward, then release the energy in a large area", ImbueColour);

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			base.LingeringEffects(area, direction, source);
			base.LingeringEffects(area, direction, source);
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			base.KillEffects(area, source);
			base.KillEffects(area, source);
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			base.SpawningEffects(area, direction);
			base.SpawningEffects(area, direction);
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			base.ExplosionEffects(position, intensity);
			base.ExplosionEffects(position, intensity);
		}
	}

	public class Thakrousi(Entity source) : DashSystem(source)
	{
		public override DamageClass DamageType => OracleDamage.Instance;
		public override bool Immune => true;
		public override float DashSpeed => 120;
		public override int DashMax => 2;
		public override bool AnyDirection => true;
		public override int Cooldown => 60 * 3;

		public override bool OnHit(Player player, Entity target) => true;

		public override void OnEnd(Player player)
		{
			AOUtils.SimulateAOE(150, 20, player.MountedCenter, 4.5f, source, OracleDamage.Instance);
			player.velocity *= .01f;
			SoundEngine.PlaySound(SoundID.Splash);
			for (int i = 0; i < 20; i++)
			{
				Imbue?.ExplosionEffects(player.MountedCenter, 2f);
				SecondImbue?.ExplosionEffects(player.MountedCenter, 2f);
			}
		}

		public override int DisplayedCooldownID => ModContent.BuffType<ThakrousiCooldown>();
	}


	public class ThakrousiCooldown : DisplayedCooldown
	{
		public override string ExtraIconTexture => AOUtils.GetTexture<TidestoneBand>();
	}
}
