using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Buffs.Stuns;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Relics;
using ArcaneOdyssey.VFX.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Relics
{
	public class EaglePatrimony : RelicWeapon
	{
		public override Color ImbueColour => Color.LightBlue;
		public override AORarities AORarity => AORarities.Special;
		public override CombinedDebuff[] CombinedDebuffs => [new(BuffID.Wet, ModContent.BuffType<AOParalyzed>())];

		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				//ModContent.BuffType<AOPetrified>(), // petrified
				//ModContent.BuffType<CharredEffect>(),
				//ModContent.BuffType<SandyEffect>(),
				//ModContent.BuffType<AOBleed>(),
				//ModContent.BuffType<AOFrozen>()
			],
			[
				new(BuffID.Chilled, 1.2f), // frozen
				new(ModContent.BuffType<AOBleed>(), 1.2f), // bleeding
				new(BuffID.Burning, 1.15f), // scalding
				new(BuffID.OnFire3, 1.075f), // melting/hellfire
				new(BuffID.Venom, 1.075f), // venom acid
				new(BuffID.Wet, 1.05f), // 
				new(BuffID.ShadowFlame,1.15f),
				new(BuffID.Oiled,0.96f),
				new(ModContent.BuffType<Crystallized>(),1.075f),
				new(ModContent.BuffType<SearedEffect>(),1.15f)
			]
			);

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = Item.height = 40;
			Item.shoot = ModContent.ProjectileType<SpiritBlast>();
			Item.shootSpeed = 15;
			Item.UseSound = SoundID.Item84 with { Pitch = .75f };
			Item.damage = 13;
			Item.useAnimation = Item.useTime = 30;
			Item.knockBack = 3.75f;
		}

		public override void LingeringEffects(Entity entity)
		{
			Dust.NewDustDirect(entity.position, entity.width, entity.height, ModContent.DustType<SpiritTentacle>(), newColor: Color.White, Scale: .75f).noGravity = true;
		}
	}
}
