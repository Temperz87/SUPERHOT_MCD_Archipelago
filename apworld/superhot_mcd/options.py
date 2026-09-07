from dataclasses import dataclass

from Options import DefaultOnToggle, PerGameCommonOptions, Toggle

class DeathLink(Toggle):
    """
    WHEN THE USER DIES, EVERYONE WHO ENABLED DEATHLINK DIES. OF COURSE, THE REVERSE IS TRUE TOO.
    """
    display_name = "Death link"


class RandomizeLevelOrder(DefaultOnToggle):
    """
    THE NODES INSIDE OF THE PYRAMID APPEAR IN A RANODM ORDER.
    NOTE THE TOP MOST LAYER WILL APPEAR IN THE SAME ORDER.
    """
    display_name = "Randomize level order"


class UnlockPyramidLayers(DefaultOnToggle):
    """
    THE USER WILL REQUIRE PRIVILEGE ESCALATIONS IN ORDER TO ACCESS HIGHER LAYERS IN THE PYRAMID.
    """

    display_name = "Unlock pyramid layers"


class UnlockWeaponFiring(Toggle):
    """
    THE USER WILL NEED FIRING CLEARNCES TO FIRE THE PISTOL, SHOTGUN, MACHINEGUN, AND SNIPERRIFLE.
    """

    display_name = "Unlock weapon firing"

@dataclass
class SHMCDOptions(PerGameCommonOptions):
    # deathlink: DeathLink
    randomizeLevelOrder: RandomizeLevelOrder
    unlockPyramidLayers: UnlockPyramidLayers
    unlockWeaponFiring: UnlockWeaponFiring
    