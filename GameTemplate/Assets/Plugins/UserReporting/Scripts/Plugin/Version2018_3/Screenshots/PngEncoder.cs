using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;

namespace Unity.Screenshots
{
    /// <summary>
    /// Provides PNG encoding.
    /// </summary>
    /// <remarks>This is a minimal implementation of a PNG encoder with no scanline filtering or additional features.</remarks>
    public static class PngEncoder
    {
        #region Nested Types

        public class Crc32
        {
            #region Static Fields

            private static UInt32 generator = 0xEDB88320;

            #endregion

            #region Constructors

            public Crc32()
            {
                this.checksumTable = Enumerable.Range(0, 256)
                    .Select(i =>
                    {
                        uint tableEntry = (uint)i;
                        for (int j = 0; j < 8; j++)
                        {
                            tableEntry = (tableEntry & 1) != 0 ? Crc32.generator ^ (tableEntry >> 1) : tableEntry >> 1;
                        }

                        return tableEntry;
                    })
                    .ToArray();
            }

            #endregion

            #region Fields

            private readonly UInt32[] checksumTable;

            #endregion

            #region Methods

            public UInt32 Calculate<T>(IEnumerable<T> byteStream)
            {
                return ~byteStream.Aggregate(0xFFFFFFFF, (checksumRegister, currentByte) => this.checksumTable[(checksumRegister & 0xFF) ^ Convert.ToByte(currentByte)] ^ (checksumRegister >> 8));
            }

            #endregion
        }

        #endregion

        #region Static Constructors

        static PngEncoder()
        {
            PngEncoder.crc32 = new Crc32();
        }

        #endregion

        #region Static Fields

        private static Crc32 crc32;

        #endregion

        #region Static Methods

        private static uint Adler32(byte[] bytes)
        {
            const int mod = 65521;
            uint a = 1, b = 0;
            foreach (byte byteValue in bytes)
            {
                a = (a + byteValue) % mod;
                b = (b + a) % mod;
            }

            return (b << 16) | a;
        }

        private static void AppendByte(this byte[] data, ref int position, byte value)
        {
            data[position] = value;
            position++;
        }

        private static void AppendBytes(this byte[] data, ref int position, byte[] value)
        {
            foreach (byte valueByte in value)
            {
                data.AppendByte(ref position, valueByte);
            }
        }

        private static void AppendChunk(this byte[] data, ref int position, string chunkType, byte[] chunkData)
        {
            byte[] chunkTypeBytes = PngEncoder.GetChunkTypeBytes(chunkType);
            if (chunkTypeBytes != null)
            {
                data.AppendInt(ref position, chunkData.Length);
                data.AppendBytes(ref position, chunkTypeBytes);
                data.AppendBytes(ref position, chunkData);
                data.AppendUInt(ref position, PngEncoder.GetChunkCrc(chunkTypeBytes, chunkData));
            }
        }

        private static void AppendInt(this byte[] data, ref int position, int value)
        {
            byte[] valueBytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(valueBytes);
            }

            data.AppendBytes(ref position, valueBytes);
        }

        private static void AppendUInt(this byte[] data, ref int position, uint value)
        {
            byte[] valueBytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(valueBytes);
            }

