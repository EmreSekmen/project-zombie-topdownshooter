using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletScript : MonoBehaviour
{
    public float speed = 30f;
    private float lifeTime = 2f;
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    // Start is called before the first frame update
    void Start()
    {
       

       
    }

    // Update is called once per frame
    void Update()
    {
      
    }


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();


    }

    private void OnEnable()
    {
        Destroy(gameObject, lifeTime);


        if(rb != null)
        {
            rb.gravityScale = 0f;

            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.velocity = transform.right * speed * Time.deltaTime;


            return;
        }
    }

   

}
