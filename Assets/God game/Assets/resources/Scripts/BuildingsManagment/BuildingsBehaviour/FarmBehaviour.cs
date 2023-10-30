using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmBehaviour : BasicBehaviour
{
    [SerializeField]
    private List<UnitControllerStateMachine> _workers = new List<UnitControllerStateMachine>();
    public int _farmersLimit = 5;
  



    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame


    protected override void UpdateBehaviour()
    {
        
    }

    public void Produce(int amount)
    {
        
        Globals.GAME_RESOURCE["food"].AddAmount(amount);
    }

    public void AddFarmer(UnitControllerStateMachine newWorker)
    {
        _workers.Add(newWorker);
    }
    public void RemoveFarmer(UnitControllerStateMachine oldWorker)
    {
        _workers.Remove(oldWorker);
    }
    public bool IsFull
    {
        get
        {
            if (_workers.Count < _farmersLimit)
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