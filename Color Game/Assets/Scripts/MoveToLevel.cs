using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class MoveToLevel : MonoBehaviour
{
    [SerializeField] int LevelToMoveTo;
    public bool allowedToMove = false;
    List<Monster> monsters = new List<Monster>();

    // Start is called before the first frame update
    void Start()
    {
        monsters.AddRange(GameObject.FindGameObjectsWithTag("Monster").Select(x => x.GetComponent<Monster>()));
    }

    // Update is called once per frame
    void Update()
    {
        if (monsters.Count > 0)
        {
            for (int i = monsters.Count - 1; i >= 0; i--)
            {
                if (monsters[i] == null)
                {
                    monsters.RemoveAt(i);
                }
            }

            if (monsters.Count == 0)
            {
                allowedToMove = true;
                GetComponent<ParticleSystem>().Play();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (allowedToMove && collision.gameObject.tag.Equals("Player"))
        {
            SceneManager.LoadScene(LevelToMoveTo);
        }
    }
}
