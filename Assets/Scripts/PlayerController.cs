using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float maxSpeed = 2.0f;

    private Animator myAnimator;
    private Rigidbody2D myRigidbody2D;
    private bool facingRight = true;
    private float hSpeed;
    private float vSpeed;

    void Start ()
    {
        myAnimator = GetComponent<Animator>();
        myRigidbody2D = GetComponent<Rigidbody2D>();
        //FlipSprite();
    }
	
	void FixedUpdate ()
    {
        hSpeed = Input.GetAxis("Horizontal");
        vSpeed = Input.GetAxis("Vertical");

        myAnimator.SetFloat("hSpeed", Mathf.Abs(hSpeed));
        myAnimator.SetFloat("vSpeed", vSpeed);

        myRigidbody2D.velocity = new Vector2(hSpeed * maxSpeed, vSpeed * maxSpeed);

        if (hSpeed > 0 && !facingRight)
            FlipSprite();
        else if (hSpeed < 0 && facingRight)
            FlipSprite();
	}

    void FlipSprite()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
