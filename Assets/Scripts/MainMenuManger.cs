using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManger : MonoBehaviour
{
    //int scorePlayer;

    public string namePlayer;

    public InputField inputName;

    public Text score;

    public MainManager.SaveAndLoad mydatapers;

    public static MainMenuManger mainMenuMangerInstance;
    private void Awake()
    {
        if (mainMenuMangerInstance != null)
        {
            Destroy(this.gameObject);
        }

        mainMenuMangerInstance = this;

        mydatapers.Load();

        if (mydatapers.name!="")
        {
            inputName.placeholder.GetComponent<Text>().text = mydatapers.name;
         }
        

       

        

        score.text = "BestScore:"+mydatapers.score;

    }

    public void StartGame()
    {
        namePlayer = inputName.text;

        mydatapers.Save();

        DontDestroyOnLoad(gameObject);

        UnityEngine.SceneManagement.SceneManager.LoadScene("main");
    }

    
}
