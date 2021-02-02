using System.Data;

namespace Domain.Interfaces
{
    public interface ISQLConnectionFactory
    {
        IDbConnection Connection();
    }
}