using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Money : MonoBehaviour
{
    
    // Update is called once per frame
    void Update()
    {
        this.gameObject.GetComponent<TextMeshProUGUI>().text = "$" + DataManager.Instance.money.ToString();
    }
}
