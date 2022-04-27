using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorSelectionUI : MonoBehaviour
{
    public static ColorSelectionUI singleton;
    Animator anim;
    public Color color;
    public Image chosenColorImage;
    //public float timeScale = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        if (singleton != null)
        {
            Destroy(singleton.gameObject);
        }
        singleton = this;
        anim = GetComponent<Animator>();
        RectTransform trans = GetComponent<RectTransform>();
        trans.localScale = new Vector3(0, 0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenSelector()
    {
        anim.SetTrigger("Open");
    }

    public void CloseSelector()
    {
        Debug.Log("Close");
        anim.SetTrigger("Close");
        Time.timeScale = 1;
        PlayerController.singleton.color = color;
    }

    public void CloseSelectorNoLock()
    {
        anim.SetTrigger("Close");
        Time.timeScale = 1;
    }

    public void FreezeTime()
    {
        Time.timeScale = 0;
    }

    public void SetColor(float rotation)
    {
        color = Color.HSVToRGB(rotation / 360, 1, 1);
        chosenColorImage.color = color;
    }
}
