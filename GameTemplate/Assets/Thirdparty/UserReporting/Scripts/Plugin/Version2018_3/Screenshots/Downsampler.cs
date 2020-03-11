using System;

namespace Unity.Screenshots
{
    public static class Downsampler
    {
        #region Static Methods

        public static byte[] Downsample(byte[] dataRgba, int stride, int maximumWidth, int maximumHeight, out int downsampledStride)
        {
            // Preconditions
            if (stride == 0)
            {
                throw new ArgumentException("The stride must be greater than 0.");
            }
            if (stride % 4 != 0)
            {
                throw new ArgumentException("The stride must be evenly divisible by 4.");
            }
            if (dataRgba == null)
            {
                throw new ArgumentNullException("dataRgba");
            }
            if (dataRgba.Length == 0)
            {
                throw new ArgumentException("The data length must be greater than 0.");
            }
            if (dataRgba.Length % 4 != 0)
            {
                throw new ArgumentException("The data must be evenly divisible by 4.");
            }
            if (dataRgba.Length % stride != 0)
            {
                throw new ArgumentException("The data must be evenly divisible by the stride.");
            }

            // Implementation
            int width = stride / 4;
            int height = dataRgba.Length / stride;
            float ratioX = maximumWidth / (float) width;
            float ratioY = maximumHeight / (float) height;
            float ratio = Math.Min(ratioX, ratioY);
            if (ratio < 1)
            {
                int downsampledWidth = (int) Math.Round(width * ratio);
                int downsampledHeight = (int) Math.Round(height * ratio);
                float[] downsampledData = new float[downsampledWidth * downsampledHeight * 4];
                float sampleWidth = width / (float) downsampledWidth;
                float sampleHeight = height / (float) downsampledHeight;
                int kernelWidth = (int) Math.Floor(sampleWidth);
                int kernelHeight = (int) Math.Floor(sampleHeight);
                int kernelSize = kernelWidth * kernelHeight;
                for (int y = 0; y < downsampledHeight; y++)
                {
                    for (int x = 0; x < downsampledWidth; x++)
                    {
                        int destinationIndex = y * downsampledWidth * 4 + x * 4;
                        int sampleLowerX = (int) Math.Floor(x * sampleWidth);
                        int sampleLowerY = (int) Math.Floor(y * sampleHeight);
                        int sampleUpperX = sampleLowerX + kernelWidth;
                        int sampleUpperY = sampleLowerY + kernelHeight;
                        for (int sampleY = sampleLowerY; sampleY < sampleUpperY; sampleY++)
                        {
                            if (sampleY >= height)
                            {
                                continue;
                            }
                            for (int sampleX = sampleLowerX; sampleX < sampleUpperX; sampleX++)
                            {
                                if (sampleX >= width)
                                {
                                    continue;
                                }
                                int sourceIndex = sampleY * width * 4 + sampleX * 4;
                                downsampledData[destinationIndex] += dataRgba[sourceIndex];
                                downsampledData[destinationIndex + 1] += dataRgba[sourceIndex + 1];
                                downsampledData[destinationIndex + 2] += dataRgba[sourceIndex + 2];
                                downsampledData[destinationIndex + 3] += dataRgba[sourceIndex + 3];
                            }
                        }
                        downsampledData[destinationIndex] /= kernelSize;
                        downsampledData[destinationIndex + 1] /= kernelSize;
                        downsampledData[destinationIndex + 2] /= kernelSize;
                        downsampledData[destinationIndex + 3] /= kernelSize;
                    }
                }
                byte[] flippedData = new byte[downsampledWidth * downsampledHeight * 4];
                for (int y = 0; y < downsampledHeight; y++)
                {
                    for (int x = 0; x < downsampledWidth; x++)
                    {
                        int sourceIndex = (downsampledHeight - 1 - y) * downsampledWidth * 4 + x * 4;
                        int destinationIndex = y * downsampledWidth * 4 + x * 4;
                        flippedData[destinationIndex] += (byte) downsampledData[sourceIndex];
                        flippedData[destinationIndex + 1] += (byte) downsampledData[sourceIndex + 1];
                        flippedData[destinationIndex + 2] += (byte) downsampledData[sourceIndex + 2];
                        flippedData[destinationIndex + 3] += (byte) downsampledData[sourceIndex + 3];
                    }
                }
                downsampledStride = downsampledWidth * 4;
                return flippedData;
            }
            else
            {
                byte[] flippedData = new byte[dataRgba.Length];
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        int sourceIndex = (height - 1 - y) * width * 4 + x * 4;
                        int destinationIndex = y * width * 4 + x * 4;
                        flippedData[destinationIndex] += (byte) dataRgba[sourceIndex];
                        flippedData[destinationIndex + 1] += (byte) dataRgba[sourceIndex + 1];
                        flippedData[destinationIndex + 2] += (byte) dataRgba[sourceIndex + 2];
                        flippedData[destinationIndex + 3] += (byte) dataRgba[sourceIndex + 3];
                    }
                }
                downsampledStride = width * 4;
                return flippedData;
            }
        }

        #endregion
    }
}