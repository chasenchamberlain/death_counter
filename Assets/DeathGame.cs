using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathGame : MonoBehaviour
{

    public Text deathCountText;
    private int deathCount;
    private string gameName;
    public Text gameNameText;
    public Dropdown dropdown;
    public Button add;
    public Button minus;
    public Button deleteAllBtn;
    public Button deleteGameBtn;
    public InputField deaths;

    private bool gameSelected = false;

    private Dictionary<string, int> gamesAndDeaths = new Dictionary<string, int>(); 

    public void AddDeathCount()
    {
        deathCount++;
        UpdatePrefs();
    }

    public void MinusDeathCount()
    {
        deathCount--;
        UpdatePrefs();
    }


    void UpdatePrefs()
    {
        deathCountText.text = deathCount.ToString();
        //PlayerPrefs.SetInt(gameName, deathCount);
        //string seralizedGames = MySerali
        System.IO.File.WriteAllText(@"C:\Users\chase\Documents\twitch\dc\example.txt", deathCount.ToString());
        Debug.Log("Saved to playerprefs the game " + gameName + " - " + deathCount + "and saved to file");
    }
    public void AddGame(string name)
    {
        gameName = name;
        if (gameName.Length != 0)
        {
            Debug.Log("name: " + gameName);

            //if (PlayerPrefs.HasKey(name)) // Game exists already
            if (gamesAndDeaths.ContainsKey(gameName))
            {
                Debug.Log("Playerprefs already has this game " + gameName);
                deathCount = gamesAndDeaths[gameName];
                Debug.Log(gameName + " with a death count of " + deathCount);
                UpdatePrefs();
            }
            else // New game
            {
                gamesAndDeaths.Add(gameName, 0);
                Debug.Log("New game, name is = " + gameName);
                deathCount = 0;
                UpdatePrefs();
            }
            gameNameText.text = gameName;
            EnableAll();
        }
        else
        {
            Debug.Log("name was whitespace most likely, tisk tisk tisk");
        }
    }

    public void DeleteGame()
    {
        PlayerPrefs.DeleteKey(gameName);
        gameNameText.text = "SET GAME FIRST";
        deathCount = 0;
        deathCountText.text = deathCount.ToString();

        System.IO.File.WriteAllText(@"C:\Users\chase\Documents\twitch\dc\example.txt", deathCount.ToString());
    }

    public void DeleteAllGames()
    {
        PlayerPrefs.DeleteAll();
        gameNameText.text = "SET GAME FIRST";
        gameName = gameNameText.text;
        deathCount = 0;
        deathCountText.text = deathCount.ToString();

        System.IO.File.WriteAllText(@"C:\Users\chase\Documents\twitch\dc\example.txt", deathCount.ToString());
    }
    // Start is called before the first frame update
    void Start()
    {
        string testString = PlayerPrefs.GetString("games");
        Debug.Log(testString);
        if (!gameSelected)
        {
            Debug.Log("No game selected, disabled all");
            DisableAll();
            gameNameText.text = "Please select a game";
        }
        PopulateList();

    }

    void PopulateList()
    {
        //dropdown.ClearOptions();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameSelected)
        {
            if (Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                AddDeathCount();
            }
            else if (Input.GetKeyDown(KeyCode.KeypadMinus))
            {
                MinusDeathCount();
            }
        }
    }

    private void DisableAll()
    {
        deaths.enabled = false;
        add.enabled = false;
        minus.enabled = false;
        deleteAllBtn.enabled = false;
        deleteGameBtn.enabled = false;
    }

    private void EnableAll()
    {
        deaths.enabled =        true;
        add.enabled =           true;
        minus.enabled =         true;
        deleteAllBtn.enabled =  true;
        deleteGameBtn.enabled = true;
    }


}
