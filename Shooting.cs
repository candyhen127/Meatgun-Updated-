using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Shooting : MonoBehaviour
{
    public Animator gunanimator;
    public Transform firePoint;
    public Transform firePoint2;
    public Transform shootPoint;
    public GameObject bulletPrefab;
    public GameObject swordblock;
    public AudioClip swordwhoosh;
    public Transform center;
    public Camera cam;
    public Rigidbody2D rb;
    public int baseMaxAmmo;
    public int maxAmmo = 6;
    
    public int ammo = 6;
    public int baseProjectiles;
    public int projectiles = 1;
    public TextMeshProUGUI AmmoDisplay;
    public TextMeshProUGUI AmmoWord;
    public int reloading = 0;
    public Coroutine reloadroutine;
    public Coroutine shootroutine;
    public int incombat = 1;
    public int shooting;
    public float baseReloadTime;
    public float reloadTime = 1.5f;
    public float baseDamage;
    public float damage = 10f;
    public float destroy;
    public bool front;

    
    
    Vector2 mousePos;

    public float bulletSpeed = 2f;
    public float attackSpeed = 0.1f;
    public float defatkspd = 0.1f;
    public float pangle; 
    public float angle;
    new public SpriteRenderer renderer;
    public Image reloadBar;
    public AudioSource aud;

    public float spread = 10;

    void Start()
    {
        AmmoWord.text = OptionsManager.Instance.g.gunname;
        baseDamage = OptionsManager.Instance.g.damage;
        baseMaxAmmo =  OptionsManager.Instance.g.ammo;
        defatkspd = OptionsManager.Instance.g.fireRate;
        baseReloadTime = OptionsManager.Instance.g.reloadTime;
        baseProjectiles = OptionsManager.Instance.g.projectiles;
        destroy = OptionsManager.Instance.g.destroy;
        bulletPrefab = OptionsManager.Instance.g.bulletPrefab;
        front = OptionsManager.Instance.g.front;

        damage = baseDamage;
        maxAmmo = baseMaxAmmo;
        reloadTime = baseReloadTime;
        projectiles = baseProjectiles;
        reloadBar.enabled = false;
        ammo = maxAmmo;

        gunanimator.runtimeAnimatorController = OptionsManager.Instance.g.animator;
        updateItems();

        if(OptionsManager.Instance.g.gunname == "FISH")
        {
            aud.clip = swordwhoosh;
        }
    }

    public void updateItems()
    {
        damage = baseDamage;
        attackSpeed = 1;
        reloadTime = baseReloadTime;
        projectiles = baseProjectiles;
        foreach(Item i in DataManager.Instance.playeritems)
        {
            if(i.itemname == "Takeout Box")
            {
                maxAmmo = baseMaxAmmo+(2*i.stacks);
                ammo = maxAmmo;
            }
            if(i.itemname == "Fast Food")
            {
                attackSpeed *= (Mathf.Pow(0.85f, i.stacks));
            }
            if(i.itemname == "24-Hour Energy")
            {
                reloadTime *= (Mathf.Pow(0.85f, i.stacks));
            }
            if(i.itemname == "Double Cherry")
            {
                projectiles += i.stacks;
                spread = 60f/projectiles;
            }
            if(i.itemname == "Life Core")
            {
                attackSpeed *= (Mathf.Pow(0.95f, i.stacks));
                reloadTime *= (Mathf.Pow(0.95f, i.stacks));
                damage *= 1 + (.05f * i.stacks);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(DataManager.Instance.paused == true){return;}
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        if(pangle > 90 || pangle < -90)
        {
            renderer.flipY = true;
        }
        else if(pangle <=90 || pangle >= -90)
        {
            renderer.flipY = false;
        }
        AmmoDisplay.text = ammo.ToString() + "/" + maxAmmo.ToString();
        if(ammo == 0)
        {
            AmmoDisplay.color = new Color(255, 0, 0, 255);
        }
        else
        {
            AmmoDisplay.color = new Color(255, 255, 255, 255);
        }
        if(reloading == 1) {return;}
        if(reloadBar.enabled) {return;}
        if(incombat == 1){
            AmmoDisplay.gameObject.SetActive(true);
        if(Input.GetButton("Fire1"))
        {
            if(ammo > 0)
            {
                getshoot(damage);
            }
        }
        if(Input.GetButtonDown("Fire1"))
        {
            if(ammo == 0)
            {
                reloadroutine = StartCoroutine(Reload());
            }
        }
        if(Input.GetKeyDown("r") && ammo < maxAmmo && GameObject.Find("Player").GetComponent<Player>().consuming == 0)
        {
            reloadroutine = StartCoroutine(Reload());
        }
        }
        else
        {
            AmmoDisplay.gameObject.SetActive(false);
        }

        if(Input.GetButtonUp("Fire1"))
        {
            if(shooting == 0){return;}
            if(OptionsManager.Instance.g.gunname == "CHICKEN")
            {
                shooting = 0;
            }
            
            gunanimator.SetFloat("firing", 0);
        }
        

        

        
    }

    void FixedUpdate()
    {
        
        Vector2 direction = mousePos - rb.position;
        angle = Mathf.Atan2(direction.y, direction.x)*Mathf.Rad2Deg;
        //Vector2 centerPos = (center.x, center.y);
        Vector2 pdirection;
        pdirection.x = mousePos.x - center.position.x;
        pdirection.y = mousePos.x - center.position.y;
        pangle = Mathf.Atan2(pdirection.y, pdirection.x)*Mathf.Rad2Deg;

        rb.rotation = angle;
        if(pangle > 90 || pangle < -90)
        {
            rb.position = firePoint2.position;
        }
        else if (pangle <=90 || pangle >= -90)
        {
            rb.position = firePoint.position;
        }

        if(front)
        {
            if(angle > 0 ) {renderer.sortingOrder = 0;}
        else{renderer.sortingOrder = 2;}
        }
        else
        {
            if(angle > 0 ) {renderer.sortingOrder = 2;}
        else{renderer.sortingOrder = 0;}
        }
        

    }
     float getPangle(){return pangle;}


    public void getshoot(float d)
    {
        
        while(Input.GetButton("Fire1") && ammo > 0)
        {
            if(shooting == 1) {return;}
            if(reloading == 1) {return;}
            gunanimator.Play("GunFire");
            aud.Play();
            for(float x = 0-(((float)projectiles/2)-0.5f); x <= (((float)projectiles)/2-0.5f)+0.1f; x+= 1)
            {
                Quaternion q = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z+(x*(spread)));
                GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation * q);
                
                //bullet.GetComponent<bullet>().bulletSpeed = bulletSpeed;

                float temp = d;

                foreach (Item i in DataManager.Instance.playeritems)
                {
                    if(i.itemname == "Salt Shaker" && ammo == maxAmmo)
                    {
                        temp*= 1+.5f*i.stacks;
                    }
                    if(i.itemname == "Pepper Shaker" && ammo == 1)
                    {
                        temp*= 1+.5f*i.stacks;
                    }
                }
                bullet.GetComponent<bullet>().damage = temp;
                bullet.GetComponent<bullet>().StartCoroutine(bullet.GetComponent<bullet>().bulletDestroy(destroy));
                
                
                
            }
                if(OptionsManager.Instance.g.gunname == "FISH")
                {
                    GameObject sword = Instantiate(swordblock, shootPoint.position, shootPoint.rotation);
                    sword.GetComponent<Melee>().damage = damage;
                    sword.GetComponent<Melee>().player = GameObject.Find("Player");
                    sword.GetComponent<Melee>().StartCoroutine(sword.GetComponent<Melee>().bulletDestroy(destroy));
                }
            ammo--;
            GameObject shopkeep = GameObject.Find("Cool Broccoli");
                if(shopkeep != null)
                {
                    shopkeep.GetComponent<Animator>().SetTrigger("mad");
                    DataManager.Instance.cost++;
                    if(MetaDataManager.Instance.ReadTheSignIdiot == false)
                    {
                        MetaDataManager.Instance.ReadTheSignIdiot = true;
                        MetaDataManager.Instance.SaveMeta();
                    }
                }
            GameObject.Find("Player").GetComponent<Player>().shotsfired = true;
            shootroutine = this.StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot()
    {
        if(shooting == 0)
        {
            shooting = 1;
            yield return new WaitForSeconds(defatkspd*attackSpeed);
            shooting = 0;
        }
        
        
    }
    IEnumerator Reload()
    {
        gunanimator.SetFloat("firing", 0.5f);
        reloading = 1;
        float time = 0;
        while (time<reloadTime)
        {
            yield return new WaitForSeconds(0);
            time+=Time.deltaTime;
            UpdateReloadBar(time, reloadTime);
            reloadBar.color = new Color(255, 255, 255, 255);
            reloadBar.enabled = true;
        }
        
        ammo = maxAmmo;
        reloading = 0;
        reloadBar.enabled = false;
        gunanimator.SetFloat("firing", -0.5f);
        reloadroutine = null;
        yield break;
    }

    public void stopreload()
    {
        if(reloadroutine != null){StopCoroutine(reloadroutine);}
        reloading = 0;
        reloadBar.enabled = false;
        gunanimator.SetFloat("firing", -0.5f);
        reloadroutine = null;
        if(shootroutine != null){StopCoroutine(shootroutine);}
        shooting = 0;
        shootroutine = null;

        
    }

    public void UpdateReloadBar(float t, float x)
    {
        reloadBar.fillAmount = Mathf.Clamp(t / x, 0, 1f);
    }
}
