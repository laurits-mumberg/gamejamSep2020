using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagPart : MonoBehaviour, IInteractableObj
{
    public PaintGameManager minigameManager;

    public string succesColorString;
    public string currentColorString = null;

    public void onClick()
    {
        if (minigameManager.userHoldingBrush)
        {
            if(minigameManager.heldBrushColorString == "red")
            {
                GetComponent<SpriteRenderer>().color = Color.red;
            }
            else if(minigameManager.heldBrushColorString == "white")
            {
                GetComponent<SpriteRenderer>().color = Color.white;
            }

            currentColorString = minigameManager.heldBrushColorString;


            //Win cond increment
            if(currentColorString == succesColorString)
            {
                if (!minigameManager.correctFlagParts.Contains(this))
                {
                    minigameManager.correctFlagParts.Add(this);
                }
            }
            else
            {
                minigameManager.correctFlagParts.Remove(this);
            }
        }
    }

    public void onHover()
    {
    }

    public void onHoverStop()
    {
    }

    public void onRelease()
    {
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
