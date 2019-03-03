using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using MyLib;

/*
 NOTES
 Remove game controller from all but first scene? Recognize stuff again
     */

public class GameController : MonoBehaviour
{
    public GUIText gameOverText, LevelText, DebugText;
    public GameObject MainMenuPanel, LevelsPanel, EscapeMenuPanel;
    private bool gameOver, restart, menu, escape;
    private int iLevel, levelReached, iSceneLevel1, iSceneMMBackground;


    //////////////////////////
    //NUMBER OF LEVELS
    private int nLevels = 9;
    //NUMBER OF LEVELS
    //////////////////////////

    // Use this for initialization
    void Start()
    {

        //Debug.Log("GameController: Delete this code!");
        //T_Angle t_Angle = new T_Angle(90, AngleType.AbsDeg);
        //Debug.Log("t_Angle.AngAbsRad = " + t_Angle.AngAbsRad);

        //DontDestroyOnLoad(this); /*Don't destroy the game controller? Done with other script*/

        //iLevel = 1; /*0 is main menu. Doing it with a function called from main menu button instead*/
        //LevelText.text = "Level " + (iLevel);

        /*Loads progress from file*/
        /*MAKE THIS WORK!*/
        DebugText.text = "";
        //DebugText.text += "\nlevelReached = " + levelReached + "   FROM GAMECONTROLLER";

        gameOver = false;
        restart = false;
        menu = true;
        escape = false;

        iSceneMMBackground = 1;
        //iSceneMainMenu = 1;
        iSceneLevel1 = 2; //This should be the scene index in the build settings of the first level
        levelReached = 1;

        gameOverText.text = "";
        LevelText.text = "";



        //Debug.Log(Application.persistentDataPath);

        /*OLD: Background is in other scene because then I can go to Main manu again, without creating new gamecontroller etc.*/

        //The game starts in SetupScene which contains this GameController, text boxes, Canvas with menus, Not camera, that is in other scenes

        //Screen.SetResolution(1280, 720, false);

        ScenesSetup();

        //loadMainMenu();
        //loadLevel()
    }

// Update is called once per frame
void Update()
    {
        /*Escape Menu*/
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menu)
            {
                QuitF();
            }
            if (!menu)
            {
                if (!escape)
                {
                    EscapeMenuPanel.SetActive(true);
                    escape = true;
                }
                else //escape == true
                {
                    EscapeMenuPanel.SetActive(false);
                    escape = false;
                }
            }
        }

        if (restart)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                restartLevel();
            }
        }
    }

    //   int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
    //if (SceneManager.sceneCount > nextSceneIndex)
    //{
    //    SceneManager.LoadScene(nextSceneIndex);
    //}

    /// <summary>
    /// CHANGE NAME£? dont do this in function?
    /// </summary>
    public void ScenesSetup()
    {
        //SceneManager.LoadScene(iSceneMMBackground); //Done this way to only load 
        //mainManuLoadedBefore = true;
        SceneManager.LoadScene(iSceneMMBackground);
    }

    public void GameOver()
    {
        gameOverText.text = "Game Over!\nPress 'r' to restart";
        gameOver = true;
        //UnityEngine.WaitForSeconds(1);
        restart = true;
    }

    public void HitGoal()
    {
        //gameOverText.text = "You got to the beach!";
        if (iLevel >= nLevels)
        {
            gameOverText.text = "You Won!";
        }
        else if (iLevel < nLevels)
        {
            iLevel++;
            loadLevel(iLevel);
        }
    }

    public void loadLevel(int n)
    {
        iLevel = n;
        levelReached = Mathf.Max(levelReached, iLevel);

        myLoadScene(n);
        LevelText.text = "Level " + (iLevel);

        //DebugText.text += "\nlevelReached = " + levelReached + "   FROM GAMECONTROLLER";

        menu = false;
        MainMenuPanel.SetActive(false);
        LevelsPanel.SetActive(false);

        //if(n == 1)
        //{
        //    Debug.Log("Got here");

        //    GameObject camera = GameObject.Find("findTestCamera");
        //    if (camera) { Debug.Log("Camera found"); }
        //    else { Debug.Log("Camera found"); }

        //    //GameObject tiles = GameObject.Find("Tiles");
        //    //if (gameOverText) { Debug.Log("gameOverText found"); }
        //    //else { Debug.Log("gameOverText not found"); }

        //    //GameObject tiles = GameObject.FindGameObjectWithTag("Tiles"); //is this a list?
        //    //if (tiles) { Debug.Log("tiles found"); }
        //    //else { Debug.Log("tiles not found"); }
        //    ////GameObject tile00 = GameObject.Find("00");
        //    //GameObject tile00 = GameObject.Find("01");
        //    //if (tile00) { Debug.Log("tile found"); }
        //    //else { Debug.Log("tile not found"); }
        //    ////GameObject.
        //    //Camera camera = tile00.transform.Find("Camera").GetComponent<Camera>();
        //    //if (camera) { Debug.Log("camera found"); }
        //    //else { Debug.Log("camera not found"); }
        //    //camera.enabled = true;
        //}
        //Save(); /*Have this somewhere else? Manual*/
        //Debug.Log("INCOMMENT SAVE!");
    }

    /// <summary>
    /// Load the scene of level n
    /// </summary>
    /// <param name="iLevel"></param>
    public void myLoadScene(int n)
    {
        int iScene = n + iSceneLevel1 - 1; //"-1" because if zero based numbering
        SceneManager.LoadScene(iScene);
    }

    public void restartLevel()
    {
        myLoadScene(iLevel);
        OceanController oceanController = GameObject.Find("OceanController").gameObject.GetComponent<OceanController>();
        if (!oceanController) throw new UnassignedReferenceException("Canoot find oceanController");
        oceanController.Restart();
        EscapeMenuPanel.SetActive(false);
        escape = false;
        gameOverText.text = "";
    }

    public void StartPressed()
    {
        loadLevel(levelReached);
    }

    public void GoToMM()
    {
        escape = false;
        EscapeMenuPanel.SetActive(false);
        gameOverText.text = "";
        SceneManager.LoadScene(iSceneMMBackground);
        MainMenuPanel.SetActive(true);
    }

    public void QuitF()
    {
        Application.Quit();
    }



}

