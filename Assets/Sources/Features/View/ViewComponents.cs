using Entitas;
using Entitas.CodeGenerator;

[View, SingleEntity]
public class MapViewComponent : IComponent
{
    public IdIndex TileViewById;
}

[View, SingleEntity]
public class CharactersViewComponent : IComponent
{
    public IdIndex CharacterViewById;
}

[Core]
public class HighlightComponent : IComponent {
    public HighlightMode Mode;
}