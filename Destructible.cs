using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class Destructible : MonoBehaviour
{
    public float health;
    public TextMeshProUGUI damagenum;
    public List<GameObject> otherdestroy;

    public bool generator;
    public int power;
    public float maxHealth;
    public GameObject spotlight;
    public GameObject rotatepoint;
    public GameObject laser;
    public Transform shootPoint;

    public bool regenerative;
    public float regentime;
    // Start is called before the first frame update
    void Start()
    {
        if(generator)
        {
            StartCoroutine(timer());
        }   
    }

    // Update is called once per frame
    void Update()
    {
        
        if(generator)
        {
            GetComponent<Animator>().SetFloat("Health", health);
            if(power == 1)
            {
                spotlight.GetComponent<SpriteRenderer>().color = new Color32(0, 0, 0, 0);
            }
            else if(power == 0)
            {
                spotlight.GetComponent<SpriteRenderer>().color = new Color32(0, 0, 0, 125);
            }
            if(GameObject.Find("Director").GetComponent<Director>().waves == -1)
            {
                power = 1;
                health = 0;
                return;
            }
        }
        if(regenerative)
        {
            if(GameObject.Find("Director").GetComponent<Director>().waves == -1)
            {
                GetComponent<Collider2D>().enabled = false;
                
                if(health != -1)
                {
                    GetComponent<Animator>().Play("CeleryHDown");
                }
                
                health = -1;
                return;
            }
        }
        if(rotatepoint != null)
        {
            transform.RotateAround(rotatepoint.transform.position, Vector3.forward, 15*Time.deltaTime);
        }
        if(health<1)
        {
            if(generator)
            {
                health = maxHealth;
                if(power == 1)
                {
                    power = 0;
                }
                else if(power == 0)
                {
                    power = 1;
                }
            }
            else if(regenerative)
            {
                GetComponent<Collider2D>().enabled = false;
                health = maxHealth;
                GetComponent<Animator>().Play("CeleryHDown");
                StartCoroutine(regentimer());
            }
            else
            {
                foreach (GameObject item in otherdestroy)
                {
                    Destroy(item);
                }
                Destroy(gameObject);
            }
        }
    }
    public void TakeDamage(float damage)
    {
        TextMeshProUGUI x = Instantiate(damagenum, GameObject.Find("Canvas").transform, false);
        x.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        x.gameObject.GetComponent<damagenum>().dnum = damage;
        health -= damage;
    }

    IEnumerator timer()
    {
        while(GameObject.Find("Director").GetComponent<Director>().waves > 0)
        {
            yield return new WaitForSeconds(1);
            health -= maxHealth/5;
        }
    }

    IEnumerator regentimer()
    {
        yield return new WaitForSeconds(regentime);
        health = maxHealth;
        GetComponent<Collider2D>().enabled = true;
        GetComponent<Animator>().Play("CeleryHUp");
    }

    public void ShootLaser()
    {
        GameObject bullet = Instantiate(laser, shootPoint.position, shootPoint.rotation);
            bullet.GetComponent<Elecwall>().damage = rotatepoint.GetComponent<Boss>().gun.damage/(1*DataManager.Instance.floor);
            bullet.GetComponent<Elecwall>().power = 1;
            bullet.GetComponent<Elecwall>().parent = shootPoint.gameObject;
            Destroy(bullet, 0.75f);
    }
}
