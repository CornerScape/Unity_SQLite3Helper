using System;

namespace Szn.Framework.SQLite3Helper
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SQLite3ConstraintAttribute : Attribute
    {
        public readonly string Constraint;

        public SQLite3ConstraintAttribute(SQLite3Constraint InConstraint)
        {
            Constraint = string.Empty;
            if ((InConstraint & SQLite3Constraint.PrimaryKey) == SQLite3Constraint.PrimaryKey)
                Constraint += "PRIMARY KEY ";
            if ((InConstraint & SQLite3Constraint.AutoIncrement) == SQLite3Constraint.AutoIncrement)
                Constraint += "AUTOINCREMENT ";
            if ((InConstraint & SQLite3Constraint.Unique) == SQLite3Constraint.Unique)
                Constraint += "UNIQUE ";
            if ((InConstraint & SQLite3Constraint.NotNull) == SQLite3Constraint.NotNull)
                Constraint += "NOT NULL ";
        }

        public static string ConvertToString(SQLite3Constraint InConstraint)
        {
            string result = string.Empty;
            if ((InConstraint & SQLite3Constraint.PrimaryKey) != 0)
                result += "SQLite3Constraint.PrimaryKey | ";
            if ((InConstraint & SQLite3Constraint.Unique) != 0)
                result += "SQLite3Constraint.Unique | ";
            if ((InConstraint & SQLite3Constraint.AutoIncrement) != 0)
                result += "SQLite3Constraint.AutoIncrement | ";
            if ((InConstraint & SQLite3Constraint.NotNull) != 0)
                result += "SQLite3Constraint.NotNull | ";

            return result == string.Empty ? string.Empty : result.Remove(result.Length - 2, 2);
        }
    }
}