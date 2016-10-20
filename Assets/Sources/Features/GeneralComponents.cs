using Entitas;


public interface ISelectedListener
{
    void SelectedChanged(Entity selectedEntity);
}

[View]
public class SelectedListenerComponent : IComponent
{
    public ISelectedListener Listener;
}
