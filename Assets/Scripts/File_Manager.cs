using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;

public class File_Manager : MonoBehaviour {

    public Deck_Manager deck_manager;
    public Text filename;

    public void Create_Deck_File()
    {
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

    public void Load_Deck_File(string filename)
    {
        string path = Application.dataPath + "/Save/";
        int value;
        GodTypes type = GodTypes.Neutre;

         if (!Directory.Exists(path))
             return;

         string data = File.ReadAllText(path + "/" + filename);
         string[] ids = data.Split(","[0]);
         int index = 0;
         foreach (string id in ids)
         {
             int.TryParse(id, out value);
             Debug.LogError(value);
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
             Debug.LogError("add index " + index);
             deck_manager.Add_Card(index);
          }
         Scene_Manager.godType = type;
    }
}
