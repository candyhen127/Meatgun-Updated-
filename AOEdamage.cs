using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AOEdamage : MonoBehaviour
{
    public float damage;
    public float destroy;
    public float radius;
    public bool hitplayer = false;
    public bool hitenemy = true;
    public bool heal = false;
    public String source;
    public float pulse = 100;
    public AudioSource aud;

    public void setvars(float da, float de, float r)
    {
        damage = da;
        destroy = de;
        radius = r;
    }

    void Start()
    {
        if(aud != null)
        {
            aud.Play();
        }
        hit();
        if(destroy > 0)
        {
            Destroy(gameObject, destroy);
        }
        
    }

    void Update()
    {
        if(pulse < 100 && GameObject.Find("Director").GetComponent<Director>().waves == 0)
        {
            Destroy(gameObject);
        }
    }

    public void hit()
    {
        Collider2D[] objectsinrange = Physics2D.OverlapCircleAll(gameObject.transform.position, radius*transform.localScale.x);
        foreach(Collider2D c in objectsinrange)
        {
            if(!heal)
            {
                if(hitenemy)
                {
                    EnemyMovement e = c.GetComponent<EnemyMovement>();
                    Enemybullet bu = c.GetComponent<Enemybullet>();
                    if(e != null)
                    {
                        e.TakeDamage(damage, source);
                    }
                    else if(bu != null)
                    {
                        bu.TakeDamage(damage, "Bullet");
                    }
                }
                if(hitplayer)
                {
                    Player p = c.GetComponent<Player>();
                    if(p != null)
                    {
                        p.TakeDamage(damage, source);
                    }
                }
                if(!hitplayer && !hitenemy){return;}
                Destructible d = c.GetComponent<Destructible>();
                if(d != null)
                {
                    if(c.gameObject.tag != "Shield")
                    {
                        d.TakeDamage(damage);
                    }
                }
            }
            else
            {
                if(hitenemy)
                {
                    EnemyMovement e = c.GetComponent<EnemyMovement>();
                    if(e != null)
                    {
                        e.EnemyHeal(damage/100*e.maxHealth);
                    }
                }
                if(hitplayer)
                {
                    Player p = c.GetComponent<Player>();
                    if(p != null)
                    {
                        p.PlayerHeal(damage/100*p.maxHealth, false);
                    }
                }
            }
            
        }
        StartCoroutine(wait());
    }

    public IEnumerator wait()
    {
        yield return new WaitForSeconds(pulse);
        hit();
    }
}
