using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    public float damage;
    public GameObject player;
    Rigidbody2D rb;
    public bool givesAmmo;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player.GetComponent<Player>().gun.pangle > 90 || player.GetComponent<Player>().gun.pangle < -90)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if(player.GetComponent<Player>().gun.pangle <=90 || player.GetComponent<Player>().gun.pangle >= -90)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        if(givesAmmo)
        {
            transform.position = player.GetComponent<Player>().gun.shootPoint.transform.position;
        }
        else
        {
            transform.position = player.GetComponent<Player>().gun.transform.position;
        }
        transform.rotation = player.GetComponent<Player>().gun.shootPoint.transform.rotation;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(damage == 0){return;}
            if(collision.gameObject.tag == "Enemy")
            {
                EnemyMovement e = collision.gameObject.GetComponent<EnemyMovement>();
                Boss b = collision.gameObject.GetComponent<Boss>();
                Enemybullet bu = collision.gameObject.GetComponent<Enemybullet>();
                if(b != null)
                {
                    b.TakeDamage(damage, "Bullet");
                    if(givesAmmo)
                    {
                        if(b.health < 1)
                        {
                            player.GetComponent<Player>().gun.ammo = player.GetComponent<Player>().gun.maxAmmo;
                            
                        }
                        else if(player.GetComponent<Player>().gun.ammo < player.GetComponent<Player>().gun.maxAmmo)
                        {
                            player.GetComponent<Player>().gun.ammo ++;
                        }
                        player.GetComponent<Player>().gun.stopreload();
                    }
                    
                }
                else if(e != null)
                {
                    e.TakeDamage(damage, "Bullet");
                    if(givesAmmo)
                    {
                        if(e.health < 1)
                        {
                            player.GetComponent<Player>().gun.ammo = player.GetComponent<Player>().gun.maxAmmo;
                            
                        }
                        else if(player.GetComponent<Player>().gun.ammo < player.GetComponent<Player>().gun.maxAmmo)
                        {
                            player.GetComponent<Player>().gun.ammo ++;
                        }
                        player.GetComponent<Player>().gun.stopreload();
                    }
                }
                else if(bu != null)
                {
                    bu.TakeDamage(damage, "Bullet");
                    if(givesAmmo)
                    {
                        if(bu.health < 1)
                        {
                            player.GetComponent<Player>().gun.ammo = player.GetComponent<Player>().gun.maxAmmo;
                            
                        }
                        else if(player.GetComponent<Player>().gun.ammo < player.GetComponent<Player>().gun.maxAmmo)
                        {
                            player.GetComponent<Player>().gun.ammo ++;
                        }
                        player.GetComponent<Player>().gun.stopreload();
                    }
                }
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>());
            }
            else if(collision.gameObject.tag == "Destructible")
            {
                collision.gameObject.GetComponent<Destructible>().TakeDamage(damage);
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>());
            }
            
    }

    public IEnumerator bulletDestroy(float time)
    {
        yield return new WaitForSeconds(time);
        
        Destroy(this.gameObject);
    }
}
