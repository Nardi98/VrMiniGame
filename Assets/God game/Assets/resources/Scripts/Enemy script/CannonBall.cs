using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    public int _damage;
    [Range(5f, 30f)]
    public float _lifeDuration;
    private float _timePassed = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _timePassed += Time.deltaTime;
        if(_timePassed > _lifeDuration)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 8)
        {
            Debug.Log($"hit {collision.gameObject.name} damage {(int)(_damage * (GetComponent<Rigidbody>().velocity.magnitude))}");
            collision.gameObject.GetComponent<BasicBehaviour>().Damage((int)(_damage * (GetComponent<Rigidbody>().velocity.magnitude)));
        }
        if(collision.gameObject.layer == LayerMask.NameToLayer("enemyUnits"))
        {
            EnemyUnitController enemy;
            enemy = collision.gameObject.GetComponent<EnemyUnitController>();
            enemy.Hit();
            enemy.Damage((int)((_damage * (GetComponent<Rigidbody>().velocity.magnitude))/6));
        }
    }
}
