using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Developer
{
	public class FrogMagic : AOMagic
	{
		public override string Texture => $"Terraria/Images/Item_{ItemID.Frog}";
		public override bool? Cold => true;
		public override AOImbuableTier ImbuableTier => AOImbuableTier.Developer;
		public override SoundStyle? ImbueSound => SoundID.Frog;
		public override Color ImbueColour => new(0, 180, 0);
		public override float AOImbueSpeed => 1f;
		public override float AOImbueDamage => 1f;
		public override float AOImbueSize => 1f;

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			NPC npc = NPC.NewNPCDirect(null, area.Center(), NPCID.Frog);
			npc.velocity = direction;
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 10; n++)
			{
				NPC npc = NPC.NewNPCDirect(null, position, NPCID.Frog);
				npc.velocity = new Vector2((Main.rand.NextFloat() - 0.5f) * 10f * intensity, (Main.rand.NextFloat() - 0.5f) * 10f * intensity);
			}
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}
	}
}