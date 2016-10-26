using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public class Deck_Manager : MonoBehaviour {

    public RectTransform myPanel;
    public GameObject myDeckPrefab;
    public Aff_card aff_card;

    public List<GameObject> Deck_cards = new List<GameObject>();
    private int count = 0;
    private int count_infinite = 0;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Add_Card(int id, string name, int cost, string type, string rarity, string img)
    {
        if (count >= 45)
            return;
        if (rarity == "infinite")
        {
            if (count_infinite < 5)
                count_infinite++;
            else
                return;
        }
        foreach (GameObject card in Deck_cards)
        {
            if (card.name == name)
            {
                if (rarity == "infinite")
                {
                    count_infinite--;
                    return;
                }
                if (rarity == "krosmik")
                {
                    return;
                }
                if (card.GetComponent<Data_Deck>().quantity < 3)
                {
                    card.GetComponent<Data_Deck>().Set_Quantity(card.GetComponent<Data_Deck>().quantity + 1);
                    count++;
                }
                return;
            }
        }
        GameObject newDeck = (GameObject)Instantiate(myDeckPrefab);
        newDeck.transform.SetParent(myPanel);
        newDeck.name = name;
        Data_Deck data = newDeck.GetComponent<Data_Deck>();
        data.Set_Name(name);
        data.Set_Cost(cost);
        data.Set_Quantity(1);
        data.Set_Type(type);
        data.Set_Rarity(rarity);
        data.Set_Img(img);
        data.id = id;
        Deck_cards.Add(newDeck);
        count++;
    }

    public void Delete_Card(GameObject card)
    {
        Data_Deck data = card.GetComponent<Data_Deck>();
        if (data.rarity == "infinite")
            count_infinite--;

        if (data.quantity == 1)
        {
            Deck_cards.Remove(card);
            Destroy(card);
            count--;
        }
        else
        {
            data.Set_Quantity(data.quantity - 1);
            count--;
        }
    }

    public void Create_Deck_File()
    {
        string path = Application.dataPath + "/Save/";
        string filename = "test_deck.sav";
        string data = "";
        Data_Deck current_data_deck;

        if (!Directory.Exists(path))
             Directory.CreateDirectory(path);
        foreach (GameObject card in Deck_cards)
        {
            current_data_deck = card.GetComponent<Data_Deck>();
            for (int i = current_data_deck.quantity; i > 0; i--)
                data += current_data_deck.id + ",";
        }
        data = data.Remove(data.Length - 1);

        File.WriteAllText(path + filename, data);
    }

    public void Load_Deck_File()
    {
        string path = Application.dataPath + "/Save/";
        string filename = "test_deck.sav";
        int value;

        if (!Directory.Exists(path))
            return;

        string data = File.ReadAllText(path + "/" + filename);
        string[] ids = data.Split(","[0]);
        foreach (string id in ids)
        {
            int.TryParse(id, out value);
            aff_card.Add_to_Deck_by_Id(value);
        }
    }
}
