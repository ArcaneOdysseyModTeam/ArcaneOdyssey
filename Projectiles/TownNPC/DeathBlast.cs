using ArcaneOdyssey.Imbues;
using ArcaneOdyssey.Imbues.Magic.Ancient;
using ArcaneOdyssey.Projectiles.Base;

namespace ArcaneOdyssey.Projectiles.TownNPC
{
	public class DeathBlast : BaseProjectile
	{
		public override string Texture => AOUtils.BlankTexture;
		public override Texture2D Sprite => ImbueID.Sets.Assets.blasts[AOUtils.ImbuableID<DeathMagic>()].Value;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			Main.projFrames[Type] = Imbue.BlastFrames;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = Projectile.width = 64;
			Projectile.timeLeft = 60;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.friendly = true;
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = Projectile.width / 4;
			height = Projectile.height / 4;
			fallThrough = true;
			return true;
		}

		public static DeathMagic Imbue => ModContent.GetInstance<DeathMagic>();

		public override void AI()
		{
			base.AI();
			Imbue?.LingeringEffects(Projectile.Hitbox, Projectile.velocity, Projectile);

			Imbue?.UpdateProjectile(Projectile);

			if (Projectile.frameCounter++ > 5)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= Main.projFrames[Type])
				{
					Projectile.frame = 0;
				}
			}
		}
	}
}
