using ArcaneOdyssey.AOPlayers;
using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Buffs.Stuns;
using ArcaneOdyssey.Imbues.Magic.Normal;
using ArcaneOdyssey.Projectiles.Relics;
using ArcaneOdyssey.VFX.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Imbues.Relics
{
	public class EaglePatrimony : SpiritEnergy
	{
		public override float UnstableSize => 1.2f;
		public override float UnstableSpeed => .9f;
		public override int UnstableDrawback => 2;

		public override float SynergyDamage => 1.15f;
		public override float SynergySize => .8f;
		public override float SynergySpeed => 1.2f;

		public override GodSoulID[] SoulSynergies => [GodSoulID.Athena];
		public override GodSoulID[] UnstableSouls => [GodSoulID.Poseidon];


		public override Color ImbueColour => new(0, 255, 251);
		public override AORarities AORarity => AORarities.Special;
		public override SoundStyle? ImbueSound => SoundID.DD2_LightningBugZap with { Volume = 2.25f };

		public override Combo[] CombinedDebuffs => [Combo.Create<Soaked, Paralyzed>()];

		public override Debuff[] ImbueDebuffs => [Debuff.Create<Paralyzed>(60, 33)];

		

		public override SynergyEffects Effects => AOUtils.CopyDamageSynergiesFromImbue<LightningMagic>();

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = Item.height = 40;
			Item.shoot = ModContent.ProjectileType<Astrapikis>();
			Item.shootSpeed = .9f;
			Item.damage = 20;
			Item.knockBack = 3.75f;
			Item.useStyle = ItemUseStyleID.Swing;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			ActivateAbility(player, false);
			return true;
		}

		public override int DustType => ModContent.DustType<SpiritTentacle>();
	}
}
