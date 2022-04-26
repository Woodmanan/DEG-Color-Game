using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    Monster monster;
    [SerializeField] Image barImage;

    // Start is called before the first frame update
    void Start()
    {
        monster = transform.parent.GetComponent<Monster>();
    }

    // Update is called once per frame
    void Update()
    {
        barImage.fillAmount = monster.health / monster.maxHealth;
    }
}
