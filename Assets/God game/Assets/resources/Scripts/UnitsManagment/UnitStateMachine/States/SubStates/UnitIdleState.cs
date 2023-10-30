using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitIdleState : UnitBaseState
{
    
    private float _timePassed = 0;

    public UnitIdleState(UnitControllerStateMachine currentContext, StateFactory unitStateFactory) : base(currentContext, unitStateFactory)
    {
        

    }
    public override void EnterState()
    {

        _ctx.SetAnimationIsIdle(true);

        _timePassed = 0f;
      
        //something with the animation
    }
    public override void UpdateState()
    {
        _timePassed += Time.deltaTime;


      
           
         Vector3 newDestination = RandomNavSphere(_ctx.UnitPosition.position, _ctx._idleMoveDistance, 0);

        

        if (_timePassed > _ctx._idleTime)
        {
            _ctx.SetDestination(newDestination);
        }


        
        //every tot second working gets called

    }
    public override void ExitState() { }
    public override void InitializeSubState() { }
    public override void CheckSwitchState()
    {
        _ctx.SetAnimationIsIdle(false);

        if (_ctx.IsGrabbed == true)
         {
             SwitchState(_factory.Grabbed());
         }
         else if (_ctx.IsWalking)
        {
            SwitchState(_factory.Walking());
        }
        else if (_ctx.WorkingOn != null)
        {
            SwitchState(_factory.Working());
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
