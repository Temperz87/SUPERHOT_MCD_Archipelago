from collections.abc import Mapping
from typing import Any
from worlds.AutoWorld import World
from . import items, locations, regions, rules, web_world
from . import options as shmcd_options  # rename due to a name conflict with World.options

class SHMCDWorld(World):
    """
    SUPERHOT: MIND CONTROL DELETE is a 2020 rogue like first person shooter game that was developed as the successor to the original SUPERHOT game released in 2016. 
    It's the most innovative shooter released in years 
    """

    game = "SUPERHOT: MIND CONTROL DELETE"
    web = web_world.SHMCDWebWorld()
    options_dataclass = shmcd_options.SHMCDOptions
    options: shmcd_options.SHMCDOptions
    location_name_to_id = locations.LOCATION_NAME_TO_ID
    item_name_to_id = items.ITEM_NAME_TO_ID
    origin_region_name = "Menu"
    def create_regions(self) -> None:
        regions.create_and_connect_regions(self)
        locations.create_all_locations(self)

    def set_rules(self) -> None:
        rules.set_all_rules(self)

    def create_items(self) -> None:
        items.create_all_items(self)

    def create_item(self, name: str) -> items.SHMCDItem:
        return items.create_item_with_correct_classification(self, name)

    def get_filler_item_name(self) -> str:
        return items.get_random_filler_item_name(self)

    def fill_slot_data(self) -> Mapping[str, Any]:
        # If you need access to the player's chosen options on the client side, there is a helper for that.
        d = self.options.as_dict(
            "deathlink", "randomizeLevelOrder", "unlockPyramidLayers", "unlockWeaponFiring"
        )
        d['order_string'] = self.order_string
        return d
