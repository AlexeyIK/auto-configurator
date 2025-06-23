using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public static class NetworkManager
{
    private const string BaseUrl = "https://localhost:5234/api/";

    public static async Task<T> GetAsync<T>(string endpoint, string token = null)
    {
        using var req = UnityWebRequest.Get(BaseUrl + endpoint);

        if (!string.IsNullOrEmpty(token))
            req.SetRequestHeader("Auth", token);

        await SendAsync(req);

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"GET {endpoint} → {req.responseCode} | {req.error}");
            return default;
        }

        return JsonConvert.DeserializeObject<T>(req.downloadHandler.text);
    }

    public static async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest payload, string token = null)
    {
        string json = JsonConvert.SerializeObject(payload);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        using var req = new UnityWebRequest(BaseUrl + endpoint, UnityWebRequest.kHttpVerbPOST)
        {
            uploadHandler = new UploadHandlerRaw(bodyRaw),
            downloadHandler = new DownloadHandlerBuffer()
        };

        req.SetRequestHeader("Content-Type", "application/json");
        if (!string.IsNullOrEmpty(token))
            req.SetRequestHeader("Auth", token);

        await SendAsync(req);

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"POST {endpoint} → {req.responseCode} | {req.error} | {req.downloadHandler.text}");
            return default;
        }

        return JsonConvert.DeserializeObject<TResponse>(req.downloadHandler.text);
    }

    /// <summary>
    /// Универсальная обёртка, превращающая UnityWebRequest в Task.
    /// </summary>
    private static async Task SendAsync(UnityWebRequest req)
    {
#if UNITY_2023_1_OR_NEWER     // начиная с Unity 2023 SendWebRequest ― awaitable
        await req.SendWebRequest();
#else                         // Unity 2022/2021: ждём вручную в цикле
        var op = req.SendWebRequest();
        while (!op.isDone)
            await Task.Yield();
#endif
    }
}