using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.PlayerClasses;
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
	public abstract class AOPlayerProjectile : AOBaseProjectile, IImbuable
	{
		public virtual bool CanHaveImbueVFX => true;

		public float ApplyScrollSpeed(float value, bool flipfloat = false)
		{
			if (Imbue is not null)
			{
				if (!flipfloat)
				{
					value *= Imbue.AOScrollSpeed;
					if (SecondImbue is not null)
						value *= SecondImbue.AOScrollSpeed;
				}
				else
				{
					value *= Imbue.AOScrollSpeed.FlipFloat();
					if (SecondImbue is not null)
						value *= SecondImbue.AOScrollSpeed.FlipFloat();
				}
			}
			return value;
		}

		public float ApplyImbueSpeed(float value, bool flipfloat = false)
		{
			if (Imbue is not null)
			{
				if (!flipfloat)
				{
					value *= Imbue.AOImbueSpeed;
					if (SecondImbue is not null)
						value *= SecondImbue.AOImbueSpeed;
				}
				else
				{
					value *= Imbue.AOImbueSpeed.FlipFloat();
					if (SecondImbue is not null)
						value *= SecondImbue.AOImbueSpeed.FlipFloat();
				}
			}
			return value;
		}

		public virtual bool CanHaveImbue => true;
		public virtual bool? Cold => null;

		public AOPlayer AOPlayerOwner => Owner?.ArcaneOdyssey();

		public Player Owner
		{
			get
			{
				if (Projectile.owner != 255 && Main.player[Projectile.owner]?.active == true)
				{
					return Main.player[Projectile.owner];
				}
				return null;
			}
		}

		public Imbuable Imbue
		{
			get => Projectile.ArcaneOdyssey()?.Imbue;
			set => Projectile.ArcaneOdyssey().Imbue = value;
		}

		public Imbuable SecondImbue
		{
			get => Projectile.ArcaneOdyssey()?.SecondImbue;
			set => Projectile.ArcaneOdyssey().SecondImbue = value;
		}

		public bool? BenifitsFromScrollStats => Projectile.ArcaneOdyssey()?.BenifitsFromScrollStats;

		public override void SetDefaults()
		{
			Projectile.scale *= AOSize;
		}

		public virtual float AOSpeed => 1f;
		public virtual float AOSize => 1f;

		public virtual Debuff? ProjectileDebuff => Debuff.Create<AOBleed>(60 * 5);
		public virtual SoundStyle? HitSound => null;
	}
}
