from __future__ import annotations

from typing import TYPE_CHECKING

from BaseClasses import Location

from . import items

if TYPE_CHECKING:
    from .world import SHMCDWorld

item_locations = [
    ("SENSORY / CACHE / piercshot.hack", 89),
    ("SENSORY / CACHE / explode.hack", 93),
    ("SENSORY / CACHE / grenade.hack", 99),
    ("SENSORY / CACHE / supthrow.hack", 92),
    ("SHORT / CACHE / defall.hack", 90),
    ("SHORT / CACHE / ricochet.hack", 94),
    ("SHORT / CACHE / suppunch.hack", 91),
    ("SHORT / CACHE / wpnmstr.hack", 88),
    ("LONG / CACHE / berserk.hack", 79),
    ("LONG / CACHE / dthstomp.hack", 96),
    ("LONG / CACHE / shotflow.hack", 95),
    ("LONG / CACHE / killheal.hack", 83),
    ("LONG / CACHE / killreload.hack", 98),
    ("LONG / CACHE / lightreflx.hack", 97),
    ("QUARANTINE / BROKEN", 58),
    ("QUARANTINE / UNSTABLE", 59),
    ("QUARANTINE / TOXIC", 60),
]

levels = [
    ("SENSORY / NODE 1A", 1),
    ("SENSORY / NODE 1B", 19),
    ("SENSORY / NODE 1C", 2),
    ("SENSORY / NODE 2", 3),
    ("SENSORY / NODE 2A", 21),
    ("SENSORY / NODE 2B", 22),
    ("SHORT / NODE 3", 4),
    ("SHORT / NODE 3A", 20),
    ("SHORT / NODE 3B", 23),
    ("SHORT / NODE 3C", 24),
    ("SHORT / NODE 4", 5),
    ("SHORT / NODE 4A", 25),
    ("SHORT / NODE 4C", 27),
    ("LONG / NODE 5", 6),
    ("LONG / NODE 5B", 28),
    ("LONG / NODE 5C", 29),
    ("LONG / NODE 5D", 71),
    ("LONG / NODE 6", 7),
    ("LONG / NODE 6B", 30),
    ("LONG / NODE 6C", 37),
    ("LONG / NODE 7", 8),
    ("LONG / NODE 7B", 31),
    ("LONG / NODE 7C", 36),
    ("CORE / NODE 8", 9),
    ("CORE / NODE 8A", 32),
    ("CORE / NODE 8B", 800),
    ("CORE / NODE 8C", 34),
    ("CORE / NODE 8D", 35),
    ("ENCRYPTED / ADDICT", 75),
    ("ENCRYPTED / NINDŻA", 76),
    ("ENCRYPTED / DOG", 77),
]

