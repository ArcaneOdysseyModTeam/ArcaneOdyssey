using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.Audio;

namespace ArcaneOdyssey.Content.Items.Imbues.FightingStyles.Normal
{
	public class VanishingStyle : FightingStyle // make barred later
	{
		//public override float DashSpeed => BarValue > (BarMax / 2) ? 1.5f : 1.2f; // instant?
		public override float DashSpeed => 1.2f;

		public override Color ImbueColour => Color.Black;
		public override SoundStyle? ImbueSound => SoundID.Item64;

		public override float AOImbueDamage => 0.9f;
		public override float AOImbueSpeed => 1.2f;
		public override float AOImbueSize => 1.056f;
		public override float AOScrollDamage => .8f;
		public override float AOScrollSize => 1f;
		public override float AOScrollSpeed => 1.2f;

		public override SynergyEffects Effects => new(
			[],
			[
				new MagicBuffMultiplier(ModContent.BuffType<FreezingEffect>(),1.15f)
			]
		);

		public override void SpawningEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Wraith, projectile.velocity.X * 2f, projectile.velocity.Y * 2f, 0, default, 3f)];
				spawnedDust.noGravity = true;
			}
		}

		public override void LingeringEffects(Entity projectile)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Wraith, 0f, 0f, 0, default, 2f)];
			spawnedDust.noGravity = true;
		}

		public override void ExplosionEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Wraith, (Main.rand.NextFloat() - 0.5f) * (35f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (35f * AOScrollSize), 0, default, 3f)];
				spawnedDust.noGravity = true;
			}
		}

		public override void KillEffects(Entity projectile)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Wraith, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), 0, default, 3f)];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, projectile.position, null);
		}

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<BasicCombat>().AddIngredient(ItemID.SoulofNight, 5).Register();
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			base.ModifyTooltips(tooltips);
			var tooltip1 = tooltips.Find(e => e.Text.Contains("{VANISH}"));
			if (tooltip1 is not null && AOKeybinds.Vanish.GetAssignedKeys().Count > 0)
			{
				var index = tooltips.IndexOf(tooltip1);
				tooltip1.Text = tooltip1.Text.Replace("{VANISH}", AOKeybinds.Vanish.GetAssignedKeys()[0]);
				tooltips[index] = tooltip1;
			}
			else if (tooltip1 is not null)
			{
				var index = tooltips.IndexOf(tooltip1);
				tooltip1.Text = tooltip1.Text.Replace("{VANISH}", Mod.CustomLocalization("RandomWords.Unbound").Value);
				tooltips[index] = tooltip1;
			}
		}
	}

	public class VanishingPlayer : ModPlayer
	{
		public override void PreUpdate()
		{
			if (Player.ArcaneOdyssey().Imbue is VanishingStyle && (!Player.ArcaneOdyssey().OnCooldown(ModContent.BuffType<VanishCooldown>())) && AOKeybinds.Vanish.JustPressed) // add more conditions later
			{
				Player.ArcaneOdyssey().SetCooldown(new VanishCooldown());
				Player.AddBuff(BuffID.Invisibility, 60 * 5);
			}
		}
	}

	public class VanishCooldown : DisplayedCooldown
	{
		public override int CooldownLength => 12 * 60;
		public override string ExtraIconTexture => GetType().Namespace.Replace('.', '/') + '/' + nameof(VanishingStyle);
	}
}
