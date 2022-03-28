using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSelectionUI : MonoBehaviour
{
    public static ColorSelectionUI singleton;
    Animator anim;
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
        anim.SetTrigger("Close");
        Time.timeScale = 1;
    }

    public void FreezeTime()
    {
        Time.timeScale = 0;
    }
}
