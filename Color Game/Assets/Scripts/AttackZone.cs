using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackZone : MonoBehaviour
{
    [HideInInspector] public List<SubZone> zones;
    int currentCombo = 0;
    [SerializeField] float timeToReset = .4f;
    float currentResetTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        zones = new List<SubZone>();
        for (int i = 0; i < transform.childCount; i++)
        {
            zones.Add(transform.GetChild(i).GetComponent<SubZone>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentResetTime > 0)
        {
            currentResetTime -= Time.deltaTime;
            if (currentResetTime <= 0)
            {
                Debug.Log("Combo timed out!");
                currentResetTime = 0;
                currentCombo = 0;
            }
        }
    }

    public void Attack(bool useCombo, float damage, Color color)
    {
        Debug.Log($"Attacking {zones[currentCombo].touching.Count} monsters!");
        zones[currentCombo].Attack(damage, color);

        if (useCombo)
        {
            currentCombo = (currentCombo + 1) % zones.Count;
            if (currentCombo != 0)
            {
                currentResetTime = timeToReset;
            }
            else
            {
                currentResetTime = 0;
            }
        }
    }
}
