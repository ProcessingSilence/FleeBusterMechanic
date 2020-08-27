using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneCycler : MonoBehaviour
{
    public GameObject[] Player = new GameObject[3];
    public string[] cycledScenes = new string [3];
    //public Vector2[] playerPos = new Vector2[3];
    private string currentScene;

    private int sceneIteration;

    private Image timerUI;

    // maxTime used in editor to place wait time amount,
    // currentTime sets bar size and determines time left.
    public float maxTime, currentTime;

    public GameObject canvasObj;
    // Start is called before the first frame update
    void Awake()
    {
        currentScene = cycledScenes[sceneIteration];
        FindPlayer();
        // Destroy self if there's already a scenecycler in place.
        var findOther = GameObject.FindWithTag("SceneCycler");
        if (findOther)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        canvasObj = Instantiate(canvasObj);
        timerUI = canvasObj.transform.GetChild(0).GetComponent<Image>();
        DontDestroyOnLoad(canvasObj);
        StartCoroutine(TimerIteration());
    }

    // Update is called once per frame
    void Update()
    {
        // Fill amount of timer UI based on timer percentage.
        timerUI.fillAmount = currentTime / maxTime;
        
        // Change scene upon current scene being different than cycledScenes iterated scene, currentScene = new scene.
        if (cycledScenes[sceneIteration] != currentScene)
        {
            Debug.Log("Scene change detected, changing scene to '" + cycledScenes[sceneIteration] + "'.");
            currentScene = cycledScenes[sceneIteration];
            SceneManager.LoadScene(cycledScenes[sceneIteration]);
            StartCoroutine(TimerIteration());
            FindPlayer();
        }
    }
    
    // Find player by "Player" tag, destroys it if there's a player in array slot, adds it to array slot if it's empty.
    void FindPlayer()
    {
        var findOther = GameObject.FindWithTag("Player");
            
        // Destroy other player if there's already an obj in iterated array slot.
        if (Player[sceneIteration])
        {
            Destroy(findOther);
            Player[sceneIteration].SetActive(true);
            Debug.Log("Player clone deleted, Player " + sceneIteration + " reactivated.");
        }
            
        // Else, place detected player in iterated array slot; set it to not be destroyed on load.
        else
        {
            Player[sceneIteration] = findOther;
            DontDestroyOnLoad(Player[sceneIteration]);
            Debug.Log("No player clone detected, current played has been placed in Player [" + sceneIteration + "]");
        }
    }

    // Iterates current time downward by 0.01f, stops repeating and changes iteration upon time equaling or going below 0.
    IEnumerator TimerIteration()
    {
        // Cycle down if above/not equal to 0.
        if (!(currentTime <= 0))
        {
            yield return new WaitForSecondsRealtime(0.01f);
            currentTime -= 0.01f;
            StartCoroutine(TimerIteration());
        }
        
        // Restart time, set current scene player to inactive, add to scene iteration (restart iteration if it goes above length).
        else
        {
            currentTime = maxTime;
            if (sceneIteration == cycledScenes.Length)
            {
                sceneIteration = 0;
                //playerPos[cycledScenes.Length] = Player[cycledScenes.Length].transform.position;
                Player[cycledScenes.Length].SetActive(false);
            }
            else
            {
                //playerPos[sceneIteration] = Player[sceneIteration].transform.position;
                Player[sceneIteration].SetActive(false);
                sceneIteration += 1;
            }
        }
    }
}
