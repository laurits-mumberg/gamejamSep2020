using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PaintGameManager : MonoBehaviour
{
    public bool boolGameIsWon;

    public GameObject mill;
    public GameObject flag;
    public GameObject brushesHolder;

    public Animator millAnimator;
    public Animator flagAnimator;
    public Animator brushesHolderAnimator;

    public bool userHoldingBrush = false;
    public Color heldBrushColor = Color.green; //Should never be green
    public string heldBrushColorString;
    public GameObject heldBrush;

    //Brush cursor
    public GameObject cursorBrush;
    public Vector3 brushCursorOffset;
    public SpriteRenderer paintSplatSpriteRenderer;

    //Succes check
    public List<FlagPart> correctFlagParts = new List<FlagPart>();

    // Start is called before the first frame update
    void Start()
    {
        userHoldingBrush = false;
        brushesHolder.SetActive(false);

        cursorBrush.SetActive(false);

        millAnimator = mill.GetComponent<Animator>();
        flagAnimator = flag.GetComponent<Animator>();
        brushesHolderAnimator = brushesHolder.GetComponent<Animator>();

        StartCoroutine(StartGame());
    }

    // Update is called once per frame
    void Update()
    {
        //Brush cursor
        if (userHoldingBrush)
        {
            //Cursor.visible = false;
            cursorBrush.SetActive(true);
            cursorBrush.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + brushCursorOffset;

            //Brush Color
            heldBrushColor.a = 255;
            paintSplatSpriteRenderer.color = heldBrushColor;
        }
        else
        {
            Cursor.visible = true;
            cursorBrush.SetActive(false);
        }

        checkWinCond();
    }

    void checkWinCond()
    {
        if(correctFlagParts.Count >= 5 && !boolGameIsWon)
        {
            boolGameIsWon = true;
            StartCoroutine(FinishMinigame());
        }
    }

    IEnumerator FinishMinigame()
    {
        //Game logic
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().CompleteMinigame();
        boolGameIsWon = true;

        //Animations
        Camera.main.GetComponent<Animator>().SetTrigger("CompleteGame");
        yield return new WaitForSeconds(1);

        //Change scene
        SceneManager.LoadScene("TransitionScene");
    }

    IEnumerator StartGame()
    {
        //Wait for camera pan
        yield return new WaitForSeconds(1);

        //Mill paint animations:
        millAnimator.SetTrigger("Paint");
        yield return new WaitForSeconds(0.2f);

        flagAnimator.SetTrigger("GettingPainted");
        yield return new WaitForSeconds(0.5f);
        flagAnimator.enabled = false;
        brushesHolder.SetActive(true);

        //stars timer
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().MinigameStarted();
    }
}
