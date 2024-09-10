using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class kitchengun : MonoBehaviour
{

    public GameObject player;
    public Transform shootPoint;
    public float damage;
    public float attackSpeed;
    public float angle;
    public GameObject bulletPrefab;
    public GameObject target;
    public Rigidbody2D rb;
    //Collider2D[] objectsinrange;
    public float maxDistance;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        
        StartCoroutine(Shoot());
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(player.transform.position, Vector3.forward, -30*Time.deltaTime);
        
    }

    void FixedUpdate()
    {
        if(target != null)
        {
            Vector2 direction = target.transform.position - transform.position;
            angle = Mathf.Atan2(direction.y, direction.x)*Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            if(angle > 90 || angle < -90)
            {
                GetComponent<SpriteRenderer>().flipY = true;
            }
            else if(angle <=90 || angle >= -90)
            {
                GetComponent<SpriteRenderer>().flipY = false;
            }
        }
        else
        {
            transform.rotation = Quaternion.identity;
            GetComponent<SpriteRenderer>().flipY = false;
        }
        
        
        Collider2D[] objectsinrange = Physics2D.OverlapCircleAll(transform.position, 1000);
        
        if(objectsinrange.Length == 0)
        {
            //target = null;
        }
        else
        {
            
            if(target != null)
            {
                maxDistance = Vector3.Distance(this.transform.position, target.gameObject.transform.position);
            }
            else
            {
                maxDistance = 99999;
            }
        
        GameObject g = target;
        foreach (Collider2D c in objectsinrange)
        {
            EnemyMovement e = c.gameObject.GetComponent<EnemyMovement>();
            Enemybullet bu = c.gameObject.GetComponent<Enemybullet>();
            Destructible d = c.gameObject.GetComponent<Destructible>();
            if(e != null)
            {
                
                if(Vector3.Distance(this.transform.position, c.gameObject.transform.position)<maxDistance)
                {
                    maxDistance = Vector3.Distance(this.transform.position, c.gameObject.transform.position);
                    g = c.gameObject;
                    
                }
            }
        }
        target = g;
        }
        
    }

    public void getshoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        bullet.GetComponent<bullet>().damage = damage;
        bullet.GetComponent<bullet>().StartCoroutine(bullet.GetComponent<bullet>().bulletDestroy(1));
                
        GetComponent<Animator>().Play("kitgunshoot");
    }

    public IEnumerator Shoot()
    {
        while(true)
        {
            yield return new WaitForSeconds(attackSpeed);
            if(target != null)
            {
                getshoot();
            }
        }
    }
}
