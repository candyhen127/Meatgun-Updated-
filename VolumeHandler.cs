using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeHandler : MonoBehaviour
{
    public AudioSource a;
    public bool music;
    public float coeff;
    public bool handler;
    [SerializeField] private GameObject volumeslider;
    [SerializeField] private GameObject sfxslider;
    // Start is called before the first frame update
    void Start()
    {
        if(!handler)
        {
            a = gameObject.GetComponent<AudioSource>();
            if(music)
            {
                a.volume = MetaDataManager.Instance.musicvolume;
            }
            else
            {
                a.volume = MetaDataManager.Instance.sfxvolume*coeff;
            }
        }
        else
        {
            volumeslider.GetComponent<Slider>().value = MetaDataManager.Instance.musicvolume*10;
            sfxslider.GetComponent<Slider>().value = MetaDataManager.Instance.sfxvolume*10;
            
        }
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if(music)
        {
            
        }
        if(handler)
        {
            MetaDataManager.Instance.musicvolume = volumeslider.GetComponent<Slider>().value/10;
            MetaDataManager.Instance.sfxvolume = sfxslider.GetComponent<Slider>().value/10;
            GameObject.Find("Menu Music").GetComponent<AudioSource>().volume = MetaDataManager.Instance.musicvolume;
        }
    }
}
