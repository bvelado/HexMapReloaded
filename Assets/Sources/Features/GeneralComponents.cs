using Entitas;
using Entitas.CodeGenerator;

public interface ISelectedListener
{
    void SelectedChanged(Entity selectedEntity);
}

[View, UI]
public class SelectedListenerComponent : IComponent
{
    public ISelectedListener Listener;
}

[Core, View, Editor]
public class IdComponent : IComponent
{
    public int Id;
}

[Core, UI, View, Input]
public class DestroyComponent : IComponent { }

[Parameters, SingleEntity]
public class ActionModeComponent : IComponent {
    public ActionMode Mode;
}

public interface IActionModeChangedListener
{
    void ActionModeChanged(ActionMode mode);
}

[Core, UI, View]
public class ActionModeChangedListenerComponent : IComponent
{
    public IActionModeChangedListener Listener;
}