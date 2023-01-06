using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggressiveEnemy : MonoBehaviour
{
    private Player _player;
    [SerializeField]
    private GameObject _explosionPrefab;
    private AudioSource _audioSource;
    private SpawnManager _spawnManager;

    private float _speed = 3f;
    [SerializeField]
    private float _range = 6f;

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
        Movement();

        if (_player != null)
        {
            AggressiveAttack();
        }
    }

    void Movement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -5.5f)
        {
            float randomx = Random.Range(-9.5f, 9.5f);
            transform.position = new Vector3(randomx, 7.5f, 0);
        }
    }

    void AggressiveAttack()
    {
        if (Vector3.Distance(_player.transform.position, transform.position) < _range)
        {
            transform.position = Vector3.MoveTowards(transform.position, _player.transform.position,
                _speed * Time.deltaTime);

            if (transform.position.y < _player.transform.position.y)
            {
                transform.Translate(Vector3.down * _speed * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && _player != null)
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
