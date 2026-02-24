using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.PlayerClasses;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Scrolls.Equipment.Rare
{
	public class BreathtakerTechnique : RareScroll
	{
		public override bool CanHaveFS => true;
		public const int Cooldown = 60 * 10;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.accessory = true;
			Item.damage = 20;
			Item.DamageType = AOUtils.TrueMeleeNoSpeed();
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			base.ModifyTooltips(tooltips);
			tooltips.RemoveAll((TooltipLine line) => line.Name == "Speed");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (HasCorrectImbue)
			{
				player.ArcaneOdyssey()?.SetDash(new Breathtaker(Item));
			}
		}
	}

	public class Breathtaker(Entity source) : DashSystem(source)
	{
		public override bool Immune => true;
		public override float DashSpeed => 120;
		public override int DashMax => 2;
		public override bool AnyDirection => true;
		public override int Cooldown => BreathtakerTechnique.Cooldown;
		public override bool OnHit(Player player, Entity target)
		{
			if (target is NPC npc)
			{
				npc.ArcaneOdyssey().DefenseLost += 2;
			}
			return true;
		}

		public override void OnEnd(Player player)
		{
			player.velocity *= .01f;
		}

		public override void OnStart(Player player)
		{
			SoundEngine.PlaySound(SoundID.Item67);
		}

		public override int DisplayedCooldownID => ModContent.BuffType<BreathtakerCooldown>();
	}

	public class BreathtakerCooldown : DisplayedCooldown
	{
		public override string ExtraIconTexture => AOUtils.GetTexture<BreathtakerTechnique>();
	}
}
