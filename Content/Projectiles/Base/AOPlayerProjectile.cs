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
	public abstract class AOPlayerProjectile : ModProjectile
	{
		public virtual bool? Cold => null;
		public Item originalItem = null;
		public Vector2? DustVelocity;
		public bool killDust = true;
		public AOPlayer aoPlayerOwner = null;
		public bool IsSpell => this is MagicSpell;

		public float BaseScale 
		{  
			get => Projectile.ArcaneOdyssey().BaseScale.GetValueOrDefault(1f);
			set => Projectile.ArcaneOdyssey().BaseScale = value; 
		}

		public float FramesAlive => Projectile.ArcaneOdyssey().FramesAlive;

		public AOMagic Imbue 
		{
			get => Projectile.ArcaneOdyssey().imbue;
			set => Projectile.ArcaneOdyssey().imbue = value;
		}

		public virtual float AOSpeed => 1f;
		public virtual float AOSize => 1f;
		public virtual float AODamage => 1f;

		public virtual AODebuffRequirement Debuff => null;
		public virtual SoundStyle? DebuffApplySound => null;

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			AOPlayerProjectile proj = Projectile.ModProjectile as AOPlayerProjectile;
			AODebuffRequirement Debuff = proj.Debuff;
			SoundStyle? DebuffApplySound = proj.DebuffApplySound;
			if (Debuff is not null && (Debuff.DebuffPercent is null or 0 || modifiers.GetDamage(Projectile.damage, true) > target.lifeMax / Debuff.DebuffPercent))
			{
				target.AddBuff(Debuff.debuffID, Debuff.debuffDuration);
				if (DebuffApplySound.HasValue)
				{
					SoundEngine.PlaySound(DebuffApplySound.Value, target.position);
				}
			}
		}

		/// <summary>
		/// Kills the projectile.
		/// </summary>
		public void Kill()
		{
			Projectile.Kill();
		}
	}
}
