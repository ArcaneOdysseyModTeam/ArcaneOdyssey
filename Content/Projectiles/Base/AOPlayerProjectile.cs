using ArcaneOdyssey.Content.Items.Base;
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
using static ArcaneOdyssey.AOConversion;


namespace ArcaneOdyssey.Content.Projectiles.Base
{
	/// <summary>
	/// Projectile created by the player, usually via weapon
	/// </summary>
	public abstract class AOPlayerProjectile : ModProjectile
	{
		public Item? originalItem = null;
		public AOPlayer? aoPlayerOwner = null;

		public const float AOSpeed = 1f;
		public const float AOSize = 1f;
		public const float AODamage = 1f;

		public virtual AODebuff? Debuff => null;
		public virtual SoundStyle? DebuffApplySound => null;


		// Projectile.ai[0] is 
		// Projectile.ai[1] is 
		// Projectile.ai[2] is

		public virtual void OnHitNPC2(NPC target, NPC.HitInfo info, int damageDone) {}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Debuff is not null && (Debuff.DebuffPercent is null or 0 || hit.Damage > (target.lifeMax / Debuff.DebuffPercent)))
			{
				target.AddBuff(Debuff.debuffID, Debuff.debuffDuration);
				SoundEngine.PlaySound(DebuffApplySound, target.position);
			}
			OnHitNPC2(target, hit, damageDone);
		}
	}
}
