using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Steamworks;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject giveUpMenu;
    [SerializeField] private GameObject detailedDescription;
    [SerializeField] private GameObject HUD;

    [SerializeField] private GameObject itemLogbook;
    [SerializeField] private GameObject enemyLogbook;
    [SerializeField] private GameObject trooperLogbook;
    [SerializeField] private GameObject gunLogbook;

    
    
    public Camera cam;
    public fadeout f;
    public List<Item> inventory;
    public Image itemprefab;
    public List<Image> invdisplay; 
    public String[] gameovermessages = {"SKILL ISSUE", "YOU'RE BAD", "GIT GUD", "MASSIVE L", "CRY ABOUT IT"};

    void Start()
    {
        HUD = GameObject.Find("HUD");
        if(SceneManager.GetActiveScene().name != "Main Menu" && SceneManager.GetActiveScene().name != "Select Screen")
        {
            inventory = DataManager.Instance.playeritems;
            ResumeButton();
            updateInv();
            HUD.SetActive(true);
            gameOverMenu.SetActive(false);
            UpdateCoreNumber();
            
        }
    }

    void Update()
    {
        if(Input.GetKeyDown("p"))
        {
            if(DataManager.Instance.paused && GameObject.Find("Resume") != null)
            {
                ResumeButton();
            }
            else if(!DataManager.Instance.paused && GameObject.Find("Pause") != null)
            {
                PauseButton();
            }
        }
    }

    public void updateInv()
    {
        float size;
        float rowsize;
        if(inventory.Count > 34)
        {
            size = 0.625f;
            rowsize = 20;
        }
        else if(inventory.Count > 30)
        {
            size = 0.75f;
            rowsize = 17;
        }
        else if(inventory.Count > 26)
        {
            size = 0.875f;
            rowsize = 15;
        }
        else
        {
            size = 1;
            rowsize = 13;
        }
        foreach(Image i in invdisplay)
        {
            Destroy(i.gameObject);
        }
        invdisplay.Clear();
        int row = 0;
        int col = 0;
        foreach(Item i in inventory)
        {
            Image x = Instantiate(itemprefab, this.gameObject.transform, false);
            x.gameObject.GetComponent<RectTransform>().localScale = new Vector3 (size, size, 1);
            Vector2 xpos = x.transform.position;
            xpos = new Vector2(xpos.x+(45*size*col*gameObject.GetComponent<RectTransform>().localScale.x), xpos.y-(45*size*row*gameObject.GetComponent<RectTransform>().localScale.y));
            x.transform.position = xpos;
            x.gameObject.GetComponent<ItemDisplay>().i = i;
            x.gameObject.GetComponent<ItemDisplay>().desc = detailedDescription;
            invdisplay.Add(x);

            if(col == rowsize-1)
            {
                row++;
                col = 0;
            }
            else
            {
                col++;
            }
        }
    }

    public void UpdateCoreNumber()
    {
        GameObject.Find("CoreNumber").GetComponent<TextMeshProUGUI>().text = ":" + MetaDataManager.Instance.cores;
    }

    public void PauseButton()
    {
        Time.timeScale = 0f;
        DataManager.Instance.paused = true;
        pauseMenu.SetActive(true);
        pauseButton.SetActive(false);
    }

    public void ResumeButton()
    {
        Time.timeScale = 1f;
        DataManager.Instance.paused = false;
        pauseMenu.SetActive(false);
        pauseButton.SetActive(true);
    }

    public void GiveUpButton()
    {
        pauseMenu.SetActive(false);
        giveUpMenu.SetActive(true);
    }

    public void NoButton()
    {
        pauseMenu.SetActive(true);
        giveUpMenu.SetActive(false);
    }

    public void YesButton()
    {
        Time.timeScale = 1f;
        DataManager.Instance.paused = false;
        pauseMenu.SetActive(false);
        giveUpMenu.SetActive(false);
        pauseButton.SetActive(true);
        Player p = GameObject.Find("Player").GetComponent<Player>();
        p.health = 0;
        p.StartCoroutine(p.PlayerDeath());
    }

    public void QuitGame()
    {
        MetaDataManager.Instance.SaveMeta();
        SteamAPI.Shutdown();
        Application.Quit();
        
    }

    public void StartButton(int x)
    {
        
        StartCoroutine(s(x)); 
    }

    public void RetryButton()
    {
        StartCoroutine(r()); 
    }

    public void QuitButton()
    {
        StartCoroutine(q()); 
    }

    public void GameOver()
    {
        MetaDataManager.Instance.SaveMeta();
        DataManager.Instance.paused = true;
        gameOverMenu.SetActive(true);
        StartCoroutine(g());
        if(MetaDataManager.Instance.cores < DataManager.Instance.revivecost)
        {
            GameObject.Find("Revive").GetComponent<Button>().interactable = false;
        }
        else
        {
            GameObject.Find("Revive").GetComponent<Button>().interactable = true;
        }
        
    }

    public void RevivePlayer()
    {
        DataManager.Instance.paused = false;
        gameOverMenu.SetActive(false);
        HUD.SetActive(true);
        Time.timeScale = 1f;
        DataManager.Instance.gameover = false;

        Player p = GameObject.Find("Player").GetComponent<Player>();
        p.health = 0;
        p.PlayerHeal(p.maxHealth, false);

        MetaDataManager.Instance.cores -= DataManager.Instance.revivecost;
        MetaDataManager.Instance.SaveMeta();
        DataManager.Instance.revivecost *= 2;
        UpdateCoreNumber();
        
        f.get1flash();
        GameObject.Find("Square").GetComponent<AudioSource>().Play();
    }

    public void openMenuButton(GameObject g)
    {
        StartCoroutine(openMenu(g));
    }

    public void backToMainButton(GameObject g)
    {
        StartCoroutine(backToMain(g));
        GameObject.Find("Meatgun Title copy").transform.position = new Vector3(0, -0.4f, 0);
    }

    public void switchLogbooks(GameObject g)
    {
        itemLogbook.SetActive(false);
        enemyLogbook.SetActive(false);
        trooperLogbook.SetActive(false);
        gunLogbook.SetActive(false);
        g.SetActive(true);
    }

    IEnumerator g()
    {
        HUD.SetActive(false);

        int sum = 0;
        foreach(Item i in DataManager.Instance.playeritems)
        {
            sum += i.stacks;
        }

        int minutes = (int)(DataManager.Instance.time/60);
        int seconds = (int)(DataManager.Instance.time%60);
        String secondstring = seconds.ToString();
        if(seconds < 10)
        {
            secondstring = "0" + secondstring;
        }

        TextMeshProUGUI gameover = GameObject.Find("Game Over").GetComponent<TextMeshProUGUI>();
        if(UnityEngine.Random.Range(0, 10) < 1)
        {
            gameover.text = gameovermessages[UnityEngine.Random.Range(0, gameovermessages.Length)];
            if(MetaDataManager.Instance.PunchingDown == false)
            {
                MetaDataManager.Instance.PunchingDown = true;
                MetaDataManager.Instance.SaveMeta();
            }
        }

        TextMeshProUGUI stats = GameObject.Find("Stats").GetComponent<TextMeshProUGUI>();
        stats.text = "Time Alive:\t" + minutes +":" + secondstring + 
        "\n\nRooms Cleared:\t"+ (DataManager.Instance.room-1) + 
        "\n\nItems Collected:\t"+ sum + 
        "\n\nMoney Earned:\t"+ DataManager.Instance.totalmoney +
        "\n\nCores Earned:\t"+ DataManager.Instance.corescollected;

        GameObject.Find("Revive").transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Revive (" + DataManager.Instance.revivecost + " Cores)";
        

        Image im = gameOverMenu.transform.GetChild(0).gameObject.GetComponent<Image>();
        gameOverMenu.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(im.color.r, im.color.g, im.color.b, 0);
        gameOverMenu.transform.GetChild(1).gameObject.GetComponent<CanvasGroup>().alpha = 0;
        for(float i = 1f; i>=.7; i-=0.01f)
        {
            gameOverMenu.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(im.color.r, im.color.g, im.color.b, i);
            
            yield return new WaitForSecondsRealtime(0.01f);
            
        }
        
        
        for(float i = 0f; i<=1; i+=0.1f)
        {
            gameOverMenu.transform.GetChild(1).gameObject.GetComponent<CanvasGroup>().alpha = i;
            yield return new WaitForSecondsRealtime(0.01f);
        }
    }

    IEnumerator s(int x)
    {
        f.Leave();
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            yield return new WaitForSecondsRealtime(1);
        }
        else
        {
            for(float i = 1; i>=0; i-=0.1f)
        {
            yield return new WaitForSecondsRealtime(0.1f);
            MenuMusic.Instance.aud.volume -= 0.1f;
        }
        }
        
        
        startButton.SetActive(false);
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            SceneManager.LoadScene("Select Screen");
        }
        else
        {
            if(x == 1)
            {
                SceneManager.LoadScene("B1-0");
            }
            if(x == 2)
            {
                SceneManager.LoadScene("B2-0");
            }
            if(x == 3)
            {
                SceneManager.LoadScene("B3-0");
            }
            if(x == 4)
            {
                SceneManager.LoadScene("B4-0");
            }
            
        }
        
    }

    

    IEnumerator r()
    {
        f.Restart();
        yield return new WaitForSecondsRealtime(1);
        gameOverMenu.SetActive(false);
        Destroy(DataManager.Instance.gameObject);
        SceneManager.LoadScene("B1-0");
        Time.timeScale = 1;
    }

    IEnumerator q()
    {
        f.Restart();
        yield return new WaitForSecondsRealtime(1);
        gameOverMenu.SetActive(false);
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            SceneManager.LoadScene("Main Menu");
        }
        else
        {
            SceneManager.LoadScene("Select Screen");
        }
        Time.timeScale = 1;
    }

    IEnumerator openMenu(GameObject g)
    {
        f.Leave();
        yield return new WaitForSeconds(1);
        g.SetActive(true);
        f.Enter();
    }

    IEnumerator backToMain(GameObject g)
    {
        f.Leave();
        yield return new WaitForSeconds(1);
        g.SetActive(false);
        f.Enter();
    }
}
