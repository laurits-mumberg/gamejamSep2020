using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsertObj : MonoBehaviour, IInteractableObj
{
    public bool isMoving = false;
    public bool isPlacedCorrectly = false;
    private bool isBeingHovered = false;

    public BoxCollider2D boxCollider2D;

    public SpriteRenderer thisSpriteRenderer;
    public Sprite glowSprite;
    public Sprite normalSPrite;

    public float dragOffsetX;
    public float dragOffsetY;

    public float allowedLenToGoal;
    public GameObject goalPosObj;
    public Vector2 targetPos;

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

        if (!isPlacedCorrectly && Vector2.Distance(targetPos, new Vector2(transform.position.x, transform.position.y)) <= allowedLenToGoal)
        {
            isPlacedCorrectly = true;
            transform.position = new Vector3(targetPos.x, targetPos.y, transform.position.z);
            GetComponent<Animator>().enabled = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        thisSpriteRenderer = GetComponent<SpriteRenderer>();
        targetPos = goalPosObj.transform.position;
        isPlacedCorrectly = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving && !isPlacedCorrectly)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position + new Vector3(dragOffsetX, dragOffsetY, 0);
            transform.Translate(mousePosition);
        }

        //GLOW
        if (!isBeingHovered && !isPlacedCorrectly)
        {
            thisSpriteRenderer.sprite = glowSprite;
        }
        else
        {
            thisSpriteRenderer.sprite = normalSPrite;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
    }
}
