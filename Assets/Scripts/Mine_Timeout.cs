using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine_Timeout : MonoBehaviour
{
    [SerializeField]
    private float _destroyDelay = 1f;
    void Start()
    {
        Destroy(gameObject, _destroyDelay);
    }
}
