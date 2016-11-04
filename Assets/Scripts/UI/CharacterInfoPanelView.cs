using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Entitas;
using System;

public class CharacterInfoPanelView : MonoBehaviour, ISelectedListener, IControlledListener
{
    [Header("Infos")]
    public Text Name;
    public Text Position;

    [Header("Action panel")]
    public GameObject ActionsPanel;

    int currentCharacterId;

    public void SelectedChanged(Entity selectedEntity)
    {
        if (selectedEntity != null && selectedEntity.hasCharacter)
        {
            Display();
            Fill(selectedEntity);
        }
        else if (selectedEntity != null && selectedEntity.hasTile && Pools.sharedInstance.core.characters.CharactersByPosition.FindEntityAtMapPosition(selectedEntity.mapPosition.Position) != null)
        {
            Display();
            Fill(Pools.sharedInstance.core.characters.CharactersByPosition.FindEntityAtMapPosition(selectedEntity.mapPosition.Position));
        } else
        {
            Clear();
            Hide();
        }
    }

    void Start()
    {
        Pools.sharedInstance.uI.CreateEntity().AddSelectedListener(this).AddControlledListener(this);
        Hide();
    }

    /// <summary>
    /// Fills the Character info panel with selected character info
    /// </summary>
    /// <param name="e">Must be filled with a iD / Character / MapPosition entity</param>
    void Fill(Entity e)
    {
        currentCharacterId = e.id.Id;

        Position.text = "Position : " + e.mapPosition.Position.x + " , " + e.mapPosition.Position.y + " , " + e.mapPosition.Position.z;
        Name.text = "Name : " + e.character.Name;

        if (e.isControllable)
            DisplayActionPanel();
        else
            HideActionPanel();
    }

    void Clear()
    {
        Position.text = "Position : ";
        Name.text = "Name : ";

        HideActionPanel();
    }

    void Hide()
    {
        gameObject.SetActive(false);
    }

    void HideActionPanel()
    {
        ActionsPanel.SetActive(false);
    }

    void Display()
    {
        gameObject.SetActive(true);
    }

    void DisplayActionPanel() {
        ActionsPanel.SetActive(true);
    }

    public void ControlledChanged(Entity controlledEntity)
    {
        if (controlledEntity.id.Id != currentCharacterId)
            HideActionPanel();
        else
            DisplayActionPanel();
    }
}
