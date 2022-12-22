using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public enum BossAction { Start, Wait, Move };

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

    private void Start()
    {
        _bossPos[0] = new Vector3(0, 3.5f, 0);
        _bossPos[1] = new Vector3(-6f, 3.5f, 0);
        _bossPos[2] = new Vector3(-2.5f, 3.5f, 0);
        _bossPos[3] = new Vector3(6f, 3.5f, 0);
        _bossPos[4] = new Vector3(2.5f, 3.5f, 0);
    }

    void Update()
    {
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

        if (_action == BossAction.Wait)
        {
            _posChosen = true;

            if (_posChosen == true)
            {
                _randomIndex = Random.Range(0, 5);
                StartCoroutine(BossStop());
            }

        }

        if (_action == BossAction.Move)
        {
            transform.position = Vector3.MoveTowards(transform.position, _bossPos[_randomIndex], _speed * Time.deltaTime);

            if (transform.position == _bossPos[_randomIndex])
            {
                //_posChosen = true;
                _action = BossAction.Wait;
            }
        }
    }

    private IEnumerator BossStop()
    {
        yield return new WaitForSeconds(3.0f);
        _posChosen = false;
        _action = BossAction.Move;

    }

    void BeginMissileFire()
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
