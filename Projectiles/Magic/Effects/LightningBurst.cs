using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Magic.Lost;
using ArcaneOdyssey.Projectiles.Base;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Projectiles.Magic.Effects
{
	public class LightningBurst : PlayerProjectile
	{
		public override bool? CanDamage() => false;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = Projectile.height = AetherExplosion.SpriteSize;
			Projectile.light = 1f;
			Projectile.hide = true;
			Projectile.timeLeft = 60;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.tileCollide = false;
		}

		public override bool CanHaveImbueVFX => false;

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
		{
			overPlayers.Add(index);
		}

		private bool setDrawInfo = false;

		public override void OnSpawn(IEntitySource source)
		{
			if (Projectile.ai[0] > 0)
			{
				Projectile.scale = Projectile.ai[0];
				Projectile.Hitbox = Utils.CenteredRectangle(Projectile.Center, new(AetherExplosion.SpriteSize)).Scaled(Projectile.scale);
			}
		}


		public struct Branch(Vector2 start, float rotation, float length)
		{
			public Vector2 Start { get; } = start;
			public float Rotation { get; } = rotation;
			public float Length { get; } = length + 2f;

			public Vector2 End => Start + (Rotation.ToRotationVector2() * Length);

			public Branch[] children = new Branch[2];

			public override string ToString() => $"{{{Start}, {End}}}";
		}

		private Branch[] branches = new Branch[Main.rand.Next(3, 6)];

		public override bool PreDraw(ref Color lightColor)
		{
			if (!setDrawInfo)
			{
				Vector2 pos = Projectile.Center;
				float length = 50f * Projectile.scale;
				float rot = Main.rand.NextFloat(MathHelper.TwoPi);
				for (int i = 0; i < branches.Length; i++)
				{
					var angle = Main.rand.NextFloat(-(MathHelper.Pi / 8f), MathHelper.Pi / 8f);
					branches[i] = new(pos - ((rot + angle).ToRotationVector2() * 2f), rot + angle, length);
					rot += MathHelper.TwoPi / branches.Length;
				}
				foreach (var branch in branches)
				{
					for (int i = 0; i < branch.children.Length; i++)
					{
						rot = branch.Rotation + Main.rand.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2);
						branch.children[i] = new(branch.End, rot, length / 2f);
					}
					foreach (var branch2 in branch.children)
					{
						for (int i = 0; i < branch2.children.Length; i++)
						{
							rot = branch.Rotation + Main.rand.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2);
							branch2.children[i] = new(branch2.End, rot, length / 4f);
						}
					}
				}
				setDrawInfo = true;
			}

			Projectile.Opacity = Projectile.timeLeft / 60f;
			Color colour = Projectile.GetAlpha(Imbue?.Colour ?? lightColor);
			foreach (var branch in branches)
			{
				AOUtils.DrawChain(branch.Start, branch.End, Sprite, Projectile.scale * .75f, colour: colour);
				foreach (var branch2 in branch.children)
				{
					AOUtils.DrawChain(branch2.Start, branch2.End, Sprite, Projectile.scale / 2f, colour: colour);
					foreach (var branch3 in branch2.children)
					{
						AOUtils.DrawChain(branch3.Start, branch3.End, Sprite, Projectile.scale / 3f, colour: colour);
					}
				}
			}

			return false;
		}
	}
}
