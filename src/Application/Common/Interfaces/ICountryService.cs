using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface ICountryService
    {
        Task<string> GetCountry();
    }
}
