using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class upanddown : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(UpAndDown());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator UpAndDown()
    {
        
        
                transform.position = new Vector3(transform.position.x, transform.position.y+0.11f); 
                yield return new WaitForSeconds(0.8f);
            
        
                transform.position = new Vector3(transform.position.x, transform.position.y-0.1f); 
                yield return new WaitForSeconds(0.8f);
                
            
            StartCoroutine(UpAndDown());
    }
}
