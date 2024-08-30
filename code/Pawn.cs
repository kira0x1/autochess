namespace Kira;

using System;

public class Pawn : Component, ISelectable, IUnit
{
    public Guid id { get; set; }

    [Property] public string Name { get; set; }
    [Property] public int Level { get; set; }

    [Property] private SkinnedModelRenderer Renderer { get; set; }
    [Property] private GameObject OutlineObject { get; set; }

    [Property] private Color HoveringTint { get; set; } = Color.Cyan.WithRed(220);
    [Property] private Color SelectedTint { get; set; } = Color.Cyan;

    public bool IsSelected { get; set; }
    public bool IsHovering { get; set; }

    public SelectableTypes SelectableType { get; set; } = SelectableTypes.Unit;
    private Color Tint { get; set; }

    public Stats Stats { get; set; } = new Stats();

    protected override void OnAwake()
    {
        id = GameObject.Id;
        Tint = Renderer.Tint;
    }

    public void OnHover()
    {
        IsHovering = true;

        if (!IsSelected)
        {
            OutlineObject.Enabled = true;
            Renderer.Tint = HoveringTint;
        }
    }

    public void OnLeaveHover()
    {
        IsHovering = false;
        OutlineObject.Enabled = IsSelected;
        Renderer.Tint = !IsSelected ? Tint : SelectedTint;
    }

    public void Select()
    {
        IsSelected = true;
        OutlineObject.Enabled = true;
        Renderer.Tint = SelectedTint;
    }

    public void Deselect()
    {
        IsSelected = false;
        OutlineObject.Enabled = false;
        Renderer.Tint = Tint;
    }
}