using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Other
{
	public class FrogMagic : AOMagic
	{
		public override bool? Cold => true;
		public override AOImbuableTier ImbuableTier => AOImbuableTier.Developer;
		public override SoundStyle? ImbueSound => SoundID.Frog;
		public override Color ImbueColour => new(0, 180, 0, 255);
		public override bool CanBeWet => true;
		public override float AOImbueSpeed => 1f;
		public override float AOImbueSize => 1f;
		public override float AOImbueDamage => 1f;
		public override float AOScrollSpeed => 1f;
		public override float AOScrollSize => 1f;
		public override float AOScrollDamage => 1f;
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
			
			],
			[
			
			]
			);
		public override void SpawningEffects(Entity projectile)
		{
			NPC npc = NPC.NewNPCDirect(null,projectile.Center,NPCID.Frog);
			npc.velocity = projectile.velocity;
			projectile.Kill();
		}

		public override void ExplosionEffects(Entity projectile)
		{
			for (int n = 0; n < 10; n++) {
				NPC npc = NPC.NewNPCDirect(null, projectile.Center, NPCID.Frog);
				npc.velocity = new Vector2((Main.rand.NextFloat() - 0.5f) * 10f, (Main.rand.NextFloat() - 0.5f) * 10f);
				projectile.Kill();
			}
		}

        public override void KillEffects(Entity projectile)
        {
			SoundEngine.PlaySound(ImbueSound, projectile.position, null);
        }
	}
}