using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic.Blasts;
using ArcaneOdyssey.Content.Items.Base;
using System;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Buffs.Stuns;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Buffs.DOT;
using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace ArcaneOdyssey.Content.Items.Magic
{
	public class FrogMagic : AOMagic
	{
		public override bool? Cold => false;
		public override SoundStyle? ImbueSound => SoundID.Frog;
		public override Color ImbueColour => new Color(0, 255, 0, 255);
		public override bool CanBeWet => true;
		public override float AOImbueSpeed => 1f;
		public override float AOImbueSize => 1f;
		public override float AOImbueDamage => 1f;
		public override float AOScrollSpeed => 1f;
		public override float AOScrollSize => 1f;
		public override float AOScrollDamage => 1f;
		public override SynergyEffects Effects => new SynergyEffects(
			[ // these are debuffs cleared on hit
			
			],
			[
			
			]
			);
		public override void SpawningEffects(Projectile projectile)
		{
			NPC npc = NPC.NewNPCDirect(null,projectile.position,NPCID.Frog);
			npc.velocity = projectile.velocity;
			projectile.Kill();
		}

		public override void ExplosionEffects(Projectile projectile)
		{
			NPC npc = NPC.NewNPCDirect(null,projectile.position,NPCID.Frog);
			npc.velocity = new Vector2((Main.rand.NextFloat()-0.5f)*10f,(Main.rand.NextFloat()-0.5f)*10f);
			projectile.Kill();
		}

        public override void KillEffects(Projectile projectile)
        {
			SoundEngine.PlaySound(this.ImbueSound, projectile.position, null);
        }
		public override Dictionary<Type, int> Skills => new([KeyValuePair.Create(typeof(BlastSpell), ModContent.ProjectileType<FrogBlast>()),]);
	}
}