using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

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

    public Action action; 
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

    public void EditDeathCount(string newCount)
    {
        if (!string.IsNullOrEmpty(newCount.Trim()))
        {
            gamesAndDeaths[gameName] = int.Parse(newCount);
        }
        UpdatePrefs();
    }

    void UpdatePrefs()
    {
        if (gameSelected)
        {
            gameNameText.text = gameName;
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
                FindDropdownIndex();
            }
            else // New game
            {
                gamesAndDeaths.Add(gameName, 0);
                Debug.Log($"New game = {gameName}");
                UpdatePrefs();
                PopulateList();
            }
            gameNameText.text = gameName;
            EnableAll();
        }
        else
        {
            Debug.Log("Name was whitespace most likely, tisk tisk tisk");
        }
        inputGame.text = "";
    }

    public void DeleteGame()
    {
        gamesAndDeaths.Remove(gameName);
        DeleteingHousekeeping();
        DropdownSelected(0);
        dropdown.RefreshShownValue();
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
        PopulateList();
        DisableAll();
        UpdatePrefs();
    }

    // Start is called before the first frame update
    void Start()
    {
        string stringDict = PlayerPrefs.GetString("games");
        if (!string.IsNullOrEmpty(stringDict.Trim()))
        {
            Debug.Log($"We got games bb = {stringDict}");
            gamesAndDeaths = MyUnserialize(stringDict);
            DropdownSelected(0);
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
        dropdown.ClearOptions();
        var gamesList = gamesAndDeaths.Keys.ToList();
        dropdown.AddOptions(gamesList);
    }

    public void DropdownSelected(int index)
    {
        if (gamesAndDeaths.Count > 0)
        {
            var gamesList = gamesAndDeaths.Keys.ToList();
            gameName = gamesList[index];
            Debug.Log($"Dropdown selected {gameName}");
            gameSelected = true;
            EnableAll();
            UpdatePrefs();
        }
    }

    void FindDropdownIndex()
    {
        if (gamesAndDeaths.Count > 0)
        {
            var gamesList = gamesAndDeaths.Keys.ToList();
            int index = gamesList.FindIndex(x => x.Equals(gameName));
            Debug.Log($"found index in dropdown of {gameName} at postion {index}");
            DropdownSelected(index);
        }
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
        deaths.interactable = false;
        add.interactable = false;
        minus.interactable = false;
        deleteAllBtn.interactable = false;
        deleteGameBtn.interactable = false;
    }

    private void EnableAll()
    {
        deaths.interactable = true;
        add.interactable = true;
        minus.interactable = true;
        deleteAllBtn.interactable = true;
        deleteGameBtn.interactable = true;
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
