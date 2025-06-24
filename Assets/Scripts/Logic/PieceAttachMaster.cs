using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Model;
using Data.ViewModel;
using UnityEngine;
using static UnityEditor.Progress;

public class PieceAttachMaster : MonoBehaviour
{
    private Dictionary<CategoryType, object> m_ItemsCache = new();

    [SerializeField] private PieceAttachSocket[] m_WheelsSockets = default;
    [SerializeField] private GameObject[] m_OriginalWheels = default;

    [SerializeField] private PieceAttachSocket m_SpoilerPoint = default;

    private void OnValidate()
    {
        if (m_OriginalWheels.Length == 0)
            Debug.LogError("You must assing original wheels points");
    }

    public async void LoadAndAttach(PieceItemData item, CategoryItemData category, bool isModified)
    {
        switch (category.Type)
        {
            case CategoryType.Wheels:
                var wheelData = await NetworkManager.GetAsync<Piece>($"pieces/{item.Id}?categoryId={category.Id}" +
                                                                     $"&automobileId={ProjectManager.Instance.CurrentCar.CarId}",
                                                                     TokenProvider.Instance.GetToken());
                var wheel = await LoadWheel(wheelData.ModelUrl);
                HideParts(m_OriginalWheels);
                ChangeParts(wheel, m_WheelsSockets.Select(w => w.transform).ToArray(), isModified);
                break;
        }
    }

    public async void LoadAndAttach(Piece piece, Category category, bool isModified)
    {
        switch (category.Type)
        {
            case CategoryType.Wheels:
                var wheel = await LoadWheel(piece.ModelUrl);
                HideParts(m_OriginalWheels);
                ChangeParts(wheel, m_WheelsSockets.Select(w => w.transform).ToArray(), isModified);
                break;
        }
    }

    private async Task<Wheel> LoadWheel(string modelPath)
    {
        // очищаем от расширения
        if (modelPath.EndsWith(".asset"))
            modelPath = modelPath[..modelPath.LastIndexOf('.')];

        var request = Resources.LoadAsync<Wheel>(modelPath);
        while (!request.isDone)
            await Task.Yield();

        var wheelPrefab = request.asset as Wheel;
        if (wheelPrefab == null)
        {
            Debug.LogError($"Couldn't load the Wheel model from resources: {modelPath}");
            return null;
        }

        return wheelPrefab;
    }

    private void ChangeParts(MonoBehaviour prefab, Transform[] sockets, bool isModified)
    {
        foreach (var point in sockets)
        {
            // сначала удаляем старую деталь, если есть
            if (isModified)
            {
                var partToChange = point.GetChild(0);
                GameObject.Destroy(partToChange.gameObject);
            }
            // затем создаем новую в тот же сокет
            GameObject.Instantiate(prefab, point.transform);
        }
    }

    private void HideParts(GameObject[] parts)
    {
        foreach (var part in parts)
            part.gameObject.SetActive(false);
    }

    private void ShowParts(GameObject[] parts)
    {
        foreach (var part in parts)
            part.gameObject.SetActive(true);
    }
}
