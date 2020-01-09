using System.Collections.Generic;
using System.IO;
using Szn.Framework.SQLite3Helper;
using Szn.Framework.UtilPackage;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

public class SQLite3BuildProcess : IPreprocessBuild
{
    public int callbackOrder { get; private set; }

    public void OnPreprocessBuild(BuildTarget InTarget, string InPath)
    {
        string streamingAssetsPath = Application.streamingAssetsPath;
        List<FileInfo> fileInfos = FileTools.GetAllFileInfos(streamingAssetsPath);
        if(null == fileInfos) return;
        List<FileInfo> dbFileInfos = new List<FileInfo>();
        int count = fileInfos.Count;
        for (int i = 0; i < count; i++)
        {
            if (fileInfos[i].Extension == ".db")
            {
                dbFileInfos.Add(fileInfos[i]);
            }
        }

        SQLite3Data data = Resources.Load<SQLite3Data>("Sqlite3Data");
        string dataPath = AssetDatabase.GetAssetPath(data);
        data = ScriptableObject.CreateInstance<SQLite3Data>();
        int dbCount = dbFileInfos.Count;
        data.AllData = new List<SQLite3SingleData>(dbCount);
        for (int i = 0; i < dbCount; i++)
        {
            SQLite3SingleData singleData = new SQLite3SingleData
            {
                Name =  dbFileInfos[i].Name,
                LocalName = MD5Tools.GetStringMd5(dbFileInfos[i].Name),
                Md5 = MD5Tools.GetFileMd5(dbFileInfos[i].FullName)
            };
            string dirPath = dbFileInfos[i].DirectoryName;
            singleData.Directory = string.IsNullOrEmpty(dirPath) || dirPath == streamingAssetsPath ? string.Empty : dirPath.Replace('\\', '/').Replace(streamingAssetsPath, string.Empty);
            
            data.AllData.Add(singleData);
        }
        AssetDatabase.CreateAsset(data, dataPath);
        AssetDatabase.SaveAssets();
    }
}