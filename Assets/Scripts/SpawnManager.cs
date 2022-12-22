using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _enemyType;
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _spiderEnemyPrefab;

    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] _powerups;
    [SerializeField]
    private bool _stopSpawning = false;

    [SerializeField]
    private Vector3[] _enemySpawnLocation;

    [SerializeField]
    private GameObject _bossPrefab;

    private bool _powerupSpawn;

    [SerializeField]
    private int _wave;
    [SerializeField]
    private int _enemiesDead;
    [SerializeField]
    private int _maxEnemies;
    [SerializeField]
    private int _enemiesLeft;

    private UIManager _uiManager;

    private void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.Log("The UI Manager is NULL");
        }

        _enemySpawnLocation[0] = new Vector3(Random.Range(-9f, 9f), 7.5f, 0);
        _enemySpawnLocation[1] = new Vector3(11f, Random.Range(-1f, 5.60f), 0);
        _enemySpawnLocation[2] = new Vector3(-11.25f, Random.Range(-1f, 5.60f), 0);
        
    }

    public void StartSpawning(int currentWave)
    {
        if (currentWave <= 10)
        {
            _stopSpawning = false;
            _enemiesDead = 0;
            _wave = currentWave;
            _uiManager.WaveNumber(currentWave);

            switch (currentWave)
            {
                case 1:
                    _enemiesLeft = 5;
                    _maxEnemies = 5;
                    StartCoroutine(SpawnEnemyRoutineEasy());
                    break;
                case 2:
                    _enemiesLeft = 7;
                    _maxEnemies = 7;
                    StartCoroutine(SpawnEnemyRoutineEasy());
                    break;
                case 3:
                    _enemiesLeft = 9;
                    _maxEnemies = 9;
                    StartCoroutine(SpawnEnemyRoutineMedium());
                    break;
                case 4:
                    _enemiesLeft = 12;
                    _maxEnemies = 12;
                    StartCoroutine(SpawnEnemyRoutineMedium());
                    break;
                case 5:
                    _enemiesLeft = 15;
                    _maxEnemies = 15;
                    StartCoroutine(SpawnEnemyRoutineMedium());
                    break;
                case 6:
                    _enemiesLeft = 17;
                    _maxEnemies = 17;
                    StartCoroutine(SpawnEnemyRoutineMedium());
                    break;
                case 7:
                    _enemiesLeft = 18;
                    _maxEnemies = 18;
                    StartCoroutine(SpawnEnemyRoutineMedium());
                    break;
                case 8:
                    _enemiesLeft = 12;
                    _maxEnemies = 12;
                    StartCoroutine(SpawnEnemyRoutineHard());
                    break;
                case 9:
                    _enemiesLeft = 15;
                    _maxEnemies = 15;
                    StartCoroutine(SpawnEnemyRoutineHard());
                    break;
                case 10:
                    Instantiate(_bossPrefab, transform.position + new Vector3(0, 11f, 0), Quaternion.identity);
                    break;
                default:
                    break;
            }
        }

        StartCoroutine(SpawnPowerupRoutine());
        StartCoroutine(SpawnRarePowerupRoutine());
    }

    IEnumerator SpawnEnemyRoutineEasy()
    {
        yield return new WaitForSeconds(3.0f);

        while (_stopSpawning == false && _enemiesDead <= _maxEnemies)
        {
            for (var i = 0; i < _enemyType.Length; i++)
            {
                GameObject newEnemy = Instantiate(_enemyType[0], _enemySpawnLocation[(Random.Range(0,3))],
                  Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;

                _enemiesLeft--;
                if (_enemiesLeft <= 0)
                {
                    _enemiesLeft = 0;
                    _stopSpawning = true;
                    yield break;
                }
                yield return new WaitForSeconds(5.0f);
            }
        }
    }

    IEnumerator SpawnEnemyRoutineMedium()
    {
        yield return new WaitForSeconds(3.0f);

        while (_stopSpawning == false && _enemiesDead <= _maxEnemies)
        {
            for (var i = 0; i < _enemyType.Length; i++)
            {
                GameObject newEnemy = Instantiate(_enemyType[Random.Range(0, 3)], _enemySpawnLocation[(Random.Range(0, 3))],
                  Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;

                _enemiesLeft--;
                if (_enemiesLeft <= 0)
                {
                    _enemiesLeft = 0;
                    _stopSpawning = true;
                    yield break;
                }
                yield return new WaitForSeconds(3.0f);
            }
        }
    }

    IEnumerator SpawnEnemyRoutineHard()
    {
        yield return new WaitForSeconds(3.0f);

        while (_stopSpawning == false && _enemiesDead <= _maxEnemies)
        {
            for (var i = 0; i < _enemyType.Length; i++)
            {
                GameObject newEnemy = Instantiate(_enemyType[Random.Range(0, 5)], _enemySpawnLocation[(Random.Range(0, 3))],
                  Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;

                _enemiesLeft--;
                if (_enemiesLeft <= 0)
                {
                    _enemiesLeft = 0;
                    _stopSpawning = true;
                    yield break;
                }
                yield return new WaitForSeconds(3.0f);
            }
        }
    }

    public void UpdateEnemyCount()
    {
        _enemiesDead++;
        if (_enemiesDead == _maxEnemies)
        {
            new WaitForSeconds(3.0f);
            StartSpawning(_wave + 1);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9f, 9f), 7.5f, 0);
            int randomPowerUp = Random.Range(0, 5);
            Instantiate(_powerups[randomPowerUp], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 8));
        }
    }

    IEnumerator SpawnRarePowerupRoutine()
    {
        yield return new WaitForSeconds(30f);

        while (_stopSpawning == false)
        {
            Vector3 posTospawn = new Vector3(Random.Range(-9f, 9f), 7.5f, 0);
            int randomPowerUp = Random.Range(5, 9);
            Instantiate(_powerups[randomPowerUp], posTospawn, Quaternion.identity);
            yield return new WaitForSeconds(30f);
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
