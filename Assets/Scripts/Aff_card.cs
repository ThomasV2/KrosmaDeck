using UnityEngine;
using System.Collections;
using Mono.Data.Sqlite;
using System.Data;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

public class Aff_card : MonoBehaviour {
    
    public GameObject[] cards;
    public Deck_Manager deck_manager;
    public GameObject cards_button;
    public GameObject deck_button;
    public Dropdown Rarity_Dropdown;
    public Text Input_text;
    public Text Check_Input_Text;

    private int[] tab_id;
    private int[] id_by_count = new int[8];
    private int page;
    private List<int> list_id = new List<int>();
    private string type = "";
    private string[] rarity_id = { "", "commune", "peu commune", "rare", "krosmik", "infinite" };

    // Use this for initialization
    void Start () {
        if (Scene_Manager.load_deck == true)
        {
            cards_button.SetActive(true);
            deck_button.SetActive(false);
            deck_manager.Load_Deck_File();
            Aff_Deck();
        }
        else
        {
            cards_button.SetActive(false);
            deck_button.SetActive(true);
            Search_Type(Scene_Manager.card_type);
            Refresh();
        }
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void Refresh()
    {
        for (int count = 0; count < 8; count++)
        {
            if (count + (page * 8) >= tab_id.Length)
            {
                Hide_img(cards[count]);
            }
            else
            {
                Change_img(cards[count], tab_id[count + (page * 8)]);
                id_by_count[count] = tab_id[count + (page * 8)];
            }
        }
    }

    public void Next_Page()
    {
        if (tab_id.GetLength(0) - (page * 8) >= 8)
        {
            page++;
            Refresh();
        }
    }

    public void Prev_Page()
    {
        if (page > 0)
        {
            page--;
            Refresh();
        }
    }

    public void Aff_Deck()
    {
        Data_Deck data;
        list_id.Clear();
        page = 0;
        foreach (GameObject card in deck_manager.Deck_cards)
        {
            data = card.GetComponent<Data_Deck>();
            if (type == "" && data.type != "Neutre")
                this.type = data.type;
            list_id.Add(data.id);
        }
        tab_id = list_id.ToArray();
        Refresh();
    }

    public void Aff_All_Cards()
    {
        list_id.Clear();
        page = 0;
        Search_Type(this.type);
        Refresh();
    }

    public void Search_by_rarity()
    {
        if (Rarity_Dropdown.value != 0)
        {
            list_id.Clear();
            page = 0;
            Search_Rarity(rarity_id[Rarity_Dropdown.value]);
            Refresh();
        }
        else
        {
            Aff_All_Cards();
        }
    }

    public void Search_by_string()
    {
        if (Check_Input_Text.enabled == false)
        {
            list_id.Clear();
            page = 0;
            Search_String(Input_text.text);
            Refresh();
        }
        else
        {
            Aff_All_Cards();
        }
    }

    public void Add_to_Deck(int i)
    {
        string conn = "URI=file:" + Application.dataPath + "/Resources/cards_database.s3db"; //Path to database.
        string name = "";
        string type = "";
        string rarity = "";
        string img = "";
        int cost = 0;

        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT Name, Cost, Type, Rarity, Img FROM Cards WHERE Id = " + id_by_count[i] + ";";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            name = reader.GetString(0);
            //cost = reader.GetInt32(1); // A RAJOUTER DANS LA BASE DE DONNE
            type = reader.GetString(2);
            rarity = reader.GetString(3);
            img = reader.GetString(4);
        }
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
        deck_manager.Add_Card(id_by_count[i], name, cost, type, rarity, img);
    }

    public void Add_to_Deck_by_Id(int id)
    {
        string conn = "URI=file:" + Application.dataPath + "/Resources/cards_database.s3db"; //Path to database.
        string name = "";
        string type = "";
        string rarity = "";
        string img = "";
        int cost = 0;

        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT Name, Cost, Type, Rarity, Img FROM Cards WHERE Id = " + id + ";";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            name = reader.GetString(0);
            //cost = reader.GetInt32(1); // A RAJOUTER DANS LA BASE DE DONNE
            type = reader.GetString(2);
            rarity = reader.GetString(3);
            img = reader.GetString(4);
        }
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
        deck_manager.Add_Card(id, name, cost, type, rarity, img);
    }

    public void Search_Type(string type)
    {
        this.type = type;
        string conn = "URI=file:" + Application.dataPath + "/Resources/cards_database.s3db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT Id FROM Cards WHERE Type = '" + type + "' ORDER BY Cost ASC;";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            list_id.Add(reader.GetInt32(0));
        }
        Additional_Search_Type("Neutre");
        tab_id = list_id.ToArray();
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }

    public void Additional_Search_Type(string type)
    {
        string conn = "URI=file:" + Application.dataPath + "/Resources/cards_database.s3db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT Id FROM Cards WHERE Type = '" + type + "' ORDER BY Cost ASC;";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            list_id.Add(reader.GetInt32(0));
        }
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }

    public void Search_Rarity(string rarity)
    {
        rarity = rarity.ToLower();
        string conn = "URI=file:" + Application.dataPath + "/Resources/cards_database.s3db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT Id FROM Cards WHERE (Type = '" + type + "' OR Type = 'Neutre') AND Rarity = '" + rarity + "' ORDER BY Cost ASC;";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            list_id.Add(reader.GetInt32(0));
        }
        tab_id = list_id.ToArray();
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }
    public void Search_String(string patern)
    {
        patern = patern.ToLower();
        string conn = "URI=file:" + Application.dataPath + "/Resources/cards_database.s3db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT Id FROM Cards WHERE (Type = '" + type + "' OR Type = 'Neutre') AND (Name LIKE '%" + patern + "%' OR Description LIKE '%" + patern + "%') ORDER BY Cost ASC;";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            list_id.Add(reader.GetInt32(0));
        }
        tab_id = list_id.ToArray();
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }

    public void Hide_img(GameObject card)
    {
        card.gameObject.SetActive(false);
    }

    public void Change_img(GameObject card, int id)
    {
        string conn = "URI=file:" + Application.dataPath + "/Resources/cards_database.s3db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT Img FROM Cards WHERE Id = " + id;
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        string path = "";
        while (reader.Read())
        {
            path = reader.GetString(0);
        }
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
        card.GetComponent<Image>().sprite = Resources.Load<Sprite>(path.Remove(0, 1).Replace(".png", ""));
        if (card.active == false)
            card.SetActive(true);
    }
}
