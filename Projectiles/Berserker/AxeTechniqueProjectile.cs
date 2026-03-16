using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Projectiles.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Achievements;
using Terraria.ID;

namespace ArcaneOdyssey.Projectiles.Berserker
{
	public class AxeTechniqueProjectile : StrengthTechnique
	{
		public override string Texture => AOUtils.SlashTexture;

		public override float AOSize => .5f;

		public override Debuff? ProjectileDebuff => Debuff.Create<AOBleed>(60 * 5);

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ProjectileID.Sets.TrailingMode[Type] = 0;
		}

		public bool CanCutTrees = !Main.mouseRight;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = Projectile.height = 75;
			Projectile.timeLeft = 30;
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = height /= 3;
			fallThrough = true;
			return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}

		public override void AI()
		{
			if (Projectile.ai[0] == 0)
			{
				Projectile.ai[0] = 1;
				if (Main.myPlayer == Projectile.owner)
				{
					Projectile.netUpdate = true;
					Projectile.netSpam = 0;
				}
			}

			Projectile.rotation = Projectile.velocity.ToRotation();

			if (CanCutTrees && Main.myPlayer == Projectile.owner)
			{
				var tilecoords = Projectile.Center.ToTileCoordinates();
				Tile tileAtPosition = AOUtils.GetTile(tilecoords.X, tilecoords.Y);
				if (!tileAtPosition.HasTile || !Main.tileAxe[tileAtPosition.TileType] || !WorldGen.CanKillTile(tilecoords.X, tilecoords.Y))
					return;
				AchievementsHelper.CurrentlyMining = true;
				WorldGen.KillTile(tilecoords.X, tilecoords.Y);
				if (Main.netMode == NetmodeID.MultiplayerClient)
					NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, tilecoords.X, tilecoords.Y);
				AchievementsHelper.CurrentlyMining = false;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			lightColor = Imbue?.Colour ?? lightColor;
			lightColor = SecondImbue?.Colour ?? lightColor;
			for (int k = Projectile.oldPos.Length - 1; k > -1; k--)
			{
				Vector2 drawPos = Projectile.oldPos[k] + (Projectile.Size / 2f) + new Vector2(0f, Projectile.gfxOffY);
				var colour2 = Projectile.GetAlpha(lightColor * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length));
				Main.EntitySpriteDraw(Sprite, drawPos - Main.screenPosition, null, colour2, Projectile.rotation, Sprite.Size() / 2, Projectile.scale - (k * .05f), SpriteEffects.None, 0);
			}
			return false;
		}

		public override bool? CanCutTiles() => CanCutTrees;
	}
}
