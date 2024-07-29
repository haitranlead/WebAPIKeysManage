using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Text;
using WebAPIKeysManage.Controllers;
using WebAPIKeysManage.Models;

namespace WebApiKeysManage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LicensesController : ControllerBase
    {
        #region Code old none DB               
        //private static List<License> keys = new List<License>
        //{
        //    //new License { Key = "ADMIN-DEF46-GHI78-JKL01", IsActive = true, ExpirationDate = DateTime.UtcNow.AddMonths(1) }
        //    new License { Key = "ADMIN-DEF46-GHI78-JKL01", IsActive = true, ExpirationDate = DateTime.UtcNow.AddDays(1) }
        //};

        //[HttpGet("generate")]
        //public IActionResult GenerateKey()
        //{
        //    var newKey = GenerateApiKeys();
        //    keys.Add(new License { Key = newKey, IsActive = true, ExpirationDate = DateTime.UtcNow.AddMonths(1) });
        //    return Ok(new { key = newKey });
        //}

        //[HttpGet("keys")]
        //public IActionResult GetKeys()
        //{
        //    return Ok(keys);
        //}

        //[HttpGet("validate/{key}")]
        //public IActionResult ValidateKey(string key)
        //{
        //    var ApiKeys = keys.FirstOrDefault(k => k.Key == key);
        //    if (ApiKeys != null && ApiKeys.IsActive && ApiKeys.ExpirationDate > DateTime.UtcNow)
        //    {
        //        return Ok(new { isValid = true });
        //    }
        //    return Ok(new { isValid = false });
        //}

        //[HttpPost("revoke")]
        //public IActionResult RevokeKey([FromBody] string key)
        //{
        //    var ApiKeys = keys.FirstOrDefault(k => k.Key == key);
        //    if (ApiKeys != null)
        //    {
        //        ApiKeys.IsActive = false;
        //        return Ok(new { success = true });
        //    }
        //    return Ok(new { success = false });
        //}

        //[HttpPost("extend")]
        //public IActionResult ExtendKey([FromBody] string key)
        //{
        //    var ApiKeys = keys.FirstOrDefault(k => k.Key == key);
        //    if (ApiKeys != null)
        //    {
        //        ApiKeys.ExpirationDate = DateTime.UtcNow.AddMonths(1);
        //        return Ok(new { success = true });
        //    }
        //    return Ok(new { success = false });
        //}

        //private static string GenerateApiKeys(int length = 20)
        //{
        //    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        //    var random = new Random();
        //    var key = new StringBuilder();

        //    for (int i = 0; i < length; i++)
        //    {
        //        if (i > 0 && i % 5 == 0)
        //        {
        //            key.Append('-'); // Thêm dấu gạch nối sau mỗi 5 ký tự
        //        }
        //        key.Append(chars[random.Next(chars.Length)]);
        //    }

        //    return key.ToString();
        //}
        #endregion

        private readonly AppDbContext _context;

        public LicensesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("generate")]
        public IActionResult GenerateKey()
        {
            var newKey = GenerateApiKeys();
            var license = new LicenseKey { Key = newKey, IsActive = true, ExpirationDate = DateTime.UtcNow.AddMonths(1) };
            _context.Licenses.Add(license);
            _context.SaveChanges();
            return Ok(new { key = newKey });
        }

        [HttpGet("keys")]
        public IActionResult GetKeys()
        {
            var keys = _context.Licenses.ToList();
            return Ok(keys);
        }

        [HttpGet("validate/{key}")]
        public IActionResult ValidateKey(string key)
        {
            var apiKey = _context.Licenses.FirstOrDefault(k => k.Key == key);
            if (apiKey != null && apiKey.IsActive && apiKey.ExpirationDate > DateTime.UtcNow)
            {
                return Ok(new { isValid = true });
            }
            return Ok(new { isValid = false });
        }

        [HttpPost("revoke")]
        public IActionResult RevokeKey([FromBody] string key)
        {
            var apiKey = _context.Licenses.FirstOrDefault(k => k.Key == key);
            if (apiKey != null)
            {
                apiKey.IsActive = false;
                _context.SaveChanges();
                return Ok(new { success = true });
            }
            return Ok(new { success = false });
        }

        [HttpPost("extend")]
        public IActionResult ExtendKey([FromBody] string key)
        {
            var apiKey = _context.Licenses.FirstOrDefault(k => k.Key == key);
            if (apiKey != null)
            {
                apiKey.ExpirationDate = DateTime.UtcNow.AddMonths(1);
                _context.SaveChanges();
                return Ok(new { success = true });
            }
            return Ok(new { success = false });
        }

        //[HttpPost("delete")]
        //public IActionResult DeleteKey([FromBody] string key)
        //{
        //    var apiKey = _context.Licenses.FirstOrDefault(k => k.Key == key);
        //    if (apiKey != null)
        //    {
        //        _context.Licenses.Remove(apiKey);                
        //        _context.SaveChanges();
        //        return Ok(new { success = true });
        //    }
        //    return Ok(new { success = false });
        //}

        [HttpDelete("delete/{key}")]
        public IActionResult DeleteKey(string key)
        {
            // Tìm license theo key
            var license = _context.Licenses.FirstOrDefault(l => l.Key == key);

            // Nếu không tìm thấy key
            if (license == null)
            {
                return NotFound(new { message = "Key not found." });
            }

            // Xóa license
            _context.Licenses.Remove(license);
            _context.SaveChanges();

            return Ok(new { message = "Key deleted successfully." });
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
                    key.Append('-');
                }
                key.Append(chars[random.Next(chars.Length)]);
            }

            return key.ToString();
        }
    }
}
