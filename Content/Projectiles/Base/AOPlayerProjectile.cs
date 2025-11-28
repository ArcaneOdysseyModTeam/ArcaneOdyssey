using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;


namespace ArcaneOdyssey.Content.Projectiles.Base
{
	/// <summary>
	/// Projectile created by the player, usually via weapon
	/// </summary>
	public abstract class AOPlayerProjectile : ModProjectile, IImbuable
	{
		public virtual bool CanHaveImbue => true;
		public virtual bool? Cold => null;
		public AOPlayer aoPlayerOwner = null;
		public bool IsSpell => this is MagicSpell;

		public float BaseScale 
		{  
			get 
			{
				if (ArcaneOdysseyConfig.Instance.ProjectileSizes)
					return Projectile.ArcaneOdyssey().BaseScale.GetValueOrDefault(1f);
				else
					return Projectile.scale;
			}
			set
			{
				if (ArcaneOdysseyConfig.Instance.ProjectileSizes)
					Projectile.ArcaneOdyssey().BaseScale = value;
				else
					Projectile.scale = value;
			}
		}

		public Imbuable Imbue
		{
			get => Projectile.ArcaneOdyssey().Imbue;
			set => Projectile.ArcaneOdyssey().Imbue = value;
		}

		public override void SetDefaults()
		{
			BaseScale = AOSize;
		}

		public virtual float AOSpeed => 1f;
		public virtual float AOSize => 1f;
		public virtual float AODamage => 1f;

		public virtual AODebuffRequirement? Debuff => new(ModContent.BuffType<AOBleed>(), 60*5);
		public virtual SoundStyle? DebuffApplySound => null;

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			if (Debuff.HasValue && (Debuff.Value.debuffPercent == 0 || modifiers.GetDamage(Projectile.damage, true) > target.lifeMax / Debuff.Value.debuffPercent))
			{
				target.AddBuff(Debuff.Value.debuffID, Debuff.Value.debuffDuration);
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

		public override bool PreDraw(ref Color lightColor)
		{
            if (ModContent.RequestIfExists<Texture2D>(Texture, out var tex))
            {
                Main.EntitySpriteDraw(tex.Value, Projectile.Center - Main.screenPosition, new(0, tex.Height() / Main.projFrames[Type] * Projectile.frame, tex.Width(), tex.Height() / Main.projFrames[Type]), lightColor with { A = (byte)(255 - Projectile.alpha) }, Projectile.rotation, new Vector2(tex.Width(), tex.Height() / Main.projFrames[Type]) / 2f, Projectile.scale, SpriteEffects.None);
                return false;
            }
            return true;
		}
	}
}
