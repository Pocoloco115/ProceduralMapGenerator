using UnityEngine;
using TMPro;

public class GenerateMapButton : MonoBehaviour
{
    [SerializeField] private MapParametersUI parametersUI;
    [SerializeField] private MapGenerator mapGenerator;
    [SerializeField] private CorridorGenerator corridorGenerator;
    [SerializeField] private BSPRoomGenerator roomGenerator;
    [SerializeField] private TMP_Dropdown mapTypeDropdown;

    public void OnClickGenerate()
    {
        if (parametersUI == null) return;
        if (!parametersUI.ApplyUIToCustomParams()) return;

        var type = (MapType.MapTypes)mapTypeDropdown.value;

        switch (type)
        {
            case MapType.MapTypes.Simple:
                if (mapGenerator != null)
                {
                    mapGenerator.GenerateMap();
                }
                break;

            case MapType.MapTypes.Corridors:
                if (corridorGenerator != null)
                {
                    corridorGenerator.GenerateMap();
                }
                break;

            case MapType.MapTypes.BSPRooms:
                if (roomGenerator != null)
                {
                    roomGenerator.GenerateMap();
                }
                break;
        }
    }
}
