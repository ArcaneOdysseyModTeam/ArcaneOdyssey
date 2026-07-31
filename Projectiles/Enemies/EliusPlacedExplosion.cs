using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Relics;
using ArcaneOdyssey.Projectiles.Base;
using ArcaneOdyssey.Projectiles.Relics;
using Terraria.Audio;
using Terraria.DataStructures;

namespace ArcaneOdyssey.Projectiles.Enemies
{
	public class EliusPlacedExplosion : BaseProjectile
	{
		public override string Texture => AOUtils.BlankTexture;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.timeLeft = 200;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.penetrate = -1;
			Projectile.height = Projectile.width = 1;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
		}
		public override void AI()
		{
			Dust.NewDustDirect(Projectile.Center-new Vector2(25,25),50,50,DustID.WitherLightning,0f,-0.1f,0,default,(200f-(float)Projectile.timeLeft)*0.005f).noGravity = true;
		}
		public override void OnKill(int timeLeft)
		{
			Projectile.NewProjectile(Projectile.GetSource_FromThis(),Projectile.Center+new Vector2(0,-1000),Vector2.Zero,ModContent.ProjectileType<EliusTrail>(),0,0f,-1,Projectile.Center.X,Projectile.Center.Y);
			Projectile.NewProjectile(Projectile.GetSource_FromThis(),Projectile.Center,Vector2.Zero,ModContent.ProjectileType<EliusExplosion>(),50,8f,-1);
			SoundEngine.PlaySound(SoundID.Thunder,Projectile.Center);
			base.OnKill(timeLeft);
		}
	}
}
