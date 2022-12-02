using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    private float _duration = 0.5f;
    private float _magnitude = 0.5f;

    public void CameraShake()
    {
        StartCoroutine(StartCameraShake());
    }
    public IEnumerator StartCameraShake()
    {
        Vector3 originalPosition = transform.position;
        float elapsed = 0f;

        while (elapsed < _duration)
        {
            float x = Random.Range(-1f, 1f) * _magnitude;
            float y = Random.Range(-1f, 1f) * _magnitude;

            transform.position = new Vector3(x, y, -10f);
            elapsed += Time.deltaTime;
            yield return 0;
        }

        transform.position = originalPosition; 
    }
}
