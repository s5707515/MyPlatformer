using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Attributes")]

    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float baseGravityScale;

    [SerializeField] private float dashPower;
    [SerializeField] private float dashDuration;

    [SerializeField] private float wallBounceForce;


    [Header("References")]

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private BoxCollider2D collider;

    [Header("Layers")]

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    private float horizontalInput;

    private bool dashing = false;
    private bool canDash = true;



    private bool wallJumping = false;

    // Update is called once per frame
    void Update()
    {

        //PLAYER MOVEMENT

        if(!dashing && !wallJumping)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);


            //PLAYER DASH

            if(canDash)
            {
                if (Input.GetKeyDown(KeyCode.Mouse1)) //On right click
                {
                    StartCoroutine(Dash());
                }
            }
            
        }

        //PlAYER FACING DIRECTION

        Vector3 faceScale = transform.localScale;

        if (horizontalInput > 0.01f)
        {
            faceScale.x = Mathf.Abs(faceScale.x);
        }
        else if (horizontalInput < -0.01f)
        {
            faceScale.x = -Mathf.Abs(faceScale.x);
        }

        transform.localScale = faceScale;


        //PLAYER JUMP MECHANIC

        if (IsOnGround() && !IsOnWall())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            }

            canDash = true; //refresh dash ability when on ground
        }

        // WALL JUMPING

        if(IsOnWall())
        {
            if(!wallJumping)
            {
                rb.velocity = Vector2.zero;
                rb.gravityScale = 0;
            }
          

            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(WallJump());
            }

        }
        else
        {
            rb.gravityScale = baseGravityScale;
        }

       
       
    }

    private bool IsOnGround() //CHECKS IF GROUND IS DIRECTLY BELOW THE PLAYER RETURNING TRUE OR FALSE
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(collider.bounds.center, collider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);

        return raycastHit.collider != null; 
    }

    private bool IsOnWall() //CHECKS IF GROUND IS DIRECTLY BELOW THE PLAYER RETURNING TRUE OR FALSE
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(collider.bounds.center, collider.bounds.size, 0,
            new Vector2(Mathf.Sign(rb.velocity.x),0), 0.1f, wallLayer);

        return raycastHit.collider != null;
    }

    private IEnumerator Dash() //THE PLAYER DASHES IN THE DIRECTION THEY ARE FACING
    {
        dashing = true;

        float dashDirection = Mathf.Sign(transform.localScale.x);

        rb.velocity = new Vector2(speed * dashPower * dashDirection, rb.velocity.y);

        yield return new WaitForSeconds(dashDuration);

        dashing = false;

        canDash = false; 
    }

  
    

    private IEnumerator WallJump() //PUSHES THE PLAYER UP AND AWAY FROM A WALL
    {
        wallJumping = true;

        rb.gravityScale = baseGravityScale;

        float jumpDirection = -Mathf.Sign(transform.localScale.x);

        rb.velocity = new Vector2(jumpDirection * wallBounceForce, jumpPower);


        yield return new WaitForSeconds(0.3f);

        wallJumping = false;

      
    }
}
