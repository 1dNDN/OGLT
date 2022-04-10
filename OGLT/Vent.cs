using System.Diagnostics;

using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OGLT;

public class Vent : GameWindow
{
    public Vent(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings) { }
    
    private Shader shader = null!;
    private int vertexBufferObject;
    private int vertexArrayObject;
    private int elementBufferObject;
    
    private Shader shader2 = null!;
    private int vertexBufferObject2;
    private int vertexArrayObject2;
    private int elementBufferObject2;

    private readonly float[] vertices = {
        0.0f, 0.5f, 0.0f,
        0.0f, -0.5f, 0.0f,
        0.5f, 0.0f, 0.0f
    };

    private readonly int[] indices = {
        0, 1, 2
    };

    private readonly float[] vertices2 = {
        -0.5f, -0.5f, 0.0f,
        -0.5f, 0.5f, 0.0f,
        0.0f, 0.0f, 0.0f
    };

    private readonly int[] indices2 = {
        0, 1, 2
    };

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        
        GL.Clear(ClearBufferMask.ColorBufferBit);
        
        shader.Use();
        
        GL.BindVertexArray(vertexArrayObject);
        GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
        
        shader2.Use();

        GL.BindVertexArray(vertexArrayObject2);
        GL.DrawElements(PrimitiveType.Triangles, indices2.Length, DrawElementsType.UnsignedInt, 0);

        GL.BindVertexArray(0);
        SwapBuffers();
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        Stopwatch stopwatch = new();
        stopwatch.Start();
        
        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
        
        vertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

        vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(vertexArrayObject);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        elementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
        
        
        vertexBufferObject2 = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject2);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices2.Length * sizeof(float), vertices2, BufferUsageHint.StaticDraw);

        vertexArrayObject2 = GL.GenVertexArray();
        GL.BindVertexArray(vertexArrayObject2);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        elementBufferObject2 = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject2);
        GL.BufferData(BufferTarget.ElementArrayBuffer, indices2.Length * sizeof(uint), indices2, BufferUsageHint.StaticDraw);
        
        GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
        
        shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
        shader.Use();
        
        shader2 = new Shader("Shaders/shader.vert", "Shaders/shader2.frag");
        shader2.Use();

        stopwatch.Stop();
        Console.WriteLine(stopwatch.ElapsedMilliseconds);
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
        
        GL.DeleteBuffer(vertexBufferObject2);
        GL.DeleteVertexArray(vertexArrayObject2);
        GL.DeleteBuffer(elementBufferObject2);

        GL.DeleteProgram(shader.Handle);

        base.OnUnload();
    }
}
