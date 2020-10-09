using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class brushScript : MonoBehaviour, IInteractableObj
{
    public PaintGameManager minigameManager;

    public Sprite normalSprite;
    public Sprite glowSprite;
    public SpriteRenderer thisSpriteRenderer;
    public Color brushColor;
    public string brushColorString;

    public void onClick()
    {
        //Place back other brush if held
        if (minigameManager.userHoldingBrush)
        {
            minigameManager.heldBrush.SetActive(true);
        }
        minigameManager.userHoldingBrush = true;
        minigameManager.heldBrush = this.gameObject;
        minigameManager.heldBrushColor = brushColor;
        minigameManager.heldBrushColorString = brushColorString;
        this.gameObject.SetActive(false);
    }

    public void onHover()
    {
        thisSpriteRenderer.sprite = normalSprite;
    }

    public void onHoverStop()
    {
        thisSpriteRenderer.sprite = glowSprite;
    }

    public void onRelease()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        thisSpriteRenderer = GetComponent<SpriteRenderer>();
        thisSpriteRenderer.sprite = glowSprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (minigameManager.boolGameIsWon)
        {
            thisSpriteRenderer.sprite = normalSprite;
        }
    }
}
