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

    public bool ignoresColor;

    public ComparisonMode comparison_mode;

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

    IEnumerator DestroyMe()
    {
        yield return new WaitForEndOfFrame();
        Destroy(this.gameObject);
    }

    public void Damage(float amount, Color color)
    {
        if (ignoresColor)
        {
            health -= amount;
            if (anim)
            {
                anim.SetTrigger("Damage");
            }
        }
        else
        {
            float similarity = ColorComparison.singleton.CompareColors(comparison_mode, sprite.color, color);
            health -= amount * similarity;

            if (anim && similarity > 0)
            {
                anim.SetTrigger("Damage");
            }
        }

        if (health <= 0)
        {
            //Any event-driven effects we want
            OnDeath.Invoke();

            //Remove this object
            StartCoroutine(DestroyMe());

            //If it's the player, then we also need to end the game or restart the level!
            if (gameObject.tag.Equals("Player"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}
