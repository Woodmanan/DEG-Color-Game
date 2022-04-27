using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimAtMouse : MonoBehaviour
{
    RectTransform rectTransform;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouse = Input.mousePosition;
        Vector2 screen = new Vector2(Screen.width, Screen.height) / 2;
        float rot = Mathf.Atan2(mouse.y - screen.y, mouse.x - screen.x) * Mathf.Rad2Deg - 90;
        rectTransform.rotation = Quaternion.Euler(0, 0, rot);
        ColorSelectionUI.singleton.SetColor(Mathf.Round(((rot + 360) % 360) / 12) * 12);
    }
}
