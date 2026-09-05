from __future__ import annotations

from typing import TYPE_CHECKING

from worlds.generic.Rules import set_rule

if TYPE_CHECKING:
    from .world import SHMCDWorld

def set_all_rules(world: SHMCDWorld) -> None:
    set_all_entrance_rules(world)
    set_all_location_rules(world)
    set_completion_condition(world)


# This is already done in regions.py
def set_all_entrance_rules(world: SHMCDWorld) -> None:
    pass

def set_all_location_rules(world: SHMCDWorld) -> None:
    give_up_charge = world.get_location("LOST / CHARGE")
    give_up_hotswitch = world.get_location("LOST / HOTSWITCH")
    give_up_recall = world.get_location("LOST / RECALL")
    give_up_more = world.get_location("LOST / CORELESS")

    set_rule(give_up_charge, lambda state: state.has("CHARGE.core", world.player))
    set_rule(give_up_hotswitch, lambda state: state.has("HOTSWITCH.core", world.player))
    set_rule(give_up_recall, lambda state: state.has("RECALL.core", world.player))
    set_rule(give_up_more, lambda state: 
                state.has("MORE.core", world.player) and
                state.has("CHARGE.core", world.player) and
                state.has("HOTSWITCH.core", world.player) and
                state.has("RECALL.core", world.player))

def set_completion_condition(world: SHMCDWorld) -> None:
    # You win when you get the victory item! (see create_events() in locations.py).
    world.multiworld.completion_condition[world.player] = lambda state: state.has("Victory", world.player)
