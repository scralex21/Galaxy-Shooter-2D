using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        //move down at 4 meters per second
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        
        //if bottom of screen
        //respawn at top 
        if (transform.position.y <= -5.5f)
        {
            float randomx = Random.Range(-9.5f, 9f);
            transform.position = new Vector3(randomx, 7.5f, 0);
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        //if other is Player
        //damage the player
        //Destroy us

        //if other is laser
        //laser
        //destory us
    }
}
