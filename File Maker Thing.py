import os

magics = "Aether AncientLightning Flare Gravity PoisonLightning Heat Vesuvius Acid Ash Crystal Earth Explosion Fire Glass Light Lightning Magma Metal Plasma Poison Sand Shadow Snow Water Wind Wood".split()

basespellthing = "		public override Dictionary<Type, int> Spells => new([KeyValuePair.Create(typeof(BlastSpell),"

path = "c:/Users/Owner/OneDrive/Documents/My Games/Terraria/tModLoader/ModSources/ArcaneOdyssey/Content/Items/Magic/"

for magic in magics:
	with open(path + f"{magic}Magic.cs", "r") as w:
		original = w.read()
		with open(path + f"{magic}Magic.cs", "w") as w2:
			w2.write(original.replace(basespellthing, f""))
