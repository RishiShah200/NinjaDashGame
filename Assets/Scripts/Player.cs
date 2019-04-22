using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxSpeed = 3;
    public float speed = 50f;
    public float jumpPower = 150f;
    public static int jumpCount = 0;
    public bool grounded;
    public bool canDoubleJump;

    //Player Stats
    public int curHealth;
    public int maxHealth = 100;

    private Rigidbody2D rb2d;
    private Animator anim;
    private gameMaster gm;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();

        curHealth = maxHealth;
        gm = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<gameMaster>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("Grounded", grounded);
        anim.SetFloat("Speed", Mathf.Abs(rb2d.velocity.x));

        if (Input.GetAxis("Horizontal") < -0.1f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (Input.GetAxis("Horizontal") > 0.1f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (grounded)
            {
                rb2d.AddForce(Vector2.up * jumpPower);
                canDoubleJump = true;
            }
            else
            {
                if (canDoubleJump)
                {
                    canDoubleJump = false;
                    rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
                    rb2d.AddForce(Vector2.up * jumpPower / 1.2f);
                }
            }
        }

        if(curHealth > maxHealth)
        {
            curHealth = maxHealth;
        }

        if(curHealth <= 0)
        {
            Die();
        }

    }

    private void FixedUpdate()
    {
        Vector3 easeVelocity = rb2d.velocity;
        easeVelocity.y = rb2d.velocity.y;
        easeVelocity.z = 0.0f;
        easeVelocity.x *= .75f;
        float h = Input.GetAxis("Horizontal");
       
        //fake friction using x speed of the player
        if (grounded)
        {

            rb2d.velocity = easeVelocity;

        }

        //moving the player based on vector
        rb2d.AddForce((Vector2.right * speed) * h);
        //limits the max speed of the player
        if(rb2d.velocity.x > maxSpeed)
        {
            rb2d.velocity = new Vector2(maxSpeed, rb2d.velocity.y);
        }
        if(rb2d.velocity.x < -maxSpeed)
        {
            rb2d.velocity = new Vector2(-maxSpeed, rb2d.velocity.y);
        }
    }

    void Die()
    {

        Application.LoadLevel(Application.loadedLevel);
    }
    public void damage (int dmg)
    {
        curHealth -= dmg;
        gameObject.GetComponent<Animation>().Play("Player_RedFlash");
    }

    public IEnumerator Knockback(float knockDur, float knockbackPwr, Vector3 knockbackDir)
    {
        float timer = 0;
        rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
        while(knockDur > timer)
        {
            timer += Time.deltaTime;
            rb2d.velocity = new Vector2(0, 0);
            rb2d.AddForce(new Vector3(knockbackDir.x * -100, knockbackDir.y + knockbackPwr, transform.position.z));
        }
        yield return 0;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Coin"))
        {
            Destroy(col.gameObject);
            gm.points += 1;
        }
    }
}
