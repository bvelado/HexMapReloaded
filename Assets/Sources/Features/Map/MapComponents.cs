using Entitas;
using Entitas.CodeGenerator;
using UnityEngine;

[Core]
public class MapPositionComponent : IComponent
{
    public Vector3 Position;
}

[View]
public class WorldPositionComponent : IComponent
{
    public Vector3 Position;
}

[Core]
public class TileComponent : IComponent
{
    public string Description;
}

[View]
public class TileViewComponent : IComponent
{
    public TileView View;
}

[Core, SingleEntity]
public class SelectedComponent : IComponent { }

[Core, SingleEntity]
public class MapComponent : IComponent
{
    public PositionIndex TilesByMapPosition;
    public IdIndex TilesByIndex;
}

