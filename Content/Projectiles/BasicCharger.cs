using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Projectiles
{
	public class BasicCharger : AOPlayerProjectile
	{
		public override string Texture => Mod.Name + "/Backgrounds/Blank";
		public int ChargingProjectile;
		public float charge = 1f;

		public override bool? CanDamage() => false;

		public override void SetDefaults()
		{
			//Projectile.scale = AOSize;
			Projectile.height = Projectile.width = 20;
			Projectile.tileCollide = false;
			charge = 1f;
			MarkedForDeath = false;
		}

		internal bool MarkedForDeath = false;
		internal bool originallyAltFire = false;

		public override void AI()
		{
			//Projectile.scale = AOSize * charge * Imbue.AOScrollSize;
			var dir = Owner.MountedCenter.DirectionTo(Main.MouseWorld);
			Owner.ChangeDir((dir.X > 0f).ToDirectionInt());
			if (Projectile.ai[0] == 0)
			{
				Projectile.ai[0] = 1;
				Projectile.netUpdate = true;
			}

			Imbue?.LingeringEffects(Projectile.Hitbox);
			SecondImbue?.LingeringEffects(Projectile.Hitbox);


			if (Projectile.position != Projectile.oldPosition)
			{
				Projectile.netUpdate = true;
			}

			if (Owner.channel && !MarkedForDeath)
			{
				AOPlayerOwner.chargingSpell = true;
				Owner.heldProj = Projectile.whoAmI;
				Owner.itemAnimation = Owner.PlayerItem().useAnimation;
				Owner.itemTime = Owner.PlayerItem().useTime;
				Owner.itemRotation = dir.ToRotation();
				if (Owner.direction != 1)
				{
					Owner.itemRotation += MathHelper.Pi;
				}
				if (Main.myPlayer == Projectile.owner)
					charge += 1f / 120f;
				Projectile.Center = Owner.HandPosition.GetValueOrDefault(Owner.MountedCenter + (dir * 10f));
				if (charge >= 1.5f)
				{
					Owner.channel = false;
					MarkedForDeath = true;
				}
			}
			else
			{
				Projectile.alpha += (255f / 60f).Round();
				if (Projectile.ai[1] == 0 && Main.myPlayer == Projectile.owner && ChargingProjectile != 0)
				{
					var proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, dir * 10 * Imbue.AOScrollSpeed, ChargingProjectile, (Projectile.damage * charge).Round(), Projectile.knockBack * charge, Projectile.owner);
					if (proj.ModProjectile is PulsarSpell && originallyAltFire)
					{
						proj.ai[1] = 1;
					}
					proj.netUpdate = true;
					Projectile.ai[1] = 1;
				}
				Kill();
			}

			if (Projectile.frameCounter++ > 5)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= Main.projFrames[Projectile.type])
				{
					Projectile.frame = 0;
				}
			}
		}

		public override bool PreDraw(ref Color lightColor) => false;
	}
}
