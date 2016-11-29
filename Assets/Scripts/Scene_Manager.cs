using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System.Text;
using System;

public class Scene_Manager : MonoBehaviour {

    public static GodTypes godType;
    public static bool load_deck = false;
    public static string pathfile = "";
    public static bool load_url = false;
    public GameObject fondu;
    public Text Filename;

    void Start()
    {
#if UNITY_WEBGL
        if (load_url == false && Application.absoluteURL.Contains("id=") == true)
        {
            pathfile = Application.absoluteURL.Substring(Application.absoluteURL.IndexOf("=") + 1);
            byte[] decodedBytes = Convert.FromBase64String(pathfile);
            pathfile = Encoding.UTF8.GetString(decodedBytes); // decodedText
            load_url = true;
            load_deck = true;
            Deck_Scene(0);
        }
#endif
    }

    public void Set_load_deck(bool value)
    {
        load_deck = value;
    }

    public void Open_Load_Dialog_File()
    {
#if UNITY_WEBGL
        return;
#else
        FileSelector.GetFile(Search_file_load, ".sav");
#endif
    }

    public void Search_file_load(FileSelector.Status status, string path)
    {
        if (status != FileSelector.Status.Successful)
        {
            fondu.SetActive(false);
            return;
        }
        pathfile = path;
        load_deck = true;
        Deck_Scene(0);
    }

    public void Load_by_Playerpref()
    {
        pathfile = Filename.text;
        load_deck = true;
        Deck_Scene(0);
    }

    public void Deck_Scene(int type)
    {
        godType = (GodTypes)type;
        Application.LoadLevel("Card");
    }

    static public void God_Scene()
    {
        godType = GodTypes.Neutre;
        load_deck = false;
        pathfile = "";
        Application.LoadLevel("Dieu");
    }
}
