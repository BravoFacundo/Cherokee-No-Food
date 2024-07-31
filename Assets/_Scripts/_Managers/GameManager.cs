using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private enum GameMode { StoryMode , EndlessMode }
    
    [Header("Configuration")]
    public GameState state;
    [SerializeField] private GameMode gameMode;

    [Header("List")]
    public List<GameObject> currentEnemies;

    [Header("References")]
    [SerializeField] private UIManager uiManager;
    [SerializeField] private EnemySpawnHandler enemySpawHandler;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(transform.parent);
        }
        else Destroy(transform.parent.gameObject);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {        
        if (scene.name == "Development")
        {
            if (gameMode == GameMode.StoryMode) StartCoroutine(Tutorial_Part1());
            else if (gameMode == GameMode.EndlessMode) StartCoroutine(EndlessModeStart());
        }
        else
        if (scene.name == "Menu") Debug.Log("Menu");
    }

    void Start()
    {

    }

    private void Update()
    {
        Debug_Inputs();
    }

    public void UpdateGameState(GameState newState)
    {
        state = newState;

        switch (newState)
        {
            case GameState.Menu:
                break;
            case GameState.Gameplay:
                break;
            case GameState.Pause:
                break;
            case GameState.Win:
                break;
            case GameState.Lose:
                break;
        }
    }
    public void SetGameState(string state, bool stateBool)
    {
        if (stateBool) Time.timeScale = 0f; else Time.timeScale = 1f;
        switch (state)
        {
            case "Pause":
                print("Pause");
                break;
            case "Lose":
                print("Lose");
                break;
        }
    }

    //-------------------------------------------------------------------------------------------------------------------

    public void AddNewEnemy(GameObject newEnemy, int enemyToSpawn)
    {

        //currentEnemies.Add(newEnemy);
        //newEnemy.name = $"{newEnemy.name[0..^7]}{currentEnemies.Count}";

        newEnemy.name = $"{newEnemy.name[0..^7]}{currentEnemies.Count}";

        //enemySpawner. ;
        //uiManager.UpdateEnemyBar(currentEnemies.Count-1);
        //Estoy tomando la primera. Esto se rompe si tiene mas de 5 enemigos osea mas de 1 barra
        //enemiesBar.transform.GetChild(0).GetChild(i).GetComponent<Animator>().SetInteger("EnemyReveal", enemyToSpawn);

    }

    //-------------------------------------------------------------------------------------------------------------------

    private IEnumerator Tutorial_Part1()
    {
        Debug.Log("Tutorial Activated");

        //enemySpawHandler.SpawnEnemy(1, new Vector3(0, 0, 11));
        //enemySpawHandler.SpawnEnemy(1, new Vector3(-1.29f, 0, 11.75f));
        //enemySpawHandler.SpawnEnemy(1, new Vector3(1.12f, 0, 12));

        yield return new WaitForSeconds(1f);

        yield return new WaitForEndOfFrame();
    }

    private IEnumerator Level1_Part1()
    {
        Debug.Log("Level 1 Activated");
        yield return new WaitForSeconds(1f);
        yield return new WaitForEndOfFrame();
    }

    //-------------------------------------------------------------------------------------------------------------------

    private IEnumerator EndlessModeStart()
    {
        Debug.Log("Endless Mode Activated");
        yield return new WaitForEndOfFrame();
    }

    //-------------------------------------------------------------------------------------------------------------------

    private void Debug_Inputs()
    {        
        if (Input.GetKeyDown(KeyCode.P)) SetGameState("Pause", true);
    }

}

public enum GameState
{
    Menu,
    Gameplay,
    Pause,
    Win,
    Lose
}
