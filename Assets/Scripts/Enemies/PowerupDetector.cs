using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupDetector : MonoBehaviour
{
    private Enemy _enemy;

    private void Start()
    {
        _enemy = GetComponentInParent<Enemy>();

        if (_enemy == null)
        {
            Debug.LogError("Enemy is Null");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PowerUp")
        {
            _enemy.FireAtPowerup();
        }
    }
}
