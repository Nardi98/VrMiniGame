using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Unit hierarchical state machine to construct a changable unit type that changes when dropped in a specific position.
 * 
 */
public class Unit 
{
    protected UnitData _data;
    protected Transform _transform;
    protected UnitControllerStateMachine _controller;


    public Unit(UnitData data, Vector3 position)
    {
        _data = data;
        GameObject g = GameObject.Instantiate(data.prefab  ) as GameObject;
        _transform = g.transform;
        _controller = g.GetComponent<UnitControllerStateMachine>();

        _controller.UnitInitialization(data.healthPoints, data.attack, data.foodConsumed, position);




    }    

    public void SetPosition(Vector3 position)
    {
        _transform.position = position;
    }

    
}


