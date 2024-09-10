using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Spiketrap : MonoBehaviour
{
    public float damage = 10;
    public float radius;
    public String source;
    
    public Transform pointa;
    public Transform pointb;

    public bool elec;
    public int power;
    public Destructible generator;
    public float pulse;

    // Start is called before the first frame update
    void Start()
    {
        if(elec)
        {
            StartCoroutine(timer());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(elec)
        {
            if(GameObject.Find("Director").GetComponent<Director>().waves == -1)
            {
                power = 0;
            }
            else
            {
                power = generator.power;
            }
            
            GetComponent<Animator>().SetInteger("Power", power);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        gameObject.GetComponent<Animator>().SetTrigger("Trap");
    }

    public void Trapreset()
    {
        gameObject.GetComponent<Animator>().ResetTrigger("Trap");
    }

    public void Activate()
    {
        Collider2D[] objectsinrange = Physics2D.OverlapAreaAll(pointa.position, pointb.position, 100000001, -100, 100);
        foreach(Collider2D c in objectsinrange)
        {
            
                EnemyMovement e = c.GetComponent<EnemyMovement>();
                if(e != null)
                {
                    
                    e.TakeDamage(damage*DataManager.Instance.floor, "Trap");
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

    IEnumerator timer()
    {
        while(true)
        {
            yield return new WaitForSeconds(pulse);
            if(power == 1)
            {
                Activate();
            }
        }
    }
}
