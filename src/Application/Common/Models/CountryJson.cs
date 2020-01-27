namespace Application.Common.Models
{
    public class IpJson
    {
        public string Country { get; set; }
    }

    public class CountryJson
    {
        public IpJson Ip { get; set; }
    }
}
