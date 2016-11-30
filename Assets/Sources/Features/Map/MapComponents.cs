using Entitas;
using Entitas.CodeGenerator;
using UnityEngine;

[Core, Editor]
public class MapPositionComponent : IComponent
{
    public Vector3 Position;
}

[View]
public class WorldPositionComponent : IComponent
{
    public Vector3 Position;
}

[Core, Editor]
public class TileComponent : IComponent
{
    public string Description;
}

[View, Editor]
public class TileViewComponent : IComponent
{
    public TileView View;
}

[Core, SingleEntity]
public class SelectedComponent : IComponent { }

[Core, Editor, SingleEntity]
public class MapComponent : IComponent
{
    public PositionIndex TilesByMapPosition;
    public IdIndex TilesByIndex;
}

[Core]
public class WalkableComponent : IComponent { }

[Core]
public class NodeComponent : IComponent
{
    public Node Node;
}