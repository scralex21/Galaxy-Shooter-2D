using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private Animator _bombExplosion;

    private void Start()
    {
        _bombExplosion = GetComponent<Animator>();

        if (_bombExplosion == null)
        {
            Debug.LogError("The Bomb Explosion is NULL");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            _bombExplosion.SetTrigger("OnBombExplosion");
            Destroy(this.gameObject);
            
        }
    }
}
