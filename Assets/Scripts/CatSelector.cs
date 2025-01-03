using Spine.Unity;
using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CatSelector : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Transform[] boxes;
    public float moveDuration = 0.5f;
    // public float minAlpha = 0f;
    public float maxAlpha = 1f;
    public float normalScale = 0.5f;
    public float middleScale = 1f;
    public float scaleTransitionDuration = 0.2f;

    private bool isMoving = false;
    public int middleIndex = 0;
    public int previousMiddleIndex = 0;

    private Image[] boxImages;
    private RectTransform[] boxRects;
    private Color[] originalColors;
    private float[] originalAlphas;
    private Cat[] cats;
    private Cat middleCat;
    public GameObject[] choosingCatUI;
    public GameObject[] givingNameUI;
    public Image preview;
    public Button giveNameButton;
    public InputField inputField;

    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    public float swipeThreshold = 50f;
    // private Image[] catTypeBox;
    private Text[] catTypeTexts;
    private GameObject[] adoptedIcon;
    private CanvasGroup[] canvasGroup;
    public SkeletonGraphic giveNameSkeleton;
    public AudioClip swipeSFX;
    public AudioClip selectSFX;
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
        cats = new Cat[boxes.Length];
        canvasGroup = new CanvasGroup[boxes.Length];

        // catTypeBox = new Image[boxes.Length];
        catTypeTexts = new Text[boxes.Length];
        adoptedIcon = new GameObject[boxes.Length];

        // Simpan komponen Image dan RectTransform serta warna asli dari setiap box
        for (int i = 0; i < boxes.Length; i++)
        {
            boxImages[i] = boxes[i].GetComponent<Image>();
            boxRects[i] = boxes[i].GetComponent<RectTransform>();
            originalColors[i] = boxImages[i].color;
            originalAlphas[i] = originalColors[i].a;
            cats[i] = boxes[i].GetComponent<Cat>();
            canvasGroup[i] = boxes[i].GetComponent<CanvasGroup>();

            // catTypeBox[i] = null;
            catTypeTexts[i] = null;
            foreach (Transform child in boxes[i].transform)
            {
                if (child.GetComponent<Image>() != null)
                {
                    // catTypeBox[i] = child.GetComponent<Image>();
                    adoptedIcon[i] = child.GetChild(0).gameObject;
                }
                if (child.GetComponent<Text>() != null)
                {
                    catTypeTexts[i] = child.GetComponent<Text>();
                }
            }
            if (!string.IsNullOrEmpty(cats[i].catScriptable.name))
            {
                catTypeTexts[i].text = cats[i].catScriptable.name;
                adoptedIcon[i].SetActive(true);
            }
            else
            {
                adoptedIcon[i].SetActive(false);
            }
        }

        // PlayerPrefs.DeleteAll();
        // Cari child object yang berada di tengah saat start
        if (!PlayerPrefs.HasKey("middleIndex"))
        {
            FindMiddleIndex();
        }
        else
        {
            LoadTransforms();
            middleIndex = PlayerPrefs.GetInt("middleIndex", 2);
            previousMiddleIndex = PlayerPrefs.GetInt("previousMiddleIndex", 0);
        }

        // Atur skala child object yang berada di tengah ke skala middle
        //StartCoroutine(ScaleBoxes());
        //StartCoroutine(ScaleMiddleBox());
        BackChoosingUI(false);
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
        if (string.IsNullOrWhiteSpace(inputField.text) || !Regex.IsMatch(inputField.text, @"^(?=.*[a-zA-Z])[a-zA-Z0-9'\s]+$"))
        {
            giveNameButton.interactable = false;
        }
        else
        {
            giveNameButton.interactable = true;
        }
    }

    public void MoveRight()
    {
        if (!isMoving)
        {
            StartCoroutine(MoveBoxesRight());
            SFXManager.Instance.PlaySFX(SoundEffect.Swipe);
        }
    }

    public void MoveLeft()
    {
        if (!isMoving)
        {
            StartCoroutine(MoveBoxesLeft());
            SFXManager.Instance.PlaySFX(SoundEffect.Swipe);
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
        // for (int i = 0; i < boxes.Length; i++)
        // {
        //     boxes[i].SetSiblingIndex(i);
        // }

        isMoving = false;

        PlayerPrefs.SetInt("middleIndex", middleIndex);
        PlayerPrefs.SetInt("previousMiddleIndex", previousMiddleIndex);
        PlayerPrefs.Save();
        SaveTransforms();
        Debug.Log("Saving PlayerPrefs");
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
        // for (int i = 0; i < boxes.Length; i++)
        // {
        //     boxes[i].SetSiblingIndex(i);
        // }

        isMoving = false;

        PlayerPrefs.SetInt("middleIndex", middleIndex);
        PlayerPrefs.SetInt("previousMiddleIndex", previousMiddleIndex);
        PlayerPrefs.Save();
        SaveTransforms();
        Debug.Log("Saving PlayerPrefs");
    }

    IEnumerator MoveAndScaleBoxSmoothly(Transform box, Vector3 targetPosition, bool isMiddleBox)
    {
        float elapsedTime = 0f;
        Vector3 startPosition = box.position;
        //Vector3 startScale = box.localScale;
        //Vector3 targetScaleVector = isMiddleBox ? new Vector3(middleScale, middleScale, 1f) : new Vector3(normalScale, normalScale, 1f);

        while (elapsedTime < moveDuration)
        {
            float t = elapsedTime / moveDuration;
            box.position = Vector3.Lerp(startPosition, targetPosition, t);
            //box.localScale = Vector3.Lerp(startScale, targetScaleVector, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        box.position = targetPosition;
        //box.localScale = targetScaleVector;

        //float opacity = isMiddleBox ? maxAlpha : 0.4f;

        //SetBoxOpacity(box, opacity);
    }

    void SetBoxOpacity(Transform box, float alpha)
    {
        int boxIndex = box.GetSiblingIndex();
        // Color newColor = originalColors[boxIndex];
        // newColor.a = alpha;
        // boxImages[boxIndex].color = newColor;
        // // Get the child Image and Text components
        // Image childImage = catTypeBox[Array.IndexOf(boxes, box)];
        // Text childText = catTypeTexts[Array.IndexOf(boxes, box)];

        // // Set the child Image and Text alpha
        // if (childImage != null)
        // {
        //     Color childImageColor = childImage.color;
        //     childImageColor.a = alpha;
        //     childImage.color = childImageColor;
        // }

        // if (childText != null)
        // {
        //     Color childTextColor = childText.color;
        //     childTextColor.a = alpha;
        //     childText.color = childTextColor;
        // }
        canvasGroup[boxIndex].alpha = alpha;
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
                float opacity = i == middleIndex ? maxAlpha : 0.4f;
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
        GameManager.instance.CatProfile = middleCat;
        // Debug.Log(middleCat.catScriptable.animationFolderPath);
    }

    public void GiveNameUI()
    {
        if (GameManager.instance.CatProfile.catScriptable.state == CatState.Unnamed)
        {
            // Sprite sprite;
            // if (GameManager.instance.CatProfile.catScriptable.phase == CatPhase.Baby)
            // {
            //     sprite = Resources.Load<Sprite>(GameManager.instance.CatProfile.catScriptable.spriteFolderPath + "baby");
            // }
            // else if (GameManager.instance.CatProfile.catScriptable.phase == CatPhase.Child)
            // {
            //     sprite = Resources.Load<Sprite>(GameManager.instance.CatProfile.catScriptable.spriteFolderPath + "child");
            // }
            // else if (GameManager.instance.CatProfile.catScriptable.phase == CatPhase.Adult)
            // {
            //     sprite = Resources.Load<Sprite>(GameManager.instance.CatProfile.catScriptable.spriteFolderPath + "adult");
            // }
            // else
            // {
            //     sprite = preview.sprite;
            // }
            // preview.sprite = sprite;
            GameEvents.GiveName(middleCat.catScriptable.phase, middleCat.catScriptable.id);
            // Debug.Log("id skin = " + middleCat.catScriptable.id);
            giveNameSkeleton.Skeleton.SetSkin(middleCat.catScriptable.id.ToString());
            foreach (GameObject UI in choosingCatUI)
            {
                UI.SetActive(false);
            }

            inputField.text = catTypeTexts[middleIndex].text;
            foreach (GameObject UI in givingNameUI)
            {
                UI.SetActive(true);
            }
            SFXManager.Instance.PlaySFX(SoundEffect.Select);
        }
        else
        {
            SceneLoader.LoadSceneStatic("MainPage");
        }
    }

    public void BackChoosingUI(bool sfx = true)
    {
        foreach (GameObject UI in choosingCatUI)
        {
            UI.SetActive(true);
        }

        foreach (GameObject UI in givingNameUI)
        {
            UI.SetActive(false);
        }
        if (sfx)
        {
            SFXManager.Instance.PlaySFX(SoundEffect.Back);
        }
    }

    public void NamingCat()
    {
        // GameManager.instance.CatProfile.catScriptable.name = inputField.text.Trim();
        // GameManager.instance.CatProfile.catScriptable.Save();
        GameManager.instance.GiveName(inputField.text.Trim());
        // Debug.Log(GameManager.instance.CatProfile.name);
        // Debug.Log(GameManager.instance.CatProfile.state);
    }

    void OnDisable()
    {
        if (GameManager.instance.currentCatName != null)
        {
            Debug.Log("Cat Tidak Null");
            GameManager.instance.previousCatName = GameManager.instance.currentCatName;
        }
        else
        {
            Debug.Log("Cat Null");
        }
        if (GameManager.instance.CatProfile != null)
        {
            GameManager.instance.currentCatName = GameManager.instance.CatProfile.catScriptable.name;
        }
    }

    void SaveTransforms()
    {
        for (int i = 0; i < boxes.Length; i++)
        {
            // Save position
            PlayerPrefs.SetFloat("Box_" + i + "_PosX", boxes[i].position.x);
            PlayerPrefs.SetFloat("Box_" + i + "_PosY", boxes[i].position.y);
            PlayerPrefs.SetFloat("Box_" + i + "_PosZ", boxes[i].position.z);

            // Save rotation
            PlayerPrefs.SetFloat("Box_" + i + "_RotX", boxes[i].rotation.eulerAngles.x);
            PlayerPrefs.SetFloat("Box_" + i + "_RotY", boxes[i].rotation.eulerAngles.y);
            PlayerPrefs.SetFloat("Box_" + i + "_RotZ", boxes[i].rotation.eulerAngles.z);

            // Save scale
            PlayerPrefs.SetFloat("Box_" + i + "_ScaleX", boxes[i].localScale.x);
            PlayerPrefs.SetFloat("Box_" + i + "_ScaleY", boxes[i].localScale.y);
            PlayerPrefs.SetFloat("Box_" + i + "_ScaleZ", boxes[i].localScale.z);
        }

        for (int i = 0; i < canvasGroup.Length; i++)
        {
            float alpha = canvasGroup[i].alpha;
            PlayerPrefs.SetFloat($"CanvasGroupAlpha_{i}", alpha);
        }

        PlayerPrefs.Save();
    }

    void LoadTransforms()
    {
        Debug.Log("Load Transform");
        for (int i = 0; i < boxes.Length; i++)
        {
            // Load position
            float posX = PlayerPrefs.GetFloat("Box_" + i + "_PosX", boxes[i].position.x);
            float posY = PlayerPrefs.GetFloat("Box_" + i + "_PosY", boxes[i].position.y);
            float posZ = PlayerPrefs.GetFloat("Box_" + i + "_PosZ", boxes[i].position.z);
            boxes[i].position = new Vector3(posX, posY, posZ);

            // Load rotation
            float rotX = PlayerPrefs.GetFloat("Box_" + i + "_RotX", boxes[i].rotation.eulerAngles.x);
            float rotY = PlayerPrefs.GetFloat("Box_" + i + "_RotY", boxes[i].rotation.eulerAngles.y);
            float rotZ = PlayerPrefs.GetFloat("Box_" + i + "_RotZ", boxes[i].rotation.eulerAngles.z);
            boxes[i].rotation = Quaternion.Euler(rotX, rotY, rotZ);

            // Load scale
            float scaleX = PlayerPrefs.GetFloat("Box_" + i + "_ScaleX", boxes[i].localScale.x);
            float scaleY = PlayerPrefs.GetFloat("Box_" + i + "_ScaleY", boxes[i].localScale.y);
            float scaleZ = PlayerPrefs.GetFloat("Box_" + i + "_ScaleZ", boxes[i].localScale.z);
            boxes[i].localScale = new Vector3(scaleX, scaleY, scaleZ);
        }

        for (int i = 0; i < canvasGroup.Length; i++)
        {
            float alpha = PlayerPrefs.GetFloat($"CanvasGroupAlpha_{i}");
            canvasGroup[i].alpha = alpha;
        }
    }
    // private void OnApplicationQuit()
    // {
    //     PlayerPrefs.DeleteAll();
    // }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Debug.Log("Touched");
        startTouchPosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // You can track the dragging movement here if needed
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Debug.Log("PointerUp");
        endTouchPosition = eventData.position;
        DetectSwipeDirection();
    }

    private void DetectSwipeDirection()
    {
        Vector2 swipeVector = endTouchPosition - startTouchPosition;
        // Debug.Log(swipeVector.magnitude);
        if (swipeVector.magnitude >= swipeThreshold)
        {
            float xDifference = Mathf.Abs(swipeVector.x);
            float yDifference = Mathf.Abs(swipeVector.y);
            // Debug.Log("xDifference: " + xDifference + "| yDifference: " + yDifference);

            if (xDifference > yDifference)
            {
                // Debug.Log("swipeVector.x");
                if (swipeVector.x > 0)
                {
                    MoveLeft();
                }
                else
                {
                    MoveRight();
                }
            }
        }
    }
}
