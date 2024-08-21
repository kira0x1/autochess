namespace Kira;

public class Player : Component
{
    private CameraComponent cam;
    private bool IsHovering { get; set; }
    private ISelectable hovering;

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

            Log.Info(trace.GameObject.Name);
        }
        else if (IsHovering)
        {
            hovering.OnLeaveHover();
            IsHovering = false;
        }
    }
}