using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoosingCarouselMenu : MonoBehaviour
{
    public Transform[] boxes;
    public float moveDuration = 0.5f;
    public float minAlpha = 0f;
    public float maxAlpha = 1f;
    public float normalScale = 0.5f;
    public float middleScale = 1f;
    public float scaleTransitionDuration = 0.2f;

    private bool isMoving = false;
    private int middleIndex = 0;
    private int previousMiddleIndex = 0;

    private Image[] boxImages;
    private RectTransform[] boxRects;
    private Color[] originalColors;
    private float[] originalAlphas;
    public CatManager[] cats;
    public CatManager middleCat;
    public GameObject[] choosingCatUI;
    public GameObject[] givingNameUI;

    void Start()
    {
        boxes = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            boxes[i] = transform.GetChild(i);
        }

        boxImages = new Image[boxes.Length];
        boxRects = new RectTransform[boxes.Length];
        originalColors = new Color[boxes.Length];
        originalAlphas = new float[boxes.Length];
        cats = new CatManager[boxes.Length];

        // Simpan komponen Image dan RectTransform serta warna asli dari setiap box
        for (int i = 0; i < boxes.Length; i++)
        {
            boxImages[i] = boxes[i].GetComponent<Image>();
            boxRects[i] = boxes[i].GetComponent<RectTransform>();
            originalColors[i] = boxImages[i].color;
            originalAlphas[i] = originalColors[i].a;
            cats[i] = boxes[i].GetComponent<CatManager>();
        }

        // Cari child object yang berada di tengah saat start
        FindMiddleIndex();

        // Atur skala child object yang berada di tengah ke skala middle
        StartCoroutine(ScaleBoxes());
        StartCoroutine(ScaleMiddleBox());
    }

    void Update()
    {
        middleCat = cats[middleIndex];
        if (Input.GetKeyDown(KeyCode.RightArrow) && !isMoving)
        {
            StartCoroutine(MoveBoxesRight());
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && !isMoving)
        {
            StartCoroutine(MoveBoxesLeft());
        }
    }

    public void MoveRight()
    {
        if (!isMoving)
        {
            StartCoroutine(MoveBoxesLeft());
        }
    }

    public void MoveLeft()
    {
        if (!isMoving)
        {
            StartCoroutine(MoveBoxesRight());
        }
    }

    void FindMiddleIndex()
    {
        int totalBoxes = boxes.Length;
        middleIndex = (totalBoxes - 1) / 2;
    }

    IEnumerator MoveBoxesLeft()
    {
        isMoving = true;

        previousMiddleIndex = middleIndex;

        // Geser posisi tengah ke child object sebelumnya
        middleIndex = (middleIndex - 1 + boxes.Length) % boxes.Length;

        // Gerakkan setiap box ke posisi box di bawahnya dan lakukan rescaling serta perubahan opacity secara bersamaan
        for (int i = 0; i < boxes.Length; i++)
        {
            int nextIndex = (i + 1) % boxes.Length;
            StartCoroutine(MoveAndScaleBoxSmoothly(boxes[i], boxes[nextIndex].position, i == middleIndex));
        }

        yield return new WaitForSeconds(moveDuration);

        // Update urutan box dalam hirarki
        for (int i = 0; i < boxes.Length; i++)
        {
            boxes[i].SetSiblingIndex(i);
        }

        isMoving = false;
    }

    IEnumerator MoveBoxesRight()
    {
        isMoving = true;

        previousMiddleIndex = middleIndex;

        // Geser posisi tengah ke child object setelahnya
        middleIndex = (middleIndex + 1) % boxes.Length;

        // Gerakkan setiap box ke posisi box di atasnya dan lakukan rescaling serta perubahan opacity secara bersamaan
        for (int i = boxes.Length - 1; i >= 0; i--)
        {
            int prevIndex = (i - 1 + boxes.Length) % boxes.Length;
            StartCoroutine(MoveAndScaleBoxSmoothly(boxes[i], boxes[prevIndex].position, i == middleIndex));
        }

        yield return new WaitForSeconds(moveDuration);

        // Update urutan box dalam hirarki
        for (int i = 0; i < boxes.Length; i++)
        {
            boxes[i].SetSiblingIndex(i);
        }

        isMoving = false;
    }

    IEnumerator MoveAndScaleBoxSmoothly(Transform box, Vector3 targetPosition, bool isMiddleBox)
    {
        float elapsedTime = 0f;
        Vector3 startPosition = box.position;
        Vector3 startScale = box.localScale;
        Vector3 targetScaleVector = isMiddleBox ? new Vector3(middleScale, middleScale, 1f) : new Vector3(normalScale, normalScale, 1f);

        while (elapsedTime < moveDuration)
        {
            float t = elapsedTime / moveDuration;
            box.position = Vector3.Lerp(startPosition, targetPosition, t);
            box.localScale = Vector3.Lerp(startScale, targetScaleVector, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        box.position = targetPosition;
        box.localScale = targetScaleVector;

        // Reset opacity sesuai dengan status box (tengah atau bukan)
        float opacity = isMiddleBox ? maxAlpha : 0.2f;
        SetBoxOpacity(box, opacity);
    }

    void SetBoxOpacity(Transform box, float alpha)
    {
        int boxIndex = box.GetSiblingIndex();
        Color newColor = originalColors[boxIndex];
        newColor.a = alpha;
        boxImages[boxIndex].color = newColor;
    }

    IEnumerator ScaleMiddleBox()
    {
        // Dapatkan child object yang berada di tengah saat ini
        Transform middleBox = boxes[middleIndex];

        // Simpan skala asli child object
        Vector3 originalScale = middleBox.localScale;

        // Tentukan skala target (middleScale)
        Vector3 targetScale = new Vector3(middleScale, middleScale, 1f);

        // Inisialisasi waktu yang diperlukan untuk perubahan skala
        float elapsedTime = 0f;

        // Melakukan perubahan skala secara perlahan menggunakan lerp
        while (elapsedTime < scaleTransitionDuration)
        {
            float t = elapsedTime / scaleTransitionDuration;

            // Menggunakan interpolasi sinusoidal untuk perubahan skala
            t = Mathf.Sin(t * Mathf.PI * 0.5f);

            // Interpolasi skala dari skala asli ke skala target
            middleBox.localScale = Vector3.Lerp(originalScale, targetScale, t);
            SetBoxOpacity(middleBox, 1f);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Mengatur skala target agar sesuai dengan skala middle
        middleBox.localScale = targetScale;
    }
    IEnumerator ScaleBoxes()
    {
        for (int i = 0; i < boxes.Length; i++)
        {
            Transform box = boxes[i];

            // Tentukan skala target berdasarkan apakah box adalah middle atau bukan
            Vector3 targetScale = (i == middleIndex) ? new Vector3(middleScale, middleScale, 1f) : new Vector3(normalScale, normalScale, 1f);

            // Simpan skala asli box
            Vector3 originalScale = box.localScale;

            // Inisialisasi waktu yang diperlukan untuk perubahan skala
            float elapsedTime = 0f;

            // Melakukan perubahan skala secara perlahan menggunakan lerp
            while (elapsedTime < scaleTransitionDuration)
            {
                float t = elapsedTime / scaleTransitionDuration;

                // Menggunakan interpolasi sinusoidal untuk perubahan skala
                t = Mathf.Sin(t * Mathf.PI * 0.5f);

                // Interpolasi skala dari skala asli ke skala target
                box.localScale = Vector3.Lerp(originalScale, targetScale, t);
                float opacity = i == middleIndex ? maxAlpha : 0.2f;
                SetBoxOpacity(box, opacity);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Mengatur skala target agar sesuai dengan skala middle atau normal
            box.localScale = targetScale;
        }
    }

    public void ChooseCat()
    {
        GameManager.instance.AssignCat(middleCat.catScriptable);
        Debug.Log(middleCat.catScriptable.animationFolderPath);
    }

    public void GiveNameUI()
    {
        foreach (GameObject UI in choosingCatUI)
        {
            UI.SetActive(false);
        }

        foreach (GameObject UI in givingNameUI)
        {
            UI.SetActive(true);
        }
    }

    public void BackChoosingUI()
    {
        foreach (GameObject UI in choosingCatUI)
        {
            UI.SetActive(true);
        }

        foreach (GameObject UI in givingNameUI)
        {
            UI.SetActive(false);
        }
    }

    public void NamingCat(InputField inputField)
    {
        GameManager.instance.CatName = inputField.text;
        Debug.Log(GameManager.instance.CatName);
    }
}
