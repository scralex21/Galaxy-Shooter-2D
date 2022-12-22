using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteEnemy : MonoBehaviour
{
    private Player _player;
    [SerializeField]
    private GameObject _explosionPrefab;
    private AudioSource _audioSource;
    private SpawnManager _spawnManager;

    private float _speed = 4f;
    private float _fireRate = 3f;
    private float _canFire = -1f;

    [SerializeField]
    private GameObject _eliteMissilePrefab;


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
        Avoid();
        Attack();
    }

    void Movement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -5.5f)
        {
            float randomx = Random.Range(-9.5f, 9f);
            transform.position = new Vector3(randomx, 7.5f, 0);
        }
    }

    void Avoid()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 3f, Vector3.down,
        LayerMask.GetMask("Laser"));

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Laser"))
            {
                Debug.Log("The Player Laser was detected!");
                transform.Translate(Vector3.right * (_speed * 3) * Time.deltaTime);
            }
        }
    }

    void Attack()
    {
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            Instantiate(_eliteMissilePrefab, transform.position + new Vector3(0, -0.85f, 0), Quaternion.identity);
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
