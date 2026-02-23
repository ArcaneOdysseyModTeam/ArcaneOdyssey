using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Imbues.FightingStyles.Normal
{
	public class Boxing : FightingStyle
	{
		public override float Aura => 1.25f;
		public override float DashSpeed => 1.4f; // instant
		public override float KBMulti => 2f;
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			if (Main.netMode != NetmodeID.Server)
			{
				EquipLoader.GetEquipSlot(Mod, Name, EquipType.HandsOn);
				EquipLoader.GetEquipSlot(Mod, Name, EquipType.HandsOff);
			}
		}
		public override Color ImbueColour => Color.Red;
		public override SoundStyle? ImbueSound => SoundID.Item39;
		public override float AOImbueDamage => 0.9f;
		public override float AOImbueSpeed => 1.2f;
		public override float AOImbueSize => 1.056f;
		public override float AOScrollDamage => .8f;
		public override float AOScrollSize => 1f;
		public override float AOScrollSpeed => 1.2f;

		public override void Load()
		{
			if (Main.netMode != NetmodeID.Server)
			{
				EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.HandsOn}", EquipType.HandsOn, this);
				EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.HandsOff}", EquipType.HandsOff, this);
			}
		}

		public override SynergyEffects Effects => new(
			[],
			[
				new(ModContent.BuffType<FreezingEffect>(), 1.15f)
			]
		);

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.BubbleBurst_White, direction.X * 2f, direction.Y * 2f, Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.BubbleBurst_White, Scale: area.RelativeScale())];
			spawnedDust.noGravity = true;
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.BubbleBurst_White, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), Scale: 3f * intensity)];
				spawnedDust.noGravity = true;
			}
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.BubbleBurst_White, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}
		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<BasicCombat>().AddIngredient(ItemID.Silk, 15).Register();
		}
	}

	public class BoxingGlovesHelper : ModPlayer
	{
		public override void FrameEffects()
		{
			if (Player?.ArcaneOdyssey()?.Imbue is Boxing || Player?.PlayerItem()?.type != ItemID.None && Player?.PlayerItem()?.ArcaneOdyssey()?.Imbue is Boxing)
			{
				Player.handon = EquipLoader.GetEquipSlot(Mod, typeof(Boxing).Name, EquipType.HandsOn);
				Player.handoff = EquipLoader.GetEquipSlot(Mod, typeof(Boxing).Name, EquipType.HandsOff);
			}
		}
	}
}
