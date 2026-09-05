from dataclasses import dataclass

from Options import DefaultOnToggle, PerGameCommonOptions, Toggle

class DeathLink(Toggle):
    """
    WHEN THE USER DIES, EVERYONE WHO ENABLED DEATHLINK DIES. OF COURSE, THE REVERSE IS TRUE TOO.
    """
    display_name = "Death link"


class RandomizeLevelOrder(DefaultOnToggle):
    """
    WILL THE NODES INSIDE OF THE PYRAMID APPEAR IN A RANODM ORDER?
    NOTE THAT CACHES AND QUARANTINES WILL BE RANDOMIZED SEPARATELY;
    AND THE ENDING SEQUENCE WILL BE IN THE SAME ORDER.
    """
    display_name = "Randomize level order"


class UnlockPyramidLayers(DefaultOnToggle):
    """
    THE USER WILL REQUIRE PRIVILEGE ESCALATIONS IN ORDER TO ACCESS HIGHER LEVELS IN THE PYRAMID.
    """

    display_name = "Unlock Pyramid Layers"


class UnlockWeaponFiring(DefaultOnToggle):
    """
    THE USER WILL NEED FIRING CLEARNCES TO FIRE THE PISTOL, SHOTGUN, MACHINEGUN, AND SNIPERRIFLE.
    """

    display_name = "Unlock Weapon Firing"

@dataclass
class SHMCDOptions(PerGameCommonOptions):
    deathlink: DeathLink
    randomizeLevelOrder: RandomizeLevelOrder
    unlockPyramidLayers: UnlockPyramidLayers
    unlockWeaponFiring: UnlockWeaponFiring
    