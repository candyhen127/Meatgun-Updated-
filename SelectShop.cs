using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectShop : MonoBehaviour
{
    [SerializeField] private GameObject buyMenu;
    [SerializeField] private GameObject yesButton;
    public SpriteRenderer playerdisplay;
    public SpriteRenderer gundisplay;
    public Menu canvas;
    public ItemDisplay temp;
    // Start is called before the first frame update
    void Start()
    {
        canvas.UpdateCoreNumber();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void buttonClick(ItemDisplay i)
    {
        if(i.g != null)
        {
            if(!i.g.locked)
            {
                setGun(i.g);
            }
            else
            {
                getBuy(i);
            }
        }
        if(i.p != null)
        {
            if(!i.p.locked)
            {
                setPlayer(i.p);
            }
            else
            {
                getBuy(i);
            }
        }
    }

    public void getBuy(ItemDisplay i)
    {
        temp = i;
        buyMenu.SetActive(true);
        if(i.g != null)
        {
            buyMenu.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "Buy "+ i.g.gunname+"?";
            buyMenu.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = "Cost: "+ i.g.cost + " Cores";
            if(i.g.cost > MetaDataManager.Instance.cores)
            {
                yesButton.GetComponent<Button>().interactable = false;
            }
            else
            {
                yesButton.GetComponent<Button>().interactable = true;
            }
        }
        if(i.p != null)
        {
            buyMenu.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "Buy "+ i.p.troopername+"?";
            buyMenu.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = "Cost: "+ i.p.cost + " Cores";
            if(i.p.cost > MetaDataManager.Instance.cores)
            {
                yesButton.GetComponent<Button>().interactable = false;
            }
            else
            {
                yesButton.GetComponent<Button>().interactable = true;
            }
        }
    }

    public void cancelBuy()
    {
        buyMenu.SetActive(false);
    }

    public void confirmBuy()
    {
        if(temp.g != null)
        {
            temp.g.locked = false;
            MetaDataManager.Instance.cores -= temp.g.cost;
            if(temp.g.gunname == "STEAK")
            {
                MetaDataManager.Instance.SteakUnlocked = true;
            }
            if(temp.g.gunname == "SAUSAGE")
            {
                MetaDataManager.Instance.SausageUnlocked = true;
            }
            if(temp.g.gunname == "HAM")
            {
                MetaDataManager.Instance.HamUnlocked = true;
            }
            if(temp.g.gunname == "FISH")
            {
                MetaDataManager.Instance.FishUnlocked = true;
            }
            if(temp.g.gunname == "BACON")
            {
                MetaDataManager.Instance.BaconUnlocked = true;
            }
            
        }
        if(temp.p != null)
        {
            temp.p.locked = false;
            MetaDataManager.Instance.cores -= temp.p.cost;
            if(temp.p.troopername == "Heavy Trooper")
            {
                MetaDataManager.Instance.HTunlocked = true;
            }
            if(temp.p.troopername == "Marksman")
            {
                MetaDataManager.Instance.MMunlocked = true;
            }
            if(temp.p.troopername == "Demolitionist")
            {
                MetaDataManager.Instance.DMunlocked = true;
            }
            if(temp.p.troopername == "Crusader")
            {
                MetaDataManager.Instance.CSunlocked = true;
            }
            if(temp.p.troopername == "Metalsmith")
            {
                MetaDataManager.Instance.MSunlocked = true;
            }
            
        }
        canvas.UpdateCoreNumber();
        temp.updateLock();

        if(MetaDataManager.Instance.ILoveTheEconomy == false)
        {
            MetaDataManager.Instance.ILoveTheEconomy = true;
        }

        if(MetaDataManager.Instance.HTunlocked &&
        MetaDataManager.Instance.MMunlocked &&
        MetaDataManager.Instance.DMunlocked &&
        MetaDataManager.Instance.CSunlocked &&
        MetaDataManager.Instance.MSunlocked)
        {
            if(MetaDataManager.Instance.RallyTheTroops == false)
            {
                MetaDataManager.Instance.RallyTheTroops = true;
            }
        }

        if(MetaDataManager.Instance.SteakUnlocked &&
        MetaDataManager.Instance.SausageUnlocked &&
        MetaDataManager.Instance.HamUnlocked &&
        MetaDataManager.Instance.FishUnlocked &&
        MetaDataManager.Instance.BaconUnlocked)
        {
            if(MetaDataManager.Instance.RightToBearArms == false)
            {
                MetaDataManager.Instance.RightToBearArms = true;
            }
        }

        MetaDataManager.Instance.SaveMeta();
        buyMenu.SetActive(false);
    }

    public void setGun(Gun g)
    {
        OptionsManager.Instance.setGun(g);
        gundisplay.sprite = g.displaysprite;
    }

    public void setPlayer(PlayerStats player)
    {
        OptionsManager.Instance.setPlayer(player);
        playerdisplay.sprite = player.displaysprite;
    }
}
