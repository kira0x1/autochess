namespace Kira;

public class Player : Component
{
    private CameraComponent cam;

    public bool IsHovering { get; set; }
    public bool HasSelection { get; set; }

    public IUnit hovering;
    public IUnit SelectedUnit { get; set; }

    protected override void OnAwake()
    {
        base.OnAwake();
        cam = Scene.Components.GetAll<CameraComponent>().FirstOrDefault();
    }

    protected override void OnUpdate()
    {
        UpdateRay();
    }

    private void UpdateRay()
    {
        var ray = cam.ScreenPixelToRay(Mouse.Position).Project(1400f);
        var trace = Scene.Trace.Ray(cam.Transform.Position, ray).WithTag("selectable").Run();

        if (!trace.Hit)
        {
            if (IsHovering)
            {
                hovering.OnLeaveHover();
                IsHovering = false;
            }

            if (HasSelection && Input.Pressed("attack1"))
            {
                SelectedUnit.Deselect();
                HasSelection = false;
            }

            return;
        }

        var sl = trace.GameObject.Components.Get<ISelectable>();
        if (sl != null)
        {
            if (sl.SelectableType != SelectableTypes.Unit)
            {
                return;
            }

            var unit = trace.GameObject.Components.Get<IUnit>();
            if (unit == null) return;

            if (Input.Pressed("attack1"))
            {
                SelectUnit(unit);
                return;
            }

            HandleHovering(unit);
        }
    }

    private void SelectUnit(IUnit unit)
    {
        if (HasSelection)
        {
            SelectedUnit.Deselect();
        }

        unit.Select();
        SelectedUnit = unit;
        HasSelection = true;
    }

    private void HandleHovering(IUnit selectable)
    {
        if (IsHovering && selectable.id != hovering.id)
        {
            hovering.OnLeaveHover();
        }

        if (!IsHovering)
        {
            selectable.OnHover();
            hovering = selectable;
            IsHovering = true;
        }
    }
}