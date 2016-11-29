using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using System;
using System.Text;

public class File_Manager : MonoBehaviour {

    public Deck_Manager deck_manager;
    public string pathfile;
    public Text filename;
    public Text infos;
    public GameObject save_continue;

    private TextEditor te = new TextEditor();

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

        foreach (KeyValuePair<int, int> pair in deck_manager.Deck_cards)
        {
            for (int i = pair.Value; i > 0; i--)
                data += Data_All.data_tab[pair.Key].Id + ",";
        }
        data = data.Remove(data.Length - 1);

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        File.WriteAllText(path + file, data);
    }

    public void Create_Deck_File_Web()
    {
        if (deck_manager.Deck_cards.Count == 0)
        {
            infos.text = "<b>Erreur !</b>\nAucune carte présente dans le deck.";
            te.text = "";
            return;
        }

        string data = "";

        foreach (KeyValuePair<int, int> pair in deck_manager.Deck_cards)
        {
            for (int i = pair.Value; i > 0; i--)
                data += Data_All.data_tab[pair.Key].Id + ",";
        }
        data = data.Remove(data.Length - 1);

        byte[] bytesToEncode = Encoding.UTF8.GetBytes(data);
        string encodedText = Convert.ToBase64String(bytesToEncode);

        te.text = "http://thomasv2.github.io/krosmadeck?id=" + encodedText;
        infos.text = "Sauvegarde réussie !\nVotre <b>lien de partage</b> est disponible en appuyant sur \"Copier\"\nCopiez votre lien après le rechargement de la page.";
    }

    public void Copy_url()
    {
       
        Application.OpenURL(te.text);
//        GUIUtility.systemCopyBuffer = te.text;
    }

    public void Save_File()
    {
#if UNITY_WEBGL
        Create_Deck_File_Web();
#else
        Create_Deck_File();
#endif
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
                     if (Data_All.data_tab[index].GodType != (int)GodTypes.Neutre && Data_All.data_tab[index].GodType != (int)type)
                         type = (GodTypes)Data_All.data_tab[index].GodType;
                     break;
                 }
             }
             deck_manager.Add_Card(index);
          }
         Scene_Manager.godType = type;
    }

    public void Load_Deck_File_Web(string data)
    {
        int value;
        GodTypes type = GodTypes.Neutre;

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
                    if (Data_All.data_tab[index].GodType != (int)GodTypes.Neutre && Data_All.data_tab[index].GodType != (int)type)
                        type = (GodTypes)Data_All.data_tab[index].GodType;
                    break;
                }
            }
            deck_manager.Add_Card(index);
        }
        Scene_Manager.godType = type;
    }
}
