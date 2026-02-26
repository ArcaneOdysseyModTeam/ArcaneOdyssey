using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.VFX.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Imbues.Relics
{
	public class SpiritEnergy : Imbuable
	{
		public override Color ImbueColour => SpiritColor;
		public static Color SpiritColor => new(0, 183, 255);

		public virtual int AOValue => 0;

		public override SoundStyle? ImbueSound => SoundID.NPCDeath6;

		public override float AOImbueDamage => AOScrollDamage;
		public override float AOImbueSize => AOScrollSize;
		public override float AOImbueSpeed => AOScrollSpeed;

		public override float AOScrollSpeed => 1f;
		public override float AOScrollDamage => 1f;
		public override float AOScrollSize => 1f;

		public override float? DashResist => 1.2f;

		public override string AttackPrefix => "Spirit";

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.DamageType = DamageClass.Summon;
			Item.noMelee = true;
			Item.value = AOUtils.GalleonToCopper(AOValue);
		}

		public override bool CanShoot(Player player) => player.ownedProjectileCounts[Item.shoot] < 1 && !player.AltUse();

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			for (float i = 0; i < 5; i++)
			{
				Dust.NewDustDirect(area.TopLeft(), area.Width, area.Height, ModContent.DustType<SpiritDust>(), direction.GetValueOrDefault().X / 2, direction.GetValueOrDefault().Y / 2, Scale: area.RelativeScale(), Alpha: 255 / 4, newColor: SpiritColor).noGravity = true;
			}
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			int amount = 25 * 3;
			for (float i = 0; i < amount; i++)
			{
				var centre = (MathHelper.TwoPi / amount * i).ToRotationVector2() * 20 * area.RelativeScale();
				AOUtils.NewDustImperfect(area.Center(), DustID.IcyMerman, centre * area.RelativeScale() / (13 + (Main.rand.NextFloat() * 2)), Scale: area.RelativeScale(), Alpha: 255 / 4, newColor: SpiritColor).noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.IcyMerman, direction.X * 0.5f, direction.Y * 0.5f, Scale: area.RelativeScale(), Alpha: 255 / 4, newColor: SpiritColor)];
				spawnedDust.noGravity = true;
			}
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.IcyMerman, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), Scale: intensity, Alpha: 255 / 4, newColor: SpiritColor)];
				spawnedDust.noGravity = true;
			}
		}
	}
}
