using ArcaneOdyssey.AOPlayers;
using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Imbues.Base;
using Terraria;
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

		public virtual bool CanHaveImbue => true;
		public virtual bool? Cold => null;

		public AOPlayer AOPlayerOwner => Owner?.ArcaneOdyssey();

		public void NetUpdate()
		{
			Projectile.netSpam = 0;
			Projectile.netUpdate = true;
		}

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
			set => Projectile.ArcaneOdyssey().SecondImbue = value;
		}

		public bool? BenifitsFromScrollStats => Projectile.ArcaneOdyssey()?.BenifitsFromScrollStats;

		public override void SetDefaults()
		{
			Projectile.scale *= Size;
		}

		public virtual float Speed => 1f;
		public virtual float Size => 1f;

		public virtual Debuff? ProjectileDebuff => Debuff.Create<AOBleed>(60 * 5);
		public virtual SoundStyle? HitSound => null;
	}
}
