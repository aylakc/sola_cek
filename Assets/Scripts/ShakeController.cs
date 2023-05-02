using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeController : MonoBehaviour
{
    public float shakeDuration = 0.5f; // The duration of the camera shake
    public float shakeIntensity = 0.1f; // The intensity of the camera shake
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public IEnumerator ShakeGameObject()
    {
        var initialPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeIntensity;
            float y = Random.Range(-1f, 1f) * shakeIntensity;

            transform.position += new Vector3(x, y, 0f);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.position = initialPosition;
    }
}
