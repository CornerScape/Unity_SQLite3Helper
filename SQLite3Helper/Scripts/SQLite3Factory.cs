using System;
using System.IO;
using UnityEngine;

namespace Szn.Framework.SQLite3Helper
{
    public static class SQLite3Factory
    {
        /// <summary>
        /// If there is no database in the persistentDataPath directory, or database in the persistentDataPath directory md5 not match original,
        /// Then copy the database from the streamingAssetsPath directory to the persistentDataPath directory and open the database in read-only mode
        /// </summary>
        /// <param name="InDbName">The name of the SQLite3 database.</param>
        /// <returns>Operation SQLite3 database handle.</returns>
        public static SQLite3Operate OpenToRead(string InDbName)
        {
            SQLite3Data data = Resources.Load<SQLite3Data>("Sqlite3Data");
            if (null == data || null == data.AllData) throw new Exception("Not found sqlite3 data file in '/SQLite3Helper/Resources/Sqlite3Data.asset'");
            int count = data.AllData.Count;
            SQLite3SingleData singleData = null;
            for (int i = 0; i < count; i++)
            {
                if (data.AllData[i].Name == InDbName)
                {
                    singleData = data.AllData[i];
                    break;
                }
            }

            if (singleData == null) throw new Exception("Not found sqlite3 data named + " + InDbName);

#if UNITY_EDITOR
            string dbPath = Application.streamingAssetsPath;
            if (!string.IsNullOrEmpty(singleData.Directory)) dbPath = Path.Combine(dbPath, singleData.Directory);
            dbPath = Path.Combine(dbPath, singleData.Name);
            if (!File.Exists(dbPath)) throw new Exception("Not found sqlite3 file named + " + InDbName);
#else
            string dbPath = string.Format("{0}/{1}/", Application.persistentDataPath, singleData.Directory);
            if (!Directory.Exists(dbPath)) Directory.CreateDirectory(dbPath);
            dbPath = string.Format("{0}{1}.png",dbPath, singleData.LocalName);

            Debug.LogError(dbPath);
            bool needUpdate = true;
            if (File.Exists(dbPath))
            {
                if (Szn.Framework.UtilPackage.MD5Tools.GetFileMd5(dbPath) == singleData.Md5) needUpdate = false;
            }

            if (needUpdate)
            {
#if UNITY_ANDROID
                string streamPath = string.IsNullOrEmpty(singleData.Directory)
                    ? string.Format("jar:file://{0}!/assets/{1}", Application.dataPath, singleData.Name)
                    : string.Format("jar:file://{0}!/assets{1}/{2}", Application.dataPath, singleData.Directory, singleData.Name);

                using (WWW www = new WWW(streamPath))
                {
                    while (!www.isDone)
                    {

                    }

                    if (string.IsNullOrEmpty(www.error)) File.WriteAllBytes(dbPath, www.bytes);
                    else Debug.LogError("www error " + www.error);
                }
#elif UNITY_IOS
            string streamPath = string.IsNullOrEmpty(singleData.Directory)
                ? string.Format("{0}/{1}", Application.streamingAssetsPath, singleData.Name)
                : string.Format("{0}{1}/{2}", Application.streamingAssetsPath, singleData.Directory, singleData.Name);

                File.Copy(streamPath, dbPath, true);
#endif
            }

#endif
            return new SQLite3Operate(dbPath, SQLite3OpenFlags.ReadOnly);
        }

        /// <summary>
        /// Open a SQLite3 database that exists in the persistentDataPath directory as read-write,
        /// If the database does not exist, create an empty database.
        /// </summary>
        /// <param name="InDbName">The name of the SQLite3 database.</param>
        /// <returns>Operation SQLite3 database handle.</returns>
        public static SQLite3Operate OpenToWrite(string InDbName)
        {
            string persistentDbPath = Path.Combine(Application.persistentDataPath, InDbName);

            return new SQLite3Operate(persistentDbPath, SQLite3OpenFlags.Create | SQLite3OpenFlags.ReadWrite);
        }
    }
}