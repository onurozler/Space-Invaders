using System.Threading.Tasks;
using UnityEngine.Networking;

namespace Network.WebNetworkService.LocalAPI
{
    public interface ILocalRestAPI
    {
        Task<string> FetchData(UnityWebRequest unityWebRequest);
    }
}