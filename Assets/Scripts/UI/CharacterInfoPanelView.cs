using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Entitas;
using System;

public class CharacterInfoPanelView : MonoBehaviour, ISelectedListener, IControlledListener
{
    [Header("Stats")]
    public Text Name;
    public Text Position;
    public Text HealthPoints;
    public Text MovementPoints;
    public Text SpeedPoints;

    [Header("Action panel")]
    public GameObject ActionsPanel;

    int currentCharacterId;

    public void SelectedChanged(Entity selectedEntity)
    {
        // If the selected entity is actually a character
        if (selectedEntity != null && selectedEntity.hasCharacter)
        {
            // Display the infos of the selected character
            Display();
            Fill(selectedEntity);
        }
        // If the selected entity is a tile and is occupied by a character
        else if (selectedEntity != null && selectedEntity.hasTile && Pools.sharedInstance.core.characters.CharactersByPosition.FindEntityAtMapPosition(selectedEntity.mapPosition.Position) != null)
        {
            // Display the infos of the character on the tile
            Display();
            Fill(Pools.sharedInstance.core.characters.CharactersByPosition.FindEntityAtMapPosition(selectedEntity.mapPosition.Position));
        }
        // If no tile nor character is selected
        else if (Pools.sharedInstance.core.isControllable)
        {
            // Display the infos of the current controllable character
            Display();
            Fill(Pools.sharedInstance.core.controllableEntity);
        }
        // Else
        else
        {
            // Hides the panel
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
        Name.text = "Name : " + e.character.Unit.Name;
        HealthPoints.text = e.character.Unit.Stats.HealthPoints.ShortName + " : " + e.character.Unit.Stats.HealthPoints.GetFinalValue() + "/" + e.character.Unit.Stats.HealthPoints.GetBase();
        MovementPoints.text = e.character.Unit.Stats.MovementPoints.ShortName + " : " + e.character.Unit.Stats.MovementPoints.GetFinalValue() + "/" + e.character.Unit.Stats.MovementPoints.GetBase();
        SpeedPoints.text = e.character.Unit.Stats.SpeedPoints.ShortName + " : " + e.character.Unit.Stats.SpeedPoints.GetFinalValue();

        if (e.isControllable)
            DisplayActionPanel();
        else
            HideActionPanel();
    }

    void Clear()
    {
        Position.text = "Position : ";
        Name.text = "Name : ";
        HealthPoints.text = "";
        MovementPoints.text = "";
        SpeedPoints.text = "";

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
