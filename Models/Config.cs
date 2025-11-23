using Npgsql;

namespace api.Models
{
    public static class Config
    {
        public static readonly string JWTSecretKey = "gC9pNO4KBa8bBo5K97wEFUqkP3761FDUDX5M7g3vlz4XhweJAq";
        public static readonly string JWTIssuer = "my-authentication-server-issues.com";
        public static readonly string JWTAudience = "my-authentication-server-audience.com";
        public static readonly string DatabaseConnectionString = "Host=localhost;Username=admin;Password=TEST001;Database=application_test; Maximum Pool Size=200;";
        public static readonly string EncrptionKey = "RcH5J7cUTm2uIWTxkkNUtPoP3SEPrdXd";
        public static NpgsqlConnection? DatabaseConnection;
    }
}
