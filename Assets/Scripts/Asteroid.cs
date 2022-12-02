using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour

{
    [SerializeField]
    private float _rotateSpeed = 20f;
    [SerializeField]
    private GameObject _explosionPrefab;
    private SpawnManager _spawnManager;
    private UIManager _uiManager;

    private void Start()
    {
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }

    void Update()
    { 
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);  
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            int currentWave = 1;
            _uiManager.WaveNumber(currentWave);
            _spawnManager.StartSpawning(currentWave);
            Destroy(this.gameObject, 0.25f);

            _uiManager.AsteroidDestroyedSequence();
            
        }
    }
}
