using ArcaneOdyssey.Content.Buffs.Mounts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
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
			MountData.runSpeed = 5f; // only works on water lol
			MountData.dashSpeed = 0f;
			MountData.flightTimeMax = 0;

			MountData.fatigueMax = 0;
			MountData.buff = ModContent.BuffType<RowboatBuff>();

			MountData.spawnDust = DustID.WoodFurniture;

			MountData.totalFrames = 4;
			MountData.playerYOffsets = Enumerable.Repeat(MountData.heightBoost, MountData.totalFrames).ToArray();
			MountData.xOffset = 13;
			MountData.yOffset = -12;
			MountData.playerHeadOffset = 22;
			MountData.bodyFrame = 3;

			MountData.standingFrameCount = 1;
			MountData.standingFrameDelay = 50;
			MountData.standingFrameStart = 0;

			MountData.runningFrameCount = 4;
			MountData.runningFrameDelay = 50;
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

			MountData.swimFrameCount = 4;
			MountData.swimFrameDelay = 50;
			MountData.swimFrameStart = 0;

			if (Main.netMode != NetmodeID.Server)
			{
				MountData.textureWidth = MountData.backTexture.Width();
				MountData.textureHeight = MountData.backTexture.Height();
			}
		}

		public override void UpdateEffects(Player player)
		{
			player.fishingSkill += 15;
			if (!player.wet)
			{
				player.velocity = new Vector2(0, player.maxFallSpeed * MountData.fallDamage);
			}
			else
			{
				player.maxFallSpeed = 0;
			}
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
