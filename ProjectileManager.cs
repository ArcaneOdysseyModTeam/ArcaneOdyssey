using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Magic;
using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;
using static System.Net.Mime.MediaTypeNames;

namespace ArcaneOdyssey
{
	public class ProjectileManager : GlobalProjectile
	{
		public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
		{
			if (projectile.owner == Main.myPlayer && (ArcaneOdysseyConfig.Instance.IgnoredProjectiles is null || !ArcaneOdysseyConfig.Instance.IgnoredProjectiles.Contains(projectile.Name)))
			{
				if (projectile.TryGetImbue(Main.player[projectile.owner], out AOMagic imbue))
				{
					var spell = projectile.ModProjectile is MagicSpell;
                    if (spell)
                        modifiers.FinalDamage += ((projectile.damage + (GetBossKillCount() * 2f)) / projectile.damage) - 1;
                    modifiers.FinalDamage += (!spell ? imbue.AOImbueDamage : imbue.AOMagicDamage).MultiToPercent();
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
							if (target.HasBuff(buffkeys.requirement) || (buffkeys.requirement == BuffID.Wet && target.wet))
							{
								target.AddBuff(buffkeys.result, buffkeys.duration);
							}
						}
					}

					foreach (MagicBuffMultiplier multiplier in imbue.Effects.magicBuffMultipliers)
					{
						if (target.HasBuff(multiplier.buffID) || (multiplier.buffID == BuffID.Wet && target.wet))
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

		public static Dictionary<string, Vector2> OriginalScales = [];

		public override void ModifyDamageHitbox(Projectile projectile, ref Rectangle hitbox)
		{
			if (projectile.owner == Main.myPlayer)
			{
				Player player = Main.LocalPlayer;
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
					float mult = 1f;
					if (projectile.ModProjectile is AOPlayerProjectile proj)
						mult = proj.BaseScale.GetValueOrDefault(1f) + proj.AOSize.MultiToPercent();
					if (projectile.TryGetImbue(player, out AOMagic imbue))
					{
						mult = (projectile.ModProjectile is MagicSpell ? imbue.AOMagicSize : imbue.AOImbueSize).MultiToPercent() + mult + player.ArcaneOdyssey().GetSizeMulti(projectile).MultiToPercent();
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
            if (projectile.ModProjectile is AOPlayerProjectile proj)
            {
                proj.thisMagic ??= Main.player[projectile.owner].ArcaneOdyssey().imbue;
            }
            if (projectile.owner == Main.myPlayer)
                if (projectile.TryGetImbue(Main.LocalPlayer, out AOMagic imbue) && imbue.PreEffects(projectile))
				{
					if (projectile.DamageType != DamageClass.MeleeNoSpeed)
						projectile.velocity *= projectile.ModProjectile is MagicSpell ? imbue.AOMagicSpeed : imbue.AOImbueSpeed;
					AOMagic.CreateMagicCircle(projectile);
					imbue.SpawningEffects(projectile);
				}
		}

		public override void AI(Projectile projectile)
		{
			if (projectile.owner == Main.myPlayer)
			{
				if (projectile.ModProjectile is AOBaseProjectile based)
				{
					based.FramesAlive++;
				}
				if (projectile.TryGetImbue(Main.LocalPlayer, out AOMagic imbue) && imbue.PreEffects(projectile))
				{
					imbue.LingeringEffects(projectile);
				}
			}
		}

		public override void OnKill(Projectile projectile, int timeLeft)
		{
			if (projectile.owner == Main.myPlayer)
			{
				if (projectile.TryGetImbue(Main.LocalPlayer, out AOMagic imbue) && imbue.PreEffects(projectile))
				{
					if (projectile.ModProjectile is not ExplosionSpell && projectile.ModProjectile is not ExplosionTracker)
					{
						imbue.KillEffects(projectile);
					}
				}
			}
		}

		public override bool PreDraw(Projectile projectile, ref Color lightColor)
		{
			bool returntype = true;
			if (Main.player[projectile.owner].ArcaneOdyssey().imbue is PoisonMagic && (projectile.type == ProjectileID.SporeGas || projectile.type == ProjectileID.SporeGas2 || projectile.type == ProjectileID.SporeGas3))
			{
				Main.instance.LoadProjectile(projectile.type);
				var asset = TextureAssets.Projectile[projectile.type];
				Main.EntitySpriteDraw(asset.Value, projectile.Center - Main.screenPosition, null, Color.DarkViolet, projectile.rotation, new Vector2(projectile.height / 2, projectile.height / 2), projectile.scale * 1.12f, SpriteEffects.None);
				returntype = false;
			}

			else if (Main.player[projectile.owner].ArcaneOdyssey().imbue is AshMagic && projectile.type == ProjectileID.SporeCloud)
			{
				Main.instance.LoadProjectile(projectile.type);
				var asset = TextureAssets.Projectile[projectile.type];
				Main.EntitySpriteDraw(asset.Value, projectile.Center - Main.screenPosition, new(0, 30 * projectile.frame, 28, 30), Color.DarkRed, projectile.rotation, new Vector2(projectile.height / 2, projectile.height / 2), projectile.scale, SpriteEffects.None);
				returntype = false;
			}

			return returntype; 
		}
	}
}
