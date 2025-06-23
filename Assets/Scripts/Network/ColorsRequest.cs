using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Model;

public static class ColorsRequest
{
    private static List<Color> availableColors = new();

    public static async Task<List<Color>> GetColors()
    {
        if (availableColors.Count == 0)
            availableColors = await NetworkManager.GetAsync<List<Color>>("colors");

        return availableColors;
    }
}

