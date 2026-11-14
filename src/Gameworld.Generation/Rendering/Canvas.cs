using System.Buffers.Binary;
using System.IO.Compression;

namespace Gameworld.Generation.Rendering;

public readonly record struct Rgb(byte R, byte G, byte B)
{
    public Rgb Scale(double factor) => new(Clamp(R * factor), Clamp(G * factor), Clamp(B * factor));

    public static Rgb Lerp(Rgb a, Rgb b, double t)
    {
        t = Math.Clamp(t, 0, 1);
        return new Rgb(Clamp(a.R + (b.R - a.R) * t), Clamp(a.G + (b.G - a.G) * t), Clamp(a.B + (b.B - a.B) * t));
    }

    private static byte Clamp(double v) => (byte)Math.Clamp(Math.Round(v), 0, 255);
}

/// <summary>A small RGB raster with the few drawing primitives the map needs.</summary>
public sealed class Canvas
{
    public Canvas(int width, int height)
    {
        Width = width;
        Height = height;
        Pixels = new byte[width * height * 3];
    }

    public int Width { get; }
    public int Height { get; }
    /// <summary>Row by row, 3 bytes per pixel.</summary>
    public byte[] Pixels { get; }

    public void Set(int x, int y, Rgb c)
    {
        if ((uint)x >= (uint)Width || (uint)y >= (uint)Height) return;
        var o = (y * Width + x) * 3;
        Pixels[o] = c.R;
        Pixels[o + 1] = c.G;
        Pixels[o + 2] = c.B;
    }

    public Rgb Get(int x, int y)
    {
        var o = (y * Width + x) * 3;
        return new Rgb(Pixels[o], Pixels[o + 1], Pixels[o + 2]);
    }

    public void Blend(int x, int y, Rgb c, double alpha)
    {
        if ((uint)x >= (uint)Width || (uint)y >= (uint)Height || alpha <= 0) return;
        if (alpha > 1) alpha = 1;
        var o = (y * Width + x) * 3;
        Pixels[o] = (byte)(Pixels[o] + (c.R - Pixels[o]) * alpha);
        Pixels[o + 1] = (byte)(Pixels[o + 1] + (c.G - Pixels[o + 1]) * alpha);
        Pixels[o + 2] = (byte)(Pixels[o + 2] + (c.B - Pixels[o + 2]) * alpha);
    }

    /// <summary>An anti-aliased line with round caps.</summary>
    public void Line(double x0, double y0, double x1, double y1, double width, Rgb c, double alpha = 1)
    {
        var half = Math.Max(0.3, width / 2);
        var minX = (int)Math.Floor(Math.Min(x0, x1) - half - 1);
        var maxX = (int)Math.Ceiling(Math.Max(x0, x1) + half + 1);
        var minY = (int)Math.Floor(Math.Min(y0, y1) - half - 1);
        var maxY = (int)Math.Ceiling(Math.Max(y0, y1) + half + 1);
        if (maxX < 0 || maxY < 0 || minX >= Width || minY >= Height) return;
        minX = Math.Max(minX, 0);
        minY = Math.Max(minY, 0);
        maxX = Math.Min(maxX, Width - 1);
        maxY = Math.Min(maxY, Height - 1);
        var dx = x1 - x0;
        var dy = y1 - y0;
        var len2 = dx * dx + dy * dy;
        for (var y = minY; y <= maxY; y++)
            for (var x = minX; x <= maxX; x++)
            {
                var px = x + 0.5 - x0;
                var py = y + 0.5 - y0;
                var t = len2 > 0 ? Math.Clamp((px * dx + py * dy) / len2, 0, 1) : 0;
                var ex = px - t * dx;
                var ey = py - t * dy;
                var d = Math.Sqrt(ex * ex + ey * ey);
                var cover = Math.Clamp(half + 0.5 - d, 0, 1);
                if (cover > 0) Blend(x, y, c, cover * alpha);
            }
    }

    public void Disc(double cx, double cy, double radius, Rgb c, double alpha = 1) =>
        Line(cx, cy, cx, cy, radius * 2, c, alpha);

