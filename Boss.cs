using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using Pathfinding;

public class Boss : EnemyMovement
{

    public String bossname;
    public String bosstitle;
    public HealthBar healthbar;
    public Animator bossanim;
    public GameObject weapon1;
    public GameObject weapon0;
    public GameObject weapon2;
    public GameObject weapon3;
    public GameObject weapon4;
    public GameObject minion;
    public GameObject shield;
    public GameObject shield2;
    public GameObject spotlight;
    public List<GameObject> spoonList;
    public List<Transform> spwnlist;
    public bool ready1 = true;
    public bool ready2 = true;
    public bool ready3 = true;
    public int phase = 1;
    public float cooldown = 6;
    
    public TextMeshProUGUI BossName;

    public int mode;
    public Coroutine attackroutine;
    
    public List<Transform> teleportpoints;
    public RuntimeAnimatorController gunanim2;
    

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        startAttacks();
        healthbar = GameObject.Find("BossHealthBar").GetComponent<HealthBar>();
        healthbar.boss = this;
        if(shield != null)
        {
            spawnShields(shield);
        }
        spotlight = GameObject.Find("Spotlight");
        if(bossname == "Meat Substitute")
        {
            setTarget();
        }
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        if(mode == 0)
        {  
            
            GetComponent<AIPath>().canMove = true;
            
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            gun.hasGun = true;
        }
        else if (mode == 1)
        {
            GetComponent<AIPath>().canMove = false;
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            gun.hasGun = false;
            gun.renderer.enabled = false;
        }
        else
        {
            GetComponent<AIPath>().canMove = false;
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            gun.hasGun = false;
            gun.renderer.enabled = true;
        }
        
        if(spotlight !=null)
        {
            if(GameObject.Find("Yelectric 1(Clone)") != null || GameObject.Find("Batterylaser(Clone)") != null)
            {
                spotlight.GetComponent<SpriteRenderer>().color = new Color32(0, 0, 0, 0);
            }
            else
            {
                spotlight.GetComponent<SpriteRenderer>().color = new Color32(0, 0, 0, 125);
            }
        }

