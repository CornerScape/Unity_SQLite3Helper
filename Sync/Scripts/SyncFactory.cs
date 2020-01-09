using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Szn.Framework.Sync
{
    public class SyncProperty
    {
        public readonly Type ClassType;
        public readonly string ClassName;
        public readonly PropertyInfo[] Infos;
        public readonly int InfosLength;
        public readonly Dictionary<string, PropertyInfo> InfoDict;

        public SyncProperty(Type InClassType, Dictionary<int, PropertyInfo> InPropertyInfos)
        {
            ClassType = InClassType;
            ClassName = ClassType.Name;
            InfosLength = InPropertyInfos.Count;
            Infos = new PropertyInfo[InfosLength];
            InfoDict = new Dictionary<string, PropertyInfo>(InfosLength);
            foreach (KeyValuePair<int, PropertyInfo> propertyInfo in InPropertyInfos)
            {
                if (propertyInfo.Key >= InfosLength) throw new IndexOutOfRangeException("Please add the SyncAttribute id in order");
                Infos[propertyInfo.Key] = propertyInfo.Value;
                InfoDict.Add(propertyInfo.Value.Name, propertyInfo.Value);
            }
        }
    }

    public static class SyncFactory
    {
        private static readonly Dictionary<Type, SyncProperty> propertyDict = new Dictionary<Type, SyncProperty>(16);

        public static SyncProperty GetSyncProperty(Type InType)
        {
            SyncProperty property;
            if (propertyDict.TryGetValue(InType, out property)) return property;
            
            try
            {
                PropertyInfo[] infos = InType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                int len = infos.Length;
                Dictionary<int, PropertyInfo> propertyInfoDict = new Dictionary<int, PropertyInfo>(len);

                for (int i = 0; i < len; i++)
                {
                    object[] attrs = infos[i].GetCustomAttributes(SyncConfig.SYNC_ATTR_TYPE, false);

                    if (1 == attrs.Length)
                    {
                        SyncAttribute syncAttribute = attrs[0] as SyncAttribute;
                        if (null != syncAttribute)
                        {
                            if (propertyInfoDict.ContainsKey(syncAttribute.SyncID)) throw new Exception();
                            propertyInfoDict.Add(syncAttribute.SyncID, infos[i]);
                        }
                    }
                }

                property = new SyncProperty(InType, propertyInfoDict);
                propertyDict.Add(InType, property);
            }
            catch (Exception ex)
            {
                Debug.LogError(InType + " Create SyncFactory Error : " + ex.Message);
            }


            return property;
        }

        public static SyncProperty GetSyncProperty<T>() where T : SyncBase
        {
            return GetSyncProperty(typeof(T));
        }

        public static SyncProperty GetSyncProperty(object InObj)
        {
            return GetSyncProperty(InObj.GetType());
        }
    }
}