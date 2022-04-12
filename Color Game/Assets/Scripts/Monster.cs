using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
public class Monster : MonoBehaviour
{
    public float maxHealth;
    public float health;
    SpriteRenderer sprite;

    [SerializeField] UnityEvent OnDeath;


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
        health -= amount * ColorComparison.singleton.CompareColors(sprite.color, color);
        if (health <= 0)
        {
            OnDeath.Invoke();
        }
    }
}
