using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

public static class NetworkManager
{
    private static readonly HttpClient m_HttpClient = new HttpClient();
    private const string m_BaseUrl = "https://localhost:5234/api/";

    public static async Task<T> GetAsync<T>(string endpoint, string token = null)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, m_BaseUrl + endpoint);
        if (!string.IsNullOrEmpty(token))
            request.Headers.Add("Auth", token);

        var response = await m_HttpClient.SendAsync(request);
        string json = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            Debug.LogError($"GET {endpoint} failed: {response.StatusCode} - {json}");
            return default;
        }

        return JsonConvert.DeserializeObject<T>(json);
    }

    public static async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest payload, string token = null)
    {
        var json = JsonConvert.SerializeObject(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var request = new HttpRequestMessage(HttpMethod.Post, m_BaseUrl + endpoint)
        {
            Content = content
        };

        if (!string.IsNullOrEmpty(token))
            request.Headers.Add("Auth", token);

        var response = await m_HttpClient.SendAsync(request);
        string responseJson = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            Debug.LogError($"POST {endpoint} failed: {response.StatusCode} - {responseJson}");
            return default;
        }

        return JsonConvert.DeserializeObject<TResponse>(responseJson);
    }
}