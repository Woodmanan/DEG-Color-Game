using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Basic player controller - the main idea here is to use the RigidBody2D
 * to hook into the physics system, and let that do all the work of making
 * the character move right!
 */

//Quick Enum for testing the different gameplay modes
public enum AttackType
{
    MeleeNoCombo,
    MeleeCombo,
    RangedNoGrav,
    RangedGrav
}

//This tag forces a rigidbody to be on this object as well
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    //[SerializeField] exposes this variable to the editor
    [SerializeField] float speed = 1; //Speed controls the speed of the object
    [SerializeField] float jumpForce; //Force of the jump impulse
    [SerializeField] float groundDistForJump = 1f; //Distance for boxcast - how close you need to be to the ground to jump
    [SerializeField] AttackType attackType;
    [SerializeField] float rangedAttackSpawnDist;
    [SerializeField] float rangedAttackSpeed;
    [SerializeField] GameObject projectile;
    [SerializeField] float meleeAttackDamage;

    [Header("Melee Colliders")]
    [SerializeField] AttackZone leftAttack;
    [SerializeField] AttackZone rightAttack;

    public Color color;

    Coroutine openWindowRoutine;

    float jumpCooldown = 0f;

    public static PlayerController singleton;

    Rigidbody2D rigid;
    // Start is called before the first frame update
    void Start()
    {
        //Get the component from the main object (no null check, since it's forced to be there)
        rigid = GetComponent<Rigidbody2D>();

        if (singleton != this && singleton != null)
        {
            Destroy(singleton.gameObject);
        }
        singleton = this;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(openWindowRoutine == null);
        //Create new vector from X input
        Vector2 moveVelocity = new Vector2(Input.GetAxis("Horizontal"), 0);
        moveVelocity *= speed;

        //Add in existing Y velocity
        moveVelocity += Vector2.up * rigid.velocity;

        rigid.velocity = moveVelocity;

        //Jump code goes here!
        if (Input.GetKeyDown(KeyCode.W) && jumpCooldown <= 0)
        {
            if (IsGrounded())
            {
                rigid.AddForce(Vector2.up * jumpForce);
            }
        }

        //Gravity fix (doubles fall speed, makes falling less floaty)
        if (rigid.velocity.y < 0)
        {
            rigid.AddForce(Vector2.down * rigid.gravityScale, ForceMode2D.Force);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }

        if (Input.GetKey(KeyCode.Space))
        {
            if (openWindowRoutine == null)
            {
                openWindowRoutine = StartCoroutine(OpenWindow());
            }
        }
        else
        {
            //Fixes a weird timescale bug - we'll have to change this if we want more things freezing time
            Time.timeScale = 1;
        }
    }

    bool IsGrounded()
    {

        //Generate boxcast (like raycast, but catches corners - gives you leeway when you jump off the edge of a platform)
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one, 0, Vector2.down, groundDistForJump);
        if (hit.collider)
        {
            return true;
        }
        return false;
    }

    //Draws that nice green line that ma
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, Vector3.down * groundDistForJump);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.right * rangedAttackSpawnDist);
    }

    IEnumerator OpenWindow()
    {
        ColorSelectionUI.singleton.OpenSelector();
        yield return new WaitUntil(() => !Input.GetKey(KeyCode.Space));
        ColorSelectionUI.singleton.CloseSelector();
        openWindowRoutine = null;
    }

    public void Attack()
    {
        switch (attackType)
        {
            case AttackType.MeleeNoCombo:
                MeleeAttack(false);
                break;
            case AttackType.MeleeCombo:
                MeleeAttack(true);
                break;
            case AttackType.RangedNoGrav:
                FireProjectile(false);
                break;
            case AttackType.RangedGrav:
                FireProjectile(true);
                break;
        }
    }

    public void MeleeAttack(bool useCombo)
    {
        Vector3 mouseDir = GetMouseDir();
        if (mouseDir.x < 0)
        {
            leftAttack.Attack(useCombo, meleeAttackDamage, color);
        }
        else
        {
            rightAttack.Attack(useCombo, meleeAttackDamage, color);
        }
    }

    public void FireProjectile(bool useGravity)
    {
        Vector3 dir = GetMouseDir();
        Rigidbody2D rigid = Instantiate(projectile, transform.position + dir * rangedAttackSpawnDist, new Quaternion()).GetComponent<Rigidbody2D>();
        rigid.velocity = ((Vector2)dir) * rangedAttackSpeed;
        rigid.gravityScale = useGravity ? rigid.gravityScale : 0;
        rigid.GetComponent<SpriteRenderer>().color = color;
    }

    public Vector3 GetMouseDir()
    {
        Vector2 mouse = Input.mousePosition;
        Vector2 screen = Camera.main.WorldToScreenPoint(transform.position); //Bad code, but we don't need to be optimized
        float rot = Mathf.Atan2(mouse.y - screen.y, mouse.x - screen.x);//- 90 * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(rot), Mathf.Sin(rot), 0);
    }
}
