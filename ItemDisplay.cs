using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using TMPro;
using Pathfinding;

public class ItemDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public Item i;
    public Gun g;
    public PlayerStats p;
    public GameObject e;
    public bool efound;
    public String edescription;
    public GameObject desc;

    public bool logbook;

    
    
    void Start()
    {
        
        if(i != null)
        {
            
            gameObject.GetComponent<Image>().sprite = i.sprite;
            if(DataManager.Instance != null)
            {
                TextMeshProUGUI t = gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
                if(i.stacks < 2)
                {
                    t.enabled = false;
                }
                else
                {
                    t.text = "x" + i.stacks;
                    t.enabled = true;
                }
            }
            if(MetaDataManager.Instance != null)
            {
                i.found = false;
                foreach(string x in MetaDataManager.Instance.founditems)
                {
                    if(x == i.itemname)
                    {
                        i.found = true;
                    }
                }
            }
            if(!i.found)
            {
                gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 1);
            }
        
        }
        if(g != null)
        {
            if((g.gunname == "CHICKEN")
            ||(g.gunname == "STEAK" && MetaDataManager.Instance.SteakUnlocked == true)
            ||(g.gunname == "SAUSAGE" && MetaDataManager.Instance.SausageUnlocked == true)
            ||(g.gunname == "HAM" && MetaDataManager.Instance.HamUnlocked == true)
            ||(g.gunname == "FISH" && MetaDataManager.Instance.FishUnlocked == true)
            ||(g.gunname == "BACON" && MetaDataManager.Instance.BaconUnlocked == true))
            {
                g.locked = false;
            }
            else
            {
                g.locked = true;
            }

            if(logbook)
            {
                if(g.locked)
                {
                    gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 1);
                }
            }
            
        }
        if(p != null)
        {
            
            if((p.troopername == "Assault Trooper")
            ||(p.troopername == "Heavy Trooper" && MetaDataManager.Instance.HTunlocked == true)
            ||(p.troopername == "Marksman" && MetaDataManager.Instance.MMunlocked == true)
            ||(p.troopername == "Demolitionist" && MetaDataManager.Instance.DMunlocked == true)
            ||(p.troopername == "Crusader" && MetaDataManager.Instance.CSunlocked == true)
            ||(p.troopername == "Metalsmith" && MetaDataManager.Instance.MSunlocked == true))
            {
                p.locked = false;
            }
            else
            {
                p.locked = true;
            }

            if(logbook)
            {
                if(p.locked)
                {
                    gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 1);
                }
            }
            
        }
        if(e != null)
        {
            gameObject.GetComponent<Image>().sprite = e.GetComponent<SpriteRenderer>().sprite;
            foreach(String s in MetaDataManager.Instance.foundenemies)
            {
                if(s == e.name + "(Clone)")
                {
                    efound = true;
                }
            }
            if(!efound)
            {
                gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 1);
            }
        }
        updateLock();
    }

    public void updateLock()
    {
        if(logbook){return;}
        if(g != null)
        {
            
            if(g.locked)
            {
                GetComponent<Image>().color = new Color32(200, 200, 200, 255);
            }
            else
            {
                GetComponent<Image>().color = new Color32(255, 0, 0, 255);
            }
        }
        if(p != null)
        {
            if(p.locked)
            {
                GetComponent<Image>().color = new Color32(200, 200, 200, 255);
            }
            else
            {
                GetComponent<Image>().color = new Color32(255, 0, 0, 255);
            }
        }
    }

    public void OnPointerEnter(PointerEventData d)
    {
        if(i != null)
        {
            if(!i.found)
            {
                return;
            }
            if(DataManager.Instance != null)
            {
                if(!DataManager.Instance.paused)
                {
                    foreach (Image i in GameObject.Find("Canvas").GetComponent<Menu>().invdisplay)
                    {
                        i.color = new Color32(255, 255, 255, 125);
                    }
                }
        
            }
            desc.SetActive(true);
        
        desc.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = i.sprite;
        desc.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = i.itemname + " (" + i.type + ")";
        desc.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = i.flavortext;
        desc.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>().text = i.description;
    
        if(i.type == "Damage")
        {
            desc.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().color = new Color32(255, 180, 0, 255);
        }
        else if(i.type == "Healing")
        {
            desc.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().color = new Color32(0, 255, 0, 255);
        }
        else if(i.type == "Utility")
        {
            desc.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().color = new Color32(190, 78, 255, 255);
            
        }
        else if(i.type == "Classified")
        {
            desc.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().color = new Color32(255, 0, 0, 255);
            
        }
        else
        {
            desc.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().color = new Color32(202, 202, 202, 255);
            desc.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = i.itemname;
        }
        }
        if(g != null)
        {
            if(logbook && g.locked){return;}
            desc.SetActive(true);
            desc.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = g.gunname+g.realgunname;
            if(!g.locked)
            {
                
                desc.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = 
                g.description +"\nDamage: "+g.damage+"\nFire Rate: "+g.fireRate+"\nProjectiles: "+ g.projectiles ;
                desc.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = 
                "\nAmmo: "+g.ammo+"\nReload Time: "+g.reloadTime;
                //desc.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>().text = 
                //"\""+g.description+"\"";
            }
            else
            {
                
                desc.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = 
                "Cost: "+ g.cost + " Cores";
                desc.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = 
                "";
            }
        }
        if(p != null)
        {
            if(logbook && p.locked){return;}
            desc.SetActive(true);
            
        
            desc.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "M.E. " + p.troopername;
            if(!p.locked)
            {
                desc.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = 
                "Max Health: "+p.maxHealth+"\nAbility: "+p.ability+" (Cooldown: "+ p.cooldown+ " seconds)\n"+ p.abilitydescription;
                desc.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = 
                "Move Speed: "+p.moveSpeed;
            }
            else
            {
                desc.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = 
                "Cost: "+ p.cost + " Cores";
                desc.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = 
                "";
            }
            
        }
        if(e != null)
        {
            if(!efound)
            {
                return;
            }
            
            desc.SetActive(true);
            
            desc.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = e.GetComponent<SpriteRenderer>().sprite;
            desc.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = e.name;
            desc.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = 
            edescription + "\nDamage: "+e.GetComponent<EnemyMovement>().gun.damage+"\nFire Rate: "+e.GetComponent<EnemyMovement>().gun.attackSpeed+"\nProjectiles: "+ e.GetComponent<EnemyMovement>().gun.projectiles;
            desc.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>().text = 
            "\nHealth: "+e.GetComponent<EnemyMovement>().maxHealth+"\nMove Speed: "+e.GetComponent<AIPath>().maxSpeed;
            
            
        }
    }

    public void OnPointerExit(PointerEventData d)
    {
        desc.SetActive(false);
        if(i!=null && DataManager.Instance != null)
        {
            foreach (Image i in GameObject.Find("Canvas").GetComponent<Menu>().invdisplay)
            {
                i.color = new Color32(255, 255, 255, 255);
            }
            
        }
        
    }

    
}
