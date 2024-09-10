using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public Animator gunanimator;
    public Transform firePoint;
    public Transform firePoint2;
    public Transform shootPoint;
    public GameObject bulletPrefab;
    public Transform center;
    public Rigidbody2D rb;
    public LineRenderer line;
    public bool hasGun = true;

    public bool ray = false;
    public LayerMask ignoreme; 
    public float bulletSpeed = 2f;
    public float damage = 10;
    public float attackSpeed = 0.1f;
    public float defatkspd = 0.1f;
    public int projectiles = 1;
    public int burst = 1;
    public int burstcount;
    public float pangle; 
    public float angle;
    public Vector2 playerPos;
    public float shooting;
    public bool holdPosition;
    new public SpriteRenderer renderer;
    public AudioSource aud;
    
    void Start()
    {
        damage*=DataManager.Instance.floor;
        StartCoroutine(Shoot());
        burstcount = burst;
    }

    // Update is called once per frame
    void Update()
    {
        if(!holdPosition)
        {
        if(pangle > 90 || pangle < -90)
        {
            renderer.flipY = true;
        }
        else if(pangle <=90 || pangle >= -90)
        {
            renderer.flipY = false;
        }
        }
    }

    void FixedUpdate()
    {
        
        Vector2 direction = playerPos - rb.position;
        angle = Mathf.Atan2(direction.y, direction.x)*Mathf.Rad2Deg;
        //Vector2 centerPos = (center.x, center.y);
        Vector2 pdirection;
        pdirection.x = playerPos.x - center.position.x;
        pdirection.y = playerPos.x - center.position.y;
        pangle = Mathf.Atan2(pdirection.y, pdirection.x)*Mathf.Rad2Deg;
        if(!holdPosition)
        {
            rb.rotation = angle;
            if(pangle > 90 || pangle < -90)
            {
                rb.position = firePoint2.position;
            }
            else if (pangle <=90 || pangle >= -90)
            {
                rb.position = firePoint.position;
            }
        }
        

        if(hasGun)
        {
           getshoot(); 
           renderer.enabled = true;
        }
        else
        {
            //renderer.enabled = false;
        }
        
    }

    public void getshoot()
    {
        if(shooting == 1) {return;}
        gunanimator.Play("BroccGunShoot");
        aud.Play();
        for(float i = 0-(((float)projectiles/2)-0.5f); i <= (((float)projectiles)/2-0.5f)+0.1f; i+= 1)
        {
            Quaternion q = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z+(i*(20)));

            if(ray)
            {
                RaycastHit2D hit = Physics2D.Raycast(shootPoint.position, shootPoint.up, ignoreme);
                Vector3 spos = new Vector3(shootPoint.position.x, shootPoint.position.y, 10);
                Vector3 hpos = new Vector3(hit.point.x, hit.point.y, 10);
                line.SetPosition(0, spos);
                line.SetPosition(1, hpos);
                line.enabled = true;
                StartCoroutine(linefade());
                
                if(hit.transform.tag == "Player")
                {
                    hit.transform.GetComponent<Player>().TakeDamage(damage);
                }
                if(hit.transform.tag == "Shield")
                {
                    if(hit.transform.GetComponent<spoon>() != null)
                    {
                        hit.transform.GetComponent<spoon>().TakeDamage(damage);
                    }
                    else if(hit.transform.GetComponent<Destructible>() != null)
                    {
                        hit.transform.GetComponent<Destructible>().TakeDamage(damage);
                    }
                    
                }
                
            }
            else
            {
                GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation * q);
                Rigidbody2D bulletrb = bullet.GetComponent<Rigidbody2D>();
                bullet.GetComponent<Enemybullet>().bulletSpeed = bulletSpeed;
                bullet.GetComponent<Enemybullet>().damage = damage;
            }
            
        }
        

        this.StartCoroutine(Shoot());
    }

    public IEnumerator linefade()
    {
        yield return new WaitForSeconds(0.05f);
        line.enabled = false;
    }

    public IEnumerator Shoot()
    {
        if(shooting == 0)
        {
            shooting = 1;

            if(burstcount > 1)
            {
                yield return new WaitForSeconds(0.3f);
                burstcount --;
            }
            else
            {
                int r = UnityEngine.Random.Range(0, 2);
            
                if (r == 0)
                {
                    yield return new WaitForSeconds(defatkspd*attackSpeed);
                }
                if (r == 1)
                {
                    yield return new WaitForSeconds((defatkspd*attackSpeed)+1);
                }
                burstcount = burst;
            }

            shooting = 0;
            
        }
        
    }

    public void setHold(int i)
    {
        if(i==1)
        {
            holdPosition = true;
        }
        else
        {
            holdPosition = false;
        }
        
    }
}
