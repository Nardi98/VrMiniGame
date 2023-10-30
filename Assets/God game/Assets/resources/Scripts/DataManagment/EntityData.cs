using System.Collections.Generic;
using UnityEngine;

//class that contains building data
[CreateAssetMenu(fileName = "Entity", menuName = "Scriptable Objects/Entity", order = 1)]
public class EntityData : ScriptableObject
{
    public string code;
    public string unitName;
    public int healthPoints;
    public GameObject prefab;

    public EntityData(string code, int healthPoints)
    {
        this.code = code;
        this.healthPoints = healthPoints;
    }

   
    public string Code { get => code; }
    public int HP { get => healthPoints; }


}
