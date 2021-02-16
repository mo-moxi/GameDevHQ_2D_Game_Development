using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private float _mineYRange = 3f;         // enmey minefiled Y limit
    [SerializeField]
    private float _spawnDelay = 8f;
    [SerializeField]
    private int _xRange = 8;                // X spawn range
    public int _enemyWave = 0;              // enemy wave counter
    private int _enemiesSpawned = 0;        // enemy per wave spawn counter
    private int _lowLaserCount;             // low laser warning level
    private int _lowLifeCount = 3;          // low life counter level
    private int _enemy;                     // enemy type : 0 Y-Wing, 1 X-Wing
    [SerializeField]
    private int _enemyToSpawnCount = 5;     // enemy per wave counter
    private bool _stopSpawning = false;
    public bool _mothership = false;        // mothership true / false
    [SerializeField]
    private GameObject[] _enemyPrefabs;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] _powerupPrefabs;

    public void StartSpawning()
    {   
        StartCoroutine(SpawnEnemyRoutine());        // spawn enemy routine
        StartCoroutine(SpawnPowerupRoutine());      // spawn powerup routine
        StartCoroutine(SpawnMinefieldRoutine());    // spawn enemy minefield routine
    }

    IEnumerator SpawnEnemyRoutine()     // cycle / spawn 2 ships in 3 waves
    { while(_stopSpawning == false) 
        {
            if (_enemiesSpawned >= _enemyToSpawnCount)// Decrease spawn delay every 5 enemies spawned -- test
            {
                _enemiesSpawned = 0;    // clear counter
                _enemyWave +=1;         // increment wave count
                _spawnDelay -= 0.5f;    // decrease spawn delay
            }
            if(_enemyWave > 5)          // enemy wave counter – when it reaches 7 it's BOSS time!
            {
                _enemyWave = 3;         // spawn enemy 1
                _spawnDelay +=3;        // add 3 seconds to spawn delay
                Mothership();           // spawn mothership on level 7
            }
            if (_enemyWave < 3)         // choose enemy type by wave level
            {
                _enemy = 0;             // set enemy to Y-Wing
            }
            else
            {
                _enemy = 1 ;            // set enemy to X-Wing
            }
            Vector3 posToSpawn = new Vector3(Random.Range(-_xRange, _xRange), _xRange, 0f);
            GameObject newEnemy = Instantiate(_enemyPrefabs[_enemy], posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            _enemiesSpawned +=1;
            yield return new WaitForSeconds(_spawnDelay);
        }
    }
    IEnumerator SpawnPowerupRoutine()
    {   
        while(_stopSpawning == false)
        { 
            Vector3 posToSpawn = new Vector3(Random.Range(-_xRange, _xRange), _xRange, 0f);
            int randomPowerup = Random.Range(1, _powerupPrefabs.Length);
        if (_lowLaserCount < 5) // spawn laser refill on lower laser count
        {
            randomPowerup = 4;
        }
        if (_lowLifeCount < 2 && Random.value >= 0.7f) // spawn life power up at 1 and chance > 0.75
        {
            randomPowerup = 0;
        }
        Instantiate(_powerupPrefabs[randomPowerup], posToSpawn, Quaternion.identity);
        yield return new WaitForSeconds(Random.Range(3, _spawnDelay));
        }  
    }
    IEnumerator SpawnMinefieldRoutine()
    {   
        while(_stopSpawning == false && _mothership == true) // spawn enemy minefield when wave count > 3 
        {  
            float[] mineXPos = {-3f, 3f};
            for (int i=0; i < mineXPos.Length; i++)
            { 
                Vector3 posToSpawn = new Vector3(mineXPos[i], _mineYRange, 0f);    
                Instantiate(_enemyPrefabs[3], posToSpawn, Quaternion.identity);
            }
            yield return new WaitForSeconds(Random.Range(16, 32));
        }  
    }
    public void OnPlayerDeath()                 // stop spawning on player death
    {
        _stopSpawning = true;
    }
    public void LowLaserCount(int laserCount)   // laser level update
    {
        _lowLaserCount = laserCount;
    }
    public void LowLifeCount(int lives)         // lives level update
    {
        _lowLifeCount = lives;
    }
    private void Mothership()                   // spawn mothership
    { 
        if (_mothership == true) return;        // return if mothership exists
        
        Vector2 mothershipSpawnPosition = new Vector2(0f, 10f);
        Instantiate(_enemyPrefabs[2], mothershipSpawnPosition, Quaternion.identity);
        _mothership = true;
    }
}
