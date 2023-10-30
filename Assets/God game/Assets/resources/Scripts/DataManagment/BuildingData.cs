using System.Collections.Generic;
using UnityEngine;

//class that contains building data
[CreateAssetMenu(fileName = "Building",menuName ="Scriptable Objects/Building", order = 1)]
public class BuildingData: EntityData
{
    public List<ResourceValue> cost;

    public BuildingData(string code, int healthPoints, List<ResourceValue> cost): base(code, healthPoints)
    {
        this.cost = cost;
    }

    public bool CanBuy()
    {
        foreach( ResourceValue resource in cost)
        {
            if (Globals.GAME_RESOURCE[resource.code].Amout < resource.amount)
            {
                return false;
            }

        }
        return true;
    }
    public List<ResourceValue> Cost { get => cost; }
  
}
