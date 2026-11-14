using System.Buffers.Binary;
using System.IO.Compression;
using System.Text;

namespace Gameworld.App;

/// <summary>Writes an image as a PNG file, without any imaging library.</summary>
internal static class PngWriter
{
    /// <param name="rgb">Three bytes per pixel, row by row from the top.</param>
    public static void Write(string path, int width, int height, byte[] rgb)
    {
        if (rgb.Length != width * height * 3)
            throw new ArgumentException("The image needs three bytes per pixel.", nameof(rgb));

        using var file = File.Create(path);
        file.Write([0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A]);

        var header = new byte[13];
        BinaryPrimitives.WriteInt32BigEndian(header, width);
        BinaryPrimitives.WriteInt32BigEndian(header.AsSpan(4), height);
        header[8] = 8; // bits per channel
        header[9] = 2; // colour type: RGB
        WriteChunk(file, "IHDR", header);

        using var pixels = new MemoryStream();
        using (var zlib = new ZLibStream(pixels, CompressionLevel.Optimal, leaveOpen: true))
        {
            for (var row = 0; row < height; row++)
            {
                zlib.WriteByte(0); // no filter
                zlib.Write(rgb, row * width * 3, width * 3);
            }
        }
        WriteChunk(file, "IDAT", pixels.ToArray());
        WriteChunk(file, "IEND", []);
    }

    private static void WriteChunk(Stream file, string type, byte[] data)
    {
        Span<byte> number = stackalloc byte[4];
        var name = Encoding.ASCII.GetBytes(type);

        BinaryPrimitives.WriteInt32BigEndian(number, data.Length);
        file.Write(number);
        file.Write(name);
        file.Write(data);
        BinaryPrimitives.WriteUInt32BigEndian(number, ~Crc(data, Crc(name, 0xFFFFFFFF)));
        file.Write(number);
    }

    private static uint Crc(ReadOnlySpan<byte> data, uint crc)
    {
        foreach (var value in data)
        {
            crc ^= value;
            for (var bit = 0; bit < 8; bit++)
                crc = (crc & 1) != 0 ? (crc >> 1) ^ 0xEDB88320 : crc >> 1;
        }
        return crc;
    }
}
