namespace Kira;

using System;

public class Pawn : Component, ISelectable, IUnit
{
    public Guid id { get; set; }
    public string Name { get; set; }
    public int Level { get; set; }

    public bool IsSelected { get; set; }
    public bool IsHovering { get; set; }

    [Property]
    private SkinnedModelRenderer Renderer { get; set; }

    public SelectableTypes SelectableType { get; set; } = SelectableTypes.Unit;
    private Color Tint { get; set; }
    private Color HoveringTint { get; set; } = Color.Magenta;

    protected override void OnAwake()
    {
        id = GameObject.Id;
        Tint = Renderer.Tint;
    }

    public void OnHover()
    {
        Renderer.Tint = HoveringTint;
        IsHovering = true;
    }

    public void OnLeaveHover()
    {
        Renderer.Tint = Tint;
        IsHovering = false;
    }

    public void Select()
    {
        IsSelected = true;
    }

    public void Deselect()
    {
        IsSelected = false;
    }
}