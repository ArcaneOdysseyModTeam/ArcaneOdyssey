import os

magics = "Aether AncientLightning Flare Gravity PoisonLightning Heat Vesuvius Acid Ash Crystal Earth Explosion Fire Glass Ice Light Lightning Magma Metal Plasma Poison Sand Shadow Snow Water Wind Wood".split()

basespellthing = """

"""

path = "c:/Users/Owner/OneDrive/Documents/My Games/Terraria/tModLoader/ModSources/ArcaneOdyssey/Content/Projectiles/Magic/Blasts/"

for magic in magics:
	text = basespellthing.replace("mname", magic)
	with open(path + f"{magic}Blast.cs", "w") as w:
		w.write(text)
	with open(path + "IceBlast.png", "b+r") as w:
		img = w.read()
		with open(path + f"{magic}Blast.png", "b+w") as w2:
			w2.write(img)
