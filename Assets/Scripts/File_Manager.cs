using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class File_Manager : MonoBehaviour {

    public Deck_Manager deck_manager;
    public string pathfile;
    public Text filename;
    
    public void Create_Deck_File()
    {
        if (deck_manager.Deck_cards.Count == 0)
            return;
        if (filename.text.Length == 0)
        {
            Debug.LogError("Filename absent");
            return;
        }
        string path = Application.dataPath + "/Save/";
        string file = filename.text + ".sav";
        string data = "";

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        foreach (KeyValuePair<int, int> pair in deck_manager.Deck_cards)
        {
            for (int i = pair.Value; i > 0; i--)
                data += Data_All.data_tab[pair.Key].Id + ",";
        }
        data = data.Remove(data.Length - 1);

        File.WriteAllText(path + file, data);
    }

    public void Load_Deck_File(string path)
    {
        int value;
        GodTypes type = GodTypes.Neutre;

        string data = File.ReadAllText(path);
         string[] ids = data.Split(","[0]);
         int index = 0;
         foreach (string id in ids)
         {
             int.TryParse(id, out value);
             for (int i = 0; i < Data_All.SIZE_TAB; i++)
             {
                 if (Data_All.data_tab[i].Id == value)
                 {
                     index = i;
                     if (type != GodTypes.Neutre && Data_All.data_tab[index].GodType != (int)type)
                         type = (GodTypes)Data_All.data_tab[index].GodType;
                     break;
                 }
             }
             deck_manager.Add_Card(index);
          }
         Scene_Manager.godType = type;
    }
}
