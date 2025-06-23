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

    private event Action<AppState> stateChange;

    public AppState State
    {
        get { return m_AppState; }
        set
        {
            m_AppState = value;
            stateChange?.Invoke(m_AppState);
        }
    }

    public static AppStateManager Instance { get; set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        if (TokenProvider.Instance.GetToken() != null)
            State = m_AppState;
        else
            State = AppState.NetworkError;
    }

    public void SubscribeStateChange(Action<AppState> action, bool initialRaise = true)
    {
        if (initialRaise)
            action?.Invoke(State);

        stateChange += action;
    }

    public void UnsubscriveStateChange(Action<AppState> action) => stateChange -= action;
}
