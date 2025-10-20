using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Projectiles.Berserker;
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

namespace ArcaneOdyssey.Content.Items.Weapons.Scrolls
{
    public class SmashScroll : TechniqueScroll
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.useTime = Item.useAnimation = 30;
            Item.damage = 50;
            Item.shoot = ModContent.ProjectileType<ShockwaveSmash>();
            Item.DamageType = DamageClass.Melee;
            Item.shootSpeed = 5;
        }

        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient<EmptyScroll>().AddIngredient(ItemID.BatBat).Register();
        }
    }
}
