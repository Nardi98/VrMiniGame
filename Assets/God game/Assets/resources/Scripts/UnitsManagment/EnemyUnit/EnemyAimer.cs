using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAimer : MonoBehaviour
{
    private List<GameObject> _attackList = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Transform GetAim()
    {
        GameObject[] allGameObjects;
        allGameObjects = FindObjectsOfType(typeof(GameObject)) as GameObject[];

        foreach (GameObject gameObject in allGameObjects)
        {
            if (gameObject.layer == LayerMask.NameToLayer("buildings") && (gameObject.tag == "tower" || gameObject.tag == "farm" || gameObject.tag == "house" || gameObject.tag == "woodFacility"))
            {
                return gameObject.transform;
            }
        }
        return null;
    }

    public List<GameObject> AttackList { get { return _attackList; } }


    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "house" || other.gameObject.tag == "farm" || other.gameObject.tag == "woodFacility" || other.gameObject.tag == "unit" || other.gameObject.tag == "tower")
        {

            _attackList.Add(other.gameObject);
        }


    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.tag == "house" || other.gameObject.tag == "farm" || other.gameObject.tag == "woodFacility" || other.gameObject.tag == "unit" || other.gameObject.tag == "tower")
        {

            _attackList.Remove(other.gameObject);
        }


    }
}
