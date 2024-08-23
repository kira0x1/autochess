namespace Kira;

using System;

public interface ISelectable
{
    public Guid id { get; set; }
    public bool IsSelected { get; set; }
    public bool IsHovering { get; set; }
    public SelectableTypes SelectableType { get; set; }

    public void OnHover();
    public void OnLeaveHover();
    public void Select();
    public void Deselect();
}

public interface IUnit : ISelectable
{
    public string Name { get; set; }
    public int Level { get; set; }
}

public enum SelectableTypes
{
    Item,
    Unit
}