using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteMissile : MonoBehaviour
{
    private float _speed = 5f;
    private bool _isBossMissile;

    private void Start()
    {
        if (_isBossMissile != true)
        {
            transform.Rotate(0, 0, 180f);
        }
    }

    void Update()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y < -6f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(gameObject);
        }
    }

    public void AssignBossMissile()
    {
        _isBossMissile = true;
    }
}
