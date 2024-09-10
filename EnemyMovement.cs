using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using Pathfinding;

public class EnemyMovement : MonoBehaviour
{
    
    public Rigidbody2D rb;
    public float health = 80;
    public float maxHealth = 80;
    public Animator animator;
    new public SpriteRenderer renderer;
    public EnemyShooting gun;
    public GameObject player;
    public GameObject director;
    public bool willexplode = false;
    public bool willexplode2;
    
    public float burndamage = 0;
    public Coroutine burnroutine;
    public Coroutine deathroutine;

    public TextMeshProUGUI damagenum;
    public GameObject canvas;

    public GameObject noodleprefab;
    public GameObject explosionprefab;
    public GameObject mineprefab;
    public GameObject elecprefab;
    public GameObject milkprefab;
    public GameObject popcornprefab;
    public GameObject trail;
    public Coroutine trailroutine;
    public GameObject minionprefab;
    public AudioSource aud;
    public float pulse = 1;
    
    public bool canburrow = false;
    public int burrowing;
    public GameObject burrowtrail;

    public void Start()
    {
        canvas = GameObject.Find("Canvas");
        //willexplode2 = willexplode;
        maxHealth*=DataManager.Instance.floor;
        health = maxHealth;
        if(trail != null)
        {
            trailroutine = StartCoroutine(makeTrail());
        }
        
    }

