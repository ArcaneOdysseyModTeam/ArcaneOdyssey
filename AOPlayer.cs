using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Items.Weapons.Relics;
using ArcaneOdyssey.Content.Projectiles.Weapons.Abilities;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;
using static System.Net.Mime.MediaTypeNames;

namespace ArcaneOdyssey
{
	public partial class AOPlayer : ModPlayer, IImbuableEntity
	{
		public Imbuable Imbue { get; set; }
		public bool chargingSpell = false;
		public int AOSizeStat = 0;
        public Projectile myCircle = null;
		public int timeTillNextMove = 0;
		public List<Cooldown> Cooldowns = [];
		public bool SoftFrozen => chargingSpell || Player.ownedProjectileCounts[ModContent.ProjectileType<Whirlwind>()] > 0;
		public bool Immobile => Player.CCed || timeTillNextMove > 0;
		public bool CanMoveOnGround;
		public bool FirstFrozenFrame => timeSinceSoftFrozen < 1;
		public int timeSinceSoftFrozen;

        public int pheonixHealing;

        public List<ImbueDebuffHelper> DebuffHelpers = [];

        public void UpdateDebuffHelpers(int damagedone, NPC npc, Imbuable imbue = null, bool useplayerimbue = true, bool canAddBuffs = true)
        {
            if (useplayerimbue)
            imbue ??= Imbue;
            if (imbue is not null)
            {
                foreach (var buff in imbue.ImbueDebuffs)
                {
                    var instance = DebuffHelpers.Find(e => e.buffID == buff.debuffID && e.imbue.Type == imbue.Type && e.npc.type == npc.type);
                    if (DebuffHelpers.Contains(instance))
                    {
                        int damage = instance.damagedone + damagedone;
                        if (canAddBuffs && (float)damage / npc.lifeMax > buff.debuffPercent)
                        {
                            npc.AddBuff(buff.debuffID, buff.debuffDuration);
                            damage = 0;
                        }
                        DebuffHelpers[DebuffHelpers.IndexOf(instance)] = instance with { damagedone = damage };
                    }
                    else
                    {
                        DebuffHelpers.Add(new(imbue, damagedone, npc, buff.debuffID));
                    }
                }
            }
        }

        public override void UpdateLifeRegen()
        {
            Player.lifeRegen += 5 * pheonixHealing;
        }

		public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
		{
			if (!mediumCoreDeath)
			{
				List<Item> items = [
					new Item(ModContent.ItemType<PoseidonChoice>()),
					new Item(ModContent.ItemType<EaglePatrimony>())];
				items.Add(new Item(ModContent.ItemType<Acrimony>()));
				return items;
			}
			return [];
		}

		public override void PostUpdate()
		{
			if (chargingSpell)
				Player.statDefense *= .75f;
			chargingSpell = false;
			DashStrike();
			if (Imbue is not null && !Imbue.PlayerHasImbue(Player))
			{
				Imbue = null;
			}
		}

		public void FreezeMovement() 
		{
			if (SoftFrozen)
			{
				if (FirstFrozenFrame)
				{
					CanMoveOnGround = Player.velocity.Y < 1 && Player.velocity.Y > -1 && !(Player.controlJump || Player.releaseJump);
				}
				if (!CanMoveOnGround)
				{
					Player.gravity = 0f;
					Player.velocity.X *= 0;
					Player.velocity.Y *= 0;
				}
				timeSinceSoftFrozen++;
			}
			else
			{
				timeSinceSoftFrozen = 0;
				CanMoveOnGround = false;
			}
			if (Immobile)
			{
				Player.controlDown = false;
				Player.controlUp = false;
				Player.controlLeft = false;
				Player.controlRight = false;
				Player.controlUseItem = false;
				Player.controlJump = false;
			}
		}

		public override void ResetEffects()
		{
			AOSizeStat = 0;
            pheonixHealing = 0;
			HandleDashDetection();
		}

		public float SizeMulti => AOSizeStat / 300f;
	}
}
