using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Magic;
using ArcaneOdyssey.Content.Projectiles;
using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey
{
    public class ProjectileManager : GlobalProjectile
    {
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (projectile.owner == Main.myPlayer && projectile.owner != 255 && !projectile.hostile && !projectile.npcProj)
            {
                AOPlayer playah = Main.player[projectile.owner].AOPlayer();
                if (ArcaneOdysseyConfig.Instance.IgnoredProjectiles is null || !ArcaneOdysseyConfig.Instance.IgnoredProjectiles.Contains(projectile.Name))
                {
                    if ((projectile.ModProjectile is null or AOPlayerProjectile || ArcaneOdysseyConfig.Instance.AffectsOtherMods) && ImbueClassCheck(projectile))
                    {
                        AOMagic imbue = null;
                        bool spell = false;
                        if (projectile.ModProjectile is AOPlayerProjectile proj)
                        {
                            imbue = proj.thisMagic;
                            spell = proj.IsSpell;
                        }
                        else imbue = playah.imbue;
                        if (spell)
                        {
                            modifiers.FinalDamage.Base += BonusBossKills();
                        }
                        if (imbue is not null)
                        {
                            modifiers.FinalDamage *= !spell ? imbue.AOImbueDamage : imbue.AOMagicDamage;
                            if (imbue is CrystalMagic && target.HasBuff<Crystallized>() && Crystallized.GetCrystalStack(target, target.FindBuffIndex(ModContent.BuffType<Crystallized>())) == 4)
                            {
                                modifiers.FinalDamage += .3f;
                            }

                            if ((imbue.MagicDebuff is not null) && (imbue.MagicDebuff.DebuffPercent != 0f))
                            {
                                if (imbue.MagicDebuff.DebuffPercent is null || modifiers.GetDamage(projectile.damage, true) > (target.lifeMax / imbue.MagicDebuff.DebuffPercent))
                                {
                                    target.AddBuff(imbue.MagicDebuff.debuffID, imbue.MagicDebuff.debuffDuration);
                                }
                            }

                            if ((imbue.MagicDebuff2 is not null) && (imbue.MagicDebuff2.DebuffPercent != 0f))
                            {
                                if (imbue.MagicDebuff2.DebuffPercent is null || modifiers.GetDamage(projectile.damage, true) > (target.lifeMax / imbue.MagicDebuff2.DebuffPercent))
                                {
                                    target.AddBuff(imbue.MagicDebuff2.debuffID, imbue.MagicDebuff2.debuffDuration);
                                }
                            }

                            if (imbue.CombinedDebuffs is not null)
                            {
                                foreach (CombinedDebuff buffkeys in imbue.CombinedDebuffs)
                                {
                                    if (target.HasBuff(buffkeys.requirement))
                                    {
                                        target.AddBuff(buffkeys.result, buffkeys.duration);
                                    }
                                }
                            }

                            foreach (MagicBuffMultiplier multiplier in imbue.Effects.magicBuffMultipliers)
                            {
                                if (target.HasBuff(multiplier.buffID))
                                {
                                    modifiers.FinalDamage += multiplier.multiplier.MultiToPercent();
                                }
                            }

                            if (Main.netMode == NetmodeID.SinglePlayer) // things would get chaotic in multiplayer if everyone kept clearing eachothers debuffs
                            {
                                foreach (int buffid in imbue.Effects.clearBuffs)
                                {
                                    if (target.HasBuff(buffid))
                                    {
                                        target.DelBuff(target.FindBuffIndex(buffid));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public static Dictionary<string, Vector2> OriginalScales = [];

        public override void ModifyDamageHitbox(Projectile projectile, ref Rectangle hitbox)
        {
            if (projectile.owner == Main.myPlayer && projectile.owner != 255 && !projectile.hostile && !projectile.npcProj && projectile.Name != "Falling Star")
            {
                Player player = Main.player[projectile.owner];
                Vector2 dim = new(hitbox.Width, hitbox.Height);
                if (projectile.ModProjectile is AOBaseProjectile origin)
                {
                    dim = origin.OriginalDimensions.GetValueOrDefault(dim);
                }
                else
                {
                    dim = OriginalScales.GetValueOrDefault(projectile.Name, dim);
                }
                if (ImbueClassCheck(projectile))
                {
                    AOMagic imbue = null;
                    float scale = 1f;
                    bool spell = false;
                    if (projectile.ModProjectile is AOPlayerProjectile proj)
                    {
                        imbue = proj.thisMagic;
                        scale = proj.BaseScale.GetValueOrDefault(1f) + proj.AOSize.MultiToPercent();
                        spell = proj.IsSpell;
                    }
                    else
                        imbue = Main.player[projectile.owner].AOPlayer().imbue;
                    float mult = scale;
                    if (imbue is not null)
                    {
                        mult = (spell ? imbue.AOMagicSize : imbue.AOImbueSize).MultiToPercent() + scale + player.AOPlayer().GetSizeMulti(projectile).MultiToPercent();
                    }
                    hitbox.Width = (int)(dim.X * mult);
                    hitbox.Height = (int)(dim.Y * mult);
                    projectile.scale = mult;
                }
            }
        }

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (projectile.ModProjectile is AOBaseProjectile origin)
            {
                origin.OriginalDimensions ??= projectile.Size;
                origin.BaseScale ??= projectile.scale;
            }
            else
            {
                OriginalScales[projectile.Name] = projectile.Size;
            }
            if (projectile.owner == Main.myPlayer && projectile.owner != 255 && !projectile.hostile && !projectile.npcProj && projectile.Name != "Falling Star" && ImbueClassCheck(projectile))
            {
                AOMagic imbue = Main.player[projectile.owner].AOPlayer().imbue;
                bool spell = false;
                if (projectile.ModProjectile is AOPlayerProjectile proj)
                {
                    proj.aoPlayerOwner ??= Main.player[projectile.owner].AOPlayer();
                    proj.thisMagic ??= proj.aoPlayerOwner.imbue;
                    imbue = proj.thisMagic;
                    spell = proj.IsSpell;
                }
                if (imbue is not null)
                    projectile.velocity *= spell ? imbue.AOMagicSpeed : imbue.AOImbueSpeed;


                Player player = Main.player[projectile.owner];
                if ((projectile.ModProjectile is null or AOPlayerProjectile || ArcaneOdysseyConfig.Instance.AffectsOtherMods) && !Main.dedServ && projectile.ModProjectile is not MagicCircle && imbue is not null)
                {
                    AOMagic.CreateMagicCircle(projectile);
                    imbue.SpawningEffects(projectile);
                }
            }
        }

        public override void AI(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            AOPlayer aoPlayerOwner = player.AOPlayer();
            if (projectile.ModProjectile is AOBaseProjectile based)
            {
                based.FramesAlive++;
            }
            if (projectile.ModProjectile is AOPlayerProjectile proj)
            {
                proj.aoPlayerOwner ??= aoPlayerOwner;
                proj.BaseScale ??= projectile.scale;
                if (!Main.dedServ && ImbueClassCheck(projectile))
                    proj.thisMagic?.LingeringEffects(projectile);
                if (aoPlayerOwner is not null)
                {
                    proj.thisMagic ??= aoPlayerOwner.imbue;
                }
            }
            else
            {
                if ((projectile.ModProjectile is null || ArcaneOdysseyConfig.Instance.AffectsOtherMods) && !Main.dedServ && ImbueClassCheck(projectile) && projectile.ModProjectile is not MagicCircle)
                {
                    aoPlayerOwner?.imbue?.LingeringEffects(projectile);
                }
            }
        }

        public override void OnKill(Projectile projectile, int timeLeft)
        {
            Player player = Main.player[projectile.owner];
            AOPlayer aoPlayerOwner = player.AOPlayer();
            if (projectile.ModProjectile is AOPlayerProjectile proj && !Main.dedServ && ImbueClassCheck(projectile) && projectile.ModProjectile is not MagicCircle)
            {
                proj.thisMagic?.KillEffects(projectile);
            }
            else
            {
                if ((projectile.ModProjectile is null || ArcaneOdysseyConfig.Instance.AffectsOtherMods) && !Main.dedServ && ImbueClassCheck(projectile) && projectile.ModProjectile is not MagicCircle)
                {
                    aoPlayerOwner?.imbue?.KillEffects(projectile);
                }
            }
        }
    }
}
