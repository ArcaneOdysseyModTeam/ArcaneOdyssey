using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Base
{
	public abstract class AOBaseItem : ModItem
	{
		public virtual AORarities AORarity => AORarities.RESOLVESELF;
		public virtual ItemType ItemType => ItemType.None;
	}
}
