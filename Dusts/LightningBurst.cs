namespace ArcaneOdyssey.Dusts
{
	public class LightningBurst : ModDust
	{
		public struct Branch(Vector2 start, float rotation, float length)
		{
			public Vector2 Start { get; } = start;
			public float Rotation { get; } = rotation;
			public float Length { get; } = length;

			public Vector2 End => Start + (Rotation.ToRotationVector2() * Length);

			public Branch[] children = new Branch[2];

			public override string ToString() => $"{{{Start} {End}}}";
		}

		private static Branch[][] branches = new Branch[Main.maxDust][];

		public static void SetDrawInfo(Dust dust)
		{
			dust.firstFrame = false;
			Vector2 pos = dust.position;
			float length = 200f * dust.scale;
			float rot = dust.rotation;

			int maxBranches = 1;
			if (dust.customData is int maxbranches)
			{
				maxBranches = maxbranches;
			}

			branches[dust.dustIndex] = new Branch[maxBranches];

			for (int i = 0; i < branches[dust.dustIndex].Length; i++)
			{
				var angle = Main.rand.NextFloat(-(MathHelper.Pi / 8f), MathHelper.Pi / 8f);
				branches[dust.dustIndex][i] = new(pos - ((rot + angle).ToRotationVector2() * (2f * dust.scale)), rot + angle, length / 4f);
				rot += MathHelper.TwoPi / maxBranches;
			}

			foreach (var branch in branches[dust.dustIndex])
			{
				for (int i = 0; i < branch.children.Length; i++)
				{
					rot = branch.Rotation + Main.rand.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2);
					branch.children[i] = new(branch.End, rot, length / 6f);
				}
				foreach (var branch2 in branch.children)
				{
					for (int i = 0; i < branch2.children.Length; i++)
					{
						rot = branch2.Rotation + Main.rand.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2);
						branch2.children[i] = new(branch2.End, rot, length / 8f);
					}
				}
			}
		}

		public override void OnSpawn(Dust dust)
		{
			dust.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
			dust.noGravity = true;
			dust.frame = Texture2D.Frame();
		}

		public override bool Update(Dust dust)
		{
			dust.alpha += 2;
			if (dust.alpha >= 255)
			{
				dust.active = false;
			}
			return false;
		}

		public override bool PreDraw(Dust dust)
		{
			if (dust.firstFrame)
				SetDrawInfo(dust);

			Color? colour = null;
			if (dust.color != default)
			{
				colour = dust.GetAlpha(dust.color);
			}

			foreach (var branch in branches[dust.dustIndex])
			{
				var chain = AOUtils.DrawChain(branch.Start, branch.End, Texture2D.Value, dust.scale * .75f, colour: colour, opacity: 1f - (dust.alpha / 255f));
				var mid = Vector2.Lerp(branch.Start, chain.Ending, .5f);
				if (colour.HasValue)
				{
					Lighting.AddLight(mid, colour.Value.ToVector3() * dust.scale);
					Lighting.AddLight(chain.Ending, colour.Value.ToVector3() * dust.scale);
				}
				foreach (var branch2 in branch.children)
				{
					chain = AOUtils.DrawChain(branch2.Start, branch2.End, Texture2D.Value, dust.scale / 2f, colour: colour, opacity: 1f - (dust.alpha / 255f));
					mid = Vector2.Lerp(branch2.Start, chain.Ending, .5f);
					if (colour.HasValue)
					{
						Lighting.AddLight(mid, colour.Value.ToVector3() * dust.scale);
						Lighting.AddLight(chain.Ending, colour.Value.ToVector3() * dust.scale);
					}
					foreach (var branch3 in branch2.children)
					{
						chain = AOUtils.DrawChain(branch3.Start, branch3.End, Texture2D.Value, dust.scale / 3f, colour: colour, opacity: 1f - (dust.alpha / 255f));
						mid = Vector2.Lerp(branch3.Start, chain.Ending, .5f);
						if (colour.HasValue)
						{
							Lighting.AddLight(mid, colour.Value.ToVector3() * dust.scale);
							Lighting.AddLight(chain.Ending, colour.Value.ToVector3() * dust.scale);
						}
					}
				}
			}
			return false;
		}
	}
}
