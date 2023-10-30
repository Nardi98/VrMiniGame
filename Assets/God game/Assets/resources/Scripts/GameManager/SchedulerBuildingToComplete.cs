using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchedulerBuildingToComplete : MonoBehaviour
{
    private static SchedulerBuildingToComplete _instance;

    public static SchedulerBuildingToComplete Instance { get { return _instance; } }

    private List<Building> _buildingsToComplete = new List<Building>();
    // Start is called before the first frame update
    private void Awake()
    {
        //singleton initialization
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }


    }
    public void AddBuilding(Building building)
    {
        _buildingsToComplete.Add(building);
    }
    public void RemoveBuilding(Building building)
    {
        _buildingsToComplete.Remove(building);
    }

    private void Update()
    {
        foreach (Building building in _buildingsToComplete){
            if (building.Transform == null)
            {
                RemoveBuilding(building);
            }
        }
    }
    public List<Building> BuildingsToComplete { get { return _buildingsToComplete; } }
}
