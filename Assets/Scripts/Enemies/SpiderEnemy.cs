using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderEnemy : MonoBehaviour
{
    private Player _player;
    [SerializeField]
    private GameObject _explosionPrefab;
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _enemyLaserAudio;
    private AudioSource _enemyLaserAudioSource;
    private SpawnManager _spawnManager;

    private float _speed = 5.5f;
    private int _enemyMovement;

    [SerializeField]
    private GameObject _spiderEnemyLaserPrefab;
    private float _fireRate = 3f;
    private float _canFire = -1f;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _enemyLaserAudioSource = GetComponent<AudioSource>();
        _enemyLaserAudioSource.clip = _enemyLaserAudio;

        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.Log("The Player is NULL");
        }

        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.Log("The Spawn Manager is NULL");
        }

        _enemyMovement = Random.Range(1, 3);
    }


    void Update()
    {
        Movement();
        Attack();
    }

    void Movement()
    {
        if (_enemyMovement == 1)
        {
            transform.Translate(Vector3.left * _speed * Time.deltaTime);
        }

        if (_enemyMovement == 2)
        {
            transform.Translate(Vector3.right * _speed * Time.deltaTime);
        }

        if (transform.position.x > 11.75f)
        {
            float randomx = Random.Range(-1f, 5.60f);
            transform.position = new Vector3(-11.75f, randomx, 0);
        }

        if (transform.position.x < -11.75f)
        {
            float randomy = Random.Range(-1f, 5.60f);
            transform.position = new Vector3(11.75f, randomy, 0);
        }
    }

    void Attack()
    {
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_spiderEnemyLaserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }

            _enemyLaserAudioSource.Play();
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
