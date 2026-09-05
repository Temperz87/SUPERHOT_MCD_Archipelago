from __future__ import annotations

from typing import TYPE_CHECKING

from BaseClasses import Item, ItemClassification

if TYPE_CHECKING:
    from .world import SHMCDWorld

ITEM_NAME_TO_ID = {
    "PISTOL FIRING CLEARANCE": 1,
    "SHOTGUN FIRING CLEARANCE": 2,
    "MACHINEGUN FIRING CLEARANCE": 3,
    "SNIPERRIFLE FIRING CLEARANCE": 4,
    "PRIVILEGE ESCALATION: SHORT": 5,
    "PRIVILEGE ESCALATION: LONG": 6,
    "PRIVILEGE ESCALATION: CORE": 7,
    "PRIVILEGE ESCALATION: LOST": 8,
    "CORRUPTED MEMORY": 9,
    "MORE.core": 10,
    "HOTSWITCH.core": 11,
    "RECALL.core": 12,
    "CHARGE.core": 13,
    "PURE.core": 14,
    "4HP.hack": 15,
    "5HP.hack": 16,
    "recharge.hack": 17,
    "chainchrg.hack": 18,
    "prfswitch.hack": 19,
    "ultraswitch.hack": 20,
    "piercing.hack": 21,
    "flwrecall.hack": 22,
    "3HP.hack": 23,
    "grenade.hack": 24,
    "explode.hack": 25,
    "supthrow.hack": 26,
    "piercshot.hack": 27,
    "wpnmstr.hack": 28,
    "defall.hack": 29,
    "ricochet.hack": 30,
    "suppunch.hack": 31,
    "shotflow.hack": 32,
    "berserk.hack": 33,
    "killreload.hack": 34,
    "dthstomp.hack": 35,
    "lightreflx.hack": 36,
    "killheal.hack": 37,
    "heal.hack": 39,
}

# Items should have a defined default classification.
# In our case, we will make a dictionary from item name to classification.
DEFAULT_ITEM_CLASSIFICATIONS = {
    "PISTOL FIRING CLEARANCE": ItemClassification.progression | ItemClassification.useful,
    "SHOTGUN FIRING CLEARANCE": ItemClassification.progression | ItemClassification.useful,
    "MACHINEGUN FIRING CLEARANCE": ItemClassification.progression | ItemClassification.useful,
    "SNIPERRIFLE FIRING CLEARANCE": ItemClassification.progression | ItemClassification.useful,
    "PRIVILEGE ESCALATION: SHORT": ItemClassification.progression,
    "PRIVILEGE ESCALATION: LONG": ItemClassification.progression,
    "PRIVILEGE ESCALATION: CORE": ItemClassification.progression,
    "PRIVILEGE ESCALATION: LOST": ItemClassification.progression,
    "CORRUPTED MEMORY": ItemClassification.filler,
    "MORE.core": ItemClassification.progression | ItemClassification.useful,
    "CHARGE.core": ItemClassification.progression | ItemClassification.useful,
    "HOTSWITCH.core": ItemClassification.progression | ItemClassification.useful,
    "RECALL.core": ItemClassification.progression | ItemClassification.useful,
    "PURE.core": ItemClassification.filler,
    "4HP.hack": ItemClassification.useful,
    "5HP.hack": ItemClassification.useful,
    "recharge.hack": ItemClassification.useful,
    "chainchrg.hack": ItemClassification.useful,
    "prfswitch.hack": ItemClassification.useful,
    "ultraswitch.hack": ItemClassification.useful,
    "piercing.hack": ItemClassification.useful,
    "flwrecall.hack": ItemClassification.useful,
    "3HP.hack": ItemClassification.useful,
    "grenade.hack": ItemClassification.useful,
    "explode.hack": ItemClassification.useful,
    "supthrow.hack": ItemClassification.useful,
    "piercshot.hack": ItemClassification.useful,
    "wpnmstr.hack": ItemClassification.useful,
    "defall.hack": ItemClassification.useful,
    "ricochet.hack": ItemClassification.useful,
    "suppunch.hack": ItemClassification.useful,
    "shotflow.hack": ItemClassification.useful,
    "berserk.hack": ItemClassification.useful,
    "killreload.hack": ItemClassification.useful,
    "dthstomp.hack": ItemClassification.useful,
    "lightreflx.hack": ItemClassification.useful,
    "killheal.hack": ItemClassification.useful,
    "heal.hack": ItemClassification.progression | ItemClassification.useful,
}

