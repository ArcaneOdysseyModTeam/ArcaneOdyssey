using ArcaneOdyssey.Buffs.Base;
using ArcaneOdyssey.Imbues;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Magic.Normal;
using System.Collections.Generic;
using System.Linq;
using Terraria.DataStructures;

namespace ArcaneOdyssey.Buffs.DOT
{
	public class Corroding : MagicMark
	{
		public override List<int> Counterparts => [BuffID.Venom];

		public int dust = DustID.Water_Corruption;
		public Color dustColour = default;
		public Color drawColour = default;

		public void GetVFXInfo(NPC npc)
		{
			foreach (var imbueID in ImbueID.Sets.ImbuesThatCreateBuff[Type])
			{
				if (AOUtils.Singleplayer)
				{
					if (Main.LocalPlayer.HasTypeInInventory<MagicType>(out var imbue, e => e.ID == imbueID && e.magicVariant is not null))
					{
						imbue.magicVariant.BuffVisuals(ref dust, ref drawColour, ref dustColour);
					}
				}
				else
				{
					var players = Main.player.Where(e => e is not null && e.active).OrderBy(e => e.Distance(npc.Center));
					foreach (var player in players)
					{
						if (player.HasTypeInInventory<MagicType>(out var imbue, e => e.ID == imbueID && e.magicVariant is not null))
						{
							imbue.magicVariant.BuffVisuals(ref dust, ref drawColour, ref dustColour);
							break;
						}
					}
				}
			}
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.ArcaneOdyssey().corroding = true;
			if (!Main.dedServ)
			{
				if (!vfxSet)
				{
					dustColour = default;
					dust = DustID.Water_Corruption;
					drawColour = default;
					GetVFXInfo(npc);
					vfxSet = true;
				}
				var spawned = Dust.NewDustDirect(npc.position, npc.width, npc.height, dust);
				spawned.velocity *= 0.4f;
			}
		}

		public bool vfxSet = false;

		public override bool ReApply(NPC npc, int time, int buffIndex)
		{
			vfxSet = false;
			return false;
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ExternalModSupport.RegisterDoT(Type);
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.venom = true;
		}

		public static Asset<Texture2D> GlowTexture;

		public override void PostDraw(SpriteBatch spriteBatch, int buffIndex, BuffDrawParams drawParams)
		{
			if (drawColour != default && AOUtils.RequestIfExists(Texture + "_Glow", ref GlowTexture))
			{
				if (GlowTexture.IsLoaded)
				{
					drawParams.DrawColor = drawParams.DrawColor.MultiplyRGBA(drawColour);
					drawParams.Texture = GlowTexture.Value;
					spriteBatch.Draw(drawParams.Texture, drawParams.MouseRectangle, drawParams.SourceRectangle, drawParams.DrawColor);
				}
			}
		}
	}
}
