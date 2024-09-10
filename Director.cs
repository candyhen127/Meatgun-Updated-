using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Director : MonoBehaviour
{

    public List<GameObject> EnemyPrefabs;
    public GameObject player;
    public bool boss = false;

    public List<Transform> spwnlist = new List<Transform>();

    public GameObject spawnanim;
    public GameObject spawnanim2;
    public GameObject scythe;
    public int enemycount;
    public int waves;
    Coroutine cooldown;
    int c = 0;


    void Start()
    {
        if(DataManager.Instance.lastDoorType == 6 || DataManager.Instance.room == 0)
        {
            StartCoroutine(Floortitle());
        }
        if(waves > 0){
        
        
        StartCoroutine(Spawn());
        }
        
    }

    public int getWaves()
    {
        return waves;
    }

    // Update is called once per frame
    void Update()
    {
        
            if (enemycount < 1 && waves > 1)
            {
                waves --;
                StopCoroutine(cooldown);
                StartCoroutine(Spawn());
            
            }
            else if(enemycount < 1 && waves > -1)
            {
                waves --;
                if(cooldown == null) {return;}
                StopCoroutine(cooldown);
            }
        
        else
        {
            if (enemycount < 1 && waves > -1)
            {
                waves--;
            }
        }
        if(scythe != null)
        {
        if(waves < 1 && scythe.GetComponent<SpriteRenderer>() != null)
        {
            scythe.GetComponent<SpriteRenderer>().color = new Color32(0, 0, 0, 0);
        }
        }
    }

    IEnumerator Spawn()
    {

        if(c == 1)
        {
            StopCoroutine(cooldown);
        }
        if(waves > 1)
        {
            cooldown = StartCoroutine(Cooldown());
        }
        
        enemycount += spwnlist.Count;


        if(boss)
        {
            yield return new WaitForSeconds(4.4f);
        }
        else
        {
            yield return new WaitForSeconds(2);
        }
        

        List<GameObject> spawnList = new List<GameObject>();

        foreach(Transform t in spwnlist)
        {
            spawnList.Add(Instantiate(spawnanim, t.position, t.rotation));
        }
        

        foreach(GameObject b in spawnList)
        {
            
            Destroy(b, 1.063f);
        }

        yield return new WaitForSeconds(1.063f);

        List<GameObject> enemyList = new List<GameObject>();

        foreach(Transform t in spwnlist)
        {
            int index = 0;
            for(int x = 0; x < EnemyPrefabs.Count-1; x++)
            {
                if(UnityEngine.Random.Range(0, 100) < 20*(Mathf.Pow(1.1f, DataManager.Instance.difficulty)))
                {
                    index++;
                }
            }
            
            
                enemyList.Add(Instantiate(EnemyPrefabs[index], t.position, t.rotation));
            
            
        }
        

        foreach(GameObject x in enemyList)
        {
            x.GetComponent<EnemyMovement>().player = player;
            x.GetComponent<EnemyMovement>().director = this.gameObject;
            x.GetComponent<AIDestinationSetter>().target = player.GetComponent<Transform>();
            if(boss)
            {
                if(scythe != null)
                {
                    GameObject sc = Instantiate(scythe, x.transform.position, x.transform.rotation);
                
                if(x.GetComponent<Boss>().bossname == "Carolina Reaper")
                {
                    
                    x.GetComponent<Boss>().weapon1 = sc;
                    sc.GetComponent<scythe>().reaper = x.GetComponent<Boss>();
                    sc.GetComponent<scythe>().player = player.GetComponent<Player>();
                }
                if(x.GetComponent<Boss>().bossname == "24-Carrot")
                {
                    List<Transform> t = new List<Transform>();
                    for (int i = 0; i < 4; i++)
                    {
                        t.Add(sc.transform.GetChild(i));
                    }
                    
                    x.GetComponent<Boss>().teleportpoints = t;
                }
                if(x.GetComponent<Boss>().bossname == "Potato Battery")
                {
                    Destroy(sc);
                }
                if(x.GetComponent<Boss>().bossname == "Meat Substitute")
                {
                    List<Transform> t = new List<Transform>();
                    for (int i = 0; i < 8; i++)
                    {
                        t.Add(sc.transform.GetChild(i));
                    }
                    
                    x.GetComponent<Boss>().teleportpoints = t;
                }
                }
                
                
            }
        }
        
        if(!boss)
        {
        List<GameObject> spawnList2 = new List<GameObject>();

        foreach(Transform t in spwnlist)
        {
            spawnList2.Add(Instantiate(spawnanim2, t.position, t.rotation));
        }
        
        foreach(GameObject x in spawnList2)
        {
            
            Destroy(x, 0.313f);
        }
        }
        else
        {
            fadeout f = GameObject.Find("Fadeout").GetComponent<fadeout>();
            f.StartCoroutine(f.flash());
            GameObject bossname = GameObject.Find("BossName");
            
            bossname.GetComponent<CanvasGroup>().alpha = 1;
            yield return new WaitForSeconds(2);
            for(float i = 1f; i > 0; i -= 0.2f)
            {
                yield return new WaitForSeconds(0.1f);
                bossname.GetComponent<CanvasGroup>().alpha = i;
            }
            bossname.GetComponent<CanvasGroup>().alpha = 0;
            bossname.SetActive(false);
        }
        
    }

    IEnumerator Cooldown()
    {
        c = 1;
        yield return new WaitForSeconds(35);
        waves --;
        c = 0;
        StartCoroutine(Spawn());
        
    }

    public void death()
    {
        enemycount--;
    }

    IEnumerator Floortitle()
    {
        GameObject floorname = GameObject.Find("FloorName");
            if(floorname != null)
            {
            floorname.GetComponent<CanvasGroup>().alpha = 1;
            yield return new WaitForSeconds(2);
            for(float i = 1f; i > 0; i -= 0.2f)
            {
                yield return new WaitForSeconds(0.1f);
                floorname.GetComponent<CanvasGroup>().alpha = i;
            }
            floorname.GetComponent<CanvasGroup>().alpha = 0;
            floorname.SetActive(false);
            }
    }
}
