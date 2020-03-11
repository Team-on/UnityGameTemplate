using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace Unity.Screenshots
{
    public class ScreenshotRecorder
    {
        #region Nested Types

        private class ScreenshotOperation
        {
            #region Constructors

            public ScreenshotOperation()
            {
                this.ScreenshotCallbackDelegate = this.ScreenshotCallback;
                this.EncodeCallbackDelegate = this.EncodeCallback;
            }

            #endregion

            #region Fields

            public WaitCallback EncodeCallbackDelegate;

            public Action<AsyncGPUReadbackRequest> ScreenshotCallbackDelegate;

            #endregion

            #region Properties

            public Action<byte[], object> Callback { get; set; }

            public int Height { get; set; }

            public int Identifier { get; set; }

            public bool IsInUse { get; set; }

            public int MaximumHeight { get; set; }

            public int MaximumWidth { get; set; }

            public NativeArray<byte> NativeData { get; set; }

            public Texture Source { get; set; }

            public object State { get; set; }

            public ScreenshotType Type { get; set; }

            public int Width { get; set; }

            #endregion

            #region Methods

            private void EncodeCallback(object state)
            {
                byte[] byteData = this.NativeData.ToArray();
                int downsampledStride;
                byteData = Downsampler.Downsample(byteData, this.Width * 4, this.MaximumWidth, this.MaximumHeight, out downsampledStride);
                if (this.Type == ScreenshotType.Png)
                {
                    byteData = PngEncoder.Encode(byteData, downsampledStride);
                }
                if (this.Callback != null)
                {
                    this.Callback(byteData, this.State);
                }
                this.NativeData.Dispose();
                this.IsInUse = false;
            }

            private void SavePngToDisk(byte[] byteData)
            {
                if (!Directory.Exists("Screenshots"))
                {
                    Directory.CreateDirectory("Screenshots");
                }
                File.WriteAllBytes(string.Format("Screenshots/{0}.png", this.Identifier % 60), byteData);
            }

            private void ScreenshotCallback(AsyncGPUReadbackRequest request)
            {
                if (!request.hasError)
                {
                    NativeLeakDetection.Mode = NativeLeakDetectionMode.Disabled;
                    NativeArray<byte> data = request.GetData<byte>();
                    NativeArray<byte> persistentData = new NativeArray<byte>(data, Allocator.Persistent);
                    this.Width = request.width;
                    this.Height = request.height;
                    this.NativeData = persistentData;
                    ThreadPool.QueueUserWorkItem(this.EncodeCallbackDelegate, null);
                }
                else
                {
                    if (this.Callback != null)
                    {
                        this.Callback(null, this.State);
                    }
                }
                if (this.Source != null)
                {
                    UnityEngine.Object.Destroy(this.Source);
                }
            }

            #endregion
        }

        #endregion

        #region Static Fields

        private static int nextIdentifier;

        #endregion

        #region Constructors

        public ScreenshotRecorder()
        {
            this.operationPool = new List<ScreenshotOperation>();
        }

        #endregion

        #region Fields

        private List<ScreenshotOperation> operationPool;

        #endregion

        #region Methods

        private ScreenshotOperation GetOperation()
        {
            foreach (ScreenshotOperation operation in this.operationPool)
            {
                if (!operation.IsInUse)
                {
                    operation.IsInUse = true;
                    return operation;
                }
            }
            ScreenshotOperation newOperation = new ScreenshotOperation();
            newOperation.IsInUse = true;
            this.operationPool.Add(newOperation);
            return newOperation;
        }

        public void Screenshot(int maximumWidth, int maximumHeight, ScreenshotType type, Action<byte[], object> callback, object state)
        {
            Texture2D texture = ScreenCapture.CaptureScreenshotAsTexture();
            this.Screenshot(texture, maximumWidth, maximumHeight, type, callback, state);
        }

        public void Screenshot(Camera source, int maximumWidth, int maximumHeight, ScreenshotType type, Action<byte[], object> callback, object state)
        {
            RenderTexture renderTexture = new RenderTexture(maximumWidth, maximumHeight, 24);
            RenderTexture originalTargetTexture = source.targetTexture;
            source.targetTexture = renderTexture;
            source.Render();
            source.targetTexture = originalTargetTexture;
            this.Screenshot(renderTexture, maximumWidth, maximumHeight, type, callback, state);
        }

        public void Screenshot(RenderTexture source, int maximumWidth, int maximumHeight, ScreenshotType type, Action<byte[], object> callback, object state)
        {
            this.ScreenshotInternal(source, maximumWidth, maximumHeight, type, callback, state);
        }

        public void Screenshot(Texture2D source, int maximumWidth, int maximumHeight, ScreenshotType type, Action<byte[], object> callback, object state)
        {
            this.ScreenshotInternal(source, maximumWidth, maximumHeight, type, callback, state);
        }

        private void ScreenshotInternal(Texture source, int maximumWidth, int maximumHeight, ScreenshotType type, Action<byte[], object> callback, object state)
        {
            ScreenshotOperation operation = this.GetOperation();
            operation.Identifier = ScreenshotRecorder.nextIdentifier++;
            operation.Source = source;
            operation.MaximumWidth = maximumWidth;
            operation.MaximumHeight = maximumHeight;
            operation.Type = type;
            operation.Callback = callback;
            operation.State = state;
            AsyncGPUReadback.Request(source, 0, TextureFormat.RGBA32, operation.ScreenshotCallbackDelegate);
        }

        #endregion
    }
}