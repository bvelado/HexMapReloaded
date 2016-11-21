[System.Serializable]
public class UnitStats {

    public Stat<int> HealthPoints { get; private set; }
    public Stat<int> MovementPoints { get; private set; }
    public Stat<int> SpeedPoints { get; private set; }

    public UnitStats(int baseHealth, int baseMovement, int baseSpeed)
    {
        HealthPoints = new Stat<int>("HP", "Health points", "Unit health points. When they reach 0, the unit will die.", baseHealth);
        MovementPoints = new Stat<int>("Mov", "Movement points", "Unit movement points. The unit can be moved for its amount of movement points every turn.", baseMovement);
        SpeedPoints = new Stat<int>("Spd", "Speed points", "Unit speed points. During a fight, the unit play order is determined by its speed points. The higher the speed points, the earlier it will play.", baseSpeed);
    }
}

public class Unit
{
    public string Name { get; private set; }
    public UnitStats Stats { get; private set; }

    public Unit(string name, int baseHealth, int baseMovement, int baseSpeed)
    {
        Name = name;
        Stats = new UnitStats(baseHealth, baseMovement, baseSpeed);
    }
}