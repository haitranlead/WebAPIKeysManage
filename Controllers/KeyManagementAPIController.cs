using Microsoft.AspNetCore.Mvc;
using System.Text;
using WebAPIKeysManage.Models;

namespace WebAPIKeysManage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KeyManagementAPIController : ControllerBase
    {
        private static List<ApiKey> keys = new List<ApiKey>
        {
            new ApiKey { Key = "ADMIN-DEF46-GHI78-JKL01", IsActive = true, ExpirationDate = DateTime.UtcNow.AddMonths(1) }
        };

        [HttpGet("generate")]
        public IActionResult GenerateKey()
        {
            var newKey = GenerateApiKey();
            keys.Add(new ApiKey { Key = newKey, IsActive = true, ExpirationDate = DateTime.UtcNow.AddMonths(1) });
            return Ok(new { key = newKey });
        }

        [HttpGet("keys")]
        public IActionResult GetKeys()
        {
            return Ok(keys);
        }

        [HttpGet("validate/{key}")]
        public IActionResult ValidateKey(string key)
        {
            var apiKey = keys.FirstOrDefault(k => k.Key == key);
            if (apiKey != null && apiKey.IsActive && apiKey.ExpirationDate > DateTime.UtcNow)
            {
                return Ok(new { isValid = true });
            }
            return Ok(new { isValid = false });
        }

        [HttpPost("revoke")]
        public IActionResult RevokeKey([FromBody] string key)
        {
            var apiKey = keys.FirstOrDefault(k => k.Key == key);
            if (apiKey != null)
            {
                apiKey.IsActive = false;
                return Ok(new { success = true });
            }
            return Ok(new { success = false });
        }

        [HttpPost("extend")]
        public IActionResult ExtendKey([FromBody] string key)
        {
            var apiKey = keys.FirstOrDefault(k => k.Key == key);
            if (apiKey != null)
            {
                apiKey.ExpirationDate = DateTime.UtcNow.AddMonths(1);
                return Ok(new { success = true });
            }
            return Ok(new { success = false });
        }

        private static string GenerateApiKey(int length = 20)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var key = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                if (i > 0 && i % 5 == 0)
                {
                    key.Append('-'); // Thêm dấu gạch nối sau mỗi 5 ký tự
                }
                key.Append(chars[random.Next(chars.Length)]);
            }

            return key.ToString();
        }
    }
}
