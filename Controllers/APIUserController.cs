using api.Models.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Npgsql;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class APIUserController : ControllerBase
    {

        [Authorize]
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Get()
        {
            string? username = new Models.Data.DataDecryption(User.FindFirst(ClaimTypes.NameIdentifier)?.Value).data;
            if (string.IsNullOrEmpty(username)) return Unauthorized();
            return Ok(new { username });
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] UserLogin? userLogin)
        {
            if (userLogin == null) return Ok(new { result = 0, message = "No username or password" });
            if (string.IsNullOrEmpty(userLogin.Username) || string.IsNullOrEmpty(userLogin.Password)) return Ok(new { result = 0, message = "No username or password" });
            int result = 0;
            JObject data = await new Models.Data.User().GetAsync(userLogin);
            result = int.TryParse((data["result"] ?? "").ToObject<string>() ?? "", out result) ? result : 0;
            string? token = new Models.Data.JWTGenerator(userLogin.Username).data;
            return Ok(new { result = result, message = result == 0 ? (data["message"] ?? "").ToObject<string>() : token });
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] UserCreate? userCreate)
        {
            if (userCreate == null) return Ok(new { result = 0, message = "No user create data" });
            userCreate.Username = (userCreate.Username ?? "").Trim();
            userCreate.Password = (userCreate.Password ?? "").Trim();
            userCreate.RePassword = (userCreate.RePassword ?? "").Trim();
            if (string.IsNullOrEmpty(userCreate.Username) || string.IsNullOrEmpty(userCreate.Password) || string.IsNullOrEmpty(userCreate.RePassword)) return Ok(new { result = 0, message = "No user create data" });
            if (!userCreate.Password.Equals(userCreate.RePassword)) return Ok(new { result = 0, message = "Password is not the same" });
            string passwordReg = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
            if (!Regex.IsMatch(userCreate.Password, passwordReg)) return Ok(new { result = 0, message = "Password is not correct format" });

            int result = 0;
            JObject data = await new Models.Data.User().PutAsync(userCreate);
            result = int.TryParse((data["result"] ?? "").ToObject<string>() ?? "", out result) ? result : 0;
            if (result == 1)
            {
                string? token = new Models.Data.JWTGenerator(userCreate.Username).data;
                return Ok(new { result = result, message = token });
            }
            return Ok(new { result = result, message = (data["message"] ?? "").ToObject<string>() ?? "" });
        }
    }
}
