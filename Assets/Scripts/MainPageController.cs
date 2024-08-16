using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPageController : MonoBehaviour
{
    public Text catName;
    public Text catLevel;
    void Start()
    {
        catName.text = GameManager.instance.CatName;
        catLevel.text = GameManager.instance.CatLevel.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
