using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _waveSubText;

    [SerializeField]
    private Text _ammoText;
    private bool _ammoEmpty;

    [SerializeField]
    private Slider _thrusterBar;
    [SerializeField]
    private Image _thrusterBarColor;

    [SerializeField]
    private Text _missileCountText;
    [SerializeField]
    private Text _pauseText;
    [SerializeField]
    private Image _pauseBox;

    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private Text _returnMainMenuText;
    [SerializeField]
    private Text _destroyAsteroidText;
    [SerializeField]
    private Text _enemiesInboundText;
    [SerializeField]
    private Text _waveText;
    [SerializeField]
    private Text _thrusterMalfunctionText;

    [SerializeField]
    private Image _LivesImg;
    [SerializeField]
    private Sprite[] _liveSprites;

    [SerializeField]
    private Image _homingMissileImage;
    [SerializeField]
    private Image _bombImage;
    [SerializeField]
    private Image _superLaserImage;

    [SerializeField]
    private Text _motherShipText;
    [SerializeField]
    private Slider _bossHealth;
    [SerializeField]
    private Slider _bossShield;

    private GameManager _gameManager;
    private bool _isAsteroidDestroyed;

    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _ammoText.text = "Ammo: 30/30";

        _destroyAsteroidText.gameObject.SetActive(true);
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore;
    }

    public void UpdateWave(int currentWave)
    {
        _waveSubText.text = "Wave: " + currentWave; 
    }

    public void UpdateAmmo(int ammoCount)
    {
        if (ammoCount > 0)
        {
            _ammoEmpty = false;
            _ammoText.text = "Ammo: " + ammoCount + "/30";
            _ammoText.color = Color.white;
        }
        else if (ammoCount <= 0)
        {
            _ammoText.text = "Ammo: 0/30";
            _ammoText.color = Color.red;
            _ammoEmpty = true;
            StartCoroutine(NoAmmo());
        }
    }

    IEnumerator NoAmmo()
    {
        while (_ammoEmpty == true)
        {
            _ammoText.text = "Ammo: 0/30";
            yield return new WaitForSeconds(0.30f);
            _ammoText.text = "";
            yield return new WaitForSeconds(0.30f);
        }
    }

    public void WaveNumber(int currentWave)
    {
        _waveText.text = "Wave " + currentWave;
        _waveText.gameObject.SetActive(true);
        StartCoroutine(WaveNumberDisable());
    }

    IEnumerator WaveNumberDisable()
    {
        while (_waveText == true)
        {
            yield return new WaitForSeconds(3.0f);
            _waveText.gameObject.SetActive(false);
        }
    }

    public void ThrusterMalfunctionOn()
    {
        _thrusterMalfunctionText.gameObject.SetActive(true);
    }

    public void ThrusterMalfunctionOff()
    {
        _thrusterMalfunctionText.gameObject.SetActive(false);
    }

    public void ThrusterBar(float thrusterpower)
    {
        _thrusterBar.value = thrusterpower;
    }

    public void MissilePowerupOn()
    {
        _homingMissileImage.gameObject.SetActive(true);
        _missileCountText.gameObject.SetActive(true);
        _missileCountText.text = "x3";
    }
    
    public void MissilePowerupOff()
    {
        _homingMissileImage.gameObject.SetActive(false);
        _missileCountText.gameObject.SetActive(false);
    }

    public void MissileCount(int missileCount)
    {
        if (missileCount == 2)
        {
            _missileCountText.text = "x2";
        }
        else if (missileCount == 1)
        {
            _missileCountText.text = "x1";
        }
    }

    public void BombPowerupOn()
    {
        _bombImage.gameObject.SetActive(true);
    }

    public void BombPowerUpOff()
    {
        _bombImage.gameObject.SetActive(false);
    }

    public void SuperLaserPowerUpOn()
    {
        _superLaserImage.gameObject.SetActive(true);
    }

    public void SuperLaserPowerUpOff()
    {
        _superLaserImage.gameObject.SetActive(false);
    }

    public void UpdateLives(int currentlives)
    {
        _LivesImg.sprite = _liveSprites[currentlives];

        if (currentlives == 0)
        {
            GameOverSequence();
        }

        void GameOverSequence()
        {
            _gameManager.GameOver();
            _gameOverText.gameObject.SetActive(true);
            _restartText.gameObject.SetActive(true);
            _returnMainMenuText.gameObject.SetActive(true);
            StartCoroutine(GameOverFlicker());
        }

        IEnumerator GameOverFlicker()
        {
            while (true)
            {
                _gameOverText.text = "GAME OVER";
                yield return new WaitForSeconds(0.30f);
                _gameOverText.text = "";
                yield return new WaitForSeconds(0.30f);
            }
        }
    }

    public void AsteroidDestroyedSequence()
    {
        _isAsteroidDestroyed = true;
        if (_isAsteroidDestroyed == true)
        {
            _destroyAsteroidText.gameObject.SetActive(false);
            _enemiesInboundText.gameObject.SetActive(true);
            StartCoroutine(EnemiesInboundFlicker());
        }

        IEnumerator EnemiesInboundFlicker()
        {
            while (true)
            {
                _enemiesInboundText.text = "Enemies Inbound!";
                yield return new WaitForSeconds(0.2f);
                _enemiesInboundText.text = "";
                yield return new WaitForSeconds(0.2f);
                StartCoroutine(EnemiesInboundDone());
            }
        }

        IEnumerator EnemiesInboundDone()
        {
            while (true)
            {
                yield return new WaitForSeconds(2.0f);
                _enemiesInboundText.gameObject.SetActive(false);
            }
        }
    }

    public void BossHealthBarOn()
    {
        _motherShipText.gameObject.SetActive(true);
        _bossHealth.gameObject.SetActive(true);
        _bossShield.gameObject.SetActive(true);
    }

    public void BossHealthBarOff()
    {
        _motherShipText.gameObject.SetActive(false);
        _bossHealth.gameObject.SetActive(false);
        _bossShield.gameObject.SetActive(false);
    }

    public void BossShield(int shieldStength)
    {
        _bossShield.value = shieldStength;
    }

    public void BossHealth(int health)
    {
        _bossHealth.value = health;
    }

    public void PauseMenuOn()
    {
        _pauseBox.gameObject.SetActive(true);
        _pauseText.gameObject.SetActive(true);
        _returnMainMenuText.gameObject.SetActive(true);
    }

    public void PauseMenuOff()
    {
        _pauseBox.gameObject.SetActive(false);
        _pauseText.gameObject.SetActive(false);
        _returnMainMenuText.gameObject.SetActive(false);
    }
}
   
