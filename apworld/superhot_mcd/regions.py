from __future__ import annotations

from typing import TYPE_CHECKING

from BaseClasses import Entrance, Region

if TYPE_CHECKING:
    from .world import SHMCDWorld

# A region is a container for locations ("checks"), which connects to other regions via "Entrance" objects.
# Many games will model their Regions after physical in-game places, but you can also have more abstract regions.
# For a location to be in logic, its containing region must be reachable.
# The Entrances connecting regions can have rules - more on that in rules.py.
# This makes regions especially useful for traversal logic ("Can the player reach this part of the map?")

# Every location must be inside a region, and you must have at least one region.
# This is why we create regions first, and then later we create the locations (in locations.py).

def create_and_connect_regions(world: SHMCDWorld) -> None:
    create_all_regions(world)
    connect_regions(world)

def create_all_regions(world: SHMCDWorld) -> None:
    # Create a region for each pyramid layer
    menu = Region("Menu", world.player, world.multiworld)
    sensory_layer = Region("SENSORY", world.player, world.multiworld)
    short_layer = Region("SHORT", world.player, world.multiworld)
    long_layer = Region("LONG", world.player, world.multiworld)
    core_layer = Region("CORE", world.player, world.multiworld)
    lost_layer = Region("LOST", world.player, world.multiworld)
    ending_layer = Region("ENDING SEQUENCE", world.player, world.multiworld)
    regions = [menu, sensory_layer, short_layer, long_layer, core_layer, lost_layer, ending_layer]

    # Add regions to the multiworld
    world.multiworld.regions += regions


def connect_regions(world: SHMCDWorld) -> None:
    menu = world.get_region("Menu")
    sensory_layer = world.get_region("SENSORY")
    short_layer = world.get_region("SHORT")
    long_layer = world.get_region("LONG")
    core_layer = world.get_region("CORE")
    lost_layer = world.get_region("LOST")
    ending_layer = world.get_region("ENDING SEQUENCE")

    # Connect regions to each other
    # Creates that mapping
    # A has an entrance to B
    # B can go back to A
    menu.connect(sensory_layer, "Menu to SENSORY")
    if world.options.unlockPyramidLayers:
        sensory_layer.connect(short_layer, "SENSORY TO SHORT", lambda state: state.has("PRIVILEGE ESCALATION: SHORT", world.player))
        short_layer.connect(long_layer, "SHORT TO LONG",  lambda state: state.has("PRIVILEGE ESCALATION: LONG", world.player))
        long_layer.connect(core_layer, "LONG TO LAYER CORE", lambda state: state.has("PRIVILEGE ESCALATION: CORE", world.player))
        core_layer.connect(lost_layer, "CORE TO LOST", lambda state: state.has("PRIVILEGE ESCALATION: LOST", world.player))
    else:
        sensory_layer.connect(short_layer, "SENSORY TO SHORT")
        short_layer.connect(long_layer, "SHORT TO LONG")
        long_layer.connect(core_layer, "LONG TO LAYER CORE")
        core_layer.connect(lost_layer, "CORE TO LOST")

    lost_layer.connect(ending_layer, "LOST TO ENDING", lambda state: 
                state.has("MORE.core", world.player) and
                state.has("CHARGE.core", world.player) and
                state.has("HOTSWITCH.core", world.player) and
                state.has("RECALL.core", world.player))
