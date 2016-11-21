using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SimpleJSON;


public class Data_All : MonoBehaviour {

    static public int SIZE_TAB = 518; // Nombre de carte totales
    static public Data_Card[] data_tab; // Tableaux avec toute les données de carte dedans (JSON)

    // Use this for initialization
    void Start()
    {
        Init_data();
    }
    public void Init_data()
    {
        data_tab = new Data_Card[SIZE_TAB];
        TextAsset[] files = Resources.LoadAll<TextAsset>("data");
        int i = 0;
        foreach (TextAsset card in files)
        {
            string str = card.text;
            var N = JSON.Parse(str);
            Data_Card data = new Data_Card();
            data.Id = N["Id"].AsInt;
            data.Name = N["Name"].Value;
            data.CardType = N["CardType"].Value;
            data.CostAP = N["CostAP"].AsInt;
            data.Life = N["Life"].AsInt;
            data.Attack = N["Attack"].AsInt;
            data.MovementPoint = N["MovementPoint"].AsInt;
            //data.Families = N["Families"]
            data.IsToken = N["IsToken"].AsBool;
            data.Rarity = N["Rarity"].AsInt;
            data.GodType = N["GodType"].AsInt;
            data.NameFR = N["NameFR"].Value;
            data.DescriptionFR = N["DescriptionFR"].Value;
            data.Extension = N["Extension"].AsInt;
            data_tab[i] = data;
            i++;
        }
    }

}

public class Data_Card
{
    public string type { get; set; }
    public int Id { get; set; }
    public string Name { get; set; }
    public string CardType { get; set; }
    public int CostAP { get; set; }
    public int Life { get; set; }
    public int Attack { get; set; }
    public int MovementPoint { get; set; }
    public List<int> Families { get; set; }
    public bool IsToken { get; set; }
    public int Rarity { get; set; }
    public int GodType { get; set; }
    public string NameFR { get; set; }
    public string DescriptionFR { get; set; }
    public int Extension { get; set; }
    public List<object> LinkedCards { get; set; }
}
