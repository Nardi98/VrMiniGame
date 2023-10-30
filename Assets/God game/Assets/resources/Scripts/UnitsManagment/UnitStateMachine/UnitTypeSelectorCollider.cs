using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTypeSelectorCollider : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> _collidingObjecs = new List<GameObject>();




    public UnitType? GetUnitType()
    {
        if (_collidingObjecs.Count > 0)
        {
            GameObject selectedObject = _collidingObjecs[0];
            Debug.Log(selectedObject.tag);
            switch (selectedObject.tag)
            {
                case "farm":
                    return UnitType.FARMER;
                case "stoneDeposit":
                    return UnitType.MINER;
                case "woodFacility":
                    return UnitType.WOODWORKER;
                default: return null;
                    
            }
        }
        else
        {
            return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.gameObject.tag == "farm" || other.gameObject.tag == "stoneDeposit" || other.gameObject.tag == "woodFacility")
        {
            Debug.Log($"Object just added {other.gameObject.tag}");
            _collidingObjecs.Add(other.gameObject);
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        if (other.gameObject.tag == "farm" || other.gameObject.tag == "stoneDeposit" || other.gameObject.tag == "woodFacility")
        {
           Debug.Log($"Object just removed {other.gameObject.tag}"); 
            _collidingObjecs.Remove(other.gameObject);
        }
    }

    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 0.4f);
    }
}
