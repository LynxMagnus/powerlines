using System.Web;

namespace PowerLines.Inbound
{
    public interface IResultService
    {
        int Upload(bool currentSeasonOnly = false);
    }
}