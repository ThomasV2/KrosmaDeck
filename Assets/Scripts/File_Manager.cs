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
    public Text infos;
    public GameObject save_continue;

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
            return;
        }

        string id;
        if (Scene_Manager.pathfile.Length > 1)
        {
            id = Scene_Manager.pathfile;
        }
        else
        {
            do
            {
                id = Random.Range(1000, 999999).ToString();
            } while (PlayerPrefs.HasKey(id) == true);
        }

        string data = "";

        foreach (KeyValuePair<int, int> pair in deck_manager.Deck_cards)
        {
            for (int i = pair.Value; i > 0; i--)
                data += Data_All.data_tab[pair.Key].Id + ",";
        }
        data = data.Remove(data.Length - 1);

        PlayerPrefs.SetString(id, data);
        PlayerPrefs.Save();
        infos.text = "Sauvegarde réussie !\nVotre <b>ID de chargement</b> est : " + id.ToString() + "\nN'oubliez pas de le noter et merci d'avoir utiliser KrosmaDeck !";
    }

    public void Save_File()
    {
#if UNITY_WEBGL
        if (Scene_Manager.pathfile.Length > 1 && save_continue.active == false)
        {
            save_continue.SetActive(true);
            infos.text = "<b>Attention !</b>\nVous aller remplacer l'ancienne sauvegarde par celle-ci.\nÊtes-vous sûr de votre choix ?";
        }
        else
        { 
            Create_Deck_File_Web();
            save_continue.SetActive(false);
        }
        
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

    public void Load_Deck_File_Web(string name)
    {
        int value;
        GodTypes type = GodTypes.Neutre;

        if (PlayerPrefs.HasKey(name) == false)
            return;
        string data = PlayerPrefs.GetString(name);
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
