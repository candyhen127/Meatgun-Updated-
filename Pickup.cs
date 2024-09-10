using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Pickup : MonoBehaviour
{   
    public Item item;
    public TextMeshProUGUI InteractText;
    public Player player;
    public GameObject description;
    public GameObject pickup2;

    public TextMeshProUGUI CostText;
    public Camera cam;
    public int cost = 0;

    // Start is called before the first frame update
    void Start()
    {
        if(CostText != null)
        {
            CostText.enabled = true;
        }
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<Collider2D>().enabled = false;
        description.SetActive(false);
        if(DataManager.Instance.lastDoorType == 0)
        {
            item = DataManager.Instance.dmgitems[UnityEngine.Random.Range(0, DataManager.Instance.dmgitems.Count)];
        }
        else if(DataManager.Instance.lastDoorType == 1)
        {
            item = DataManager.Instance.healitems[UnityEngine.Random.Range(0, DataManager.Instance.healitems.Count)];
        }
        else if(DataManager.Instance.lastDoorType == 2)
        {
            item = DataManager.Instance.utilitems[UnityEngine.Random.Range(0, DataManager.Instance.utilitems.Count)];
        }
        else if(DataManager.Instance.lastDoorType == 3)
        {
            item = DataManager.Instance.allitems[UnityEngine.Random.Range(0, DataManager.Instance.allitems.Count)];
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            gameObject.GetComponent<Collider2D>().enabled = true;
            
            CostText.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            if(DataManager.Instance.money >= cost)
            {
                CostText.color = new Color(1, 1, 1, 1);
            }
            else        
            {
                CostText.color = new Color(1, 0, 0, 1);
            }
        }
        
        
        gameObject.GetComponent<SpriteRenderer>().sprite = item.sprite;
        StartCoroutine(UpAndDown());
    }

    // Update is called once per frame
    void Update()
    {
        
        if(DataManager.Instance.lastDoorType == 3 && CostText != null)
        {
            cost = (int)((30*DataManager.Instance.floor) + DataManager.Instance.cost);
            CostText.text = "$" + cost;
            if(DataManager.Instance.money >= cost)
            {
                CostText.color = new Color(1, 1, 1, 1);
            }
            else        
            {
                CostText.color = new Color(1, 0, 0, 1);
            }
        }
    }

    public void reRoll()
    {
        item = DataManager.Instance.allitems[UnityEngine.Random.Range(0, DataManager.Instance.allitems.Count)];
        gameObject.GetComponent<SpriteRenderer>().sprite = item.sprite;
    }

    IEnumerator UpAndDown()
    {
        var pos = transform.position;
        for(float i = 0f; i<=0.5f; i+=0.1f)
            {
                transform.position = new Vector3(pos.x, pos.y+0.1f); 
                yield return new WaitForSeconds(0.1f);
            }
        for(float i = 1f; i>=0; i-=0.1f)
            {
                transform.position = new Vector3(pos.x, pos.y-0.1f); 
                yield return new WaitForSeconds(0.1f);
            }
        for(float i = 0f; i<=0.5f; i+=0.1f)
            {
                transform.position = new Vector3(pos.x, pos.y+0.1f); 
                yield return new WaitForSeconds(0.1f);
            }
            StartCoroutine(UpAndDown());
    }

    public void reveal()
    {
        this.gameObject.GetComponent<Collider2D>().enabled = true;
        this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                if(pickup2 != null)
                {
                    pickup2.GetComponent<SpriteRenderer>().enabled = true;
                    pickup2.GetComponent<Collider2D>().enabled = true;
                }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        
        if(other.gameObject.tag == "Player" && DataManager.Instance.money >= cost)
        {
            
            if(Input.GetKey("e"))
            {
                DataManager.Instance.money -= cost;
                DataManager.Instance.addItem(item);
                player.updateItems(false);
                InteractText.enabled = false;
                StartCoroutine(popUp(item));
                this.gameObject.GetComponent<Collider2D>().enabled = false;
                this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                if(pickup2 != null)
                {
                    pickup2.GetComponent<SpriteRenderer>().enabled = false;
                    pickup2.GetComponent<Collider2D>().enabled = false;
                }
                
                if(CostText != null)
                {
                    Destroy(CostText);
                }
                if(item.itemname == "T-Bone Steak")
                {
                    player.PlayerHeal(20, false);
                }
                if(item.itemname == "Life Core")
                {
                    player.PlayerHeal(.05f*player.baseHealth, false);
                    DataManager.Instance.corescollected ++;
                    MetaDataManager.Instance.cores ++;
                    GameObject.Find("Canvas").GetComponent<Menu>().UpdateCoreNumber();
                    MetaDataManager.Instance.SaveMeta();
                }
                
            }
            
        }
    }

    public void getPopUp(Item i)
    {
        StartCoroutine(popUp(i));
    }

    public IEnumerator popUp(Item i)
    {
        description.SetActive(true);
        description.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = i.sprite;
        description.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = i.itemname;
        description.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>().text = i.description;

        if(i.type == "Damage")
        {
            description.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().color = new Color32(255, 180, 0, 255);
        }
        else if(i.type == "Healing")
        {
            description.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().color = new Color32(0, 255, 0, 255);
        }
        else if(i.type == "Utility")
        {
            description.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().color = new Color32(190, 78, 255, 255);
            
        }
        else if(i.type == "Classified")
        {
            description.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().color = new Color32(255, 0, 0, 255);
        }
        else
        {
            description.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().color = new Color32(202, 202, 202, 255);
        }

        yield return new WaitForSeconds(3);

        for(float x = 1; x>=0; x-=Time.deltaTime*2)
            {
                description.GetComponent<CanvasGroup>().alpha = x;
                yield return null;
            }

        description.SetActive(false);
    }
}