//    /*Save to file*/
//    public void Save()
//    {
//        BinaryFormatter bf = new BinaryFormatter();
//        FileStream file = File.Open(Application.persistentDataPath + "/SailboatySave.dat", FileMode.Open);

//        PlayerData data = new PlayerData();
//        data.levelReached = levelReached;

//        //DebugText.text = "levelReached = " + data.levelReached + "FROM GAMECONTROLLER";

//        gameOverText.text = "" + data.levelReached;

//        bf.Serialize(file, data);
//        file.Close();
//    }

//    /*Load from file*/
//    public void Load()
//    {
//        if( File.Exists(Application.persistentDataPath + "/SailboatySave.dat") )
//        {
//            BinaryFormatter bf = new BinaryFormatter();
//            FileStream file = File.Open(Application.persistentDataPath + "/SailboatySave.dat", FileMode.Open);
//            PlayerData data = (PlayerData) bf.Deserialize(file);
//            file.Close();

//            levelReached = data.levelReached;
//            //LevelText.text = "levelReached = " + levelReached + "   FROM GAMECONTROLLER";
//        }
//        //else
//        //{
//        //    levelReached = 1;
//        //    LevelText.text = "levelReached = " + levelReached + "   FROM GAMECONTROLLER";
//        //    /*And other default values*/
//        //}



//    }
//}

///*For saving progress to file*/
//[Serializable]
//class PlayerData
//{
//    public int levelReached;

//    /*Array of fastest times? Make defaults in Load*/
//}
