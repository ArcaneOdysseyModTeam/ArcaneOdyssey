# How ao buffs convert to terraria
BuffID.Bleeding -> bleeding
BuffID.Chilled -> frozen (for now)
                new MagicBuffMultiplier(ModContent.BuffType<FreezingEffect>(), 1.1f), // freezing
                new MagicBuffMultiplier(BuffID.Wet, 1.1f), // (dazed is paralyzed)
                new MagicBuffMultiplier(BuffID.OnFire, .9f), // burning
                new MagicBuffMultiplier(BuffID.Burning, .9f), // charred
                new MagicBuffMultiplier(BuffID.OnFire3, .8f), // scorched
idk finish this later, look at the magics what i have noted down for them and compare to the wiki