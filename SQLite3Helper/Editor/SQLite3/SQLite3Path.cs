using Szn.Framework.UtilPackage.Editor;
using UnityEngine;

namespace Szn.Framework.Editor.SQLite3Creator
{
    public static class SQLite3Path
    {
        public static string GetSingleExcelPath()
        {
            return PlayerPrefs.GetString(string.Format("{0}SingleExcel", EditorTools.GetCompanyName()));
        }

        public static string SelectExcelPath()
        {
            string prefKey = string.Format("{0}SingleExcel", EditorTools.GetCompanyName());
            string excelPath = PlayerPrefs.GetString(prefKey);

            if (string.IsNullOrEmpty(excelPath)) excelPath = Application.dataPath;
            else
            {
                if (excelPath.IndexOf('\\') != -1) excelPath = excelPath.Replace('\\', '/');
                int index = excelPath.LastIndexOf('/');
                excelPath = excelPath.Substring(0, index + 1);
            }

            excelPath = EditorTools.OpenAssetsFile("Open Excel File", excelPath, "xlsx,xls");

            if (string.IsNullOrEmpty(excelPath)) Dialog.Error("Excel file path can not be empty.");
            else PlayerPrefs.SetString(prefKey, excelPath);

            return excelPath;
        }

        public static string GetExcelFolder()
        {
            return PlayerPrefs.GetString(string.Format("{0}ExcelFolder", EditorTools.GetCompanyName()));
        }

        public static string SelectExcelFolder()
        {
            string prefKey = string.Format("{0}ExcelFolder", EditorTools.GetCompanyName());
            string excelPath = PlayerPrefs.GetString(prefKey);

            if (string.IsNullOrEmpty(excelPath)) excelPath = Application.dataPath;

            excelPath = EditorTools.SaveAssetsFolder("Open Excel Folder", excelPath);
            if (string.IsNullOrEmpty(excelPath)) Dialog.Error("Excel folder path can not be empty.");
            else PlayerPrefs.SetString(prefKey, excelPath);

            return excelPath;
        }

        public static string GetScriptSaveFolder()
        {
            return PlayerPrefs.GetString(string.Format("{0}ScriptFolder", EditorTools.GetCompanyName()));
        }

        public static string SelectScriptSaveFolder()
        {
            string prefKey = string.Format("{0}ScriptFolder", EditorTools.GetCompanyName());
            string excelPath = PlayerPrefs.GetString(prefKey);

            if (string.IsNullOrEmpty(excelPath)) excelPath = Application.dataPath;

            excelPath = EditorTools.SaveAssetsFolder("Save Script Folder", excelPath);
            if (string.IsNullOrEmpty(excelPath)) Dialog.Error("Script folder path can not be empty.");
            else PlayerPrefs.SetString(prefKey, excelPath);

            return excelPath;
        }

        public static string GetDbSavePath()
        {
            return PlayerPrefs.GetString(string.Format("{0}DbPath", EditorTools.GetCompanyName()));
        }

        public static string SelectDbSavePath()
        {
            string prefKey = string.Format("{0}DbPath", EditorTools.GetCompanyName());
            string excelPath = PlayerPrefs.GetString(prefKey);

            if (string.IsNullOrEmpty(excelPath)) excelPath = Application.dataPath;
            else
            {
                if (excelPath.IndexOf('\\') != -1) excelPath = excelPath.Replace('\\', '/');
                int index = excelPath.LastIndexOf('/');
                excelPath = excelPath.Substring(0, index + 1);
            }

            excelPath = EditorTools.SaveAssetsFile("Save Database File", excelPath, "Static", "db");

            if (string.IsNullOrEmpty(excelPath)) Dialog.Error("Database file path can not be empty.");
            else PlayerPrefs.SetString(prefKey, excelPath);
            
            return excelPath;
        }
    }
}