using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public class MetaData
{
    public int cores;
    public int deaths;
    public int kills;
    
    public float musicvolume;
    public float sfxvolume;

    public List<String> founditems = new List<String>();
    public List<String> foundenemies = new List<String>();

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

    public MetaData(MetaDataManager m)
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
}
