using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Imbues.Base
{
	public abstract class ImbueGimmick : ModType, ILocalizedModType
	{
		public virtual string LocalizationCategory => "ImbueGimmicks";

		public ushort Type { get; private set; }
		private static ushort count = 0;

		protected sealed override void Register()
		{
			Type = count++;
			ModTypeLookup<ImbueGimmick>.Register(this);
		}

		public sealed override void SetupContent()
		{
			_ = DisplayName;
			_ = Description;
			SetStaticDefaults();
		}


		public virtual void ModifyManaCost(Item item, Player player, ref float reduce, ref float mult) { }
		public virtual void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers) { }
		public virtual void ModifyHitNPC(Item item, Player player, NPC target, ref NPC.HitModifiers modifiers) { }
		public virtual void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone) { }
		public virtual void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone) { }
		public virtual void KillEffects(Projectile projectile) { }
		public virtual void SpawningEffects(Projectile projectile) { }
		public virtual void InventoryEffects(Item item, Player player) { }
		public virtual void NoInventoryEffects(Item item, Player player) { }
		public virtual void ModifyWeaponCrit(Item item, Player player, ref float crit) { }
		public virtual void UseAnimation(Item item, Player player) { }
		public virtual void OnConsumeItem(Item item, Player player) { }
		public virtual void UpdateInventory(Player player) { }
		public virtual void Update(Item item) { }
		public virtual void UpdateInventory(Item item, Player player) { }

		public virtual LocalizedText DisplayName => this.GetLocalization("DisplayName", PrettyPrintName);
		public virtual LocalizedText Description => this.GetLocalization("Description");
	}
}
