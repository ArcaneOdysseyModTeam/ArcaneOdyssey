using ArcaneOdyssey.Buffs.Base;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace ArcaneOdyssey.Buffs
{
	public class InsanityOne : BaseBuff
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			Main.debuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
			Main.pvpBuff[Type] = true;
			BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
			Main.buffNoSave[Type] = true;
			ExternalModSupport.RegisterDebuff(this);
		}

		public override string Texture => AOUtils.GetTexture<InsanityOne>();

		public virtual byte BanishmentReq => 2;

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ArcaneOdyssey().Banishment < BanishmentReq)
			{

			}
		}
	}

	public class InsanityTwo : InsanityOne
	{
		public override byte BanishmentReq => (byte)(base.BanishmentReq + 1);
		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ArcaneOdyssey().Banishment < BanishmentReq)
			{
				base.Update(player, ref buffIndex);
			}
		}
	}

	public class InsanityThree : InsanityTwo
	{
		public override byte BanishmentReq => (byte)(base.BanishmentReq + 1);
		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ArcaneOdyssey().Banishment < BanishmentReq)
			{
				base.Update(player, ref buffIndex);
			}
		}
	}

	public class InsanityFour : InsanityThree
	{
		public override byte BanishmentReq => (byte)(base.BanishmentReq + 1);
		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ArcaneOdyssey().Banishment < BanishmentReq)
			{
				base.Update(player, ref buffIndex);
				player.ArcaneOdyssey().debuffs.Add(100);
			}
		}
	}

	public class InsanityFive : InsanityFour
	{
		public override byte BanishmentReq => (byte)(base.BanishmentReq + 1);
		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ArcaneOdyssey().Banishment < BanishmentReq)
			{
				base.Update(player, ref buffIndex);
				player.ArcaneOdyssey().debuffs.Add(100);
			}
		}
	}
}
