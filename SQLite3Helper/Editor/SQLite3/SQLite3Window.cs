using System;
using System.Collections.Generic;
using System.IO;
using Szn.Framework.Editor.Excel;
using Szn.Framework.SQLite3Helper;
using Szn.Framework.UtilPackage.Editor;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Szn.Framework.Editor.SQLite3Creator
{
    public class SQLite3Window : EditorWindow
    {
        private static SQLite3Window window;
        private GUIStyle centerTittleStyle, leftTittleStyle;
        private Vector2 scrollPos;
        private float progressValue = 1.0f;

        private string selectPrefKey;
        private bool isSingleFile, preSelect;
        private TableData[][] tableData, preTableData;
        private int sheetLength, rowLength, columnLength;

        private string dataPath;
        
        public static void Init()
        {
            window = CreateInstance<SQLite3Window>();
            //window.LoadExcel(Application.dataPath);
            window.titleContent = new GUIContent("SQLite3", "Create SQLite3 table from excel.");
            window.minSize = new Vector2(555, 600);
            window.maxSize = new Vector2(555, 2000);
            window.ShowUtility();
        }

        //[MenuItem("Framework/Database/Update SQLite3 Version")]
        //private static void UpdateSQLite3Version()
        //{
        //    SQLite3Version version = CreateInstance<SQLite3Version>();

        //    version.DbName = "Static.db";
        //    version.DbMd5 = SQLite3Utility.GetFileMd5(Application.streamingAssetsPath + "/Static.db");

        //    AssetDatabase.CreateAsset(version, "Assets/ThirdPartyPlugin/SQLite3/Resources/SQLite3Version.asset");
        //    AssetDatabase.SaveAssets();
        //    AssetDatabase.Refresh();
        //}

        private string excelPath, excelFolder, scriptFolder, dbPath;

        private void OnEnable()
        {
            dataPath = Application.dataPath;
            dataPath = dataPath.Substring(0, dataPath.Length - "Assets".Length);

            selectPrefKey = string.Format("{0}SingleOrMultiSelectPrefKey", EditorTools.GetCompanyName());
            isSingleFile = preSelect = EditorPrefs.GetBool(selectPrefKey, true);

            excelPath = SQLite3Path.GetSingleExcelPath();
            excelFolder = SQLite3Path.GetExcelFolder();
            scriptFolder = SQLite3Path.GetScriptSaveFolder();
            dbPath = SQLite3Path.GetDbSavePath();

            centerTittleStyle = new GUIStyle
            {
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                normal = new GUIStyleState
                {
                    textColor = EditorGUIUtility.isProSkin ? new Color(.7f, .7f, .7f) : Color.black
                }
            };


            leftTittleStyle = new GUIStyle
            {
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleLeft,
                normal = new GUIStyleState
                {
                    textColor = EditorGUIUtility.isProSkin ? new Color(.7f, .7f, .7f) : Color.black
                }
            };
        }

        private void OnDisable()
        {
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }

        public void LoadExcel(string InExcelPath)
        {
            ExcelData[] excelData = ExcelReader.GetSingleExcelData(InExcelPath);

            #region Dialog
            //for (int i = 0; i < excelData.Length; i++)
            //{
            //    for (int j = 0; j < excelData[i].HeadRowLen; j++)
            //    {
            //        string log = j + ":";
            //        for (int k = 0; k < excelData[i].DataColumnLen; k++)
            //        {
            //            log += excelData[i].Head[j][k] + ",";
            //        }
            //        Debug.LogError(log);
            //    }

            //    for (int j = 0; j < excelData[i].BodyRowLen; j++)
            //    {
            //        string log = j + ":";
            //        for (int k = 0; k < excelData[i].DataColumnLen; k++)
            //        {
            //            if(excelData[i].Body[j][k] == null) continue;

            //            switch (excelData[i].Body[j][k].CellType)
            //            {
            //                case CellType.Unknown:
            //                    log += "Unknown,";
            //                    break;
            //                case CellType.Numeric:
            //                    log += excelData[i].Body[j][k].NumericCellValue + ",";
            //                    break;
            //                case CellType.String:
            //                    log += excelData[i].Body[j][k].StringCellValue + ",";
            //                    break;
            //                case CellType.Formula:
            //                    switch (excelData[i].Body[j][k].CachedFormulaResultType)
            //                    {
            //                        case CellType.Unknown:
            //                            log += "FormulaUnknown,";
            //                            break;
            //                        case CellType.Numeric:
            //                            log += excelData[i].Body[j][k].NumericCellValue + ",";
            //                            break;
            //                        case CellType.String:
            //                            log += excelData[i].Body[j][k].StringCellValue + ",";
            //                            break;
            //                        case CellType.Formula:
            //                            log += "FormulaFormula,";
            //                            break;
            //                        case CellType.Blank:
            //                            log += "FormulaBlank,";
            //                            break;
            //                        case CellType.Boolean:
            //                            log += "FormulaBoolean,";
            //                            break;
            //                        case CellType.Error:
            //                            log += "FormulaError,";
            //                            break;
            //                    }
            //                    break;
            //                case CellType.Blank:
            //                    log += "Blank,";
            //                    break;
            //                case CellType.Boolean:
            //                    log += "Boolean,";
            //                    break;
            //                case CellType.Error:
            //                    log += "Error,";
            //                    break;
            //            }
            //        }
            //        Debug.LogError(log);
            //    }
            //}
            #endregion

            TableData[] data = ConvertExcelToTableData(ref excelData);
            if (null != data) tableData = new[] { data };
        }

        public void LoadExcelDirectory(string InExcelDirectory)
        {
            DirectoryInfo dirInfos = new DirectoryInfo(InExcelDirectory);
            if (dirInfos.Exists)
            {
                FileInfo[] fileInfos = dirInfos.GetFiles();
                int length = fileInfos.Length;
                List<TableData[]> dataList = new List<TableData[]>(length / 2);
                for (int i = 0; i < length; ++i)
                {
                    try
                    {
                        ExcelData[] excelData = ExcelReader.GetSingleExcelData(fileInfos[i].FullName);
                        TableData[] data = ConvertExcelToTableData(ref excelData);
                        if (null != data) dataList.Add(data);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(fileInfos[i].Name + "\nError : " + e.Message);
                    }
                }

                tableData = dataList.ToArray();
            }
        }

        private TableData[] ConvertExcelToTableData(ref ExcelData[] InExcelData)
        {
            TableData[] data = null;
            if (null != InExcelData)
            {
                sheetLength = InExcelData.Length;
                data = new TableData[sheetLength];
                for (int i = 0; i < sheetLength; ++i)
                {
                    data[i].IsEnable = true;
                    data[i].TableName = InExcelData[i].SheetName;

                    columnLength = InExcelData[i].DataColumnLen;
                    data[i].ColumnName = new string[columnLength];
                    data[i].ColumnDescribes = new string[columnLength];
                    data[i].SQLite3Types = new SQLite3ValueType[columnLength];
                    data[i].SQLite3Constraints = new SQLite3Constraint[columnLength];
                    data[i].CSharpTypes = new string[columnLength];
                    data[i].IsColumnEnables = new bool[columnLength];
                    data[i].IsNeedCreateScript = true;
                    data[i].ExcelContents = InExcelData[i].Body;

                    for (int j = 0; j < columnLength; ++j)
                    {
                        data[i].ColumnName[j] = InExcelData[i].Head[SQLite3EditorConfig.NAME_ROW_INDEX_I][j].StringCellValue;

                        data[i].CSharpTypes[j] = InExcelData[i].Head[SQLite3EditorConfig.TYPE_ROW_INDEX_I][j].StringCellValue;

                        switch (data[i].CSharpTypes[j])
                        {
                            case "short":
                            case "int":
                            case "bool":
                            case "long":
                                data[i].SQLite3Types[j] = SQLite3ValueType.Integer;
                                break;
                            case "float":
                            case "double":
                                data[i].SQLite3Types[j] = SQLite3ValueType.Real;
                                break;
                            case "string":
                                data[i].SQLite3Types[j] = SQLite3ValueType.Text;
                                break;
                            default:
                                if (data[i].CSharpTypes[j].Contains("[]")) data[i].SQLite3Types[j] = SQLite3ValueType.Text;
                                else throw new NotSupportedException();//data[i].SQLite3Types[j] = SQLite3ValueType.BLOB;
                                break;
                        }

                        data[i].ColumnDescribes[j] = InExcelData[i].Head[SQLite3EditorConfig.DESCRIBE_ROW_INDEX_I][j].StringCellValue;

                        if (0 == j) data[i].SQLite3Constraints[j] =/* SQLite3Constraint.PrimaryKey | */SQLite3Constraint.Unique | SQLite3Constraint.NotNull;
                        else data[i].SQLite3Constraints[j] = SQLite3Constraint.Default;
                        data[i].IsColumnEnables[j] = true;
                    }
                }
            }

            return data;
        }

        private void OnGUI()
        {
            GUILayout.Label("Excel To SQLite3 Table", EditorStyles.boldLabel);

            EditorGUILayout.BeginVertical("box");
            {
                EditorGUILayout.BeginVertical("box");
                {
                    GUILayout.BeginHorizontal();
                    {
                        isSingleFile = EditorGUILayout.ToggleLeft("Single Excel", isSingleFile, GUILayout.Width(245));
                        isSingleFile = !EditorGUILayout.ToggleLeft("Excel Directory", !isSingleFile, GUILayout.Width(245));

                        if (preSelect != isSingleFile)
                        {
                            TableData[][] temp = tableData;
                            tableData = preTableData;
                            preTableData = temp;

                            preSelect = isSingleFile;

                            EditorPrefs.SetBool(selectPrefKey, isSingleFile);
                        }
                    }

                    GUILayout.EndHorizontal();
                    GUILayout.Space(15);

                    if (isSingleFile)
                    {
                        GUILayout.BeginHorizontal();
                        {
                            excelPath = EditorGUILayout.TextField("Excel Path", excelPath, GUILayout.Width(440));
                            if (GUILayout.Button("Select", GUILayout.MaxWidth(88)))
                            {
                                excelPath = SQLite3Path.SelectExcelPath();
                            }
                        }
                        GUILayout.EndHorizontal();

                        if (GUILayout.Button("Preview"))
                        {
                            if (string.IsNullOrEmpty(excelPath))
                                EditorUtility.DisplayDialog("Tips", "Please select an excel file first.", "OK");
                            else
                                LoadExcel(Path.Combine(dataPath, excelPath));
                        }
                    }
                    else
                    {
                        GUILayout.BeginHorizontal();
                        {
                            excelFolder = EditorGUILayout.TextField("Excel Directory", excelFolder, GUILayout.Width(440));
                            if (GUILayout.Button("Select", GUILayout.MaxWidth(88)))
                            {
                                excelFolder = SQLite3Path.SelectExcelFolder();
                            }
                        }
                        GUILayout.EndHorizontal();

                        if (GUILayout.Button("Preview"))
                        {
                            if (string.IsNullOrEmpty(excelFolder))
                                EditorUtility.DisplayDialog("Tips", "Please select a directory where excel is stored.", "OK");
                            else
                                LoadExcelDirectory(Path.Combine(dataPath, excelFolder));
                        }
                    }
                }
                EditorGUILayout.EndVertical();

                if (null != tableData)
                {
                    sheetLength = tableData.Length;
                    //config = isSingleFile ? singlePathConfig : multiPathConfig;
                    EditorGUILayout.BeginVertical("box");
                    {
                        GUILayout.BeginHorizontal();
                        {
                            dbPath = EditorGUILayout.TextField("Database Save Path", dbPath, GUILayout.Width(440));
                            if (GUILayout.Button("Select", GUILayout.MaxWidth(88)))
                            {
                                dbPath = SQLite3Path.SelectDbSavePath();
                            }
                        }
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        {
                            scriptFolder = EditorGUILayout.TextField("Script Save Directory", scriptFolder, GUILayout.Width(440));
                            if (GUILayout.Button("Select", GUILayout.MaxWidth(88)))
                            {
                                scriptFolder = SQLite3Path.SelectScriptSaveFolder();
                            }
                        }
                        GUILayout.EndHorizontal();


                        if (!isSingleFile && sheetLength > 1)
                        {

                            if (GUILayout.Button("Create All"))
                            {
                                try
                                {
                                    if (string.IsNullOrEmpty(dbPath))
                                    {
                                        EditorUtility.DisplayDialog("Tips", "Please select the storage location of the database.", "OK");
                                    }
                                    else
                                    {
                                        if (string.IsNullOrEmpty(scriptFolder))
                                        {
                                            EditorUtility.DisplayDialog("Tips", "Please select the storage location of the script.", "OK");
                                        }
                                        else
                                        {
                                            for (int i = 0; i < sheetLength; i++)
                                            {
                                                rowLength = tableData[i].Length;
                                                for (int j = 0; j < rowLength; j++)
                                                {
                                                    if (tableData[i][j].IsEnable)
                                                    {
                                                        progressValue = 1.0f;
                                                        if (tableData[i][j].IsNeedCreateScript)
                                                        {
                                                            progressValue = .5f;
                                                            EditorUtility.DisplayProgressBar("Convert excel to C# script...", "Convert excel named: " + tableData[i][j].TableName, i * progressValue / sheetLength);
                                                            ScriptWriter.Writer(string.Format("{0}{1}.cs",Path.Combine(dataPath, scriptFolder), tableData[i][j].TableName), ref tableData[i][j]);
                                                        }
                                                        EditorUtility.DisplayProgressBar("Convert excel to SQLite3 table...", "Convert excel named: " + tableData[i][j].TableName, i * .5f / sheetLength);

                                                        SQLite3Creator.Creator(ref tableData[i][j],Path.Combine(dataPath, dbPath));
                                                    }
                                                }
                                            }
                                            EditorUtility.DisplayProgressBar("CompileCSharp Script...", "Please Waiting...", 1.5f);

                                            EditorUtility.ClearProgressBar();

                                            EditorUtility.DisplayDialog("Tips", "Convert excel to SQLite3 table finished.", "OK");

                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    EditorUtility.DisplayDialog("Error", "Convert excel to SQLite3 table has an error:" + ex.Message, "OK");
                                }
                                Close();
                            }

                        }
                    }
                    EditorGUILayout.EndVertical();

                    scrollPos = GUILayout.BeginScrollView(scrollPos);
                    for (int i = 0; i < sheetLength; ++i)
                    {
                        if (null != tableData[i])
                        {
                            rowLength = tableData[i].Length;
                            for (int j = 0; j < rowLength; ++j)
                            {
                                EditorGUILayout.BeginVertical("box");
                                {
                                    GUILayout.BeginHorizontal();
                                    {
                                        tableData[i][j].TableName = EditorGUILayout.TextField("Table Name",
                                            tableData[i][j].TableName, GUILayout.Width(440));

                                        tableData[i][j].IsEnable = EditorGUILayout.ToggleLeft("Enable",
                                            tableData[i][j].IsEnable, GUILayout.Width(88));
                                    }
                                    GUILayout.EndHorizontal();

                                    if (tableData[i][j].IsEnable)
                                    {
                                        GUILayout.Space(10);
                                        EditorGUILayout.BeginVertical("box");
                                        {
                                            GUILayout.BeginHorizontal();
                                            {
                                                EditorGUILayout.LabelField("Property Name", centerTittleStyle, GUILayout.Width(104));
                                                GUILayout.Space(4);
                                                EditorGUILayout.LabelField("Property Describe", centerTittleStyle, GUILayout.Width(286));
                                                GUILayout.Space(4);
                                                EditorGUILayout.LabelField("SQLite3 Type", centerTittleStyle, GUILayout.Width(104));

                                            }
                                            GUILayout.EndHorizontal();
                                            GUILayout.Space(10);

                                            columnLength = tableData[i][j].ColumnName.Length;
                                            for (int k = 0; k < columnLength; ++k)
                                            {
                                                tableData[i][j].IsColumnEnables[k] = EditorGUILayout.BeginToggleGroup(
                                                    "Enable",
                                                    tableData[i][j].IsColumnEnables[k]);
                                                {
                                                    GUILayout.BeginVertical("box");
                                                    {
                                                        GUILayout.BeginHorizontal();
                                                        {
                                                            tableData[i][j].ColumnName[k] =
                                                                EditorGUILayout.TextField(tableData[i][j].ColumnName[k],
                                                                    GUILayout.MaxWidth(104));
                                                            GUILayout.Space(4);
                                                            tableData[i][j].ColumnDescribes[k] =
                                                                EditorGUILayout.TextField(tableData[i][j].ColumnDescribes[k],
                                                                    GUILayout.MaxWidth(286));
                                                            GUILayout.Space(4);
                                                            tableData[i][j].SQLite3Types[k] =
                                                                (SQLite3ValueType)
                                                                    EditorGUILayout.EnumPopup(tableData[i][j].SQLite3Types[k],
                                                                        GUILayout.MaxWidth(104));

                                                            //tableData[i][j].SQLite3Constraints[k] =
                                                            //    (SQLite3Constraint)
                                                            //        EditorGUILayout.EnumPopup(tableData[i][j].SQLite3Constraints[k],
                                                            //            GUILayout.MaxWidth(100));
                                                        }
                                                        GUILayout.EndHorizontal();
                                                        GUILayout.Space(10);
                                                        GUILayout.BeginHorizontal("box");
                                                        {
                                                            SQLite3Constraint constraint = tableData[i][j].SQLite3Constraints[k];
                                                            bool isPrimaryKey = (constraint & SQLite3Constraint.PrimaryKey) != 0;
                                                            bool isAutoIncrement = (constraint & SQLite3Constraint.AutoIncrement) != 0;
                                                            bool isNotNull = (constraint & SQLite3Constraint.NotNull) != 0;
                                                            bool isUnique = (constraint & SQLite3Constraint.Unique) != 0;

                                                            EditorGUILayout.LabelField("SQLite3 Constraint:", leftTittleStyle, GUILayout.Width(114));
                                                            isPrimaryKey = EditorGUILayout.ToggleLeft("PrimaryKey", isPrimaryKey, GUILayout.Width(80));

                                                            if (tableData[i][j].SQLite3Types[k] != SQLite3ValueType.Integer)
                                                            {
                                                                GUI.enabled = false;
                                                                isAutoIncrement = false;
                                                            }

                                                            isAutoIncrement = EditorGUILayout.ToggleLeft("AutoIncrement", isAutoIncrement, GUILayout.Width(104));
                                                            GUI.enabled = true;

                                                            if (isPrimaryKey)
                                                            {
                                                                isUnique = false;
                                                                isNotNull = false;
                                                                GUI.enabled = false;
                                                            }
                                                            isNotNull = EditorGUILayout.ToggleLeft("NotNull", isNotNull, GUILayout.Width(60));
                                                            isUnique = EditorGUILayout.ToggleLeft("Unique", isUnique, GUILayout.Width(60));
                                                            GUI.enabled = true;
                                                            bool isDefault = !(isPrimaryKey || isAutoIncrement || isNotNull || isUnique);
                                                            isDefault = EditorGUILayout.ToggleLeft("Default", isDefault, GUILayout.Width(60));
                                                            if (isDefault) isPrimaryKey = isAutoIncrement = isNotNull = isUnique = false;

                                                            if (isDefault) constraint = SQLite3Constraint.Default;
                                                            else constraint = constraint & ~SQLite3Constraint.Default;

                                                            if (isPrimaryKey)
                                                            {
                                                                for (int m = 0; m < columnLength; m++)
                                                                {
                                                                    tableData[i][j].SQLite3Constraints[m] &= ~SQLite3Constraint.PrimaryKey;
                                                                }
                                                                constraint |= SQLite3Constraint.PrimaryKey;
                                                            }
                                                            else constraint = constraint & ~SQLite3Constraint.PrimaryKey;

                                                            if (isAutoIncrement) constraint |= SQLite3Constraint.AutoIncrement;
                                                            else constraint &= ~SQLite3Constraint.AutoIncrement;

                                                            if (isNotNull) constraint |= SQLite3Constraint.NotNull;
                                                            else constraint &= ~SQLite3Constraint.NotNull;

                                                            if (isUnique) constraint |= SQLite3Constraint.Unique;
                                                            else constraint &= ~SQLite3Constraint.Unique;

                                                            tableData[i][j].SQLite3Constraints[k] = constraint;
                                                        }
                                                        GUILayout.EndHorizontal();
                                                    }
                                                    GUILayout.EndVertical();
                                                }
                                                EditorGUILayout.EndToggleGroup();
                                            }

                                            EditorGUILayout.BeginHorizontal("box");
                                            {
                                                tableData[i][j].IsNeedCreateScript =
                                                EditorGUILayout.ToggleLeft("Need create or update script?",
                                                    tableData[i][j].IsNeedCreateScript, GUILayout.Width(258));
                                                GUILayout.Space(10);
                                                if (GUILayout.Button("Create", GUILayout.Width(242)))
                                                {
                                                    try
                                                    {
                                                        if (string.IsNullOrEmpty(dbPath))
                                                        {
                                                            EditorUtility.DisplayDialog("Tips", "Please select the storage location of the database.", "OK");
                                                        }
                                                        else
                                                        {
                                                            if (string.IsNullOrEmpty(scriptFolder))
                                                            {
                                                                EditorUtility.DisplayDialog("Tips", "Please select the storage location of the script.", "OK");
                                                            }
                                                            else
                                                            {
                                                                if (tableData[i][j].IsNeedCreateScript) ScriptWriter.Writer(string.Format("{0}{1}.cs", Path.Combine(dataPath, scriptFolder), tableData[i][j].TableName), ref tableData[i][j]);
                                                                SQLite3Creator.Creator(ref tableData[i][j], Path.Combine(dataPath, dbPath));

                                                                EditorUtility.DisplayDialog("Tips", "Convert excel to SQLite3 table finished.", "OK");

                                                                if (isSingleFile) Close();
                                                            }
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        EditorUtility.DisplayDialog("Error", "Convert excel to SQLite3 table has an error:" + ex.Message, "OK");
                                                    }
                                                }
                                            }
                                            EditorGUILayout.EndHorizontal();

                                        }
                                        EditorGUILayout.EndVertical();
                                    }
                                }
                                EditorGUILayout.EndVertical();
                            }
                        }
                    }
                    EditorGUILayout.EndScrollView();
                }
            }
            EditorGUILayout.EndVertical();
        }
    }
}