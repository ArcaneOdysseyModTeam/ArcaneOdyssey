using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic.Blasts;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Buffs.DOT;
using Terraria.Audio;

namespace ArcaneOdyssey.Content.Items.FightingStyles
{
	public class ThermoFist : FightingStyle
	{
		public override Color ImbueColour => Color.White;

		public override float AOImbueDamage => 1.075f;
		public override float AOImbueSpeed => 1f;
		public override float AOImbueSize => 1.06f;
		public override float AOScrollDamage => .925f;
		public override float AOScrollSize => 1f;
		public override float AOScrollSpeed => 1f;
	}
}
