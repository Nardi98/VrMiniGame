using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBehaviour : BasicBehaviour
{
    [Range(0f, 60f)]
    public float _cannonInterval;
    private float _cannonIntervalRandom;
    public GameObject _cannonBall;
    private float _timePassed = 0f;
    public float time;
    private Vector3? aim;


    public Transform _shootingPoint;

    private AudioSource _audio;



    // Start is called before the first frame update
    void Start()
    {
        _audio =  GetComponent<AudioSource>();
        _cannonIntervalRandom = _cannonInterval * Random.Range(0.2f, 5f);
       
    }

    // Update is called once per frame


    protected override void UpdateBehaviour()
    {
        _timePassed += Time.deltaTime;
        if(_timePassed > _cannonIntervalRandom)
        {


             

            aim = GetTarget();
           
            _timePassed = 0f;
            _cannonIntervalRandom = _cannonInterval * Random.Range(0.2f, 5f);

            if (aim != null)
            {

                _audio.Play(0);
                GameObject cannonBall = GameObject.Instantiate(_cannonBall, _shootingPoint.position, transform.rotation);
                cannonBall.GetComponent<Rigidbody>().velocity = Aim((Vector3) aim);

            }
        }

    }

    private Vector3 Aim( Vector3 aim)
    {
        Vector3 distance = aim - transform.position;
        Vector3 distanceXZ = distance;
        distanceXZ.y = 0f;
        // create a float for our distance
        float Sy = distance.y;
        float Sxz = distance.magnitude;

        float Vxz = Sxz / time;
        float Vy = Sy/ time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;

        Vector3 result = distanceXZ.normalized;
        result *= Vxz;
        result.y = Vy - 0.7f;

        return result;

    }

 

private Vector3? GetTarget()
{
         BasicBehaviour[] allBuildings;
         List<GameObject> playerBuildings = new List<GameObject>();
         Vector3? aimPosition;

  
            
            
           allBuildings = Object.FindObjectsOfType<BasicBehaviour>();

        if (allBuildings.Length > 0) {
                foreach (BasicBehaviour singleBuilding in allBuildings)
                {
                GameObject singleGameObject = singleBuilding.gameObject;
                if (singleGameObject.layer == LayerMask.NameToLayer("buildings") && (singleGameObject.tag != "worldBuildings" && singleGameObject.tag != "stoneDeposit"))
                {
                    playerBuildings.Add(singleGameObject);
                    
                    }
                }
            if (playerBuildings.Count > 0)
            {
                int index = Random.Range(0, playerBuildings.Count);
                
                aimPosition = playerBuildings[index].transform.position;
                return aimPosition;
            }
            return null;
    }
        return null;
        
}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        if (aim != null)
        {
            Gizmos.DrawSphere((Vector3)aim, 0.5f);
        }
    }
}

