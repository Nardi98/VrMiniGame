using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;

//class that handles the istances of buildings

// stated in which a building cn be duirnign the construction process
public enum BuildingPlacement
{
    VALID,
    FIXED,
    INVALID, 
    BUILT
};

public class Building 
{
    private BuildingData _data; // coming from the building data class
    private Transform _transform;
    private int _currentHealth;

    //variables to check on the correct placement
    private BuildingPlacement _placement;
    private List<Material> _materials;

    private BuildingManager _buildingManager;
    private GameObject g;

    private DecalProjector _decal;




    public Building(BuildingData data)
    {
        _data = data;
        _currentHealth = data.HP;
        // importing the prefab from the scriptable object
         g = GameObject.Instantiate(data.prefab) as GameObject;

        _transform = g.transform;                               // set the attribute _transform equal to the transform of the newrly instantiated gameobject
        _buildingManager = g.GetComponent<BuildingManager>();   //gets the building manager component in the newrly instantiated object
        g.GetComponent<BasicBehaviour>().ThisBuilding = this;



        //Creates a new set of materials and than saves in the default materials of the mesh


        _decal = g.GetComponentInChildren(typeof(DecalProjector)) as DecalProjector;
        _decal.enabled = true;

      

             
        

        CheckValidPlacement();  
        SetMaterials();
    }




    // sets the correct material depending on the value of the placement (valid invalid fixed)


    public void SetMaterials() { SetMaterials(_placement); }
    public void SetMaterials(BuildingPlacement placement)
    {

        

        
        if (placement == BuildingPlacement.VALID)
        {
            Material refMaterial = Resources.Load("Materials/BuildingState/Valid") as Material;        // loads the refmaterial from resources

            _decal.material = refMaterial;                       // loads the refmaterial from resources

        }
        else if(placement == BuildingPlacement.INVALID)            //this is done in this way because the rendere takes a list and not just a single material
        {
            Material refMaterial = Resources.Load("Materials/BuildingState/Invalid") as Material;       // loads the refmaterial from resources

            _decal.material = refMaterial;
        }
        else if(placement == BuildingPlacement.FIXED)
        {
            _decal.enabled = false;             
        }
        else
        {
            return;
        }
              
    }

    public void SetPosition(Vector3 position)
    {
        _transform.position = position;
    }

    public void SetRotation(Quaternion rotation)
    {
        Vector3 eulerRotation = rotation.eulerAngles;
        eulerRotation = new Vector3(0f, eulerRotation.y, 0f);
        _transform.rotation = Quaternion.Euler(eulerRotation);
    }

    //place function to place the building 
    public void Place()
    {
        //set placement state
        _placement = BuildingPlacement.FIXED;


        // Change building material
        SetMaterials();
        // remove the "is trigger" flag from the collider
        // to allow collisions with units
        //_transform.GetComponent<BoxCollider>().isTrigger = false;

        _transform.GetComponent<NavMeshObstacle>().enabled = true;

        foreach(ResourceValue resource in _data.Cost)
        {
            Globals.GAME_RESOURCE[resource.code].AddAmount(-resource.amount);
        }
        _currentHealth = 0;
        g.transform.localScale = new Vector3(1, (float)(_currentHealth / _data.healthPoints) + 0.01f, 1); 
        SchedulerBuildingToComplete.Instance.AddBuilding(this);

    }
   

    public void Build(int addHealthPoint)
    {
        _currentHealth += addHealthPoint;
        if(_currentHealth >= _data.healthPoints)
        {
            _currentHealth = _data.healthPoints;
            BuildingComplete();
           
        }
        g.transform.localScale = new Vector3(1, ((float)_currentHealth /(float) _data.healthPoints), 1);
 
    }
    private void BuildingComplete()
    {
        _placement = BuildingPlacement.BUILT;
        SchedulerBuildingToComplete.Instance.RemoveBuilding(this);
    }

    public bool Invulnerable()
    {
        if(_placement == BuildingPlacement.VALID || _placement == BuildingPlacement.INVALID)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool CanBuy()
    {
        return _data.CanBuy();
    }

    public void CheckValidPlacement()
    {
        if (CanBuy())
        {
            if (_placement == BuildingPlacement.FIXED) return;
            _placement = _buildingManager.CheckPlacement()
                ? BuildingPlacement.VALID
                : BuildingPlacement.INVALID;
        }
        else
        {
            _placement = BuildingPlacement.INVALID;
        }
        
    }


    // getter and setter
    public string Code { get => _data.Code; }
    public Transform Transform { get => _transform; }
    public int HP { get => _currentHealth; set => _currentHealth = value; }
    public int MaxHp { get => _data.HP; }
    public int DataIndex
    {
        get
        {
            for(int i = 0; i< Globals.BUILDING_DATA.Length; i++)
            {
                if (Globals.BUILDING_DATA[i].Code == _data.Code)
                {
                    return i;
                }
            }
            return -1;
        }
    }
    public bool IsFixed { get => _placement == BuildingPlacement.FIXED; }
  
    public bool HasValidPlacement { get => _placement == BuildingPlacement.VALID; }
    
    public bool IsCompleted { get => _placement == BuildingPlacement.BUILT; }
        
    
    
}
