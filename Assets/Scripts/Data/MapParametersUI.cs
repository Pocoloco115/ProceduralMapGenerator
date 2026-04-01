using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapParametersUI : MonoBehaviour
{
    [Header("Scriptable Objects")]
    [SerializeField] private MapGeneratorSO defaultParameters;
    [SerializeField] private MapGeneratorSO customParameters;

    [Header("Selectors")]
    [SerializeField] private TMP_Dropdown mapTypeDropdown;
    [SerializeField] private TMP_Dropdown modeDropdown;

    [Header("Panels by type")]
    [SerializeField] private GameObject panelSimpleRandomWalkRoom;
    [SerializeField] private GameObject panelCorridors;
    [SerializeField] private GameObject panelBSPRooms;

    [Header("Random walk rooms")]
    [SerializeField] private TMP_InputField walkLengthInput;
    [SerializeField] private TMP_InputField iterationsInput;
    [SerializeField] private Toggle startRandomlyEachIterationToggle;

    [Header("Corridors")]
    [SerializeField] private TMP_InputField corridorLengthInput;
    [SerializeField] private TMP_InputField corridorCountInput;
    [SerializeField] private Slider roomPercentInput;
    [SerializeField] private TMP_Dropdown corridorWideningModeDropdown;

    [Header("BSP Rooms")]
    [SerializeField] private TMP_InputField dungeonWidthInput;
    [SerializeField] private TMP_InputField dungeonHeightInput;
    [SerializeField] private TMP_InputField minRoomWidthInput;
    [SerializeField] private TMP_InputField minRoomHeightInput;
    [SerializeField] private Slider offsetInput;
    [SerializeField] private Toggle randomWalkRoomsToggle;

    [Header("Invalid data feedback")]
    [SerializeField] private Animator invalidDataAnimator;
    [SerializeField] private string invalidDataTriggerName = "Show";

    private MapType.MapTypes currentType => (MapType.MapTypes)mapTypeDropdown.value;
    private bool isDefaultMode => modeDropdown.value == 0;
    private MapGeneratorSO lastCustomSnapshot;

    private void Start()
    {
        if (defaultParameters != null && customParameters != null)
        {
            CopyParams(defaultParameters);
        }

        lastCustomSnapshot = ScriptableObject.CreateInstance<MapGeneratorSO>();
        CopyParams(customParameters, lastCustomSnapshot);

        mapTypeDropdown.onValueChanged.AddListener(_ => RefreshPanelsAndLoad());
        modeDropdown.onValueChanged.AddListener(OnModeChanged);

        RefreshPanelsAndLoad();
    }

    private void OnDestroy()
    {
        mapTypeDropdown.onValueChanged.RemoveAllListeners();
        modeDropdown.onValueChanged.RemoveAllListeners();
    }

    private void OnModeChanged(int _)
    {
        if (isDefaultMode)
        {
            if (defaultParameters != null && customParameters != null)
            {
                CopyParams(defaultParameters);
            }
        }
        else if (lastCustomSnapshot != null && customParameters != null)
        {
            CopyParams(lastCustomSnapshot, customParameters);
        }

        RefreshPanelsAndLoad();
    }

    private void RefreshPanelsAndLoad()
    {
        if (panelBSPRooms) { panelBSPRooms.SetActive(true); }
        if (panelCorridors) { panelCorridors.SetActive(true); }
        if (panelSimpleRandomWalkRoom) { panelSimpleRandomWalkRoom.SetActive(true); }

        if (isDefaultMode)
        {
            SetPanelInteractable(panelBSPRooms, false);
            SetPanelInteractable(panelCorridors, false);
            SetPanelInteractable(panelSimpleRandomWalkRoom, false);

            LoadParamsToUI(customParameters);
            return;
        }

        SetPanelInteractable(panelBSPRooms, false);
        SetPanelInteractable(panelCorridors, false);
        SetPanelInteractable(panelSimpleRandomWalkRoom, false);

        switch (currentType)
        {
            case MapType.MapTypes.Simple:
                SetPanelInteractable(panelSimpleRandomWalkRoom, true);
                break;
            case MapType.MapTypes.Corridors:
                SetPanelInteractable(panelSimpleRandomWalkRoom, true);
                SetPanelInteractable(panelCorridors, true);
                break;
            case MapType.MapTypes.BSPRooms:
                SetPanelInteractable(panelSimpleRandomWalkRoom, true);
                SetPanelInteractable(panelBSPRooms, true);
                break;
        }

        LoadParamsToUI(customParameters);
    }

    private void LoadParamsToUI(MapGeneratorSO so)
    {
        if (so == null) { return; }

        if (walkLengthInput) { walkLengthInput.text = so.walkLength.ToString(); }
        if (iterationsInput) { iterationsInput.text = so.iterations.ToString(); }
        if (startRandomlyEachIterationToggle) { startRandomlyEachIterationToggle.isOn = so.startRandomlyEachIteration; }

        if (corridorLengthInput) { corridorLengthInput.text = so.corridorLength.ToString(); }
        if (corridorCountInput) { corridorCountInput.text = so.corridorCount.ToString(); }
        if (roomPercentInput) { roomPercentInput.value = so.roomPercent / 100; }
        if (corridorWideningModeDropdown) { corridorWideningModeDropdown.value = (int)so.corridorWideningMode; }

        if (dungeonWidthInput) { dungeonWidthInput.text = so.dungeonWidth.ToString(); }
        if (dungeonHeightInput) { dungeonHeightInput.text = so.dungeonHeight.ToString(); }
        if (minRoomWidthInput) { minRoomWidthInput.text = so.minRoomWidth.ToString(); }
        if (minRoomHeightInput) { minRoomHeightInput.text = so.minRoomHeight.ToString(); }
        if (offsetInput) { offsetInput.value = so.offset; }
        if (randomWalkRoomsToggle) { randomWalkRoomsToggle.isOn = so.randomWalkRooms; }
    }

    private void SetPanelInteractable(GameObject panel, bool isInteractable)
    {
        if (panel == null) { return; }

        foreach (var input in panel.GetComponentsInChildren<TMP_InputField>(true))
            input.interactable = isInteractable;

        foreach (var toggle in panel.GetComponentsInChildren<Toggle>(true))
            toggle.interactable = isInteractable;

        foreach (var dropdown in panel.GetComponentsInChildren<TMP_Dropdown>(true))
            dropdown.interactable = isInteractable;

        foreach (var slider in panel.GetComponentsInChildren<Slider>(true))
            slider.interactable = isInteractable;

        Color textColor = isInteractable ? Color.white : new Color(1f, 1f, 1f, 0.4f);
        Color imageColor = isInteractable ? Color.white : new Color(1f, 1f, 1f, 0.3f);

        foreach (var text in panel.GetComponentsInChildren<TMP_Text>(true))
        {
            if (text.CompareTag("Panel")) continue;
            text.color = textColor;
        }

        foreach (var image in panel.GetComponentsInChildren<Image>(true))
        {
            if (image.CompareTag("Panel")) continue;
            image.color = imageColor;
        }
    }

    internal void CopyParams(MapGeneratorSO from)
    {
        CopyParams(from, customParameters);
    }

    internal void CopyParams(MapGeneratorSO from, MapGeneratorSO to)
    {
        if (from == null || to == null) { return; }

        to.walkLength = from.walkLength;
        to.iterations = from.iterations;
        to.startRandomlyEachIteration = from.startRandomlyEachIteration;

        to.corridorLength = from.corridorLength;
        to.corridorCount = from.corridorCount;
        to.roomPercent = from.roomPercent;
        to.corridorWideningMode = from.corridorWideningMode;

        to.dungeonWidth = from.dungeonWidth;
        to.dungeonHeight = from.dungeonHeight;
        to.minRoomWidth = from.minRoomWidth;
        to.minRoomHeight = from.minRoomHeight;
        to.offset = from.offset;
        to.randomWalkRooms = from.randomWalkRooms;
    }

    public bool ApplyUIToCustomParams()
    {
        if (isDefaultMode) return true;
        if (customParameters == null) return false;

        bool invalid = false;

        int walkLen = customParameters.walkLength;
        int iters = customParameters.iterations;

        if (walkLengthInput && !string.IsNullOrWhiteSpace(walkLengthInput.text))
        {
            if (!int.TryParse(walkLengthInput.text, out walkLen) || walkLen < 1)
                invalid = true;
        }

        if (iterationsInput && !string.IsNullOrWhiteSpace(iterationsInput.text))
        {
            if (!int.TryParse(iterationsInput.text, out iters) || iters < 1)
                invalid = true;
        }

        int corridorLen = customParameters.corridorLength;
        int corridorCnt = customParameters.corridorCount;
        float roomPercent = customParameters.roomPercent;

        if (currentType == MapType.MapTypes.Corridors)
        {
            if (corridorLengthInput && !string.IsNullOrWhiteSpace(corridorLengthInput.text))
            {
                if (!int.TryParse(corridorLengthInput.text, out corridorLen) || corridorLen < 1)
                    invalid = true;
            }

            if (corridorCountInput && !string.IsNullOrWhiteSpace(corridorCountInput.text))
            {
                if (!int.TryParse(corridorCountInput.text, out corridorCnt) || corridorCnt < 1)
                    invalid = true;
            }

            if (roomPercentInput)
                roomPercent = roomPercentInput.value / 100;
        }

        int dungeonW = customParameters.dungeonWidth;
        int dungeonH = customParameters.dungeonHeight;
        int minW = customParameters.minRoomWidth;
        int minH = customParameters.minRoomHeight;
        int off = customParameters.offset;

        if (currentType == MapType.MapTypes.BSPRooms)
        {
            if (dungeonWidthInput && !string.IsNullOrWhiteSpace(dungeonWidthInput.text))
            {
                if (!int.TryParse(dungeonWidthInput.text, out dungeonW) || dungeonW < 1)
                    invalid = true;
            }

            if (dungeonHeightInput && !string.IsNullOrWhiteSpace(dungeonHeightInput.text))
            {
                if (!int.TryParse(dungeonHeightInput.text, out dungeonH) || dungeonH < 1)
                    invalid = true;
            }

            if (minRoomWidthInput && !string.IsNullOrWhiteSpace(minRoomWidthInput.text))
            {
                if (!int.TryParse(minRoomWidthInput.text, out minW) || minW < 1)
                    invalid = true;
            }

            if (minRoomHeightInput && !string.IsNullOrWhiteSpace(minRoomHeightInput.text))
            {
                if (!int.TryParse(minRoomHeightInput.text, out minH) || minH < 1)
                    invalid = true;
            }

            if (offsetInput)
                off = Mathf.RoundToInt(offsetInput.value);

            if (minW > dungeonW || minH > dungeonH)
                invalid = true;

            int maxOffset = Mathf.Max(0, Mathf.Min(minW - 1, minH - 1));
            if (off < 0 || off > maxOffset)
                invalid = true;
        }

        if (invalid)
        {
            Debug.LogWarning("Invalid input detected. Please correct the values.");
            TriggerInvalidDataAnimation();
            return false;
        }

        customParameters.walkLength = walkLen;
        customParameters.iterations = iters;
        if (startRandomlyEachIterationToggle)
            customParameters.startRandomlyEachIteration = startRandomlyEachIterationToggle.isOn;

        if (currentType == MapType.MapTypes.Corridors)
        {
            customParameters.corridorLength = corridorLen;
            customParameters.corridorCount = corridorCnt;
            customParameters.roomPercent = roomPercent;
            if (corridorWideningModeDropdown)
            {
                customParameters.corridorWideningMode =
                    (CorridorGenerator.CorridorWideningMode)corridorWideningModeDropdown.value;
            }
        }

        if (currentType == MapType.MapTypes.BSPRooms)
        {
            customParameters.dungeonWidth = dungeonW;
            customParameters.dungeonHeight = dungeonH;
            customParameters.minRoomWidth = minW;
            customParameters.minRoomHeight = minH;

            int maxOffset = Mathf.Max(0, Mathf.Min(minW - 1, minH - 1));
            off = Mathf.Clamp(off, 0, maxOffset);
            customParameters.offset = off;

            if (randomWalkRoomsToggle)
                customParameters.randomWalkRooms = randomWalkRoomsToggle.isOn;
        }

        if (lastCustomSnapshot != null)
        {
            CopyParams(customParameters, lastCustomSnapshot);
        }

        return true;
    }

    private void TriggerInvalidDataAnimation()
    {
        if (invalidDataAnimator != null && !string.IsNullOrEmpty(invalidDataTriggerName))
            invalidDataAnimator.SetTrigger(invalidDataTriggerName);
    }
}
