using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Steamworks;


public class MetaDataManager : MonoBehaviour
{
    public static MetaDataManager Instance;
    public int cores;
    public int deaths;
    public int kills;

    public float musicvolume;
    public float sfxvolume;

    public List<String> founditems = new List<String>();
    public List<String> foundenemies = new List<String>();

    //Unlocks
    public bool HTunlocked;
    public bool MMunlocked;
    public bool DMunlocked;
    public bool CSunlocked;
    public bool MSunlocked;

    public bool SteakUnlocked;
    public bool SausageUnlocked;
    public bool HamUnlocked;
    public bool FishUnlocked;
    public bool BaconUnlocked;

    //Achievements
    public bool HarvestSeason;
    public bool RootOfAllEvil;
    public bool GreenEnergy;
        public bool DoppelGanger;
    public bool Hoarder;
    public bool Botanist;
        public bool RallyTheTroops;
        public bool RightToBearArms;
    public bool SkillIssue;
    public bool ILoveTheEconomy;
        public bool Genocide;
    public bool HeavyHitter;
    public bool OnePercenter;
        public bool Hoplophobia;
    public bool Obsession;
    public bool TimeFlies;
    public bool SkinOfYourTeeth;
    public bool PunchingDown;
    public bool ReadTheSignIdiot;
    public bool DejaVu; 

    

    private void Awake()
    {
        if(Instance != null )
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadMeta();

        SteamAPI.Init();
        if(SteamManager.Initialized) {
            string name = SteamFriends.GetPersonaName();
            Debug.Log(name);
        }
    }

