using UnityEngine;

public class StateFactory 
{
    UnitControllerStateMachine _context;

    public StateFactory(UnitControllerStateMachine currentContext)
    {
        _context = currentContext;
    }

    public UnitBaseState Farmer() {
        return new UnitFarmerState(_context, this);
    }
    public UnitBaseState Builder() {
        return new UnitBuilderState(_context, this);
    }
    
    public UnitBaseState WoodWorker() {
        return new UnitWoodWorkerState(_context, this);
    }
    
    public UnitBaseState Miner() {
        return new UnitMinerState(_context, this);
    }
    
    public UnitBaseState Idle() {
        return new UnitIdleState(_context, this);
    }
    public UnitBaseState Walking() {
        return new UnitWalkState(_context, this);
    }
    public UnitBaseState Working()
    {
        return new UnitWorkingState(_context, this);
    }
    public UnitBaseState Grabbed()
    {
        return new UnitGrabbedState(_context,this);
    }

}
