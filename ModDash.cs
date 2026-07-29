using ArcaneOdyssey.AOPlayers;
using ArcaneOdyssey.Imbues.Base;

namespace ArcaneOdyssey
{
	public abstract class ModDash(Entity source) : ModType, IImbuable
	{
		public Entity Source { get; } = source;
		public Imbuable Imbue { get; set; }
		public Imbuable SecondImbue { get; set; }

		public virtual bool FallThrough => true;


		/// <summary>
		/// Whether the player is immune to contact damage while dashing, does not affect projectiles
		/// </summary>
		public abstract bool Immune { get; }

		/// <summary>
		/// Damage of the dash, set to 0 to disable damage
		/// </summary>
		public virtual int Damage
		{
			get
			{
				if (Source is Projectile projectile)
				{
					return projectile.damage;
				}
				if (Source is Item item)
				{
					if (item.ArcaneOdyssey()?.owner is not null)
					{
						if (item.ModItem is Imbuable imbue && imbue.Dash is not null)
						{
							return (int)item.ArcaneOdyssey().owner.GetTotalDamage(DamageType).ApplyTo(imbue.Dash.Damage);
						}
						return (int)item.ArcaneOdyssey().owner.GetTotalDamage(item.DamageType).ApplyTo(item.damage);
					}
					return item.damage;
				}
				return 0;
			}
		}

		public virtual bool ContactDamage => Damage > 0;

		public virtual DamageClass DamageType => DamageClass.Default;

		/// <summary>
		/// Knockback of the dash
		/// </summary>
		public virtual float Knockback
		{
			get
			{
				if (Source is Projectile projectile)
				{
					return projectile.knockBack;
				}
				if (Source is Item item)
				{
					if (item.ArcaneOdyssey()?.owner is not null)
					{
						if (item.ModItem is Imbuable imbue)
						{
							if (imbue.Dash is not null)
								return item.ArcaneOdyssey().owner.GetTotalKnockback(DamageType).ApplyTo(imbue.Dash.Knockback);
						}
						return  item.ArcaneOdyssey().owner.GetTotalKnockback(item.DamageType).ApplyTo(item.knockBack);
					}
					return item.knockBack;
				}
				return 0;
			}
		}

		/// <summary>
		/// Whether the dash can be trigger via hotkey, and if it can be used to go directions other than left and right
		/// </summary>
		public abstract bool LocksPlayer { get; }

		/// <summary>
		/// The cooldown between dash uses
		/// </summary>
		public abstract int Cooldown { get; }

		/// <summary>
		/// How long the dash lasts for
		/// </summary>
		public abstract int DashMax { get; }


		/// <summary>
		/// Sets the dash's cooldown
		/// </summary>
		/// <param name="player"></param>
		public void SetCooldown(Player player)
		{
			if (DisplayedCooldownID != -1)
			{
				player.ArcaneOdyssey()?.SetCooldown(DisplayedCooldownID, Cooldown);
			}
			else
				player.ArcaneOdyssey()?.SetCooldown(AOCooldown);
		}

		/// <summary>
		/// Whether the dash is on cooldown
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public bool OnCooldown(Player player)
		{
			if (DisplayedCooldownID != -1)
			{
				return player.ArcaneOdyssey().OnCooldown(DisplayedCooldownID) && !ArcaneOdysseyMod.DevMode;
			}
			if (LocksPlayer)
				return (player.ArcaneOdyssey().OnCooldown(Name) || player.ArcaneOdyssey().dashing) && !ArcaneOdysseyMod.DevMode;
			else
				return (player.ArcaneOdyssey().OnCooldown("StandardDash") || player.ArcaneOdyssey().dashing) && !ArcaneOdysseyMod.DevMode;
		}

		public sealed override void SetupContent() => SetStaticDefaults();

		/// <summary>
		/// Called every frame, and before the dash starts
		/// </summary>
		/// <param name="player"></param>
		/// <returns>Whether to keep dashing</returns>
		public virtual bool ExtraCheck(Player player) => true;

		/// <summary>
		/// The speed of the dash per tick
		/// </summary>
		public abstract float DashSpeed { get; }
		public bool? UseScrollImbueStats => Source.AnyArcaneOdyssey()?.BenifitsFromScrollStats;


		/// <summary>
		/// called every frame
		/// </summary>
		/// <param name="player"></param>
		public virtual void DashEffect(Player player) { }

		/// <summary>
		/// called once at start of dash
		/// </summary>
		/// <param name="player"></param>
		public virtual void OnStart(Player player) { }

