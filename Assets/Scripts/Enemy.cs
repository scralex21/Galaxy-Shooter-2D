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

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnemyFireLaser());

        _player = GameObject.Find("Player").GetComponent<Player>();
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

            Destroy(this.gameObject);
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            // Add 10 to score
            if (_player != null)
            {
                _player.AddScore();
            }
            Destroy(this.gameObject);
        }
    }

    IEnumerator EnemyFireLaser()
    {
        var _randomFire = Random.Range(5f, 7f);
        Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(_randomFire);
    }
}

   
