using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Imbues.Relics;
using ArcaneOdyssey.Content.NPCS.Minibosses;
using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Relics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Enemies
{
	public class DuskBeam : AOBaseProjectile
	{
		public override string Texture => AOUtils.BlankTexture;


		public Imbuable Imbue = ModContent.GetInstance<NyxStaff>();
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.DamageType = DamageClass.Summon;
			Projectile.height = Projectile.width = 40; // hitscan
			Projectile.extraUpdates = 100;
			Projectile.timeLeft = 60;
			Projectile.hostile = true;
		}

		public override void AI()
		{
			Imbue.LingeringEffects(Projectile.Hitbox, Projectile.velocity, Projectile);
			if (Projectile.ai[0] == 0)
			{
				Projectile.ai[0] = 1;
				SoundEngine.PlaySound(Imbue?.ImbueSound, Projectile.Center);
				if (Projectile.owner == Main.myPlayer)
				{
					Projectile.netUpdate = true;
					Projectile.netSpam = 0; ;
				}
			}

			foreach (var player in Main.ActivePlayers)
			{
				if (player.Hitbox.Distance(Projectile.Center) < 500)
				{
					Projectile.velocity = Projectile.SafeDirectionTo(player.Center, Vector2.UnitX);
				}
			}
		}

		public override bool PreDraw(ref Color lightColor) => false;
	}
}
