using Entitas;
using Entitas.CodeGenerator;
using UnityEngine;

[Core, SingleEntity]
public class PathComponent : IComponent {
    public Vector3[] MapPositions;
}

[View]
public class PathViewComponent : IComponent
{
    public GameObject View;
}