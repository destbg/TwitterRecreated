namespace Common
{
    public interface IMemoryCacheService
    {
        T GetCacheValue<T>(string key);
        void SetCacheValue<T>(string key, T value);
    }
}
