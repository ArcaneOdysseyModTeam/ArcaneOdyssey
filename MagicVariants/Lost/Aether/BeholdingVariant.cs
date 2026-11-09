using ArcaneOdyssey.Imbues.Magic.Lost;
using ArcaneOdyssey.MagicVariants.Base;
using ArcaneOdyssey.Projectiles.Magic.Effects;
using Terraria.Audio;

namespace ArcaneOdyssey.MagicVariants.Lost.Aether
{
	public class BeholdingVariant : MagicVariant<AetherMagic>
	{
		public override Color Colour => Color.BlueViolet;
		public override Color? Colour2 => Color.Purple;

		public override bool SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.DungeonSpirit, direction.X * 0.2f, direction.Y * 0.2f, Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.DemonTorch, direction.X * 0.2f, direction.Y * 0.2f, Scale: 3f * area.RelativeScale())];
				spawnedDust2.noGravity = true;
			}
			return false;
		}

		public override bool LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.DungeonSpirit, Scale: area.RelativeScale());
			Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.DemonTorch, Scale: 2f * area.RelativeScale())];
			spawnedDust.noGravity = true;
			Lighting.AddLight(area.Center(), 2, 2, 0);
			return false;
		}

		public override bool ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			if (Main.projectile.Find(e => e.active && (e.type == ModContent.ProjectileType<AetherExplosion>()) && (e.Center.ToTileCoordinates16() == position.ToTileCoordinates16()) && (e.ai[0] != 0)) is null)
				Projectile.NewProjectile(ImbueInstance.Item.GetSource_FromThis(), position, Vector2.Zero, ModContent.ProjectileType<AetherExplosion>(), 0, 0, ai0: 1.6f * intensity);
			return false;
		}

		public override bool KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.DungeonSpirit, 28f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 28f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.DemonTorch, 28f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 28f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale())];
				spawnedDust2.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueInstance.ImbueSound, area.Center());
			return false;
		}
	}
}
