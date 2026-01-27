using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Normal;
using ArcaneOdyssey.Content.Projectiles.Relics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Imbues.Relics
{
	public class NyxStaff : RelicImbue
	{
		public override int AOValue => 700;
		public override SoundStyle? ImbueSound => SoundID.Item8;
		public override Color ImbueColour => Color.Purple;
		public override float AOScrollDamage => .9f;
		public override float AOScrollSize => 1.1f;
		public override float AOScrollSpeed => 1.1f;

		public override SynergyEffects Effects => CopyDamageSynergiesFromImbue<ShadowMagic>();

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			Item.staff[Type] = true;
			ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.width = Item.height = 46;
			Item.shoot = ModContent.ProjectileType<Nichtetheis>();
			Item.noUseGraphic = false;
			Item.damage = (30 * AOScrollDamage).Round();
			Item.shootSpeed = 7f * AOScrollSpeed;
		}

		public override WeaponAbility? Ability => new(Mod, "Nichtetheis", "Fire a beam of spirit energy that disorients anyone it hits", ImbueColour);

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			base.LingeringEffects(area, direction, source);
			for (float i = 0; i < 3; i++)
			{
				Dust.NewDustDirect(area.TopLeft(), area.Width, area.Height, DustID.IcyMerman, direction.GetValueOrDefault().X / 2, direction.GetValueOrDefault().Y / 2, newColor: Color.Purple, Scale: area.RelativeScale()).noGravity = true;
			}
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			base.KillEffects(area, source);
			for (float i = 0; i < 40; i++)
			{
				var centre = (MathHelper.TwoPi / 40 * i).ToRotationVector2() * 60 * area.RelativeScale();
				NewDustImperfect(area.Center(), DustID.IcyMerman, centre * area.RelativeScale() / (13 + (Main.rand.NextFloat() * 2)), newColor: Color.Purple, Scale: area.RelativeScale()).noGravity = true;
				NewDustImperfect(area.Center(), DustID.IcyMerman, centre * area.RelativeScale() / (14 + (Main.rand.NextFloat() * 2)), newColor: Color.Purple, Scale: area.RelativeScale()).noGravity = true;
				NewDustImperfect(area.Center(), DustID.IcyMerman, centre * area.RelativeScale() / (15 + (Main.rand.NextFloat() * 2)), newColor: Color.Purple, Scale: area.RelativeScale()).noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			base.SpawningEffects(area, direction);
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.IcyMerman, direction.X * 0.5f, direction.Y * 0.5f, newColor: Color.Purple, Scale: area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			base.ExplosionEffects(position, intensity);
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.IcyMerman, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), newColor: Color.Purple, Scale: intensity)];
				spawnedDust.noGravity = true;
			}
		}
	}
}