    public void SaveMeta()
    {
        if(SteamManager.Initialized) {
        bool tutorialCompleted;
        Steamworks.SteamUserStats.GetAchievement("Harvest Season", out tutorialCompleted);
        if (tutorialCompleted == false && HarvestSeason == true)
        {
            Steamworks.SteamUserStats.SetAchievement("Harvest Season");
        }   
        Steamworks.SteamUserStats.GetAchievement("The Root Of All Evil", out tutorialCompleted);
        if (tutorialCompleted == false && RootOfAllEvil == true)
        {
            Steamworks.SteamUserStats.SetAchievement("The Root Of All Evil");
        }   
        Steamworks.SteamUserStats.GetAchievement("GreenEnergy", out tutorialCompleted);
        if (tutorialCompleted == false && GreenEnergy == true)
        {
            Steamworks.SteamUserStats.SetAchievement("GreenEnergy");
        }   
        Steamworks.SteamUserStats.GetAchievement("Hoarder", out tutorialCompleted);
        if (tutorialCompleted == false && Hoarder == true)
        {
            Steamworks.SteamUserStats.SetAchievement("Hoarder");
        }   
        Steamworks.SteamUserStats.GetAchievement("Botanist", out tutorialCompleted);
        if (tutorialCompleted == false && Botanist == true)
        {
            Steamworks.SteamUserStats.SetAchievement("Botanist");
        }      
        Steamworks.SteamUserStats.GetAchievement("SkillIssue", out tutorialCompleted);
        if (tutorialCompleted == false && SkillIssue == true)
        {
            Steamworks.SteamUserStats.SetAchievement("SkillIssue");
        }   
        Steamworks.SteamUserStats.GetAchievement("ILoveTheEconomy", out tutorialCompleted);
        if (tutorialCompleted == false && ILoveTheEconomy == true)
        {
            Steamworks.SteamUserStats.SetAchievement("ILoveTheEconomy");
        }   
        Steamworks.SteamUserStats.GetAchievement("HeavyHitter", out tutorialCompleted);
        if (tutorialCompleted == false && HeavyHitter == true)
        {
            Steamworks.SteamUserStats.SetAchievement("HeavyHitter");
        }   
        Steamworks.SteamUserStats.GetAchievement("OnePercenter", out tutorialCompleted);
        if (tutorialCompleted == false && OnePercenter == true)
        {
            Steamworks.SteamUserStats.SetAchievement("OnePercenter");
        }   
        Steamworks.SteamUserStats.GetAchievement("Obsession", out tutorialCompleted);
        if (tutorialCompleted == false && Obsession == true)
        {
            Steamworks.SteamUserStats.SetAchievement("Obsession");
        }   
        Steamworks.SteamUserStats.GetAchievement("TimeFlies", out tutorialCompleted);
        if (tutorialCompleted == false && TimeFlies == true)
        {
            Steamworks.SteamUserStats.SetAchievement("TimeFlies");
        }   
        Steamworks.SteamUserStats.GetAchievement("SkinOfYourTeeth", out tutorialCompleted);
        if (tutorialCompleted == false && SkinOfYourTeeth == true)
        {
            Steamworks.SteamUserStats.SetAchievement("SkinOfYourTeeth");
        }   
        Steamworks.SteamUserStats.GetAchievement("PunchingDown", out tutorialCompleted);
        if (tutorialCompleted == false && PunchingDown == true)
        {
            Steamworks.SteamUserStats.SetAchievement("PunchingDown");
        }   
        Steamworks.SteamUserStats.GetAchievement("ReadTheSignIdiot", out tutorialCompleted);
        if (tutorialCompleted == false && ReadTheSignIdiot == true)
        {
            Steamworks.SteamUserStats.SetAchievement("ReadTheSignIdiot");
        }   
        Steamworks.SteamUserStats.GetAchievement("DejaVu", out tutorialCompleted);
        if (tutorialCompleted == false && DejaVu == true)
        {
            Steamworks.SteamUserStats.SetAchievement("DejaVu");
        }   
        if (tutorialCompleted == false && DoppelGanger == true)
        {
            Steamworks.SteamUserStats.SetAchievement("DoppelGanger");
        } 
        if (tutorialCompleted == false && RallyTheTroops == true)
        {
            Steamworks.SteamUserStats.SetAchievement("RallyTheTroops");
        } 
        if (tutorialCompleted == false && RightToBearArms == true)
        {
            Steamworks.SteamUserStats.SetAchievement("RightToBearArms");
        } 
        if (tutorialCompleted == false && Genocide == true)
        {
            Steamworks.SteamUserStats.SetAchievement("Genocide");
        } 
        if (tutorialCompleted == false && Hoplophobia == true)
        {
            Steamworks.SteamUserStats.SetAchievement("Hoplophobia");
        } 
        }

        SaveSystem.SaveMeta(this);
    }
    public void LoadMeta()
    {
        MetaData m = SaveSystem.LoadMeta();

        if(m != null)
        {
            cores = m.cores;
            deaths = m.deaths;
            kills = m.kills;

            musicvolume = m.musicvolume;
            sfxvolume = m.sfxvolume;

            founditems = m.founditems;
            foundenemies = m.foundenemies;

            HTunlocked = m.HTunlocked;
            MMunlocked = m.MMunlocked;
            DMunlocked = m.DMunlocked;
            CSunlocked = m.CSunlocked;
            MSunlocked = m.MSunlocked;

            SteakUnlocked = m.SteakUnlocked;
            SausageUnlocked = m.SausageUnlocked;
            HamUnlocked = m.HamUnlocked;
            FishUnlocked = m.FishUnlocked;
            BaconUnlocked = m.BaconUnlocked;

            HarvestSeason = m.HarvestSeason;
            RootOfAllEvil = m.RootOfAllEvil;
            GreenEnergy = m.GreenEnergy;
                DoppelGanger = m.DoppelGanger;
            Hoarder = m.Hoarder;
            Botanist = m.Botanist;
                RallyTheTroops = m.RallyTheTroops;
                RightToBearArms = m.RightToBearArms;
            SkillIssue = m.SkillIssue;
            ILoveTheEconomy = m.ILoveTheEconomy;
                Genocide = m.Genocide;
            HeavyHitter = m.HeavyHitter;
            OnePercenter = m.OnePercenter;
                Hoplophobia = m.Hoplophobia;
            Obsession = m.Obsession;
            TimeFlies = m.TimeFlies;
            SkinOfYourTeeth = m.SkinOfYourTeeth;
            PunchingDown = m.PunchingDown;
            ReadTheSignIdiot = m.ReadTheSignIdiot;
            DejaVu = m.DejaVu;
        }
        else
        {
            cores = 0;
            deaths = 0;
            kills = 0;
            musicvolume = 0.4f;
            sfxvolume = 0.4f;
            HTunlocked = false;
            MMunlocked = false;
            DMunlocked = false;
            CSunlocked = false;
            MSunlocked = false;
            SteakUnlocked = false;
            SausageUnlocked = false;
            HamUnlocked = false;
        }
        
    }
    
}
