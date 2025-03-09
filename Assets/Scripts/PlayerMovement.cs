using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Attributes")]

    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;

    [SerializeField] private float dashPower;
    [SerializeField] private float dashDuration;


    [Header("References")]

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private BoxCollider2D collider;

    [Header("Layers")]

    [SerializeField] private LayerMask groundLayer;

    private float horizontalInput;
    private bool dashing = false;

  
    

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

        //PLAYER MOVEMENT

        if(!dashing)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);


            //PLAYER DASH

            if (Input.GetKeyDown(KeyCode.Mouse1)) //On right click
            {
                StartCoroutine(Dash());
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

        if (IsOnGround())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            }
        }

       
       
    }

    private bool IsOnGround() //CHECKS IF GROUND IS DIRECTLY BELOW THE PLAYER RETURNING TRUE OR FALSE
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(collider.bounds.center, collider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);

        return raycastHit.collider != null; 
    }

    private IEnumerator Dash() //THE PLAYER DASHES IN THE DIRECTION THEY ARE FACING
    {
        dashing = true;

        float dashDirection = Mathf.Sign(transform.localScale.x);

        rb.velocity = new Vector2(speed * dashPower * dashDirection, rb.velocity.y);

        yield return new WaitForSeconds(dashDuration);

        dashing = false;
    }
}
