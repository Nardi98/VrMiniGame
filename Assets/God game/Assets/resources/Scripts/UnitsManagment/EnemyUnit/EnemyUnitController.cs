using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyUnitController : MonoBehaviour
{
    private enum State
    {
        IDLE,
        MOVING,
        ATTACK
    }
    private enum AttackedType
    {
        BUILDING,
        UNIT
    }
    private NavMeshAgent _agent;

    private Transform _movingAim = null;
    private Transform _attackAim = null;
    private AttackedType _attackedType;
    private List<GameObject> _attackList = new List<GameObject>();
    private State _currentState = State.IDLE;
    private BasicBehaviour _attackedBuilding;
    private UnitControllerStateMachine _attackedUnit;
    private float _timePassed = 0;
    public float _attackTime = 0;

    public int _currentHealth;

    private AudioSource _audio;

    [Range( 0, 30)]
    public int _attack = 10;
    public Animator _animator;

    public GameObject _aimerObject;
    private EnemyAimer _aimer;
    private bool _created = false;

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _aimerObject = Instantiate(_aimerObject) as GameObject;
        _aimerObject.transform.position = transform.position;
        _aimer = _aimerObject.GetComponent<EnemyAimer>();
        _audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        _attackList = _aimer.AttackList;
        _aimerObject.transform.position = transform.position;
        if (!_agent.enabled)
        {
            _animator.SetBool("isIdle", true);
            _animator.SetBool("isAttacking", false);
            _animator.SetBool("isMoving", false);
            if (!_created)
            {
                NavMeshHit closestHit;
                if (NavMesh.SamplePosition(transform.position, out closestHit, 500, 1))
                {
                    gameObject.transform.position = closestHit.position;

                    //TODO

                }
                else
                {
                    Dead();
                    Debug.Log("problemi nel posizionare l'agent");
                }
                _created = true;
                _agent.enabled = true;
            }
        }
        else
        {

            _currentState = CheckSwitchState();

            switch (_currentState)
            {
                case State.IDLE:
                    IdleState();
                    break;
                case State.MOVING:
                    Moving();
                    break;
                case State.ATTACK:
                    Attack();
                    break;

            }


        }
    }




    private void IdleState()
    {
        _animator.SetBool("isIdle", true);
        _animator.SetBool("isAttacking", false);
        _animator.SetBool("isMoving", false);

        Debug.Log($"Aiming at this {_movingAim}");

        if (_movingAim == null)
        {
            _movingAim = GetAim();
            _agent.SetDestination(_movingAim.position);
        }
        else
        {
            CorrectDestinationPosition(_movingAim);
        }
        
    }
    private void Moving()
    {
        
        _animator.SetBool("isIdle", false);
        _animator.SetBool("isAttacking", false);
        _animator.SetBool("isMoving", true);
        _animator.SetFloat("speed", _agent.velocity.magnitude/_agent.speed);

        if (_attackAim != null)
        {
            CorrectDestinationPosition(_attackAim);
            
        }
        else if (_movingAim != null)
        {
            
            CorrectDestinationPosition(_movingAim);
        } 
    }

    private void Attack()
    {
        CorrectDestinationPosition(_attackAim);
        _animator.SetBool("isIdle", false);
        _animator.SetBool("isAttacking", true);
        _animator.SetBool("isMoving", false);
        _animator.SetFloat("speed", _agent.velocity.magnitude / _agent.speed);
        _timePassed += Time.deltaTime;
        if (_timePassed >= _attackTime)
        {
            _audio.Play(0);
            if (_attackedType == AttackedType.BUILDING)
            {
               
                _attackedBuilding.Damage(_attack);
            }
            else
            {
                
                _attackedUnit.Damage(_attack);
            }
            _timePassed = 0f;
        }
    }

    private void CorrectDestinationPosition(Transform _currentDestination)
    {
        float distance = Vector3.Distance(_currentDestination.position, _agent.destination);
        if(distance > (2*_agent.stoppingDistance))
        {
            _agent.SetDestination(_currentDestination.position);
        }
    }
    private State CheckSwitchState()
    {
        //getting someone to attack if there is someone
        if(_attackList.Count > 0)
        {
            //Debug.Log("checking the list");
                foreach (GameObject possibleAim in _attackList)
                {
                    if (possibleAim != null && (_attackAim == possibleAim.transform || _attackAim == null))
                    {
                       // Debug.Log("setting up a new attacked");
                        _attackAim = possibleAim.transform;
                        CorrectDestinationPosition(_attackAim);
                        if (_attackAim.gameObject.layer == LayerMask.NameToLayer("buildings"))
                        {
                           // Debug.Log("is a building");
                            _attackedType = AttackedType.BUILDING;
                            _attackedBuilding = _attackAim.GetComponent<BasicBehaviour>();
                            
                            break;

                        }
                        else
                        {
                            //Debug.Log("is a unit");
                            _attackedType = AttackedType.UNIT;
                            _attackedUnit = _attackAim.GetComponent<UnitControllerStateMachine>();
                           
                            break;
                        }
                    }
                }
            }
        

        if (_movingAim == null)
        {
            
            return State.IDLE;
        }
        else if (_agent.hasPath && _agent.remainingDistance >= _agent.stoppingDistance)
        {
           
            return State.MOVING;
        }
        else if (_agent.hasPath &&  _agent.remainingDistance <= _agent.stoppingDistance && _attackAim != null)
        {
           
            return State.ATTACK;
        }

        return State.IDLE;

        
        
    }
    
    private Transform GetAim()
    {
        GameObject[] allGameObjects;
        allGameObjects = FindObjectsOfType(typeof(GameObject)) as GameObject[];

        foreach(GameObject gameObject in allGameObjects)
        {
            if(gameObject.layer == LayerMask.NameToLayer("buildings") && (gameObject.tag == "tower" || gameObject.tag == "farm" || gameObject.tag == "house" || gameObject.tag == "woodFacility"))
            {
                return gameObject.transform;
            }
        }
        return null;
    }

    public void Damage( int damage)
    {
        _currentHealth -= damage;

        Debug.Log("got Damaged ");
        if(_currentHealth <= 0)
        {
            Dead();
        }
    }
    private void Dead()
    {
        Destroy(gameObject);
    }

    public void Hit()
    {
        Rigidbody rigidBody;
        _agent.enabled = false;
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.isKinematic = false;
        StartCoroutine(ReactivateAfterHit());
    }

   

    IEnumerator ReactivateAfterHit()
    {
        yield return new WaitForSeconds(5);
        Rigidbody rigidBody;
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.isKinematic = true;
        NavMeshHit closestHit;
        if (NavMesh.SamplePosition(transform.position, out closestHit, 5, 1))
        {
            gameObject.transform.position = closestHit.position;

        }
        else
        {
            Debug.Log("To far to be alive ");
            Dead();
        }
        _agent.enabled = true;

    }
   


}
