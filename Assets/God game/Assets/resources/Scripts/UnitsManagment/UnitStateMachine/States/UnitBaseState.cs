
using UnityEngine;

public abstract class UnitBaseState 
{
    protected bool _rootState = false;

    protected UnitControllerStateMachine _ctx;
    protected StateFactory _factory;
    protected UnitBaseState _currentSubState;
    protected UnitBaseState _currentSuperState;

    public UnitBaseState(UnitControllerStateMachine currentContex, StateFactory unitStateFactory)
    {
        _ctx = currentContex;
        _factory = unitStateFactory;
    }
    public abstract void EnterState();

    public abstract void UpdateState();

    public abstract void ExitState();

    public abstract void CheckSwitchState();

    public abstract void InitializeSubState();

    public abstract void Working();

    public void UpdateStates() {
        UpdateState();
        if (_currentSubState != null)
        {
            
            _currentSubState.UpdateStates();
        }

        
        CheckSwitchState();
    }

    protected void SwitchState(UnitBaseState newState)
    {
        ExitState();

        newState.EnterState();
        if (_rootState == true) { 
        _ctx.CurrentState = newState;
        }else if (_currentSuperState != null)
        {
            _currentSuperState.SetSubState(newState);
        }
    }

    public void ExitStates()
    {
        if (_currentSubState != null)
        {
            _currentSubState.ExitStates();
        }
        ExitState();

    }
    protected void SetSuperState(UnitBaseState newSuperState) {
        _currentSuperState = newSuperState;
    }

    protected void SetSubState(UnitBaseState newSubState) {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }

    

 
}
