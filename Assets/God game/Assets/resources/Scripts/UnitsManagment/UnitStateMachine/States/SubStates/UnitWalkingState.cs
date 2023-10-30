using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitWalkState : UnitBaseState
{
    public UnitWalkState(UnitControllerStateMachine currentContext, StateFactory unitStateFactory) : base(currentContext, unitStateFactory)
    {

    }
    public override void EnterState()
    {
        _ctx.SetAnimationIsWalking(true);
        Debug.LogWarning("state is walking");
        //something with the animation
    }
    public override void UpdateState()
    {
       
        
    }
    public override void ExitState() {
        _ctx.SetAnimationIsWalking(false);
        
    }
    public override void InitializeSubState() { }
    public override void CheckSwitchState()
    {
       
       if (_ctx.IsGrabbed == true)
        {
            SwitchState(_factory.Grabbed());
        }
        else if (_ctx.WorkingOn != null)
        {
            SwitchState(_factory.Working());
        }
        else if (_ctx.WorkingOn == null && !_ctx.IsWalking)
        {
            SwitchState(_factory.Idle());
        }
    }
    public override void Working() { }
}
