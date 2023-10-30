using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Building selector is a singleton used to create a selector of building
 * the selectors will then be able to access it in order to create a new building. 
 */
[RequireComponent(typeof(BuildingPlacer))]
public class BuildingSelector : MonoBehaviour
{
    // singletone initialization 
    private static BuildingSelector _instance;
    //singleton getter
    public static BuildingSelector Instance{ get{ return _instance; }}
    

    // variables
    private BuildingPlacer _buildingPlacer;

    // Direction and distance between two different selectors
    [Header("Attributes to decide selectors apperance")]
    [Range(0f, 1f)]
    public float _distance;
    public Vector3 _direction ;


    public Transform buildingSelector; 

    private void Awake()
    {
        //singleton system to limit the total number to one. 
        
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        // actuale wake call 
        _buildingPlacer = GetComponent<BuildingPlacer>();
        Vector3 currentSelector = buildingSelector.position;
        

        // create selectors for each building type
        for(int i = 0; i < Globals.BUILDING_DATA.Length; i++)
        {
            string data = Globals.BUILDING_DATA[i].Code;
            GameObject selector = GameObject.Instantiate(Resources.Load($"Prefabs/BuildingSelectors/{data}Selector" ), currentSelector, buildingSelector.transform.rotation) as GameObject;
            Debug.Log(currentSelector);
            currentSelector += _direction.normalized * _distance;
            selector.GetComponent<BuildingSelectorsCall>().Index = i;
           
        }
    }

    public void _AddBuilding(int i)
    {
        _buildingPlacer.SelectPlacedBuilding(i);
    }

    public void _CancelBuilding()
    {
        _buildingPlacer._CancelPacedBuilding();
    }
   
}
