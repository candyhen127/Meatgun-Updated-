using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    public float baseMoveSpeed = 30f;
    public float moveSpeed = 0.1f;
    public Rigidbody2D rb;
    Vector2 movement;
    public float baseHealth = 100f;
    public float health = 100f;
    public float maxHealth = 100f;
    public float baseConsumeTime = 1.5f;
    public float consumeTime = 1.5f;
    public String ability;
    public GameObject abilityprefab;
    public bool abilityready = true;
    public float baseabilitycooldown;
    public float abilitycooldown;
    public int lives = 1;
    public HealthBar healthbar;
    public Animator animator;
    new public SpriteRenderer renderer;
    public Shooting gun;
    public int consuming = 0;
    public int incombat = 1;
    public int invincibility = 0;
    int bandaiding = 0;
    int bungusing = 0;
    float bungusheal = 0;
    float bungusdelay = 2;
    float regenthreshold = 0;
    float coffeespeed = 0;
    float tempspeed = 0;
    Coroutine lastroutine;
    Coroutine abilityroutine;
    Coroutine bandaidroutine;
    Coroutine bungusroutine;
    public GameObject director;
    public Menu canvas;
    public fadeout f;

    public Image reloadBar;
    public Image abilityBar;

    public TextMeshProUGUI HealthText;
    public TextMeshProUGUI InteractText;
    public TextMeshProUGUI damagenum;

    public GameObject spoon;
    public int spoonnum;
    public List<GameObject> spoonList;

    public GameObject kitgun;
    public int kitgunnum;
    public List<GameObject> kitgunList;
    public GameObject bubbleshield;
    public GameObject acid;
    public GameObject gummyAoe;

    public GameObject spotlight;

    public Transform spawnup;
    public Transform spawndown;
    public Transform spawnleft;
    public Transform spawnright;

    public AudioSource aud;
    public AudioClip hit;
    public AudioClip heal;
    public AudioClip revive;


    public bool shotsfired = false;

    void Start()
    {
        baseHealth = OptionsManager.Instance.p.maxHealth;
        baseMoveSpeed = OptionsManager.Instance.p.moveSpeed;
        ability = OptionsManager.Instance.p.ability;
        abilityprefab = OptionsManager.Instance.p.abilityprefab;
        baseabilitycooldown = OptionsManager.Instance.p.cooldown;
        abilityBar = GameObject.Find("AbilityBar").GetComponent<Image>();
        animator.runtimeAnimatorController = OptionsManager.Instance.p.animator;
    

        InteractText.enabled = false;
        abilityBar.enabled = false;
        health = DataManager.Instance.health;
        updateItems(true);
        
        healthbar.UpdateHealthBar();
        if(regenthreshold > 0 )
        {
            StartCoroutine(Regen());
        }
        
        if(DataManager.Instance.lastDoor == "Up")
        {
            rb.position = spawndown.position;
        }
        if(DataManager.Instance.lastDoor == "Down" || DataManager.Instance.lastDoor == "Elevator")
        {
            rb.position = spawnup.position;
        }
        if(DataManager.Instance.lastDoor == "Left")
        {
            rb.position = spawnright.position;
        }
        if(DataManager.Instance.lastDoor == "Right")
        {
            rb.position = spawnleft.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(DataManager.Instance.paused == true){return;}
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        
        if(movement.sqrMagnitude == 0 && bungusheal > 0 && bungusing == 0 && incombat == 1)
        {
            bungusroutine = StartCoroutine(Bungus());
            
        }
        else if (movement.sqrMagnitude != 0 && bungusroutine != null)
        {
            StopCoroutine(bungusroutine);
            bungusing = 0;
            bungusdelay = 2;
        }


        animator.SetFloat("Speed", movement.sqrMagnitude);
        if(SceneManager.GetActiveScene().name != "B1-0" &&
        SceneManager.GetActiveScene().name != "B1-Shop" &&
        SceneManager.GetActiveScene().name != "B2-Shop"&&
        SceneManager.GetActiveScene().name != "B3-Shop"&&
        SceneManager.GetActiveScene().name != "B4-Shop")
        {
            if(director.GetComponent<Director>().getWaves() == 0 || DataManager.Instance.lastDoorType > 4)
            {
                incombat = 0;
            }
        }
        gun.incombat = incombat;

        HealthText.text = ((int)(health)).ToString() + "/" + ((int)(maxHealth)).ToString();

        if(gun.pangle > 90 || gun.pangle < -90)
        {
            renderer.flipX = true;
        }
        else if(gun.pangle <=90 || gun.pangle >= -90)
        {
            renderer.flipX = false;
        }
        if(incombat == 1)
        {
        if(Input.GetButton("Fire2") && gun.reloading == 0 && consuming == 0 && gun.ammo > 0)
        {
            consuming = 1;
            
            lastroutine = StartCoroutine(Consume());
        }
        }
        if((Input.GetButtonUp("Fire2") && consuming == 1))
        {
            consuming = 0;
            StopCoroutine(lastroutine);
            reloadBar.enabled = false;
            gun.renderer.enabled = true;
        }

        if(Input.GetKeyDown("space") && abilityready && consuming == 0)
        {
            useAbility();
            
            abilityroutine = StartCoroutine(coolAbility());
        }
        

        
    }

    void FixedUpdate()
    {
        if(consuming == 0)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            rb.MovePosition(rb.position);
        }
        
        if(gun.angle > 0 ) {animator.SetFloat("Angle", 1);}
        else{animator.SetFloat("Angle", 0);}
        animator.SetInteger("Consuming", consuming);

        if(spotlight != null)
        {
            spotlight.transform.position = transform.position;
        }
    }

    public void updateItems(bool start)
    {
        gun.updateItems();
        maxHealth = baseHealth;
        moveSpeed = baseMoveSpeed;
        consumeTime = baseConsumeTime;
        abilitycooldown = baseabilitycooldown;
        foreach(Item i in DataManager.Instance.playeritems)
        {
            if(i.itemname == "T-Bone Steak")
            {
                maxHealth += (20*i.stacks);
                
            }
            if(i.itemname == "Chili Dog")
            {
                moveSpeed *= (1+(.1f*i.stacks));
            }
            if(i.itemname == "Bustling Fungus")
            {
                bungusheal = i.stacks*2;
            }
            if(i.itemname == "Comically Large Spoon")
            {
                
                spoonnum = i.stacks;
                if(spoonnum > spoonList.Count)
                {
                    spoonUpdate();
                }
            }
            if(i.itemname == "Rewards Card")
            {
                if(start)
                {
                    DataManager.Instance.cardbonus += i.stacks;
                    
                    health+=i.stacks;
                }
                maxHealth+=DataManager.Instance.cardbonus;
                
            }
            if(i.itemname == "Trail Mix")
            {
                consumeTime *= Mathf.Pow(0.9f, i.stacks);
            }
            if(i.itemname == "Fortune Cookie")
            {
                DataManager.Instance.luck = 100-(5*i.stacks);
            }
            if(i.itemname == "Kitchen Gun")
            {
                
                kitgunnum = i.stacks;
                if(kitgunnum > kitgunList.Count)
                {
                    kitgunUpdate();
                }
            }
            if(i.itemname == "Candy Apple")
            {
                if(bubbleshield != null)
                {
                    bubbleshield.GetComponent<Destructible>().health = i.stacks*(maxHealth*0.25f);
                }
                
            }
            if(i.itemname == "Game-Fuel")
            {
                abilitycooldown *= Mathf.Pow(0.8f, i.stacks);
            }
            if(i.itemname == "Ambrosia")
            {
                lives= 1+i.stacks;
            }
            if(i.itemname == "Breadstick Bowl")
            {
                regenthreshold = 20*i.stacks;
            }
            
            
                if(i.itemname == "Life Core")
                {
                    maxHealth += .05f * baseHealth * i.stacks;
                    
                    moveSpeed *= 1 + (.05f * i.stacks);
                    consumeTime *= Mathf.Pow(0.95f, i.stacks);
                }
            if(i.itemname == "Shotgun Coffee")
            {
                tempspeed = moveSpeed;
                coffeespeed = moveSpeed*(1+(0.25f*i.stacks));
                if(health < 0.3f*maxHealth)
                {
                    moveSpeed = coffeespeed;
                }
                else
                {
                    moveSpeed = tempspeed;
                }
            }
            

            if(i.stacks == 0)
            {
                DataManager.Instance.playeritems.Remove(i);
            }
            
        }
        canvas.updateInv();
        healthbar.UpdateHealthBar();
    }

    

    public void PlayerHeal(float h, bool green)
    {
        foreach(Item i in DataManager.Instance.playeritems)
        {
            if(i.itemname == "Chocolate Chip")
            {
                h+=i.stacks;
            }
            if(i.itemname == "Pan Galactic Gargle-Blaster")
            {
                GameObject g = Instantiate(acid, this.transform.position, Quaternion.identity);
                if(g!=null){g.GetComponent<AOEdamage>().damage = 10;
                g.GetComponent<AOEdamage>().destroy = 1f*i.stacks;}
            }
        }
        if(green)
        {
            f.getheal();
            aud.clip = heal;
            aud.Play();
        }
        health += h;

        if(coffeespeed != 0 && health >= maxHealth*0.3f)
        {
            moveSpeed = tempspeed;
        }
        healthbar.UpdateHealthBar();

        TextMeshProUGUI x = Instantiate(damagenum, canvas.transform, false);
        x.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        x.gameObject.GetComponent<damagenum>().dnum = h;
        x.gameObject.GetComponent<damagenum>().heal = true;
    }

    public void TakeDamage(float damage)
    {
        if(health < 1){return;}
        if(invincibility == 1){return;}
        
        if(bandaiding == 1)
        {
            StopCoroutine(bandaidroutine);
        }
        

        foreach(Item i in DataManager.Instance.playeritems)
        {
            if(i.itemname == "Band-Aid")
            {
                bandaidroutine = StartCoroutine(Bandaid(i.stacks));
            }
            if(i.itemname == "Oven Mitts")
            {
                if(UnityEngine.Random.Range(0, DataManager.Instance.luck) < 15*i.stacks)
                {
                    damage *= 0.2f;
                }
            }
            if(i.itemname == "Chocolate Coins")
            {
                DataManager.Instance.money += i.stacks;
            }
            if(i.itemname == "Sour Gummies")
            {
                GameObject g = Instantiate(gummyAoe, transform.position, Quaternion.identity);
                g.GetComponent<AOEdamage>().damage = damage * i.stacks;
            }
            if(i.itemname == "Aloe Vera")
            {
                PlayerHeal(1*i.stacks, false);
            }
        }

        TextMeshProUGUI x = Instantiate(damagenum, canvas.transform, false);
        x.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        x.gameObject.GetComponent<damagenum>().dnum = damage;

        Invince();
        health -= damage;
        aud.clip = hit;
            aud.Play();
        
        if(health < 1)
        {
            lives--;
            List<Item> removelist = new List<Item>();
            foreach(Item i in DataManager.Instance.playeritems)
            {
                if(i.itemname == "Ambrosia")
                {
                    i.stacks--;
                    
                }
                if(i.stacks == 0)
                {
                    removelist.Add(i);
                }
            }
            foreach (Item i in removelist)
            {
                DataManager.Instance.playeritems.Remove(i);
            }
            updateItems(false);
            
            MetaDataManager.Instance.deaths ++;
            if(MetaDataManager.Instance.SkillIssue == false && MetaDataManager.Instance.deaths >= 10)
            {
                MetaDataManager.Instance.SkillIssue = true;
                MetaDataManager.Instance.SaveMeta();
            }

            if(lives <1)
            {
                StartCoroutine(PlayerDeath());
            }
            else
            {
                f.get1flash();
                health = 0;
                PlayerHeal(maxHealth*0.25f, false);
                aud.clip = revive;
                aud.Play();
                GameObject.Find("Pickup").GetComponent<Pickup>().getPopUp(DataManager.Instance.emptybottle);
                DataManager.Instance.addItem(DataManager.Instance.emptybottle);
                updateItems(false);
            }
        }
        else
        {
            if(damage < 10)
            {

            }
            else
            {
                f.getdamage();
            }
            
        }

        if(coffeespeed != 0 && health < maxHealth*0.3f)
        {
            moveSpeed = coffeespeed;
        }

        healthbar.UpdateHealthBar();
        
    }

    public void TakeDamage(float d, String source)
    {
        if(source == "Recoil")
        {
            d/=3;
        }
        TakeDamage(d);
    }

        
    

    public IEnumerator PlayerDeath()
    {
        
        DataManager.Instance.gameover = true;
        canvas.GameOver();
        for(float i = 1f; i>=0; i-=Time.unscaledDeltaTime)
        {
            Time.timeScale = i;
            yield return null;
        }
        Time.timeScale = 0;
        
        
    
    }


    IEnumerator Bandaid(int s)
    {
        bandaiding = 1;
        yield return new WaitForSeconds(2);
        
        PlayerHeal(5*s, true);
        
        bandaiding = 0;
    }

    IEnumerator Bungus()
    {
        bungusing = 1;
        yield return new WaitForSeconds(bungusdelay);
        if(bungusdelay == 2)
        {
            bungusdelay = 1;
        }
        
        PlayerHeal(bungusheal, false);
        bungusing = 0;
        
    }
    IEnumerator Regen()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.5f);
        
            if((health < maxHealth*(regenthreshold/100f)) && incombat == 1)
            {
                PlayerHeal(1, false);
            }
        }
    }
        

    IEnumerator Invince()
    {
        invincibility = 1;
        yield return new WaitForSeconds(0.5f);
        invincibility = 0;
    }

    IEnumerator Consume()
    {
        consuming = 1;
        gun.renderer.enabled = false;
        float t = 0;
        while (t<consumeTime)
        {
            if(Input.GetKeyUp(KeyCode.Mouse1))
            {
                consuming = 0;
                gun.UpdateReloadBar(0, 1);
                yield break;
            }
            yield return new WaitForSeconds(0);
            t+=Time.deltaTime;
            gun.UpdateReloadBar(t, consumeTime);
            reloadBar.color = new Color(0, 255, 0, 255);
            reloadBar.enabled = true;
        }
        

        float heal = maxHealth*0.05f;

        GameObject shopkeep = GameObject.Find("Cool Broccoli");
        if(shopkeep != null)
        {
            shopkeep.GetComponent<Animator>().SetTrigger("mad");
            DataManager.Instance.cost++;
        }

        foreach (Item i in DataManager.Instance.playeritems)
        {
            if(i.itemname == "Vitamin Gummies")
            {
                heal += 3*i.stacks;
            }
            
        }
        PlayerHeal(heal, true);
        gun.ammo --;
        
        consuming = 0;
        reloadBar.enabled = false;
        gun.renderer.enabled = true;
    }
    public void useAbility()
    {
        
    }
    IEnumerator coolAbility()
    {
        foreach (Item i in DataManager.Instance.playeritems)
        {
            if(i.itemname == "Hand Sanitizer")
            {
                if(incombat == 1)
                {
                    PlayerHeal(5*i.stacks, true);
                }
                
            }
            
        }

        abilityready = false;
        if(ability == "Dash")
        {
            float s = moveSpeed;
            moveSpeed = s*5;
            for (int i = 0; i < 3; i++)
            {
                yield return new WaitForSeconds(0.025f);
                GameObject g = Instantiate(abilityprefab, transform.position, Quaternion.identity);
                if(renderer.flipX)
                {
                    g.GetComponent<SpriteRenderer>().flipX = true;
                }
                if(movement.y > 0)
                {
                    g.GetComponent<SpriteRenderer>().sortingOrder = 2;
                }
                else
                {
                    g.GetComponent<SpriteRenderer>().sortingOrder = 0;
                }
                
            }
            yield return new WaitForSeconds(0.025f);
            moveSpeed = s;
        }
        if(ability == "Steak Knife")
        {
            gun.renderer.enabled = false;
            gun.reloading = 0;
            //gun.shooting = 1;
            GameObject g = Instantiate(abilityprefab, transform.position, gun.shootPoint.rotation);
            if(gun.angle > 0 ) {g.GetComponent<SpriteRenderer>().sortingOrder = 0;}
            else{g.GetComponent<SpriteRenderer>().sortingOrder = 2;}
            
            g.GetComponent<Melee>().player = this.gameObject;
            g.GetComponent<Melee>().StartCoroutine(g.GetComponent<Melee>().bulletDestroy(0.5f));
            yield return new WaitForSeconds(0.5f);
            gun.renderer.enabled = true;
            //gun.shooting = 0;
        }
        if(ability == "Enhancement Field")
        {
            gun.renderer.enabled = false;
            
            //gun.shooting = 1;
            GameObject g = Instantiate(abilityprefab, gun.shootPoint.position, gun.shootPoint.rotation);
            
            yield return new WaitForSeconds(0.5f);
            gun.renderer.enabled = true;
            //gun.shooting = 0;
        }
        if(ability == "Proximity Mines")
        {
            gun.renderer.enabled = false;
            
            //gun.shooting = 1;
            GameObject g = Instantiate(abilityprefab, transform.position, Quaternion.identity);
            
            yield return new WaitForSeconds(0.5f);
            gun.renderer.enabled = true;
            //gun.shooting = 0;
        }
        if(ability == "Healing Field")
        {
            if(incombat == 1)
            {
                gun.renderer.enabled = false;
            
                //gun.shooting = 1;
                GameObject g = Instantiate(abilityprefab, transform.position, Quaternion.identity);
            
                yield return new WaitForSeconds(0.5f);
                gun.renderer.enabled = true;
                //gun.shooting = 0;
            }
            
        }
        if(ability == "Sawblade")
        {
            gun.renderer.enabled = false;
            
            //gun.shooting = 1;
            GameObject g = Instantiate(abilityprefab, gun.shootPoint.position, gun.shootPoint.rotation);
            
            g.GetComponent<bullet>().StartCoroutine(g.GetComponent<bullet>().bulletDestroy(7f));
            yield return new WaitForSeconds(0.5f);
            gun.renderer.enabled = true;
            //gun.shooting = 0;
        }
        float t = 0;
        while (t<abilitycooldown)
        {
            
            yield return new WaitForSeconds(0);
            t+=Time.deltaTime;
            UpdateAbilityBar(t, abilitycooldown);
            
            abilityBar.enabled = true;
        }
        abilityready = true;

        
        
        
        abilityBar.enabled = false;
        yield break;
    }

    public void UpdateAbilityBar(float t, float x)
    {
        abilityBar.fillAmount = Mathf.Clamp(t / x, 0, 1f);
    }

    public void spoonUpdate()
    {
        foreach(GameObject s in spoonList)
        {
            Destroy(s);
        }
        spoonList.Clear();
        for(int i = 0; i < spoonnum; i++)
        {
            GameObject sp = Instantiate(spoon, transform, false);
            spoonList.Add(sp);
            sp.transform.RotateAround(transform.position, Vector3.forward, i*(360/spoonnum));
            sp.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    public void kitgunUpdate()
    {
        foreach(GameObject s in kitgunList)
        {
            Destroy(s);
        }
        kitgunList.Clear();
        for(int i = 0; i < kitgunnum; i++)
        {
            GameObject sp = Instantiate(kitgun, transform, false);
            kitgunList.Add(sp);
            sp.transform.RotateAround(transform.position, Vector3.forward, i*(360/kitgunnum));
            sp.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        
        if(other.gameObject.tag == "Door" && other.gameObject.GetComponent<door>().locked == 0)
        {
            if(other.gameObject.GetComponent<door>().elevator)
            {
                InteractText.text = "E: Descend";
            }
            else
            {
                InteractText.text = "E: Proceed";
            }
            
            InteractText.enabled = true;
            
        }
        else if(other.gameObject.tag == "Pickup")
        {
            if(DataManager.Instance.lastDoorType == 3)
            {
                InteractText.text = "E: Buy";
            }
            else
            {
                InteractText.text = "E: Take";
            }
            
            InteractText.enabled = true;
        }
        else if(other.gameObject.tag == "Reroller")
        {
            InteractText.text = "E: Reroll";
            InteractText.enabled = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        InteractText.enabled = false;
    }

}
