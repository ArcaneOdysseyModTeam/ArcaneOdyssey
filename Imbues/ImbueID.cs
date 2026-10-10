using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.MagicVariants.Base;
using ReLogic.Reflection;
using System.Collections.Generic;
using Terraria.GameContent;

namespace ArcaneOdyssey.Imbues
{
	public class ImbueID
	{
		public const byte BasicCombat = 0;
		public const byte Acid = 1;
		public const byte Ash = 2;
		public const byte Crystal = 3;
		public const byte Earth = 4;
		public const byte Explosion = 5;
		public const byte Fire = 6;
		public const byte Glass = 7;
		public const byte Ice = 8;
		public const byte Light = 9;
		public const byte Lightning = 10;
		public const byte Metal = 11;
		public const byte Magma = 12;
		public const byte Plasma = 13;
		public const byte Poison = 14;
		public const byte Sand = 15;
		public const byte Shadow = 16;
		public const byte Snow = 17;
		public const byte Water = 18;
		public const byte Wind = 19;
		public const byte Wood = 20;
		public const byte Spirit = 21;
		public static readonly byte Count = 22;
		public static readonly IdDictionary Search = IdDictionary.Create<ImbueID, byte>();

		[ReinitializeDuringResizeArrays]
		public class Sets : ModSystem
		{
			public static SetFactory Factory = new(Imbuable.count, nameof(ImbueID), Search);

			public static List<int>[] Mutations = Factory.CreateCustomSet<List<int>>(null);

			public static int?[] baseImbues = ItemID.Sets.Factory.CreateCustomSet<int?>(null);

			public static bool[] Solid = Factory.CreateBoolSet(Crystal, Earth, Glass, Ice, Metal, Wood);
			public static bool[] Bright = Factory.CreateBoolSet(Ash, Fire, Light, Lightning, Magma, Plasma, Spirit);

			public static int[] DefaultVariant = Factory.CreateIntSet();

			public static bool[] HasVariants = Factory.CreateBoolSet();

			public static bool[] VariantsUseSprites = Factory.CreateBoolSet();

			public static int[] BlastFrames = Factory.CreateIntSet(1);

			public static float[] AuraPower = Factory.CreateFloatSet(.7f,
					Acid, .8f,
					BasicCombat, 1f,
					Crystal, 1.2f,
					Earth, 1.3f,
					Glass, .2f,
					Ice, 1.1f,
					Magma, 1f,
					Metal, 1.4f,
					Sand, 1f,
					Shadow, 1f,
					Snow, .9f,
					Water, .8f,
					Wood, 1.2f,
					BasicCombat, 1f
				);

			public static float[] KBMulti = Factory.CreateFloatSet(1f,
					Wind, 2f
				);

			/// <summary>
			/// Whether this magic is a:
			/// <list>Lesser Lost Magic</list>
			/// <list>Lost Spirit Mutation</list>
			/// <list>Ancient Spirit Mutation</list>
			/// <list>Mythical Spirit Mutation</list>
			/// </summary>
			public static bool[] Special = Factory.CreateBoolSet(Acid, Ash, Crystal, Explosion, Glass, Ice, Magma, Metal, Plasma, Poison, Sand, Snow, Wood);

			public static Color?[] DefaultAnimatedColours = Factory.CreateCustomSet<Color?>(null);

			public override void ResizeArrays()
			{
				// manually change default value
				for (int i = 0; i < Mutations.Length; i++)
				{
					Mutations[i] = [];
				}
			}

			public override void PostSetupContent()
			{
				foreach (var imbue in ModContent.GetContent<MagicType>())
				{
					var ID = imbue.ID;
					if (TextureAssets.Item[imbue.Type].Name == "Assets\\Blank")
					{
						if (HasVariants[ID] && VariantsUseSprites[ID])
						{
							TextureAssets.Item[imbue.Type] = MagicVariant.GetFromID(DefaultVariant[ID]).Icon;
						}
						else
						{
							ArcaneOdysseyMod.NoticeQueue.Add(imbue.FullName + " is missing sprite");
						}
					}
					if (Assets.blasts[ID] is null)
					{
						if (HasVariants[ID] && VariantsUseSprites[ID])
						{
							var Blast = MagicVariant.GetFromID(DefaultVariant[ID]).Blast;
							Assets.blasts[ID] = Blast;
							BlastFrames[ID] = (Blast.Height() / ((float)Blast.Width())).Round();
						}
						else
						{
							ArcaneOdysseyMod.NoticeQueue.Add(imbue.FullName + " is missing blast sprite");
						}
					}
					if (Assets.annihilationSprites[ID] is null)
					{
						if (HasVariants[ID] && VariantsUseSprites[ID])
						{
							Assets.annihilationSprites[ID] = MagicVariant.GetFromID(DefaultVariant[ID]).Annihilation;
						}
						else if(ArcaneOdysseyMod.DevMode)
						{
							ArcaneOdysseyMod.NoticeQueue.Add(imbue.FullName + " is missing annihilation sprite");
						}
					}
					if (Assets.raySprites[ID] is null)
					{
						if (HasVariants[ID] && VariantsUseSprites[ID])
						{
							Assets.raySprites[ID] = MagicVariant.GetFromID(DefaultVariant[ID]).Ray;
						}
						else
						{
							ArcaneOdysseyMod.NoticeQueue.Add(imbue.FullName + " is missing ray sprite");
						}
					}
					if (Assets.rayEndSprites[ID] is null)
					{
						if (HasVariants[ID] && VariantsUseSprites[ID])
						{
							Assets.rayEndSprites[ID] = MagicVariant.GetFromID(DefaultVariant[ID]).RayEnd;
						}
						else
						{
							ArcaneOdysseyMod.NoticeQueue.Add(imbue.FullName + " is missing ray end sprite");
						}
					}
					if (Assets.rayStartSprites[ID] is null)
					{
						if (HasVariants[ID] && VariantsUseSprites[ID])
						{
							Assets.rayStartSprites[ID] = MagicVariant.GetFromID(DefaultVariant[ID]).RayStart;
						}
						else
						{
							ArcaneOdysseyMod.NoticeQueue.Add(imbue.FullName + " is missing ray start sprite");
						}

					}
				}
			}

			[ReinitializeDuringResizeArrays]
			public static class Assets
			{
				public static Asset<Texture2D>[] annihilationSprites = ItemID.Sets.Factory.CreateCustomSet(Asset<Texture2D>.Empty);

				public static Asset<Texture2D>[] raySprites = ItemID.Sets.Factory.CreateCustomSet(Asset<Texture2D>.Empty);

				public static Asset<Texture2D>[] rayEndSprites = ItemID.Sets.Factory.CreateCustomSet(Asset<Texture2D>.Empty);

				public static Asset<Texture2D>[] rayStartSprites = ItemID.Sets.Factory.CreateCustomSet(Asset<Texture2D>.Empty);

				public static Asset<Texture2D>[] blasts = ItemID.Sets.Factory.CreateCustomSet(Asset<Texture2D>.Empty);
			}
		}
	}
}
