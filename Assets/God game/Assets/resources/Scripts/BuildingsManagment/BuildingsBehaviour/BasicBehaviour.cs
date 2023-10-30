using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasicBehaviour : MonoBehaviour
{
    public bool _invulnerable = false;
    // Start is called before the first frame update
    protected Building _thisBuilding = null;
    [Range(100, 10000)]
    public int _maxHealth = 100;
    private int _currentHealth;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }
    // Update is called once per frame
    void Update()
    {
        if(_thisBuilding != null)
        _invulnerable = _thisBuilding.Invulnerable();
       // Debug.Log($" {gameObject.name} current health = {_currentHealth}");
        if(_currentHealth <= 0)
        {
            
            Destroy(gameObject);
        }
        if (_thisBuilding != null) { 
            if (_thisBuilding.IsCompleted )
            {
                if (_currentHealth <= 0)
                {
                    Destroy(gameObject);
                }
                UpdateBehaviour();
            }
        }
        else
        {
            if (_currentHealth <= 0)
            {
                Destroy(gameObject);
            }
            UpdateBehaviour();
        }
    }

    protected abstract void UpdateBehaviour();

    public void Damage(int damage)
    {
        
        if (!_invulnerable)
        {
            Debug.LogWarning($"damaged {damage} current life {_currentHealth}");
            _currentHealth -= damage;
        }
    }

    public Building ThisBuilding { set { _thisBuilding = value; } }
}
