using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitGrabbedState : UnitBaseState
{
    
    private RaycastHit _rayCastHit;
    private GameObject _typeSelectorObject;
    private SphereCollider _sphereCollider;
    private UnitTypeSelectorCollider _typeSelector;
    private UnitType? _selectedType;

    public UnitGrabbedState(UnitControllerStateMachine currentContext, StateFactory unitStateFactory) : base(currentContext, unitStateFactory)
    {
        

    }
    public override void EnterState()
    {
        Debug.Log("Entered In grab");
        _ctx.SetAnimationIsIdle(true);
        _typeSelectorObject = new GameObject();
        _sphereCollider = _typeSelectorObject.AddComponent(typeof(SphereCollider)) as SphereCollider;
        _sphereCollider.radius = _ctx.SelectorRange;
        _sphereCollider.isTrigger = true;
        _typeSelector = _typeSelectorObject.AddComponent(typeof(UnitTypeSelectorCollider)) as UnitTypeSelectorCollider;
        

    }
    public override void UpdateState()
    {
        if (Physics.Raycast(_ctx.transform.position, -Vector3.up, out _rayCastHit, 10000f, Globals.TERRAIN_LAYERMASK))
        {
            Debug.DrawRay(_ctx.transform.position, Vector3.down*10, Color.green);
            _typeSelectorObject.transform.position = _rayCastHit.point;

        }
      
           
        

        


        
        //every tot second working gets called

    }
    public override void ExitState() {
        _ctx.SetAnimationIsIdle(false);
        _selectedType = _typeSelector.GetUnitType();
        if(_selectedType != null)
        {
            _ctx.UnitType = (UnitType)_selectedType;
        }
        else
        {
            _ctx.UnitType = UnitType.BUILDER;
        }

        GameObject.Destroy(_typeSelectorObject);
        Debug.Log("exit Grabbed state");
    }
    public override void InitializeSubState() { }
    public override void CheckSwitchState()
    {
        if( _ctx.IsGrabbed == false)
        {
            if (_ctx.IsWalking)
            {
                SwitchState(_factory.Walking());
            }
            else if (_ctx.WorkingOn != null)
            {
                SwitchState(_factory.Working());
            }
            else
            {
                SwitchState(_factory.Idle());
            }
        }

    }
    public override void Working() { }


    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, NavMesh.AllAreas);

        return navHit.position;

    }


}
