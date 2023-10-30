using System.Collections.Generic;

public class Globals
{
    public static int TERRAIN_LAYERMASK = 1 << 6;
    public static BuildingData[] BUILDING_DATA;
    public static UnitData[] UNIT_DATA;
    

    public static Dictionary<string, GameResource> GAME_RESOURCE =
        new Dictionary<string, GameResource>()
        {
            {"gold", new GameResource("Gold", 1000) },
            {"wood", new GameResource("Wood", 1000) },
            {"stone", new GameResource("Stone", 1000) },
            {"food", new GameResource("Food", 1000) }
        };



}