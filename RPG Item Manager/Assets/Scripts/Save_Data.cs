using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using UnityEngine;

public class Save_Data : MonoBehaviour
{
    Item_Data Placeholder = new Item_Data("No items found", "Once you have another item, this item dissapear");

    public Item_Data[] AllData;
    string Caminho;
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

                PrepararCaminho();
            }
            else
            {
                Destroy(this);
            }
        }
    }

    void PrepararCaminho()
    {
        //O c é a barra apropriada do SO
        c = Directory.GetDirectoryRoot(Directory.GetCurrentDirectory())[Directory.GetDirectoryRoot(Directory.GetCurrentDirectory()).Length - 1];
        //c = '\\';
        //print(c);
        //print(Directory.GetCurrentDirectory());
        //print(Application.persistentDataPath);
        Caminho = Directory.GetCurrentDirectory() + "GameData";

        for (int i = 0; i < Path.GetInvalidPathChars().Length; i++)
        {
            print(Path.GetInvalidPathChars()[i]);
            if (Caminho.Contains(Path.GetInvalidPathChars()[i].ToString()))
            {
                Caminho = Caminho.Replace(Path.GetInvalidPathChars()[i].ToString(),"z");
            }
        }
        print(Caminho);
        try
        {
            Directory.CreateDirectory(Caminho);
        }
        catch (UnauthorizedAccessException)
        {
            Debug.LogError("Sem acesso, mudando caminho..");

            Caminho = Application.persistentDataPath + c + "GameData";
            Directory.CreateDirectory(Caminho);

            throw;
        }

        AllData = LoadAll();
    }

    public void Save(Item_Data data)
    {
        //BinaryFormatter formatter = new BinaryFormatter();
        DataContractJsonSerializer son = new DataContractJsonSerializer(typeof(Item_Data));
        if (!Directory.Exists(Caminho))
        {
            Directory.CreateDirectory(Caminho);
        }
        string caminhoDoArquivo = Caminho + c + data.ItemName + ".json";
        print(caminhoDoArquivo);
        FileStream file = new FileStream(caminhoDoArquivo, FileMode.Create);
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

        if (!Directory.Exists(Caminho))
        {
            Directory.CreateDirectory(Caminho);
        }

        string[] files = Directory.GetFiles(Caminho, "*.json");

        if (files.Length == 0)
        {
            itemDatas.Add(Placeholder);
            return itemDatas.ToArray();
        }

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

    public void DeleteData(Item_Data data)
    {
        if (File.Exists(Caminho + c + data.ItemName + ".json"))
        {
            File.Delete(Caminho + c + data.ItemName + ".json");
            AllData = LoadAll();
        }
        else
        {
            print(Directory.GetFiles(Caminho, "*.json")[1]);
        }
    }
}
