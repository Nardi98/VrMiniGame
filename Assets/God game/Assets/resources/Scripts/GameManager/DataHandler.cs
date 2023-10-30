using UnityEngine;

public static class DataHandler 
{
   public static void LoadGameData()
    {
        Globals.BUILDING_DATA = Resources.LoadAll<BuildingData>("ScriptableObjects/Buildings") as BuildingData[];
        Globals.UNIT_DATA = Resources.LoadAll<UnitData>("ScriptableObjects/Units") as UnitData[];
    }
}
