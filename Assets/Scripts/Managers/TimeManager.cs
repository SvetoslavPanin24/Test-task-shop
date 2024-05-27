using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Networking;

namespace MyGame.Managers
{
    public class TimeManager : MonoBehaviour
    {
        private const string TimeUrl = "https://worldtimeapi.org/api/timezone/Etc/UTC";
        private DateTime serverTime;

        public static TimeManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private IEnumerator GetServerTime(Action<DateTime> onTimeReceived)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(TimeUrl))
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("Error fetching server time: " + request.error);
                    onTimeReceived(DateTime.MinValue);
                }
                else
                {
                    string json = request.downloadHandler.text;
                    ServerTimeResponse response = JsonUtility.FromJson<ServerTimeResponse>(json);
                    serverTime = DateTime.Parse(response.datetime);
                    onTimeReceived(serverTime);
                }
            }
        }

        public void GetTime(Action<DateTime> onTimeReceived)
        {
            StartCoroutine(GetServerTime(onTimeReceived));
        }
    }

    [Serializable]
    public class ServerTimeResponse
    {
        public string datetime;
    }
}
