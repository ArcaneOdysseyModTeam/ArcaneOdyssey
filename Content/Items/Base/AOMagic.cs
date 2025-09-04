using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Base
{

    /// <summary>
    /// Imbue values are applied as multipliers to imbued projectiles,
    /// Magic values are applied as multipliers to projectiles created using spell scrolls
    /// </summary>
    public abstract class AOMagic : ModItem
    {
        /// <summary>
        /// magic works underwater
        /// </summary>
        public virtual bool CanBeWet => true;
        public virtual float AOImbueSpeed => .9f;
        public virtual float AOImbueSize => .9f;
        public virtual float AOImbueDamage => .9f;
        public virtual float AOMagicSpeed => AOImbueSpeed;
        public virtual float AOMagicSize => AOImbueSize;
        public virtual float AOMagicDamage => AOImbueDamage;
        public virtual AOMagicTier MagicTier => AOMagicTier.Normal;
        public virtual AODebuff MagicDebuff => null;

        /// <summary>
		/// used for having freezing and frozen on a single magic ect
		/// </summary>
        public virtual AODebuff MagicDebuff2 => null;
        public virtual MagicEffects Effects => null;
        public virtual Color MagicColour => Color.Transparent;
        public virtual CombinedDebuff[] CombinedDebuffs => null;

        public virtual Dictionary<Type, int> Spells => [];

        public bool FirstFrame = true;

        public virtual void SetDefaultsMagic() { }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.DrinkOld;
            Item.useTime = 50;
            Item.useAnimation = 50;
            Item.noUseGraphic = true;
            SetDefaultsMagic();
        }

        public override bool CanUseItem(Player player)
        {
            FirstFrame = true;
            return base.CanUseItem(player);
        }



        public override bool CanReforge() => false;

        public virtual void SpawningDust(Projectile projectile) { }
        public virtual void LingeringDust(Projectile projectile) { }
        public virtual void KillDust(Projectile projectile) { }
        public void CreateMagicCircle(Projectile projectile) {
            if(projectile.ModProjectile is BlastSpell){
                Projectile circleprojectile = Main.projectile[Projectile.NewProjectile(null,Main.player[projectile.owner].position.X+((float)Main.player[projectile.owner].width/2f),Main.player[projectile.owner].position.Y+((float)Main.player[projectile.owner].height/2f),0f,0f,ModContent.ProjectileType<MagicCircle>(),0,0f,255,0f,0f)];
                circleprojectile.rotation = projectile.velocity.ToRotation();
                Vector2 circleVec = Vector2.Normalize(projectile.velocity)*15f;
                circleprojectile.position = circleprojectile.position+circleVec;
                circleprojectile.owner = projectile.owner;
            }
        }
        // Dust stuff below for copy/paste
        // Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X+(projectile.width*(float)rand.NextDouble()),projectile.position.Y+(projectile.height*(float)rand.NextDouble())),1,1,DustID.Water,0f,0f,0,default,1f)];
    }
}