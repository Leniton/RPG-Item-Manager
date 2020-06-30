using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Manager : MonoBehaviour
{

    [SerializeField] GameObject FirstItem;
    List<GameObject> Items = new List<GameObject>();

    [Space]

    [SerializeField] GameObject CreationMenu;
    [SerializeField] Item_Data Item = new Item_Data("nOname","");

    /*Child ID guide:
     0 - Backrgound
     1 - Item Name
     2 - Ammo Text
     3 - Add ammo
     4 - Remove ammo
     5 - Description Buttom*/

    /*creation child guide:
     3 - Nome
     6 - tipo
     7 - ammo
     8 - max
     9 - desc
     10 - barra*/

    void Start()
    {
        MakeItemList();
    }

    public void MakeItemList()
    {
        Items.Add(FirstItem);
        PlaceItemValues(FirstItem);
        //174
        for (int i = 1; i < Save_Data.DataSaver.AllData.Length; i++)
        {
            Items.Add(Instantiate(FirstItem, FirstItem.transform.parent));
            Items[i].transform.position = Items[i - 1].transform.position + (Vector3.down * (FirstItem.GetComponent<RectTransform>().rect.height * 1.1f));
            PlaceItemValues(Items[i]);
        }
        
        if(FirstItem.transform.parent.parent.GetComponent<RectTransform>().rect.height < (FirstItem.transform.GetComponent<RectTransform>().rect.height * Items.Count) * 1.1f)
        {
            print((FirstItem.transform.GetComponent<RectTransform>().rect.height * Items.Count) * 1.1f);
            print(FirstItem.transform.parent.parent.GetComponent<RectTransform>().rect.height);

            float h = (FirstItem.transform.GetComponent<RectTransform>().rect.height * Items.Count) *1.1f;

            FirstItem.transform.parent.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(FirstItem.transform.parent.parent.GetComponent<RectTransform>().rect.width, h);
        }
    }

    void PlaceItemValues(GameObject item)
    {
        item.name = Save_Data.DataSaver.AllData[Items.IndexOf(item)].ItemName;
        item.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = Save_Data.DataSaver.AllData[Items.IndexOf(item)].ItemName;

        if (Save_Data.DataSaver.AllData[Items.IndexOf(item)].TypeItem == 1)
        {
            item.transform.GetChild(2).gameObject.SetActive(true);
            item.transform.GetChild(3).gameObject.SetActive(true);
            item.transform.GetChild(4).gameObject.SetActive(true);

            item.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = Save_Data.DataSaver.AllData[Items.IndexOf(item)].ammoCount.ToString();
        }else if (Save_Data.DataSaver.AllData[Items.IndexOf(item)].TypeItem == 2)
        {
            item.transform.GetChild(2).gameObject.SetActive(true);
            item.transform.GetChild(3).gameObject.SetActive(true);
            item.transform.GetChild(4).gameObject.SetActive(true);

            item.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = Save_Data.DataSaver.AllData[Items.IndexOf(item)].ammoCount + "/" + Save_Data.DataSaver.AllData[Items.IndexOf(item)].MaxCount;
        }
    }

    public void AddAmmo(GameObject item)
    {
        Save_Data.DataSaver.AllData[Items.IndexOf(item)].ammoCount++;

        if(Save_Data.DataSaver.AllData[Items.IndexOf(item)].TypeItem == 2)
        {
            if(Save_Data.DataSaver.AllData[Items.IndexOf(item)].ammoCount >=
               Save_Data.DataSaver.AllData[Items.IndexOf(item)].MaxCount)
            {
                Save_Data.DataSaver.AllData[Items.IndexOf(item)].ammoCount = Save_Data.DataSaver.AllData[Items.IndexOf(item)].MaxCount;
                item.transform.GetChild(3).GetComponent<Button>().interactable = false;
            }
            else if (Save_Data.DataSaver.AllData[Items.IndexOf(item)].ammoCount > 0 && item.transform.GetChild(4).GetComponent<Button>().interactable == false)
            {
                item.transform.GetChild(4).GetComponent<Button>().interactable = true;
            }
        }
        else if (Save_Data.DataSaver.AllData[Items.IndexOf(item)].TypeItem == 1)
        {
            if (Save_Data.DataSaver.AllData[Items.IndexOf(item)].ammoCount > 0 && item.transform.GetChild(4).GetComponent<Button>().interactable == false)
            {
                item.transform.GetChild(4).GetComponent<Button>().interactable = true;
            }
        }

        PlaceItemValues(item);
    }

    public void RemoveAmmo(GameObject item)
    {
        Save_Data.DataSaver.AllData[Items.IndexOf(item)].ammoCount--;

        if (Save_Data.DataSaver.AllData[Items.IndexOf(item)].TypeItem == 2)
        {
            if (Save_Data.DataSaver.AllData[Items.IndexOf(item)].ammoCount <
                Save_Data.DataSaver.AllData[Items.IndexOf(item)].MaxCount && item.transform.GetChild(3).GetComponent<Button>().interactable == false)
            {
                item.transform.GetChild(3).GetComponent<Button>().interactable = true;
            }
            else if (Save_Data.DataSaver.AllData[Items.IndexOf(item)].ammoCount <= 0)
            {
                Save_Data.DataSaver.AllData[Items.IndexOf(item)].ammoCount = 0;
                item.transform.GetChild(4).GetComponent<Button>().interactable = false;
            }

        }
        else if(Save_Data.DataSaver.AllData[Items.IndexOf(item)].TypeItem == 1)
        {
            if (Save_Data.DataSaver.AllData[Items.IndexOf(item)].ammoCount <= 0)
            {
                Save_Data.DataSaver.AllData[Items.IndexOf(item)].ammoCount = 0;
                item.transform.GetChild(4).GetComponent<Button>().interactable = false;
            }
        }

        PlaceItemValues(item);
    }

    public void AdaptPerType(int type)
    {
        Item.TypeItem = type;
        switch (type)
        {
            case 0:
                CreationMenu.transform.GetChild(7).gameObject.SetActive(false);
                CreationMenu.transform.GetChild(8).gameObject.SetActive(false);
                CreationMenu.transform.GetChild(10).gameObject.SetActive(false);

                CreationMenu.transform.GetChild(9).GetComponent<TMP_InputField>().placeholder.GetComponent<TextMeshProUGUI>().text = "Default type of item, write description...";
                break;
            case 1:
                CreationMenu.transform.GetChild(7).gameObject.SetActive(true);
                CreationMenu.transform.GetChild(8).gameObject.SetActive(false);
                CreationMenu.transform.GetChild(10).gameObject.SetActive(false);

                CreationMenu.transform.GetChild(9).GetComponent<TMP_InputField>().placeholder.GetComponent<TextMeshProUGUI>().text = "Ammo type of item, write description...";
                break;
            case 2:
                CreationMenu.transform.GetChild(7).gameObject.SetActive(true);
                CreationMenu.transform.GetChild(8).gameObject.SetActive(true);
                CreationMenu.transform.GetChild(10).gameObject.SetActive(true);

                CreationMenu.transform.GetChild(9).GetComponent<TMP_InputField>().placeholder.GetComponent<TextMeshProUGUI>().text = "Limited charges type of item, write description...";
                break;
        }
    }

    public void ItemName(string n)
    {
        Item.ItemName = n;
    }
    public void ItemDesctription(string d)
    {
        Item.DescriptionItem = d;
    }
    public void ItemAmmo(string a)
    {
        if(a == "")
        {
            Item.ammoCount = 0;
        }
        else
        {
            Item.ammoCount = int.Parse(a);
        }
    }
    public void ItemMaxAmmo(string m)
    {
        if (m == "")
        {
            Item.MaxCount = 0;
        }
        else
        {
            Item.MaxCount = int.Parse(m);
        }
    }


}
