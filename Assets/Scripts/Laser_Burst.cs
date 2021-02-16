using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser_Burst : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8f;    
    private float _vectorSpeed;
    [SerializeField]
    private int _laserID;
    [SerializeField]
    private int _laserRange = 8;

    private void Update()
    {   _vectorSpeed = _speed * Time.deltaTime;
        switch (_laserID)
        {   
            case 0:
            transform.Translate(Vector3.left * _vectorSpeed);
            break;
            case 1:
            transform.Translate(-_vectorSpeed,_vectorSpeed,0);
            break;
            case 2:
            transform.Translate(Vector3.up * _vectorSpeed);
            break;
            case 3:
            transform.Translate(_vectorSpeed,_vectorSpeed,0);
            break;
            case 4:
            transform.Translate(Vector3.right * _vectorSpeed);
            break;
        }   
        if (transform.position.y < -_laserRange || transform.position.y > _laserRange ||
        transform.position.x < -_laserRange || transform.position.x > _laserRange)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }
}
