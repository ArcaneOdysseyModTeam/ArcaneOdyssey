using ArcaneOdysseyMusic;
using Terraria.GameContent;

namespace ArcaneOdyssey.NPCs.Base
{
	public abstract class BaseNPC : ModNPC, ILocalizedModType
	{
		public sealed override string LocalizationCategory => GetType().Namespace.Replace($"{Mod.Name}.");

		public virtual MusicTrack Theme => null;

		public override void SetDefaults()
		{
			if (Theme is not null && !Main.dedServ)
				Music = Theme.MusicSlot;
		}

		public Asset<Texture2D> Sprite => Texture != $"{Mod.Name}/{TextureAssets.Npc[Type]?.Name.Replace("\\", "/") ?? Texture}" ? ModContent.Request<Texture2D>(Texture) : TextureAssets.Npc[Type];
	}
}
