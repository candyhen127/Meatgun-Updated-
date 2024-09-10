using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class damagenum : MonoBehaviour
{

    public float dnum;
    public bool heal;
    public bool crit;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<TextMeshProUGUI>().text = ((int)(dnum)).ToString();
        if(heal)
        {
            gameObject.GetComponent<TextMeshProUGUI>().color = new Color(0, 1, 0, 1);
        }
        else if (crit)
        {
            gameObject.GetComponent<TextMeshProUGUI>().color = new Color32(243, 97, 255, 255);
        }
        else
        {
            gameObject.GetComponent<TextMeshProUGUI>().color = new Color(1, 10f/dnum, 10f/dnum, 1);
        }

        gameObject.GetComponent<TextMeshProUGUI>().fontSize = 9+(.1f*dnum);
        StartCoroutine(destro());
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + new Vector3(0, 0.3f*GameObject.Find("Canvas").GetComponent<RectTransform>().localScale.x, 0);
    }

    IEnumerator destro()
    {
        yield return new WaitForSecondsRealtime(0.25f);
        Destroy(gameObject);
    }
}
