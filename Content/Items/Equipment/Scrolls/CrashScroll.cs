using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.VFX.Gores;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using static ArcaneOdyssey.AOUtils;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Equipment.Scrolls
{
	public class CrashScroll : TechniqueScroll
	{
		public const int Cooldown = 60 * 7;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.accessory = true;
			Item.damage = 50;
			Item.DamageType = TrueMeleeNoSpeed();
			Item.useTime = Cooldown;
		}

		public override void ModifyWeaponCrit(Player player, ref float crit) => crit *= 0;

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			AOPlayer playah = player.ArcaneOdyssey();
			Item.ArcaneOdyssey().imbue = playah.imbue;
			if (playah.imbue is FightingStyle)
			{
				Item.color = playah.imbue.ImbueColour;
				player.ArcaneOdyssey().SetDash(new Crash());
			}
			else Item.color = Color.Transparent;

		}
		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<EmptyScroll>().AddIngredient(ItemID.ClimbingClaws).Register();
			CreateRecipe().AddIngredient<EmptyScroll>().AddIngredient(ItemID.ShoeSpikes).Register();
		}
	}

	public class Crash : DashSystem
	{
		public override DamageClass DamageType => TrueMeleeNoSpeed();
		public override int Cooldown => CrashScroll.Cooldown;

		public override bool AnyDirection => true;

		public override int Damage => 50;
		public override void DashEffect(Player player)
		{
			if (player.TryGetImbue(out var imbue))
			{
				imbue.LingeringEffects(player);
			}
		}

		public override bool OnHit(Player player, Entity target)
		{
			var gore = Gore.NewGorePerfect(player.GetSource_Misc("Dash"), target.Center, Vector2.Zero, ModContent.GoreType<Impact>(), player.Imbue().AOImbueSize);
			gore.Centre(target.Center);
			return true;
		}

		public override void OnEnd(Player player)
		{
			player.velocity = Vector2.Zero;
			SoundEngine.PlaySound(SoundID.Item14 with { Pitch = -.25f }, player.MountedCenter + player.velocity);
			if (player.TryGetImbue(out var imbue))
			{
				for (int i = 0; i < 10; i++)
					imbue.ExplosionEffects(player);
			}
		}

		public override float DashSpeed => 15;

		public override int DashMax => 15;

		public override float Knockback => 2f;

		public override bool Immune => true;

		public override void NaturalEnd(Player player)
		{
			var gore = Gore.NewGorePerfect(player.GetSource_Misc("Dash"), player.velocity + player.MountedCenter, Vector2.Zero, ModContent.GoreType<Impact>(), player.Imbue().AOImbueSize);
			gore.Centre(player.MountedCenter + player.velocity);
			player.ArcaneOdyssey().StartDash(new Smash(), 2);
		}

		public override void OnStart(Player player)
		{
			if (player.TryGetImbue(out Imbuable imbue))
			{
				player.ArcaneOdyssey().DashVelocity *= imbue.AOScrollSpeed;
			}
		}
	}

	public class Smash : DashSystem
	{
		public override DamageClass DamageType => TrueMeleeNoSpeed();
		public override bool AnyDirection => true;

		public override int Damage => 50;
		public override int Cooldown => 0;

		public override float DashSpeed => 10;

		public override int DashMax => 99999;
		public override float Knockback => 0;
		public override bool Immune => true;

		public override void OnStart(Player player)
		{
			if (player.TryGetImbue(out Imbuable imbue))
			{
				player.ArcaneOdyssey().DashVelocity *= imbue.AOScrollSpeed;
			}
		}
		public override bool OnHit(Player player, Entity target)
		{
			var gore = Gore.NewGorePerfect(player.GetSource_Misc("Dash"), player.velocity + player.Center, Vector2.Zero, ModContent.GoreType<Impact>(), player.Imbue().AOImbueSize);
			gore.Centre(target.Center);
			return false;
		}

		public override void OnEnd(Player player)
		{
			var gore = Gore.NewGorePerfect(player.GetSource_Misc("Dash"), player.velocity + player.MountedCenter, Vector2.Zero, ModContent.GoreType<Impact>(), player.Imbue().AOImbueSize);
			gore.Centre(player.Bottom);

			foreach (NPC npc in Main.ActiveNPCs)
			{
				if (npc.Hitbox.Distance(player.MountedCenter) < Player.defaultHeight * 2 && !npc.friendly && npc.immune[player.whoAmI] <= 0)
				{
					npc.SimpleStrikeNPC(player.ArcaneOdyssey().DashDamage(npc), (player.MountedCenter.X - npc.Center.X > 0).ToDirectionInt(), knockBack: Knockback, damageType: DamageType);
				}
			}
			player.ArcaneOdyssey().timeTillNextMove += 15;
			SoundEngine.PlaySound(SoundID.Item14 with { Pitch = -.25f }, player.MountedCenter + player.velocity);
			if (player.TryGetImbue(out var imbue))
			{
				for (int i = 0; i < 20; i++)
					imbue.ExplosionEffects(player);
			}
		}

		public override void DashEffect(Player player)
		{
			if (player.TryGetImbue(out var imbue))
			{
				imbue.LingeringEffects(player);
			}
		}
	}
}
