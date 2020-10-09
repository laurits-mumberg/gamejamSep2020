using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MillHandler : MonoBehaviour, IInteractableObj
{
    public GameObject mainGameManager;
    public bool boolGameIsWon = false;

    Collider2D wingCollider;
    float zRotation;
    bool isBeingHold = false;

    public float passiveRotateSpeed;

    //Glow animation
    bool isGlowing = true;
    public Sprite glowSprite;
    public Sprite notGlowSprite;
    public SpriteRenderer thisSpriteRenderer;

    //Minigame Points
    public int totalRots = 0;
    public float goalRots = 15;
    public float nextAngle;

    //Moncher animation
    public GameObject cruncherObj;
    public SpriteRenderer cruncherSpriteRenderer;
    public Sprite[] muncherSprites;

    //CameraAnimation
    Animator cameraAnimator;

    void Start()
    {
        //Find gameManager
        mainGameManager = GameObject.FindGameObjectWithTag("GameManager");

        //Find main camera animator
        cameraAnimator = Camera.main.GetComponent<Animator>();

        //Starter timer
        StartCoroutine(StartGame());

        boolGameIsWon = false;

        wingCollider = GetComponent<Collider2D>();
        zRotation = transform.rotation.z;
        nextAngle = 90;

        //cruncher
        cruncherSpriteRenderer = cruncherObj.GetComponent<SpriteRenderer>();

        //glow
        thisSpriteRenderer = GetComponent<SpriteRenderer>();

    }


    void FixedUpdate()
    {
        //WInning cond
        if (totalRots >= goalRots && boolGameIsWon == false)
        {
            //Wins minigame
            StartCoroutine(FinishMinigame());
        }

        if (isBeingHold && !boolGameIsWon)
        {
            //Visual rotations
            Vector3 mouse = Input.mousePosition;
            Vector3 screenPoint = Camera.main.WorldToScreenPoint(transform.localPosition);
            Vector3 offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
            float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, 0, angle + zRotation);



            //Rotation calculations FUCKING DÅRLIGT SKAMMER MIG
            if(transform.rotation.eulerAngles.z > nextAngle && nextAngle != 0)
            {
                nextAngle = (nextAngle + 90) % 360;
            }
            if(nextAngle == 0)
            {
                if(transform.rotation.eulerAngles.z > nextAngle && transform.rotation.eulerAngles.z <= 90)
                {
                    totalRots++;
                    nextAngle = 90;

                    //Update Muncher animation
                    cruncherSpriteRenderer.sprite = muncherSprites[totalRots % 5];
                }
            }
        }

        else
        {
            transform.Rotate(Vector3.forward * passiveRotateSpeed);
        }

        //Glow:
        if (isGlowing)
        {
            if (!boolGameIsWon && !isBeingHold)
            {
                thisSpriteRenderer.sprite = glowSprite;
            }
            
        }
        else
        {
            thisSpriteRenderer.sprite = notGlowSprite;
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
        boolGameIsWon = true;

        //Animations
        cameraAnimator.SetTrigger("CompleteGame");
        yield return new WaitForSeconds(1);

        //Change scene
        SceneManager.LoadScene("TransitionScene");
    }

    public void onClick()
    {
        isBeingHold = true;
        zRotation = transform.rotation.z;
    }

    public void onRelease()
    {
        isBeingHold = false;
    }

    public void onHover()
    {
        isGlowing = false;
    }

    public void onHoverStop()
    {
        isGlowing = true;
    }
}
