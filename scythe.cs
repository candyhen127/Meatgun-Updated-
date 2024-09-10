using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class scythe : MonoBehaviour
{
    public Sprite idle;
    public Sprite spin;
    public float damage;
    public Boss reaper;
    public Player player;
    public int mode;

    public Coroutine throwroutine;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.eulerAngles = new Vector3(0, 0, -45);
        GetComponent<SpriteRenderer>().sprite = idle;
        //StartCoroutine(throwScythe());
    }

    // Update is called once per frame
    void Update()
    {
        if(reaper == null)
        {
            Destroy(gameObject);
        }
        if(GetComponent<SpriteRenderer>().sprite == spin)
        {
            transform.Rotate(0, 0, -50*Time.timeScale);
            
            GetComponent<SpriteRenderer>().sortingOrder = 2;
            GetComponent<AudioSource>().volume = 0.3f;
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, -45);
            
            GetComponent<SpriteRenderer>().sortingOrder = 0;
            GetComponent<AudioSource>().volume = 0f;
        }
        if(mode == 0)
        {
            gameObject.transform.position = reaper.gameObject.transform.position;
            GetComponent<AIPath>().canMove = false;
            GetComponent<Collider2D>().enabled = false;
        }
        else
        {
            GetComponent<AIPath>().canMove = true;
            if(mode == 1)
            {
                GetComponent<Collider2D>().enabled = true;
                GetComponent<AIPath>().maxSpeed = 25;
                GetComponent<AIDestinationSetter>().target = player.gameObject.transform;
            }
            if(mode == 2)
            {
                GetComponent<AIPath>().maxSpeed = 100;
                if(reaper.phase == 2)
                {
                    GetComponent<AIPath>().canMove = false;
                }
                GetComponent<AIDestinationSetter>().target = reaper.gameObject.transform;
                GetComponent<Collider2D>().enabled = false;
            }
        }
        
    }

    public IEnumerator throwScythe()
    {
        reaper.mode = 1;
        GetComponent<SpriteRenderer>().sprite = spin;
        yield return new WaitForSeconds(1);
        
        mode = 1;
        yield return new WaitForSeconds(2);
        reaper.mode = 0;
        yield return new WaitForSeconds(8);
        StartCoroutine(catchScythe());
    }
    public IEnumerator catchScythe()
    {
        StopCoroutine(throwroutine);
        
        mode = 2;
        if(reaper.phase == 1)
        {
            GetComponent<SpriteRenderer>().sprite = idle;
        }
        else
        {
            reaper.StartCoroutine(reaper.teleport());
            GetComponent<Collider2D>().enabled = false;
        }
        mode = 2;
        reaper.bossanim.ResetTrigger("weapon1");
        reaper.StartCoroutine(reaper.cool(1));
        while(Vector3.Distance(this.transform.position, reaper.transform.position) > 10f)
        {
            yield return new WaitForSeconds(0.02f);
        }
        
        mode = 0;
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        
        
            if(collision.gameObject.tag == "Player")
            {
                collision.gameObject.GetComponent<Player>().TakeDamage(damage);
                StartCoroutine(catchScythe());
            }
            
        
    }
}
