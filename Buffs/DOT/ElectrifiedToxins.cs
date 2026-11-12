using ArcaneOdyssey.Buffs.Base;
using ArcaneOdyssey.Dusts;
using ArcaneOdyssey.Imbues;
using ArcaneOdyssey.Imbues.Base;
using System.Linq;
using Terraria.DataStructures;

namespace ArcaneOdyssey.Buffs.DOT
{
	public class ElectrifiedToxins : MagicMark
	{
		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.ArcaneOdyssey().elecToxins = true;
			if (!Main.dedServ)
			{
				if (!vfxSet)
				{
					GetVFXInfo(npc);
					vfxSet = true;
				}
				var spawnedDust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.SteampunkSteam, newColor: dustColour);
				spawnedDust.velocity *= 0.4f;
				Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<TintableElectric>(), newColor: dustColour);
			}
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ExternalModSupport.RegisterDoT(Type);
		}
		private bool vfxSet = false;
		public Color dustColour = Color.Purple;

		public void GetVFXInfo(NPC npc)
		{
			foreach (var imbueID in ImbueID.Sets.ImbuesThatCreateBuff[Type])
			{
				var _ = 0;
				var _1 = Color.White;
				if (AOUtils.Singleplayer)
				{
					if (Main.LocalPlayer.HasTypeInInventory<MagicType>(out var imbue, e => e.ID == imbueID && e.magicVariant is not null))
					{
						imbue.magicVariant.BuffVisuals(ref _, ref _1, ref dustColour);
					}
				}
				else
				{
					var players = Main.player.Where(e => e is not null && e.active).OrderBy(e => e.Distance(npc.Center));
					foreach (var player in players)
					{
						if (player.HasTypeInInventory<MagicType>(out var imbue, e => e.ID == imbueID && e.magicVariant is not null))
						{
							imbue.magicVariant.BuffVisuals(ref _, ref _1, ref dustColour);
							break;
						}
					}
				}
			}
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.ArcaneOdyssey().debuffs.Add(20);
			if (!Main.dedServ)
			{
				var spawnedDust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.SteampunkSteam, newColor: dustColour);
				spawnedDust.velocity *= 0.4f;
				Dust.NewDust(player.position, player.width, player.height, ModContent.DustType<TintableElectric>(), newColor: dustColour);
			}
		}

		public override bool ReApply(NPC npc, int time, int buffIndex)
		{
			vfxSet = false;
			return base.ReApply(npc, time, buffIndex);
		}

		public static Asset<Texture2D> GlowTexture;

		public override void PostDraw(SpriteBatch spriteBatch, int buffIndex, BuffDrawParams drawParams)
		{
			if (dustColour != Color.Purple && AOUtils.RequestIfExists(Texture + "_Glow", ref GlowTexture))
			{
				if (GlowTexture.IsLoaded)
				{
					drawParams.DrawColor = drawParams.DrawColor.MultiplyRGBA(dustColour);
					drawParams.Texture = GlowTexture.Value;
					spriteBatch.Draw(drawParams.Texture, drawParams.MouseRectangle, drawParams.SourceRectangle, drawParams.DrawColor);
				}
			}
		}
	}
}
