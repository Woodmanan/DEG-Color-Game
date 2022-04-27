using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubZone : MonoBehaviour
{
    public List<GameObject> touching;
    SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack(float damage, Color color)
    {
        foreach (GameObject g in touching)
        {
            Debug.Log(g.name);
            Monster monster;
            if ((monster = g.GetComponent<Monster>()))
            {
                monster.Damage(damage, color);
            }
        }

        StartCoroutine(ShowAttack(color));
    }

    IEnumerator ShowAttack(Color color)
    {
        sprite.enabled = true;
        sprite.color = color;
        yield return new WaitForSeconds(.2f);
        sprite.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Zone touched a new thing!");
        touching.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        touching.Remove(collision.gameObject);
    }
}
