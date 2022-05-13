using OpenTK.Graphics.OpenGL;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace OGLT;

public class Texture
{
    public readonly int Handle;

    public Texture(int handle)
    {
        Handle = handle;
    }

    public static Texture LoadFromFile(string path)
    {
        int handle = GL.GenTexture();

        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2D, handle);

        Image<Rgba32> image = Image.Load<Rgba32>(path);
        image.Mutate(x => x.Flip(FlipMode.Vertical));

        List<byte> pixels = new(4 * image.Width * image.Height);

        image.ProcessPixelRows(accessor => {
            Rgba32 transparent = Color.Transparent;

            for (int y = 0; y < accessor.Height; y++)
            {
                Span<Rgba32> pixelRow = accessor.GetRowSpan(y);

                for (int x = 0; x < pixelRow.Length; x++)
                {
                    ref Rgba32 pixel = ref pixelRow[x];
                    if (pixel.A == 0)
                        pixel = transparent;

                    pixels.Add(pixel.R);
                    pixels.Add(pixel.G);
                    pixels.Add(pixel.B);
                    pixels.Add(pixel.A);
                }
            }
        });

        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels.ToArray());

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

        GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

        return new Texture(handle);
    }

    public void Use(TextureUnit unit)
    {
        GL.ActiveTexture(unit);
        GL.BindTexture(TextureTarget.Texture2D, Handle);
    }
}
