using System;
using System.Reflection;
using UnityEngine;

namespace Szn.Framework.Sync
{
    public abstract class SyncBase
    {
        private readonly Delegate[] propertyDelegates;

        protected SyncProperty Property;

        protected SyncBase()
        {
            Property = SyncFactory.GetSyncProperty(GetType());
            
            propertyDelegates = new Delegate[Property.InfosLength];
        }

        public virtual void OnSyncOne(int InIndex, bool InValue)
        {
            if (InIndex > -1 && InIndex < Property.InfosLength)
            {
                PropertyInfo info = Property.Infos[InIndex];
                if (info.PropertyType == SyncConfig.BOOL_TYPE)
                {
                    bool oldValue = (bool)info.GetValue(this, null);
                    if (oldValue != InValue)
                    {
                        info.SetValue(this, InValue, null);
                        InvokePropertyChanged(InIndex, info.Name, oldValue, InValue);
                    }
                }
                else throw new Exception("The type of the property is \"" + info.PropertyType + "\", and the input parameter type is \"bool\".");
            }
            else throw new IndexOutOfRangeException();
        }

        public virtual void OnSyncOne(int InIndex, char InValue)
        {
            if (InIndex > -1 && InIndex < Property.InfosLength)
            {
                PropertyInfo info = Property.Infos[InIndex];
                if (info.PropertyType == SyncConfig.CHAR_TYPE)
                {
                    char oldValue = (char)info.GetValue(this, null);
                    if (oldValue != InValue)
                    {
                        info.SetValue(this, InValue, null);
                        InvokePropertyChanged(InIndex, info.Name, oldValue, InValue);
                    }
                }
                else throw new Exception("The type of the property is \"" + info.PropertyType + "\", and the input parameter type is \"char\".");
            }
            else throw new IndexOutOfRangeException();
        }

        public virtual void OnSyncOne(int InIndex, short InValue)
        {
            if (InIndex > -1 && InIndex < Property.InfosLength)
            {
                PropertyInfo info = Property.Infos[InIndex];
                if (info.PropertyType == SyncConfig.SHORT_TYPE)
                {
                    short oldValue = (short)info.GetValue(this, null);
                    if (oldValue != InValue)
                    {
                        info.SetValue(this, InValue, null);
                        InvokePropertyChanged(InIndex, info.Name, oldValue, InValue);
                    }
                }
                else throw new Exception("The type of the property is \"" + info.PropertyType + "\", and the input parameter type is \"short\".");
            }
            else throw new IndexOutOfRangeException();
        }

        /// <summary>
        /// 此处需要注意的是当传递参数为short，long的数值，
        /// 以及float，double只有整数部分时，
        /// 会默认为int，需要根据属性值进行转发
        /// </summary>
        public virtual void OnSyncOne(int InIndex, int InValue)
        {
            if (InIndex > -1 && InIndex < Property.InfosLength)
            {
                PropertyInfo info = Property.Infos[InIndex];
                if (info.PropertyType == SyncConfig.INT_TYPE)
                {
                    int oldValue = (int)info.GetValue(this, null);
                    if (oldValue != InValue)
                    {
                        info.SetValue(this, InValue, null);
                        InvokePropertyChanged(InIndex, info.Name, oldValue, InValue);
                    }
                }
                else if (info.PropertyType == SyncConfig.SHORT_TYPE)
                {
                    Debug.LogWarning("The type of the property is \"short\", please call method \"OnSyncOne(int, short)\"");
                    OnSyncOne(InIndex, (short)InValue);
                }
                else if (info.PropertyType == SyncConfig.LONG_TYPE)
                {
                    Debug.LogWarning("The type of the property is \"long\", please call method \"OnSyncOne(int, long)\"");
                    OnSyncOne(InIndex, (long)InValue);
                }
                else if (info.PropertyType == SyncConfig.FLOAT_TYPE)
                {
                    Debug.LogWarning("The type of the property is \"float\", please call method \"OnSyncOne(int, float)\"");
                    OnSyncOne(InIndex, (float)InValue);
                }
                else if (info.PropertyType == SyncConfig.DOUBLE_TYPE)
                {
                    Debug.LogWarning("The type of the property is \"double\", please call method \"OnSyncOne(int, double)\"");
                    OnSyncOne(InIndex, (double)InValue);
                }
                else if (info.PropertyType == SyncConfig.CHAR_TYPE)
                {
                    Debug.LogWarning("The type of the property is \"char\", please call method \"OnSyncOne(int, char)\"");
                    OnSyncOne(InIndex, (char)InValue);
                }
                else throw new Exception("The type of the property is \"" + info.PropertyType + "\", and the input parameter type is \"int\".");
            }
            else throw new IndexOutOfRangeException();
        }

        public virtual void OnSyncOne(int InIndex, long InValue)
        {
            if (InIndex > -1 && InIndex < Property.InfosLength)
            {
                PropertyInfo info = Property.Infos[InIndex];
                if (info.PropertyType == SyncConfig.LONG_TYPE)
                {
                    long oldValue = (long)info.GetValue(this, null);
                    if (oldValue != InValue)
                    {
                        info.SetValue(this, InValue, null);
                        InvokePropertyChanged(InIndex, info.Name, oldValue, InValue);
                    }
                }
                else throw new Exception("The type of the property is \"" + info.PropertyType + "\", and the input parameter type is \"long\".");
            }
            else throw new IndexOutOfRangeException();
        }

