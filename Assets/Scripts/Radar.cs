using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    [SerializeField]
    private float _radarScanSpeed = 300f;
    private UIManager _uiManager;
    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _uiManager.RotateRadarImageSpeed(_radarScanSpeed);
    }
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.back * Time.deltaTime * _radarScanSpeed, Space.Self);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        _uiManager.RadarObjectPosition(other.transform.position.x, other.transform.position.y, other.tag);
    }
}