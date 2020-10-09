using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TopGameHandler : MonoBehaviour
{
    public GameManager mainGameManager;


    public GameObject wings;
    public GameObject millHat;
    public GameObject badGear;
    public GameObject newGear;

    public MoveAwayObj wingsScript;
    public MoveAwayObj millHatScript;
    public MoveAwayObj badGearScript;
    public InsertObj newGearScript;

    public bool hasMovedWings = false;
    public bool hasMovedmillHat = false;
    public bool hasMovedbadGear = false;
    public bool hasInsertedNewGear = false;

    private bool gameIsDone = false;

    void Start()
    {
        mainGameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        gameIsDone = false;

        wingsScript = wings.GetComponent<MoveAwayObj>();
        millHatScript = millHat.GetComponent<MoveAwayObj>();
        badGearScript = badGear.GetComponent<MoveAwayObj>();
        newGearScript = newGear.GetComponent<InsertObj>();

        millHatScript.enabled = false;

        //For moving in right order
        millHat.GetComponent<BoxCollider2D>().enabled = false;
        badGear.GetComponent<BoxCollider2D>().enabled = false;

        StartCoroutine(StartGame());
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasMovedWings)
        {
            if (wingsScript.hasMovedAway)
            {
                hasMovedWings = true;
                millHat.GetComponent<BoxCollider2D>().enabled = true;
                millHatScript.enabled = true;
            }
        }
        if(hasMovedWings && !hasMovedmillHat)
        {
            if (millHatScript.hasMovedAway)
            {
                hasMovedmillHat = true;
                badGear.GetComponent<BoxCollider2D>().enabled = true;
            }
        }

        if(hasMovedWings && hasMovedmillHat && !hasMovedbadGear)
        {
            if (badGearScript.hasMovedAway)
            {
                hasMovedbadGear = true;
                newGear.GetComponent<BoxCollider2D>().enabled = true;
                newGearScript.enabled = true;
            }
        }
        if(newGearScript.isPlacedCorrectly && !gameIsDone)
        {
            gameIsDone = true;
            StartCoroutine(FinishMinigame());
        }
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(0.5f);
        mainGameManager.GetComponent<GameManager>().MinigameStarted();
    }

    IEnumerator FinishMinigame()
    {
        //Game logic
        mainGameManager.GetComponent<GameManager>().CompleteMinigame();

        //Animations
        yield return new WaitForSeconds(0.4f);
        Camera.main.GetComponent<Animator>().SetTrigger("CompleteGame");
        yield return new WaitForSeconds(1);

        //Change scene
        SceneManager.LoadScene("TransitionScene");
    }
}
