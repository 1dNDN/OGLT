using System.Diagnostics;

using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OGLT;

public class Vent : GameWindow
{
    public Vent(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
    {
    }

    private Shader shader = null!;
    private Texture texture;
    private int vertexBufferObject;
    private int vertexArrayObject;
    private int elementBufferObject;
        //pos3, color3, tex2
    private readonly float[] vertices = {
         0.5f,  0.5f, 0.0f,  1.0f, 0.0f, 0.0f,  1.0f, 1.0f, //top right
         0.5f, -0.5f, 0.0f,  0.0f, 1.0f, 0.0f,  1.0f, 0.0f, //bottom right
        -0.5f, -0.5f, 0.0f,  0.0f, 0.0f, 1.0f,  0.0f, 0.0f, //bottom left
        -0.5f,  0.5f, 0.0f,  1.0f, 1.0f, 0.0f,  0.0f, 1.0f  //top left
    };

    private readonly int[] indices = {
        0, 1, 2,
        0, 2, 3
    };

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        Stopwatch stopwatch = new();
        stopwatch.Start();
        
        base.OnRenderFrame(args);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        GL.BindVertexArray(vertexArrayObject);

        Matrix4 rotationY = Matrix4.CreateRotationY(DateTime.Now.Millisecond / 200.0f);
        Matrix4 rotationX = Matrix4.CreateRotationX(DateTime.Now.Millisecond / 200.0f + 200);
        //Matrix4 model = rotationY * rotationX;
        Matrix4 model = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(-55.0f));
        
        Matrix4 view = Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f);
        Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), Size.X / (float)Size.Y, 0.1f, 100.0f);

        texture.Use(TextureUnit.Texture0);
        shader.Use();
        
        shader.SetMatrix4("model", model);
        shader.SetMatrix4("view", view);
        shader.SetMatrix4("projection", projection);
        
        GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
        
        SwapBuffers();

        stopwatch.Stop();
        Console.WriteLine(stopwatch.ElapsedMilliseconds);
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
        
        vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(vertexArrayObject);
        
        vertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
        
        elementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
        
        shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
        shader.Use();
        
        //vertex pos
        int vertexLocaton = shader.GetAttribLocation("aPosition");
        GL.VertexAttribPointer(vertexLocaton, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
        GL.EnableVertexAttribArray(vertexLocaton);
        
        //color
        int colorLocaton = shader.GetAttribLocation("aColor");
        GL.VertexAttribPointer(colorLocaton, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(colorLocaton);
        
        //texture
        int texLocaton = shader.GetAttribLocation("aTexCoord");
        GL.VertexAttribPointer(texLocaton, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));
        GL.EnableVertexAttribArray(texLocaton);
        
        texture = Texture.LoadFromFile("Textures/container.jpg");
        texture.Use(TextureUnit.Texture0);
        
        GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        if (KeyboardState.IsKeyDown(Keys.Escape))
            Close();

        base.OnUpdateFrame(args);
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);

        GL.Viewport(0, 0, Size.X, Size.Y);
    }

    protected override void OnUnload()
    {
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindVertexArray(0);
        GL.UseProgram(0);

        GL.DeleteBuffer(vertexBufferObject);
        GL.DeleteVertexArray(vertexArrayObject);
        GL.DeleteBuffer(elementBufferObject);

        GL.DeleteProgram(shader.Handle);

        base.OnUnload();
    }
}
