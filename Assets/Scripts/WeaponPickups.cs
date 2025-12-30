using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickups : MonoBehaviour
{
    private string weaponName;

    public GameObject heldWeaponPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {


            var shooter = collision.GetComponent<PlayerShooting>();



            if(shooter == null)
            {
                Debug.Log("prefab alaný boþ");
                return;
            }

            shooter.EquipWeapon(weaponName, heldWeaponPrefab);



            Destroy(gameObject);
        }
    }
}
