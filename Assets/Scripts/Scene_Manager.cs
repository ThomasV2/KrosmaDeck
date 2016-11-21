using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class Scene_Manager : MonoBehaviour {

    public static GodTypes godType;
    public static bool load_deck = false;
    public static string pathfile = "";
    public GameObject fondu;
    public Text Filename;

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
