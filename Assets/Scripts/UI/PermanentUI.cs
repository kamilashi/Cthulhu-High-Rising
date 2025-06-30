using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class PermanentUI : MonoBehaviour
{
    [Header("Setup in Prefab")]
    public TMP_Text headerText;
    public TMP_Text descriptionText;
    public GameObject specsInfoPanel;
    public Vector2 positionOffset;

    public bool updatePositionContinuously = true;

    [Header("Debug View")]
    [SerializeField] private Selectables hoverSource = Selectables.None;

    void Awake()
    {
        SetSpecsPanelActive(false);

        EventManager.onBlockHoverStartEvent.AddListener(OnBlockHovered);
        EventManager.onBlockHoverEndEvent.AddListener(OnDeHovered);

        EventManager.onBlockModifiedEvent.AddListener(SetBlockSpecsInfo);

        EventManager.onEquipmentHoverStartEvent.AddListener(OnEquipmentHovered);
        EventManager.onEquipmentHoverEndEvent.AddListener(OnDeHovered);

        EventManager.onEquipmentModifiedEvent.AddListener(SetEquipmentSpecsInfo);
    }

    private void Update()
    {
        if (hoverSource != Selectables.None && updatePositionContinuously)
        {
            SetSpecsInfoPosition();
        }
    }

    void SetSpecsPanelActive(bool enabled)
    {
        specsInfoPanel.SetActive(enabled);
    }

    void SetBlockSpecsInfo(Block blockObject)
    {
        headerText.text = blockObject.data.givenName;

        string description = "";
        description += "Free slots: " + blockObject.availableSlotCount + "\n";

        descriptionText.text = description;
    }

    void SetEquipmentSpecsInfo(Equipment equipmentObject)
    {
        headerText.text = equipmentObject.equipmentData.givenName;

        string description = "";
        description += "Damage: " + equipmentObject.equipmentData.damage.GetAndStoreValue() + "\n";
        description += "Attack Range: " + equipmentObject.equipmentData.attackRange.GetAndStoreValue() + "\n";
        description += "Attack Speed: " + equipmentObject.equipmentData.attackSpeed.GetAndStoreValue() + "\n";

        descriptionText.text = description;
    }
    void OnBlockHovered(Block hovered)
    {
        if (hoverSource != Selectables.None)
        {
            return;
        }

        SetBlockSpecsInfo(hovered);
        SetSpecsInfoPosition();
        SetSpecsPanelActive(true);
        hoverSource = Selectables.Block;
    }

    void OnEquipmentHovered(Equipment hovered)
    {
        if (hoverSource != Selectables.None)
        {
            return;
        }

        SetEquipmentSpecsInfo(hovered);
        SetSpecsInfoPosition();
        SetSpecsPanelActive(true);
        hoverSource = Selectables.Equipment;
    }

    void OnDeHovered(IHoverable hovered)
    {
        hoverSource = Selectables.None;
        SetSpecsPanelActive(false);
    }

    void SetSpecsInfoPosition()
    {
        Vector2 mousePosition = positionOffset;
        mousePosition.x += Input.mousePosition.x;
        mousePosition.y += Input.mousePosition.y;
        RectTransform rectTransform = specsInfoPanel.transform as RectTransform;

        float halfWidth = 0.0f; //(float) rectTransform.rect.width * 0.5f;
        float halfHeight = 0.0f; // (float) rectTransform.rect.height * 0.5f;

        mousePosition.x = (int) Mathf.Min(Screen.width - halfWidth, Mathf.Max(mousePosition.x, halfWidth));
        mousePosition.y = (int) Mathf.Min(Screen.height - halfHeight, Mathf.Max(mousePosition.y, halfHeight));

        rectTransform.position = mousePosition;
    }
}
