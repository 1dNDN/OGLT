using OpenTK.Mathematics;

namespace OGLT;

public class Camera
{
    private float fov = MathHelper.PiOver2;
    private Vector3 front = -Vector3.UnitZ;

    private float pitch;

    private float yaw = -MathHelper.PiOver2;

    public Camera(Vector3 position, float aspectRatio)
    {
        Position = position;
        AspectRatio = aspectRatio;
    }

    public Vector3 Position { get; set; }

    public float AspectRatio { private get; set; }

    public Vector3 Front => front;

    public Vector3 Up { get; private set; } = Vector3.UnitY;

    public Vector3 Right { get; private set; } = Vector3.UnitX;

    public float Pitch
    {
        get => MathHelper.RadiansToDegrees(pitch);
        set {
            float angle = MathHelper.Clamp(value, -89f, 89f);
            pitch = MathHelper.DegreesToRadians(angle);
            UpdateVectors();
        }
    }

    public float Yaw
    {
        get => MathHelper.RadiansToDegrees(yaw);
        set {
            yaw = MathHelper.DegreesToRadians(value);
            UpdateVectors();
        }
    }

    public float Fov
    {
        get => MathHelper.RadiansToDegrees(fov);
        set {
            float angle = MathHelper.Clamp(value, 1f, 90f);
            fov = MathHelper.DegreesToRadians(angle);
        }
    }

    public Matrix4 GetViewMatrix()
    {
        return Matrix4.LookAt(Position, Position + front, Up);
    }

    public Matrix4 GetProjectionMatrix()
    {
        return Matrix4.CreatePerspectiveFieldOfView(fov, AspectRatio, 0.01f, 100f);
    }

    private void UpdateVectors()
    {
        front.X = MathF.Cos(pitch) * MathF.Cos(yaw);
        front.Y = MathF.Sin(pitch);
        front.Z = MathF.Cos(pitch) * MathF.Sin(yaw);

        front = Vector3.Normalize(front);

        Right = Vector3.Normalize(Vector3.Cross(front, Vector3.UnitY));
        Up = Vector3.Normalize(Vector3.Cross(Right, front));
    }
}