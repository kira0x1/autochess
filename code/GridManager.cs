namespace Kira;

using System;

public sealed class GridManager : Component
{
    [Property, Range(1, 12)] public int CellCount { get; set; } = 9;
    [Property, Range(0, 25)] public float CellSize { get; set; } = 14f;
    [Property, Range(0, 15)] public float CellOffset { get; set; } = 7.7f;
    [Property, Range(0, 50f)] public float CellHeight { get; set; } = 16f;
    [Property, Range(0.1f, 4f)] public float LineThickness { get; set; } = 1f;

    private List<CellSlot> cells = new List<CellSlot>();
    private bool updateCells = false;

    protected override void OnValidate()
    {
        updateCells = true;
    }

    protected override void OnStart()
    {
        CreateCells();
    }

    protected override void OnUpdate()
    {
        if (updateCells)
        {
            UpdateCells();
        }

        DrawCells();
    }

    private void DrawCells()
    {
        foreach (CellSlot c in cells)
        {
            Gizmo.Draw.Color = c.Occupied ? Color.Red : Color.White;
            Gizmo.Draw.LineThickness = LineThickness;

            var p = c.Position;
            var max = p + CellSize;
            max.z = p.z + CellHeight;
            BBox b = new BBox(p, max);

            Gizmo.Draw.LineBBox(b);
        }
    }

    private void CreateCells()
    {
        cells = new List<CellSlot>();
        var pos = GameObject.Transform.Position;

        float py = pos.y;
        py -= CellSize * CellCount;
        py /= 2f;
        py -= CellSize / 2f;
        py -= CellCount / 2f * CellOffset;

        for (int i = 1; i < CellCount; i++)
        {
            bool isocc = Random.Shared.Float(0f, 1f) > 0.6f;
            py += CellOffset + CellSize;
            pos.y = py;

            CellSlot slot = new CellSlot(i);
            slot.Position = pos;
            slot.Occupied = isocc;
            cells.Add(slot);
        }

        updateCells = false;
    }

    private void UpdateCells()
    {
        float halfsize = CellSize / 2f;

        var pos = GameObject.Transform.Position;
        float py = (Transform.Position.y - CellSize * CellCount / 2f) - halfsize - CellCount / 2f * CellOffset;

        foreach (CellSlot cell in cells)
        {
            py += CellOffset + CellSize;
            pos.y = py;

            cell.Position = pos;
        }

        updateCells = false;
    }
}

public class CellSlot
{
    public int Id { get; set; }
    public bool Occupied { get; set; }
    public Vector3 Position { get; set; }

    public CellSlot(int id)
    {
        Id = id;
    }
}