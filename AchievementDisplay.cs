using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class AchievementDisplay : MonoBehaviour
{
    public Sprite image;
    public Sprite lockedimage;
    public String  achievementname;
    public String description;
    public bool unlocked;

    public TextMeshProUGUI nametext;
    public TextMeshProUGUI desctext;
    public Image i;

    

    // Start is called before the first frame update
    void Start()
    {
        if(( achievementname == "Harvest Season" && MetaDataManager.Instance.HarvestSeason) ||
        ( achievementname == "The Root Of All Evil" && MetaDataManager.Instance.RootOfAllEvil) ||
        ( achievementname == "Green Energy" && MetaDataManager.Instance.GreenEnergy) ||
            ( achievementname == "Doppleganger" && MetaDataManager.Instance.DoppelGanger) ||
        ( achievementname == "Hoarder" && MetaDataManager.Instance.Hoarder) ||
        ( achievementname == "Botanist" && MetaDataManager.Instance.Botanist) ||
        ( achievementname == "Skill Issue" && MetaDataManager.Instance.SkillIssue) ||
        ( achievementname == "I Love The Economy" && MetaDataManager.Instance.ILoveTheEconomy) ||
        ( achievementname == "Heavy Hitter" && MetaDataManager.Instance.HeavyHitter) ||
        ( achievementname == "One Percenter" && MetaDataManager.Instance.OnePercenter) ||
        ( achievementname == "Obsession" && MetaDataManager.Instance.Obsession) ||
        ( achievementname == "Time Flies" && MetaDataManager.Instance.TimeFlies) ||
        ( achievementname == "Skin Of Your Teeth" && MetaDataManager.Instance.SkinOfYourTeeth) ||
        ( achievementname == "Punching Down" && MetaDataManager.Instance.PunchingDown) ||
        ( achievementname == "Read The Sign, Idiot" && MetaDataManager.Instance.ReadTheSignIdiot) ||
        ( achievementname == "Deja Vu" && MetaDataManager.Instance.DejaVu) ||
        
        ( achievementname == "Doppelganger" && MetaDataManager.Instance.DoppelGanger)||
        ( achievementname == "Rally The Troops" && MetaDataManager.Instance.RallyTheTroops)||
        ( achievementname == "Right To Bear Arms" && MetaDataManager.Instance.RightToBearArms)||
        ( achievementname == "Genocide" && MetaDataManager.Instance.Genocide)||
        ( achievementname == "Hoplophobia" && MetaDataManager.Instance.Hoplophobia)
        )
        {
            unlocked = true;
        }
        if(unlocked)
        {
            i.sprite = image;
        }
        else
        {
            i.sprite = lockedimage;
        }
        nametext.text =  achievementname;
        desctext.text = description;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
