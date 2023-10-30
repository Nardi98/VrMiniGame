using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingPlacer : MonoBehaviour
{
    private Building _placedBuilding = null;

    [Header("Delete building button")]
    public InputActionProperty _deleteBuilding;
    [Header("Set building")]
    public InputActionProperty _buildBuilding;
    [Header("Grabbing hand position")]
    public Transform _grabbingHand;

    //Ray cast variables
    private RaycastHit _raycastHit;
    private Vector3 _lastPlacementPosition;
   
   
    // Update is called once per frame
    void Update()
    {
        if (_placedBuilding != null)
        {


            if (_deleteBuilding.action.triggered)
            {
                _CancelPacedBuilding();
                return;
            }

            if(Physics.Raycast(_grabbingHand.transform.position, Vector3.down, out _raycastHit, 1000f, Globals.TERRAIN_LAYERMASK))
            {
                _placedBuilding.SetPosition(_raycastHit.point);
                _placedBuilding.SetRotation(_grabbingHand.transform.rotation);

                if(_lastPlacementPosition != _raycastHit.point) //every time that the hand is moved checks the validity of the new position
                {
                    _placedBuilding.CheckValidPlacement();
                }
                _lastPlacementPosition = _raycastHit.point; // update of the last position
            }
            if(_placedBuilding.HasValidPlacement && _buildBuilding.action.triggered )
            {
                
                _PlaceBuilding();
            }
        }
        
    }

    public void SelectPlacedBuilding( int buildingDataIndex)
    {
        _PreparePlacedBuilding(buildingDataIndex);
    }

    void _PreparePlacedBuilding(int buildDataIndex)
    {
        //destroy the previous phantom if there is one 
        if(_placedBuilding != null && !_placedBuilding.IsFixed)
        {
            Destroy(_placedBuilding.Transform.gameObject);
        }
        // loads the building data from Globals
        Building building = new Building(
            Globals.BUILDING_DATA[buildDataIndex]
            );
        // link the data into the building manager instance in the prefab
        building.Transform.GetComponent<BuildingManager>().Initialize(building);

        _placedBuilding = building;
        _lastPlacementPosition = Vector3.zero;
    }


    public void _CancelPacedBuilding()
    {
        //destroy the phantom building
        Destroy(_placedBuilding.Transform.gameObject);
        _placedBuilding = null;
    }

    void _PlaceBuilding()
    {
        _placedBuilding.Place();
        // keep on building the same building type
        _PreparePlacedBuilding(_placedBuilding.DataIndex);
    }
}
