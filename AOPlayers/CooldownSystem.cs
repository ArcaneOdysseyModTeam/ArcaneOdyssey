using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using ArcaneOdyssey.Items.Weapons.RavennaNoble;
using Microsoft.Xna.Framework;

namespace ArcaneOdyssey.AOPlayers
{
	public abstract class DisplayedCooldown : ModBuff, ILocalizedModType
	{
		public override void SetStaticDefaults()
		{
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
		}

		public static Asset<Texture2D> debuffBackground;

		public override bool PreDraw(SpriteBatch spriteBatch, int buffIndex, ref BuffDrawParams drawParams)
		{
			var ogrect = drawParams.MouseRectangle;
			drawParams.MouseRectangle.Width = (32 * (drawParams.MouseRectangle.Width / (float)Math.Max(drawParams.Texture.Width, drawParams.Texture.Height))).Round();
			drawParams.MouseRectangle.Height = (32 * (drawParams.MouseRectangle.Height / (float)Math.Max(drawParams.Texture.Width, drawParams.Texture.Height))).Round();

			var scaledx = (drawParams.Texture.Width * (drawParams.MouseRectangle.Width / (float)Math.Max(drawParams.Texture.Width, drawParams.Texture.Height))) - drawParams.MouseRectangle.Width;
			var scaledy = (drawParams.Texture.Height * (drawParams.MouseRectangle.Height / (float)Math.Max(drawParams.Texture.Width, drawParams.Texture.Height))) - drawParams.MouseRectangle.Height;

			drawParams.TextPosition.Y = (32 * (ogrect.Height / (float)drawParams.Texture.Height)) + drawParams.Position.Y;

			if (AOUtils.RequestIfExists(AOUtils.DebuffTexture, ref debuffBackground))
			{
				spriteBatch.Draw(debuffBackground.Value, drawParams.Position, null, drawParams.DrawColor, 0f, default, Math.Max(ogrect.Height, ogrect.Width) / (float)Math.Max(drawParams.Texture.Width, drawParams.Texture.Height), SpriteEffects.None, 0f);
			}

			spriteBatch.Draw(drawParams.Texture, drawParams.Position - (new Vector2(scaledx, scaledy) * 1.5f), null, drawParams.DrawColor, 0f, default, Math.Max(drawParams.MouseRectangle.Width / (float)drawParams.Texture.Width, drawParams.MouseRectangle.Height / (float)drawParams.Texture.Height), SpriteEffects.None, 0f);

			if (this is TwinCrecsentsCooldown)
			{
				spriteBatch.Draw(drawParams.Texture, drawParams.Position - (new Vector2(scaledx, scaledy) * 1.5f), null, drawParams.DrawColor, 0f, default, Math.Max(drawParams.MouseRectangle.Width / (float)drawParams.Texture.Width, drawParams.MouseRectangle.Height / (float)drawParams.Texture.Height), SpriteEffects.FlipHorizontally, 0f);
			}

			drawParams.MouseRectangle.Width = (32 * (ogrect.Width / (float)drawParams.Texture.Width)).Round();
			drawParams.MouseRectangle.Height = (32 * (ogrect.Height / (float)drawParams.Texture.Height)).Round();

			return false;
		}

		public virtual int CooldownLength => 0;

		public override string LocalizationCategory => "Cooldowns";

		public override LocalizedText Description => Language.GetOrRegister(Mod.GetLocalizationKey($"{LocalizationCategory}.{Name}.Description"), () => $"{DisplayName.Value.Replace(" Cooldown", null)} is on cooldown");
	}

	public struct Cooldown
	{
		public string ID;
		public LocalizedText Name;
		public int cooldownRemaining;

		public Cooldown(string ID, LocalizedText Name, int CooldownLength)
		{
			this.ID = ID;
			this.Name = Name;
			cooldownRemaining = CooldownLength;
		}

		public Cooldown(string ID, Mod mod, string Name, int CooldownLength)
		{
			this.ID = ID;
			this.Name = Language.GetOrRegister(mod.GetLocalizationKey("Cooldowns." + ID), () => Name);
			cooldownRemaining = CooldownLength;
		}

		public Cooldown(string ID, Mod mod, int CooldownLength)
		{
			this.ID = ID;
			Name = mod.CustomLocalization("Cooldowns." + ID);
			cooldownRemaining = CooldownLength;
		}
	}

	public partial class AOPlayer : ModPlayer, IImbuable
	{
		public int StatHaste;
		private List<Cooldown> toremove = [];
		private Dictionary<int, Cooldown> tochange = [];
		public override void PreUpdate()
		{
			if (timeTillNextMove > 0)
			{
				for (int i = 0; i < 4; i++)
					Player.doubleTapCardinalTimer[i] = 0;
				timeTillNextMove--;
			}

			foreach (var Cooldown in tochange)
			{
				Cooldowns[Cooldown.Key] = Cooldown.Value;
			}

			foreach (var Cooldown in Cooldowns)
			{
				var cool = Cooldown;
				if (--cool.cooldownRemaining <= 0 || ArcaneOdysseyMod.DevMode)
				{
					if (OnCooldown(Cooldown.ID) && !toremove.Contains(Cooldown))
						toremove.Add(Cooldown);
				}
				else
				{
					if (OnCooldown(Cooldown.ID))
						tochange[Cooldowns.IndexOf(Cooldown)] = cool;
				}
			}

			foreach (var Cooldown in tochange)
			{
				Cooldowns[Cooldown.Key] = Cooldown.Value;
			}
			foreach (var Cooldown in toremove)
			{
				Cooldowns.Remove(Cooldown);
			}
			tochange = [];
			toremove = [];
		}

		public bool OnCooldown(string ID) => GetCooldown(ID).ID is not null;

		public bool OnCooldown(int ID) => Player.HasBuff(ID);


		public bool OnCooldown<T>() where T : DisplayedCooldown
		{
			return Player.HasBuff<T>();
		}

		public Cooldown GetCooldown(string ID)
		{
			return Cooldowns.Find(e => e.ID == ID);
		}

		public void SetCooldown(Cooldown cooldown)
		{
			if (OnCooldown(cooldown.ID))
			{
				tochange[Cooldowns.IndexOf(GetCooldown(cooldown.ID))] = cooldown;
			}
			else
			{
				Cooldowns.Add(cooldown);
			}
		}

		public void SetCooldown(int cooldown, int length)
		{
			Player.AddBuff(cooldown, (length * CooldownDurationMulti).Round());
		}

		public void SetCooldown<T>(int length = -1) where T : DisplayedCooldown
		{
			if (length == -1)
			{
				length = ModContent.GetInstance<T>().CooldownLength;
			}
			SetCooldown(ModContent.BuffType<T>(), length);
		}
	}
}
