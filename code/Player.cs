namespace Kira;

public class Player : Component
{
    private CameraComponent cam;
    public bool IsHovering { get; set; }
    public ISelectable hovering;

    protected override void OnAwake()
    {
        base.OnAwake();
        cam = Scene.Components.GetAll<CameraComponent>().FirstOrDefault();
    }

    protected override void OnUpdate()
    {
        var ray = cam.ScreenPixelToRay(Mouse.Position).Project(1400f);
        var trace = Scene.Trace.Ray(cam.Transform.Position, ray).WithTag("selectable").Run();


        if (trace.Hit)
        {
            var sl = trace.GameObject.Components.Get<ISelectable>();

            if (!IsHovering)
            {
                sl.OnHover();
                hovering = sl;
                IsHovering = true;
            }
            else if (sl.id != hovering.id)
            {
                hovering.OnLeaveHover();
                sl.OnHover();
                hovering = sl;
                IsHovering = true;
            }
        }
        else if (IsHovering)
        {
            hovering.OnLeaveHover();
            IsHovering = false;
        }
    }
}