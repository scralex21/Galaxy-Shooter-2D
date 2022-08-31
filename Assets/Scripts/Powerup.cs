using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // move down at the speed of 3
        // When we leave the screen, destroy object
   
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -5.75f)
        {
            Destroy(this.gameObject);
        }
    }

    //OnTriggerCollision
    //Only be collected by the Player (HINT: Use Tags)
    //on collected, destroy

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //communicate with player script
            Destroy(this.gameObject);
        }
    }

}
