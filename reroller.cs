using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class reroller : MonoBehaviour
{
    public TextMeshProUGUI CostText;
    public Camera cam;
    public List<Pickup> pickups;
    public int cost;
    public int cooldown;
    // Start is called before the first frame update
    void Start()
    {
        
        CostText.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        cooldown = 0;
    }

    // Update is called once per frame
    void Update()
    {
        cost = (int)((10f*DataManager.Instance.floor)+DataManager.Instance.cost);
        CostText.text = "$" + cost;
        if(DataManager.Instance.lastDoorType == 3 && CostText != null)
        {
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

    void OnTriggerStay2D(Collider2D other)
    {
        
        if(other.gameObject.tag == "Player" && DataManager.Instance.money >= cost)
        {
            
            if(Input.GetKey("e") && cooldown == 0)
            {
                DataManager.Instance.money -= cost;
                foreach(Pickup p in pickups)
                {
                    p.reRoll();
                    StartCoroutine(Cooldown());
                }
                
            }
            
        }
    }

    IEnumerator Cooldown()
    {
        cooldown = 1;
        yield return new WaitForSeconds(0.5f);
        cooldown = 0;
    }
}
