using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Entitas;

public class CharacterInfoPanelView : MonoBehaviour, ISelectedListener
{
    public Text Name;
    public Text Position;

    [Header("Action panel")]
    public GameObject ActionsPanel;

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
        Pools.sharedInstance.uI.CreateEntity().AddSelectedListener(this);
        Hide();
    }

    void Fill(Entity e)
    {
        Position.text = "Position : " + e.mapPosition.Position.x + " , " + e.mapPosition.Position.y + " , " + e.mapPosition.Position.z;
        Name.text = "Name : " + e.character.Name;
    }

    void Clear()
    {
        Position.text = "Position : ";
        Name.text = "Name : ";
    }

    void Hide()
    {
        gameObject.SetActive(false);
    }

    void Display()
    {
        gameObject.SetActive(true);
    }
}
