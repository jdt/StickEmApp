using System;
using System.Data;
using System.Data.Common;
using NHibernate;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using StickEmApp.Entities;

namespace StickEmApp.Dal.UserTypes
{
    public class MoneyUserType : IUserType
    {
        public bool Equals(object x, object y)
        {
            return object.Equals(x, y);
        }

        public int GetHashCode(object x)
        {
            return x.GetHashCode();
        }

        public object DeepCopy(object value)
        {
            return value;
        }

        public object Replace(object original, object target, object owner)
        {
            return original;
        }

        public object Assemble(object cached, object owner)
        {
            return cached;
        }

        public object Disassemble(object value)
        {
            return value;
        }

        public Type ReturnedType
        {
            get { return typeof(Money); }
        }

        public SqlType[] SqlTypes
        {
            get
            {
                return new[] { NHibernateUtil.Decimal.SqlType };
            }
        }

        public bool IsMutable
        {
            get { return true; }
        }

        public void NullSafeSet(IDbCommand cmd, object value, int index)
        {
            var parameter = (DbParameter)cmd.Parameters[index];

            if (value == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = (value as Money).Value;
            }
        }

        public object NullSafeGet(IDataReader rs, string[] names, object owner)
        {
            return new Money((decimal)NHibernateUtil.Decimal.NullSafeGet(rs, names[0]));
        }
    }
}