namespace Kira;

public sealed class GridManager : Component
{
    [Property, Range(1, 12)] public int CellCount { get; set; } = 9;
    [Property, Range(0, 225)] public float CellSize { get; set; } = 14f;
    [Property, Range(0, 15)] public float CellOffset { get; set; } = 7.7f;
    [Property, Range(0, 50f)] public float CellHeight { get; set; } = 16f;
    [Property, Range(0.1f, 4f)] public float LineThickness { get; set; } = 1f;

    [Property, Category("Text"), Range(0, 100)] private float TextSize { get; set; } = 50;
    [Property, Category("Text")] private bool ShowText { get; set; }

    private List<CellSlot> cells = new List<CellSlot>();
    private bool updateCells = false;
    private Vector2 MousePos { get; set; }

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
            CreateCells();
            UpdateCells();
        }

        HandleCamera();
        DrawCells();
    }

    private void DrawCells()
    {
        for (var i = 0; i < cells.Count; i++)
        {
            CellSlot c = cells[i];
            Gizmo.Draw.Color = c.Occupied ? Color.Red : Color.White;
            Gizmo.Draw.LineThickness = LineThickness;

            var p = c.Position;
            var max = p + CellSize;
            max.z = p.z + CellHeight;
            BBox b = new BBox(p, max);

            var isX = MousePos.x < max.x && MousePos.x > p.x;
            var isY = MousePos.y < max.y && MousePos.y > p.y;

            if (isX && isY)
            {
                Gizmo.Draw.Color = Color.Magenta;
            }

            Gizmo.Draw.LineBBox(b);

            if (ShowText)
            {
                float cx = b.Center.x;
                float cy = b.Center.y;

                Gizmo.Draw.WorldText($"{cy:F0}, {cx:F0}", new Transform(b.Center, Rotation.From(0, -90, 0), 0.2f), "Roboto", TextSize, TextFlag.Center);
            }
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
            py += CellOffset + CellSize;
            pos.y = py;

            CellSlot slot = new CellSlot(i);
            slot.Position = pos;
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
            pos.z = 0;
            cell.Position = pos;
        }

        updateCells = false;
    }

    private void HandleCamera()
    {
        CameraComponent cam = Scene.Components.GetAll<CameraComponent>().FirstOrDefault();

        if (!cam.IsValid())
        {
            Log.Info("cam not found");
            return;
        }

        var ray = cam.ScreenPixelToRay(Mouse.Position).Project(400f);
        var mpos = Scene.Trace.Ray(cam.Transform.Position, ray).Run().EndPosition;
        MousePos = new Vector2(mpos.x, mpos.y);
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