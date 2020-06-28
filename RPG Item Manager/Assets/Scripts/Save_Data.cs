using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using UnityEngine;

public class Save_Data : MonoBehaviour
{
    public Item_Data[] AllData;
    public char c;

    public static Save_Data DataSaver;

    private void Awake()
    {
        if(DataSaver != this)
        {
            if(DataSaver == null)
            {
                DataSaver = this;
                DontDestroyOnLoad(this);

                //O c é a barra apropriada do SO
                c = Directory.GetDirectoryRoot(Directory.GetCurrentDirectory())[Directory.GetDirectoryRoot(Directory.GetCurrentDirectory()).Length - 1];
                //print(c);
                AllData = LoadAll();
            }
            else
            {
                Destroy(this);
            }
        }
    }

    public void Save(Item_Data data)
    {
        //BinaryFormatter formatter = new BinaryFormatter();
        DataContractJsonSerializer son = new DataContractJsonSerializer(typeof(Item_Data));
        if (!Directory.Exists(Directory.GetCurrentDirectory() + c +"Game_data"))
        {
            Directory.CreateDirectory(Directory.GetCurrentDirectory() + c + "Game_data");
        }
        string caminho = Directory.GetCurrentDirectory() + c + "Game_data" + c + data.ItemName + ".json";
        print(caminho);
        FileStream file = new FileStream(caminho, FileMode.Create);
        //formatter.Serialize(file, data);
        son.WriteObject(file, data);
        file.Close();

        //adicionar magia
        AllData = LoadAll();
    }

    //Directory.GetFiles(string caminho, "*.extensão");

    public Item_Data[] LoadAll()
    {
        List<Item_Data> itemDatas = new List<Item_Data>();
        string caminho = Directory.GetCurrentDirectory() + c + "Game_data";

        if (!Directory.Exists(caminho))
        {
            Directory.CreateDirectory(caminho);
        }

        string[] files = Directory.GetFiles(caminho, "*.json");

        for (int i = 0; i < files.Length; i++)
        {
            //print(files[i]);
            itemDatas.Add(Load(files[i]));
        }

        return itemDatas.ToArray();
    }

    public Item_Data Load(string path)
    {
        if (File.Exists(path))
        {
            //BinaryFormatter formatter = new BinaryFormatter();
            DataContractJsonSerializer son = new DataContractJsonSerializer(typeof(Item_Data));
            FileStream file = new FileStream(path, FileMode.Open);
            file.Position = 0;
            //Item_Data data = formatter.Deserialize(file) as Item_Data;
            Item_Data data = (Item_Data)son.ReadObject(file);
            file.Close();

            return data;
        }
        else
        {
            Debug.LogError("não encontrado");
            return null;
        }
    }

}
