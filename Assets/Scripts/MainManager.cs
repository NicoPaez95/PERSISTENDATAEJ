using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    public Text NameandHighscore;

    public SaveAndLoad mydatapers;

    private void Awake()
    {
        mydatapers.Load();
    }

    // Start is called before the first frame update
    void Start()
    {
       

        NameandHighscore.text= "Best Score:"+MainMenuManger.mainMenuMangerInstance.namePlayer +":"+mydatapers.score;

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {

        if (mydatapers.score<m_Points)
        {
            mydatapers.score = m_Points;
        }

        mydatapers.name = MainMenuManger.mainMenuMangerInstance.namePlayer;

        mydatapers.Save();

        m_GameOver = true;
        GameOverText.SetActive(true);
    }

    

    [System.Serializable]
    public class SaveAndLoad
    {
        
        public int score;

        public string name;
        public void Save()
        {
            SaveAndLoad myData = new SaveAndLoad();

            myData.score = score;

            myData.name = name;

            var json = JsonUtility.ToJson(myData);

            File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
        }

        public void Load()
        {
            string path = Application.persistentDataPath + "/savefile.json";

            if (File.Exists(path))
            {
              
               var json = File.ReadAllText(path);

                SaveAndLoad mydata = JsonUtility.FromJson<SaveAndLoad>(json);

                score = mydata.score;

                name = mydata.name;

            }

            
        }
    }

}

