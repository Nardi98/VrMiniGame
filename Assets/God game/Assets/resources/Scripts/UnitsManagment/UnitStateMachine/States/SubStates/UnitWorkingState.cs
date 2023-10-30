using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitWorkingState : UnitBaseState
{
    private float _timePassed = 0f;
    private AudioSource _audio;
    public UnitWorkingState(UnitControllerStateMachine currentContext, StateFactory unitStateFactory) : base(currentContext, unitStateFactory)
    {

    }
    public override void EnterState() {

        _ctx.SetAnimationIsWorking(true);
        Debug.LogWarning("state is work");
        _audio = _ctx.Audio;
        //something with the animation
    }
    public override void UpdateState() {
        //every tot second working gets called
        _timePassed += Time.deltaTime;
        if (_timePassed > _ctx.WorkingFrequency)
        {
            _audio.Play(0);
            _currentSuperState.Working();
            _timePassed = 0;
        }
    }
    public override void ExitState() {
        _ctx.SetAnimationIsWorking(false);
    }
    public override void InitializeSubState() { }
    public override void CheckSwitchState() {
        ;
         if (_ctx.IsGrabbed == true)
        {
            SwitchState(_factory.Grabbed());
        }
        else
        if (_ctx.IsWalking)
        {
            SwitchState(_factory.Walking());

        }else if(_ctx.WorkingOn == null)
        {
            SwitchState(_factory.Idle());
        }
    }
    public override void Working(){}
}
