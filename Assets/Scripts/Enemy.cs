using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
    [SerializeField]
    private GameObject _enemyLaserPrefab;
    private float _fireRate = 3f;
    private float _canFire = -1f;

    private Player _player;
    private Animator _enemyExplosion;

    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _enemyLaserAudio;
    [SerializeField]
    private AudioClip _explosionAudio;
    private AudioSource _enemyLaserAudioSource;
    private SpawnManager _spawnManager;
    [SerializeField]
    private int _enemyMovement;

    [SerializeField]
    private GameObject _shield;
    [SerializeField]
    private bool _isShieldActive;
    private int _shieldID;

    private bool _enemyDestroyed;

    void Start()
    {
        _enemyMovement = Random.Range(1, 5);
        _shieldID = Random.Range(1, 5);
        if (_shieldID == 1)
        {
            EnemyShield();
        }

        _audioSource = GetComponent<AudioSource>();
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.Log("The Spawn Manager is NULL");
        }

        _enemyLaserAudioSource = GetComponent<AudioSource>();
        _enemyLaserAudioSource.clip = _enemyLaserAudio;

        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.Log("The Player is NULL");
        }

        _enemyExplosion = GetComponent<Animator>();
        if (_enemyExplosion == null)
        {
            Debug.Log("The Animator is NULL");
        }
    }

    void Update()
    {
        EnemyMovement();
        EnemyFire();
    }

    void EnemyMovement()
    {
        switch (_enemyMovement)
        {
            case 1:
                transform.Translate((Vector3.down + Vector3.left) * (_speed / 2) * Time.deltaTime);
                break;
            case 2:
                transform.Translate((Vector3.down + Vector3.right) * (_speed / 2) * Time.deltaTime);
                break;
            case 3:
                transform.Translate(Vector3.down * _speed * Time.deltaTime);
                break;
            case 4:
                transform.Translate(Vector3.right * _speed * Time.deltaTime);
                break;
            case 5:
                transform.Translate(Vector3.left * _speed * Time.deltaTime);
                break;
            default:
                break;
        }

        if (transform.position.y < -5.5f)
        {
            float randomx = Random.Range(-9.5f, 9f);
            transform.position = new Vector3(randomx, 7.5f, 0);
        }

        if (transform.position.y > 7.5f)
        {
            float randomy = Random.Range(-1f, 5.60f);
            transform.position = new Vector3(11f, randomy, 0);
        }

        if (transform.position.x > 11.75f)
        {
            float randomy = Random.Range(-1f, 5.60f);
            transform.position = new Vector3(-11.75f, randomy, 0);
        }

        if (transform.position.x < -11.75f)
        {
            float randomy = Random.Range(-1f, 5.60f);
            transform.position = new Vector3(11.75f, randomy, 0);
        }
    }

    void EnemyFire()
    {
        if (Time.time > _canFire && _enemyDestroyed == false)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            
            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }

            _enemyLaserAudioSource.Play();
        }
    }

    public void FireAtPowerup()
    {
        GameObject enemyLaser = Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity);
        Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

        for (int i = 0; i < lasers.Length; i++)
        {
            lasers[i].AssignEnemyLaser();
        }

        _enemyLaserAudioSource.Play();

    }

    void EnemyShield()
    {
        _isShieldActive = true;
        _shield.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && _isShieldActive == true)
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }

            _isShieldActive = false;
            _shield.SetActive(false);
        }

        if (other.tag == "Player" && _isShieldActive == false)
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();

            }

            _enemyDestroyed = true;
            _enemyExplosion.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            _spawnManager.UpdateEnemyCount();

            Destroy(this.gameObject, 2.37f);
        }

        if (other.tag == "Laser" && _isShieldActive == true)
        {
            Destroy(other.gameObject);
            _isShieldActive = false;
            _shield.SetActive(false);
            return;
        }

        if (other.tag == "Laser" && _isShieldActive == false)
        {
            Destroy(other.gameObject);

            if (_player != null)
            {
                _player.AddScore();
            }

            _enemyDestroyed = true;
            _enemyExplosion.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            _spawnManager.UpdateEnemyCount();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.37f);
        }

        if (other.tag == "Missile" && _isShieldActive == true)
        {
            Destroy(other.gameObject);
            _isShieldActive = false;
            _shield.SetActive(false);
            return;
        }

        if (other.tag == "Missile" && _isShieldActive == false)
        {
            Destroy(other.gameObject);

            if (_player != null)
            {
                _player.AddScore();
            }

            _enemyDestroyed = true;
            _enemyExplosion.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            _spawnManager.UpdateEnemyCount();
            Destroy(this.gameObject, 2.37f);

        }

        if (other.tag == "Bomb")
        {
            if (_player != null)
            {
                _player.AddScore();
            }

            _enemyDestroyed = true;
            _enemyExplosion.SetTrigger("OnEnemyDeath");
            _shield.SetActive(false);
            _speed = 0;
            _audioSource.Play();
            _spawnManager.UpdateEnemyCount();
            Destroy(this.gameObject, 2.37f);
        }

        if (other.tag == "SuperLaser")
        {
            if (_player != null)
            {
                _player.AddScore();
            }

            _enemyDestroyed = true;
            _enemyExplosion.SetTrigger("OnEnemyDeath");
            _shield.SetActive(false);
            _speed = 0;
            _audioSource.Play();
            _spawnManager.UpdateEnemyCount();
            Destroy(this.gameObject, 2.37f);
        }

       
    }
}

   
