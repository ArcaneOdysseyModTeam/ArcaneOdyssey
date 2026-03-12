using Terraria;

namespace ArcaneOdyssey.Projectiles.Relics.Minions
{
	public class MinionBlast : SpiritBlast
	{
		public override string Texture => AOUtils.GetTexture<SpiritBlast>();

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.tileCollide = false;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) { } // so it doesnt change minion target
	}
}
