using NPOI.SS.UserModel;
using Szn.Framework.SQLite3Helper;

namespace Szn.Framework.Editor.SQLite3Creator
{
    public enum SQLite3ValueType
    {
        Integer,
        Real,
        Text,
       // BLOB
    }

    public struct TableData
    {
        public bool IsEnable;
        public string TableName;
        public string[] ColumnName;
        public SQLite3ValueType[] SQLite3Types;
        public SQLite3Constraint[] SQLite3Constraints;
        public string[] CSharpTypes;
        public string[] ColumnDescribes;
        public bool[] IsColumnEnables;
        public bool IsNeedCreateScript;

        public ICell[][] ExcelContents;
    }
    
    public static class SQLite3EditorConfig
    {
        public const int NAME_ROW_INDEX_I = 0;
        public const int TYPE_ROW_INDEX_I = 1;
        public const int DESCRIBE_ROW_INDEX_I = 2;

        public static readonly string EXCEL_CONFIG_DIRECTORY_S = UnityEngine.Application.dataPath +  "/ConfigTable";
        public static readonly string SQLITE3_SCRIPT_SAVE_DIRECTORY_S = UnityEngine.Application.dataPath + "/Scripts/Data/Config/";
        public static readonly string SQLITE3_DATABASE_SAVE_DIRECTORY_S = UnityEngine.Application.streamingAssetsPath + "/";
    }
}
