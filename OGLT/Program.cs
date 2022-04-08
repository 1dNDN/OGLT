﻿using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

using RustedWarfareClient;

namespace OGLT;

internal static class Program
{
    static void Main()
    {
        Console.WriteLine("Hello World!");
    }

    private static void Init()
    {
        NativeWindowSettings nativeWindowSettings = new()
        {
            Size = new Vector2i(800, 600),
            Title = "RW Client",
            Flags = ContextFlags.ForwardCompatible
        };

        using Vent window = new(GameWindowSettings.Default, nativeWindowSettings);
        window.Run();
    }
}
