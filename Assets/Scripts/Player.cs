using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private int _ammoCount = 30;
    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire = -1f;

    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;

    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldActive = false;

    [SerializeField]
    private GameObject _thruster;

    [SerializeField]
    private int _shieldStrength;
    [SerializeField]
    private GameObject _shieldVisual;
    [SerializeField]
    private SpriteRenderer _shieldStatus;

    [SerializeField]
    private int _score;
    private UIManager _uiManaager;
    
    [SerializeField]
    private GameObject _rightEngineDamage;
    [SerializeField]
    private GameObject _leftEngineDamage;

    [SerializeField]
    private AudioClip _laserAudio;
    [SerializeField]
    private AudioClip _noAmmoAudio;
    private AudioSource _audioSource;

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);

        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _uiManaager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL");
        }

        if (_uiManaager == null)
        {
            Debug.LogError("The UI Manager is NULL");
        }

        if (_audioSource == null)
        {
            Debug.LogError("The Audio Source is NULL");
        }
        else
        {
            _audioSource.clip = _laserAudio;
        }
    }

    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            if (_ammoCount == 0)
            {
                AudioSource.PlayClipAtPoint(_noAmmoAudio, transform.position);
                return;
            }

            FireLaser();
        }

    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (_isSpeedBoostActive == true)
        {
            transform.Translate(Vector3.right * horizontalInput * _speed * 2 * Time.deltaTime);
            transform.Translate(Vector3.up * verticalInput * _speed * 2 * Time.deltaTime);

            if (Input.GetKey(KeyCode.LeftShift))
            {
                transform.Translate(Vector3.right * horizontalInput * ((_speed * 2) * 2) * Time.deltaTime);
                transform.Translate(Vector3.up * verticalInput * ((_speed * 2) * 2) * Time.deltaTime);
            }
        }
        else
        {
            transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);
            transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);
        }

        if (transform.position.y >= 6.0f)
        {
            transform.position = new Vector3(transform.position.x, 6.0f, 0);
        }
        else if (transform.position.y <= -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }

        if (transform.position.x > 11.0f)
        {
            transform.position = new Vector3(-11.0f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.0f)
        {
            transform.position = new Vector3(11.0f, transform.position.y, 0);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.Translate(Vector3.right * horizontalInput * _speed * 2 * Time.deltaTime);
            transform.Translate(Vector3.up * verticalInput * _speed * 2 * Time.deltaTime);
            _thruster.SetActive(true);
        }
        else
        {
            _thruster.SetActive(false);
        }
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_ammoCount <= 30)
        {

            if (_isTripleShotActive == true)
            {
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
                _ammoCount = _ammoCount - 3;
                _uiManaager.UpdateAmmo(_ammoCount);
            }
            else
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
                _ammoCount = _ammoCount - 1;
                _uiManaager.UpdateAmmo(_ammoCount);
            }

            AmmoCap();
        }

        _audioSource.Play();
    }

    public void AmmoCollected()
    {
        _ammoCount = _ammoCount + 10;
        AmmoCap();
        _uiManaager.UpdateAmmo(_ammoCount);
    }

    void AmmoCap()
    {
        if (_ammoCount <= 0)
        {
            _ammoCount = 0;
        }
        else if ( _ammoCount > 30)
        {
            _ammoCount = 30;
        }
    }

    public void Damage()
    {    
        if (_isShieldActive == true && _shieldStrength <= 3)
        {
            ShieldStrength();
            return;
        }

        _lives = _lives - 1;

        if (_lives == 2)
        {
            _rightEngineDamage.SetActive(true);
        }
        else if (_lives == 1)
        {
            _leftEngineDamage.SetActive(true);
        }

        _uiManaager.UpdateLives(_lives);

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "EnemyLaser")
        {
            Damage();
            Destroy(other.gameObject);
        }
        if (other.tag == "EnemyLaserPair")
        {
            Destroy(other.gameObject);
        }
    }

    public void ShieldActive()
    {
        if (_isShieldActive != true)
        {
            _isShieldActive = true;
            _shieldStrength = 3;
            _shieldVisual.SetActive(true);
            _shieldStatus.color = Color.cyan;
        }
    }

    void ShieldStrength()
    {
        _shieldStrength = _shieldStrength - 1;

        switch (_shieldStrength)
        {  
            case 2:
                _shieldStatus.color = Color.yellow;
                break;
            case 1:
                _shieldStatus.color = Color.red;
                break;
            case 0:
                _shieldVisual.SetActive(false);
                _isShieldActive = false;
                break;
        }

    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDown());
    }

    IEnumerator TripleShotPowerDown()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        StartCoroutine(SpeedBoostPowerDown());
    }

    IEnumerator SpeedBoostPowerDown()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
    }

    public void AddScore()
    {
        _score = _score + 10;
        _uiManaager.UpdateScore(_score);
    }
}    
    

