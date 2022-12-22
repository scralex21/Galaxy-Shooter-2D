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

    private bool _isRarePowerUpActive = false;

    [SerializeField]
    private GameObject _missilePrefab;
    [SerializeField]
    private int _missileCount = 3;
    private bool _isHomingMissileActive = false;

    [SerializeField]
    private GameObject _bombPrefab;
    private bool _isBombActive;

    [SerializeField]
    private GameObject _superLaser;
    private bool _isSuperLaserActive = false;

    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;

    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldActive = false;
    [SerializeField]
    private bool _isSlowSpeedActive = false;

    [SerializeField]
    private GameObject _thruster;
    [SerializeField]
    private bool _isThrustersActive = false;
    [SerializeField]
    private float _thrusterPower = 100f;
    [SerializeField]
    private float _thrusterUsage = 0.1f;


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

    private MainCamera _cameraShake;
    private Powerup _powerUp;

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);

        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _uiManaager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _cameraShake = GameObject.Find("Main Camera").GetComponent<MainCamera>();
        _audioSource = GetComponent<AudioSource>();
        //_powerUp = GameObject.FindGameObjectWithTag("PowerUp").GetComponent<Powerup>();


        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL");
        }

        if (_uiManaager == null)
        {
            Debug.LogError("The UI Manager is NULL");
        }

        if (_cameraShake == null)
        {
            Debug.LogError("The Main Camera Shake is NULL");
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
        PowerUpDetect();


        if (_isRarePowerUpActive != true)
        {
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

        else
        {
            //Rare Powerup Functions, When Rare Powerup is true, these become the function of the space key
            HomingMissileFire();
            Bomb();
            SuperLaser();
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

        if (_superLaser.activeInHierarchy || _isSlowSpeedActive == true)
        {
            transform.Translate(Vector3.right * horizontalInput * (_speed / 2) * Time.deltaTime);
            transform.Translate(Vector3.up * verticalInput * (_speed / 2) * Time.deltaTime);

            if (Input.GetKey(KeyCode.LeftShift))
            {
                _uiManaager.ThrusterMalfunctionOn();
                return;
            }
        }

        else
        {
            _uiManaager.ThrusterMalfunctionOff();
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

        if (Input.GetKey(KeyCode.LeftShift) && _thrusterPower > 0)
        {
            transform.Translate(Vector3.right * horizontalInput * _speed * 2 * Time.deltaTime);
            transform.Translate(Vector3.up * verticalInput * _speed * 2 * Time.deltaTime);
            _thruster.SetActive(true);
            StartCoroutine(ThrusterActive());
        }

        else
        {
            _thruster.SetActive(false);
            StartCoroutine(ThrusterRegen());
        }

    }

    IEnumerator ThrusterActive()
    {
        _isThrustersActive = true;

        while (Input.GetKey(KeyCode.LeftShift) && _thrusterPower > 0)
        {
            yield return null;
            _thrusterPower -= _thrusterUsage * Time.deltaTime;
            _uiManaager.ThrusterBar(_thrusterPower);
        }
    }

    IEnumerator ThrusterRegen()
    {
        _isThrustersActive = false;

        if (_thrusterPower < 100 && _isThrustersActive == false)
        {
            yield return new WaitForSeconds(3.0f);
        }

        while (_thrusterPower < 100 && _isThrustersActive == false)
        {
            yield return null;
            _thrusterPower += _thrusterUsage * Time.deltaTime;
            _uiManaager.ThrusterBar(_thrusterPower);
        }
    }

    void PowerUpDetect()
    {
        RaycastHit2D powerupDetect = Physics2D.CircleCast(transform.position, 5.5f, Vector3.down,
         LayerMask.GetMask("PowerUps"));

        if (powerupDetect.collider != null)
        {
            if (powerupDetect.collider.CompareTag("PowerUp"))
            {
                Debug.Log("The Powerup is in range");
                if (Input.GetKey(KeyCode.C))
                {
                    _powerUp.MoveToPlayer();
                }
            }
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
        _cameraShake.CameraShake();

        switch (_lives)
        {
            case 2:
                _rightEngineDamage.SetActive(true);
                break;
            case 1:
                _leftEngineDamage.SetActive(true);
                break;
            case 0:
                _spawnManager.OnPlayerDeath();
                Destroy(this.gameObject);
                break;
        }
        _uiManaager.UpdateLives(_lives);
    }

     public void HealthCollected()
    {
        _lives = _lives + 1;
        
        if (_lives >= 3)
        {
            _lives = 3;
            _rightEngineDamage.SetActive(false);
        }
        else if (_lives == 2)
        {
            _leftEngineDamage.SetActive(false);
        }

        _uiManaager.UpdateLives(_lives);   
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

    public void SlowSpeedActive()
    {
        _isSlowSpeedActive = true;
        StartCoroutine(SlowSpeedPowerDown());
    }

    IEnumerator SlowSpeedPowerDown()
    {
        yield return new WaitForSeconds(5.0f);
        _isSlowSpeedActive = false;
        _uiManaager.ThrusterMalfunctionOff();
    }

    public void HomingMissileActive()
    {
        _isRarePowerUpActive = true;
        _isHomingMissileActive = true;
        _uiManaager.MissilePowerupOn();
        _missileCount = 3;
    }

    void HomingMissileFire()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _isHomingMissileActive == true)
        {
            Instantiate(_missilePrefab, transform.position + new Vector3(0, 1.10f, 0), Quaternion.identity);
            _missileCount = _missileCount - 1;
            _uiManaager.MissileCount(_missileCount);

            if (_missileCount == 0)
            {
                _isRarePowerUpActive = false;
                _isHomingMissileActive = false;
                _uiManaager.MissilePowerupOff();
            }
        }
    }

    public void BombActive()
    {
        _isRarePowerUpActive = true;
        _isBombActive = true;
        _uiManaager.BombPowerupOn();
    }

    void Bomb()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _isBombActive == true)
        {
            Instantiate(_bombPrefab, transform.position + new Vector3(0, -1.25f, 0), Quaternion.identity);

            if (Input.GetKeyDown(KeyCode.Space))

            {
                _isRarePowerUpActive = false;
                _isBombActive = false;
                _uiManaager.BombPowerUpOff();
            }
        }
    }

    public void SuperLaserActive()
    {
        _isRarePowerUpActive = true;
        _isSuperLaserActive = true;
        _uiManaager.SuperLaserPowerUpOn();
    }

    void SuperLaser()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _isSuperLaserActive == true)
        {
            _superLaser.SetActive(true);
            _uiManaager.SuperLaserPowerUpOff();
            StartCoroutine(SuperLaserCooldown());
        }
    }

    IEnumerator SuperLaserCooldown()
    {
        yield return new WaitForSeconds(5.0f);
        _superLaser.SetActive(false);

        _isSuperLaserActive = false;
        _isRarePowerUpActive = false;
    }

    public void AddScore()
    {
        _score = _score + 10;
        _uiManaager.UpdateScore(_score);
    }
}    
    

