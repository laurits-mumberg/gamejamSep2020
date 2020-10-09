using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    private GameObject currenltyInteractedObj;

    //For timing games
    public int numberOfWins = 0;
    public float timerDecreaseAmount;
    public float[] minigameTime;
    public bool lostGame = false;
    public bool gameRunning;
    public float timeSpent;
    public float TimeTotal;
    public GameObject BarObject;

    public GameObject scoreObject;
    public TextMeshProUGUI scoreTextMesh;

    //For choosing minigame
    public int numberOfMinigames;
    public List<int> notPlayedMinigames = new List<int>();
    int nextMinigame;

    //For DontDestroyOnLoad
    private static GameManager gameManagerInstance = null;


    void Awake()
    {
        DontDestroyOnLoad(this);

        if (gameManagerInstance == null)
        {
            gameManagerInstance = this;
        }
        else
        {
            GameObject.Destroy(this.gameObject);
        }
    }

    private void Start()
    {

        scoreTextMesh =  scoreObject.GetComponent<TextMeshProUGUI>();
        StartCoroutine(StartNextMinigame());
        BarObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        bool isHoveringIneractable =  checkHover();

        //If the left mouse button is clicked.
        if (Input.GetMouseButtonDown(0))
        {

            if (isHoveringIneractable)
            {
                currenltyInteractedObj.GetComponent<IInteractableObj>().onClick();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if(currenltyInteractedObj != null)
            {
                currenltyInteractedObj.GetComponent<IInteractableObj>().onRelease();
                currenltyInteractedObj = null;
            }
        }

        scoreTextMesh.text = numberOfWins.ToString() + (numberOfWins == 1 ? " STED FIKSET" : " STEDER FIKSET");
    }

    public void FixedUpdate()
    {
        if (gameRunning)
        {
            timeSpent += Time.deltaTime;
            if(timeSpent >= TimeTotal && !lostGame)
            {
                print("Lost game");
                lostGame = true;
                SceneManager.LoadScene("LoseScene");
                scoreObject.GetComponent<Animator>().SetBool("OnScreen", true);
                BarObject.SetActive(false);
            }
        }
    }

    bool checkHover()
    {
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

        if (hit.collider != null && hit.transform.tag == "Interactable")
        {
            currenltyInteractedObj = hit.transform.gameObject;
            currenltyInteractedObj.GetComponent<IInteractableObj>().onHover();
            return true;
        }
        else if(currenltyInteractedObj != null)
        {
            currenltyInteractedObj.GetComponent<IInteractableObj>().onHoverStop();
            currenltyInteractedObj = null;
            return false;
        }
        else
        {
            return false;
        }
    }

    IEnumerator StartNextMinigame()
    {
        yield return new WaitForSeconds(0.5f);
        //Checks if all games have been played
        if(notPlayedMinigames.Count <= 0)
        {
            for (int i = 0; i < numberOfMinigames; i++)
            {
                notPlayedMinigames.Add(i + 2);
            }
        }

        //Choose next minigame
        nextMinigame = notPlayedMinigames[Random.Range(0, notPlayedMinigames.Count)];
        notPlayedMinigames.Remove(nextMinigame);

        scoreObject.GetComponent<Animator>().SetBool("OnScreen", true);

        yield return new WaitForSeconds(2);
        //Pan in on map
        Animator mainCameraAnimator = Camera.main.GetComponent<Animator>();

        mainCameraAnimator.SetInteger("PanInAnim", Random.Range(1, 6));
        mainCameraAnimator.SetTrigger("StartMinigame");
        yield return new WaitForSeconds(0.75f);
        GameObject.FindGameObjectWithTag("Map").GetComponent<Animator>().SetTrigger("FadeOut");
        scoreObject.GetComponent<Animator>().SetBool("OnScreen", false);
        yield return new WaitForSeconds(0.25f);

        //Load next level
        SceneManager.LoadScene(nextMinigame);

    }

    public void MinigameStarted()
    {
        print("Recieved MinigameStarted");
        timeSpent = 0;
        TimeTotal = minigameTime[nextMinigame] - timerDecreaseAmount * numberOfWins;
        gameRunning = true;
        BarObject.GetComponent<Animator>().speed = minigameTime[nextMinigame] / TimeTotal;
        BarObject.SetActive(true);
    }

    public void CompleteMinigame()
    {
        gameRunning = false;
        numberOfWins++;
        BarObject.SetActive(false);
        StartCoroutine(StartNextMinigame());
    }
}
