using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Basic player controller - the main idea here is to use the RigidBody2D
 * to hook into the physics system, and let that do all the work of making
 * the character move right!
 */

//This tag forces a rigidbody to be on this object as well
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    //[SerializeField] exposes this variable to the editor
    [SerializeField] float speed = 1; //Speed controls the speed of the object
    [SerializeField] float jumpForce; //Force of the jump impulse
    [SerializeField] float groundDistForJump = 1f; //Distance for boxcast - how close you need to be to the ground to jump

    Coroutine openWindowRoutine;

    float jumpCooldown = 0f;

    Rigidbody2D rigid;
    // Start is called before the first frame update
    void Start()
    {
        //Get the component from the main object (no null check, since it's forced to be there)
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(openWindowRoutine == null);
        //Create new vector from X input
        Vector2 moveVelocity = new Vector2(Input.GetAxis("Horizontal"), 0);
        moveVelocity *= speed;

        //Add in existing Y velocity
        moveVelocity += Vector2.up * rigid.velocity;

        rigid.velocity = moveVelocity;

        //Jump code goes here!
        if (Input.GetKeyDown(KeyCode.Space) && jumpCooldown <= 0)
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

        if (Input.GetMouseButton(0))
        {
            if (openWindowRoutine == null)
            {
                openWindowRoutine = StartCoroutine(OpenWindow());
            }
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
        Gizmos.DrawRay(transform.position, transform.position + Vector3.down * groundDistForJump);
    }

    IEnumerator OpenWindow()
    {
        for (float t = 0; t < .05f; t+= Time.deltaTime)
        {
            if (!Input.GetMouseButton(0))
            {
                openWindowRoutine = null;
                yield break;
            }
            yield return null;
        }

        ColorSelectionUI.singleton.OpenSelector();
        yield return new WaitUntil(() => (!Input.GetMouseButton(0)));
        ColorSelectionUI.singleton.CloseSelector();
        openWindowRoutine = null;
    }
}
