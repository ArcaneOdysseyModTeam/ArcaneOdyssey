using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using ArcaneOdyssey.Content.Items.Materials;
using Terraria.Audio;

namespace ArcaneOdyssey.Content.Items.Imbues.FightingStyles.Normal
{
	public class Boxing : FightingStyle
	{
		public override float DashSpeed => 1.5f; // instant
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
				new(ModContent.BuffType<FreezingEffect>(),1.15f)
			]
		);

		public override void SpawningEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.BubbleBurst_White, projectile.velocity.X * 2f, projectile.velocity.Y * 2f, 0, default, 3f)];
				spawnedDust.noGravity = true;
			}
		}
		public override void LingeringEffects(Entity projectile)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.BubbleBurst_White, 0f, 0f, 0, default, 1f)];
			spawnedDust.noGravity = true;
		}
		public override void ExplosionEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.BubbleBurst_White, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 3f)];
				spawnedDust.noGravity = true;
			}
		}
		public override void KillEffects(Entity projectile)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.BubbleBurst_White, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), 0, default, 3f)];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, projectile.Center);
		}
		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<BasicCombat>().AddIngredient(ItemID.Silk, 10).AddIngredient<Paper>(5).Register();
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
