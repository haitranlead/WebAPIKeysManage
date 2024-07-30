using Microsoft.AspNetCore.Mvc;
using System.Text;
using WebAPIKeysManage.Controllers;
using WebAPIKeysManage.Models;

namespace WebApiKeysManage.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LicensesController : ControllerBase
	{
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

		[HttpPost("revoke/{key}")]
		public IActionResult RevokeKey(string key)
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

		[HttpPost("extend/{key}")]
		public IActionResult ExtendKey(string key)
		{
			var apiKey = _context.Licenses.FirstOrDefault(k => k.Key == key);
			if (apiKey != null)
			{
				// Cộng thêm 1 tháng vào ExpirationDate hiện tại
				apiKey.ExpirationDate = apiKey.ExpirationDate.AddMonths(1);
				_context.SaveChanges();
				return Ok(new { success = true });
			}
			return Ok(new { success = false });
		}

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

		[HttpPut("update/{key}")]
		public IActionResult UpdateKey(string key, [FromBody] LicenseKey updatedLicense)
		{
			if (updatedLicense == null)
			{
				return BadRequest("Dữ liệu không hợp lệ.");
			}

			var existingLicense = _context.Licenses.FirstOrDefault(l => l.Key == key);
			if (existingLicense == null)
			{
				return NotFound("Key không tồn tại.");
			}

			// Cập nhật thuộc tính của license
			existingLicense.IsActive = updatedLicense.IsActive;
			existingLicense.ExpirationDate = updatedLicense.ExpirationDate;
			existingLicense.Key = updatedLicense.Key;

			_context.SaveChanges();

			return Ok(new { message = "Key updated successfully." });
		}
	}
}
