using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;


public class UnitFarmerState : UnitBaseState
{
    private GameObject[] _farmsList = null;
    private FarmBehaviour _workingIn;

    private float _timePassed;
    private float _timeBetweenSearchs = 10;
    private float _currentDistance;

    // Start is called before the first frame update
    public UnitFarmerState(UnitControllerStateMachine currentContext, StateFactory unitStateFactory) : base(currentContext, unitStateFactory)
    {
        
    }
    public override void EnterState()
    {
        Debug.Log("enter farmer");
        _timePassed = 0;
        _rootState = true;
        _workingIn = ClosestFarm();
        if(_workingIn != null){
            _workingIn.AddFarmer(_ctx);
        }
        UpdateState();

        InitializeSubState();
    }
    public override void UpdateState()
    {
        _timePassed += Time.deltaTime;
        if(_workingIn != null)
        {
            _currentDistance = Vector3.Distance(_ctx.transform.position, _workingIn.transform.position);
            if(_currentDistance <= _ctx._actionDistance)
            {
                _ctx.WorkingOn = _workingIn.transform;
            }
            else
            {
                _ctx.SetDestination(_workingIn.transform);
            }
        }else if(_timePassed> _timeBetweenSearchs)
        {
            _workingIn = ClosestFarm();
            if (_workingIn != null)
            {
                _timePassed = 0;
                _workingIn.AddFarmer(_ctx);
            }
        }
    }
    public override void ExitState()
    {
        if (_workingIn != null)
        {
            _workingIn.RemoveFarmer(_ctx);
        }
    }


    public override void InitializeSubState()
    {
        /*
        if (_ctx.IsGrabbed)
        {
            SetSubState(_factory.Grabbed());
        }
        else if (_currentDistance > _ctx._actionDistance)
        {
            SetSubState(_factory.Walking());
        }
        else if (_currentDistance <= _ctx._actionDistance)
        {
            SetSubState(_factory.Working());
        }
        else
        {*/
            SetSubState(_factory.Idle());
        //}
    }
    public override void CheckSwitchState() {
        if (_ctx.UnitType == UnitType.BUILDER)
        {
            SwitchState(_factory.Builder());
        }
        else if (_ctx.UnitType == UnitType.MINER)
        {
            SwitchState(_factory.Miner());
        }
        else if (_ctx.UnitType == UnitType.WOODWORKER)
        {
            SwitchState(_factory.WoodWorker());
        }
    }

    public override void Working()
    {
        _workingIn.Produce(_ctx.Strength);
        
    }

    private FarmBehaviour ClosestFarm()
    {
        float minDistance = -1f;
        FarmBehaviour closestFarm = null;
        _farmsList = GameObject.FindGameObjectsWithTag("farm");
        if(_farmsList.Length == 0)
        {
            return null;
        }

        foreach (GameObject farm in _farmsList)
        {
            FarmBehaviour currentFarm = farm.GetComponent<FarmBehaviour>();
            Debug.Log(currentFarm);
            if (!currentFarm.IsFull)
            {
                float distance = Vector3.Distance(farm.GetComponent<Transform>().position, _ctx.transform.position);
                if (distance <= minDistance || minDistance < 0) {
                    closestFarm = currentFarm;
                }
            }
        }
        return closestFarm;
    }


}
