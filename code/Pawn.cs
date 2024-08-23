namespace Kira;

using System;

public class Pawn : Component, ISelectable, IUnit
{
    public Guid id { get; set; }

    [Property] public string Name { get; set; }
    [Property] public int Level { get; set; }

    [Property] private SkinnedModelRenderer Renderer { get; set; }
    [Property] private Color HoveringTint { get; set; } = Color.Cyan.WithRed(220);
    [Property] private Color SelectedTint { get; set; } = Color.Cyan;

    public bool IsSelected { get; set; }
    public bool IsHovering { get; set; }

    public SelectableTypes SelectableType { get; set; } = SelectableTypes.Unit;
    private Color Tint { get; set; }


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
            Renderer.Tint = HoveringTint;
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
        Renderer.Tint = SelectedTint;
    }

    public void Deselect()
    {
        IsSelected = false;
        Renderer.Tint = Tint;
    }
}