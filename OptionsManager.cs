using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsManager : MonoBehaviour
{
    public static OptionsManager Instance;
    public Gun g;
    public PlayerStats p;
    

    private void Awake()
    {
        
        if(Instance != null )
        {
            Destroy(Instance.gameObject);
            //return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    
    
    public void setGun(Gun gun)
    {
        g = gun;
    }

     public void setPlayer(PlayerStats player)
    {
        p = player;
    }
}
