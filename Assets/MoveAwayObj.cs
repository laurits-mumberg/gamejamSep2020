using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAwayObj : MonoBehaviour, IInteractableObj
{
    public bool isMoving = false;
    public bool hasMovedAway = false;
    private bool isBeingHovered = false;

    public SpriteRenderer thisSpriteRenderer;
    public Sprite glowSprite;
    public Sprite normalSPrite;

    public float dragOffsetX;
    public float dragOffsetY;

    public void onClick()
    {
        isMoving = true;
    }

    public void onHover()
    {
        isBeingHovered = true;
    }

    public void onHoverStop()
    {
        isBeingHovered = false;
    }

    public void onRelease()
    {
        isMoving = false;
        hasMovedAway = true;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }

    // Start is called before the first frame update
    void Start()
    {
        thisSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position + new Vector3(dragOffsetX, dragOffsetY,0);
            transform.Translate(mousePosition);
        }

        //GLOW
        if(!isBeingHovered && !hasMovedAway)
        {
            thisSpriteRenderer.sprite = glowSprite;
        }
        else
        {
            thisSpriteRenderer.sprite = normalSPrite;
        }
    }
}
