using Entitas;
using Entitas.CodeGenerator;

[Core]
public class TurnOrderComponent : IComponent {
    public int OrderIndex;
}

/// <summary>
/// When an entity is Character & Controllable, the player can control the character and have him execute actions
/// </summary>
[Core, SingleEntity]
public class ControllableComponent : IComponent { }
