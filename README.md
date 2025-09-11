/*
How ao buffs convert to terraria
BuffID.Bleeding -> bleeding
BuffID.Chilled -> frozen (for now)
BuffID.ShadowFlame -> Scorched
                new MagicBuffMultiplier(ModContent.BuffType<FreezingEffect>(), 1.1f), // freezing
                new MagicBuffMultiplier(BuffID.Wet, 1.1f), // (dazed is paralyzed)
                new MagicBuffMultiplier(BuffID.OnFire, .9f), // burning
                new MagicBuffMultiplier(BuffID.Burning, .9f), // charred
                new MagicBuffMultiplier(BuffID.OnFire3, .8f), // melting
                
idk finish this later, look at the magics what i have noted down for them and compare to the wiki

Humble Beginnings:
- Morden guide npc, immune to most damage, but doesn't attack
- Explosion spell
- Prometheus acrimony
- Ash and poison lingering clouds
- Add death magic (for morden) (he won't use it lol)
- Vfx changes/tweaks to fire & magma
- Possible sfx change for Vesuvius and magma
- Add tucker gravestone
- Possible blast resprites
> fire blast
> lightning blast
> magma blast
*/