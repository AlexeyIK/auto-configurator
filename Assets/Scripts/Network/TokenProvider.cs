using System.Runtime.InteropServices;
using UnityEngine;

public class TokenProvider : MonoBehaviour
{
    [DllImport("__Internal")] private static extern string GetCookie(string name);

    [SerializeField] private string m_AuthToken = "";

    public static TokenProvider Instance { get; set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public string GetToken()
    {
#if UNITY_EDITOR
        return m_AuthToken;
#else
        return GetCookie("auth_token");
#endif
    }
}
