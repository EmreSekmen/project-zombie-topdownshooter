using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Item[] Inventory = new Item[5];
    private InventoryUI InventoryUI;

    public float speed = 3f;
    public float verticalInput;
    public float HorizıntalInput;
    private Animator animator;

    public Transform firePoint;

    public bool isFacingRight = true;

    public Vector2 lastAimDir { get; private set; } = Vector2.right;

    public int Health = 100;

    public TextMeshProUGUI HealthText;

    public CameraScript CameraScript;

    // Start is called before the first frame update
    void Start()
    {
      animator = GetComponent<Animator>();
        InventoryUI = FindAnyObjectByType<InventoryUI>();
        Debug.Log("InventoryUI bulundu mu? → " + InventoryUI);


        for (int i = 0; i < Inventory.Length; i++)
        {
            Inventory[i] = null;
        }

    }

    // Update is called once per frame
    void Update()
    {
        PlayerControllerr();
        Animation();
        HealthUI();
    }



    void PlayerControllerr()
    {

        Vector2 move = new Vector2(HorizıntalInput, verticalInput);

        if(move != Vector2.zero)
        {
            lastAimDir = move.normalized;
            transform.Translate(move.normalized * speed * Time.deltaTime);


        }


        firePoint.right = lastAimDir;
       



        verticalInput = Input.GetAxis("Vertical");
        HorizıntalInput = Input.GetAxis("Horizontal");

        if(HorizıntalInput > 0)
        {
            isFacingRight = true;
        }
        else if(HorizıntalInput < 0)
        {
            isFacingRight = false;
        }


            transform.Translate(Vector3.up * speed * verticalInput * Time.deltaTime);
        transform.Translate(Vector3.right * speed * HorizıntalInput * Time.deltaTime);


        if(transform.position.x > 12.51202f)
        {
            transform.position = new Vector3(12.51202f, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < -12.51202f)
        {
            transform.position = new Vector3(-12.51202f, transform.position.y, transform.position.z);
        }
        else if (transform.position.y > 4.410937f)
        {
            transform.position = new Vector3(transform.position.x, 4.410937f, transform.position.z);
        }
        else if (transform.position.y < -4.410937f)
        {
            transform.position = new Vector3(transform.position.x, -4.410937f, transform.position.z);
        }


      

    }


    void Animation()
    {
        animator.SetFloat("speed", Mathf.Abs(verticalInput) + Mathf.Abs(HorizıntalInput));
        animator.SetFloat("MoveY", verticalInput);
        animator.SetFloat("MoveX", HorizıntalInput);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(20);
            StartCoroutine(CameraScript.Shake());
            StartCoroutine(TakingDamageNerf());
        }


      
    }

    public void TakeDamage(int Damage)
    {

        Health -= Damage;


        if(Health <= 0)
        {
            Die();
        }


    }

    void HealthUI()
    {
        HealthText.text = $"Health: {Health} / 100";
    }


    void Die()
    {
        Destroy(gameObject);
    }


    void MedKit(int amount)
    {
        Health += amount;

        if(Health > 100)
        {
            Health = 100;
        }

        
        HealthUI();
        
    }


   public IEnumerator TakingDamageNerf()
    {
        speed = 1f;
        yield return new WaitForSeconds(0.5f);
        speed = 3f;
    }


    public void AddItem(Item newItem)
    {
        for(int i = 0; i < Inventory.Length; i++)
        {
            Debug.Log("Inventory check: " + i + " = " + Inventory[i]);

            if (Inventory[i] == null)
            {
                Inventory[i] = newItem;
                InventoryUI.SetItem(i, newItem.icon);

                Debug.Log(newItem.itemName + "Slot" + i + "İçine eklendi");
                return;
            }
        }
        Debug.Log("İnventory Dolu!");
    }
}
