using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DeathGame : MonoBehaviour
{

    private string gameName;

    public Text gameNameText;
    public Dropdown dropdown;
    public Button add;
    public Button minus;
    public Button deleteAllBtn;
    public Button deleteGameBtn;
    public InputField deaths;
    public InputField inputGame;

    private bool gameSelected = false;

    private Dictionary<string, int> gamesAndDeaths = new Dictionary<string, int>();

    public void AddDeathCount()
    {
        gamesAndDeaths[gameName]++;
        UpdatePrefs();
    }

    public void MinusDeathCount()
    {
        gamesAndDeaths[gameName]--;
        UpdatePrefs();
    }


    void UpdatePrefs()
    {
        if (gameSelected)
        {
            deaths.text = gamesAndDeaths[gameName].ToString();
            System.IO.File.WriteAllText(@"C:\Users\chase\Documents\twitch\dc\example.txt", gamesAndDeaths[gameName].ToString());
            Debug.Log($"Saved to playerprefs the game {gameName}:{gamesAndDeaths[gameName]} and saved to file");
        }
        string seralizedGames = MySerialize(gamesAndDeaths);
        Debug.Log($"SERALIZED: {seralizedGames}");
        PlayerPrefs.SetString("games", seralizedGames);
        PlayerPrefs.Save();
    }
    public void AddGame(string name)
    {
        if (!string.IsNullOrEmpty(name.Trim()))
        {
            gameSelected = true;
            gameName = name;
            Debug.Log("name: " + gameName);
            if (gamesAndDeaths.ContainsKey(gameName))
            {
                Debug.Log($"Already have the game {gameName} with a count of {gamesAndDeaths[gameName]}");
                UpdatePrefs();
            }
            else // New game
            {
                gamesAndDeaths.Add(gameName, 0);
                Debug.Log($"New game = {gameName}");
                UpdatePrefs();
            }
            gameNameText.text = gameName;
            EnableAll();
        }
        else
        {
            Debug.Log("Name was whitespace most likely, tisk tisk tisk");
        }
    }

    public void DeleteGame()
    {
        gamesAndDeaths.Remove(gameName);
        DeleteingHousekeeping();
        System.IO.File.WriteAllText(@"C:\Users\chase\Documents\twitch\dc\example.txt", "0");
        Debug.Log("Erased game");
    }

    public void DeleteAllGames()
    {
        gamesAndDeaths.Clear();
        DeleteingHousekeeping();
        PlayerPrefs.DeleteAll();
        System.IO.File.WriteAllText(@"C:\Users\chase\Documents\twitch\dc\example.txt", "0");
        Debug.Log("Deleted all games");
    }

    private void DeleteingHousekeeping()
    {
        gameSelected = false;
        gameNameText.text = "Please select a game";
        deaths.text = "";
        inputGame.text = "";
        gameName = "";
        DisableAll();
        UpdatePrefs();
    }

    // Start is called before the first frame update
    void Start()
    {
        string stringDict = PlayerPrefs.GetString("games");
        if (stringDict.Length != 0)
        {
            Debug.Log($"We got games bb = {stringDict}");
            gamesAndDeaths = MyUnserialize(stringDict);
        }
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
        deaths.enabled = true;
        add.enabled = true;
        minus.enabled = true;
        deleteAllBtn.enabled = true;
        deleteGameBtn.enabled = true;
    }

    private string MySerialize(Dictionary<string, int> dict)
    {
        string result = "";

        foreach (KeyValuePair<string, int> game in dict)
        {
            string toBeAddedString = string.Format("{0}:{1};", game.Key, game.Value);
            result += toBeAddedString;
        }
        return result;
    }

    private Dictionary<string, int> MyUnserialize(string stringDict)
    {
        Dictionary<string, int> tempDict = new Dictionary<string, int>();

        //var dict = stringDict.Split(';')
        //                     .Select(x => x.Split(':'))
        //                     .ToDictionary(x => x[0], x => int.Parse(x[1]));

        foreach (string gnc in stringDict.Split(';'))
        {
            if (!string.IsNullOrEmpty(gnc.Trim()))
            {
                //Debug.Log($"dumb game combo {gnc}");
                string game = gnc.Split(':')[0];
                //Debug.Log($"foreach game is = {game}");
                string count = gnc.Split(':')[1];
                //Debug.Log($"foreach game is = {count}");


                if (!string.IsNullOrEmpty(game.Trim()) && !string.IsNullOrEmpty(count.Trim()))
                {
                    Debug.Log($"{game}:{count}");
                    tempDict.Add(game, int.Parse(count));
                }
            }
        }

        return tempDict;
    }


}
