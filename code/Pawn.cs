namespace Kira;

using System;

public class PawnVitals
{
    public float Health { get; set; } = 100f;
    public float MaxHealth { get; set; } = 10f;
    public float BaseMaxHealth { get; set; } = 10f;
    public float StaminaMod { get; set; } = 10f;

    public void UpdateVitals(Stats stats)
    {
        UpdateMaxHealth(stats.Stamina);
    }

    public void UpdateMaxHealth(Stat stamina)
    {
        float max = BaseMaxHealth;
        max += stamina.Value * StaminaMod;
        MaxHealth = max;
    }
}

public class Pawn : Component, IUnit
{
    public Guid id { get; set; }

    [Property] public string Name { get; set; }
    [Property] public int Level { get; set; }

    public PawnInventory Inventory { get; set; } = new PawnInventory();
    public Stats Stats { get; set; } = new Stats();
    public PawnVitals Vitals { get; set; } = new PawnVitals();

    [Property, Category("Outline")] private SkinnedModelRenderer Renderer { get; set; }
    [Property, Category("Outline")] private GameObject OutlineObject { get; set; }
    [Property, Category("Outline")] private bool UseOutlineModel { get; set; }
    [Property] private GameObject SelectionGlow { get; set; }

    [Property] private Color HoveringTint { get; set; } = Color.Cyan.WithRed(220);
    [Property] private Color SelectedTint { get; set; } = Color.Cyan;
    private Color Tint { get; set; }

    public SelectableTypes SelectableType { get; set; } = SelectableTypes.Unit;
    public bool IsSelected { get; set; }
    public bool IsHovering { get; set; }

    protected override void OnAwake()
    {
        id = GameObject.Id;
        Tint = Renderer.Tint;
        Vitals.UpdateVitals(Stats);
    }

    public void OnHover()
    {
        IsHovering = true;
        Renderer.Tint = HoveringTint;

        if (!IsSelected)
        {
            if (UseOutlineModel) OutlineObject.Enabled = true;
        }
    }

    public void OnLeaveHover()
    {
        IsHovering = false;
        Renderer.Tint = !IsSelected ? Tint : SelectedTint;
    }

    public void Select()
    {
        IsSelected = true;
        if (UseOutlineModel) OutlineObject.Enabled = true;
        SelectionGlow.Enabled = true;
    }

    public void Deselect()
    {
        IsSelected = false;
        if (UseOutlineModel) OutlineObject.Enabled = false;
        SelectionGlow.Enabled = false;
        Renderer.Tint = Tint;
    }
}