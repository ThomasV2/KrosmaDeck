using UnityEngine;
using System.Collections;

public class Scene_Manager : MonoBehaviour {

    public static string card_type = "";
    public static bool load_deck = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Set_load_deck(bool value)
    {
        load_deck = value;
    }

    public void Deck_Scene(string type)
    {
        card_type = type;
        Application.LoadLevel("Card");
    }
}
