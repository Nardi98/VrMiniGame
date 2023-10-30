using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSelectorsCall : MonoBehaviour
{
    private int? _index = null;

    
    private Transform _grabbableTransform;
    private Quaternion _initialRotation;
    private Vector3 _initialPosition;

    public int Index { set => _index = value;  }
    // Start is called before the first frame update
    public void Start()
    {
        _grabbableTransform = this.gameObject.transform.Find("Grabbable");
        _initialPosition = _grabbableTransform.position;
        _initialRotation = _grabbableTransform.rotation;
    }
    public void BuildingSelected()
    {
        Debug.Log("buiding selected call");
        Debug.Log(_index);
        if (null != _index)
        {
            BuildingSelector.Instance._AddBuilding((int)_index);
        }
    }

    public void BuildingDeselected()
    {
        _grabbableTransform.position = _initialPosition;
        _grabbableTransform.rotation = _initialRotation;
        BuildingSelector.Instance._CancelBuilding();
    }
}
