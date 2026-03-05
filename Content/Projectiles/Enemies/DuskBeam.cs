using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Imbues.Relics;
using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
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
			Projectile.extraUpdates = 4;
			Projectile.timeLeft = 90;
			Projectile.hostile = true;
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			target.AddBuff(ModContent.BuffType<DrainedEffect>(), 60 * 5);
			Imbue?.KillEffects(Projectile.Hitbox, Projectile);
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

			if (Projectile.velocity.Length() < 3f)
			{
				foreach (var player in Main.ActivePlayers)
				{
					if (player.Hitbox.Distance(Projectile.Center) < 500)
					{
						Projectile.velocity = Projectile.SafeDirectionTo(player.Center, Vector2.UnitX) * 7f;
					}
				}
			}
		}

		public override bool PreDraw(ref Color lightColor) => false;

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width /= 4;
			height /= 4;
			fallThrough = true;
			return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}
	}
}
