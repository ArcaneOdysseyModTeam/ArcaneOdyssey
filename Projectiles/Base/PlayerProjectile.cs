using ArcaneOdyssey.AOPlayers;
using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Imbues.Base;
using Terraria.Audio;


namespace ArcaneOdyssey.Projectiles.Base
{
	/// <summary>
	/// Projectile created by the player, usually via weapon
	/// </summary>
	public abstract class PlayerProjectile : BaseProjectile, IImbuable
	{
		public virtual bool CanHaveImbueVFX => true;

		public float ApplySpeed(float value, bool flipfloat = false) => Projectile.ArcaneOdyssey().ApplySpeed(value, flipfloat);

		public float ApplySize(float value, bool flipfloat = false) => Projectile.ArcaneOdyssey().ApplySize(value, flipfloat);

		public float ApplyKnockback(float value, bool flipfloat = false) => Projectile.ArcaneOdyssey().ApplyKnockback(value, flipfloat);

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
			set
			{
				if (Projectile.ArcaneOdyssey() is not null)
					Projectile.ArcaneOdyssey().Imbue = value;
			}
		}

		public Imbuable SecondImbue
		{
			get => Projectile.ArcaneOdyssey()?.SecondImbue;
			set
			{
				if (Projectile.ArcaneOdyssey() is not null)
					Projectile.ArcaneOdyssey().SecondImbue = value;
			}
		}

		public bool? BenifitsFromScrollStats => Projectile.ArcaneOdyssey()?.BenifitsFromScrollStats;

		public virtual Debuff? ProjectileDebuff => Debuff.Create<Bleeding>(60 * 5);
		public virtual SoundStyle? HitSound => null;
	}
}
