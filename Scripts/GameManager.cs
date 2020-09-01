using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;


    public GameObject winPanel;
    public GameObject gameOverPanel;
    public GameObject startPanel;
    public int sceneNumber;

    public bool isStarted;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.singleton = this;
    }

    // Update is called once per frame
    public void StartGame()
    {
        isStarted = true;
        startPanel.SetActive(false);
    }

    public void TryAgain()
    {
        SceneManager.LoadScene(sceneNumber);
    }

    public void NextLevel()
    {
        sceneNumber = sceneNumber + 1;
        SceneManager.LoadScene(sceneNumber);
    }
}
