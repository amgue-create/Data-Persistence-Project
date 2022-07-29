using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text highScoreText;
    public GameObject GameOverText;

    private int highScore;
    private string bestPlayerName;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    
    // Start is called before the first frame update
    void Start()
    {
        //spawns in all the blocks
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

        bestPlayerName = NameManager.bestPlayerName;
        highScore = NameManager.highScore;
        highScoreText.text = "Best Score: " + NameManager.bestPlayerName + ": " + NameManager.highScore;
    }

    private void Update()
    {
        //puts the ball in play
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
        //restart button
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        //escape button
        if(Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity Player
#endif
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = NameManager.playerName + "'s" + $" Score : {m_Points}";
        if(m_Points > highScore)
        {
            NameManager.highScore = m_Points;
            NameManager.bestPlayerName = NameManager.playerName;
            highScoreText.text = "Best Score: " + NameManager.playerName + ": " + m_Points;
        }
    }

    public void GameOver()
    {
        if(m_Points > highScore)
        {
            NameManager.Instance.SaveScore();
        }
        m_GameOver = true;
        GameOverText.SetActive(true);
    }
}
