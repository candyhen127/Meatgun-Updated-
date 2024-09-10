using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fadeout : MonoBehaviour
{

    public GameObject syn;
    public GameObject syn2;
    public GameObject box;
    public GameObject box2;
    public GameObject pickup;

    // Start is called before the first frame update
    void Start()
    {
        Enter();
    }

    public void Enter()
    {
        this.gameObject.GetComponent<Image>().enabled = true;
        StartCoroutine(FadeImage(true));
    }
    public void Leave()
    {
        this.gameObject.GetComponent<Image>().enabled = true;
        StartCoroutine(FadeImage(false));
    }

    public void Restart()
    {
        this.gameObject.GetComponent<Image>().enabled = true;
        StartCoroutine(FadeImage2());
    }

    public void getflash()
    {
        this.gameObject.GetComponent<Image>().enabled = true;
        StartCoroutine(flash());
        StartCoroutine(flash2());
    }

    public void get1flash()
    {
        this.gameObject.GetComponent<Image>().enabled = true;
        StartCoroutine(flash());
    }
    public void getheal()
    {
        this.gameObject.GetComponent<Image>().enabled = true;
        StartCoroutine(heal());
    }
    public void getdamage()
    {
        this.gameObject.GetComponent<Image>().enabled = true;
        StartCoroutine(damage());
    }

    public IEnumerator damage()
    {
        
        
        this.gameObject.GetComponent<Image>().color = new Color(255, 0, 0, 180);
        for(float i = 0.5f; i>=0; i-=Time.unscaledDeltaTime*2)
            {
                this.gameObject.GetComponent<Image>().color = new Color(255, 0, 0, i);
                yield return null;
            }
            this.gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);

            this.gameObject.GetComponent<Image>().enabled = false;
    }

    public IEnumerator heal()
    {
        

        this.gameObject.GetComponent<Image>().color = new Color(0, 255, 0, 180);
        for(float i = 0.5f; i>=0; i-=Time.unscaledDeltaTime*2)
            {
                this.gameObject.GetComponent<Image>().color = new Color(0, 255, 0, i);
                yield return null;
            }
            this.gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);

            this.gameObject.GetComponent<Image>().enabled = false;
    }

    public IEnumerator flash()
    {
        this.gameObject.GetComponent<Image>().enabled = true;

        this.gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 255);
        

        for(float i = 0.5f; i>=0; i-=Time.unscaledDeltaTime*2)
            {
                this.gameObject.GetComponent<Image>().color = new Color(255, 255, 255, i);
                yield return null;
            }
            this.gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);

            this.gameObject.GetComponent<Image>().enabled = false;
    }

    public IEnumerator flash2()
    {
        box.GetComponent<AudioSource>().volume = MetaDataManager.Instance.sfxvolume*0.25f;
        box.GetComponent<AudioSource>().Play();
        box.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
        if(box2!= null){box2.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);}
        pickup.GetComponent<Pickup>().reveal();
        syn.GetComponent<Animator>().SetInteger("On", 1);
        if(syn2!= null){syn2.GetComponent<Animator>().SetInteger("On", 1);}
        
        if(MetaDataManager.Instance.SkinOfYourTeeth == false)
        {
            Player p = GameObject.Find("Player").GetComponent<Player>();
            if(p.health <= p.maxHealth*0.25f)
            {
                MetaDataManager.Instance.SkinOfYourTeeth = true;
                MetaDataManager.Instance.SaveMeta();
            }
        }
        if(MetaDataManager.Instance.Hoplophobia == false)
        {
            Player p = GameObject.Find("Player").GetComponent<Player>();
            if(!p.shotsfired)
            {
                MetaDataManager.Instance.Hoplophobia = true;
                MetaDataManager.Instance.SaveMeta();
            }
        }
        

        yield return new WaitForSeconds(0.25f);
        for(float i = 0.5f; i>=0; i-=Time.unscaledDeltaTime*2)
            {
                box.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, i);
                if(box2!= null){box2.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, i);}
                yield return null;
            }
            box.gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
            if(box2!= null){box2.gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);}

            this.gameObject.GetComponent<Image>().enabled = false;
    }

    public IEnumerator FadeImage(bool b)
    {
        this.gameObject.GetComponent<Image>().enabled = true;
        if(b)
        {
            this.gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 1);
            for(float i = 1; i>=0; i-=Time.deltaTime*2)
            {
                this.gameObject.GetComponent<Image>().color = new Color(0, 0, 0, i);
                yield return null;
            }
            this.gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            this.gameObject.GetComponent<Image>().enabled = false;
            
        }
        else
        {
            this.gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            for(float i = 0; i<=1; i+=Time.deltaTime*2)
            {
                this.gameObject.GetComponent<Image>().color = new Color(0, 0, 0, i);
                yield return null;
            }
            this.gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 1);
        }
        
        
    }

    public IEnumerator FadeImage2()
    {
        this.gameObject.GetComponent<Image>().enabled = true;
        
        {
            this.gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            for(float i = 0; i<=1; i+=Time.unscaledDeltaTime*2)
            {
                this.gameObject.GetComponent<Image>().color = new Color(0, 0, 0, i);
                yield return null;
            }
            this.gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 1);
        }
        
        
    }
}