    // Update is called once per frame
    public void Update()
    {
        gun.playerPos = player.GetComponent<Transform>().position;
        
        if(gun.pangle > 90 || gun.pangle < -90)
        {
            renderer.flipX = true;
        }
        else if(gun.pangle <=90 || gun.pangle >= -90)
        {
            renderer.flipX = false;
        }

        if(trail != null && trailroutine == null)
        {
            trailroutine = StartCoroutine(makeTrail());
        }
        if(trail == null && trailroutine != null)
        {
            StopCoroutine(trailroutine);
            trailroutine = null;
        }
        
        if(GameObject.Find("Director").GetComponent<Director>().waves == 0)
        {
            Destroy(gameObject);
        }
        if(willexplode2)
        {
            if(Vector3.Distance(this.transform.position, player.transform.position) < 25f)
            {
                willexplode = false;
                animator.SetTrigger("Explode");
                gameObject.GetComponent<AIPath>().maxSpeed = 0;

                
               

                deathroutine = StartCoroutine(delayedDeath(0.4f));
                
            }
        }
        if(canburrow)
        {
            if(burrowing == 0)
            {
                trail = null;
                gun.hasGun = true;
                gun.renderer.enabled = true;
                if(Vector3.Distance(this.transform.position, player.transform.position) > 100f)
                {
                    animator.SetTrigger("enterburrow");
                    animator.ResetTrigger("exitburrow");
                }
                else
                {
                    gameObject.GetComponent<AIPath>().maxSpeed = 15;
                    gameObject.GetComponent<Collider2D>().enabled = true;
                }
            }
            
            if(burrowing == 1)
            {
                trail = null;
                
                gun.hasGun = false;
                gun.renderer.enabled = false;
                gameObject.GetComponent<AIPath>().maxSpeed = 0;
            }
            if(burrowing == 2)
            {
                gun.hasGun = false;
                gun.renderer.enabled = false;
                gameObject.GetComponent<AIPath>().maxSpeed = 45;
                gameObject.GetComponent<Collider2D>().enabled = false;
                trail = burrowtrail;
                
                if(Vector3.Distance(this.transform.position, player.transform.position) < 20f)
                {
                    animator.SetTrigger("exitburrow");
                    animator.ResetTrigger("enterburrow");
                    
                }
            }
        }
        if(director == null)
        {
            Destroy(gameObject);
        }
        if(health < 1)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        if(deathroutine != null)
        {
            StopCoroutine(deathroutine);
        }
        if(willexplode)
        {
            GameObject e = Instantiate(explosionprefab, this.transform.position, Quaternion.identity);
            if(e.GetComponent<AOEdamage>() != null)
            {
                e.GetComponent<AOEdamage>().setvars(gun.damage, 0.5f, 24);
                e.GetComponent<AOEdamage>().source = "Explosion";
            }
            if(e.GetComponent<MultiBullet>() != null)
            {
                e.GetComponent<MultiBullet>().damage = gun.damage;
                e.GetComponent<MultiBullet>().bulletSpeed = gun.bulletSpeed;
            }
            
        }
        if(mineprefab != null)
        {
            GameObject g = Instantiate(mineprefab, transform.position, Quaternion.identity);
            g.GetComponent<ProximityMine>().damage = gun.damage*3f;
        }
        if(minionprefab != null)
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject x = Instantiate(minionprefab, transform.position, Quaternion.identity);
                x.GetComponent<EnemyMovement>().player = player;
                x.GetComponent<EnemyMovement>().director = director;
                x.GetComponent<EnemyMovement>().canvas = canvas;
                x.GetComponent<AIDestinationSetter>().target = player.GetComponent<Transform>();
                director.GetComponent<Director>().enemycount++;
            }
            
            
        }
        else{
        foreach (Item i in DataManager.Instance.playeritems)
        {
            if(i.itemname == "Instant Noodles")
            {
                if(UnityEngine.Random.Range(0, DataManager.Instance.luck) < 10*i.stacks)
                {
                    GameObject x = Instantiate(noodleprefab, this.transform.position, Quaternion.identity);
                    x.GetComponent<noodle>().h = 10;
                }
            }
            if(i.itemname == "Spilt Milk")
            {
                GameObject x = Instantiate(milkprefab, this.transform.position, Quaternion.identity);
                if(x!=null){x.GetComponent<AOEdamage>().damage = maxHealth*(0.1f*i.stacks);}
            }
            if(i.itemname == "Microwave Popcorn")
            {
                GameObject x = Instantiate(popcornprefab, this.transform.position, Quaternion.identity);
                x.GetComponent<MultiBullet>().damage = (6*i.stacks);
                x.GetComponent<MultiBullet>().bulletSpeed = 100;
            }
            
        }
        }
        if(director.GetComponent<Director>() != null)
        {
            director.GetComponent<Director>().death();
        }
        
        DataManager.Instance.money +=1;
        DataManager.Instance.totalmoney +=1;

        if(MetaDataManager.Instance.foundenemies.Contains(name) == false)
                {
                    MetaDataManager.Instance.foundenemies.Add(name);
                    if(MetaDataManager.Instance.Botanist == false)
                    {
                    int t = 0;
                    foreach (String s in MetaDataManager.Instance.foundenemies)
                    {
                        foreach (GameObject x in DataManager.Instance.allenemies)
                        {
                            if(x.name+"(Clone)" == s)
                            {
                                t++;
                            }
                        }
                    }
                    if(t == DataManager.Instance.allenemies.Count)
                    {
                        MetaDataManager.Instance.Botanist = true;
                        
                    }
                    }
                    
                }
        MetaDataManager.Instance.kills++;
        if(MetaDataManager.Instance.kills >= 1000 && MetaDataManager.Instance.Genocide == false)
        {
            MetaDataManager.Instance.Genocide = true;
        }
        
        MetaDataManager.Instance.SaveMeta();
        Destroy(this.gameObject);
    }

    public virtual void TakeDamage(float damage, String source)
    {
        bool crit = false;
        //Debug.Log(Vector3.Distance(this.transform.position, player.transform.position));
        if(source != "Trap"){
        foreach(Item i in DataManager.Instance.playeritems)
        {
            if(source != i.itemname)
            {
            if(i.itemname == "Hardtack Biscuit")
            {
                damage += 2*i.stacks;
            }
            if(i.itemname == "Chef's Knife")
            {
                if(health/maxHealth > .9)
                damage = damage*(1+(.5f*i.stacks));
            }
            if(i.itemname == "Lucky Cereal")
            {
                if(UnityEngine.Random.Range(0, DataManager.Instance.luck) < 10*i.stacks)
                {
                    damage *= 2;
                    crit = true;
                }
            }
            if(i.itemname == "Hot Sauce")
            {
                if(UnityEngine.Random.Range(0, DataManager.Instance.luck) < 10*i.stacks)
                {
                    burndamage += 5;
                    if(burnroutine == null)
                    {
                        burnroutine = StartCoroutine(burn(burndamage, 5));
                    }
                }
            }
            if(i.itemname == "Fork In A Plug")
            {
                if(UnityEngine.Random.Range(0, DataManager.Instance.luck) < 10*i.stacks)
                {
                    GameObject e = Instantiate(elecprefab, this.transform.position, Quaternion.identity);
                    e.GetComponent<AOEdamage>().setvars(damage/2f, 0.5f, 24);
                    e.GetComponent<AOEdamage>().source = "Fork In A Plug";
                }
            }
            if(i.itemname == "Bloody Mary")
            {
                if(UnityEngine.Random.Range(0, DataManager.Instance.luck) < 10*i.stacks)
                {
                    player.GetComponent<Player>().PlayerHeal(damage*0.1f, false);
                }
            }
            
            
            }
        }
        }

        TextMeshProUGUI x = Instantiate(damagenum, canvas.transform, false);
        x.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        x.gameObject.GetComponent<damagenum>().dnum = damage;
        x.gameObject.GetComponent<damagenum>().crit = crit;
        
        health -= damage;  
        //aud.Play();
        if(MetaDataManager.Instance.HeavyHitter == false && damage >= 300)
        {
            MetaDataManager.Instance.HeavyHitter = true;
            MetaDataManager.Instance.SaveMeta();
        }
    }

    public virtual void EnemyHeal(float h)
    {
        
        health += h;
        if(health > maxHealth)
        {
            health = maxHealth;
        }

        TextMeshProUGUI x = Instantiate(damagenum, canvas.transform, false);
        x.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        x.gameObject.GetComponent<damagenum>().dnum = h;
        x.gameObject.GetComponent<damagenum>().heal = true;
    }

    public IEnumerator delayedDeath(float time)
    {
        yield return new WaitForSeconds(time);
        GameObject e = Instantiate(explosionprefab, this.transform.position, Quaternion.identity);
        if(e.GetComponent<AOEdamage>() != null)
            {
                e.GetComponent<AOEdamage>().setvars(gun.damage, 0.5f, 24);
                e.GetComponent<AOEdamage>().source = "Explosion";
            }
            if(e.GetComponent<MultiBullet>() != null)
            {
                e.GetComponent<MultiBullet>().damage = gun.damage;
                e.GetComponent<MultiBullet>().bulletSpeed = gun.bulletSpeed;
            }
        Die();
    }

    public IEnumerator burn(float damage, int stacks)
    {
        burndamage = damage;
        int i = 0;
        
        while(i <= stacks-1)
        {
            renderer.color = new Color32(255, 76, 76, 255);
            yield return new WaitForSeconds(1f);
            
            TakeDamage(burndamage, "Hot Sauce");

            renderer.color = new Color32(255, 0, 0, 255);
            yield return new WaitForSeconds(0.1f);
            renderer.color = new Color32(255, 76, 76, 255);
            i++;
        }
        renderer.color = new Color32(255, 255, 255, 255);
        burndamage = 0;
        StopCoroutine(burnroutine);
        burnroutine = null;
        yield break;
    }

    void FixedUpdate()
    {
        
        
    }

    IEnumerator makeTrail()
    {
        while(true)
        {
        if(trail != null){
        GameObject e = Instantiate(trail, this.transform.position, Quaternion.identity);
            
        e.GetComponent<AOEdamage>().source = "Trail";
        e.GetComponent<AOEdamage>().damage *= DataManager.Instance.floor;
        }
        yield return new WaitForSeconds(pulse);
        
        }
    }

    public void changeburrow(int i)
    {
        burrowing = i;
    }
    
    public void aoe(float radius)
    {
        Collider2D[] objectsinrange = Physics2D.OverlapCircleAll(gameObject.transform.position, radius);
        foreach(Collider2D c in objectsinrange)
        {
           
                Player p = c.GetComponent<Player>();
                if(p != null)
                {
                    p.TakeDamage(gun.damage, "AOE");
                }
            
            
        }
    }
}
