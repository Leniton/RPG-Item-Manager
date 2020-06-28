using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Manager : MonoBehaviour
{

    [SerializeField] GameObject FirstItem;
    List<GameObject> Items = new List<GameObject>();

    /*Child ID guide:
     0 - Backrgound
     1 - Item Name
     2 - Ammo Text
     3 - Add ammo
     4 - Remove ammo
     5 - Description Buttom*/

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
        print(FirstItem.GetComponent<RectTransform>().rect.height);
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
}
