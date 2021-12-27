using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public GameObject escMenu;
    public GameObject tutorial;
    public GameObject sumUp;
    public GameObject gameOver;
    public GameObject statisticObject;
    public GameObject incubatorObject;
    public GameObject finishLevelObject;

    public Text aliveStat;
    public Text deathStat_1;
    public Text expStat_1;
    public Text deathStat_2;
    public Text expStat_2;  

    public float menuSlideSpeed = 2f;
    Vector3 gameplayStatistics;
    Vector3 incubationMenu;

    //[Header("Debug Only")]


    StatisticGatherer statisticGatherer;
    LevelSettings levelSettings;
    SoundController soundController;
    MusicPlayer musicPlayer;
  

    bool escMenuOn = false;
    bool levelFinished = false;


    enum LevelStages { Start, ReadyToMunch, GameOver, Completed, LevelSumUp }
    


    private void Awake()
    {
        
        StartLevel();

        gameplayStatistics = statisticObject.transform.position;
        incubationMenu = incubatorObject.transform.position;
    }

    private void Start()
    {
        soundController = FindObjectOfType<SoundController>();
        levelSettings = FindObjectOfType<LevelSettings>();
        statisticGatherer = FindObjectOfType<StatisticGatherer>();
        musicPlayer = FindObjectOfType<MusicPlayer>();
  
   }

    private void PlayMusic()
    {
        if (!musicPlayer) return;
        musicPlayer.GetComponent<AudioSource>().Play();
        musicPlayer.GetComponent<AudioSource>().loop = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !escMenuOn && Time.timeScale > 0) RunEscMenu();
        else if (Input.GetKeyDown(KeyCode.Escape) && escMenuOn) CloseEscMenu();
    }


    private void SlideAway(GameObject obj, string where)
    {
        if (where == "horizontal") obj.transform.position += new Vector3(2000, 0, 0);
        else if (where == "vertical") obj.transform.position += new Vector3(0, 2000, 0);
    }

    private void SlideIn(GameObject obj, string where)
    {
        if (where == "horizontal") obj.transform.position += new Vector3(-2000, 0, 0);
        else if (where == "vertical") obj.transform.position += new Vector3(0, -2000, 0);
    }

    private void RunEscMenu()
    {
        Debug.Log("esc menu");        
        escMenu.SetActive(true);
        PauseGame(); 
        escMenuOn = true;
    }

    public void CloseEscMenu()
    {
        Debug.Log("off");
        escMenu.SetActive(false);
        ResumeGame();
        escMenuOn = false;
    }


    public void QuitGame()
    {
        Application.Quit();
    }
    private void PauseGame()
    {
        Camera.main.GetComponent<FollowPlayer>().enabled = false;
        SlideAway(statisticObject, "vertical");
        SlideAway(incubatorObject, "horizontal");
        Time.timeScale = 0;
        MuteGame();
    }
    private void ResumeGame()
    {
        Camera.main.GetComponent<FollowPlayer>().enabled = true;
        SlideIn(statisticObject, "vertical");
        SlideIn(incubatorObject, "horizontal");
        Time.timeScale = 1;
        UnMuteGame();
    }
    public void MuteGame()
    {
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        
        foreach (AudioSource audio in allAudioSources)
        {
            audio.mute = true;
        }
    }
    public void UnMuteGame()
    {
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();

        foreach (AudioSource audio in allAudioSources)
        {
            audio.mute = false;
        }
    }
    private void Activator(LevelStages stage)
    {
        if (stage == LevelStages.Start)
        {
            levelFinished = false;
            tutorial.SetActive(true);
            sumUp.SetActive(false);
            gameOver.SetActive(false);
        } 
        else if (stage == LevelStages.ReadyToMunch)
        {
            tutorial.SetActive(false);
            PlayMusic();           
        }
        else if (stage == LevelStages.Completed)
        {
            if (!levelFinished) finishLevelObject.SetActive(true);
        }
        else if (stage == LevelStages.LevelSumUp)
        {
            levelFinished = true;
            finishLevelObject.SetActive(false);
            sumUp.SetActive(true);            
            // statisticObject.SetActive(false);
            // incubatorObject.SetActive(false);
        }
        else if (stage == LevelStages.GameOver)
        {
            levelFinished = true;
            finishLevelObject.SetActive(false);
            gameOver.SetActive(true);
            //  statisticObject.SetActive(false);
            //  incubatorObject.SetActive(false);
        }
    }
    private void StartLevel()
    {
        PauseGame();
        Activator(LevelStages.Start);
    }


    public void LevelCompleted() // jeszcze nie koñczy levelu, a daje graczowi mo¿liwoœæ zakoñczenia
    {
        soundController.WinSound();
        Activator(LevelStages.Completed);        
    }

    public void LevelSumUp()
    {
        PauseGame();
        Activator(LevelStages.LevelSumUp);

        aliveStat.text = "Alive Munchies: " + statisticGatherer.aliveMunchies;
        deathStat_1.text = "Death Counter: " + statisticGatherer.deaths;
        if (!levelSettings.turnOffExperience) expStat_1.text = "Total Experience Points " + statisticGatherer.totalExpGathered;
        else expStat_1.text = "";
    }

    public void GameOver()
    {
        soundController.LoseSound();
        PauseGame();
        Activator(LevelStages.GameOver);
        

        deathStat_2.text = "Death Counter: " + statisticGatherer.deaths;
        if (!levelSettings.turnOffExperience) expStat_2.text = "Total Experience Points " + statisticGatherer.totalExpGathered;
        else expStat_2.text = "";       
    }

    // Buttons
    public void ReadyToMunch()
    {
        ResumeGame();
        Activator(LevelStages.ReadyToMunch);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadStartScreen()
    {
        SceneManager.LoadScene("Start Screen");
    }
}
