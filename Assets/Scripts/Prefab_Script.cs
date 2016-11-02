using UnityEngine;
using System.Collections;

public class Prefab_Script : MonoBehaviour {

    public int index;

    public void Delete_in_Deck()
    {
        GameObject.Find("Canvas").GetComponent<Deck_Manager>().Delete_Card(this.index);
        GameObject.Find("Canvas").GetComponent<Aff_card>().Refresh_Deck();
    }
}
