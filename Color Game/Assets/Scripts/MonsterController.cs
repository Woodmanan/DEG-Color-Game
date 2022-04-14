using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This tag forces a rigidbody to be on this object as well
[RequireComponent(typeof(Rigidbody2D))]
public class MonsterController : MonoBehaviour
{
    public float speed = 3; //Speed controls the speed of the object
    public bool going_right = true;

    public float platform_distance;

    // public Color color;

    Rigidbody2D rigid;
    // Start is called before the first frame update
    void Start()
    {
        //Get the component from the main object (no null check, since it's forced to be there)
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(rigid.position + Vector2.right * 3, Vector3.down, platform_distance);
        Debug.Log(hit.collider != null);
        if (hit.collider != null) {
            rigid.velocity = new Vector3(speed, 0, 0);
        }
        else {
            rigid.velocity = Vector2.zero;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, Vector3.down * platform_distance);
    }
}
