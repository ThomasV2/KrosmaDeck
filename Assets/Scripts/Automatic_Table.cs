using UnityEngine;
using System.Collections;
using System.IO;
using Mono.Data.Sqlite;
using System.Data;
using System;

public class Automatic_Table : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        //My_command("UPDATE Cards SET Cost = 2, Atk = 0 WHERE Id = 23;");
    }

    /*void Start()
    {
        DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "/Resources/Cards");
        DirectoryInfo[] info = dir.GetDirectories("*.*");
        string data = "";
        uint id = 1;
        foreach (DirectoryInfo type in info)
        {
            Debug.LogError("classe is: " + type.Name);
            foreach (DirectoryInfo rarity in type.GetDirectories("*.*"))
            {
                Debug.LogError("rarity is : " + rarity.Name);
                foreach (FileInfo card in rarity.GetFiles("*.png"))
                {
                    data += "(" + id + ", '" + card.Name.Replace(".png", "").Replace("_", " ") + "', '" + type.Name + "', '" + rarity.Name + "', '" + "/Cards/" + type.Name + "/" + rarity.Name + "/" + card.Name + "')";
              
                    data += ", ";
                    id++;
                }
            }
        }
        data = data.Remove(data.Length - 2);
        data += ";";
        //Debug.LogError(data.Remove(0, 30000));


        string conn = "URI=file:" + Application.dataPath + "/Resources/cards_database.s3db"; //Path to database.
         IDbConnection dbconn;
         dbconn = (IDbConnection)new SqliteConnection(conn);
         dbconn.Open(); //Open connection to the database.
         IDbCommand dbcmd = dbconn.CreateCommand();
         data.Remove(data.Length - 1);
        // "CREATE TABLE \"Cards\" ( `Id` INTEGER, `Name` TEXT, `Type` TEXT, `Rarity` TEXT, `Img` TEXT, `Description` TEXT, `Cost` INTEGER, `Atk` INTEGER, `Mvt` INTEGER, `Life` INTEGER, `Stars` INTEGER, `Range` INTEGER );";
        string sqlQuery = "INSERT INTO Cards (Id, Name, Type, Rarity, Img) VALUES " + data + ";";
         dbcmd.CommandText = sqlQuery;
         IDataReader reader = dbcmd.ExecuteReader();
         while (reader.Read())
         {
         }
         reader.Close();
         reader = null;
         dbcmd.Dispose();
         dbcmd = null;
         dbconn.Close();
         dbconn = null;
         Debug.LogError("fini");
    }*/
    void My_command(string command)
    {
        string conn = "URI=file:" + Application.dataPath + "/Resources/cards_database.s3db"; //Path to database.
         IDbConnection dbconn;
         dbconn = (IDbConnection)new SqliteConnection(conn);
         dbconn.Open(); //Open connection to the database.
         IDbCommand dbcmd = dbconn.CreateCommand();
         string sqlQuery = command;
         dbcmd.CommandText = sqlQuery;
         IDataReader reader = dbcmd.ExecuteReader();
         while (reader.Read())
         {
         }
         reader.Close();
         reader = null;
         dbcmd.Dispose();
         dbcmd = null;
         dbconn.Close();
         dbconn = null;
         Debug.LogError("fini");

    }

    // Update is called once per frame
    void Update () {
	
	}
}
