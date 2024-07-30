using Microsoft.AspNetCore.Mvc;
using System.Text;
using WebAPIKeysManage.Controllers;
using WebAPIKeysManage.Models;

namespace WebApiKeysManage.Controllers
{
    #region OldCode	
    //[Route("api/[controller]")]
    //[ApiController]
    //public class LicensesController : ControllerBase
    //{
    //	private readonly AppDbContext _context;

    //	public LicensesController(AppDbContext context)
    //	{
    //		_context = context;
    //	}

    //	[HttpGet("generate")]
    //	public IActionResult GenerateKey()
    //	{
    //		var newKey = GenerateApiKeys();
    //		var license = new LicenseKey { Key = newKey, IsActive = true, ExpirationDate = DateTime.UtcNow.AddMonths(1) };
    //		_context.Licenses.Add(license);
    //		_context.SaveChanges();
    //		return Ok(new { key = newKey });
    //	}

    //	[HttpGet("keys")]
    //	public IActionResult GetKeys()
    //	{
    //		var keys = _context.Licenses.ToList();
    //		return Ok(keys);
    //	}

    //	[HttpGet("validate/{key}")]
    //	public IActionResult ValidateKey(string key)
    //	{
    //		var apiKey = _context.Licenses.FirstOrDefault(k => k.Key == key);
    //		if (apiKey != null && apiKey.IsActive && apiKey.ExpirationDate > DateTime.UtcNow)
    //		{
    //			return Ok(new { isValid = true });
    //		}
    //		return Ok(new { isValid = false });
    //	}

    //	[HttpPost("revoke/{key}")]
    //	public IActionResult RevokeKey(string key)
    //	{
    //		var apiKey = _context.Licenses.FirstOrDefault(k => k.Key == key);
    //		if (apiKey != null)
    //		{
    //			apiKey.IsActive = false;
    //			_context.SaveChanges();
    //			return Ok(new { success = true });
    //		}
    //		return Ok(new { success = false });
    //	}

    //	[HttpPost("extend/{key}")]
    //	public IActionResult ExtendKey(string key)
    //	{
    //		var apiKey = _context.Licenses.FirstOrDefault(k => k.Key == key);
    //		if (apiKey != null)
    //		{
    //			// Cộng thêm 1 tháng vào ExpirationDate hiện tại
    //			apiKey.ExpirationDate = apiKey.ExpirationDate.AddMonths(1);
    //			_context.SaveChanges();
    //			return Ok(new { success = true });
    //		}
    //		return Ok(new { success = false });
    //	}

    //	[HttpDelete("delete/{key}")]
    //	public IActionResult DeleteKey(string key)
    //	{
    //		// Tìm license theo key
    //		var license = _context.Licenses.FirstOrDefault(l => l.Key == key);

    //		// Nếu không tìm thấy key
    //		if (license == null)
    //		{
    //			return NotFound(new { message = "Key not found." });
    //		}

    //		// Xóa license
    //		_context.Licenses.Remove(license);
    //		_context.SaveChanges();

    //		return Ok(new { message = "Key deleted successfully." });
    //	}

    //	private static string GenerateApiKeys(int length = 20)
    //	{
    //		const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    //		var random = new Random();
    //		var key = new StringBuilder();

    //		for (int i = 0; i < length; i++)
    //		{
    //			if (i > 0 && i % 5 == 0)
    //			{
    //				key.Append('-');
    //			}
    //			key.Append(chars[random.Next(chars.Length)]);
    //		}

    //		return key.ToString();
    //	}

    //	[HttpPut("update/{key}")]
    //	public IActionResult UpdateKey(string key, [FromBody] LicenseKey updatedLicense)
    //	{
    //		if (updatedLicense == null)
    //		{
    //			return BadRequest("Dữ liệu không hợp lệ.");
    //		}

    //		var existingLicense = _context.Licenses.FirstOrDefault(l => l.Key == key);
    //		if (existingLicense == null)
    //		{
    //			return NotFound("Key không tồn tại.");
    //		}

    //		// Cập nhật thuộc tính của license
    //		existingLicense.IsActive = updatedLicense.IsActive;
    //		existingLicense.ExpirationDate = updatedLicense.ExpirationDate;
    //		existingLicense.Key = updatedLicense.Key;

    //		_context.SaveChanges();

    //		return Ok(new { message = "Key updated successfully." });
    //	}
    //}
    #endregion

    /// <summary>
    /// Controller quản lý License API Keys
    /// </summary>
    [Route("api/[controller]")]
    [ApiController] // Chỉ định đây là một API controller
    public class LicensesController : ControllerBase // Kế thừa từ ControllerBase
    {
        private readonly AppDbContext _context; // Biến lưu trữ ngữ cảnh cơ sở dữ liệu

        /// <summary>
        /// Khởi tạo LicensesController với ngữ cảnh cơ sở dữ liệu
        /// </summary>
        /// <param name="context">Ngữ cảnh cơ sở dữ liệu</param>
        public LicensesController(AppDbContext context)
        {
            _context = context; // Gán ngữ cảnh vào biến
        }

        /// <summary>
        /// Endpoint để tạo key mới
        /// </summary>
        /// <returns>Key mới được tạo</returns>
        [HttpGet("generate")]
        public IActionResult GenerateKey()
        {
            var newKey = GenerateApiKeys(); // Tạo key mới
            var license = new LicenseKey
            {
                Key = newKey,
                IsActive = true,
                ExpirationDate = DateTime.UtcNow.AddMonths(1) // Đặt thời gian hết hạn
            };
            _context.Licenses.Add(license); // Thêm license vào ngữ cảnh
            _context.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
            return Ok(new { key = newKey }); // Trả về key mới
        }

