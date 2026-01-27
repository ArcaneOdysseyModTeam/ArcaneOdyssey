using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
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

		public override SynergyEffects Effects => AOUtils.CopySynergiesFromImbue<WaterMagic>();

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
			for (float i = 0; i < 2; i++)
			{
				Dust.NewDustDirect(area.TopLeft(), area.Width, area.Height, DustID.IcyMerman, direction.GetValueOrDefault().X / 2, direction.GetValueOrDefault().Y / 2, newColor: Color.LightBlue, Scale: area.RelativeScale()).noGravity = true;
			}
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			base.KillEffects(area, source);
			for (float i = 0; i < 25; i++)
			{
				var centre = (MathHelper.TwoPi / 50 * i).ToRotationVector2() * 60 * area.RelativeScale();
				AOUtils.NewDustImperfect(area.Center(), DustID.IcyMerman, centre * area.RelativeScale() / (13 + (Main.rand.NextFloat() * 2)), newColor: Color.LightBlue, Scale: area.RelativeScale()).noGravity = true;
				AOUtils.NewDustImperfect(area.Center(), DustID.IcyMerman, centre * area.RelativeScale() / (14 + (Main.rand.NextFloat() * 2)), newColor: Color.LightBlue, Scale: area.RelativeScale()).noGravity = true;
				AOUtils.NewDustImperfect(area.Center(), DustID.IcyMerman, centre * area.RelativeScale() / (15 + (Main.rand.NextFloat() * 2)), newColor: Color.LightBlue, Scale: area.RelativeScale()).noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			base.SpawningEffects(area, direction);
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.IcyMerman, direction.X * 0.5f, direction.Y * 0.5f, newColor: Color.LightBlue, Scale: area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			base.ExplosionEffects(position, intensity);
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.IcyMerman, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), newColor: Color.LightBlue, Scale: intensity)];
				spawnedDust.noGravity = true;
			}
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
