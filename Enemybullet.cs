using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Pathfinding;

public class Enemybullet : MonoBehaviour
{
    public float bulletSpeed;
    Rigidbody2D rb;
    public float damage;
    public GameObject trail;
    public GameObject explosionprefab;
    public float pulse;
    public bool nofirsttrail;
    public float bounce;
    public bool destructible = false;
    public bool explodeondestroy;
    public bool explodeblockable;
    public float health;
    public TextMeshProUGUI damagenum;
    public float destroy;

    public int wavy = 0;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        if(trail != null)
        {
            StartCoroutine(makeTrail());
        }
        if(wavy != 0)
        {
            StartCoroutine(wave());
        }
        if(GetComponent<AIDestinationSetter>() != null)
        {
            GetComponent<AIDestinationSetter>().target = GameObject.Find("Player").GetComponent<Transform>();
            GetComponent<AIPath>().maxSpeed = bulletSpeed;
            
            
        }
        if(destroy != 0){Destroy(gameObject, destroy);}
    }
    void Update()
    {
        if(GetComponent<AIDestinationSetter>() == null)
        {
            rb.velocity = transform.up * bulletSpeed;
        }
        
        if(GameObject.Find("Director").GetComponent<Director>().waves == 0)
        {
            Destroy(gameObject);
        }
        if(destructible && health < 1)
        {
            if(explodeondestroy)
            {
                if(explosionprefab != null)
            {   
                GameObject e = Instantiate(explosionprefab, this.transform.position, Quaternion.identity);
                if(e.GetComponent<AOEdamage>() != null)
                {
                    e.GetComponent<AOEdamage>().setvars(damage, 0.5f, 25);
                    e.GetComponent<AOEdamage>().source = "Explosion";
                }
                else if(e.GetComponent<MultiBullet>() != null)
                {
                    e.GetComponent<MultiBullet>().setvars(damage, bulletSpeed+10);
                
                }
            
            }
            }
            Destroy(gameObject);
        }
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(destructible && collision.gameObject.tag == "Bullet")
        {
        
        return;
        }
        if(!explodeblockable || collision.gameObject.tag != "Melee")
        {
            if(explosionprefab != null && collision.gameObject.tag != "Shield")
            {
                GameObject e = Instantiate(explosionprefab, this.transform.position, Quaternion.identity);
                if(e.GetComponent<AOEdamage>() != null)
                {
                    e.GetComponent<AOEdamage>().setvars(damage, 0.5f, 25);
                    e.GetComponent<AOEdamage>().source = "Explosion";
                }
                else if(e.GetComponent<MultiBullet>() != null)
                {
                    e.GetComponent<MultiBullet>().setvars(damage, bulletSpeed+10);
                
                }
            
            }
        
        
        else if(collision.gameObject.tag == "Shield")
        {
            if(collision.gameObject.GetComponent<spoon>() != null)
            {
                collision.gameObject.GetComponent<spoon>().TakeDamage(damage);
            }
            if(collision.gameObject.GetComponent<Destructible>() != null)
            {
                collision.gameObject.GetComponent<Destructible>().TakeDamage(damage);
            }
            
        }
        else if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(damage);
        }
        else if(collision.gameObject.tag == "Destructible")
        {
            collision.gameObject.GetComponent<Destructible>().TakeDamage(damage);
        }
        }
        if(bounce > 0 && collision.gameObject.tag != "Player")
                {
                    Vector3 v = Vector3.Reflect(transform.up, collision.contacts[0].normal);
                    float rot = Mathf.Atan2(-v.x, v.y) * Mathf.Rad2Deg;
                    transform.eulerAngles = new Vector3(0, 0, rot);
                    
                    bounce --;
                }
                else
                {
                    Destroy(this.gameObject);
                }
        
        
    }

    IEnumerator makeTrail()
    {
        while(true)
        {
        if(nofirsttrail)
        {
            nofirsttrail = false;
        }
        else
        {
            GameObject e = Instantiate(trail, this.transform.position, Quaternion.identity);
            
            e.GetComponent<AOEdamage>().source = "Explosion";
            e.GetComponent<AOEdamage>().damage *= DataManager.Instance.floor;
        }
        
        yield return new WaitForSeconds(pulse);    
        }
        
    }

    IEnumerator wave()
    {
        
        while(true)
        {
            
        for(float i = 0 ; i < 90; i+=5)
        {
            if(wavy == 1)
            {
                transform.rotation = Quaternion.Euler(0, 0, 180+i);
            }
            if(wavy == 2)
            {
                transform.rotation = Quaternion.Euler(0, 0, 180-i);
            }
            
            yield return new WaitForSeconds(Time.deltaTime);
        }
        if(wavy == 1)
            {
                wavy = 2;
            }
            else if(wavy == 2)
            {
                wavy = 1;
            }
        transform.rotation = Quaternion.Euler(0, 0, 180);
            
        }
    }

    public void TakeDamage(float damage, String source)
    {
        if(damagenum == null){return;}
        TextMeshProUGUI x = Instantiate(damagenum, GameObject.Find("Canvas").transform, false);
        x.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        x.gameObject.GetComponent<damagenum>().dnum = damage;
        health -= damage;
    }
}
