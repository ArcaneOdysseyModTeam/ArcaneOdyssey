using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Terraria.ModLoader;
using ArcaneOdyssey.Content.Buffs.Pets;
using UtfUnknown.Core.Probers;
namespace ArcaneOdyssey.Content.Projectiles.Pets
{
    public class ElfPetProjectile : ModProjectile
    {
        private Vector2 targetPosition;
        private bool wasThereABoss = false;
        private bool isThereABoss = false;
        private bool haveICelebrated = false;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 13;
            Main.projPet[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.width = 132;
            Projectile.height = 109;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ai[0] = 0;
            Projectile.frameCounter = 0;
            Projectile.frame = 12;
        }
        public override bool PreAI()
        {
            wasThereABoss = isThereABoss;
            isThereABoss = false;
            foreach (NPC npc in Main.ActiveNPCs)
            {
                if (npc.boss)
                {
                    isThereABoss = true;
                }
            }
            if (!isThereABoss && wasThereABoss)
            {
                wasThereABoss = false;
                isThereABoss = false;
                Projectile.ai[0] = 300;
                Projectile.frame = 0;
                haveICelebrated = false;
            }
            return true;
        }
        public override void AI()
        {
            Projectile.active = true;
            Player player = Main.player[Projectile.owner];
            AOPlayer modPlayer = player.GetModPlayer<AOPlayer>();
            if (modPlayer.elfPet)
            {
                Projectile.timeLeft = 2;
            }
            if (Projectile.ai[0] > 0) {
                wasThereABoss = false;
                targetPosition = player.Center + new Vector2(player.direction * -17f, -30f);
                if (Projectile.Center.X > player.Center.X)
                {
                    Projectile.spriteDirection = -1;
                }
                else
                {
                    Projectile.spriteDirection = 1;
                }
                //Get frame
                if (Projectile.frame > 3)
                {
                    Projectile.frame = 0;
                }
                if (Projectile.frameCounter > 10)
                {
                    Projectile.frameCounter = 0;
                    Projectile.frame++;
                    if (Projectile.frame > 3)
                    {
                        Projectile.frame = 0;
                    }
                    if (Projectile.frame == 3)
                    {
                        if (!haveICelebrated) {
                            // Confetti
                            for (int n = 0;n < 20;n++)
                            {
                                int[] confettis = [DustID.Confetti_Blue,DustID.Confetti_Green,DustID.Confetti_Pink,DustID.Confetti_Yellow];
                                Dust.NewDust(Projectile.Center + new Vector2(0f,-25f),1,1,confettis[(int)Math.Round(Main.rand.NextFloat()*3f)],0,0);
                            }
                            if (ArcaneOdysseyConfig.Instance.ElfPetSoundEffects)
                            {
                                //Audio here
                                Main.NewText("Elf yippee sound effect would be here");
                            }
                            haveICelebrated = true;
                        
                        }
                    }
                }
            } else
            {
                targetPosition = player.Center + new Vector2(player.direction * 30f, -30f);
                Projectile.spriteDirection = player.direction;
                //Get frame
                if (Projectile.frame < 4)
                {
                    Projectile.frame = 4;
                }
                if (Projectile.frameCounter > 10)
                {
                    Projectile.frameCounter = 0;
                    Projectile.frame++;
                    if (Projectile.frame < 4)
                    {
                    Projectile.frame = 4;
                    } else if (Projectile.frame > 11)
                    {
                        Projectile.frame = 4;
                    }
                }
            }
            float targetAngle = Projectile.Center.AngleTo(targetPosition);
            if (Vector2.Distance(Projectile.Center, targetPosition) >= 5)
            {
                Projectile.Center += new Vector2(MathF.Cos(targetAngle), MathF.Sin(targetAngle)) * 5;
            }
            if(Vector2.Distance(Projectile.Center, targetPosition) > 1500)
            {
                Projectile.Center = targetPosition;
                //idk add like teleport dust here
                Main.NewText("Wow what amazing teleport dust");
            } else if (Vector2.Distance(Projectile.Center, targetPosition) > 500)
            {
                Projectile.Center += new Vector2(MathF.Cos(targetAngle), MathF.Sin(targetAngle)) * 15;
            }
            if (Projectile.ai[0] > -1f)
            {
                Projectile.ai[0] -= 1f;
            }
            Projectile.frameCounter++;
        }
    }
}