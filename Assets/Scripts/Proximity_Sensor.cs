using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proximity_Sensor : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyShip;

    private void OnTriggerEnter2D(Collider2D other)
    {   
        if (other.tag == "Laser")
        {
            var enemyScript = _enemyShip.GetComponent<Enemy>();
            enemyScript.ProximityPosition();
        }
    }
}