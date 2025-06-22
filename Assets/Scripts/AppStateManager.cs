using System;
using UnityEngine;

public class AppStateManager : MonoBehaviour
{
    public enum AppState
    {
        Initial,
        Start,
        ProjectModification,
        NetworkError
    }

    [SerializeField]
    private AppState m_AppState = AppState.Initial;

    public AppState State
    {
        get { return m_AppState; }
        set
        {
            m_AppState = value;
            StateChange?.Invoke(m_AppState);
        }
    }


    public static AppStateManager Instance { get; set; }

    public event Action<AppState> StateChange;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        StateChange?.Invoke(State);
    }

    private void Start()
    {
        if (TokenProvider.Instance.GetToken() != null)
            State = m_AppState;
        else
            State = AppState.NetworkError;
    }
}
