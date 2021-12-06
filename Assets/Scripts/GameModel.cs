using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameModel : MonoBehaviour
{
    public static GameModel instance;
    public string VictoryScene;
    public string GameoverScene;
    private bool levelLoaded = false;

    public int fixScore;

    public int player_health;
    public int player_maxhealth;
    public int player_ammo;
    public int player_defaultammo;
    public bool player_superpowered;
    public bool player_keyitem;
    public bool level_keyitem_dropped;

    bool NewLevelLoaded = false;
    float LevelEndTimer = -1f;
    string NextLevel;

    bool PostGame = false;

    void Awake()
    {
        // if the singleton hasn't been initialized yet
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        NewLevelLoaded = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (levelLoaded)
        {
            if (Input.GetKey("escape")) // quit game
            {
                Application.Quit();
            }

            if (PostGame && Input.GetKeyDown(KeyCode.R)) // reset game
            {
                PostGame = false;
                ResetLevel();
            }

            //Timer for starting next level.
            if (LevelEndTimer >= 0)
            {
                LevelEndTimer -= Time.deltaTime;
                if (LevelEndTimer < 0)
                {
                    if (NextLevel == VictoryScene)
                    {
                        GameWon();
                    }
                    else if (NextLevel == GameoverScene)
                    {
                        GameOver();
                    }
                    else
                    {
                        LoadLevel(NextLevel);
                    }
                }
            }
        }
        else if (NewLevelLoaded)
        {
            StartLevel();
        }
    }

    //The Default Values Set at the beginning of each level
    public void SetDefaultValues()
    {
        player_superpowered = false;
        player_keyitem = false;
        level_keyitem_dropped = false;
        fixScore = 0;
        player_health = player_maxhealth;
        player_ammo = player_defaultammo;
        //Update GUI
        ChangeScore(0);
        ChangeLife(0);
        ChangeAmmo(0);

        //Singleton References
        UIObjective ui_objective = UIObjective.instance;
        LevelInfo level_info = LevelInfo.instance;

        ui_objective.SetText(level_info.starting_text);
    }

    //Resets the Game.
    public void ResetGame()
    {
        LevelEndTimer = -1;
        StartNextLevel("MainScene", 0);
    }

    public void ResetLevel()
    {
        LevelEndTimer = -1;
        StartNextLevel(NextLevel, 0);
        //StartNextLevel(SceneManager.GetActiveScene().name, 0);
    }

    public void StartNextLevel(string sceneName, float waitTime)
    {
        if (LevelEndTimer < 0)
        {
            LevelEndTimer = waitTime;
            NextLevel = sceneName;
        }
    }

    //Loads the named level
    public void LoadLevel(string sceneName)
    {
        //Listen to sceneLoaded
        SceneManager.sceneLoaded += OnSceneChanged;
        levelLoaded = false;
        SceneManager.LoadScene(sceneName);
    }

    //Listener sceneLoaded
    private void OnSceneChanged(Scene scene, LoadSceneMode mode)
    {
        //Remove listen from sceneLoaded
        SceneManager.sceneLoaded -= OnSceneChanged;
        NewLevelLoaded = true;
    }

    //Resets Values and sets level Loaded to True.
    public void StartLevel()
    {
        SetDefaultValues();
        levelLoaded = true;
        NewLevelLoaded = false;
        NextLevel = SceneManager.GetActiveScene().name;
    }

    //Changes Score by Amount
    public void ChangeScore(int amount)
    {
        //Singleton References
        UIScore ui_score = UIScore.instance;
        SFXHandler sfx = SFXHandler.instance;

        fixScore += amount;
        ui_score.UpdateScore(fixScore);

        if (amount > 0)
        {
            sfx.PlayFix();
        }
    }

    //Changes Score by Amount
    public void ChangeLife(int amount)
    {
        //Singleton Reference
        UIHealthBar ui_healthbar = UIHealthBar.instance;

        //Set Player health, min is 0,max is player_maxhealth
        player_health = Mathf.Clamp(player_health + amount, 0, player_maxhealth);

        //Update Health Bar
        ui_healthbar.SetValue(player_health / (float)player_maxhealth);

        //If Player health is zero, set up lose condition...
        if (player_health == 0)
        {
            //Singleton Reference
            PlayerController controller = PlayerController.instance;
            controller.SetSimulated(false);
            Lose();
        }
    }

    //Changes Score by Amount
    public void ChangeAmmo(int amount)
    {
        //Make sure player is not superpowered.
        //Player can still pick up ammo otherwise.
        if (!player_superpowered || amount > 0)
        {
            //Singleton Reference
            UIAmmo ui_ammo = UIAmmo.instance;

            player_ammo += amount;
            //If Player ammo has underflowed, set to 0...
            if (player_ammo < 0)
            {
                player_ammo = 0;
            }
            ui_ammo.UpdateAmmo(player_ammo);
        }
    }

    public bool HasAmmo()
    {
        //Check if player is superpowered, or has ammo
        return (player_superpowered || player_ammo > 0);
    }

    public void PickUp_KeyItem()
    {
        //Singleton References
        UIObjective ui_objective = UIObjective.instance;
        LevelInfo level_info = LevelInfo.instance;

        ui_objective.SetText(level_info.victory_text);

        player_keyitem = true;
    }

    public void Drop_KeyItem(GameObject dropper)
    {
        if (HasObjective() && !HasKeyItemDropped())
        {
            //Singleton References
            PlayerController controller = PlayerController.instance;
            LevelInfo level_info = LevelInfo.instance;

            //Spawn Key Item
            GameObject newkeyitem = Instantiate(level_info.victory_item);
            newkeyitem.transform.position = Vector3.Lerp(dropper.transform.position, controller.transform.position, 0.1f);

            level_keyitem_dropped = true;

            UIObjective ui_objective = UIObjective.instance;
            ui_objective.SetText("Pick up the Key!");
        }
    }

    public bool HasKeyItem()
    {
        return player_keyitem;
    }

    public bool HasKeyItemDropped()
    {
        return level_keyitem_dropped;
    }

    public bool HasObjective()
    {
        //Singleton Reference
        LevelInfo level_info = LevelInfo.instance;
        return (fixScore >= level_info.objective_count);
    }

    public void Win()
    {
        //Make sure level isnt loading already...
        if (LevelEndTimer < 0)
        {
            //Singleton Reference
            SFXHandler sfx = SFXHandler.instance;
            LevelInfo level_info = LevelInfo.instance;



            //Check if final level
            if (!level_info.finalLevel)
            {
                sfx.PlayQuest();
                StartNextLevel(level_info.nextLevel, 5f);
            }
            else
            {
                sfx.PlayWin();
                StartNextLevel(VictoryScene, 5f);
            }
        }
    }

    public void Lose()
    {
        //Make sure level isnt loading already...
        if (LevelEndTimer < 0)
        {
            //Singleton References
            UIDialogue ui_diag = UIDialogue.instance;
            SFXHandler sfx = SFXHandler.instance;

            ui_diag.SetText("Oh no, you lost!");
            ui_diag.Show();
            sfx.PlayLose();

            StartNextLevel(GameoverScene, 5f);
        }
    }

    private void GameOver()
    {
        NewLevelLoaded = false;
        NextLevel = SceneManager.GetActiveScene().name;
        PostGame = true;
        SceneManager.LoadScene(GameoverScene);
        //DestroySelf();
    }

    private void GameWon()
    {
        NewLevelLoaded = false;
        NextLevel = "MainScene";
        PostGame = true;
        SceneManager.LoadScene(VictoryScene);
        //DestroySelf();
    }

    private void DestroySelf()
    {
        instance = null;
        Destroy(gameObject);
    }
}