using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneDepositBehaviour : BasicBehaviour
{
    [SerializeField]
    private List<UnitControllerStateMachine> _workers = new List<UnitControllerStateMachine>();
    public int _minersLimit = 5;
    
  



    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame


    protected override void UpdateBehaviour()
    {
        _invulnerable = true;
    }

    public void Produce(int amount)
    {
        Debug.Log($"current stone ammount {Globals.GAME_RESOURCE["stone"].Amout} adding{amount}");
        Globals.GAME_RESOURCE["stone"].AddAmount(amount);
    }

    public void AddMiner(UnitControllerStateMachine newWorker)
    {
        _workers.Add(newWorker);
    }
    public void RemoveMiner(UnitControllerStateMachine oldWorker)
    {
        _workers.Remove(oldWorker);
    }
    public bool IsFull
    {
        get
        {
            if (_workers.Count < _minersLimit)
            {
                
                return false;
            }
            else
            {
                
                return true;
            }
        }
    }
}