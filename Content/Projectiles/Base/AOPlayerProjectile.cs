using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using static ArcaneOdyssey.AOUtils;


namespace ArcaneOdyssey.Content.Projectiles.Base
{
	/// <summary>
	/// Projectile created by the player, usually via weapon
	/// </summary>
	public abstract class AOPlayerProjectile : AOBaseProjectile
	{
		public Item originalItem = null;
        public Vector2? DustVelocity;
        public bool killDust = true;
        public bool isSpell = false;
		public AOPlayer aoPlayerOwner = null;

        /// <summary>
        /// does not change when the player's imbue changes, make sure to assign in the ai using ??= to only apply on the first frame
        /// </summary>
        public AOMagic thisMagic = null;

        public const float AOSpeed = 1f;
		public const float AOSize = 1f;
		public const float AODamage = 1f;

		public virtual AODebuff Debuff => null;
		public virtual SoundStyle? DebuffApplySound => null;

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            AOPlayerProjectile proj = Projectile.ModProjectile as AOPlayerProjectile;
            AODebuff Debuff = proj.Debuff;
            SoundStyle? DebuffApplySound = proj.DebuffApplySound;
            if (Debuff is not null && (Debuff.DebuffPercent is null or 0 || modifiers.GetDamage(Projectile.damage, true) > (target.lifeMax / Debuff.DebuffPercent)))
            {
                target.AddBuff(Debuff.debuffID, Debuff.debuffDuration);
                if (DebuffApplySound.HasValue)
                {
                    SoundEngine.PlaySound(DebuffApplySound.Value, target.position);
                }
            }
        }
	}
}