        public virtual void OnSyncOne(int InIndex, float InValue)
        {
            if (InIndex > -1 && InIndex < Property.InfosLength)
            {
                PropertyInfo info = Property.Infos[InIndex];
                if (info.PropertyType == SyncConfig.FLOAT_TYPE)
                {
                    float oldValue = (float)info.GetValue(this, null);
                    if (Math.Abs(oldValue - InValue) > float.Epsilon)
                    {
                        info.SetValue(this, InValue, null);
                        InvokePropertyChanged(InIndex, info.Name, oldValue, InValue);
                    }
                }
                else if (info.PropertyType == SyncConfig.DOUBLE_TYPE)
                {
                    Debug.LogWarning("The type of the property is \"double\", please call method \"OnSyncOne(int, double)\"");
                    OnSyncOne(InIndex, (double)InValue);
                }
                else throw new Exception("The type of the property is \"" + info.PropertyType + "\", and the input parameter type is \"float\".");
            }
            else throw new IndexOutOfRangeException();
        }

        public virtual void OnSyncOne(int InIndex, double InValue)
        {
            if (InIndex > -1 && InIndex < Property.InfosLength)
            {
                PropertyInfo info = Property.Infos[InIndex];
                if (info.PropertyType == SyncConfig.DOUBLE_TYPE)
                {
                    double oldValue = (double)info.GetValue(this, null);
                    if (Math.Abs(oldValue - InValue) > double.Epsilon)
                    {
                        info.SetValue(this, InValue, null);
                        InvokePropertyChanged(InIndex, info.Name, oldValue, InValue);
                    }
                }
                else if(info.PropertyType == SyncConfig.FLOAT_TYPE)
                {
                    Debug.LogWarning("The type of the property is \"float\", please call method \"OnSyncOne(int, float)\"");
                    OnSyncOne(InIndex, (float)InValue);
                }
                else throw new Exception("The type of the property is \"" + info.PropertyType + "\", and the input parameter type is \"double\".");
            }
            else throw new IndexOutOfRangeException();
        }

        public virtual void OnSyncOne(int InIndex, string InValue)
        {
            if (InIndex > -1 && InIndex < Property.InfosLength)
            {
                PropertyInfo info = Property.Infos[InIndex];
                if (info.PropertyType == SyncConfig.STRING_TYPE)
                {
                    string oldValue = (string)info.GetValue(this, null);
                    if (oldValue != InValue)
                    {
                        info.SetValue(this, InValue, null);
                        InvokePropertyChanged(InIndex, info.Name, oldValue, InValue);
                    }
                }
                else throw new Exception("Property not match.");
            }
            else throw new IndexOutOfRangeException();
        }

        public virtual void OnSyncOne(int InIndex, object InValue)
        {
            if (InIndex > -1 && InIndex < Property.InfosLength)
            {
                PropertyInfo info = Property.Infos[InIndex];
                if (info.PropertyType == SyncConfig.BOOL_TYPE)
                {
                    string value = InValue.ToString();

                    bool b;
                    if (value == "1") b = true;
                    else if (value == "0") b = false;
                    else if (!bool.TryParse(value, out b)) b = false;
                    info.SetValue(this, b, null);
                }
                else
                {
                    info.SetValue(this, Convert.ChangeType(InValue, info.PropertyType), null);
                }
            }
            else throw new IndexOutOfRangeException();
        }

        public virtual void OnSyncAll(object[] InValues)
        {
            if (null != InValues && InValues.Length == Property.InfosLength)
            {
                for (int i = 0; i < Property.InfosLength; i++)
                {
                    PropertyInfo info = Property.Infos[i];
                    if (info.PropertyType == SyncConfig.BOOL_TYPE)
                    {
                        string value = InValues[i].ToString();
                        bool b;
                        if (value == "1") b = true;
                        else if (value == "0") b = false;
                        else if (!bool.TryParse(value, out b)) b = false;
                        info.SetValue(this, b, null);
                    }
                    else
                    {
                        info.SetValue(this, Convert.ChangeType(InValues[i], info.PropertyType), null);
                    }
                }
            }
        }

        public void RegisterPropertyChanged<T>(int InPropertyIndex, Action<string, T, T> InPropertyChangedAction)
        {
            if (null == propertyDelegates[InPropertyIndex])
                propertyDelegates[InPropertyIndex] = InPropertyChangedAction;
            else propertyDelegates[InPropertyIndex] = (Action<string, T, T>)propertyDelegates[InPropertyIndex] + InPropertyChangedAction;
        }

        public void UnRegisterPropertyChanged<T>(int InPropertyIndex, Action<string, T, T> InPropertyChangedAction)
        {
            if (null != propertyDelegates[InPropertyIndex])
                // ReSharper disable once DelegateSubtraction
                propertyDelegates[InPropertyIndex] = (Action<string, T, T>)propertyDelegates[InPropertyIndex] - InPropertyChangedAction;
        }

        private void InvokePropertyChanged<T>(int InPropertyIndex, string InPropertyName, T InOldValue, T InCurrentValue)
        {
            if (null != propertyDelegates[InPropertyIndex]) ((Action<string, T, T>)propertyDelegates[InPropertyIndex]).Invoke(InPropertyName, InOldValue, InCurrentValue);
        }

        public override string ToString()
        {
            string log =  Property.ClassName + "\n";
            for (int i = 0; i < Property.InfosLength; i++)
            {
                log += Property.Infos[i].Name + " = " + Property.Infos[i].GetValue(this, null) + "\n";
            }
            return log;
        }
    }
}
