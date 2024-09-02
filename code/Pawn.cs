namespace Kira;

using System;

public class Pawn : Component, ISelectable, IUnit
{
    public Guid id { get; set; }

    [Property] public string Name { get; set; }
    [Property] public int Level { get; set; }

    [Property, Category("Outline")] private SkinnedModelRenderer Renderer { get; set; }
    [Property, Category("Outline")] private GameObject OutlineObject { get; set; }
    [Property, Category("Outline")] private bool UseOutlineModel { get; set; }
    [Property] private GameObject SelectionGlow { get; set; }

    [Property] private Color HoveringTint { get; set; } = Color.Cyan.WithRed(220);
    [Property] private Color SelectedTint { get; set; } = Color.Cyan;
    private Color Tint { get; set; }

    public SelectableTypes SelectableType { get; set; } = SelectableTypes.Unit;
    public Stats Stats { get; set; } = new Stats();
    public bool IsSelected { get; set; }
    public bool IsHovering { get; set; }

    protected override void OnAwake()
    {
        id = GameObject.Id;
        Tint = Renderer.Tint;
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
        // SelectionGlow.Enabled = IsSelected;
        // if (UseOutlineModel) OutlineObject.Enabled = IsSelected;
    }

    public void Select()
    {
        IsSelected = true;
        if (UseOutlineModel) OutlineObject.Enabled = true;
        SelectionGlow.Enabled = true;
        // Renderer.Tint = SelectedTint;
    }

    public void Deselect()
    {
        IsSelected = false;
        if (UseOutlineModel) OutlineObject.Enabled = false;
        SelectionGlow.Enabled = false;
        Renderer.Tint = Tint;
    }
}