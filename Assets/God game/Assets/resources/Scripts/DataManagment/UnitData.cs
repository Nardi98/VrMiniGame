using System.Collections.Generic;
using UnityEngine;

//class that contains building data
[CreateAssetMenu(fileName = "Unit", menuName = "Scriptable Objects/Unit", order = 1)]
public class UnitData : EntityData
{
    public int attack;
    public int foodConsumed;

    public  UnitData(string code, int HealthPoints, int attack) :base(code, HealthPoints){
        this.attack = attack;
    }
  
    public int Attack { get => attack;  }

}
