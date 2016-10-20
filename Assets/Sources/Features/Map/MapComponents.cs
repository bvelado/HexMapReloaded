using Entitas;
using Entitas.CodeGenerator;
using UnityEngine;

[Core]
public class MapPositionComponent : IComponent
{
    public Vector3 Position;
}

[Core]
public class WorldPositionComponent : IComponent
{
    public Vector3 Position;
}

[Core]
public class TileComponent : IComponent
{
    public string Description;
}

[Core, SingleEntity]
public class MapComponent : IComponent
{
    public PositionIndex Map;
}
