using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMusic : MonoBehaviour
{
    public static MenuMusic Instance;
    public AudioSource aud;
    
    private void Awake()
    {
        aud = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
        
        if(Instance == null)
        {
            Instance = this;
            
        }
        else
        {
            Destroy(this.gameObject);
        }
        
        
    }

    void Update()
    {
        if(SceneManager.GetActiveScene().name != "Main Menu" && SceneManager.GetActiveScene().name != "Select Screen")
        {
            Destroy(this.gameObject);
        }
    }


}
