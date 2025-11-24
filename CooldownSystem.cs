using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ArcaneOdyssey
{
	public abstract class DisplayedCooldown : ModBuff, ILocalizedModType
	{
		public override string Texture => Mod.Name + "/Assets/Debuff";
		public virtual string ExtraIconTexture => null;

		public override void SetStaticDefaults()
		{
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
		}

		public int TypeID
		{
			get
			{
				if (ArcaneOdysseyMod.Instance.TryFind<ModBuff>(Name, out var cooldown))
				{
					return cooldown.Type;
				}
				else
					return Type;
			}
		}

		public override void PostDraw(SpriteBatch spriteBatch, int buffIndex, BuffDrawParams drawParams)
		{
			if (ExtraIconTexture is not null)
			{
				if (ModContent.RequestIfExists<Texture2D>(ExtraIconTexture, out var tex))
				{
					spriteBatch.Draw(tex.Value, drawParams.MouseRectangle with { Height = drawParams.MouseRectangle.Height - (drawParams.MouseRectangle.Height / 32 * 4), Width = drawParams.MouseRectangle.Width - (drawParams.MouseRectangle.Width / 32 * 4), X = drawParams.MouseRectangle.X + (drawParams.MouseRectangle.Width / 32 * 2), Y = drawParams.MouseRectangle.Y + (drawParams.MouseRectangle.Height / 32 * 2) }, drawParams.DrawColor);
				}
			}
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
		private List<Cooldown> toremove = [];
		private Dictionary<int, Cooldown> tochange = [];
		public override void PreUpdate()
		{
			if (timeTillNextMove > 1)
			{
				for (int i = 0; i < 4; i++)
					Player.doubleTapCardinalTimer[i] = 0;
				timeTillNextMove--;
			}
			else timeTillNextMove = 0;

			foreach (var Cooldown in tochange)
			{
				Cooldowns[Cooldown.Key] = Cooldown.Value;
			}

			foreach (var Cooldown in Cooldowns)
			{
				var cool = Cooldown;
				if (--cool.cooldownRemaining <= 0 || ArcaneOdysseyMod.devMode)
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

		public void SetCooldown(DisplayedCooldown cooldown)
		{
			Player.AddBuff(cooldown.TypeID, cooldown.CooldownLength);
		}

		public void SetCooldown(int cooldown, int length)
		{
			Player.AddBuff(cooldown, length);
		}
	}
}
