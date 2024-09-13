using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Shower : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 originalPosition;
    private RectTransform rectTransform;
    private Canvas canvas;


    public Image m_Image;
    public Sprite[] m_SpriteArray;
    public float m_Speed = .02f;
    private int m_IndexSprite;
    Coroutine m_CorotineAnim;
    bool IsAnimDone;
    public Sprite transparentImg;

    public BathController bathController;

    private float holdTime = 2.0f;
    private float holdTimer = 0f;

    private bool isOverPet = false;
    private float minusYRaycast = 1.5f;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        originalPosition = rectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Optional: Do something when drag starts (e.g., play a sound)
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

        // Convert the screen position to a world position
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Adjust the position to be 80 units below the mouse position in the Y-axis
        Vector2 adjustedPosition = new Vector2(mouseWorldPosition.x, mouseWorldPosition.y - minusYRaycast);

        // // Perform a raycast
        // RaycastHit2D hit = Physics2D.Raycast(adjustedPosition, Vector2.zero);

        // IsAnimDone = false;
        // m_CorotineAnim = StartCoroutine(Func_PlayAnimUI());
        // if (hit.collider != null && hit.collider.CompareTag("Foam"))
        // {
        //     Destroy(hit.collider.gameObject);
        // }

        // Perform a raycast with a larger radius
        RaycastHit2D[] hits = Physics2D.CircleCastAll(adjustedPosition, 1, Vector2.zero);

        IsAnimDone = false;
        m_CorotineAnim = StartCoroutine(Func_PlayAnimUI());

        // Iterate over all hit objects
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.collider.CompareTag("Pet") && !bathController.IsWet)
            {
                isOverPet = true;
            }

            if (bathController.IsWet && bathController.IsSoapy)
            {
                if (hit.collider != null && hit.collider.CompareTag("Foam"))
                {
                    Destroy(hit.collider.gameObject);

                }
            }
        }
    }

    private void Update()
    {
        if (isOverPet)
        {
            holdTimer += Time.deltaTime;
            if (holdTimer >= holdTime)
            {
                bathController.IsWet = true;
                Debug.Log(bathController.IsWet);
                holdTimer = 0f;
            }
        }


        if (bathController.IsSoapy && !bathController.IsShowered && CountFoamObjects() == 0)
        {
            bathController.IsShowered = true;
        }

        if (IsAnimDone)
        {
            m_Image.sprite = transparentImg;
        }
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition = originalPosition;
        IsAnimDone = true;
        StopCoroutine(m_CorotineAnim);
        isOverPet = false;
        holdTimer = 0f;
    }

    public int CountFoamObjects()
    {
        int foamCount = 0;
        GameObject[] foams = GameObject.FindGameObjectsWithTag("Foam");
        foamCount = foams.Length;
        Debug.Log(foamCount);
        return foamCount;
    }

    IEnumerator Func_PlayAnimUI()
    {
        yield return new WaitForSeconds(m_Speed);
        if (m_IndexSprite >= m_SpriteArray.Length)
        {
            m_IndexSprite = 0;
        }
        m_Image.sprite = m_SpriteArray[m_IndexSprite];
        m_IndexSprite += 1;
        if (IsAnimDone == false)
            m_CorotineAnim = StartCoroutine(Func_PlayAnimUI());
    }
}
