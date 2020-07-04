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

    [Space]

    [SerializeField] GameObject Description;
    int IDItem;

    /*Child ID guide:
     0 - Backrgound
     1 - Item Name
     2 - Ammo Text
     3 - Add ammo
     4 - Remove ammo
     5 - Description Buttom*/

    /*creation child guide:
     1 - Nome
     4 - tipo
     5 - ammo
     6 - max
     7 - desc
     8 - barra*/

    void Start()
    {
        //Invoke("MakeItemList", 0.5f);

        if(Save_Data.DataSaver.AllData.Length == 0)
        {
            Save_Data.DataSaver.AllData = Save_Data.DataSaver.LoadAll();
        }

        MakeItemList();
    }

    //functions to show the items
    public void MakeItemList()
    {
        if(Items.Count > 0)
        {
            for (int i = 1; i < Items.Count; i++)
            {
                Destroy(Items[i]);
            }
        }

        Items.Clear();
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
            //print((FirstItem.transform.GetComponent<RectTransform>().rect.height * Items.Count) * 1.1f);
            //print(FirstItem.transform.parent.parent.GetComponent<RectTransform>().rect.height);
            //print(FirstItem.transform.parent.parent.GetComponent<RectTransform>().offsetMin);

            float h = (FirstItem.transform.GetComponent<RectTransform>().rect.height * Items.Count) *1.1f;

            FirstItem.transform.parent.parent.GetComponent<RectTransform>().offsetMin = new Vector2(0.0f, -h);
        }
    }

    void PlaceItemValues(GameObject item)
    {
        if(Save_Data.DataSaver.AllData.Length == 0)
        {
            CreationMenu.SetActive(true);
            return;
        }

        item.name = Save_Data.DataSaver.AllData[Items.IndexOf(item)].ItemName;
        item.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = Save_Data.DataSaver.AllData[Items.IndexOf(item)].ItemName;

        if(Save_Data.DataSaver.AllData[Items.IndexOf(item)].TypeItem == 0)
        {
            item.transform.GetChild(2).gameObject.SetActive(false);
            item.transform.GetChild(3).gameObject.SetActive(false);
            item.transform.GetChild(4).gameObject.SetActive(false);
        }
        else if (Save_Data.DataSaver.AllData[Items.IndexOf(item)].TypeItem == 1)
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
        Save_Data.DataSaver.Save(Save_Data.DataSaver.AllData[Items.IndexOf(item)]);
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
        Save_Data.DataSaver.Save(Save_Data.DataSaver.AllData[Items.IndexOf(item)]);
    }

    //functions to create a item
    public void AdaptPerType(int type)
    {
        Item.TypeItem = type;
        switch (type)
        {
            case 0:
                CreationMenu.transform.GetChild(5).gameObject.SetActive(false);
                CreationMenu.transform.GetChild(6).gameObject.SetActive(false);
                CreationMenu.transform.GetChild(8).gameObject.SetActive(false);

                CreationMenu.transform.GetChild(7).GetComponent<TMP_InputField>().placeholder.GetComponent<TextMeshProUGUI>().text = "Default type of item, write description...";
                break;
            case 1:
                CreationMenu.transform.GetChild(5).gameObject.SetActive(true);
                CreationMenu.transform.GetChild(6).gameObject.SetActive(false);
                CreationMenu.transform.GetChild(8).gameObject.SetActive(false);

                CreationMenu.transform.GetChild(7).GetComponent<TMP_InputField>().placeholder.GetComponent<TextMeshProUGUI>().text = "Ammo type of item, write description...";
                break;
            case 2:
                CreationMenu.transform.GetChild(5).gameObject.SetActive(true);
                CreationMenu.transform.GetChild(6).gameObject.SetActive(true);
                CreationMenu.transform.GetChild(8).gameObject.SetActive(true);

                CreationMenu.transform.GetChild(7).GetComponent<TMP_InputField>().placeholder.GetComponent<TextMeshProUGUI>().text = "Limited charges type of item, write description...";
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

    public void CreateItem()
    {
        for (int i = 0; i < Save_Data.DataSaver.AllData.Length; i++)
        {
            if (Item.ItemName == Save_Data.DataSaver.AllData[i].ItemName)
            {
                print("item com esse nome já existe");
                return;
            }
        }

        Save_Data.DataSaver.Save(Item);
        MakeItemList();
        CreationMenu.SetActive(false);
    }

    public void CancelItem()
    {
        CreationMenu.SetActive(false);
    }

    public void ClearItemCreation()
    {
        print("clear");
    }

    //function to see and edit description of the item
    public void OpenDescription(GameObject item)
    {
        IDItem = Items.IndexOf(item);
        Description.GetComponent<TMP_InputField>().text = Save_Data.DataSaver.AllData[Items.IndexOf(item)].DescriptionItem;
        Description.SetActive(true);
    }

    public void SaveDescription()
    {
        Save_Data.DataSaver.AllData[IDItem].DescriptionItem = Description.GetComponent<TMP_InputField>().text;
        Save_Data.DataSaver.Save(Save_Data.DataSaver.AllData[IDItem]);
    }

    //delete items functions
    public void DeletionSelection(GameObject Container)
    {
        for (int i = 0; i < Container.transform.childCount; i++)
        {
            Container.transform.GetChild(i).GetComponent<Button>().enabled = true;
            Container.transform.GetChild(i).GetComponent<Button>().targetGraphic.gameObject.SetActive(true);
        }
    }

    public void CancelSelection(GameObject Container)
    {
        for (int i = 0; i < Container.transform.childCount; i++)
        {
            Container.transform.GetChild(i).GetComponent<Button>().enabled = false;
            Container.transform.GetChild(i).GetComponent<Button>().targetGraphic.gameObject.SetActive(false);
        }
    }

    public void DeleteItem(GameObject item)
    {
        Save_Data.DataSaver.DeleteData(Save_Data.DataSaver.AllData[Items.IndexOf(item)]);
        Destroy(item);
        MakeItemList();
    }
}
