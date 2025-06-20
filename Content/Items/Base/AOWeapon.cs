using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOConversion;
using Terraria.ID;
using Terraria.WorldBuilding;
using Microsoft.Xna.Framework.Graphics;

namespace ArcaneOdyssey.Content.Items.Base
{
	public abstract class AOWeapon : ModItem
	{
		public virtual float AOSpeed => 1f;
		public virtual float AOSize => 1f;
		public virtual float AODamage => 1f;
		public virtual int AOValue => 0;
		public virtual int AORarity => AORarities.Common;
		public virtual int AOWeaponTier => AOWeaponTiers.Old;
		public virtual AOMagic? CurrentImbue => null;
		public virtual AODebuff? WeaponDebuff => null;

		public virtual void SetDefaultsWeapon() { }

        public override void SetDefaults()
		{
			Item.useTime = 27;
			Item.knockBack = 4.5f;
			Item.rare = AORarity;
			Item.value = GalleonToCopper(AOValue, Item.rare);
			Item.autoReuse = true;
			Item.useAnimation = 27;
			Item.damage = (int)WeaponDamage(AOWeaponTier);
			Item.DamageType = DamageClass.Melee;
			SetDefaultsWeapon();
		}

		public virtual void ModifyHitNPC2(Player player, NPC target, ref NPC.HitModifiers modifiers) {}

		public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
		{
			AOPlayer playah = player.GetModPlayer<AOPlayer>();
			if (WeaponDebuff.DebuffPercent is null || modifiers.GetDamage(Item.damage, true) > (target.lifeMax/WeaponDebuff.DebuffPercent)) 
			{
				target.AddBuff(WeaponDebuff.debuffID, WeaponDebuff.debuffDuration);
				if (playah.imbue is not null)
				{
					playah.imbue.ApplyDebuffsandStuff(target, modifiers.GetDamage(Item.damage, true), this);
				}
			}
			ModifyHitNPC2(player, target, ref modifiers);
		}
    }

	public abstract class AOMagic : ModItem
	{
		public virtual float AOImbueSpeed => .9f;
		public virtual float AOImbueSize => .9f;
		public virtual float AOImbueDamage => .9f;
		public virtual int MagicTier => AOMagicTier.Normal;
		public virtual AODebuff? MagicDebuff => null;
		public virtual MagicEffects Effects => null;
		public virtual string? ColourCode => null;
		
		public virtual void SetDefaultsMagic() { }

		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.HoldUp;
			SetDefaultsMagic();
		}

		public override bool CanReforge() => false;

		public void ClearBuffs(NPC npc)
		{
			if (Main.netMode == NetmodeID.SinglePlayer || Main.netMode == NetmodeID.Server) 
			{
				if (Effects is not null)
				{
					foreach (int effect in Effects.clearBuffs)
					{
						if (npc.HasBuff(effect))
						{
							npc.DelBuff(npc.FindBuffIndex(effect));
						}
					}
				}
            }
		}

		public virtual void ApplyDebuffsandStuff(NPC npc, int damagedone, AOWeapon weapon)
		{
			if (MagicDebuff.DebuffPercent is null || damagedone > (npc.lifeMax * MagicDebuff.DebuffPercent))
			{
				npc.AddBuff(MagicDebuff.debuffID, MagicDebuff.debuffDuration);
			}
			ClearBuffs(npc);
		}
	}
}
