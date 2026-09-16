namespace ArcaneOdyssey.Imbues.Base
{
	public abstract class ImbueGimmick : ModType, ILocalizedModType
	{
		/// <inheritdoc/>
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

		/// <summary>
		/// Modifies the mana cost of imbued items
		/// </summary>
		/// <param name="item"></param>
		/// <param name="player"></param>
		/// <param name="reduce"></param>
		/// <param name="mult"></param>
		public virtual void ModifyManaCost(Item item, Player player, ref float reduce, ref float mult) { }

		/// <summary>
		/// Modifies the mana cost of the imbue
		/// </summary>
		/// <param name="player"></param>
		/// <param name="reduce"></param>
		/// <param name="mult"></param>
		public virtual void ModifyManaCost(Player player, ref float reduce, ref float mult) { }
		/// <summary>
		/// Called when an imbued projectile hits an NPC
		/// </summary>
		/// <param name="projectile"></param>
		/// <param name="target"></param>
		/// <param name="modifiers"></param>
		public virtual void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers) { }
		/// <summary>
		/// Called when an imbued item hits an NPC
		/// </summary>
		/// <param name="item"></param>
		/// <param name="player"></param>
		/// <param name="target"></param>
		/// <param name="modifiers"></param>
		public virtual void ModifyHitNPC(Item item, Player player, NPC target, ref NPC.HitModifiers modifiers) { }
		/// <summary>
		/// Called after an imbued item hits an NPC
		/// </summary>
		/// <param name="item"></param>
		/// <param name="player"></param>
		/// <param name="target"></param>
		/// <param name="hit"></param>
		/// <param name="damageDone"></param>
		public virtual void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone) { }
		/// <summary>
		/// Called after an imbued projectile hits an NPC
		/// </summary>
		/// <param name="projectile"></param>
		/// <param name="target"></param>
		/// <param name="hit"></param>
		/// <param name="damageDone"></param>
		public virtual void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone) { }
		/// <summary>
		/// Used while simulating AoE
		/// </summary>
		/// <param name="imbue"></param>
		/// <param name="player"></param>
		/// <param name="target"></param>
		/// <param name="hit"></param>
		/// <param name="damageDone"></param>
		public virtual void OnHitNPC(Imbuable imbue, Player player, NPC target, NPC.HitInfo hit, int damageDone) { }
		/// <summary>
		/// Called when an imbued projectile dies
		/// </summary>
		/// <param name="projectile"></param>
		public virtual void KillEffects(Projectile projectile) { }
		/// <summary>
		/// Called when an imbued projectile spawns
		/// </summary>
		/// <param name="projectile"></param>
		public virtual void SpawningEffects(Projectile projectile) { }
		/// <summary>
		/// Called on all items in the same inventory as this imbue
		/// </summary>
		/// <param name="item"></param>
		/// <param name="player"></param>
		public virtual void InventoryEffects(Item item, Player player) { }
		/// <summary>
		/// Called on all items in your inventory, when you don't have this imbue in your inventory
		/// </summary>
		/// <param name="item"></param>
		/// <param name="player"></param>
		public virtual void NoInventoryEffects(Item item, Player player) { }
		/// <summary>
		/// Modifies the crit of imbued items
		/// </summary>
		/// <param name="item"></param>
		/// <param name="player"></param>
		/// <param name="crit"></param>
		public virtual void ModifyWeaponCrit(Item item, Player player, ref float crit) { }
		/// <summary>
		/// Called when you use an imbued item
		/// </summary>
		/// <param name="item"></param>
		/// <param name="player"></param>
		public virtual void UseAnimation(Item item, Player player) { }
		/// <summary>
		/// Called when you consume an item while having this imbue selected
		/// </summary>
		/// <param name="item"></param>
		/// <param name="player"></param>
		public virtual void OnConsumeItem(Item item, Player player) { }
		/// <summary>
		/// Called on the imbue every frame in inventory
		/// </summary>
		/// <param name="player"></param>
		public virtual void UpdateInventory(Player player) { }
		/// <summary>
		/// Called on imbue first frame not in inventory
		/// </summary>
		/// <param name="item"></param>
		public virtual void Update(Item item) { }
		/// <summary>
		/// Called on imbued items every frame in inventory
		/// </summary>
		/// <param name="item"></param>
		/// <param name="player"></param>
		public virtual void UpdateInventory(Item item, Player player) { }

		public virtual LocalizedText DisplayName => this.GetLocalization("DisplayName", PrettyPrintName);
		public virtual LocalizedText Description => this.GetLocalization("Description");
	}
}
