using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float damage;
    public float bulletSpeed;
    Rigidbody2D rb;
    public int pierce;
    public int bounce;
    public GameObject explosionprefab;
    public float pulse;
    public List<GameObject> pierced;
    public bool dontDestroy;

    void Start()
    {
        foreach(Item i in DataManager.Instance.playeritems)
        {
            if(i.itemname == "Cookie Wand")
            {
                pierce += i.stacks;
            }
            if(i.itemname == "Stapler In Jello")
            {
                bounce += i.stacks;
            }
        }
        
        rb = gameObject.GetComponent<Rigidbody2D>();
        if(pulse != 0)
        {
            StartCoroutine(makeTrail());
        }
    }
    void Update()
    {
        rb.velocity = transform.up * bulletSpeed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>(), false);
        
            if(explosionprefab != null)
            {
                if(collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Destructible" || collision.gameObject.tag == "Bullet")
                {
                    if(pierce == 0)
                    {
                        GameObject e = Instantiate(explosionprefab, this.transform.position, Quaternion.identity);
                        e.GetComponent<AOEdamage>().setvars(damage, 0.5f, 25);
                        e.GetComponent<AOEdamage>().source = "Recoil";
                        e.transform.localScale = transform.localScale;
                        Destroy(this.gameObject);
                    }
                    else
                    {
                        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>());
                        pierced.Add(collision.gameObject);
                        pierce--;
                        EnemyMovement e = collision.gameObject.GetComponent<EnemyMovement>();
                        Boss b = collision.gameObject.GetComponent<Boss>();
                        Enemybullet bu = collision.gameObject.GetComponent<Enemybullet>();
                        if(b != null)
                        {
                            b.TakeDamage(damage/3, "Bullet");
                        }
                        else if(e != null)
                        {
                            e.TakeDamage(damage/3, "Bullet");
                        }
                        else if(bu != null)
                        {
                            bu.TakeDamage(damage/3, "Bullet");
                        }
                    }
                }
                else if(bounce > 0)
                {
                    Vector3 v = Vector3.Reflect(transform.up, collision.contacts[0].normal);
                    float rot = Mathf.Atan2(-v.x, v.y) * Mathf.Rad2Deg;
                    transform.eulerAngles = new Vector3(0, 0, rot);
                    foreach (GameObject g in pierced)
                    {
                        if(g != null)
                        {
                            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), g.GetComponent<Collider2D>(), false);
                        }
                    }
                    bounce --;
                }
                else
                {
                    GameObject e = Instantiate(explosionprefab, this.transform.position, Quaternion.identity);
                    e.GetComponent<AOEdamage>().setvars(damage, 0.5f, 25);
                    e.GetComponent<AOEdamage>().source = "Recoil";
                
                    Destroy(this.gameObject);
                }
                
            }
            else if(collision.gameObject.tag == "Enhancement Field")
            {
                if(OptionsManager.Instance.g.gunname == "FISH")
                {
                    Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>());
                    if(collision.gameObject.GetComponents<Collider2D>()[1] != null)
                    {
                        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.gameObject.GetComponents<Collider2D>()[1]);
                    }
                    pierced.Add(collision.gameObject);
                    pierce--;
                }
                else if(bounce > 0)
                {
                    Vector3 v = Vector3.Reflect(transform.up, collision.contacts[0].normal);
                    float rot = Mathf.Atan2(-v.x, v.y) * Mathf.Rad2Deg;
                    transform.eulerAngles = new Vector3(0, 0, rot);
                    foreach (GameObject g in pierced)
                    {
                        if(g != null)
                        {
                            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), g.GetComponent<Collider2D>(), false);
                        }
                        
                    }
                    bounce --;
                }
                else
                {
                    Destroy(this.gameObject);
                }
            }
            else if(collision.gameObject.tag == "Enemy")
            {
                EnemyMovement e = collision.gameObject.GetComponent<EnemyMovement>();
                Boss b = collision.gameObject.GetComponent<Boss>();
                Enemybullet bu = collision.gameObject.GetComponent<Enemybullet>();
                if(b != null)
                {
                    b.TakeDamage(damage, "Bullet");
                }
                else if(e != null)
                {
                    e.TakeDamage(damage, "Bullet");
                }
                else if(bu != null)
                {
                    bu.TakeDamage(damage, "Bullet");
                }
                
                if(pierce == 0)
                {
                    Destroy(this.gameObject);
                }
                else
                {
                    Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>());
                    
                    pierced.Add(collision.gameObject);
                    pierce--;
                    
                }
                
            }
            else if(collision.gameObject.tag == "Destructible")
            {
                collision.gameObject.GetComponent<Destructible>().TakeDamage(damage);
                if(pierce == 0)
                {
                    Destroy(this.gameObject);
                }
                else
                {
                    Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>());
                    pierced.Add(collision.gameObject);
                    pierce--;
                }
            }
            
            else if(bounce > 0)
            {
                Vector3 v = Vector3.Reflect(transform.up, collision.contacts[0].normal);
                    float rot = Mathf.Atan2(-v.x, v.y) * Mathf.Rad2Deg;
                    transform.eulerAngles = new Vector3(0, 0, rot);
                    foreach (GameObject g in pierced)
                    {
                        if(g != null)
                        {
                            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), g.GetComponent<Collider2D>(), false);
                        }
                        
                    }
                    bounce --;
            }
            else if(!dontDestroy)
            {
                Destroy(this.gameObject);
            }
    }

    public IEnumerator bulletDestroy(float time)
    {
        yield return new WaitForSeconds(time);
        if(explosionprefab != null)
            {
                GameObject e = Instantiate(explosionprefab, this.transform.position, Quaternion.identity);
                e.GetComponent<AOEdamage>().setvars(damage, 0.5f, 25);
                e.GetComponent<AOEdamage>().source = "Recoil";
                e.transform.localScale = transform.localScale;
                
            }
        Destroy(this.gameObject);
    }

    IEnumerator makeTrail()
    {
        while(true)
        {
            
        GameObject e = Instantiate(explosionprefab, this.transform.position, Quaternion.identity);
            
        e.GetComponent<AOEdamage>().source = "Explosion";
        e.GetComponent<AOEdamage>().damage = damage;
        yield return new WaitForSeconds(pulse);
        }
        
    }
}
