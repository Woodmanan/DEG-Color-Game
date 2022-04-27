using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SpriteRenderer))]
public class Monster : MonoBehaviour
{
    public float maxHealth;
    public float health;
    SpriteRenderer sprite;
    Animator anim;

    [SerializeField] UnityEvent OnDeath;


    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        health = maxHealth;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(float amount, Color color)
    {
        health -= amount * ColorComparison.singleton.CompareColors(sprite.color, color);
        if (anim)
        {
            anim.SetTrigger("Damage");
        }
        if (health <= 0)
        {
            //Any event-driven effects we want
            OnDeath.Invoke();

            //Remove this object
            Destroy(this.gameObject);

            //If it's the player, then we also need to end the game or restart the level!
            if (gameObject.tag.Equals("Player"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}
