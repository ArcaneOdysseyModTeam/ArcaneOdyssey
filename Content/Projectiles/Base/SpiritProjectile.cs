using ArcaneOdyssey.Content.Items.Base;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Projectiles.Base
{
	public abstract class SpiritProjectile : AOPlayerProjectile
    {
        public override AODebuffRequirement? Debuff => null;
        public virtual CombinedDebuff[] CombinedDebuffs => [];
        public virtual SynergyEffects Effects => new([], []);


        public override void SetDefaults()
		{
			Projectile.DamageType = ModContent.GetInstance<SpiritDamage>();
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			base.ModifyHitNPC(target, ref modifiers);

            if (CombinedDebuffs is not null)
            {
                foreach (CombinedDebuff buffkeys in CombinedDebuffs)
                {
                    if (target.HasBuff(buffkeys.requirement) || (buffkeys.requirement == BuffID.Wet && target.wet))
                    {
                        target.AddBuff(buffkeys.result, buffkeys.duration);
                    }
                }
            }

            foreach (MagicBuffMultiplier multiplier in Effects.magicBuffMultipliers)
            {
                if (target.HasBuff(multiplier.buffID) || (multiplier.buffID == BuffID.Wet && target.wet))
                {
                    modifiers.FinalDamage += multiplier.multiplier.MultiToPercent();
                }
            }

            if (Main.netMode == NetmodeID.SinglePlayer) // things would get chaotic in multiplayer if everyone kept clearing eachothers debuffs
            {
                foreach (int buffid in Effects.clearBuffs)
                {
                    if (target.HasBuff(buffid))
                    {
                        target.DelBuff(target.FindBuffIndex(buffid));
                    }
                }
            }
        }

		public virtual void ManageSynergies(ref NPC.HitModifiers modifiers) { }

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
		}
	}
}
