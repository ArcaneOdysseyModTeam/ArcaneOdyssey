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
using static ArcaneOdyssey.AOUtils;


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
	}
}
