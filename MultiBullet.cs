using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiBullet : MonoBehaviour
{
    public float damage;
    public float bulletSpeed;
    public List<Enemybullet> bullets;
    public List<MultiBullet> multibullets;
    public List<bullet> playerbullets;
    public List<ProximityMine> mines;

    public float destroy;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Enemybullet e in bullets)
        {
            e.damage = damage;
            e.bulletSpeed = bulletSpeed;
            e.destroy = destroy;
        }
        foreach (MultiBullet e in multibullets)
        {
            e.damage = damage;
            e.bulletSpeed = bulletSpeed;
            e.destroy = destroy;
        }
        foreach (bullet e in playerbullets)
        {
            e.damage = damage;
            e.bulletSpeed = bulletSpeed;
        }
        foreach (ProximityMine m in mines)
        {
            m.damage = damage;
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < bullets.Count; i++)
        {
            if(bullets[i] == null){bullets.RemoveAt(i);}
        }
        for (int i = 0; i < multibullets.Count; i++)
        {
            if(multibullets[i] == null){multibullets.RemoveAt(i);}
        }
        for (int i = 0; i < playerbullets.Count; i++)
        {
            if(playerbullets[i] == null){playerbullets.RemoveAt(i);}
        }
        for (int i = 0; i < mines.Count; i++)
        {
            if(mines[i] == null){mines.RemoveAt(i);}
        }
        if(bullets.Count == 0 && multibullets.Count == 0 && playerbullets.Count == 0 && mines.Count == 0)
        {
            Destroy(gameObject);
        }
    }

    public void setvars(float d, float s)
    {
        damage = d;
        bulletSpeed = s;
    }
}