class SHMCDItem(Item):
    game = "SUPERHOT: MIND CONTROL DELETE"


def get_random_filler_item_name(world: SHMCDWorld) -> str:
    return "CORRUPTED MEMORY"

def create_item_with_correct_classification(world: SHMCDWorld, name: str) -> SHMCDItem:
    # Our world class must have a create_item() function that can create any of our items by name at any time.
    # So, we make this helper function that creates the item by name with the correct classification.
    # Note: This function's content could just be the contents of world.create_item in world.py directly,
    # but it seemed nicer to have it in its own function over here in items.py.
    classification = DEFAULT_ITEM_CLASSIFICATIONS[name]
    return SHMCDItem(name, classification, ITEM_NAME_TO_ID[name], world.player)


def create_all_items(world: SHMCDWorld) -> None:
    # Create necessary items
    itempool: list[Item] = [
        # world.create_item("MORE.core"),
        world.create_item("CHARGE.core"),
        world.create_item("HOTSWITCH.core"),
        world.create_item("RECALL.core"),
        # world.create_item("PURE.core"),
        world.create_item("4HP.hack"),
        world.create_item("5HP.hack"),
        world.create_item("recharge.hack"),
        world.create_item("chainchrg.hack"),
        world.create_item("prfswitch.hack"),
        world.create_item("ultraswitch.hack"),
        world.create_item("piercing.hack"),
        world.create_item("flwrecall.hack"),
        world.create_item("3HP.hack"),
        world.create_item("grenade.hack"),
        world.create_item("explode.hack"),
        world.create_item("supthrow.hack"),
        world.create_item("piercshot.hack"),
        world.create_item("wpnmstr.hack"),
        world.create_item("defall.hack"),
        world.create_item("ricochet.hack"),
        world.create_item("suppunch.hack"),
        world.create_item("shotflow.hack"),
        world.create_item("berserk.hack"),
        world.create_item("killreload.hack"),
        world.create_item("dthstomp.hack"),
        world.create_item("lightreflx.hack"),
        world.create_item("killheal.hack"),
        # world.create_item("heal.hack"),
    ]

    # Create conditional items
    if world.options.unlockPyramidLayers:
        itempool.append(world.create_item("PRIVILEGE ESCALATION: SHORT"))
        itempool.append(world.create_item("PRIVILEGE ESCALATION: LONG"))
        itempool.append(world.create_item("PRIVILEGE ESCALATION: CORE"))
        itempool.append(world.create_item("PRIVILEGE ESCALATION: LOST"))

    if world.options.unlockWeaponFiring:
        itempool.append(world.create_item("PISTOL FIRING CLEARANCE"))
        itempool.append(world.create_item("SHOTGUN FIRING CLEARANCE"))
        itempool.append(world.create_item("MACHINEGUN FIRING CLEARANCE"))
        itempool.append(world.create_item("SNIPERRIFLE FIRING CLEARANCE"))    

    # Fill the rest of the pool with our filler item
    number_of_items = len(itempool)
    number_of_unfilled_locations = len(world.multiworld.get_unfilled_locations(world.player))
    needed_number_of_filler_items = number_of_unfilled_locations - number_of_items
    if needed_number_of_filler_items > 0:
        itempool.append(world.create_item("PURE.core"))
        needed_number_of_filler_items -= 1
    
    itempool += [world.create_filler() for _ in range(needed_number_of_filler_items)]

    # This is how the generator actually knows about the existence of our items.
    world.multiworld.itempool += itempool

    # Push starting items
    world.push_precollected(world.create_item("MORE.core"))
    world.push_precollected(world.create_item("heal.hack"))
