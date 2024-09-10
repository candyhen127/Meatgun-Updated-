using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Efield : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(destroy());
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody2D>().velocity = transform.up * 100;
    }

    IEnumerator destroy()
    {
        yield return new WaitForSeconds(0.2f);
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(8f);
        Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<bullet>()!=null)
        {
            other.gameObject.GetComponent<bullet>().damage *= 1.5f;
            other.gameObject.GetComponent<bullet>().transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);
        }
    }
}
