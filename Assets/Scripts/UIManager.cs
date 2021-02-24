using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    [SerializeField]
    private int _radarScale = 5;
    [SerializeField]
    private bool _isGameOver;
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private int _score;
    [SerializeField]
    private Text _gameOver;
    [SerializeField]
    private Image _thrusterSprite;
    [SerializeField]
    private Image _tripleShot;
    [SerializeField]
    private Image _sceneFadeImage;
    [SerializeField]
    private Image _playerRadarSprite;
    [SerializeField]
    private Image _laserBurstSprite;
    [SerializeField]
    private Sprite[] _lasersSprite;
    [SerializeField]
    private Image _lasersImage;
    [SerializeField]
    private Sprite[] _livesSprite;
    [SerializeField]
    private Image _shieldSprite;
    [SerializeField]
    private Image _missileSprite;
    [SerializeField]
    private Image _livesImage;
    private AudioSource _audio;
    private GameManager _gameManager;
    [SerializeField]
    private Canvas _radarCanvas;
    [SerializeField]
    private GameObject[] _radarDot;

    private int _dotColour;

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameOver.gameObject.SetActive(false);
        _audio = GetComponent<AudioSource>();
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        StartCoroutine(FadeImage(true));
        if(_gameManager == null)
            Debug.LogError("GameManager is null");
        _sceneFadeImage.gameObject.SetActive(true); //enable image fade effect 
    }
    public void RotateRadarImageSpeed(float radarScanSpeed)
    {
        //_radarScanSpeed = _scanSpeed;
        StartCoroutine("RotateRadarImage", radarScanSpeed);
    }
    public void RadarObjectPosition(float objectPositionX, float objectPositionY, string objectTag)
    {   
        // Draw radar dot to match object type
        if (objectTag =="Enemy" || objectTag == "Negative_Powerup" )
        {
            _dotColour = 1;
        }
        else if (objectTag =="Powerup")
        {
            _dotColour = 0;
        }
        else
        {
            return;
        }
        Instantiate(_radarDot[_dotColour], _radarCanvas.transform.position + new Vector3(objectPositionX*_radarScale,objectPositionY*_radarScale,0), Quaternion.identity, _radarCanvas.transform);
    }
    public void UpdateScore(int playerScore)
    {   _score += playerScore;
        _scoreText.text = "Score: " + _score.ToString();
    }
    public void LaserCount(int laserCount)
    {
        laserCount = Mathf.Clamp(laserCount, 0, 15);
        if (laserCount <= 3)
        {
            _audio.Play();
            StartCoroutine("LaserImageFlicker", laserCount);
        }
        else
        {
           _audio.Stop();
           StopCoroutine("LaserImageFlicker");
           _lasersImage.gameObject.SetActive(true);
        }
        _lasersImage.sprite = _lasersSprite[laserCount];
    }
    public void UpdateLives(int lives)
    {
        _livesImage.sprite = _livesSprite[lives];
        if(lives < 1)
        {
            GameOver();
            return;
        }
    }
    public void ThrusterImage(bool thruster)
    {
        _thrusterSprite.gameObject.SetActive(thruster);
    }
    public void TripleShot(bool tshot)
    {
        _tripleShot.gameObject.SetActive(tshot);
    }
    public void ShieldLevel(int shieldLevel)
    {
        switch (shieldLevel)
        {
        case 0:
            _shieldSprite.gameObject.SetActive(false);
            break;
        case 3:
            _shieldSprite.gameObject.SetActive(true);
            break;  
        }
    }
    public void LaserBurstImage(int laserBurstCount)
    {
        if (laserBurstCount > 0)
        {
            _laserBurstSprite.gameObject.SetActive(true);
        }
        else
        {
            _laserBurstSprite.gameObject.SetActive(false);    
        }
    }
    public void MissileSprite(int missileCount)
    {
        if (missileCount > 0)
        {
            _missileSprite.gameObject.SetActive(true);
        }
        else
        {
            _missileSprite.gameObject.SetActive(false);    
        }
    }
    void GameOver()
    {   
        _isGameOver = true;
    //    _gameManager.GameOver();
        _gameOver.gameObject.SetActive(true);
        _audio.Stop();
        StartCoroutine(FadeImage(false));
        StartCoroutine(GameOverFlicker());
    }
    public void PlayerWins()
    {   
        _isGameOver = true;
        _audio.Stop();
        StartCoroutine(LoadEndScene());
    }
    IEnumerator LoadEndScene()
    {
        StartCoroutine(FadeImage(false));
        yield return new WaitForSeconds(3);
        _gameManager.PlayerWins();
    }
    IEnumerator GameOverFlicker()
    {
        while(_isGameOver == true)
        {
            _gameOver.text = "GAME OVER!";
            yield return new WaitForSeconds(Random.Range(0.1f, 0.9f));
            _gameOver.text = "";
            yield return new WaitForSeconds(Random.Range(0.2f, 0.5f));
        }
    }
    IEnumerator LaserImageFlicker(int laserCount)
    {
        while(laserCount <= 3)
        {
            _lasersImage.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            _lasersImage.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }
    }
    IEnumerator RotateRadarImage(int radarScanSpeed)
    {
        while(true)
        {
         //Rotate radar image
        _playerRadarSprite.transform.Rotate(Vector3.back * Time.deltaTime * radarScanSpeed, Space.Self);
        yield return null;
        }
    }
    private void FadeInFadeout(bool fade)
    {
        StartCoroutine(FadeImage(fade));
    }
    IEnumerator FadeImage(bool fade)
    {
        // fade from opaque to transparent
        if (fade)
        {
            // fade in over 2 second
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                // set color with i as alpha
                _sceneFadeImage.color = new Color(0, 0, 0, i);
                yield return null;
            }
        }
        // fade from transparent to opaque
        else
        {
            // fade in over 2 second
            for (float i = 0; i <= 1; i += Time.deltaTime)
            {
                // set color with i as alpha
                _sceneFadeImage.color = new Color(0, 0, 0, i);
                yield return null;
            }
        }
    }
}
