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

    //index = valeur contenue dans le curent_tab selectionné
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
        Deck_cards.Add(index, 1);
    }

    public void Delete_Card(int index)
    {
        Data_Card current = Data_All.data_tab[index];
        if (current.Rarity == (int)Rarity.Infinite)
                count_infinite--;
        Deck_cards[index] = Deck_cards[index] - 1;
        if (Deck_cards[index] == 0)
        {
            Deck_cards.Remove(index);
        }
    }
}