# Maps every SUPER HOT: MCD location to its RunID
# Key is its English translation name
LOCATION_NAME_TO_ID = {
    # Randomized levels
    "SENSORY / CACHE / piercshot.hack": 89,
    "SENSORY / CACHE / explode.hack": 93,
    "SENSORY / CACHE / grenade.hack": 99,
    "SENSORY / CACHE / supthrow.hack": 92,
    "SHORT / CACHE / defall.hack": 90,
    "SHORT / CACHE / ricochet.hack": 94,
    "SHORT / CACHE / suppunch.hack": 91,
    "SHORT / CACHE / wpnmstr.hack": 88,
    "LONG / CACHE / berserk.hack": 79,
    "LONG / CACHE / dthstomp.hack": 96,
    "LONG / CACHE / shotflow.hack": 95,
    "LONG / CACHE / killheal.hack": 83,
    "LONG / CACHE / killreload.hack": 98,
    "LONG / CACHE / lightreflx.hack": 97,
    "QUARANTINE / BROKEN": 58,
    "QUARANTINE / UNSTABLE": 59,
    "QUARANTINE / TOXIC": 60,
    "SENSORY / NODE 1A": 1,
    "SENSORY / NODE 1B": 19,
    "SENSORY / NODE 1C": 2,
    "SENSORY / NODE 2": 3,
    "SENSORY / NODE 2A": 21,
    "SENSORY / NODE 2B": 22,
    "SHORT / NODE 3": 4,
    "SHORT / NODE 3A": 20,
    "SHORT / NODE 3B": 23,
    "SHORT / NODE 3C": 24,
    "SHORT / NODE 4": 5,
    "SHORT / NODE 4A": 25,
    "SHORT / NODE 4C": 27,
    "LONG / NODE 5": 6,
    "LONG / NODE 5B": 28,
    "LONG / NODE 5C": 29,
    "LONG / NODE 5D": 71,
    "LONG / NODE 6": 7,
    "LONG / NODE 6B": 30,
    "LONG / NODE 6C": 37,
    "LONG / NODE 7": 8,
    "LONG / NODE 7B": 31,
    "LONG / NODE 7C": 36,
    "CORE / NODE 8": 9,
    "CORE / NODE 8A": 32,
    "CORE / NODE 8B": 800,
    "CORE / NODE 8C": 34,
    "CORE / NODE 8D": 35,
    "ENCRYPTED / ADDICT": 75,
    "ENCRYPTED / NINDŻA": 76,
    "ENCRYPTED / DOG": 77,

    # Not randomized levels
    "SENSORY / NODE 0": 0,
    # "ALL": 61, # Unused
    # "HACK_UNLOCK": 62, # tutorial level
    # "QUARANTINE / PURE": 200, # where you get pure.core
    # "CORE / NODE 100": 18, 
    # "ERROR / ▒▪▒▓▌░■■█░": 39,
    # "ERROR / ■░░▬▓█▌▒▄": 40,
    # "ERROR / ▐▐░▓█▬▒▄▄▀▐": 38,
    "LOST / HOTSWITCH": 300,
    "LOST / RECALL": 301,
    "LOST / CHARGE": 302,
    "LOST / CORELESS": 44,
    "LOST / POWERLESS": 45,
    "LOST / POWERS": 303,
    # "ERROR / ▬█▒▒■▒█▬": 48,
    # "ERROR / ▄▐▓▓▀▬█░▀": 41,
    # "RUN_NAME_Start": 74,
    # "CORE / DEATH": 70, # Unused, I think it got replaced with LOST / DEATH
    "SENSORY / BIRTH": 123,
    "SHORT / AGING": 124,
    "CORE / SICKNESS": 125,
    "LOST / DEATH": 304,
    # "CORE / INFINITE": 73,
    # "CORE / final.key": 400, # SAMSARA, the spinning wheel level that says "no more secrets"
    # "STORY": 63, # Unused I think?
}

class SHMCDLocation(Location):
    game = "SUPERHOT: MIND CONTROL DELETE"

def get_location_names_with_ids(location_names: list[str]) -> dict[str, int | None]:
    return {location_name: LOCATION_NAME_TO_ID[location_name] for location_name in location_names}

def create_all_locations(world: SHMCDWorld) -> None:
    create_regular_locations(world)
    create_events(world)

