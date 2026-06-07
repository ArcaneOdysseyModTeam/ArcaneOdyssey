using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Projectiles.Magic.Effects;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Imbues.Gimmicks
{
	public class AshClouds : ImbueGimmick
	{
		public override void KillEffects(Projectile projectile)
		{
			var area = projectile.Hitbox;
			var proj = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), new(area.X + area.Width * Main.rand.NextFloat(), area.Y + area.Height * Main.rand.NextFloat()), Vector2.Zero, ModContent.ProjectileType<AshCloud>(), (int)MathF.Ceiling(projectile.damage / 5f), 0f);
			proj.scale *= projectile.Hitbox.RelativeScale(max: 2f);
			proj.Hitbox = proj.Hitbox.Scaled(projectile.Hitbox.RelativeScale(max: 2f));
			proj.netUpdate = true;
		}
	}
}
