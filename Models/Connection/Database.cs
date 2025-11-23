using Newtonsoft.Json.Linq;
using Npgsql;
using System.Data;

namespace api.Models.Connection
{
    public abstract class Database
    {
        protected async Task<JObject> PrcUser_AccountAsync(int mode, JObject? in_data = null)
        {
            JObject data = new JObject()
            {
                { "result", 0 },
                { "message", null }
            };
            try
            {
                if (Models.Config.DatabaseConnection == null)
                {
                    Models.Config.DatabaseConnection = new NpgsqlConnection(Models.Config.DatabaseConnectionString);
                    await Models.Config.DatabaseConnection.OpenAsync();
                }

                await using NpgsqlCommand comm = Models.Config.DatabaseConnection.CreateCommand();
                comm.CommandText = "CALL \"user\".prc_user(@mode, @in_data, null);";
                comm.Parameters.Add(new NpgsqlParameter("@mode", NpgsqlTypes.NpgsqlDbType.Integer) { Value = mode });
                if (in_data == null) comm.Parameters.Add(new NpgsqlParameter("@in_data", NpgsqlTypes.NpgsqlDbType.Jsonb) { Value = DBNull.Value });
                else comm.Parameters.Add(new NpgsqlParameter("@in_data", NpgsqlTypes.NpgsqlDbType.Jsonb) { Value = in_data.ToString() });
                Console.WriteLine(comm.CommandText);
                NpgsqlDataReader reader = await comm.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    data = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(reader.GetString(0)) ?? new JObject();
                }
                await reader.CloseAsync();
                await comm.DisposeAsync();
            }
            catch (Exception error)
            {
                data["message"] = error.ToString();
            }
            return data;
        }
    }
}
