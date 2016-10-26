using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Data_Deck : MonoBehaviour {

    public string name;
    public int id;
    public int cost;
    public int quantity;
    public string type;
    public string rarity;
    public string img;

    public Text name_text;
    public Text cost_text;
    public Text quantity_text;

    private Deck_Manager manager;

    // Use this for initialization
    void Start () {
        manager = GameObject.Find("Canvas").GetComponent<Deck_Manager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Set_Name(string name)
    {
        this.name = name;
        name_text.text = name;
    }
    public void Set_Quantity(int quantity)
    {
        this.quantity = quantity;
        quantity_text.text = quantity.ToString();
    }
    public void Set_Cost(int cost)
    {
        this.cost = cost;
        cost_text.text = cost.ToString();
    }
    public void Set_Type(string type)
    {
        this.type = type;
    }
    public void Set_Rarity(string rarity)
    {
        this.rarity = rarity;
    }
    public void Set_Img(string img)
    {
        this.img = img;
    }
    public void Delete()
    {
        manager.Delete_Card(this.gameObject);
    }
}
