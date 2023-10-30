using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [Range(0f, 300f)]
    public float _generationTime = 0;
    private float _passedTime = 0;

    public int generationBurst;
    // Start is called before the first frame update
    void Start()
    {
        _generationTime = Random.Range(1f, 3f) * _generationTime;
    }

    // Update is called once per frame
    void Update()
    {
        _passedTime += Time.deltaTime;

        if(_passedTime > _generationTime)
        {
            generationBurst++;
            _generationTime = Random.Range(0.5f, 1.5f) * _generationTime;
            _passedTime = 0f;
            for(int i = 0; i < generationBurst; i++)
            {
                StartCoroutine(Generator(i));
            }
        }
    }

    IEnumerator  Generator(int i)
    {
        yield return new WaitForSeconds(i);
        GameObject generatedEnemy = GameObject.Instantiate(Resources.Load("prefabs/Units/EnemyUnit/EnemyUnit") as GameObject, transform.position, transform.rotation);
    }
}
