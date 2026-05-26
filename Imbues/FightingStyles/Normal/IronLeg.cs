using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Imbues.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Imbues.FightingStyles.Normal
{
	public class IronLeg : FightingStyle
	{
		public override float Aura => 1.5f;
		public override float? DashResist => 1.35f;
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			if (Main.netMode != NetmodeID.Server)
			{
				EquipLoader.GetEquipSlot(Mod, Name, EquipType.Shoes);
			}
		}

		public override void Load()
		{
			base.Load();
			if (Main.netMode != NetmodeID.Server)
			{
				EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.Shoes}", EquipType.Shoes, this);
			}
		}

		public override Color ImbueColour => Color.LightGray;
		public override SoundStyle? ImbueSound => SoundID.Item99;

		public override float ImbueDamage => 1.125f;
		public override float ImbueSpeed => 0.75f;
		public override float ImbueSize => 1.1f;
		public override float ScrollDamage => .95f;
		public override float ScrollSize => 1.1f;
		public override float ScrollSpeed => 0.75f;

		public override Debuff[] ImbueDebuffs => [Debuff.Create<Bleeding>()];
		public override SynergyEffects Effects => new(
			[
				ClearBuff.Create<FreezingEffect>()
			],
			[
				Synergy.Create<Crystallized>(1.05f),
				Synergy.Create<FreezingEffect>(1.2f),
				Synergy.Create<SandyEffect>(1.1f),
				Synergy.Create<Melting>(1.1f),
				Synergy.Create<Corroding>(1.1f)
			]
		);
		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Mercury, direction.X * 0.4f, direction.Y * 0.4f, Scale: area.RelativeScale());
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.SilverFlame, Scale: area.RelativeScale())];
			spawnedDust.noGravity = true;
			spawnedDust.noLight = true;
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust.NewDust(position, 0, 0, DustID.Mercury, (Main.rand.NextFloat() - 0.5f) * (15f * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * intensity), Scale: 2f * intensity);
			}
		}
		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 30; n++)
			{
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Mercury, 2f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 2f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: area.RelativeScale());
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}
		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<BasicCombat>().AddRecipeGroup(RecipeGroupID.IronBar, 15).Register();
		}
	}

	public class ILegLegHelper : ModPlayer
	{
		public override void FrameEffects()
		{
			if (Player?.ArcaneOdyssey()?.Imbue is IronLeg || Player?.PlayerItem()?.type != ItemID.None && Player?.PlayerItem()?.ArcaneOdyssey()?.Imbue is IronLeg)
			{
				Player.shoe = EquipLoader.GetEquipSlot(Mod, typeof(IronLeg).Name, EquipType.Shoes);
			}
		}
	}
}
