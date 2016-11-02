using Entitas;
using Entitas.CodeGenerator;

[View, SingleEntity]
public class MapViewComponent : IComponent
{
    public IdIndex TileViewById;
}

[Core]
public class HighlightComponent : IComponent {
    public HighlightMode Mode;
}