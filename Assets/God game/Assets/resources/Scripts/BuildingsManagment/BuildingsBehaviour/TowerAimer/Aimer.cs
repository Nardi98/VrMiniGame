using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aimer : MonoBehaviour
{
    private List<EnemyUnitController> Enemies = new List<EnemyUnitController>();
    // Start is called before the first frame update

    public EnemyUnitController? GetAim()
    {
        if (Enemies.Count > 0)
        {
            
        while (Enemies[0] == null)
        {
            Debug.Log($"the first enemy is null{Enemies.Count}");
            Enemies.Remove(Enemies[0]);
            if(Enemies.Count == 0)
                {
                    break;
                }
        } 
        }
        
        if (Enemies.Count > 0)
        {
            
            Debug.Log(Enemies[0]);
            return Enemies[0];
        }
        else
        {
            return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "enemyUnit")
        {
            Debug.Log("something entered");
            Enemies.Add(other.gameObject.GetComponent<EnemyUnitController>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "enemyUnit")
        {
            Debug.Log("something when went out");
            Enemies.Remove(other.gameObject.GetComponent<EnemyUnitController>());
        }
    }

}
