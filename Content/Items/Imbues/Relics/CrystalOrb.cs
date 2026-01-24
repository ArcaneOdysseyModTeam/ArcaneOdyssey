using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Imbues.Relics
{
	public class CrystalOrb : RelicImbue
	{
		public override Color ImbueColour => new(255, 255, 0, 255);
		public override SoundStyle? ImbueSound => SoundID.Item9;
		public override float AOScrollSpeed => 1.2f;
		public override float AOScrollSize => 1f;
		public override float AOScrollDamage => 0.9f;
		public override WeaponAbility? Ability => new(Mod, "Aithiraki", "Summon a minion made of spirit energy", ImbueColour);
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				
			],
			[
				new(ModContent.BuffType<Crystallized>(),1.075f),
				new(ModContent.BuffType<DrainedEffect>(),0.8f)
			]
			);

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = Item.height = 56;
			Item.noUseGraphic = false;
			Item.holdStyle = ItemHoldStyleID.HoldGolfClub;
			Item.scale = .25f;
			Item.useStyle = ItemUseStyleID.Swing;
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			base.LingeringEffects(area, direction, source);
			for (float i = 0; i < 5; i++)
			{
				Dust.NewDustDirect(area.TopLeft(), area.Width, area.Height, DustID.YellowStarDust, direction.GetValueOrDefault().X / 2, direction.GetValueOrDefault().Y / 2, Scale: area.RelativeScale()).noGravity = true;
			}
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			base.KillEffects(area, source);
			for (float i = 0; i < 50; i++)
			{
				var centre = (MathHelper.TwoPi / 50 * i).ToRotationVector2() * 60 * area.RelativeScale();
				AOUtils.NewDustImperfect(area.Center(), DustID.YellowStarDust, centre * area.RelativeScale() / (13 + (Main.rand.NextFloat() * 2)), Scale: area.RelativeScale()).noGravity = true;
				AOUtils.NewDustImperfect(area.Center(), DustID.YellowStarDust, centre * area.RelativeScale() / (14 + (Main.rand.NextFloat() * 2)), Scale: area.RelativeScale()).noGravity = true;
				AOUtils.NewDustImperfect(area.Center(), DustID.YellowStarDust, centre * area.RelativeScale() / (15 + (Main.rand.NextFloat() * 2)), Scale: area.RelativeScale()).noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			base.SpawningEffects(area, direction);
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.YellowStarDust, direction.X * 0.5f, direction.Y * 0.5f, Scale: area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			base.ExplosionEffects(position, intensity);
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.YellowStarDust, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), Scale: intensity)];
				spawnedDust.noGravity = true;
			}
		}
	}
}
