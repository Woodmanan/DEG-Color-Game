using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Monster : MonoBehaviour
{
    public float maxHealth;
    public float health;
    SpriteRenderer sprite;
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(float amount, Color color)
    {

    }
}
