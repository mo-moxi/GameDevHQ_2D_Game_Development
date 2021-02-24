using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // Transform of the camera to shake. Grabs the gameObject's transform if null.
    private Transform camTransform;
	
    // How long the object should shake for.
    private float shakeDuration = 0.5f;
	
    // Amplitude of the shake. A larger value shakes the camera harder.
    private float shakeAmount = 0.7f;
    private float decreaseFactor = 1.0f;
    private Vector3 originalPos;
    private bool shakeCam;
    private void Awake()
    {
        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }
    private void OnEnable()
    {
        originalPos = camTransform.localPosition;
    }
    private void Update()
    {
        if(shakeCam == true)
        {
        if (shakeDuration > 0f )
        {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
			
            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shakeDuration = 0.5f;
            camTransform.localPosition = originalPos;
            shakeCam = false;
        }
        }
    }
    public void ShakeCamera()
    {
       shakeCam = true;
    }
}
