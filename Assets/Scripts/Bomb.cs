using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private Animator _bombExplosion;
    private float _rotateSpeed = 20f;

    private void Start()
    {
        _bombExplosion = GetComponent<Animator>();

        if (_bombExplosion == null)
        {
            Debug.LogError("The Bomb Explosion is NULL");
        }
    }

    private void Update()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            _bombExplosion.SetTrigger("OnBombExplosion");
            Destroy(this.gameObject, 0.65f);
            
        }
    }
}
