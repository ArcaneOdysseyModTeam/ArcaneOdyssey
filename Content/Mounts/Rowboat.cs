using ArcaneOdyssey.Content.Buffs.Mounts;
using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Mounts
{
	public class Rowboat : ModMount
	{
		public override void SetStaticDefaults()
		{
			MountData.jumpHeight = 0;
			MountData.acceleration = 1;
			MountData.jumpSpeed = 5;
			MountData.blockExtraJumps = false;
			MountData.constantJump = false;
			MountData.heightBoost = 20;
			MountData.fallDamage = 1.5f;
			MountData.runSpeed = 5f;
			MountData.dashSpeed = 0f;
			MountData.flightTimeMax = 0;

			MountData.fatigueMax = 0;
			MountData.buff = ModContent.BuffType<RowboatBuff>();

			MountData.spawnDust = DustID.PalmWood;

			MountData.totalFrames = 4;
			MountData.playerYOffsets = Enumerable.Repeat(MountData.heightBoost, MountData.totalFrames).ToArray();
			MountData.xOffset = 13;
			MountData.yOffset = -10;
			MountData.playerHeadOffset = 22;
			MountData.bodyFrame = 3;

			MountData.standingFrameCount = 1;
			MountData.standingFrameDelay = 50;
			MountData.standingFrameStart = 0;

			MountData.runningFrameCount = 4;
			MountData.runningFrameDelay = 10;
			MountData.runningFrameStart = 0;

			MountData.flyingFrameCount = 0;
			MountData.flyingFrameDelay = 0;
			MountData.flyingFrameStart = 0;

			MountData.inAirFrameCount = 1;
			MountData.inAirFrameDelay = 12;
			MountData.inAirFrameStart = 0;

			MountData.idleFrameCount = 1;
			MountData.idleFrameDelay = 50;
			MountData.idleFrameStart = 0;
			MountData.idleFrameLoop = true;

			MountData.swimFrameCount = 1;
			MountData.swimFrameDelay = 10;
			MountData.swimFrameStart = 0;

			if (!Main.dedServ)
			{
				MountData.textureWidth = MountData.backTexture.Width();
				MountData.textureHeight = MountData.backTexture.Height();
			}
		}

		public override void UpdateEffects(Player player)
		{
			player.fishingSkill += 15;
			if (player.wet)
			{
				player.velocity = new(0, -20f);
				if (player.position.Y == player.oldPosition.Y)
				{
					player.breathEffectiveness *= 0;
				}
			}
			else
			{
				player.velocity = new Vector2(0, player.maxFallSpeed * MountData.fallDamage);
			}
			player.controlDown = false;
			player.waterWalk = true;
		}

		public override void SetMount(Player player, ref bool skipDust)
		{
			if (Main.LocalPlayer == player)
			{
				SoundEngine.PlaySound(SoundID.Dig);
			}
		}
	}
}
