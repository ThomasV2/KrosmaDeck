using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class Scene_Manager : MonoBehaviour {

    public static GodTypes godType;
    public static bool load_deck = false;
    public static string filename = "";

    public Text file;

    public void Set_load_deck(bool value)
    {
        load_deck = value;
    }

    public void Set_filename()
    {
        if (File.Exists(Application.dataPath + "/Save/" + file.text + ".sav") == true)
        {
            filename = file.text + ".sav";
            load_deck = true;
            Deck_Scene(0);
        }
        else
        {
            file.text = "Erreur ! Mauvais nom de fichier.";
        }
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
        filename = "";
        Application.LoadLevel("Dieu");
    }
}
