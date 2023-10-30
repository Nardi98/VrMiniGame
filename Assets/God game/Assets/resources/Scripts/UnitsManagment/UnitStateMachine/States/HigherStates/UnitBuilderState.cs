using System.Collections.Generic;
using UnityEngine;


public class UnitBuilderState : UnitBaseState
{

    protected List<Building> _buildingsToBuild;
    protected SchedulerBuildingToComplete _buildings;
    protected Building _currentBuilding = null;

    protected float _currentDistance;

    // Start is called before the first frame update
    public UnitBuilderState(UnitControllerStateMachine currentContext, StateFactory unitStateFactory) : base(currentContext, unitStateFactory) 
    {
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameController");
        _buildings = gameManager.GetComponent<SchedulerBuildingToComplete>();

        _rootState = true;
        
        UpdateState();
        InitializeSubState(); 
    }
    public override void EnterState() {
        

    }
    public override void  UpdateState() {

        _buildingsToBuild = _buildings.BuildingsToComplete;

        if(_buildingsToBuild.Count > 0 && (_currentBuilding == null || _currentBuilding != _buildingsToBuild[0]))
        {
            _currentBuilding = _buildingsToBuild[0];
            _ctx.SetDestination(_currentBuilding.Transform);
        }
        if(_buildingsToBuild.Count == 0)
        {
            _currentBuilding = null;
            _ctx.WorkingOn = null;
        }

        if(_currentBuilding != null && _currentBuilding.Transform != null && _ctx.UnitPosition != null)
        {
            _currentDistance = Vector3.Distance(_ctx.UnitPosition.position, _currentBuilding.Transform.position); //_currentBuilding.Transform.position);
            if(_currentDistance <= _ctx._actionDistance)
            {
                _ctx.WorkingOn = _currentBuilding.Transform;
            }
            else
            {
                _ctx.WorkingOn = null;
            }
        }

        


    }
    public override void  ExitState() {
        _ctx.WorkingOn = null;
    }
    public override void  InitializeSubState() {
     /*   if (_ctx.IsGrabbed)
        {
            SetSubState(_factory.Grabbed());
        }
        else if (_ctx.IsWalking)
        {
            SetSubState(_factory.Walking());
        }else if (_ctx.WorkingOn != null)
        {
            SetSubState(_factory.Working());
        }else
        {*/

            SetSubState(_factory.Idle());
       //}
                
    }
    public override void  CheckSwitchState() {
        if (_ctx.UnitType == UnitType.MINER)
        {
            SwitchState(_factory.Miner());
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
        _currentBuilding.Build(_ctx.Strength);
        
    }



}
