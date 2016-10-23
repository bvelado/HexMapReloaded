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