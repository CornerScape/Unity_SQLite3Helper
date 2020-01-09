using System;
using System.Text;

namespace Szn.Framework.SQLite3Helper
{
    public static class SQLite3Utility
    {
        public static byte[] ConvertToUTF8Bytes(this string InStr)
        {
            return Encoding.UTF8.GetBytes(InStr);
        }

        public static object ConvertToPropertyValue(object InContent, Type InType)
        {
            if (InType == SQLite3Config.BOOL_TYPE)
            {
                int value;
                if (int.TryParse(InContent.ToString(), out value)) return value == 1;
                return false;
            }

            if (InType == SQLite3Config.CHAR_TYPE) return Convert.ToChar(InContent.ToString());


            return InContent;
        }

        public static string ConvertToSqlValue(object InContent)
        {
            if (null == InContent) return "";
            return ConvertToSqlValue(InContent, InContent.GetType());
        }

        public static string ConvertToSqlValue(object InContent, Type InType)
        {
            string content = InContent.ToString();

            if (InType == SQLite3Config.BOOL_TYPE)
            {
                return content == "TRUE" || content == "1" ? "1" : "0";
            }
            if (InType == SQLite3Config.STRING_TYPE)
            {
                if (content.Contains("'"))
                {
                    content = content.Replace("'", "''");
                }

                return string.Format("'{0}'", content);
            }

            return content;
        }

        public static string CheckSqlValue(string InContent)
        {
            if (InContent.Contains("'")) InContent = InContent.Replace("'", "''");

            return string.Format("'{0}'", InContent);
        }

        public static string ConvertSQLite3ConstraintToStr(SQLite3Constraint InConstraint)
        {
            string result = string.Empty;
            if ((InConstraint & SQLite3Constraint.PrimaryKey) != 0)
                result += " PrimaryKey ";
            if ((InConstraint & SQLite3Constraint.Unique) != 0)
                result += " Unique ";
            if ((InConstraint & SQLite3Constraint.AutoIncrement) != 0)
                result += " AutoIncrement ";
            if ((InConstraint & SQLite3Constraint.NotNull) != 0)
                result += " NotNull ";

            return result == string.Empty ? string.Empty : result.Remove(result.Length - 1, 1);
        }
    }
}