namespace WebAPIKeysManage.Models
{
    public class LicenseKey
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public bool IsActive { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