# TODO:
# 1. separate out runs/crypts and caches/cores
# 2. see how many runs/crypts and caches/cores each region has
# 3. create massive byte stream consisting of RunIds, where byte 255 = RunID 800
# 4. 1 becomes first in byte array, 1a becomes second, etc. etc.
# 5. win
def randomize_locations(world: SHMCDWorld) -> None:
    # Get each region
    sensory_layer = world.get_region("SENSORY")
    short_layer = world.get_region("SHORT")
    long_layer = world.get_region("LONG")
    core_layer = world.get_region("CORE")

    # Calculate how many nodes each level will need
    sensory_cache_amount = 4
    sensory_quarantine_amount = 1
    sensory_items = sensory_cache_amount + sensory_quarantine_amount
    sensory_level_amount = 6

    short_cache_amount = 4
    short_quarantine_amount = 2
    short_items = short_cache_amount + short_quarantine_amount
    short_level_amount = 7

    long_cache_amount = 6
    long_quarantine_amount = 0
    long_items = long_cache_amount + long_quarantine_amount
    long_level_amount = 13

    core_level_amount = 5

    sensory_nodes = sensory_items + sensory_level_amount
    short_nodes = short_items + short_level_amount
    long_nodes = long_items + long_level_amount
    core_nodes = core_level_amount

    all_locations = levels + item_locations

    # Level order to send to client
    order_string = ''

    # Place nodes
    needed = [(sensory_layer, sensory_nodes), 
              (short_layer, short_nodes),
              (long_layer, long_nodes),
              (core_layer, core_nodes)]
    for region, nodes in needed:
        locations = []
        while nodes > 0:
            chosen = world.random.choice(all_locations)
            all_locations.remove(chosen)
            locations.append(chosen[0])

            # every level is one or two digits
            # except for the dog level
            # so let's do this instead to save some bytes:
            if chosen[1] == 800:
                # 81 shouldn't be taken, although it is a magic number :(
                to_send = '81' 
            elif chosen[1] < 10:
                to_send = '0' + str(chosen[1])
            else:
                to_send = str(chosen[1])

            order_string += to_send
            nodes -= 1

        new_locations = get_location_names_with_ids(locations)
        region.add_locations(new_locations, SHMCDLocation)

    world.order_string = order_string

def place_locations(region, locations):
    new_locations = get_location_names_with_ids(locations)
    region.add_locations(new_locations, SHMCDLocation)


def put_locations_in_default_regions(world: SHMCDWorld) -> None:
    world.order_string = 'default'
    
    # Finally, we need to put the Locations ("checks") into their regions.
    # Once again, before we do anything, we can grab our regions we created by using world.get_region()
    sensory_layer = world.get_region("SENSORY")
    short_layer = world.get_region("SHORT")
    long_layer = world.get_region("LONG")
    core_layer = world.get_region("CORE")

    # Place levels
    sensory_levels = [
        location[0] for location in levels + item_locations
        if location[0].startswith("SENSORY")
    ]

    place_locations(sensory_layer, sensory_levels)
    short_levels = [
        location[0] for location in levels + item_locations
        if location[0].startswith("SHORT")
    ]

    place_locations(short_layer, short_levels)
    long_levels = [
        location[0] for location in levels + item_locations
        if location[0].startswith("LONG") or location[0].startswith("QUARANTINE")
    ]

    place_locations(long_layer, long_levels)
    core_levels = [
        location[0] for location in levels + item_locations
        if location[0].startswith("CORE")
    ]

    place_locations(core_layer, core_levels)

def create_regular_locations(world: SHMCDWorld) -> None:
    sensory_layer = world.get_region("SENSORY")
    short_layer = world.get_region("SHORT")
    core_layer = world.get_region("CORE")
    lost_layer = world.get_region("LOST")

    # Place locations that can't be randomized
    place_locations(sensory_layer, ["SENSORY / NODE 0", "SENSORY / BIRTH"])
    place_locations(short_layer, ["SHORT / AGING"])
    place_locations(core_layer, ["CORE / SICKNESS"])
    lost_levels = [
        location for location,_ in LOCATION_NAME_TO_ID.items()
        if location.startswith("LOST")
    ]

    place_locations(lost_layer, lost_levels)

    # Place random locations
    if world.options.randomizeLevelOrder:
        randomize_locations(world)
    else:
        put_locations_in_default_regions(world)

def create_events(world: SHMCDWorld) -> None:
    ending_layer = world.get_region("ENDING SEQUENCE")
    ending_layer.add_event(
        "Recovering data", "Victory", location_type=SHMCDLocation, item_type=items.SHMCDItem
    )
