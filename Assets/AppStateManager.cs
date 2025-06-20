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
    private AppState _appState = AppState.Initial;

    public AppState State
    {
        get { return _appState; }
        set
        {
            _appState = value;
            StateChange?.Invoke(_appState);
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
            State = AppState.Start;
        else
            State = AppState.NetworkError;
    }
}
