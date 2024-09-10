using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Elecwall : MonoBehaviour
{
    public int power;
    public Collider2D physwall;
    
    public float damage;
    public float pulse;
    public String source;
    public Transform pointa;
    public Transform pointb;
    public Destructible generator;
    public GameObject parent;
    // Start is called before the first frame update
    void Start()
    {
        if(generator != null){physwall.enabled = false;}
        
        
        StartCoroutine(wait());
    }

    // Update is called once per frame
    void Update()
    {
        if(generator != null)
        {
        if(GameObject.Find("Director").GetComponent<Director>().waves > 0)
        {
            power = generator.power;
        }
        else
        {
            power = 0;
        }
        GetComponent<Animator>().SetInteger("Power", power);
        
        if(power == 1)
        {
            physwall.enabled = true;
            
        }
        else
        {
            physwall.enabled = false;
            
        }
        }
        if(parent != null)
        {
            transform.position = parent.transform.position;
            transform.rotation = parent.transform.rotation;
        }
    }

    public void hit()
    {
        
        if(power == 0) {return;}
        
        Collider2D[] objectsinrange = Physics2D.OverlapAreaAll(pointa.position, pointb.position, 100000001, -100, 100);
        if(generator == null)
        {
            physwall.OverlapCollider(new ContactFilter2D(), objectsinrange);
        }
        foreach(Collider2D c in objectsinrange)
        {
            
                EnemyMovement e = c.GetComponent<EnemyMovement>();
                Enemybullet bu = c.GetComponent<Enemybullet>();
                if(e != null)
                {
                    e.TakeDamage(damage*DataManager.Instance.floor, "Trap");
                }
                else if(bu != null)
                {
                    bu.TakeDamage(damage*DataManager.Instance.floor, "Trap");
                }
            
                Player p = c.GetComponent<Player>();
                if(p != null)
                {
                    p.TakeDamage(damage*DataManager.Instance.floor, "Trap");
                }
            
            Destructible d = c.GetComponent<Destructible>();
                if(d != null)
                {
                    d.TakeDamage(damage*DataManager.Instance.floor);
                }
        }
        
    }

    public IEnumerator wait()
    {
        while(true)
        {
        yield return new WaitForSeconds(pulse);
        hit();
        }
    }
}
