using ArcaneOdyssey.DamageClasses;
using ArcaneOdyssey.Items.Scrolls;
using ArcaneOdyssey.Skills.Base;
using System.IO;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;

namespace ArcaneOdyssey.Items.Base
{
	public abstract class ArcaniumWeapon : Weapon
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ArcaneOdysseyMod.Sets.weaponType[Type] = WeaponType.Arcanium;
			ItemID.Sets.GamepadWholeScreenUseRange[Type] = true;
			ItemID.Sets.LockOnIgnoresCollision[Type] = true;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.DamageType = ModContent.GetInstance<ArcaniumDamageClass>();
			Item.shoot = ProjectileID.BouncyGlowstick;
			Item.autoReuse = true;
		}

		public sealed override Color Motif => Color.White;

		public AttackSkill Attack;

		private int actualShoot = 0;

		internal string cachedSpell = null;

		public Item InstanceItem => new(Type);

		public override void UpdateInventory(Player player)
		{
			base.UpdateInventory(player);
			Item.color = Colour;
		}

		public override bool CanUseItem(Player player)
		{
			if (Imbue is not null && Attack is not null && !Attack.PreActivate(player, Imbue))
			{
				return false;
			}
			return base.CanUseItem(player);
		}

		public void RemoveSkill()
		{
			if (Attack is null)
			{
				if (!string.IsNullOrWhiteSpace(cachedSpell))
				{
					var item = Main.LocalPlayer.QuickSpawnItemDirect(Item.GetSource_FromThis(), ModContent.ItemType<UnloadedScroll>()).ModItem as UnloadedScroll;
					item.CachedFullName = cachedSpell;
					cachedSpell = null;
				}
			}
			else
			{
				if (Attack.Scroll != 0)
					Main.LocalPlayer.QuickSpawnItem(Item.GetSource_FromThis(), Attack.Scroll);
			}

			Attack = null;

			Item.noMelee = InstanceItem.noMelee;
			Item.noUseGraphic = InstanceItem.noUseGraphic;
			Item.mana = InstanceItem.mana;
			Item.damage = InstanceItem.damage;
			Item.knockBack = InstanceItem.knockBack;
			Item.channel = InstanceItem.channel;
			Item.useAnimation = InstanceItem.useAnimation;
			Item.useTime = InstanceItem.useTime;
			actualShoot = 0;
			Item.shootSpeed = InstanceItem.shootSpeed;
			Item.useStyle = InstanceItem.useStyle;
		}


		public void SetSkill(AttackSkill skill, bool refund = true)
		{
			if (refund)
				RemoveSkill();

			Attack = skill;
			if (skill is not null)
			{
				Item.noMelee = true;
				Item.noUseGraphic = true;
				Item.mana = skill.ManaCost;
				Item.damage = skill.Damage;
				Item.knockBack = skill.Knockback;
				Item.channel = skill.Channel;
				Item.useAnimation = Item.useTime = skill.Time;
				actualShoot = skill.Shoot;
				Item.shootSpeed = skill.Speed;
				Item.useStyle = skill.UseStyleID;
			}
		}

		public override void NetSend(BinaryWriter writer)
		{
			base.NetSend(writer);
			writer.Write(Attack?.Type);
		}

		public override void NetReceive(BinaryReader reader)
		{
			base.NetReceive(reader);
			Attack = ModSkill.Sets.All[reader.ReadInt32()] as AttackSkill;
		}

		public override void SaveData(TagCompound tag)
		{
			base.SaveData(tag);
			if (Attack is not null || cachedSpell is not null)
			{
				tag.Add("attack", Attack?.FullName ?? cachedSpell);
			}
		}

		public override void LoadData(TagCompound tag)
		{
			base.LoadData(tag);

			var attack = tag.GetString("attack");

			if (!string.IsNullOrWhiteSpace(attack))
			{
				if (!ModContent.TryFind(attack, out Attack))
				{
					cachedSpell = attack;
				}
			}

			SetSkill(Attack, false);
		}

		public override bool CanShoot(Player player)
		{
			if (Imbue is not null)
			{
				return Attack?.PreActivate(player, Imbue) ?? true;
			}
			return true;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (Imbue is not null && Attack is not null)
			{
				if (actualShoot != 0)
					type = actualShoot;
				Attack.AttackStats(player, Imbue, ref position, ref velocity, ref damage, ref knockback);
			}
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (Imbue is not null && Attack != null)
			{
				return Attack.Attack(player, Imbue, source, position, velocity, damage, knockback);
			}

			return type != ProjectileID.BouncyGlowstick;
		}

		public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
		{
			if (Imbue is not null)
			{
				Attack?.ModifyManaCost(player, ref reduce, ref mult);
			}
		}
	}
}
