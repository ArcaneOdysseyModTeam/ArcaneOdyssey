using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Tiles.Base
{
    public abstract class BaseBossRelic : ModTile
    {
        public override string Texture => $"{ArcaneOdyssey.InternalName}/Assets/RelicBase";

        public Asset<Texture2D> FloaterTexture;

        public override void Load()
        {
            FloaterTexture = ModContent.Request<Texture2D>(base.Texture);
        }

        public override void SetStaticDefaults()
        {
            Main.tileShine[Type] = 400;
            Main.tileFrameImportant[Type] = true;
            TileID.Sets.InteractibleByNPCs[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newTile.DrawYOffset = 2;
            TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
            TileObjectData.newTile.StyleHorizontal = false;
            
            TileObjectData.newTile.StyleWrapLimitVisualOverride = 2;
            TileObjectData.newTile.StyleMultiplier = 2;
            TileObjectData.newTile.StyleWrapLimit = 2;
            TileObjectData.newTile.styleLineSkipVisualOverride = 0;

            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
            TileObjectData.addAlternate(1);
            TileObjectData.addTile(Type);
            AddMapEntry(new Color(233, 207, 94), Language.GetText("MapObject.Relic"));
        }
    }
}
