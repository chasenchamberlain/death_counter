using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathGame : MonoBehaviour
{
    
    public Text deathCountText;
    public int deathCount;
    public string gameName;
    public Text gameNameText;
    
    public Dropdown dropdown;
    public void addDeathCount ()
    {
        deathCount++;

        updatePrefs();
    }

    public void minusDeathCount ()
    {
        deathCount--;

        updatePrefs();
    }


    void updatePrefs()
    {
        deathCountText.text = deathCount.ToString();
        PlayerPrefs.SetInt(gameName, deathCount);
        System.IO.File.WriteAllText(@"C:\Users\chase\Documents\twitch\dc\example.txt", deathCount.ToString());
    }
    public void addGame(string name)
    {
        gameName = name;
        Debug.Log("name: " + gameName);
        if(PlayerPrefs.HasKey(name)) // Game exists already
        {
            gameName = PlayerPrefs.GetString(name);
            deathCount = PlayerPrefs.GetInt(name);
            updatePrefs();
        }
        else // New game
        {
            deathCount = 0;
            updatePrefs();
        }
        gameNameText.text = gameName;
    }

    public void deleteGame()
    {
        PlayerPrefs.DeleteKey(gameName);
        gameNameText.text = "SET GAME FIRST";
        deathCount = 0;
        deathCountText.text = deathCount.ToString();

        System.IO.File.WriteAllText(@"C:\Users\chase\Documents\twitch\dc\example.txt", deathCount.ToString());
    }

    public void deleteAllGames()
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
        if(PlayerPrefs.HasKey(gameName))
        {
            Debug.Log("Game existed " + gameName);
            gameNameText.text = gameName;
            deathCountText.text = PlayerPrefs.GetInt(gameName).ToString();
            deathCount = PlayerPrefs.GetInt(gameName);
        }
        else
        {
            Debug.Log("Game doesn't exist");
            deathCountText.text = PlayerPrefs.GetInt(gameName, 0).ToString();
            gameNameText.text = "SET GAME FIRST";
            gameName = gameNameText.text;
        }
    }

    void PopulateList()
    {
        dropdown.ClearOptions();
        List <string> games = new List<string>();

    }
 
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            addDeathCount();
        }
        else if(Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            minusDeathCount();
        }
    } 


}
