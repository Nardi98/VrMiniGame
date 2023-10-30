using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviour : BasicBehaviour
{
    private Aimer _aimer;
    [Range(1 , 100)]
    public int _attack;

    private EnemyUnitController? _aim;

    public GameObject _aimerObject;
    private bool _completed = false;

    public float _timeBetweenAttack;
    private float _timePassed = 0f;
    public float _arrowSpeed = 10f;
    public Transform _shootingPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame


    protected override void UpdateBehaviour()
    {
        if(!_completed)
        {
             _aimer = (Instantiate(_aimerObject, transform) as GameObject).GetComponent<Aimer>();
            _completed = true;
        }

        _timePassed += Time.deltaTime;
        if (_timePassed >= _timeBetweenAttack)
        {
            
            _aim = _aimer.GetAim();
            
            if(_aim!= null) {

                StartCoroutine(CreateArrow()); 

            _aim.Damage(_attack);
            _timePassed = 0f;
                }
        }
    }

    IEnumerator CreateArrow()
    {
        
        Vector3 lookDirection = _aim.gameObject.transform.position - _shootingPoint.position;
        Quaternion arrowRotation = Quaternion.LookRotation(lookDirection);
        GameObject arrow = GameObject.Instantiate(Resources.Load("prefabs/arrow"), _shootingPoint.position, arrowRotation) as GameObject;

        arrow.GetComponent<Rigidbody>().velocity = lookDirection.normalized * _arrowSpeed;

        yield return new WaitForSeconds(_timeBetweenAttack);
        Destroy(arrow);
    }
}