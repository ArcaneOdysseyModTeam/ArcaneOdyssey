using ArcaneOdyssey.Projectiles.Base;
using System;
using System.IO;
using Terraria.Audio;
using Terraria.DataStructures;

namespace ArcaneOdyssey.Projectiles.Abilities
{
	public class BeastInstinct : PlayerProjectile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.timeLeft = 50;
			Projectile.width = Projectile.height = 250;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.DamageType = AOUtils.TrueMelee();
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
		}

		private Vector2[] spots = new Vector2[10];

		public override void OnSpawn(IEntitySource source)
		{
			instance = this;
			Projectile.velocity = Vector2.Zero;
			Owner.GiveImmuneTimeForCollisionAttack(Projectile.timeLeft);
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			for (int i = 0; i < spots.Length; i++)
			{
				writer.Write(spots[i]);
			}
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			for (int i = 0; i < spots.Length; i++)
			{
				spots[i] = reader.ReadVector2();
			}
		}

		private byte spot = 0;

		internal static BeastInstinct instance;
		private float? dist = null;
		public override void AI()
		{
			if (Projectile.ai[0] == 0)
			{
				for (int i = 0; i < spots.Length; i++)
				{
					spots[i] = Projectile.Center + (Main.rand.NextFloat(MathHelper.TwoPi).ToRotationVector2() * (AOUtils.Average(Projectile.width, Projectile.height) / 2f));
				}
				Projectile.ai[0] = 1;
				NetUpdate();
			}
			Owner.itemAnimation = Owner.itemTime = 2;
			AOPlayerOwner.HeavySkillActive = true;
			AOPlayerOwner.CanMoveOnGround = false;
			Projectile.Opacity = Projectile.timeLeft / 50f;
			Owner.Center = Owner.Center.MoveTowards(spots[spot], ApplySpeed(dist.GetValueOrDefault(30f)));
			if (Owner.Center == spots[spot])
			{
				if (!Main.dedServ)
					SoundEngine.PlaySound(SoundID.Item71 with { MaxInstances = 0, Pitch = .5f }, spots[spot]);
				spot++;
				if (spot >= spots.Length)
					spot = 0;
				dist = spots[spot].Distance(Owner.Center) / 4f;
				Owner.ChangeDir(Math.Sign(Owner.SafeDirectionTo(spots[spot], Vector2.One).X));
				NetUpdate();
				if (Main.myPlayer == Projectile.owner)
				{
					AOUtils.SimulateAOE(Projectile.Hitbox, Projectile.damage / 10, Projectile.knockBack, Projectile, Projectile.DamageType, false);
				}
			}
		}

		public override bool? CanDamage() => false;

		public override void OnKill(int timeLeft)
		{
			Owner.Center = Projectile.Center;
			
			if (Main.myPlayer == Projectile.owner)
			{
				instance = null;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			var colour = Projectile.GetAlpha(Imbue?.Colour.MultiplyRGB(lightColor) ?? lightColor);
			Vector2 lastpoint = Projectile.Center;
			foreach (var point in spots)
			{
				AOUtils.DrawChain(lastpoint, point, Sprite, Projectile.scale, colour: colour);
				lastpoint = point;
			}
			AOUtils.DrawChain(lastpoint, Projectile.Center, Sprite, Projectile.scale, colour: colour);
			return false;
		}
	}

	public class BeastInstinctCamera : ModSystem
	{
		public override void ModifyScreenPosition()
		{
			if (BeastInstinct.instance is not null && BeastInstinct.instance.Projectile.active)
			{
				Main.screenPosition = BeastInstinct.instance.Projectile.Center - (Main.ScreenSize.ToVector2() / 2f);
			}
			else
			{
				BeastInstinct.instance = null;
			}
		}
	}
}
