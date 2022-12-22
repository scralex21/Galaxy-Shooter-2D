using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3f;
    [SerializeField] 
    private int _powerupID;
    [SerializeField]
    private AudioClip _powerupAudio;

    [SerializeField]
    private GameObject _explosionPrefab;

    private Player _player;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    void Update()
    { 
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -5.75f)
        {
            Destroy(this.gameObject);
        }
    }

    public void MoveToPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, _player.transform.position,
            _speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            AudioSource.PlayClipAtPoint(_powerupAudio, transform.position);

            if (player != null)
            {
                switch(_powerupID)
                {
                    case 0:
                        player.TripleShotActive(); // Triple Shot
                        break;
                    case 1:
                        player.SpeedBoostActive(); // Speed Boost
                        break;
                    case 2:
                        player.ShieldActive(); // Shield
                        break;
                    case 3:
                        player.AmmoCollected(); // Ammo Pickup
                        break;
                    case 4:
                        player.SlowSpeedActive(); // Slow Powerup
                        break;
                    case 5:
                        player.HealthCollected(); // Health Pickup (Rare)
                        break;
                    case 6:
                        player.HomingMissileActive(); // Homing Missile (Rare)
                        break;
                    case 7:
                        player.BombActive(); // Bomb (Rare)
                        break;
                    case 8:
                        player.SuperLaserActive(); // Super Laser (Rare)
                        break;
                    default:
                        Debug.Log("Default Value");
                        break;
                }
            }
            Destroy(this.gameObject);
        }

        if (other.tag == "EnemyLaser")
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
        if (other.tag == "EnemyLaserPair")
        {
            Destroy(other.gameObject);
        }
    }
}