        /// <summary>
        /// Endpoint để lấy danh sách các keys
        /// </summary>
        /// <returns>Danh sách keys</returns>
        [HttpGet("keys")]
        public IActionResult GetKeys()
        {
            var keys = _context.Licenses.ToList(); // Lấy danh sách licenses từ cơ sở dữ liệu
            return Ok(keys); // Trả về danh sách keys
        }

        /// <summary>
        /// Endpoint để xác thực key
        /// </summary>
        /// <param name="key">Key cần xác thực</param>
        /// <returns>Kết quả xác thực</returns>
        [HttpGet("validate/{key}")]
        public IActionResult ValidateKey(string key)
        {
            var apiKey = _context.Licenses.FirstOrDefault(k => k.Key == key); // Tìm key trong cơ sở dữ liệu
            if (apiKey != null && apiKey.IsActive && apiKey.ExpirationDate > DateTime.UtcNow)
            {
                return Ok(new { isValid = true }); // Nếu key hợp lệ
            }
            return Ok(new { isValid = false }); // Nếu key không hợp lệ
        }

        /// <summary>
        /// Endpoint để thu hồi key
        /// </summary>
        /// <param name="key">Key cần thu hồi</param>
        /// <returns>Kết quả thu hồi</returns>
        [HttpPost("revoke/{key}")]
        public IActionResult RevokeKey(string key)
        {
            var apiKey = _context.Licenses.FirstOrDefault(k => k.Key == key); // Tìm key
            if (apiKey != null)
            {
                apiKey.IsActive = false; // Đánh dấu key là không hoạt động
                _context.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
                return Ok(new { success = true }); // Trả về thông báo thành công
            }
            return Ok(new { success = false }); // Nếu không tìm thấy key
        }

        /// <summary>
        /// Endpoint để gia hạn key
        /// </summary>
        /// <param name="key">Key cần gia hạn</param>
        /// <returns>Kết quả gia hạn</returns>
        [HttpPost("extend/{key}")]
        public IActionResult ExtendKey(string key)
        {
            var apiKey = _context.Licenses.FirstOrDefault(k => k.Key == key); // Tìm key
            if (apiKey != null)
            {
                // Cộng thêm 1 tháng vào ExpirationDate hiện tại
                apiKey.ExpirationDate = apiKey.ExpirationDate.AddMonths(1);
                _context.SaveChanges(); // Lưu thay đổi
                return Ok(new { success = true }); // Trả về thông báo thành công
            }
            return Ok(new { success = false }); // Nếu không tìm thấy key
        }

        /// <summary>
        /// Endpoint để xóa key
        /// </summary>
        /// <param name="key">Key cần xóa</param>
        /// <returns>Kết quả xóa</returns>
        [HttpDelete("delete/{key}")]
        public IActionResult DeleteKey(string key)
        {
            // Tìm license theo key
            var license = _context.Licenses.FirstOrDefault(l => l.Key == key);

            // Nếu không tìm thấy key
            if (license == null)
            {
                return NotFound(new { message = "Key not found." }); // Trả về thông báo không tìm thấy
            }

            // Xóa license
            _context.Licenses.Remove(license);
            _context.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu

            return Ok(new { message = "Key deleted successfully." }); // Trả về thông báo thành công
        }

        /// <summary>
        /// Tạo key API ngẫu nhiên
        /// </summary>
        /// <param name="length">Độ dài của key</param>
        /// <returns>Key đã tạo</returns>
        private static string GenerateApiKeys(int length = 20)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"; // Ký tự cho key
            var random = new Random(); // Tạo đối tượng Random
            var key = new StringBuilder(); // Chuỗi để lưu key

            // Tạo key với độ dài xác định
            for (int i = 0; i < length; i++)
            {
                if (i > 0 && i % 5 == 0) // Thêm dấu '-' mỗi 5 ký tự
                {
                    key.Append('-');
                }
                key.Append(chars[random.Next(chars.Length)]); // Chọn ngẫu nhiên ký tự từ chars
            }

            return key.ToString(); // Trả về key đã tạo
        }

        /// <summary>
        /// Endpoint để cập nhật key
        /// </summary>
        /// <param name="key">Key cần cập nhật</param>
        /// <param name="updatedLicense">Dữ liệu license cập nhật</param>
        /// <returns>Kết quả cập nhật</returns>
        [HttpPut("update/{key}")]
        public IActionResult UpdateKey(string key, [FromBody] LicenseKey updatedLicense)
        {
            if (updatedLicense == null) // Kiểm tra dữ liệu đầu vào
            {
                return BadRequest("Dữ liệu không hợp lệ."); // Trả về thông báo lỗi
            }

            var existingLicense = _context.Licenses.FirstOrDefault(l => l.Key == key); // Tìm license
            if (existingLicense == null)
            {
                return NotFound("Key không tồn tại."); // Nếu không tìm thấy key
            }

            // Cập nhật thuộc tính của license
            existingLicense.IsActive = updatedLicense.IsActive;
            existingLicense.ExpirationDate = updatedLicense.ExpirationDate;
            existingLicense.Key = updatedLicense.Key;

            _context.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu

            return Ok(new { message = "Key updated successfully." }); // Trả về thông báo thành công
        }
    }
}
