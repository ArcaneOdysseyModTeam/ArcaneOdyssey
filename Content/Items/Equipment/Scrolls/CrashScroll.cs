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
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.accessory = true;
			Item.damage = 50;
			Item.DamageType = DamageClass.Melee;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			AOPlayer playah = player.ArcaneOdyssey();
			Item.ArcaneOdyssey().imbue = playah.imbue;
			if (playah.imbue is FightingStyle)
			{
				Item.color = playah.imbue.ImbueColour;
				player.DashPlayer().SetDash(new Crash());
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
		public override DamageClass DamageType => DamageClass.Melee;
		public override int Cooldown => 60 * 7;

		public override bool AnyDirection => true;

		public override int Damage => 50;

		public override bool OnHit(Player player, Entity target)
		{
			var gore = Gore.NewGorePerfect(player.GetSource_Misc("Dash"), player.velocity + player.Center, Vector2.Zero, ModContent.GoreType<Impact>(), player.Imbue().AOImbueSize);
			gore.Centre(target.Center);
			return true;
		}
		public override void OnEnd(Player player)
		{
			player.velocity = Vector2.Zero;
		}

		public override float DashSpeed => 15;

		public override int DashMax => 15;

		public override float Knockback => 2f;

		public override bool Immune => true;

		public override void NaturalEnd(Player player)
		{
			var gore = Gore.NewGorePerfect(player.GetSource_Misc("Dash"), player.velocity + player.MountedCenter, Vector2.Zero, ModContent.GoreType<Impact>(), player.Imbue().AOImbueSize);
			gore.Centre(player.MountedCenter + player.velocity);
			player.DashPlayer().StartDash(new Smash(), 2);
		}

		public override void OnStart(Player player)
		{
			if (player.TryGetImbue(out Imbuable imbue))
			{
				player.DashPlayer().DashVelocity *= imbue.AOScrollSpeed;
			}
		}
	}

	public class Smash : DashSystem
	{
		public override DamageClass DamageType => DamageClass.Melee;
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
				player.DashPlayer().DashVelocity *= imbue.AOScrollSpeed;
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
				if (npc.Center.Distance(player.MountedCenter) < Player.defaultHeight * 2 && !npc.friendly && npc.immune[player.whoAmI] <= 0)
				{
					npc.SimpleStrikeNPC(Damage, (player.MountedCenter.X - npc.Center.X > 0).ToDirectionInt(), knockBack: Knockback, damageType: DamageType);
				}
			}
			player.ArcaneOdyssey().timeTillNextMove += 15;
		}
	}
}
