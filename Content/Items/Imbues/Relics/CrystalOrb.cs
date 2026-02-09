using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Normal;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Items.Imbues.Relics
{
	public class CrystalOrb : SpiritImbue
	{
		public override Color ImbueColour => new(255, 255, 0, 255);
		public override SoundStyle? ImbueSound => SoundID.Item9;
		public override float AOScrollSpeed => 1.2f;
		public override float AOScrollSize => 1f;
		public override float AOScrollDamage => 0.9f;
		public override int AOValue => 700;
		public override WeaponAbility? Ability => new(this, ImbueColour);
		public override SynergyEffects Effects => AOUtils.CopyDamageSynergiesFromImbue<LightMagic>();

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = Item.height = 32;
			Item.noUseGraphic = false;
			Item.holdStyle = ItemHoldStyleID.HoldGolfClub;
			Item.scale = .5f;
			Item.useStyle = ItemUseStyleID.Swing;
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			base.LingeringEffects(area, direction, source);
			Dust.NewDustDirect(area.TopLeft(), area.Width, area.Height, DustID.YellowStarDust, direction.GetValueOrDefault().X / 2, direction.GetValueOrDefault().Y / 2, Scale: area.RelativeScale()).noGravity = true;
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			base.KillEffects(area, source);
			var amount = 12 * 2;
			for (float i = 0; i < amount; i++)
			{
				var centre = (MathHelper.TwoPi / amount * i).ToRotationVector2() * 20 * area.RelativeScale();
				AOUtils.NewDustImperfect(area.Center(), DustID.YellowStarDust, centre * area.RelativeScale() / (13 + (Main.rand.NextFloat() * 2)), Scale: area.RelativeScale()).noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			base.SpawningEffects(area, direction);
			for (int n = 0; n < 2; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.YellowStarDust, direction.X * 0.5f, direction.Y * 0.5f, Scale: area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			base.ExplosionEffects(position, intensity);
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.YellowStarDust, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), Scale: intensity)];
				spawnedDust.noGravity = true;
			}
		}
	}
}
