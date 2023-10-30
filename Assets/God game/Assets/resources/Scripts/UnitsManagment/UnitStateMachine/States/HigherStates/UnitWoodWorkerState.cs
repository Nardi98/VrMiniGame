using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;



public class UnitWoodWorkerState : UnitBaseState
{
    private GameObject _GameManager = null;
    private TreeManager _trees;
    private TreeInstance? _workingIn;
    private GameObject _workingOn = new GameObject();

    private Terrain _terrain;

    
    private float _currentDistance;

    // Start is called before the first frame update
    public UnitWoodWorkerState(UnitControllerStateMachine currentContext, StateFactory unitStateFactory) : base(currentContext, unitStateFactory)
    {
        
    }
    public override void EnterState()
    {
        _rootState = true;

        _GameManager = GameObject.FindGameObjectWithTag("GameController");
        Debug.Log(_GameManager.name);
        _trees = _GameManager.GetComponent<TreeManager>();
        _terrain = _trees._terrain;
        
        Debug.Log("enter wood worker");


        _ctx.ChangeStoppingDistance();
        
        UpdateState();

        InitializeSubState();
    }

    public override void UpdateState()
    {
        
        if(_workingIn != null)
        {
            
            _currentDistance = Vector3.Distance(_ctx.transform.position, TreeCoordinateToWorld((TreeInstance)_workingIn));
            if(_currentDistance <= _ctx._actionDistance)
            {
                _workingOn.transform.position = TreeCoordinateToWorld((TreeInstance)_workingIn);
                _ctx.WorkingOn = _workingOn.transform;
            }
            else
            {
                _ctx.SetDestination(TreeCoordinateToWorld((TreeInstance)_workingIn));
            }
        }
        else
        {
            TreeInstance? tree;
            tree = _trees.GetClosestTree(_ctx.transform.position);
            if (tree == null)
            {
                _ctx.WorkingOn = null;
            }
            else
            {
                _workingIn = tree;
            }
        }
    }
    public override void ExitState()
    {
     

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
    
        if(_ctx.UnitType == UnitType.BUILDER)
        {
            SwitchState(_factory.Builder());
        }else if(_ctx.UnitType == UnitType.FARMER)
        {
            SwitchState(_factory.Farmer());
        }else if(_ctx.UnitType == UnitType.MINER)
        {
            SwitchState(_factory.Miner());
        }
    }

    public override void Working()
    {
        if (!_trees.Produce((TreeInstance)_workingIn, _ctx.Strength))
        {
            _workingIn = null;
        }
        
    }

    private Vector3 TreeCoordinateToWorld(TreeInstance treeInstance)
    {
        TerrainData terrainData = _terrain.terrainData;

        var treeInstancePos = treeInstance.position;
        Vector3 localPos = new Vector3(treeInstancePos.x * terrainData.size.x, treeInstancePos.y * terrainData.size.y, treeInstancePos.z * terrainData.size.z);
        Vector3 worldPos = Terrain.activeTerrain.transform.TransformPoint(localPos);
        return worldPos;

    }

    

}
