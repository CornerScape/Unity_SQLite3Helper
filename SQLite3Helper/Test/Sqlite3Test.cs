#if UNIT_TEST
using SQLite3TableDataTmp;
using Szn.Framework.SQLite3Helper;
using UnityEngine;

public class Sqlite3Test : MonoBehaviour
{
    void OnGUI()
    {
        GUI.skin.button.fontSize = 32;
        if (GUILayout.Button("Load Sqlite3 Data"))
        {
            SQLite3Data data = Resources.Load<SQLite3Data>("Sqlite3Data");
            foreach (SQLite3SingleData singleData in data.AllData)
            {
                Debug.LogError(singleData);
            }
        }

        if (GUILayout.Button("Load Static Data"))
        {
            SQLite3Operate operate = SQLite3Factory.OpenToRead("Static.db");
            Debug.LogError(operate.SelectTbyId<LevelConfig>(40000));
        }

        if (GUILayout.Button("Load Dynamic Data"))
        {
            SQLite3Operate operate = SQLite3Factory.OpenToRead("Dynamic.db");
            Debug.LogError(operate.SelectTbyId<LevelConfig>(40002));
        }
    }
}
#endif