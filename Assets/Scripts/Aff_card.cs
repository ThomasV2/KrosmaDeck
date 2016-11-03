using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

public class Aff_card : MonoBehaviour {
    
    // Data
    private Sprite[] prev_img = new Sprite[8] { null, null, null, null, null, null, null, null };
    private Sprite[] current_img = new Sprite[8] { null, null, null, null, null, null, null, null };
    private Sprite[] next_img = new Sprite[8] { null, null, null, null, null, null, null, null };
    public Research_Card research;

    // Affichage
    public GameObject[] cards;
    public Deck_Manager deck_manager;
    public File_Manager file_manager;
    public GameObject scroll_content;
    public GameObject prefab_deck;

    // Input
    public GameObject cards_button;
    public GameObject deck_button;
    public Dropdown Rarity_Dropdown;
    public Text Input_text;

    private int page;

    // Use this for initialization
    void Start () {
        if (Scene_Manager.load_deck == true)
        {
            cards_button.SetActive(true);
            deck_button.SetActive(false);
            file_manager.Load_Deck_File(Scene_Manager.filename);
            Refresh_Deck();
            Aff_Deck();
        }
        else
        {
            research.Research();
            cards_button.SetActive(false);
            deck_button.SetActive(true);
            Init_Img();
            Refresh();
        }

	}

    public void Refresh()
    {
        for (int count = 0; count < 8; count++)
        {
            if (current_img[count] == null)
                cards[count].SetActive(false);
            else
            {
                cards[count].SetActive(true);
                cards[count].GetComponent<Image>().sprite = current_img[count];
            }
        }
    }

    public void Refresh_Deck()
    {
        foreach (Transform child in scroll_content.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (KeyValuePair<int, int> pair in deck_manager.Deck_cards)
        {
            GameObject newCard = (GameObject)Instantiate(prefab_deck);
            newCard.GetComponent<Prefab_Script>().index = pair.Key;
            Text component;
            foreach (Transform child in newCard.transform)
            {
                component = child.GetComponent<Text>();
                if (child.name == "Qantity")
                    component.text = pair.Value.ToString();
                else if (child.name == "Name")
                    component.text = Data_All.data_tab[pair.Key].Name;
                else // Cost
                    component.text = Data_All.data_tab[pair.Key].CostAP.ToString();
            }   
            newCard.transform.SetParent(scroll_content.transform);
        }
    }

    public void Next_Page()
    {
        if (Research_Card.current_cards.Count - (page * 8) >= 8)
        {
            page++;
            Preload_Img(true);
            Refresh();
        }
    }

    public void Prev_Page()
    {
        if (page > 0)
        {
            page--;
            Preload_Img(false);
            Refresh();
        }
    }

    public void Init_Img()
    {
        int index;
        page = 0;
        //Debug.LogError("size current is " + Research_Card.current_cards.Count);
        for (int count = 0; count < 8; count++)
        {
            if (count < Research_Card.current_cards.Count)
            {
                index = Research_Card.current_cards[count + (page * 8)];
                current_img[count] = Resources.Load<Sprite>("Cards/" + Data_All.data_tab[index].Name);
            }
            else
            {
                current_img[count] = null;
            }
        }
        for (int count = 0; count < 8; count++)
        {
            if (count + 8 < Research_Card.current_cards.Count)
            {
                index = Research_Card.current_cards[count + ((page + 1) * 8)];
                next_img[count] = Resources.Load<Sprite>("Cards/" + Data_All.data_tab[index].Name);
            }
            else
            {
                next_img[count] = null;
            }
        }
    }

    void Preload_Img(bool is_next)
    {
        int index;
        if (is_next == true)
        {
            // prev -> unload
            StartCoroutine(Unload_Imgs());
            for (int count = 0; count < 8; count++)
            {
                // current -> prev
                prev_img[count] = current_img[count];
                // next -> current
                current_img[count] = next_img[count];
                // preload next
                if (count + ((page + 1) * 8) < Research_Card.current_cards.Count)
                {
                    index = Research_Card.current_cards[count + ((page + 1) * 8)];
                    StartCoroutine(Load_Img_Next(index, count));
                }
                else
                {
                    next_img[count] = null;
                }
            }
        }
        else
        {
            //next -> unload OK
            StartCoroutine(Unload_Imgs());
            for (int count = 0; count < 8; count++)
            {
                //current -> next
                next_img[count] = current_img[count];
                //prev -> current
                current_img[count] = prev_img[count];
                //preload new prev
                if (page > 0)
                {
                    index = Research_Card.current_cards[count + ((page - 1) * 8)];
                    StartCoroutine(Load_Img_Prev(index, count));
                }
                else
                {
                    prev_img[count] = null;
                }
            }
        }
    }

    IEnumerator Load_Img_Next(int index, int count)
    {
        ResourceRequest request = Resources.LoadAsync<Sprite>("Cards/" + Data_All.data_tab[index].Name);
        yield return request;
        next_img[count] = request.asset as Sprite;
    }

    IEnumerator Load_Img_Prev(int index, int count)
    {
        ResourceRequest request = Resources.LoadAsync<Sprite>("Cards/" + Data_All.data_tab[index].Name);
        yield return request;
        prev_img[count] = request.asset as Sprite;
    }

    IEnumerator Unload_Imgs()
    {
        Resources.UnloadUnusedAssets();
        yield return null;
    }


    public void Aff_Deck()
    {
        List<int> new_current = new List<int>();

        foreach (KeyValuePair<int, int> pair in deck_manager.Deck_cards)
        {
            new_current.Add(pair.Key);
        }
        Research_Card.current_cards = new_current;
        Init_Img();
        Refresh();
    }

    public void Add_to_Deck(int count)
    {
        deck_manager.Add_Card(Research_Card.current_cards[count + (page * 8)]);
        Refresh_Deck();
    }
}
