using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Base
{
	public abstract class RelicImbue : Imbuable, ILocalizedModType
	{
		public override string LocalizationCategory => base.LocalizationCategory + ".Relics";
		public virtual int AOValue => 0;

		public override float AOImbueDamage => AOScrollDamage;
		public override float AOImbueSize => AOScrollSize;
		public override float AOImbueSpeed => AOScrollSpeed;

		public override float? DashResist => 1.2f;

		public override string AttackPrefix => "Spirit";

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.DamageType = OracleDamage.Instance;
			Item.noMelee = true;
			Item.value = AOUtils.GalleonToCopper(AOValue);
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			if (Ability.HasValue)
				Ability.Value.GenerateTooltip();
		}

		public virtual WeaponAbility? Ability => null;

		public override bool AltFunctionUse(Player player) => player.ownedProjectileCounts[Item.shoot] < 1;

		public override bool CanShoot(Player player) => player.AltUse();

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			for (float i = 0; i < 5; i++)
			{
				Dust.NewDustDirect(area.TopLeft(), area.Width, area.Height, DustID.IcyMerman, direction.GetValueOrDefault().X / 2, direction.GetValueOrDefault().Y / 2, Scale: area.RelativeScale()).noGravity = true;
			}
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			int amount = 25 * 3;
			for (float i = 0; i < amount; i++)
			{
				var centre = (MathHelper.TwoPi / amount * i).ToRotationVector2() * 20 * area.RelativeScale();
				AOUtils.NewDustImperfect(area.Center(), DustID.IcyMerman, centre * area.RelativeScale() / (13 + (Main.rand.NextFloat() * 2)), Scale: area.RelativeScale()).noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.IcyMerman, direction.X * 0.5f, direction.Y * 0.5f, Scale: area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.IcyMerman, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), Scale: intensity)];
				spawnedDust.noGravity = true;
			}
		}
	}
}
