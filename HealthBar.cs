using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    public Image healthBarImage;
    public Player player;
    public Boss boss;


    public void UpdateHealthBar()
    {
        
        if(player != null)
        {
            if(player.health>player.maxHealth) 
            {
                player.health = player.maxHealth;
            }
            healthBarImage.fillAmount = Mathf.Clamp(player.health / player.maxHealth, 0, 1f);
        }
        else
        {
            //float health = boss.gethealth();
            //float maxHealth = boss.getmaxHealth();
            if(boss.health>boss.maxHealth) 
            {
                boss.health = boss.maxHealth;
            }
            healthBarImage.fillAmount = Mathf.Clamp(boss.health / boss.maxHealth, 0, 1f);
        }
        
        
    }
}