    public void Diamond(double cx, double cy, double radius, Rgb c)
    {
        for (var y = (int)Math.Floor(cy - radius - 1); y <= (int)Math.Ceiling(cy + radius + 1); y++)
            for (var x = (int)Math.Floor(cx - radius - 1); x <= (int)Math.Ceiling(cx + radius + 1); x++)
            {
                var d = Math.Abs(x + 0.5 - cx) + Math.Abs(y + 0.5 - cy);
                var cover = Math.Clamp(radius + 0.5 - d, 0, 1);
                if (cover > 0) Blend(x, y, c, cover);
            }
    }

    /// <summary>Fills a polygon with the even-odd rule, sampled at pixel centres.</summary>
    public void FillPolygon(IReadOnlyList<(double X, double Y)> points, Rgb c, double alpha = 1)
    {
        if (points.Count < 3) return;
        var minY = Math.Max(0, (int)Math.Floor(points.Min(p => p.Y)));
        var maxY = Math.Min(Height - 1, (int)Math.Ceiling(points.Max(p => p.Y)));
        var crossings = new List<double>();
        for (var y = minY; y <= maxY; y++)
        {
            var sy = y + 0.5;
            crossings.Clear();
            for (int i = 0, j = points.Count - 1; i < points.Count; j = i++)
            {
                var (xi, yi) = points[i];
                var (xj, yj) = points[j];
                if ((yi > sy) != (yj > sy))
                    crossings.Add(xi + (sy - yi) / (yj - yi) * (xj - xi));
            }
            crossings.Sort();
            for (var k = 0; k + 1 < crossings.Count; k += 2)
            {
                var xa = Math.Max(0, (int)Math.Ceiling(crossings[k] - 0.5));
                var xb = Math.Min(Width - 1, (int)Math.Floor(crossings[k + 1] - 0.5));
                for (var x = xa; x <= xb; x++) Blend(x, y, c, alpha);
            }
        }
    }

    /// <summary>Encodes the raster as an 8-bit RGB PNG.</summary>
    public byte[] ToPng()
    {
        using var output = new MemoryStream();
        output.Write([0x89, 0x50, 0x4e, 0x47, 0x0d, 0x0a, 0x1a, 0x0a]);

        Span<byte> header = stackalloc byte[13];
        BinaryPrimitives.WriteInt32BigEndian(header, Width);
        BinaryPrimitives.WriteInt32BigEndian(header[4..], Height);
        header[8] = 8;  // bit depth
        header[9] = 2;  // colour type: RGB
        header[10] = 0; // compression
        header[11] = 0; // filter
        header[12] = 0; // interlace
        WriteChunk(output, "IHDR", header);

        using (var data = new MemoryStream())
        {
            using (var zlib = new ZLibStream(data, CompressionLevel.Fastest, leaveOpen: true))
            {
                var stride = Width * 3;
                for (var y = 0; y < Height; y++)
                {
                    zlib.WriteByte(0); // filter: none
                    zlib.Write(Pixels, y * stride, stride);
                }
            }
            WriteChunk(output, "IDAT", data.GetBuffer().AsSpan(0, (int)data.Length));
        }

        WriteChunk(output, "IEND", ReadOnlySpan<byte>.Empty);
        return output.ToArray();
    }

    private static readonly uint[] CrcTable = BuildCrcTable();

    private static uint[] BuildCrcTable()
    {
        var table = new uint[256];
        for (uint n = 0; n < 256; n++)
        {
            var c = n;
            for (var k = 0; k < 8; k++) c = (c & 1) != 0 ? 0xedb88320u ^ (c >> 1) : c >> 1;
            table[n] = c;
        }
        return table;
    }

    private static void WriteChunk(Stream output, string type, ReadOnlySpan<byte> data)
    {
        Span<byte> buffer = stackalloc byte[4];
        BinaryPrimitives.WriteInt32BigEndian(buffer, data.Length);
        output.Write(buffer);
        Span<byte> name = stackalloc byte[4];
        for (var i = 0; i < 4; i++) name[i] = (byte)type[i];
        output.Write(name);
        output.Write(data);
        var crc = 0xffffffffu;
        foreach (var b in name) crc = CrcTable[(crc ^ b) & 0xff] ^ (crc >> 8);
        foreach (var b in data) crc = CrcTable[(crc ^ b) & 0xff] ^ (crc >> 8);
        BinaryPrimitives.WriteUInt32BigEndian(buffer, crc ^ 0xffffffffu);
        output.Write(buffer);
    }
}
