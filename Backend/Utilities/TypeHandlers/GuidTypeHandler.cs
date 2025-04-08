using System.Data;
using Dapper;

namespace Backend.Utilities.TypeHandlers;

public class GuidTypeHandler : SqlMapper.TypeHandler<Guid>
{
    public override void SetValue(IDbDataParameter parameter, Guid value)
    {
        parameter.Value = value.ToString();
        parameter.DbType = DbType.String;
    }

    public override Guid Parse(object value)
    {
        if (value is string s && Guid.TryParse(s, out var guid))
        {
            return guid;
        }
        throw new DataException($"Cannot convert {value} to {typeof(Guid).FullName}");
    }
}