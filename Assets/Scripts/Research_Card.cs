using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum GodTypes
{
    Neutre = 0,
    Iop = 1,
    Cra = 2,
    Eniripsa = 3,
    Ecaflip = 4,
    Sram = 6,
    Xelor = 7,
    Sacrieur = 8,
    Sadida = 10
};

public enum Rarity
{
    All = -1,
    Commune = 0,
    Peu_Commune = 1,
    Rare = 2,
    Krosmique = 3,
    Infinite = 4
}

public class Research_Card : MonoBehaviour {

    static public List<int> current_cards = new List<int>(); // int = position dans data_all
    public Rarity rarity = Rarity.All;
    public bool[] costAP = new bool[8] {false, false, false, false, false, false, false, false};
    public string description = "";

    void Start()
    {
    }

    public void Research()
    {
        //Execute la recherche selon les critères présent
        current_cards.Clear();
        //On initialise la list des carte potentielle (Dieu + Neutre)
        Research_GodType();
        //On ajoute la recherche par rareté si elle est présente
        if (this.rarity != Rarity.All)
            Research_Rarity();
        //Ensuite, les différents coûts en PA possible. On enlève des cartes les couts en 'false'
        for (int i = 0; i < 8; i++)
        {
            if (costAP[i] == true)
            {
                Research_CostAP();
                break;
            }
        }
        // Voir pour les familles/type ?
        //Enfin, selon leur description ou nom
        Research_String();
        current_cards.Sort(delegate (int a, int b)
        {
            int xdiff = Data_All.data_tab[a].GodType.CompareTo(Data_All.data_tab[b].GodType);
            if (xdiff != 0) return  -1 * xdiff;
            else return Data_All.data_tab[a].CostAP.CompareTo(Data_All.data_tab[b].CostAP);
        });
    }

    void Research_GodType()
    {
        for (int i = 0; i < Data_All.SIZE_TAB; i++)
        {
            if (Data_All.data_tab[i].GodType == (int)Scene_Manager.godType || Data_All.data_tab[i].GodType == (int)GodTypes.Neutre)
            {
                current_cards.Add(i);
            }
        }
    }

    void Research_Rarity()
    {
        for (int index = current_cards.Count - 1; index >= 0; index--)
        {
            if (Data_All.data_tab[current_cards[index]].Rarity != (int)rarity)
                current_cards.Remove(current_cards[index]);
        }
    }
    
    void Research_CostAP()
    {
        for (int index = current_cards.Count - 1; index >= 0; index--)
        {
            if (Data_All.data_tab[current_cards[index]].CostAP >= 7)
            {
                if (costAP[7] == false)
                    current_cards.Remove(current_cards[index]);
            }
            else if (costAP[Data_All.data_tab[current_cards[index]].CostAP] == false)
            {
                current_cards.Remove(current_cards[index]);
            }
        }
    }

    void Research_String()
    {
        // DO SOMETHING
    }

    //-1 car le dropdown commence a 0 et non -1
    public void Set_Rarity(int rarity)
    {
        this.rarity = (Rarity)(rarity - 1);
    }

    public void Set_CostAP(int index)
    {
        costAP[index] = !costAP[index];
    }

}
