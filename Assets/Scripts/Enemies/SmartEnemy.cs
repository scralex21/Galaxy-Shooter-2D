using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartEnemy : MonoBehaviour
{
    private Player _player;
    [SerializeField]
    private GameObject _explosionPrefab;
    private AudioSource _audioSource;
    private SpawnManager _spawnManager;

    [SerializeField]
    private float _speed = 4f;
    [SerializeField]
    private bool _followingPlayer;

    [SerializeField]
    private GameObject _enemyMissilePrefab;
    private float _fireRate = 1f;
    private float _canFire = -1f;
    

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("The Player is NULL");
        }

        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.Log("The Spawn Manager is NULL");
        }
    }

    void Update()
    {
        float xPos = _player.transform.position.x;
        float yPos = _player.transform.position.y;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(xPos, yPos - 4f),
            _speed * Time.deltaTime);


        if (transform.position.y == yPos - 4f)
        {
            _followingPlayer = true;
            StartCoroutine(EnemyFollow());
        }

        /*if (_speed == 0f)
        {
            _followingPlayer = false;

            if (Time.time > _canFire)
            {
                _canFire = Time.time + _fireRate;
                Instantiate(_enemyMissilePrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
            }

        }*/

        if (transform.position.y < -9.5f)
        {
            float randomx = Random.Range(-9.5f, 9.5f);
            transform.position = new Vector3(randomx, 7.5f, 0);
        }

    }

    IEnumerator EnemyFollow()
    {
        while (_followingPlayer == true)
        {
            yield return new WaitForSeconds(5.0f);
            _speed = 0;

            if (_speed == 0f)
            {
                if (Time.time > _canFire)
                {
                    _canFire = Time.time + _fireRate;
                    Instantiate(_enemyMissilePrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
                }
            }

            yield return new WaitForSeconds(4.0f);
            _followingPlayer = false;
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

            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _audioSource.Play();
            _spawnManager.UpdateEnemyCount();
            Destroy(this.gameObject);
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);

            if (_player != null)
            {
                _player.AddScore();
            }

            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            _audioSource.Play();
            _spawnManager.UpdateEnemyCount();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject);
        }

        if (other.tag == "Missile")
        {
            Destroy(other.gameObject);

            if (_player != null)
            {
                _player.AddScore();
            }

            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            _audioSource.Play();
            _spawnManager.UpdateEnemyCount();
            Destroy(this.gameObject);

        }

        if (other.tag == "Bomb")
        {
            if (_player != null)
            {
                _player.AddScore();
            }

            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            _audioSource.Play();
            _spawnManager.UpdateEnemyCount();
            Destroy(this.gameObject);
        }

        if (other.tag == "SuperLaser")
        {
            if (_player != null)
            {
                _player.AddScore();
            }

            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _audioSource.Play();
            _spawnManager.UpdateEnemyCount();
            Destroy(this.gameObject);
        }
    }
}
