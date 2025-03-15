using Godot;

public partial class Player : RigidBody3D
{
    private Camera3D camera;
    private Node3D head;
    private float acceleration = 5f; // Higher acceleration for better control
    private float maxSpeed = 5f; // Defines movement speed
    private float friction = 2f; // Helps slow down naturally
    private Vector3 velocity = Vector3.Zero;
    private float sensitivity = 0.004f;
    private float goalHeadRot;

    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;
        head = GetNode<Node3D>("Head");
        camera = head.GetNode<Camera3D>("Camera3D");
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion eventMouseMotion)
        {
            goalHeadRot += -eventMouseMotion.Relative.X*sensitivity;
            head.RotateX(eventMouseMotion.Relative.Y*sensitivity);
            Vector3 headRot = head.Rotation;
            headRot.X = Mathf.Clamp(headRot.X, Mathf.DegToRad(-80f),Mathf.DegToRad(80f));
            head.Rotation = headRot;
        }
    }

    public override void _IntegrateForces(PhysicsDirectBodyState3D state3D)
    {
        Vector2 moveInput = Input.GetVector("Right", "Left", "Backward", "Forward");
        Vector3 direction = (Transform.Basis * new Vector3(moveInput.X, 0, moveInput.Y)).Normalized();

        if (moveInput.Y < 0) // Walking backward slows movement
        {
            direction *= 0.5f;
        }

        // Apply acceleration
        velocity = velocity.Lerp(direction * maxSpeed, acceleration * state3D.Step);

        // Apply friction (slows down when not moving)
        velocity = velocity.Lerp(Vector3.Zero, friction * state3D.Step);

        // Set the new velocity
        state3D.LinearVelocity = velocity;

        Basis newBasis = new Basis(Vector3.Up, goalHeadRot);
        state3D.Transform = new Transform3D(newBasis, state3D.Transform.Origin);
    }
}
