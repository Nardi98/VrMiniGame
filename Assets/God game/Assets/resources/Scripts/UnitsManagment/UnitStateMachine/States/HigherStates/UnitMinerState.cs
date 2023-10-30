using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;


public class UnitMinerState : UnitBaseState
{
    private GameObject[] _stoneDepositList = null;
    private StoneDepositBehaviour _workingIn;

    private float _timePassed;
    private float _timeBetweenSearchs = 10;
    private float _currentDistance;

    // Start is called before the first frame update
    public UnitMinerState(UnitControllerStateMachine currentContext, StateFactory unitStateFactory) : base(currentContext, unitStateFactory)
    {
        
    }
    public override void EnterState()
    {
        Debug.Log("enter miner");
        _timePassed = 0;
        _rootState = true;
        _workingIn = ClosestStoneDeposit();
        if(_workingIn != null){
            _workingIn.AddMiner(_ctx);
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
            _workingIn = ClosestStoneDeposit();
            if (_workingIn != null)
            {
                _timePassed = 0;
                _workingIn.AddMiner(_ctx);
            }
        }
    }
    public override void ExitState()
    {
        if (_workingIn != null)
        {
            _workingIn.RemoveMiner(_ctx);
        }
    }


    public override void InitializeSubState()
    {
         
       /** if (_ctx.IsGrabbed)
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
        else if (_ctx.UnitType == UnitType.FARMER)
        {
            SwitchState(_factory.Farmer());
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

    private StoneDepositBehaviour ClosestStoneDeposit()
    {
        float minDistance = -1f;
        StoneDepositBehaviour closestStoneDeposit = null;
        _stoneDepositList = GameObject.FindGameObjectsWithTag("stoneDeposit");
        if(_stoneDepositList.Length == 0)
        {
            return null;
        }

        foreach (GameObject stoneDeposit in _stoneDepositList)
        {
            StoneDepositBehaviour currentStoneDeposit = stoneDeposit.GetComponent<StoneDepositBehaviour>();
            if (!currentStoneDeposit.IsFull)
            {
                float distance = Vector3.Distance(stoneDeposit.GetComponent<Transform>().position, _ctx.transform.position);
                if (distance <= minDistance || minDistance < 0) {
                    closestStoneDeposit = currentStoneDeposit;
                }
            }
        }
        return closestStoneDeposit;
    }


}
