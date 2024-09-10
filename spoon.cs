using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class spoon : MonoBehaviour
{

    public GameObject player;
    public Menu canvas;
    public TextMeshProUGUI damagenum;
    public float health;
    public float maxHealth;
    public float delay;

    // Start is called before the first frame update
    void Start()
    {
        health = 10;
        delay = 8;
        player = GameObject.Find("Player");
        canvas = GameObject.Find("Canvas").GetComponent<Menu>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(player.transform.position, Vector3.forward, 45*Time.deltaTime);
        transform.rotation = Quaternion.identity;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        TextMeshProUGUI x = Instantiate(damagenum, canvas.transform, false);
        x.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        x.gameObject.GetComponent<damagenum>().dnum = damage;
        if(health < 1)
        {
            StartCoroutine(cooldown());
        }
    }

    public IEnumerator cooldown()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(delay);
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
        health = maxHealth;
    }
}
