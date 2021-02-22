using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    private float _speedMultiplier = 2f;
    [SerializeField]
    private float _fireRate = 0.2f;
    [SerializeField]
    private float _canFire;
    [SerializeField]
    private int _shieldLevel = 3;
    [SerializeField]
    private int _laserCount = 15;
    [SerializeField]
    private int _missileCount = 3;
    [SerializeField]
    private int _laserBurstCount;
    [SerializeField]
    private int _lives = 3;
    private int _shieldUpdate = -1;
    private bool _speedBoost;
    private bool _isTripleShotActive;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleLaserPrefab;
    [SerializeField]
    private GameObject _laserBurstPrefab;
    [SerializeField]
    private GameObject _missilePrefab;
    [SerializeField]
    private GameObject _shield;
    [SerializeField]
    private GameObject _thruster;
    [SerializeField]
    private GameObject _explosion;
    [SerializeField]
    private GameObject _leftEngineFire, _rightEngineFire;
    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    [SerializeField]
    private AudioClip _laser_shot;
    [SerializeField]
    private AudioClip _laserBurstShot;
    [SerializeField]
    private AudioClip _missile_shot;
    private AudioSource _audio;
    [SerializeField]
    private CameraShake _mainCameraShake;

    private void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audio = GetComponent<AudioSource>();
        _mainCameraShake = GameObject.Find("Main Camera").GetComponent<CameraShake>();
        _uiManager.MissileSprite(_missileCount);

        if (_spawnManager == null)
            Debug.Log("The Spawn Manager is NULL");
        if (_uiManager == null)
            Debug.Log("The UI Manager is NULL");
        if (_audio == null)
            Debug.LogError("Audio source is NULL");
        if (_mainCameraShake == null)
            Debug.LogError("Main Camera transform is NULL");
    }
    private void Update()
    {
        CalculateMovement();
        FireWeapons();
    }
    private void FireWeapons()
    {
        if (Time.time > _canFire)
        {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FireLaser();
            _canFire = Time.time + _fireRate;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            FireLaserBurst();
            _canFire = Time.time + _fireRate;
        } 
        if (Input.GetKeyDown(KeyCode.Z))
        {
            FireMissile();
            _canFire = Time.time + _fireRate;
        }
        }
    }
    void CalculateMovement()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");
        if (Input.GetKeyDown(KeyCode.LeftShift) && _speedBoost != true) // check for speed boost key press
            {
                SpeedBoostActive();
            } 
        var direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3, 3), 0);
        if (transform.position.x < -11)
            transform.position = new Vector3(11, transform.position.y, 0); // move position when out of bounds
        else if (transform.position.x > 11)
            transform.position = new Vector3(-11, transform.position.y, 0); // move position when out of bounds
    }
    private void FireLaser()                            // fire laser
    {
        if (_isTripleShotActive && _laserCount >= 3)    // test for 3-shot power up and laser count
        {
            Instantiate(_tripleLaserPrefab, transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
            _laserCount -= 3;
            LaserUpdate();
        }
        else if (_laserCount > 0)                       // test valid laser count
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
            _laserCount--;
            LaserUpdate();
        }
    }
    private void LaserUpdate()
        {
            _audio.PlayOneShot(_laser_shot);
            _uiManager.LaserCount(_laserCount);         // update laser level on UI
            _spawnManager.LowLaserCount(_laserCount);   // update spawn manger laser count
        }
    public void LaserRefill(int laserRefill)            // update laser level
    {
        _laserCount +=laserRefill;
        _laserCount = Mathf.Clamp(_laserCount, 0, 15);
        _uiManager.LaserCount(_laserCount);             // update UI laser count
    }
    public void LaserBurst()                            // update laser burst count
    {
        _laserBurstCount +=1;
        _uiManager.LaserBurstImage(_laserBurstCount);   // update UI laser burst count
    }
    void FireLaserBurst()                               // fire laser burst
    {   
        if(_laserBurstCount > 0)
        {
            Instantiate(_laserBurstPrefab, transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
            _laserBurstCount -= 1;
            _laserCount -= 1;
            _audio.PlayOneShot(_laserBurstShot, 0.7f);
            _uiManager.LaserBurstImage(_laserBurstCount); // update UI laser burst count
        }
    }
    public void MissileCount()                      // update missile count
    {
        _missileCount +=1;
        _uiManager.MissileSprite(_missileCount);    // update UI missile count
    }
    void FireMissile()  // fire missile
    {
        if(_missileCount > 0)
        {
            Instantiate(_missilePrefab, transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
            _audio.PlayOneShot(_missile_shot, 1.4f);
            _missileCount -=1;
            _uiManager.MissileSprite(_missileCount); // update UI missile count
        }
    }
    public void Damage()
    {   
        if (_shieldLevel > 0)                        // check if shield level
        {
            ShieldActive(_shieldUpdate);            // adjust shield level
            Explosion();
            return;
        }        
        Lives(-1);                                  // decrease life count
        Explosion();
        _mainCameraShake.shakecamera();             // shake camera when ship is hit
    }
    private void Explosion()
    {
        Instantiate(_explosion, this.transform.position, Quaternion.identity);
        AudioManager.Instance.PlayExplosion();
    }
    public void Lives(int livesUpdate)              // update live level
    {   
        _lives += livesUpdate;
        _lives = Mathf.Clamp(_lives, 0, 3);
        _uiManager.UpdateLives(_lives);     // update live count on UI
        switch (_lives)                     // set ship damage according to life level
        {
        case 3:
            _leftEngineFire.SetActive(false);
            _rightEngineFire.SetActive(false);
            break;
        case 2:
            _leftEngineFire.SetActive(true);
            _rightEngineFire.SetActive(false);
            break;
        case 1:
            _leftEngineFire.SetActive(true);
            _rightEngineFire.SetActive(true);
            _spawnManager.LowLifeCount(_lives); // send low life warning to spawn manager
            break;
        case 0:
            _spawnManager.OnPlayerDeath();      // notify spawn manger to stop spawning
            Destroy(this.gameObject);
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
        if (_speedBoost == false)                                           // check speed boost is inactive
        {
            _speed *= _speedMultiplier;
            _speedBoost = true;
            _thruster.transform.localScale = new Vector3(0.5f,1.1f,1f);    // increase thrust image size
            _uiManager.ThrusterImage(true);                                 // enable UI thruster icon
            AudioManager.Instance.PlayPowerUp();
            StartCoroutine(SpeedBoostPowerDown());
        }
    }
    IEnumerator SpeedBoostPowerDown()
    {
        yield return new WaitForSeconds(5.0f);
        _thruster.transform.localScale = new Vector3(0.3f,0.8f,1f);         // reduce thrust image size
        _speedBoost = false;
        _speed /= _speedMultiplier;
        AudioManager.Instance.PlayPowerDown();
        _uiManager.ThrusterImage(false);                                    // disable UI thruster icon
    }
    public void ShieldActive(int shieldUpdate)                              // update shield level
    {   
        _shieldLevel += shieldUpdate;
        _shieldLevel = Mathf.Clamp(_shieldLevel, 0, 3);
        _uiManager.ShieldLevel(_shieldLevel);                              // update UI shield count
        switch (_shieldLevel)                                               // modify shield size and colour according to level
        {
            case 0:
                _shield.SetActive(false);
                break;
            case 1:
                _shield.transform.localScale = new Vector3(1.35f,1.35f,1f);
                break;
            case 2:
                _shield.transform.localScale = new Vector3(1.65f,1.65f,1f);
                _shield.GetComponent<SpriteRenderer>().color = new Color(0.5f,0.5f,0.75f);
                break;
            case 3: //set full size
                _shield.SetActive(true);
                _shield.GetComponent<SpriteRenderer>().color = new Color(1,1,1);
                _shield.transform.localScale = new Vector3(2,2,2);
                break;
        }
    }
}
