using UnityEngine;
using System.Collections;
using Entitas;
using System;
using UnityEngine.UI;

public class TileInfoPanelView : MonoBehaviour, ISelectedListener {

    public Text Position;
    public Text Occupied;

    public void SelectedChanged(Entity selectedEntity)
    {
        if(selectedEntity != null && (selectedEntity.hasTile || selectedEntity.hasCharacter))
        {
            Display();
            Fill(selectedEntity);
        } else
        {
            Clear();
            Hide();
        }
    }
    
    void Start () {
        Pools.sharedInstance.uI.CreateEntity().AddSelectedListener(this);
        Hide();
	}
	
    void Fill(Entity e)
    {
        Position.text = "Position : " + e.mapPosition.Position.x + " , " + e.mapPosition.Position.y + " , " + e.mapPosition.Position.z;

        if (Pools.sharedInstance.core.characters.Characters.FindEntityAtMapPosition(e.mapPosition.Position) != null)
        {
            Occupied.text = Pools.sharedInstance.core.characters.Characters.FindEntityAtMapPosition(e.mapPosition.Position).character.Name;
        } else
        {
            Occupied.text = " Empty";
        }
    }

    void Clear()
    {
        Position.text = "Position :";
        Occupied.text = "Empty";
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
