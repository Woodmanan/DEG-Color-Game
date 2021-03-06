using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This tag forces a rigidbody to be on this object as well
[RequireComponent(typeof(Rigidbody2D))]
public class MonsterController : MonoBehaviour
{
    Animator animator;
    public float speed = 3; //Speed controls the speed of the object
    public bool going_right = true;
    public float platform_distance;
    public float wall_distance;
    private float detect_range = 6f;
    public float rangedAttackSpawnDist;
    public float rangedAttackSpeed;
    public GameObject projectile;
    public Transform player;
    public float fireRate = 0.5f;
    public float nextFire = 0f;

    public Color color;

    Rigidbody2D rigid;
    // Start is called before the first frame update
    void Start()
    {
        //Get the component from the main object (no null check, since it's forced to be there)
        rigid = GetComponent<Rigidbody2D>();
        //rigid.velocity = Vector2.right * speed;
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < detect_range){
            follow_player();
            
            if(Time.time > nextFire) {
                nextFire = Time.time + fireRate;
                Attack();
            }
        }
        else
        {
            patrolling();
        }


        if (rigid.velocity.x >= 0)
        {
            going_right = true;
        }
        else
        {
            going_right = false;
        }

        animator?.SetBool("Moving_Right", going_right);

    }

    private void patrolling()
    {
        RaycastHit2D hit_down = Physics2D.Raycast(rigid.position + Vector2.right * Mathf.Sign(rigid.velocity.x) * wall_distance, Vector3.down, platform_distance);
        RaycastHit2D hit_left = Physics2D.Raycast(rigid.position, Vector2.left, wall_distance);
        RaycastHit2D hit_right = Physics2D.Raycast(rigid.position, Vector2.right, wall_distance);

        // switch directions if detect edge or wall
        if (hit_down.collider == null || hit_left.collider != null || hit_right.collider != null) {
            rigid.velocity = new Vector3(-Mathf.Sign(rigid.velocity.x) * speed, 0);
        }
    }

    private void follow_player()
    {
        RaycastHit2D hit_down = Physics2D.Raycast(rigid.position + Vector2.right * Mathf.Sign(rigid.velocity.x) * wall_distance, Vector3.down, platform_distance);

        // stop if about to fall off platform
        if (hit_down.collider == null)
        {
            rigid.velocity = Vector2.zero;
        }
        else
        {
            Vector3 direction = player.position - transform.position;
            //This returns a vector - doesn't modify the current instance
            //direction.Normalize(); 

            //Old code moved position manually, which made it right the rigidbody and incorrectly check hit_down.
            //rigid.MovePosition(transform.position + (direction.normalized * speed * Time.deltaTime));

            rigid.velocity = rigid.velocity * Vector2.up + speed * Mathf.Sign(direction.x) * Vector2.right;
        }

        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, Vector3.down * platform_distance);
        Gizmos.DrawRay(transform.position, Vector3.left * wall_distance);
    }

    public void Attack()
    {
        FireProjectile(false);
    }

    public void FireProjectile(bool useGravity)
    {
        Vector3 dir = player.position - transform.position;
        // dir = dir.Normalize();
        Rigidbody2D rigid = Instantiate(projectile, transform.position + dir.normalized * rangedAttackSpawnDist, new Quaternion()).GetComponent<Rigidbody2D>();
        rigid.velocity = ((Vector2)dir).normalized * rangedAttackSpeed;
        rigid.gravityScale = useGravity ? rigid.gravityScale : 0;
        rigid.GetComponent<SpriteRenderer>().color = GetComponent<SpriteRenderer>().color;
    }
}
