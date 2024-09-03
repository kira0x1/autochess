namespace Kira;

public class Stats
{
    public Stat Stamina { get; set; } = new Stat("Stamina");
    public Stat Evasion { get; set; } = new Stat("Evasion");
}

public class Stat
{
    public string Name { get; set; }
    public int Points { get; set; } = 1;

    public int MaxPoints { get; set; } = 100;
    public float PointsMod { get; set; } = 10;

    public float BaseValue { get; set; } = 10;
    public bool IsDirty { get; set; }

    private float _value;
    private Stack<StatMod> Mods { get; set; } = new Stack<StatMod>();

    public Stat(string name, int points = 1, int maxPoints = 100)
    {
        this.Name = name;
        this.Points = points;
        this.MaxPoints = maxPoints;
    }

    public float Value
    {
        get
        {
            if (IsDirty)
            {
                Recalculate();
            }

            return _value;
        }
    }

    private void Recalculate()
    {
        float val = BaseValue;

        foreach (StatMod mod in Mods)
        {
            if (mod.ModType == ModType.Flat)
            {
                val += mod.Value;
            }
            else if (mod.ModType == ModType.Percentage)
            {
                float v = mod.Value;
                v *= val;
                val += v;
            }
        }

        _value = val;
        IsDirty = false;
    }
}

public enum ModType
{
    Flat,
    Percentage
}

public class StatMod
{
    public ModType ModType { get; set; }
    public float Value { get; set; }
}