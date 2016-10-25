using Entitas;
using Entitas.CodeGenerator;

[Core]
public class CharacterComponent : IComponent
{
    public string Name;
}

[View]
public class CharacterViewComponent : IComponent
{
    public CharacterView View;
}

[Core, SingleEntity]
public class CharactersComponent : IComponent
{
    public IdIndex CharactersByID;
    public PositionIndex CharactersByPosition;
}

public interface IControlledListener
{
    void ControlledChanged(Entity controlledEntity);
}

[Core, View, UI]
public class ControlledListenerComponent : IComponent
{
    public IControlledListener Listener;
}