using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class noodle : MonoBehaviour
{

    public float h;

    // Start is called before the first frame update
    void Start()
    {
        //Destroy(gameObject, 4f);
    }

    // Update is called once per frame
    void Update()
    {
        //StartCoroutine(UpAndDown());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        
        if(other.gameObject.tag == "Player" )
        {
            other.gameObject.GetComponent<Player>().PlayerHeal(h, true);
            Destroy(gameObject);
        }
    }

    IEnumerator UpAndDown()
    {
        var pos = transform.position;
        for(float i = 0f; i<=0.5f; i+=0.1f)
            {
                transform.position = new Vector3(pos.x, pos.y+0.1f); 
                yield return new WaitForSeconds(0.1f);
            }
        for(float i = 1f; i>=0; i-=0.1f)
            {
                transform.position = new Vector3(pos.x, pos.y-0.1f); 
                yield return new WaitForSeconds(0.1f);
            }
        for(float i = 0f; i<=0.5f; i+=0.1f)
            {
                transform.position = new Vector3(pos.x, pos.y+0.1f); 
                yield return new WaitForSeconds(0.1f);
            }
            StartCoroutine(UpAndDown());
    }
}
