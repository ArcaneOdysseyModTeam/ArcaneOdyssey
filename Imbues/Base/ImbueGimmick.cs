using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Imbues.Base
{
	public abstract class ImbueGimmick : ModType, ILocalizedModType
	{
		public virtual string LocalizationCategory => "ImbueGimmicks";

		protected sealed override void Register()
		{
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

		public virtual LocalizedText DisplayName => Mod.CoolCustomLocalization(LocalizationCategory + "." + Name + ".DisplayName", PrettyPrintName);
		public virtual LocalizedText Description => Mod.CoolCustomLocalization(LocalizationCategory + "." + Name + ".Description");
	}
}
