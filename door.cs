using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class door : MonoBehaviour
{
    public Animator a;
    public GameObject director;
    public fadeout f;
    public Player player;

    public int locked = 1;
    public bool elevator = false;
    public float type;
    public String direction;

    void Start()
    {
        if(elevator == false)
        {
        if(DataManager.Instance.room == 0 || direction == "Down" || DataManager.Instance.lastDoorType == 3)
        {
            type = UnityEngine.Random.Range(0, 3);
        }
        else
        {
            type = UnityEngine.Random.Range(0, 4);
        }

        if(DataManager.Instance.roomstogo == 0)
        {
            type = 4;
        }
        if(DataManager.Instance.lastDoorType == 4)
        {
            type = 5;
        }
        
        if(locked != 0){
        StartCoroutine(close());
        }
        else
        {
            locked = 1;
            if((DataManager.Instance.lastDoor == "Up" && direction == "Down") ||
            (DataManager.Instance.lastDoor == "Left" && direction == "Right") ||
            (DataManager.Instance.lastDoor == "Right" && direction == "Left") ||
            (DataManager.Instance.lastDoorType == 5))
            {
            
            }
            else
            {
                locked = 0;
            }
            
        }
        }
        else
        {
            type = 6;
            if(locked != 0)
            {
                StartCoroutine(close());
            }
        }
        
    }

    IEnumerator close()
    {
        
        locked = 2;
        yield return new WaitForSeconds(0.625f);
        locked = 1;
        if((DataManager.Instance.lastDoor == "Up" && direction == "Down") ||
        (DataManager.Instance.lastDoor == "Down" && direction == "Up") ||
        (DataManager.Instance.lastDoor == "Left" && direction == "Right") ||
        (DataManager.Instance.lastDoor == "Right" && direction == "Left") ||
        (direction == "Down" && DataManager.Instance.lastDoorType == 4) || 
        (DataManager.Instance.lastDoorType == 5) ||
        elevator == true)
        
        {
            locked = 3;
        }
        
    }

    void Update()
    {
        a.SetFloat("Type", type);
        a.SetInteger("Locked", locked);
        if(DataManager.Instance.lastDoorType != 3)
            {
                
            
        if(director.GetComponent<Director>().getWaves() == 0 && locked == 1)
        {
            locked = 0;
            if(DataManager.Instance.lastDoorType != 3)
            {
                f.getflash();
            }
            
        }
            }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        
        if(other.gameObject.tag == "Player" && locked == 0)
        {
            
            if(Input.GetKey("e"))
            {
                locked = -1;
                
                StartCoroutine(nextStage());
            }
            
        }
    }

    IEnumerator nextStage()
    {
        yield return new WaitForSeconds(1);
        
        f.Leave();
        if(type == 4 || type == 5)
        {
            DataManager.Instance.StartCoroutine(DataManager.Instance.AudioFade());
        }

        yield return new WaitForSeconds(1);

        DataManager.Instance.health = player.health;
        DataManager.Instance.lastDoor = direction;
        DataManager.Instance.lastDoorType = type;

        if(elevator == true)
        {
            yield return new WaitForSeconds(1);
            
            DataManager.Instance.floor*= 1.5f;
            DataManager.Instance.roomstogo = 8;
            if(DataManager.Instance.realfloor == 1)
            {
                DataManager.Instance.realfloor = 2;
                DataManager.Instance.totalrooms = 15;
                SceneManager.LoadScene("B2-0");
            }
            else if(DataManager.Instance.realfloor == 2)
            {
                DataManager.Instance.realfloor = 3;
                DataManager.Instance.totalrooms = 15;
                SceneManager.LoadScene("B3-0");
            }
            else if(DataManager.Instance.realfloor == 3)
            {
                DataManager.Instance.realfloor = 4;
                DataManager.Instance.totalrooms = 15;
                SceneManager.LoadScene("B4-0");
            }
            else if(DataManager.Instance.realfloor == 4)
            {
                DataManager.Instance.realfloor = 1;
                DataManager.Instance.totalrooms = 15;
                SceneManager.LoadScene("B1-0");
                if(MetaDataManager.Instance.DejaVu ==false)
                {
                    MetaDataManager.Instance.DejaVu = true;
                    MetaDataManager.Instance.SaveMeta();
                }
            }
            DataManager.Instance.resetRooms();
        }
        else{

        if(type == 3)
        {
            if(DataManager.Instance.realfloor == 1){SceneManager.LoadScene("B1-Shop");}
            else if(DataManager.Instance.realfloor == 2){SceneManager.LoadScene("B2-Shop");}
            else if(DataManager.Instance.realfloor == 3){SceneManager.LoadScene("B3-Shop");}
            else if(DataManager.Instance.realfloor == 4){SceneManager.LoadScene("B4-Shop");}
        }
        else if(type == 4)
        {
            DataManager.Instance.room++;
            if(DataManager.Instance.realfloor == 1){SceneManager.LoadScene("B1-Boss");}
            else if(DataManager.Instance.realfloor == 2){SceneManager.LoadScene("B2-Boss");}
            else if(DataManager.Instance.realfloor == 3){SceneManager.LoadScene("B3-Boss");}
            else if(DataManager.Instance.realfloor == 4){SceneManager.LoadScene("B4-Boss");}
        }
        else if(type == 5)
        {
            if(DataManager.Instance.realfloor == 1){SceneManager.LoadScene("B1-Elevator");}
            else if(DataManager.Instance.realfloor == 2){SceneManager.LoadScene("B2-Elevator");}
            else if(DataManager.Instance.realfloor == 3){SceneManager.LoadScene("B3-Elevator");}
            else if(DataManager.Instance.realfloor == 4){SceneManager.LoadScene("B4-Elevator");}
        }
        else
        {
            if(DataManager.Instance.rooms.Count == 0){DataManager.Instance.resetRooms();}

            int roomindex = UnityEngine.Random.Range(0, DataManager.Instance.rooms.Count);
            int room = DataManager.Instance.rooms[roomindex];
            DataManager.Instance.rooms.RemoveAt(roomindex);
            DataManager.Instance.room++;
            DataManager.Instance.roomstogo--;
            if(DataManager.Instance.realfloor == 1)
            {
                SceneManager.LoadScene(room+3);
            }
            else if(DataManager.Instance.realfloor == 2)
            {
                SceneManager.LoadScene(room+22);
            }
            else if(DataManager.Instance.realfloor == 3)
            {
                SceneManager.LoadScene(room+41);
            }
            else if(DataManager.Instance.realfloor == 4)
            {
                SceneManager.LoadScene(room+60);
            }
            
        }
        }
    }

    
}