            data.AppendBytes(ref position, valueBytes);
        }

        private static byte[] Compress(byte[] bytes)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                using (DeflateStream gZipStream = new DeflateStream(outStream, CompressionMode.Compress))
                using (MemoryStream mStream = new MemoryStream(bytes))
                {
                    mStream.WriteTo(gZipStream);
                }

                byte[] compressedBytes = outStream.ToArray();
                return compressedBytes;
            }
        }

        public static byte[] Encode(byte[] dataRgba, int stride)
        {
            // Preconditions
            if (dataRgba == null)
            {
                throw new ArgumentNullException("dataRgba");
            }

            if (dataRgba.Length == 0)
            {
                throw new ArgumentException("The data length must be greater than 0.");
            }

            if (stride == 0)
            {
                throw new ArgumentException("The stride must be greater than 0.");
            }

            if (stride % 4 != 0)
            {
                throw new ArgumentException("The stride must be evenly divisible by 4.");
            }

            if (dataRgba.Length % 4 != 0)
            {
                throw new ArgumentException("The data must be evenly divisible by 4.");
            }

            if (dataRgba.Length % stride != 0)
            {
                throw new ArgumentException("The data must be evenly divisible by the stride.");
            }

            // Dimensions
            int pixels = dataRgba.Length / 4;
            int width = stride / 4;
            int height = pixels / width;

            // IHDR Chunk
            byte[] ihdrData = new byte[13];
            int ihdrPosition = 0;
            ihdrData.AppendInt(ref ihdrPosition, width);
            ihdrData.AppendInt(ref ihdrPosition, height);
            ihdrData.AppendByte(ref ihdrPosition, 8); // Bit depth
            ihdrData.AppendByte(ref ihdrPosition, 6); // Color type (color + alpha)
            ihdrData.AppendByte(ref ihdrPosition, 0); // Compression method (always 0)
            ihdrData.AppendByte(ref ihdrPosition, 0); // Filter method (always 0)
            ihdrData.AppendByte(ref ihdrPosition, 0); // Interlace method (no interlacing)

            // IDAT Chunk
            byte[] scanlineData = new byte[dataRgba.Length + height];
            int scanlineDataPosition = 0;
            int scanlinePosition = 0;
            for (int i = 0; i < dataRgba.Length; i++)
            {
                if (scanlinePosition >= stride)
                {
                    scanlinePosition = 0;
                }

                if (scanlinePosition == 0)
                {
                    scanlineData.AppendByte(ref scanlineDataPosition, 0);
                }

                scanlineData.AppendByte(ref scanlineDataPosition, dataRgba[i]);
                scanlinePosition++;
            }

            byte[] compressedScanlineData = PngEncoder.Compress(scanlineData);
            byte[] idatData = new byte[1 + 1 + compressedScanlineData.Length + 4];
            int idatPosition = 0;
            idatData.AppendByte(ref idatPosition, 0x78); // Zlib header
            idatData.AppendByte(ref idatPosition, 0x9C); // Zlib header
            idatData.AppendBytes(ref idatPosition, compressedScanlineData); // Data
            idatData.AppendUInt(ref idatPosition, PngEncoder.Adler32(scanlineData)); // Adler32 checksum

            // Png
            byte[] png = new byte[8 + ihdrData.Length + 12 + idatData.Length + 12 + 12];

            // Position
            int position = 0;

            // Signature
            png.AppendByte(ref position, 0x89); // High bit set
            png.AppendByte(ref position, 0x50); // P
            png.AppendByte(ref position, 0x4E); // N
            png.AppendByte(ref position, 0x47); // G
            png.AppendByte(ref position, 0x0D); // DOS line ending
            png.AppendByte(ref position, 0x0A); // DOS line ending
            png.AppendByte(ref position, 0x1A); // DOS end of file
            png.AppendByte(ref position, 0x0A); // Unix line ending

            // Assemble
            png.AppendChunk(ref position, "IHDR", ihdrData);
            png.AppendChunk(ref position, "IDAT", idatData);
            png.AppendChunk(ref position, "IEND", new byte[0]);

            // Return
            return png;
        }

        public static void EncodeAsync(byte[] dataRgba, int stride, Action<Exception, byte[]> callback)
        {
            ThreadPool.QueueUserWorkItem((state) =>
            {
                try
                {
                    byte[] png = PngEncoder.Encode(dataRgba, stride);
                    callback(null, png);
                }
                catch (Exception ex)
                {
                    callback(ex, null);
                    throw;
                }
            }, null);
        }

        private static uint GetChunkCrc(byte[] chunkTypeBytes, byte[] chunkData)
        {
            byte[] combinedBytes = new byte[chunkTypeBytes.Length + chunkData.Length];
            Array.Copy(chunkTypeBytes, 0, combinedBytes, 0, chunkTypeBytes.Length);
            Array.Copy(chunkData, 0, combinedBytes, 4, chunkData.Length);
            return PngEncoder.crc32.Calculate(combinedBytes);
        }

        private static byte[] GetChunkTypeBytes(string value)
        {
            char[] characters = value.ToCharArray();
            if (characters.Length < 4)
            {
                return null;
            }

            byte[] type = new byte[4];
            for (int i = 0; i < type.Length; i++)
            {
                type[i] = (byte)characters[i];
            }

            return type;
        }

        #endregion
    }
}
