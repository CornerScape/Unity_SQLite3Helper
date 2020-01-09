using System;
using System.Collections.Generic;
using UnityEngine;

namespace Szn.Framework.SQLite3Helper
{
    [Serializable]
    public class SQLite3SingleData
    {
        public string Directory;
        public string Name;
        public string LocalName;
        public string Md5;

        public override string ToString()
        {
            return "Directory = " + Directory + "; Name = " + Name + "; LocalName = " + LocalName + "; Md5 = " + Md5;
        }
    }
    
    public class SQLite3Data : ScriptableObject
    {
        public List<SQLite3SingleData> AllData;
    }
}
