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
    public float wall_distance;
    private float detect_range = 6f;

    public Transform player;

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
        move();
    }

    // Follows player within a range, else patrols the area
    private void move() {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < detect_range){
            Debug.Log("FOLLOW");
            follow_player();
        }
        else
        {
            Debug.Log("PATROL");
            patrolling();
        }
    }

    private void patrolling()
    {
        RaycastHit2D hit_down = Physics2D.Raycast(rigid.position + Vector2.right, Vector3.down, platform_distance);
        RaycastHit2D hit_left = Physics2D.Raycast(rigid.position, Vector3.left, wall_distance);
        RaycastHit2D hit_right = Physics2D.Raycast(rigid.position, Vector3.right, wall_distance);

        // switch directions if detect edge or wall
        if (hit_down.collider == null || hit_left.collider != null || hit_right.collider != null) {
            speed *= -1;
        }

        rigid.velocity = new Vector3(speed, 0, 0);
    }

    private void follow_player()
    {
        RaycastHit2D hit_down = Physics2D.Raycast(rigid.position + Vector2.right, Vector3.down, platform_distance);

        if (hit_down.collider == null)
        {
            rigid.velocity = Vector2.zero;
        }
        else
        {
            Vector3 direction = player.position - transform.position;
            direction.Normalize();
            rigid.MovePosition(transform.position + (direction * speed * Time.deltaTime));
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, Vector3.down * platform_distance);
        Gizmos.DrawRay(transform.position, Vector3.left * wall_distance);
    }
}
