using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseBehaviour : BasicBehaviour
{
    public Transform _spawnPosition;
    [Range(0f, 600f)]
    public float _spawnTime;
    public string _unitName;
    public int _spawnLimit = 1;
    private int _spawns = 0;
    private UnitData _unit;
    private float _timePassed = 0f;

    // Start is called before the first frame update
    void Start()
    {
        foreach(UnitData unit in Globals.UNIT_DATA)
        {
            if(unit.Code == _unitName)
            {
                _unit = unit;
            }
        }
        
    }

    // Update is called once per frame
   

    protected  override void UpdateBehaviour()
    {
        _timePassed += Time.deltaTime;

        if (_timePassed >= _spawnTime && _spawns < _spawnLimit)
        {

            _spawns += 1;
            new Unit(_unit,  _spawnPosition.position);
            _timePassed = 0f;
        }
    }


}
