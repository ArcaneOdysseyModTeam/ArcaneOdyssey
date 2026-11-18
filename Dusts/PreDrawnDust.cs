namespace ArcaneOdyssey.Dusts
{
	public abstract class PreDrawnDust : ModDust
	{
		public virtual int Rows => 1;
		public virtual int Columns => 1;

		public override void OnSpawn(Dust dust)
		{
			dust.frame = Texture2D.Frame(Columns, Rows, Main.rand.Next(Columns), Main.rand.Next(Rows));
			dust.rotation = MathHelper.PiOver2 * Main.rand.Next(4);
			dust.noGravity = true;
		}

		public override bool PreDraw(Dust dust)
		{
			Main.EntitySpriteDraw(Texture2D.Value, dust.position - Main.screenPosition, dust.frame, dust.GetAlpha(dust.color == default ? Lighting.GetColor(dust.position.ToTileCoordinates()) : dust.color), dust.rotation, dust.frame.Size() / 2f, dust.scale, SpriteEffects.None);
			return false;
		}
	}
}
