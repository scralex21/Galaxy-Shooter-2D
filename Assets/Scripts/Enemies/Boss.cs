using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public enum BossAction 
    { 
        Start,
        Wait,
        Move,
        Attack1, // Boss Laser - Right To Left
        Attack2, // Boss Laser - Left To Right
        Attack3 // Thrust Attack
    }

    [SerializeField]
    private BossAction _action = BossAction.Start;

    private float _speed = 3f;

    [SerializeField]
    private Vector3[] _bossPos;
    [SerializeField]
    private bool _posChosen;
    [SerializeField]
    private int _randomIndex;

    [SerializeField]
    private GameObject _bossMissilePrefab;
    private float _fireRate = 3f;
    private float _canFire = -1f;

    [SerializeField]
    private GameObject _bossLaser;
    [SerializeField]
    private bool _isBossLaserActive;

    private float _endPoint;

    private bool _isAttack3Ready;
    private bool _isThrusting;
    private bool _isBossShieldActive = true;

    [SerializeField]
    private int _shieldPower = 100;
    [SerializeField]
    private int _health = 100;
    [SerializeField]
    private GameObject _bossShield;

    private Player _player;
    private UIManager _uIManager;
    private AudioSource _audioSource;

    [SerializeField]
    private GameObject _explosionPrefab;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("The Player is NULL");
        }

        _uIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uIManager == null)
        {
            Debug.LogError("The UI Manager is NULL");
        }

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("The Audio Source is NULL");
        }
    }

    void Update()
    {
        if (this.gameObject != null)
        {
            _uIManager.BossHealthBarOn();
        }

        if (_action != BossAction.Start)
        {
            BeginMissileFire();
        }

        if (_action == BossAction.Start)
        {
            if (transform.position != _bossPos[0])
            {
                transform.position = Vector3.MoveTowards(transform.position, _bossPos[0],
                    _speed * Time.deltaTime);
            }

            else
            {
                _action = BossAction.Wait;
            }
        }

        Movement();
        Attacks();
    }

    void Movement()
    {
        if (_action == BossAction.Wait)
        {
            _posChosen = true;
            StartCoroutine(BossStop());

            if (_posChosen == true)
            {
                _randomIndex = Random.Range(0, 5);
            }

        }

        if (_action == BossAction.Move)
        {
            transform.position = Vector3.MoveTowards(transform.position, _bossPos[_randomIndex], _speed * Time.deltaTime);

            if (transform.position == _bossPos[_randomIndex])
            {
                _endPoint = transform.position.x;
                _action = BossAction.Wait;
            }
        }
    }

    void Attacks()
    {
        if (_action == BossAction.Attack1)
        {
            if (_isBossLaserActive == false)
            {
                transform.position = Vector3.MoveTowards(transform.position, _bossPos[5], _speed * Time.deltaTime);
            }

            if (transform.position.x > -6.5f && transform.position.y == 3.95f)
            {
                _isBossLaserActive = true;

            }

            if (_isBossLaserActive == true)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(-6.7f, 3.95f, 0),
                _speed * Time.deltaTime);

                _bossLaser.SetActive(true);

                if (transform.position.x == -6.7f && transform.position.y == 3.95f)
                {
                    _isBossLaserActive = false;
                    _bossLaser.SetActive(false);
                    _action = BossAction.Move;
                }
            }
        }

        if (_action == BossAction.Attack2)
        {
            if (_isBossLaserActive == false)
            {
                transform.position = Vector3.MoveTowards(transform.position, _bossPos[6], _speed * Time.deltaTime);
            }

            if (transform.position.x < 6.5f && transform.position.y == 3.95f)
            {
                _isBossLaserActive = true;

            }

            if (_isBossLaserActive == true)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(6.7f, 3.95f, 0),
                _speed * Time.deltaTime);

                _bossLaser.SetActive(true);

                if (transform.position.x == 6.7f && transform.position.y == 3.95f)
                {
                    _isBossLaserActive = false;
                    _bossLaser.SetActive(false);
                    _action = BossAction.Move;
                }
            }
        }

        if (_action == BossAction.Attack3)
        {
            if (transform.position.y < 6f)
            {
                transform.position = Vector3.MoveTowards(transform.position, _bossPos[7], (_speed / 2) * Time.deltaTime);
            }

            if (transform.position.y == 6f)
            {
                transform.position = Vector3.MoveTowards(transform.position,
                    new Vector3(_player.transform.position.x, 6f, 0), _speed * Time.deltaTime);

                StartCoroutine(PrepareAttack3());
            }

            if (_isAttack3Ready == true)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x,
                    10f, 0), (_speed / 2) * Time.deltaTime);

                if (transform.position.y == 10f)
                {
                    _isThrusting = true;
                }

                if (_isThrusting == true)
                {
                    transform.Translate(Vector3.down * (_speed * 4f) * Time.deltaTime);
                }

                if (transform.position.y <= -12f)
                {
                    _isAttack3Ready = false;
                    _isThrusting = false;
                    _action = BossAction.Move;
                }
            }
        }
    }

    private IEnumerator BossStop()
    {
        yield return new WaitForSeconds(3.0f);
        _posChosen = false;

        if (_endPoint < 0)
        {
            _action = BossAction.Attack1;
        }

        else if (_endPoint > 0)
        {
            _action = BossAction.Attack2;
        }

        else if (_endPoint == 0)
        {
            _action = BossAction.Attack3;
        }

    }

    private IEnumerator PrepareAttack3()
    {
        yield return new WaitForSeconds(5.0f);
        _isAttack3Ready = true;
    }

    void BeginMissileFire()
    {
        if (_isBossLaserActive != true || _isAttack3Ready != true)
        {
            if (Time.time > _canFire)
            {
                _fireRate = 3f;
                _canFire = Time.time + _fireRate;
                GameObject bossMissiles = Instantiate(_bossMissilePrefab, transform.position, Quaternion.identity);
                EliteMissile[] missiles = bossMissiles.GetComponentsInChildren<EliteMissile>();

                for (int i = 0; i < missiles.Length; i++)
                {
                    missiles[i].AssignBossMissile();
                }
            }
        }
    }

    public void Damage()
    {
        if (_isBossShieldActive == true)
        {
            _shieldPower -= 4;
            _uIManager.BossShield(_shieldPower);

            if (_shieldPower == 0)
            {
                _bossShield.SetActive(false);
                _isBossShieldActive = false;
            }
        }

        else
        {
            _health -= 4;
            _uIManager.BossHealth(_health); 
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _player.Damage();
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            Damage();

            if (_health == 0)
            {
                Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
                _audioSource.Play();
                Destroy(this.gameObject);
                _uIManager.BossHealthBarOff();
            }
        }

    }
}
