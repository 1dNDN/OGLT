﻿using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace OGLT;

public class Shader
{
    public readonly int Handle;

    private readonly Dictionary<string, int> uniformLocations;

    public Shader(string vertPath, string fragPath)
    {
        string shaderSource = File.ReadAllText(vertPath);

        int vertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexShader, shaderSource);
        CompileShader(vertexShader);

        shaderSource = File.ReadAllText(fragPath);
        int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentShader, shaderSource);
        CompileShader(fragmentShader);

        Handle = GL.CreateProgram();
        GL.AttachShader(Handle, vertexShader);
        GL.AttachShader(Handle, fragmentShader);
        LinkProgram(Handle);

        GL.DetachShader(Handle, vertexShader);
        GL.DetachShader(Handle, fragmentShader);
        GL.DeleteShader(fragmentShader);
        GL.DeleteShader(vertexShader);

        GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out int numberOfUniforms);
        uniformLocations = new Dictionary<string, int>();

        for (int i = 0; i < numberOfUniforms; i++)
        {
            string? key = GL.GetActiveUniform(Handle, i, out _, out _);
            int location = GL.GetUniformLocation(Handle, key);
            uniformLocations.Add(key, location);
        }
    }

    private static void CompileShader(int shader)
    {
        GL.CompileShader(shader);
        GL.GetShader(shader, ShaderParameter.CompileStatus, out int code);
        if (code != (int)All.True)
            throw new Exception($"Error occurred whilst compiling Shader({shader}).\n\n{GL.GetShaderInfoLog(shader)}");
    }

    private static void LinkProgram(int program)
    {
        GL.LinkProgram(program);
        GL.GetProgram(program, GetProgramParameterName.LinkStatus, out int code);

        if (code != (int)All.True)
            throw new Exception($"Error occurred whilst linking Program({program})");
    }

    public void Use()
    {
        GL.UseProgram(Handle);
    }

    public int GetAttribLocation(string attribName)
    {
        return GL.GetAttribLocation(Handle, attribName);
    }

    /// <summary>
    ///     Set a uniform int on this shader.
    /// </summary>
    /// <param name="name">The name of the uniform</param>
    /// <param name="data">The data to set</param>
    public void SetInt(string name, int data)
    {
        GL.UseProgram(Handle);
        GL.Uniform1(uniformLocations[name], data);
    }

    /// <summary>
    ///     Set a uniform float on this shader.
    /// </summary>
    /// <param name="name">The name of the uniform</param>
    /// <param name="data">The data to set</param>
    public void SetFloat(string name, float data)
    {
        GL.UseProgram(Handle);
        GL.Uniform1(uniformLocations[name], data);
    }

    /// <summary>
    ///     Set a uniform Matrix4 on this shader
    /// </summary>
    /// <param name="name">The name of the uniform</param>
    /// <param name="data">The data to set</param>
    /// <remarks>
    ///     <para>
    ///         The matrix is transposed before being sent to the shader.
    ///     </para>
    /// </remarks>
    public void SetMatrix4(string name, Matrix4 data)
    {
        GL.UseProgram(Handle);
        GL.UniformMatrix4(uniformLocations[name], true, ref data);
    }

    /// <summary>
    ///     Set a uniform Vector3 on this shader.
    /// </summary>
    /// <param name="name">The name of the uniform</param>
    /// <param name="data">The data to set</param>
    public void SetVector3(string name, Vector3 data)
    {
        GL.UseProgram(Handle);
        GL.Uniform3(uniformLocations[name], data);
    }
}