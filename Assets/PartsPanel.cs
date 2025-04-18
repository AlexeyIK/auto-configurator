using UnityEngine;

public class PartsPanel : MonoBehaviour
{
    public static PartsPanel Sigleton = null;

    private void Awake()
    {
        if (Sigleton == null)
            Sigleton = this;

        DontDestroyOnLoad(gameObject);
    }
}
