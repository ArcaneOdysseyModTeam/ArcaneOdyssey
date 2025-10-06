using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Weapons
{
	public class EaglePatrimony : RelicWeapon
	{
		public override int AOValue => 500;
		public override AORarities AORarity => AORarities.Special;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = Item.height = 40;
			Item.shoot = ModContent.ProjectileType<SpiritBlast>();
			Item.shootSpeed = 15;
			Item.UseSound = SoundID.Item84 with { Pitch = .5f };
			Item.damage = 25;
			Item.useTime = Item.useAnimation = 60;
			Item.knockBack = 3.75f;
		}
	}
}
