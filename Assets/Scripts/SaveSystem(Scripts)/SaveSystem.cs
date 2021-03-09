﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// Contains all of the utilities needed to save SaveInfo Objects
/// </summary>
public class SaveSystem
{
#if UNITY_EDITOR
    public static readonly string SAVE_FOLDER = Application.dataPath + "/Saves/";
#elif UNITY_STANDALONE
    public static readonly string SAVE_FOLDER = Application.persistentDataPath + "/Saves/";
#endif
    public static readonly int SavesNumber = 4;
    private static readonly string SaveName = "SaveSlot";
    private static readonly string fileFormat = "save";

    /// <summary>
    /// Checks if the saveFolder exists others it will procede to create one
    /// </summary>
    public static void Init()
    {
        if (!Directory.Exists(SAVE_FOLDER))
        {
            Directory.CreateDirectory(SAVE_FOLDER);
        };
    }
    /// <summary>
    /// Checks if the file at that slot doesn't exist so that it can create it.
    /// Otherwise if the file exists it will update it instead
    /// </summary>
    /// <param name="info"></param>
    /// <param name="slot"></param>
    public static void Save(SaveInfo info,int slot)
    {
        BinaryFormatter binFor = new BinaryFormatter();
        if (!File.Exists($"{SAVE_FOLDER}{SaveName}{slot.ToString()}.{fileFormat}"))
        {
            FileStream file = File.Create($"{SAVE_FOLDER}{SaveName}{slot.ToString()}.{fileFormat}");
            binFor.Serialize(file, info);
            file.Close();
        }
        else
        {
            FileStream file = File.Open($"{SAVE_FOLDER}{SaveName}{slot.ToString()}.{fileFormat}", FileMode.Open);
            binFor.Serialize(file, info);
            file.Close();
        }
    }
    
    /// <summary>
    /// It will delete the file at the given slot if exists
    /// </summary>
    /// <param name="slot"></param>
    public static void Delete(int slot)
    {
        if (File.Exists($"{SAVE_FOLDER}{SaveName}{slot.ToString()}.{fileFormat}"))
        {
            File.Delete($"{SAVE_FOLDER}{SaveName}{slot.ToString()}.{fileFormat}");
        }
    }
    /// <summary>
    /// It will return all the posible Save files found
    /// </summary>
    /// <returns></returns>
    public static List<SaveInfo> LoadAll()
    {
        List<SaveInfo> saveInfos = new List<SaveInfo>();
        for (int i = 0; i < SavesNumber; i++)
        {
            if (File.Exists($"{SAVE_FOLDER}{SaveName}{i.ToString()}.{fileFormat}"))
            {
                BinaryFormatter binFor = new BinaryFormatter();
                FileStream file = File.Open($"{SAVE_FOLDER}{SaveName}{i.ToString()}.{fileFormat}", FileMode.Open);
                SaveInfo saveinfo = (SaveInfo)binFor.Deserialize(file);
                file.Close();
                saveInfos.Add(saveinfo);
            }
        }
        return saveInfos;
    }
}