using System.IO;
using System.Threading.Tasks;

namespace PDF_Reader.Pages
{
    public interface IBrandTrackProcessor
    {
        Task ExtractData(Stream file , string fileName);
        //Task<bool?> IsValid(string productid);
        Task CreatStockIn(string fileName);
    }
}
