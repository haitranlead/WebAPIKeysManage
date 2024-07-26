using Microsoft.AspNetCore.Mvc;
using System.Text;
using WebAPIKeysManage.Models;

namespace WebApiKeysManage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LicensesController : ControllerBase
    {
        private static List<License> keys = new List<License>
        {
            new License { Key = "ADMIN-DEF46-GHI78-JKL01", IsActive = true, ExpirationDate = DateTime.UtcNow.AddMonths(1) }
        };

        [HttpGet("generate")]
        public IActionResult GenerateKey()
        {
            var newKey = GenerateApiKeys();
            keys.Add(new License { Key = newKey, IsActive = true, ExpirationDate = DateTime.UtcNow.AddMonths(1) });
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
            var ApiKeys = keys.FirstOrDefault(k => k.Key == key);
            if (ApiKeys != null && ApiKeys.IsActive && ApiKeys.ExpirationDate > DateTime.UtcNow)
            {
                return Ok(new { isValid = true });
            }
            return Ok(new { isValid = false });
        }

        [HttpPost("revoke")]
        public IActionResult RevokeKey([FromBody] string key)
        {
            var ApiKeys = keys.FirstOrDefault(k => k.Key == key);
            if (ApiKeys != null)
            {
                ApiKeys.IsActive = false;
                return Ok(new { success = true });
            }
            return Ok(new { success = false });
        }

        [HttpPost("extend")]
        public IActionResult ExtendKey([FromBody] string key)
        {
            var ApiKeys = keys.FirstOrDefault(k => k.Key == key);
            if (ApiKeys != null)
            {
                ApiKeys.ExpirationDate = DateTime.UtcNow.AddMonths(1);
                return Ok(new { success = true });
            }
            return Ok(new { success = false });
        }

        private static string GenerateApiKeys(int length = 20)
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
