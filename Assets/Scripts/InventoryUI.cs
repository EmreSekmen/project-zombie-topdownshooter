using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Image[] slots;
    public Sprite emptySlotSprite;


    public void SetItem(int index, Sprite icon)
    {
        slots[index].sprite = icon;
    }

    public void ClearItem(int index)
    {
        slots[index].sprite = emptySlotSprite;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
