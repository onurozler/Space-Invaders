using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Network.WebNetworkService.Responses;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

namespace Network.WebNetworkService.LocalAPI
{
    public class LocalRestAPI : ILocalRestAPI
    {
        private TextAsset _textDatabase;
        
        public LocalRestAPI()
        {
            LoadLeaderboardFromDB();
        }

        public Task<string> FetchData(UnityWebRequest unityWebRequest)
        {
            var method = HttpMethodHelper.ToType(unityWebRequest.method);
            var taskCompletionSource = new TaskCompletionSource<string>();
                                 
            if (method == HttpMethod.GET)
            {
                // Delay Simulation from Local Web Response
                var delay = Random.Range(WebConstants.LocalRestAPIResponseDelayMin, WebConstants.LocalRestAPIResponseDelayMax);
                var delayedResponse = Task.Delay(TimeSpan.FromSeconds(delay));
                
                if (unityWebRequest.url.Contains(WebConstants.RegisterEndPoint))
                {
                    delayedResponse.ContinueWith(task => taskCompletionSource.SetResult(JsonUtility.ToJson(new WebRegisterResponse())));
                }
                else if (unityWebRequest.url.Contains(WebConstants.LeaderboardGetEndPoint))
                {
                    if (ValidateRequest(unityWebRequest))
                    {
                        if (_textDatabase != null)
                        {
                            taskCompletionSource.SetResult(_textDatabase.text);
                        }
                        else
                        {
                            LoadLeaderboardFromDB(()=>
                            {
                                taskCompletionSource.SetResult(_textDatabase.text);
                            });
                        }
                    }
                }
            }
            else
            {
                if (unityWebRequest.url.Contains(WebConstants.LeaderboardPostEndPoint) && ValidateRequest(unityWebRequest))
                {
                    var parameters = Encoding.UTF8.GetString(unityWebRequest.uploadHandler.data);
                    var result = System.Web.HttpUtility.ParseQueryString(parameters);

                    var leaderboard = JsonUtility.FromJson<WebLeaderboardResponse>(_textDatabase.text);
                    var players = leaderboard.@group.players.OrderBy(x=>x.score).ToList();

                    var playerName = "";
                    var playerScore =  0;
                    
                    foreach (var key in result.AllKeys)
                    {
                        if (key.Contains("name"))
                        {
                            playerName = result[key];
                        }
                        else if (key.Contains("score"))
                        {
                            playerScore = int.Parse(result[key]);
                        }
                    }

                    for (int i = 0; i < players.Count; i++)
                    {
                        if (playerScore < players[i].score)
                        {
                            players.Insert(i,new WebLeaderboardPlayers
                            {
                                name = playerName,
                                score = playerScore
                            });
                            break;
                        }
                    }
                    
                    leaderboard.@group.players = players.ToArray();
                    File.WriteAllText(AssetDatabase.GetAssetPath(_textDatabase), JsonUtility.ToJson(leaderboard));
                    EditorUtility.SetDirty(_textDatabase);
                    AssetDatabase.Refresh();
                    taskCompletionSource.SetResult(JsonUtility.ToJson(new WebLeaderboardSubmitResponse{code = 1}));
                }
            }
            
            return taskCompletionSource.Task;
        }

        private void LoadLeaderboardFromDB(Action onCompleted = null)
        {
            var loadRequest = Resources.LoadAsync<TextAsset>(WebConstants.LocalLeaderboardPath);
            loadRequest.completed += operation =>
            {
                _textDatabase = (TextAsset) loadRequest.asset;
            };
        }

        private bool ValidateRequest(UnityWebRequest unityWebRequest)
        {
            // This validation.. is perfect
            return !string.IsNullOrEmpty(unityWebRequest.GetRequestHeader("token"));
        }
    }
}