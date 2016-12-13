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
    public Text total_text;
    public Text cost_text;
    public GameObject lock_panel;
    public GameObject prefab_deck;
    public Sprite[] rarity_logo; 

    // Input
    public GameObject cards_button;
    public GameObject deck_button;
    public Dropdown Rarity_Dropdown;
    public Text Input_text;

    private int page;
    private int cost_total = 0;
    private int[] cost_by_rarity = new int[] {30, 120, 350, 700, 2000};

    // Use this for initialization
    void Start () {
        if (Scene_Manager.load_deck == true)
        {
            cards_button.SetActive(true);
            deck_button.SetActive(false);
#if UNITY_WEBGL
            file_manager.Load_Deck_File_Web(Scene_Manager.pathfile);
#else
            file_manager.Load_Deck_File(Scene_Manager.pathfile);
#endif
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
        cost_total = 0;
        foreach (Transform child in scroll_content.transform)
        {
            Destroy(child.gameObject);
        }
        int count = 0;

        List<KeyValuePair<int, int>> list_pair = new List<KeyValuePair<int, int>>(deck_manager.Deck_cards);
        list_pair.Sort(delegate(KeyValuePair<int, int> a, KeyValuePair<int, int> b)
        {
            int xdiff = Data_All.data_tab[a.Key].GodType.CompareTo(Data_All.data_tab[b.Key].GodType);
            if (xdiff != 0) return -1 * xdiff;
            else
            {
                int ydiff = Data_All.data_tab[a.Key].CostAP.CompareTo(Data_All.data_tab[b.Key].CostAP);
                if (ydiff != 0) return ydiff;
                else
                {
                    return Data_All.data_tab[a.Key].Texts.NameFR.CompareTo(Data_All.data_tab[b.Key].Texts.NameFR);
                }
            }
        });

        foreach (KeyValuePair<int, int> pair in list_pair)
        {
            GameObject newCard = (GameObject)Instantiate(prefab_deck);
            newCard.GetComponent<Prefab_Script>().index = pair.Key;
            Text component;
            count += pair.Value;
            foreach (Transform child in newCard.transform)
            {
                component = child.GetComponent<Text>();
                if (child.name == "Qantity")
                    component.text = "x" + pair.Value.ToString();
                else if (child.name == "Name")
                    component.text = Data_All.data_tab[pair.Key].Texts.NameFR;
                else if (child.name == "Cost")
                    component.text = Data_All.data_tab[pair.Key].CostAP.ToString();
                else //Rarity
                {
                    child.GetComponent<Image>().sprite = rarity_logo[Data_All.data_tab[pair.Key].Rarity];
                    cost_total += cost_by_rarity[Data_All.data_tab[pair.Key].Rarity] * pair.Value;
                }
            }
            newCard.transform.SetParent(scroll_content.transform);
        }
        total_text.text = "" + count.ToString() + " / 45";
        cost_text.text = "" + cost_total.ToString();

        if (count >= 45)
            lock_panel.SetActive(true);
        else
            lock_panel.SetActive(false);
    }

    public void Next_Page()
    {
        if (Research_Card.current_cards.Count - (page * 8) > 8)
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
                current_img[count] = Resources.Load<Sprite>("Cards/" + Data_All.data_tab[index].Name) as Sprite;
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
                next_img[count] = Resources.Load<Sprite>("Cards/" + Data_All.data_tab[index].Name) as Sprite;
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
        lock_panel.SetActive(false);
    }

    public void Add_to_Deck(int count)
    {
        if (Data_All.data_tab[Research_Card.current_cards[count + (page * 8)]].IsToken == false)
        {
            deck_manager.Add_Card(Research_Card.current_cards[count + (page * 8)]);
            Refresh_Deck();
        }
    }

    public void Return_to_Menu()
    {
        Scene_Manager.God_Scene();
    }
}
