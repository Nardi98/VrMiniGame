using UnityEngine;


// class that manages the collisions between objects
[RequireComponent(typeof(BoxCollider))]
public class BuildingManager : MonoBehaviour
{
    private BoxCollider _collider;

    private Building _building = null;
    private int _nCollision = 0;

    //checks on placement validiti
    [Range(0f, 2f)]
    public float _bottomRayLength = 0.55f;

    public void Initialize(Building building)
    {
        _collider = GetComponent<BoxCollider>();
        _building = building;
    }

    // checks collision with objects ecluding objects that are tagged as terrain or player
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Terrain" || other.tag == "Player" || other.gameObject.layer == LayerMask.NameToLayer("untangible")) return;

        Debug.Log(other.gameObject.name);
        _nCollision++;
        CheckPlacement();
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Terrain" || other.tag == "Player" || other.gameObject.layer == LayerMask.NameToLayer("untangible")) return;
        
        _nCollision--;
        CheckPlacement();
    }


    public bool CheckPlacement()
    {
        if (_building == null) return false;
        if (_building.IsFixed) return false;
        bool validPlacement = HasValidPlacement();

        if (!validPlacement)
        {
            _building.SetMaterials();
        }
        else
        {
            _building.SetMaterials();
        }
        return validPlacement;
    }
    public bool HasValidPlacement()
    {
        if (_nCollision > 0) return false;

        // get the position of the 4 corners
        Vector3 p = transform.position;
        Vector3 c = _collider.center;
        Vector3 e = _collider.size / 2f;
        float bottomHeight = c.y - e.y + 0.5f;
        Vector3[] bottomCorners = new Vector3[]
        {
            new Vector3(c.x - e.x, bottomHeight, c.z - e.z),
            new Vector3(c.x - e.x, bottomHeight, c.z + e.z),
            new Vector3(c.x + e.x, bottomHeight, c.z - e.z),
            new Vector3(c.x + e.x, bottomHeight, c.z + e.z)
        };


        // cast a small ray beneth the corner to check for close ground
        // (if at least two are not valid the placement is invalid
        int invalidCornersCount = 0;
        foreach(Vector3 corner in bottomCorners)
        {
            if(!Physics.Raycast(p + corner, Vector3.up* -1f, _bottomRayLength, Globals.TERRAIN_LAYERMASK))
            {
                invalidCornersCount++;
            }

        }
        return invalidCornersCount < 3;

    }

}
