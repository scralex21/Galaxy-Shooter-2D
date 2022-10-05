using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;

    [SerializeField]
    private Text _ammoText;
    private bool _ammoEmpty;

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
    private Image _LivesImg;
    [SerializeField]
    private Sprite[] _liveSprites;
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

 }
   
