using Szn.Framework.Editor.SQLite3Creator;
using UnityEditor;
using UnityEngine;

public class Sqlite3EditorTest : MonoBehaviour {

    [MenuItem("Framework/Test/Get Excel File")]
    public static void OpenExcelFile()
    {
        Debug.LogError(SQLite3Path.GetSingleExcelPath());
    }

    [MenuItem("Framework/Test/Get Excel Folder")]
    public static void OpenExcelFolder()
    {
        Debug.LogError(SQLite3Path.GetExcelFolder());
    }

    [MenuItem("Framework/Test/Get Script Folder")]
    public static void SaveScriptFolder()
    {
        Debug.LogError(SQLite3Path.GetScriptSaveFolder());
    }

    [MenuItem("Framework/Test/Get Db Folder")]
    public static void SaveDbFolder()
    {
        Debug.LogError(SQLite3Path.GetDbSavePath());
    }
}