		/// <summary>
		/// Called when the dash collisions a target
		/// </summary>
		/// <param name="player"></param>
		/// <param name="target"></param>
		/// <returns>Whether to end the dash</returns>
		public abstract bool OnHit(Player player, NPC target);

		public virtual void OnEnd(Player player) { }

		/// <summary>
		/// Called if the dash ends naturally without hitting any enemies
		/// </summary>
		public virtual void NaturalEnd(Player player) { }

		public virtual int DisplayedCooldownID => -1;

		public Cooldown AOCooldown => new(LocksPlayer ? Name : "StandardDash", Mod, Cooldown);

		protected sealed override void Register()
		{
			ModTypeLookup<ModDash>.Register(this);
		}

		public float ApplySpeed(float value, bool flipfloat = false)
		{
			if (UseScrollImbueStats.HasValue)
			{
				if (UseScrollImbueStats.Value)
				{
					if (Imbue is not null)
					{
						if (!flipfloat)
						{
							value *= Imbue.ScrollSpeed;
							if (SecondImbue is not null)
								value *= SecondImbue.ImbueSpeed;
						}
						else
						{
							value *= Imbue.ScrollSpeed.FlipFloat();
							if (SecondImbue is not null)
								value *= SecondImbue.ImbueSpeed.FlipFloat();
						}
					}
				}
				else
				{
					if (Imbue is not null)
					{
						if (!flipfloat)
						{
							value *= Imbue.ImbueSpeed;
							if (SecondImbue is not null)
								value *= SecondImbue.ImbueSpeed;
						}
						else
						{
							value *= Imbue.ImbueSpeed.FlipFloat();
							if (SecondImbue is not null)
								value *= SecondImbue.ImbueSpeed.FlipFloat();
						}
					}
				}
			}
			return value;
		}

		public float ApplySize(float value, bool flipfloat = false, Player player = null)
		{
			value *= player?.ArcaneOdyssey()?.SizeMulti ?? 1f;
			if (UseScrollImbueStats.HasValue)
			{
				if (UseScrollImbueStats.Value)
				{
					if (Imbue is not null)
					{
						if (!flipfloat)
						{
							value *= Imbue.ScrollSize;
							if (SecondImbue is not null)
								value *= SecondImbue.ImbueSize;
						}
						else
						{
							value *= Imbue.ScrollSize.FlipFloat();
							if (SecondImbue is not null)
								value *= SecondImbue.ImbueSize.FlipFloat();
						}
					}
				}
				else
				{
					if (Imbue is not null)
					{
						if (!flipfloat)
						{
							value *= Imbue.ImbueSize;
							if (SecondImbue is not null)
								value *= SecondImbue.ImbueSize;
						}
						else
						{
							value *= Imbue.ImbueSize.FlipFloat();
							if (SecondImbue is not null)
								value *= SecondImbue.ImbueSize.FlipFloat();
						}
					}
				}
			}
			return value;
		}

		public float ApplyKnockback(float value, bool flipfloat = false)
		{
			if (UseScrollImbueStats.HasValue)
			{
				if (!flipfloat)
				{
					if (Imbue is not null)
					{
						value *= Imbue.KBMulti;
						if (SecondImbue is not null)
							value *= SecondImbue.KBMulti;
					}
				}
				else
				{
					if (Imbue is not null)
					{
						value *= 1f / Imbue.KBMulti;
						if (SecondImbue is not null)
							value *= 1f / SecondImbue.KBMulti;
					}
				}
				if (UseScrollImbueStats.Value)
				{
					if (Imbue is not null)
					{
						if (!flipfloat)
						{
							value *= Imbue.ScrollSize * Imbue.ScrollSize;
							if (SecondImbue is not null)
								value *= SecondImbue.ImbueSize * SecondImbue.ImbueSize;
						}
						else
						{
							value *= Imbue.ScrollSize.FlipFloat() * Imbue.ScrollSize.FlipFloat();
							if (SecondImbue is not null)
								value *= SecondImbue.ImbueSize.FlipFloat() * SecondImbue.ImbueSize.FlipFloat();
						}
					}
				}
				else
				{
					if (Imbue is not null)
					{
						if (!flipfloat)
						{
							value *= Imbue.ImbueSize * Imbue.ImbueSize;
							if (SecondImbue is not null)
								value *= SecondImbue.ImbueSize * SecondImbue.ImbueSize;
						}
						else
						{
							value *= Imbue.ImbueSize.FlipFloat();
							if (SecondImbue is not null)
								value *= SecondImbue.ImbueSize.FlipFloat() * SecondImbue.ImbueSize.FlipFloat();
						}
					}
				}
			}
			return value;
		}
	}
}
