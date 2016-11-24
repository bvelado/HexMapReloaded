using UnityEngine;
using System.Collections;
using Entitas;
using System;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class TileInfoPanelView : MonoBehaviour, ISelectedListener {

    public TextMeshProUGUI Text;

    private StringBuilder sb;
    private string originContent;

    public void SelectedChanged(Entity selectedEntity)
    {
        if (selectedEntity != null && (selectedEntity.hasTile || selectedEntity.hasCharacter))
        {
            Display();
            Fill(selectedEntity);
        }
        else
        {
            Clear();
            Hide();
        }
    }
    
    void Awake()
    {
        originContent = Text.text;
    }

    void Start () {
        Pools.sharedInstance.uI.CreateEntity().AddSelectedListener(this);
        Hide();
	}
	
    void Fill(Entity e)
    {
        sb = new StringBuilder(originContent);

        //sb.Append("<size=\"20\"><font=\"KENPIXEL SQUARE SDF\">");
        //sb.Append("Tile");
        //sb.Append("</size></font>");
        //sb.Append(Environment.NewLine);

        sb.Replace("%position%", e.mapPosition.Position.x + " , " + e.mapPosition.Position.y + " , " + e.mapPosition.Position.z);
        //sb.Append(e.mapPosition.Position.x + " , " + e.mapPosition.Position.y + " , " + e.mapPosition.Position.z);
        
        if (Pools.sharedInstance.core.characters.CharactersByPosition.FindEntityAtMapPosition(e.mapPosition.Position) != null)
        {
            sb.Append(Environment.NewLine);
            sb.Replace("%occupant%", Pools.sharedInstance.core.characters.CharactersByPosition.FindEntityAtMapPosition(e.mapPosition.Position).character.Unit.Name);
        } else
        {
            sb.Replace("%occupant%", "");
        }

        Text.SetText(sb);
    }

    void Clear()
    {
        Text.SetText("Title");
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
