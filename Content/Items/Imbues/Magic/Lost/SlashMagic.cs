using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Normal;
using ArcaneOdyssey.VFX.Dusts;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Lost
{
	public class SlashMagic : AOMagic
	{
		public override AOImbuableTier ImbuableTier => AOImbuableTier.Lost;
		public override float AOImbueDamage => 1.1f;
		public override float AOImbueSpeed => 1.1f;
		public override float AOImbueSize => .8f;

		public override void LingeringEffects(Entity entity)
		{
			Dust.NewDust(entity.position, entity.width, entity.height, ModContent.DustType<SlashDust>());
		}

		public override void AddRecipes()
		{
			CreateLostRecipe(typeof(MetalMagic), typeof(GlassMagic), typeof(WoodMagic));
		}
	}
}
