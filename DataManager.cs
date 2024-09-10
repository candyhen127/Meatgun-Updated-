using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    public Texture2D crosshair;
    public float health;
    public int money;
    public float totalmoney;
    public int corescollected;
    public int revivecost;
    public float cardbonus;
    public float luck = 100;
    public float time;
    public int room;
    public int roomstogo;
    public int cost;
    public int difficulty;
    public float floor;
    public float realfloor;
    public int startingitems;
    public String lastDoor;
    public float lastDoorType;
    public bool paused;
    public bool gameover = false;
    public List<int> rooms = new List<int>{0, 1, 2};
    public int totalrooms;
    public List<Item> allitems = new List<Item>();
    public List<Item> dmgitems = new List<Item>();
    public List<Item> healitems = new List<Item>();
    public List<Item> utilitems = new List<Item>();
    public List<Item> bossitems = new List<Item>();
    public Item emptybottle;
    public List<Item> playeritems = new List<Item>();
    public List<GameObject> allenemies = new List<GameObject>();
    
    public AudioSource aud;
    public AudioClip main;
    public AudioClip boss;
    public AudioClip gameovertrack;
    public AudioClip main2;
    public AudioClip boss2;
    public AudioClip main3;
    public AudioClip boss3;
    public AudioClip main4;
    public AudioClip boss4;
    public bool audiopausechange;
    

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        setMusic();
    }

    void setMusic()
    {
        audiopausechange = true;
        if(SceneManager.GetActiveScene().name == "B1-Boss" && aud.clip != boss)
        {
            aud.clip = boss;
            aud.Play();
        }
        else if (SceneManager.GetActiveScene().name == "B2-Boss" && aud.clip != boss2)
        {
            aud.clip = boss2;
            aud.Play();
        }
        else if (SceneManager.GetActiveScene().name == "B3-Boss" && aud.clip != boss3)
        {
            aud.clip = boss3;
            aud.Play();
        }
        else if (SceneManager.GetActiveScene().name == "B4-Boss" && aud.clip != boss4)
        {
            aud.clip = boss4;
            aud.Play();
        }

        else if (realfloor == 1 && aud.clip != main)
        {
            aud.clip = main;
            aud.Play();
        }
        else if (realfloor == 2 && aud.clip != main2)
        {
            aud.clip = main2;
            aud.Play();
        }
        else if (realfloor == 3 && aud.clip != main3)
        {
            aud.clip = main3;
            aud.Play();
        }
        else if (realfloor == 4 && aud.clip != main4)
        {
            aud.clip = main4;
            aud.Play();
        }
        if(lastDoorType == 5)
        {
            aud.volume = 0f;
            audiopausechange = false;
        }
        else
        {
            aud.volume = MetaDataManager.Instance.musicvolume;
        } 
    }

    void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0 ||SceneManager.GetActiveScene().buildIndex == 1)
        {
            Destroy(gameObject);
            return;
        }
        time += Time.deltaTime;
        if(MetaDataManager.Instance.TimeFlies == false && time >= 1800)
        {
            MetaDataManager.Instance.TimeFlies = true;
            MetaDataManager.Instance.SaveMeta();
        }
        if(MetaDataManager.Instance.OnePercenter == false && money >= 500)
        {
            MetaDataManager.Instance.OnePercenter = true;
            MetaDataManager.Instance.SaveMeta();
        }
        difficulty = (int)(time/300f);

        if(gameover && aud.clip != gameovertrack)
        {
            audiopausechange = false;
            aud.clip = gameovertrack; 
            aud.volume = MetaDataManager.Instance.musicvolume*0.75f;
            
            aud.Play();
        }
        else if(audiopausechange)
        {
            if(paused)
            {
                aud.volume = MetaDataManager.Instance.musicvolume*0.125f;
            }
            else
            {
                aud.volume = MetaDataManager.Instance.musicvolume;
            }
        }

        if(aud.clip == gameovertrack && !gameover)
        {
            setMusic();
        }
        
    }

    private void Awake()
    {
        
        if(Instance != null )
        {
            Destroy(gameObject);
            return;
        }

        Vector2 cursorOffset = new Vector2(crosshair.width/2, crosshair.height/2);
        Cursor.SetCursor(crosshair, cursorOffset, CursorMode.Auto);
        
        foreach(Item i in allitems)
        {
            i.stacks = 0;
            if(i.type == "Damage")
            {
                dmgitems.Add(i);
            }
            if(i.type == "Healing")
            {
                healitems.Add(i);
            }
            if(i.type == "Utility")
            {
                utilitems.Add(i);
            }
            //playeritems.Add(i);
            //MetaDataManager.Instance.founditems.Add(i.name);
        }
        foreach(Item i in bossitems)
        {
            i.stacks = 0;
        }
        emptybottle.stacks = 0;
        //totalrooms = 15;
        
        room = 0;
        health = 100;
        if(OptionsManager.Instance != null)
        {
            health = OptionsManager.Instance.p.maxHealth;
        }
        
        roomstogo = 8;
        money = 0;
        corescollected = 0;
        revivecost = 2;
        time = 0;
        //floor = 1;
        //realfloor = 1;
        cost = 0;
        
        for(int i = 0; i < startingitems; i++)
        {
            addItem(allitems[UnityEngine.Random.Range(0, allitems.Count)]);
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
    }

    public void addItem(Item i)
    {
        i.stacks++;
        i.found = true;
                if(playeritems.Contains(i) == false)
                {
                    playeritems.Add(i);
                }
                if(MetaDataManager.Instance.founditems.Contains(i.itemname) == false)
                {
                    MetaDataManager.Instance.founditems.Add(i.itemname);
                    MetaDataManager.Instance.SaveMeta();
                }
                if(MetaDataManager.Instance.Obsession == false && i.stacks == 5)
                {
                    MetaDataManager.Instance.Obsession = true;
                    MetaDataManager.Instance.SaveMeta();
                }
                if(MetaDataManager.Instance.Hoarder == false)
                {
                    int t = 0;
                    foreach (String s in MetaDataManager.Instance.founditems)
                    {
                        foreach (Item x in allitems)
                        {
                            if(x.itemname == s)
                            {
                                t++;
                            }
                        }
                    }
                    if(t == allitems.Count)
                    {
                        MetaDataManager.Instance.Hoarder = true;
                        MetaDataManager.Instance.SaveMeta();
                    }
                }
    }
    public void resetRooms()
    {
        rooms.Clear();
        for(int i = 0; i < totalrooms; i++)
        {
            rooms.Add(i);
        
        }
        //rooms.Remove(SceneManager.GetActiveScene().buildIndex-3);
    }

    public IEnumerator AudioFade()
    {
        audiopausechange = false;
        for(float i = 1; i>=0; i-=0.1f)
        {
            yield return new WaitForSecondsRealtime(0.1f);
            aud.volume -= MetaDataManager.Instance.musicvolume*0.25f;
        }
        
    }
}
