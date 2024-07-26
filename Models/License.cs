namespace WebAPIKeysManage.Models
{
    public class License
    {
        public string Key { get; set; }
        public bool IsActive { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
