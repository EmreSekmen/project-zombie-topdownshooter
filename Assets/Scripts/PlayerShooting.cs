using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
public class PlayerShooting : MonoBehaviour
{
    [Header("Weapon Attach")]
    public GameObject firePoint;
    public Transform firePointt;
    public Transform handTransform;
    private GameObject heldInstance;


    [Header("Weapon State")]
    public GameObject[] Ammo;
    public float Bulletspeed = 30f;
    public bool hasWeapon = false;
    private int pistolAmmo = 14;
    private string currentWeapon = "";




    [Header("Fire Rate")]
    private float fireRate = 2f;
    private float nextFireTime = 0f;


  
    public CameraScript cameraScript;
    public Transform weaponPivot;
    public PlayerController playerControllerSc;

    public SpriteRenderer muzzleFlash;

    // fireRate deðiþkenleri



    PlayerControls control;
    private bool isGamepadShooting;
    private bool isOutOfAmmo;


    public TextMeshProUGUI ammoText;

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame

    private void Awake()
    {
        control = new PlayerControls();

        control.Player.Shoot.performed += ctx => isGamepadShooting = true;
        control.Player.Shoot.canceled += ctx => isGamepadShooting = false;

        control.Player.Reload.performed += ctx => isOutOfAmmo = true;
        control.Player.Reload.canceled += ctx => isOutOfAmmo = false;

    }

    private void OnEnable()
    {
        control.Enable();
    }
    private void OnDisable()
    {
        control.Disable();
    }


    void Update()
    {




        PlayerShootingUI();
        UpdateAmmoUI();

    }


    void PlayerShootingUI()
    {
        if ((Input.GetKey(KeyCode.Space) || isGamepadShooting) && Time.time >= nextFireTime && pistolAmmo > 0)
        {
            TryShoot();
            pistolAmmo--;

            nextFireTime = Time.time + fireRate;
        }
        else if (pistolAmmo <= 0)

        {
            Debug.Log("Mermin bitti! þarjör deðiþtirmek için R'ye bas");
        }

        if ((pistolAmmo < 14 && Input.GetKeyDown(KeyCode.R)) || isOutOfAmmo)
        {
            pistolAmmo = 14;

        }

        if (playerControllerSc.isFacingRight)
        {
            weaponPivot.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            weaponPivot.localScale = new Vector3(-1, 1, 1);
        }
    }

    void UpdateAmmoUI()
    {
        if (!hasWeapon)
        {
            ammoText.text = "";
            return;
        }
        else
        {
            ammoText.text = $"{pistolAmmo} /14";
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Pistol"))
        {
            currentWeapon = "Pistol";
            hasWeapon = true;
            Debug.Log("Pistol Aldýn");
        }
    }



    public void TryShoot()
    {
        if (!hasWeapon)
        {
            Debug.Log("Silahýn yok ateþ edemezsin");
            return;
        }
        else
        {
            Shoot();
            Debug.Log("Gamepad Shooting: " + isGamepadShooting);

        }
    }

    public void EquipWeapon(string weaponName, GameObject heldWeaponPrefab)
    {
        hasWeapon = true;
        weaponName = currentWeapon;
        Debug.Log($"{currentWeapon} silahýný aldýnýz");


       



        if(heldWeaponPrefab == null)
        {
            Debug.LogWarning("heldWeaponPrefab atanmamýþ!");
            return;
        }

        if (handTransform == null)
        {
            Debug.LogWarning("handTransform atanmamýþ!");
            return;
        }

        if (heldInstance != null) Destroy(heldInstance);

      



        heldInstance = Instantiate(heldWeaponPrefab, weaponPivot);
        heldInstance.transform.localPosition = Vector3.zero;
        heldInstance.transform.localRotation = Quaternion.Euler(0, 0, 90);

        heldInstance.transform.localScale = Vector3.one;

        firePointt = heldInstance.transform.Find("FirePoint");

        var flashObj = heldInstance.transform.Find("MuzzleFlash");

        if (flashObj != null)
        {
            muzzleFlash = flashObj.GetComponent<SpriteRenderer>();
            muzzleFlash.enabled = false;
        }


        if (firePoint == null)
        {
            Debug.LogWarning("Held prefab içinde 'FirePoint' bulunamadý! Lütfen child ekleyin.");
        }
    }

    void Shoot()
    {
        if (Ammo.Length == 0 || Ammo[0] == null)
        {
            Debug.Log("ammo prefab atanmamýþ");
            
           



            

           
        }


        IEnumerator MuzzleFlash()
        {
            muzzleFlash.enabled = true;

            yield return new WaitForSeconds(1);
            muzzleFlash.enabled = false;

        }

       

        GameObject bullet = Instantiate(Ammo[0], firePoint.transform.position, firePoint.transform.rotation);
        bulletScript mover = bullet.GetComponent<bulletScript>();
    
     
        
        if (mover != null)
        {
            mover.speed = Bulletspeed;
        }

        Vector3 spawnPos = (firePoint != null) ? firePoint.transform.position : transform.position;
        Quaternion rot = (firePoint != null) ? firePoint.transform.rotation : Quaternion.identity;

        var rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Silah sprite'ý saða bakýyorsa 'right'; yukarý bakýyorsa 'up' kullan
            Vector2 dir = (Vector2)(rot * Vector3.right);
            rb.velocity = dir.normalized * Bulletspeed;
         
        }

        StartCoroutine(MuzzleFlash());

        StartCoroutine(cameraScript.Shake());
        var anim = heldInstance.GetComponent<Animator>();
        anim.enabled = false;


        
    }


 
}


