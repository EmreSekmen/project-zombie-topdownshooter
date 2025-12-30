using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float duration = 0.2f;
    public float magnitude = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


 

    public IEnumerator Shake()
    {
        float elapsed = 0f;
        Vector3 cameraPos = transform.localPosition;

        while (elapsed < duration)
        {
            transform.localPosition =cameraPos + (Vector3)(Random.insideUnitCircle * magnitude);
            elapsed += Time.deltaTime;

           
            yield return null;

        }
        transform.localPosition = cameraPos;
    }
    
}
