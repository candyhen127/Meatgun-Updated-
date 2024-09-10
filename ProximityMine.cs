using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityMine : MonoBehaviour
{
    public float damage;
    public float destroy;
    public float radius;
    public GameObject explosionprefab;
    public GameObject extraprefab;
    public bool triggered;
    public bool proximity = true;

    public float timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameObject.Find("Director").GetComponent<Director>().waves == 0)
        {
            Destroy(gameObject);
        }
    }
    public void Prime()
    {
        GetComponent<Collider2D>().enabled = true;
        if(timer >0)
        {
            StartCoroutine(timedExplode());
        }
    }

    IEnumerator timedExplode()
    {
        yield return new WaitForSeconds(timer);
        triggered = true;
        GameObject e = Instantiate(explosionprefab, this.transform.position, Quaternion.identity);
        e.GetComponent<AOEdamage>().setvars(damage, destroy, radius);
        
        Destroy(this.gameObject);
    }

    public void setdamage(float d)
    {
        damage = d;
    }
    void OnTriggerEnter2D()
    {
        if(!proximity){return;}
        if(triggered){return;}
        triggered = true;
        explode();
        
        
    }

    public void explode()
    {
        GameObject e = Instantiate(explosionprefab, this.transform.position, Quaternion.identity);
        e.GetComponent<AOEdamage>().setvars(damage, destroy, radius);
        if(extraprefab != null)
        {
            GameObject x = Instantiate(extraprefab, this.transform.position, Quaternion.identity);
        }
        Destroy(this.gameObject);
    }
}
