using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Projectile : MonoBehaviour
{
    public float damagePerHit;
    SpriteRenderer sprite;
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.magnitude > 250)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Projectile>())
        {
            return;
        }
        Monster monster;
        if ((monster = collision.gameObject.GetComponent<Monster>()))
        {
            Debug.Log($"Monster is {monster}");
            monster.Damage(damagePerHit, sprite.color);
        }
        Destroy(this.gameObject);
    }
}
