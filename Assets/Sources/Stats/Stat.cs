public interface IStat<T>
{
}

[System.Serializable]
public class Stat<T> : IStat<T> {

    public string ShortName { get; private set; }
    public string LongName { get; private set; }
    public string Description { get; private set; }

    public T BaseValue { get; private set; }
    public T CurrentValue { get; private set; }

    public Stat(string shortName, string longName, string description, T baseValue)
    {
        ShortName = shortName;
        LongName = longName;
        Description = description;
        BaseValue = baseValue;
        CurrentValue = baseValue;
    }
}