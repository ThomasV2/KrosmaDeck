using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public class Deck_Manager : MonoBehaviour {

    // Key = index, Value = nombre de carte selectionné
    public Dictionary<int, int> Deck_cards = new Dictionary<int, int>();
    private int count = 0;
    private int count_infinite = 0;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Add_Card(int index)
    {
        if (count >= 45)
            return;

        Data_Card current = Data_All.data_tab[index];
        if (current.Rarity == (int)Rarity.Infinite)
        {
            if (count_infinite < 5)
                count_infinite++;
            else
                return;
        }
        foreach (KeyValuePair<int, int> card in Deck_cards)
        {
            if (card.Key == index)
            {
                if (current.Rarity == (int)Rarity.Infinite)
                {
                    count_infinite--;
                    return;
                }
                if (current.Rarity == (int)Rarity.Krosmique)
                {
                    return;
                }
                if (card.Value < 3)
                {
                    Deck_cards[index] += 1;
                    count++;
                }
                return;
            }
        }
    }

    public void Delete_Card(GameObject card)
    {
        /*Data_Deck data = card.GetComponent<Data_Deck>();
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
        }*/
    }

    public void Create_Deck_File()
    {
/*        string path = Application.dataPath + "/Save/";
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

        File.WriteAllText(path + filename, data);*/
    }

    public void Load_Deck_File()
    {
       /* string path = Application.dataPath + "/Save/";
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
        }*/
    }
}
