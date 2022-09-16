using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
    [SerializeField]
    private GameObject _enemyLaserPrefab;

    private Player _player;
    // handle to animator component
    
    private Animator _enemyExplosion;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnemyFireLaser());

        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("The Player is NULL");
        }
        // assign the component
        _enemyExplosion = GetComponent<Animator>();
        if (_enemyExplosion == null)
        {
            Debug.LogError("The Animator is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        EnemyMovement();
    }

    void EnemyMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -5.5f)
        {
            float randomx = Random.Range(-9.5f, 9f);
            transform.position = new Vector3(randomx, 7.5f, 0);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }

            //trigger animation
            _enemyExplosion.SetTrigger("OnEnemyDeath");
            _speed = 0;
            Destroy(this.gameObject, 2.37f);
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            // Add 10 to score
            if (_player != null)
            {
                _player.AddScore();
            }
            //trigger animation
            _enemyExplosion.SetTrigger("OnEnemyDeath");
            _speed = 0;
            Destroy(this.gameObject, 2.37f);
        }
    }

    IEnumerator EnemyFireLaser()
    {
        var _randomFire = Random.Range(5f, 7f);
        Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(_randomFire);
    }
}

   
