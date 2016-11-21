using System;
using System.Collections.Generic;
using UnityEngine;

public interface IStat
{
    int GetBase();
    float GetMultiplier();
}


[System.Serializable]
public class Stat : BaseStat
{
    public string ShortName { get; private set; }
    public string LongName { get; private set; }
    public string Description { get; private set; }

    private List<RawStatModifier> RawAttributeModifiers;
    private List<FinalStatModifier> FinalAttributeModifiers;

    private int RawValue;
    private int FinalValue;

    public Stat(int baseValue, string shortName, string longName, string description) : base(baseValue)
    {
        ShortName = shortName;
        LongName = longName;
        Description = description;

        // Raw modifiers are constant modifiers applied by Equipment or Perks.
        // They persist between fights.
        RawAttributeModifiers = new List<RawStatModifier>();

        // Final modifiers are fight related modifiers applied by Buffs or Debuffs.
        // They are reset at every fight start or end.
        FinalAttributeModifiers = new List<FinalStatModifier>();

        FinalValue = baseValue;
    }

    public void AddRawModifier(RawStatModifier modifier)
    {
        RawAttributeModifiers.Add(modifier);
    }

    public void RemoveRawModifier(RawStatModifier modifier)
    {
        RawAttributeModifiers.Remove(modifier);
    }

    public void AddFinalModifier(FinalStatModifier modifier)
    {
        FinalAttributeModifiers.Add(modifier);
    }

    public void RemoveFinalModifier(FinalStatModifier modifier)
    {
        FinalAttributeModifiers.Remove(modifier);
    }

    int CalculateFinalValue()
    {
        FinalValue = BaseValue;

        int RawModifierValue = 0;
        float RawModifierMultiplier = 0;

        foreach(var modifier in RawAttributeModifiers)
        {
            RawModifierValue += modifier.GetBase();
            RawModifierMultiplier += modifier.GetMultiplier();
        }

        FinalValue += RawModifierValue;
        FinalValue *= Mathf.RoundToInt(1f + RawModifierMultiplier);

        int FinalModifierValue = 0;
        float FinalModifierMultiplier = 0;

        foreach (var modifier in FinalAttributeModifiers)
        {
            FinalModifierValue += modifier.GetBase();
            FinalModifierMultiplier += modifier.GetMultiplier();
        }

        FinalValue += FinalModifierValue;
        FinalValue *= Mathf.RoundToInt(1f + FinalModifierMultiplier);

        return FinalValue;
    }

    int CalculateRawValue()
    {
        RawValue = BaseValue;

        int RawModifierValue = 0;
        float RawModifierMultiplier = 0;

        foreach (var modifier in RawAttributeModifiers)
        {
            RawModifierValue += modifier.GetBase();
            RawModifierMultiplier += modifier.GetMultiplier();
        }

        RawValue += RawModifierValue;
        RawValue *= Mathf.RoundToInt(1f + RawModifierMultiplier);

        return RawValue;
    }

    public int GetRawValue()
    {
        return CalculateRawValue();
    }

    public int GetFinalValue()
    {
        return CalculateFinalValue();
    }
}

[System.Serializable]
public class BaseStat {
    
    protected int BaseValue { get; set; }
    protected float BaseMultiplier { get; set; }

    public BaseStat(int baseValue, float baseMultiplier = 0)
    {
        BaseValue = baseValue;
        BaseMultiplier = baseValue;
    }

    public int GetBase()
    {
        return BaseValue;
    }

    public float GetMultiplier()
    {
        return BaseMultiplier;
    }
}

[System.Serializable]
public class RawStatModifier : BaseStat
{
    public RawStatModifier(int baseValue = 0, float baseMultiplier = 0) : base(baseValue, baseMultiplier)
    {
    }
}

[System.Serializable]
public class FinalStatModifier : BaseStat
{
    private int TurnsLeft;
    private Stat Parent;

    /// <summary>
    /// Creates a new stat modifier applied after the raw modifiers.
    /// </summary>
    /// <param name="baseValue">Value added to the stat</param>
    /// <param name="baseMultiplier">Value multiplied by the stat</param>
    /// <param name="turnsLeft">Number of turns the multiplier will be active. Set to -1 to an infinite modifier.</param>
    public FinalStatModifier(int baseValue = 0, float baseMultiplier = 0, int turnsLeft = 0, Stat parent = null) : base(baseValue, baseMultiplier)
    {
        if(turnsLeft > 0)
        {
            TurnsLeft = turnsLeft;

            if (parent != null)
                Parent = parent;
            else
                throw new Exception("Cannot add " + this + " because no parent stat is provided.");
        }   
    }

    public void OnTurnEnd()
    {
        if (TurnsLeft > 0)
            TurnsLeft--;

        // When turns left reach 0
        if (TurnsLeft == 0)
            Parent.RemoveFinalModifier(this);

    }
}