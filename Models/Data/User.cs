using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace api.Models.Data
{
    public class User : Connection.Database
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public DateTime? Register { get; set; }


        public async Task<JObject> GetAsync(string? username)
        {
            JObject data = new JObject()
            {
                { "result", 0 },
                { "message", "Incorrenct username" }
            };
            if (string.IsNullOrEmpty(username)) return data;
            data = await PrcUser_AccountAsync(mode: 4, new JObject()
            {
                { "username", username }
            });
            return data;
        }

        public async Task<JObject> GetAsync(UserLogin? userLogin)
        {
            JObject data = new JObject()
            {
                { "result", 0 },
                { "message", "Incorrenct username or password" }
            };
            if (userLogin == null) return data;
            userLogin.Username = new Models.Data.DataEncrption(userLogin.Username).data;
            userLogin.Password = new Models.Data.DataEncrption(userLogin.Password).data;
            if (string.IsNullOrEmpty(userLogin.Username) || string.IsNullOrEmpty(userLogin.Password)) return data;

            data = await PrcUser_AccountAsync(mode: 1, new JObject()
            {
                { "username", userLogin.Username },
                { "password", userLogin.Password },
            });
            return data;
        }

        public async Task<JObject> PutAsync(UserCreate? userLogin)
        {
            JObject data = new JObject()
            {
                { "result", 0 },
                { "message", "Incorrenct username or password" }
            };
            if (userLogin == null) return data;
            userLogin.Username = new Models.Data.DataEncrption(userLogin.Username).data;
            userLogin.Password = new Models.Data.DataEncrption(userLogin.Password).data;
            if (string.IsNullOrEmpty(userLogin.Username) || string.IsNullOrEmpty(userLogin.Password)) return data;

            data = await PrcUser_AccountAsync(mode: 3, new JObject()
            {
                { "username", userLogin.Username },
                { "password", userLogin.Password },
            });
            return data;
        }
    }
}
