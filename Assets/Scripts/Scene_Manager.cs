using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
        filename = file.text + ".sav";
    }

    public void Deck_Scene(int type)
    {
        godType = (GodTypes)type;
        Application.LoadLevel("Card");
    }
}
