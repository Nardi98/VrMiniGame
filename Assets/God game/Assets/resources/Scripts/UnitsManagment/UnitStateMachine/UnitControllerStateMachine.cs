using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum UnitType
{
    BUILDER,
    MINER,
    FARMER,
    WOODWORKER
};

[RequireComponent(typeof(NavMeshAgent))]
public class UnitControllerStateMachine : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public UnitBaseState _currentState;
    private StateFactory _states;

    public Animator _animator;
    


    private  int _maxHealth;
    protected int _currentHealth;
    [Header("Food consumption data")]
    public int _foodConsumed;
    public float _timeBetweenMeals;
    public int _starvationDamage;
    private float _timepassed = 0f;
    
    private int _strength;
    [Space]
    [Header("Work ability data")]
    [Range(0f, 3f)]
    public float _actionDistance;
    public int _workingFrequency;
    [Space]
    [Header("Idle behaviour data")]
    [Range(0f, 10f)]
    public float _idleMoveDistance = 1f;
    [Range(0f,30f)]
    public float _idleTime = 5f;
    [Space]
    [Header("Type Selection Attributes")]
    public UnitType _unitType = UnitType.BUILDER;
    [Range(0f, 2f)]
    public float _selectorRange;

    //tree manager to work with woodworker 
    public TreeManager _trees;

    private AudioSource _audio;

    protected NavMeshAgent _agent;
    private Transform _unitPosition;

    private Vector3? _unitDestination = null;
    private bool _unitIsWalking = false;

    private Transform _workingOn = null;
    private bool isGrabbed = false;


    private bool _isWalkingAnimation = false;
    private bool _isDeadAnimation = false;
    private bool _isWorkingAnimation = false;
    private bool _isIdleAnimation = false;



    public void UnitInitialization(int maxHealth, int strength, int foodConsumed, Vector3 position)
    {
        _maxHealth = maxHealth;
        _currentHealth = maxHealth;
        _strength = strength;
        _foodConsumed = foodConsumed;
        _unitPosition = transform;
        _unitPosition.position = position;

        


       

    }


        private void Awake()
    {

        

        _agent = GetComponent<NavMeshAgent>();
        
        _states = new StateFactory(this);

    }

    void Start()
    {
        _audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_agent.enabled)
        {
            NavMeshHit closestHit;
            if (NavMesh.SamplePosition(_unitPosition.position, out closestHit, 500, 1))
            {
                gameObject.transform.position = closestHit.position;

                //TODO

            }
            else
            {
                Debug.Log("problemi nel posizionare l'agent");
            }
            _agent.enabled = true;
            ChangeStoppingDistance();
            _currentState = _states.Builder();
            _currentState.EnterState();
        }
        else
        {

            _timepassed += Time.deltaTime;
           

            if (_unitDestination != null)
            {



                float distance = Vector3.Distance(_unitPosition.position, (Vector3)_unitDestination);


                if (distance <= _actionDistance)
                {

                    StopWalking();
                }
                else
                {

                    StartWalking();
                }
            }




            _currentState.UpdateStates();
            if (_timepassed >= _timeBetweenMeals)
            {
                _timepassed = 0f;
                Eat();
            }

            if (_currentHealth <= 0)
            {
                StartCoroutine(Dead());
                return;
            }
        }
       
    }

    //movement functions
    public void SetDestination(Transform destination)
    {
        if (destination != null)
        {
            if (_unitDestination != destination.position)
            {
                _unitDestination = destination.position;
                _agent.SetDestination((Vector3)_unitDestination);
                StartWalking();
            }
        }
    }
    public void SetDestination(Vector3 destination)
    {
        if (_unitDestination != destination)
        {
            _unitDestination = destination;
            _agent.SetDestination((Vector3)_unitDestination);
            StartWalking();
        }
    }
    //end of movement functions

    //state functions
    private void StartWalking()
        {
        _agent.isStopped = false;
        _unitIsWalking = true;
        }

    public void StopWalking()
    {
        _agent.isStopped = true;
        _unitIsWalking = false;
    }

    public void ChangeStoppingDistance()
    {
        _agent.stoppingDistance = _actionDistance;
    }
   
    
    public void grabbed()
    {
        isGrabbed = true;
    }
    public void released()
    {
        isGrabbed = false;
    }
    // end of state functions


    //behavioural functions

    IEnumerator Dead()
    {
        StopWalking();
        SetAnimationIsDead(true);
        SetAnimationIsIdle(false);
        SetAnimationIsWalking(false);
        SetAnimationIsWorking(false);
        _currentState.ExitStates();

        yield return new WaitForSeconds(10);

        Destroy(gameObject);
    }

    //function that access to the gloabal variables Globals check the food avaliable and if it is higher then the food needed removes 
    //that ammount, if it is lower removes some health
    private void Eat()
    {
        if (Globals.GAME_RESOURCE["food"].Amout > _foodConsumed)
        {
            Globals.GAME_RESOURCE["food"].AddAmount( -_foodConsumed);


            Damage(-_starvationDamage);
            if(_currentHealth > _maxHealth)
            {
                _currentHealth = _maxHealth;
            }
        }
        else
        {

            Damage(_starvationDamage);
        }
    }

    public void Damage(int damage)
    {
        Debug.Log($"{gameObject.name} damage {damage} HP = {_currentHealth}");
        _currentHealth -= damage;
    }

    public void SetAnimationIsWalking(bool isWalking)
    {
        _animator.SetBool("isWalking", isWalking);
        _isWalkingAnimation = isWalking;
    }

    public void SetAnimationIsWorking(bool isWorking)
    {
        _animator.SetBool("isWorking", isWorking);
        _isWorkingAnimation = isWorking;
    }

    public void SetAnimationIsIdle(bool isIdle)
    {
        _animator.SetBool("isIdle", isIdle);
        _isIdleAnimation = isIdle;
    }

    public void SetAnimationIsDead(bool isDead)
    {
        _animator.SetBool("isDead", isDead);
        _isDeadAnimation = isDead;
    }

    public bool IsGrabbed { get { return isGrabbed; } }
    public UnitBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public Transform UnitPosition { get { return transform; } }
    public Transform WorkingOn { get { return _workingOn ; } set { _workingOn = value; } }
    public int Strength { get { return _strength; } }
    public int WorkingFrequency { get { return _workingFrequency; } }
    public bool IsWalking { get { return _unitIsWalking; } }
    public float SelectorRange { get { return _selectorRange; } }
    public UnitType UnitType { get { return _unitType; } set { _unitType = value; } }
    public TreeManager Trees { get { return _trees; } }
    public AudioSource Audio { get { return _audio; } }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere (transform.position, _actionDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _selectorRange);
    }
}