        if(bossname == "Meat Substitute" && Vector3.Distance(GetComponent<AIDestinationSetter>().target.position, transform.position)<50f)
        {
            setTarget();
        }
    }

    void startAttacks()
    {
        mode = 0;
        if(attackroutine == null)
        {
            attackroutine = StartCoroutine(attacks());
        }
        
    }

    IEnumerator changePhase()
    {
        
        StopCoroutine(attackroutine);
        attackroutine = null;
        if(phase == 1)
        {
            phase = 0;
            bossanim.SetTrigger("phasechange");
            GetComponent<AIPath>().maxSpeed+= 5;
            cooldown -= 2;
            if(bossname == "24-Carrot")
            {
                gun.burst += 1;
            }
            if(bossname == "Potato Battery")
            {
                GetComponent<AIPath>().maxSpeed-= 5;
                gun.projectiles +=1;
            }
            if(bossname == "Meat Substitute")
            {
                gun.projectiles +=2;
                gun.burst -= 2;
                GetComponent<AIPath>().maxSpeed-= 5;
                gun.gameObject.GetComponent<Animator>().runtimeAnimatorController = gunanim2;
            }
            yield return new WaitForSeconds(4);
            phase = 2;
        }
    }

    void setPhase(int ph)
    {
        
        phase = ph;
        startAttacks();
    }

    IEnumerator attacks()
    {
        yield return new WaitForSeconds(cooldown);
        while(true)
        {
            //mode = 0;
            yield return new WaitForSeconds(cooldown);
            if(mode == 0)
            {
                mode = 1;
            int r = UnityEngine.Random.Range(0, 3);
            if(r==0 && ready1)
            {   
                if(bossname == "Carolina Reaper")
                {
                if(weapon1.GetComponent<scythe>().mode == 0)
                {
                    bossanim.SetTrigger("weapon1");
                    
                }
                }
                else
                {
                    bossanim.SetTrigger("weapon1");
                    StartCoroutine(cool(1));
                }
            }
            if(r == 1 && ready2)
            {
                
                bossanim.SetTrigger("weapon2");
                StartCoroutine(cool(2));
            }
            if(r == 2 && ready3)
            {
                
                bossanim.SetTrigger("weapon3");
                StartCoroutine(cool(3));
            }
            }
            
        }
    }

    public IEnumerator cool(int ready)
    {
        if(ready == 1)
        {
            ready1 = false;
        }
        if(ready == 2)
        {
            ready2 = false;
        }
        if(ready == 3)
        {
            ready3 = false;
        }
        
        yield return new WaitForSeconds(cooldown);
        
        if(ready == 1)
        {
            ready1 = true;
        }
        if(ready == 2)
        {
            ready2 = true;
        }
        if(ready == 3)
        {
            ready3 = true;
        }

        if(bossname == "Meat Substitute")
        {
            setTarget();
        }
    }

    public override void TakeDamage(float damage, String source)
    {
        base.TakeDamage(damage, source);
        
        healthbar.UpdateHealthBar();
        if(health < maxHealth/2 && phase == 1)
        {
            StartCoroutine(changePhase());
        }
    }

    public override void EnemyHeal(float h)
    {
        base.EnemyHeal(h);
        
        healthbar.UpdateHealthBar();
        
    }

    public override void Die()
    {
        if(MetaDataManager.Instance.HarvestSeason == false && bossname == "Carolina Reaper")
        {
            MetaDataManager.Instance.HarvestSeason = true;
            MetaDataManager.Instance.SaveMeta();
        }
        if(MetaDataManager.Instance.RootOfAllEvil == false && bossname == "24-Carrot")
        {
            MetaDataManager.Instance.RootOfAllEvil = true;
            MetaDataManager.Instance.SaveMeta();
        }
        if(MetaDataManager.Instance.GreenEnergy == false && bossname == "Potato Battery")
        {
            MetaDataManager.Instance.GreenEnergy = true;
            MetaDataManager.Instance.SaveMeta();
        }
        if(MetaDataManager.Instance.DoppelGanger == false && bossname == "Meat Substitute")
        {
            MetaDataManager.Instance.DoppelGanger = true;
            MetaDataManager.Instance.SaveMeta();
        }
        base.Die();
    }

    public void setMode(int m)
    {
        mode = m;
    }

    public void useweapon1(int i)
    {
        mode = 1;
        if(bossname == "Carolina Reaper")
        {
        scythe s = weapon1.GetComponent<scythe>();
        s.damage = gun.damage;
        if(s != null)
        {
            s.throwroutine = s.StartCoroutine(s.throwScythe());
        }
        }
        if(bossname == "24-Carrot")
        {
            Quaternion q;
            if(i == 0)
            {
                q = Quaternion.identity;
            }
            else
            {
                q = Quaternion.Euler(0, 0, 11.25f);
            }
            GameObject bullet = Instantiate(weapon1, transform.position, q);
            bullet.GetComponent<MultiBullet>().damage = gun.damage;
            bullet.GetComponent<MultiBullet>().bulletSpeed = gun.bulletSpeed;
            
            
        }
        if(bossname == "Potato Battery")
        {
            GameObject bullet;
            if(i == 0)
            {
                bullet = Instantiate(weapon1, GameObject.Find("Synthesizer").transform.position, Quaternion.identity);
            }
            else
            {
                bullet = Instantiate(weapon0, GameObject.Find("Synthesizer").transform.position, Quaternion.identity);
            }
            bullet.GetComponent<MultiBullet>().damage = gun.damage*2;
            
            
            
        }
        if(bossname == "Meat Substitute")
        {
            Quaternion q;
            if(i == 0)
            {
                q = Quaternion.identity;
            }
            else
            {
                q = Quaternion.Euler(0, 0, 22.5f);
            }
            GameObject bullet = Instantiate(weapon1, transform.position, q);
            bullet.GetComponent<MultiBullet>().damage = gun.damage;
            bullet.GetComponent<MultiBullet>().bulletSpeed = gun.bulletSpeed;
            
            
        }
    }

    public void useweapon2()
    {
        
        mode = 1;
        if(bossname == "Potato Battery" || bossname == "Meat Substitute"){mode = 2;}
        if(bossname == "Carolina Reaper")
        {
        int projectiles;
        if(phase == 1)
        {
            projectiles = 1;
        }
        else
        {
            projectiles = 3;
        }
        for(float i = 0-(((float)projectiles/2)-0.5f); i <= (((float)projectiles)/2-0.5f)+0.1f; i+= 1)
        {
            Quaternion q = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z+(i*(15)));
            GameObject bullet = Instantiate(weapon2, transform.position, gun.shootPoint.rotation*q);
            bullet.GetComponent<Enemybullet>().damage = gun.damage*2;

        }
        }
        if(bossname == "24-Carrot")
        {
            GameObject bullet = Instantiate(weapon2, gun.transform.position, gun.shootPoint.rotation);
            bullet.GetComponent<Enemybullet>().damage = gun.damage;
            bullet.GetComponent<Enemybullet>().bulletSpeed = gun.bulletSpeed;
            bullet.GetComponent<Enemybullet>().health = 100*DataManager.Instance.floor;
            if(phase == 1)
            {
                bullet.GetComponent<Enemybullet>().explosionprefab = weapon1;
            }
            
        }
        if(bossname == "Potato Battery")
        {
            GameObject bullet = Instantiate(weapon2, gun.shootPoint.position, gun.shootPoint.rotation);
            bullet.GetComponent<Enemybullet>().damage = gun.damage*5;
            bullet.GetComponent<Enemybullet>().bulletSpeed = gun.bulletSpeed*0.75f;
            bullet.GetComponent<Enemybullet>().health = 50*DataManager.Instance.floor;
            
        }
        if(bossname == "Meat Substitute")
        {
            GameObject bullet = Instantiate(weapon2, gun.shootPoint.position, gun.shootPoint.rotation);
            bullet.GetComponent<Enemybullet>().damage = gun.damage*2f;
            bullet.GetComponent<Enemybullet>().bulletSpeed = gun.bulletSpeed*5f;
            
        }
        
    }

    public void useweapon3()
    {
        if(bossname == "Potato Battery" || bossname == "Meat Substitute"){mode = 2;}
        else{mode = 1;}
        GameObject weapon;
        if(bossname == "Carolina Reaper")
        {
        if(phase == 1)
        {
            weapon = weapon3;
        }
        else
        {
            weapon = weapon2;
        }
        for(int i = 1; i <=4; i++)
        {
            float angle = 90*(i-1);
            GameObject bullet = Instantiate(weapon, transform.position, Quaternion.Euler(new Vector3(0, 0, angle)));
            bullet.transform.position = bullet.transform.position + bullet.transform.up*30;
            if(phase == 1)
            {
                bullet.GetComponent<MultiBullet>().damage = gun.damage;
                bullet.GetComponent<MultiBullet>().bulletSpeed = gun.bulletSpeed-10;
            }
            if(phase == 2)
            {
                bullet.GetComponent<Enemybullet>().damage = gun.damage*2;
            }
            
            
        }
        }
        if(bossname == "24-Carrot")
        {
            for (int i = 0; i < phase; i++)
            {
                GameObject bullet = Instantiate(weapon3, transform.position, Quaternion.Euler(new Vector3(0, 0, i*22.5f)));
                bullet.GetComponent<MultiBullet>().damage = gun.damage;
                bullet.GetComponent<MultiBullet>().bulletSpeed = gun.bulletSpeed;
            }
            
        }
        if(bossname == "Potato Battery")
        {
            GameObject bullet = Instantiate(weapon3, gun.shootPoint.position, gun.shootPoint.rotation);
            bullet.GetComponent<Elecwall>().damage = gun.damage/(1*DataManager.Instance.floor);
            bullet.GetComponent<Elecwall>().power = 1;
            bullet.GetComponent<Elecwall>().parent = gun.shootPoint.gameObject;
            bullet.GetComponent<BoxCollider2D>().enabled = true;
            Destroy(bullet, 0.75f);
            
        }
        if(bossname == "Meat Substitute")
        {
            GameObject bullet = Instantiate(weapon3, gun.shootPoint.position, gun.shootPoint.rotation);
            bullet.GetComponent<Enemybullet>().damage = gun.damage;
            bullet.GetComponent<Enemybullet>().bulletSpeed = gun.bulletSpeed;
            
            
        }
        
    }

    public void useweapon4()
    {
        mode = 1;
        if(bossname == "24-Carrot")
        {
            GameObject bullet = Instantiate(weapon4, new Vector3(0,200,0), Quaternion.identity);
                bullet.GetComponent<MultiBullet>().damage = gun.damage*1.5f;
                bullet.GetComponent<MultiBullet>().bulletSpeed = gun.bulletSpeed;
        }
    }

    public void setGunMode(int i)
    {
        gun.gameObject.GetComponent<Animator>().SetInteger("Mode", i);
    }

    public void playGunAnim(String s)
    {
        gun.gameObject.GetComponent<Animator>().Play(s);
    }

    public void spawnShields(GameObject spoon)
    {
        foreach(GameObject s in spoonList)
        {
            Destroy(s);
        }
        spoonList.Clear();
        for(int i = 0; i < 4; i++)
        {
            GameObject sp = Instantiate(spoon, transform, false);
            spoonList.Add(sp);
            sp.transform.RotateAround(transform.position, Vector3.forward, i*(360/4));
            sp.GetComponent<SpriteRenderer>().enabled = true;
            sp.GetComponent<Destructible>().rotatepoint = this.gameObject;
            sp.GetComponent<Destructible>().health = 125*DataManager.Instance.floor;
        }
    }

    public void spawnAdds()
    {
        List<GameObject> enemyList = new List<GameObject>();

        foreach(Transform t in spwnlist)
        {
            
            enemyList.Add(Instantiate(minion, t.position, t.rotation));
            
            
        }
        

        foreach(GameObject x in enemyList)
        {
            x.GetComponent<EnemyMovement>().player = player;
            x.GetComponent<EnemyMovement>().director = this.gameObject;
            x.GetComponent<AIDestinationSetter>().target = player.GetComponent<Transform>();
            
        }
    }

    public IEnumerator teleport()
    {
        mode = 1;
        for(float i = 1f; i > 0; i -= 0.2f)
        {
            yield return new WaitForSeconds(0.1f);
            renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, i);
        }
        renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0);
        weapon1.GetComponent<SpriteRenderer>().sprite = weapon1.GetComponent<scythe>().idle;
        transform.position = weapon1.transform.position;
        for(float i = 0f; i < 1; i += 0.2f)
        {
            yield return new WaitForSeconds(0.1f);
            renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, i);
        }
        renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 1);
        mode = 0;
        
    }
    public void randomTeleport()
    {
        int i = UnityEngine.Random.Range(0, teleportpoints.Count);
        transform.position = teleportpoints[i].position;
        
    } 
    public void goToCenter()
    {
        
        transform.position = GameObject.Find("Synthesizer").transform.position;
        
    } 
    public void hideBoss(int i)
    {
        if(i == 1)
        {
            GetComponent<Collider2D>().enabled = false;
            transform.GetChild(4).gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
        else if (i == 0)
        {
            GetComponent<Collider2D>().enabled = true;
            transform.GetChild(4).gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    public void resetTriggers()
    {
        
        bossanim.ResetTrigger("weapon1");
        bossanim.ResetTrigger("weapon2");
    }

    public void playClip(AudioClip c)
    {
        aud.clip = c;
        aud.Play();
    }

    public void stopClip()
    {
        
        aud.Stop();
    }

    public void setTarget()
    {   
        List<Transform> ordered = new List<Transform>();
        foreach (Transform t in teleportpoints)
        {
            float d = Vector3.Distance(t.position, player.transform.position);
            if(ordered.Count == 0)
            {
                ordered.Add(t);
            }
            else
            {
                for(int i = 0; i < ordered.Count; i++)
                {
                    if(d<Vector3.Distance(ordered[i].position, player.transform.position))
                    {
                        ordered.Insert(i, t);
                        i++;
                    }
                }
                if(d>Vector3.Distance(ordered[ordered.Count-1].position, player.transform.position))
                {
                    ordered.Insert(ordered.Count, t);
                }
            }
            
        }
        GetComponent<AIDestinationSetter>().target = ordered[(int)UnityEngine.Random.Range(3, 6)];
    }
}
