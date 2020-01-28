using System.Collections.Generic;

namespace Application.Common.Models
{
    public class GifDownloadCategoryJson
    {
        public IEnumerable<GifDownloadSearchtermJson> Tags { get; set; }
    }

    public class GifDownloadSearchtermJson
    {
        public string Image { get; set; }
        public string Searchterm { get; set; }
    }

    public class GifDownloadSearchJson
    {
        public IEnumerable<GifDownloadSearchMediaJson> Results { get; set; }
    }

    public class GifDownloadSearchMediaJson
    {
        public IReadOnlyList<GifDownloadSearchInnerMediaJson> Media { get; set; }
    }

    public class GifDownloadSearchInnerMediaJson
    {
        public GifDownloadSearchUrlJson Gif { get; set; }
        public GifDownloadSearchUrlJson TinyGif { get; set; }
    }

    public class GifDownloadSearchUrlJson
    {
        public string Url { get; set; }
    }
}
