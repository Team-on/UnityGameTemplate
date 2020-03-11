using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Screenshots
{
    public class ScreenshotManager
    {
        #region Nested Types

        private class ScreenshotOperation
        {
            #region Properties

            public Action<int, byte[]> Callback { get; set; }

            public byte[] Data { get; set; }

            public int FrameNumber { get; set; }

            public bool IsAwaiting { get; set; }

            public bool IsComplete { get; set; }

            public bool IsInUse { get; set; }

            public int MaximumHeight { get; set; }

            public int MaximumWidth { get; set; }

            public object Source { get; set; }

            #endregion

            #region Methods

            public void Use()
            {
                this.Callback = null;
                this.Data = null;
                this.FrameNumber = 0;
                this.IsAwaiting = true;
                this.IsComplete = false;
                this.IsInUse = true;
                this.MaximumHeight = 0;
                this.MaximumWidth = 0;
                this.Source = null;
            }

            #endregion
        }

        #endregion

        #region Constructors

        public ScreenshotManager()
        {
            this.screenshotRecorder = new ScreenshotRecorder();
            this.screenshotCallbackDelegate = this.ScreenshotCallback;
            this.screenshotOperations = new List<ScreenshotOperation>();
        }

        #endregion

        #region Fields

        private Action<byte[], object> screenshotCallbackDelegate;

        private List<ScreenshotOperation> screenshotOperations;

        private ScreenshotRecorder screenshotRecorder;

        #endregion

        #region Methods

        private ScreenshotOperation GetScreenshotOperation()
        {
            foreach (ScreenshotOperation screenshotOperation in this.screenshotOperations)
            {
                if (!screenshotOperation.IsInUse)
                {
                    screenshotOperation.Use();
                    return screenshotOperation;
                }
            }
            ScreenshotOperation newScreenshotOperation = new ScreenshotOperation();
            newScreenshotOperation.Use();
            this.screenshotOperations.Add(newScreenshotOperation);
            return newScreenshotOperation;
        }

        public void OnEndOfFrame()
        {
            foreach (ScreenshotOperation screenshotOperation in this.screenshotOperations)
            {
                if (screenshotOperation.IsInUse)
                {
                    if (screenshotOperation.IsAwaiting)
                    {
                        screenshotOperation.IsAwaiting = false;
                        if (screenshotOperation.Source == null)
                        {
                            this.screenshotRecorder.Screenshot(screenshotOperation.MaximumWidth, screenshotOperation.MaximumHeight, ScreenshotType.Png, this.screenshotCallbackDelegate, screenshotOperation);
                        }
                        else if (screenshotOperation.Source is Camera)
                        {
                            this.screenshotRecorder.Screenshot(screenshotOperation.Source as Camera, screenshotOperation.MaximumWidth, screenshotOperation.MaximumHeight, ScreenshotType.Png, this.screenshotCallbackDelegate, screenshotOperation);
                        }
                        else if (screenshotOperation.Source is RenderTexture)
                        {
                            this.screenshotRecorder.Screenshot(screenshotOperation.Source as RenderTexture, screenshotOperation.MaximumWidth, screenshotOperation.MaximumHeight, ScreenshotType.Png, this.screenshotCallbackDelegate, screenshotOperation);
                        }
                        else if (screenshotOperation.Source is Texture2D)
                        {
                            this.screenshotRecorder.Screenshot(screenshotOperation.Source as Texture2D, screenshotOperation.MaximumWidth, screenshotOperation.MaximumHeight, ScreenshotType.Png, this.screenshotCallbackDelegate, screenshotOperation);
                        }
                        else
                        {
                            this.ScreenshotCallback(null, screenshotOperation);
                        }
                    }
                    else if (screenshotOperation.IsComplete)
                    {
                        screenshotOperation.IsInUse = false;
                        try
                        {
                            if (screenshotOperation != null && screenshotOperation.Callback != null)
                            {
                                screenshotOperation.Callback(screenshotOperation.FrameNumber, screenshotOperation.Data);
                            }
                        }
                        catch
                        {
                            // Do Nothing
                        }
                    }
                }
            }
        }

        private void ScreenshotCallback(byte[] data, object state)
        {
            ScreenshotOperation screenshotOperation = state as ScreenshotOperation;
            if (screenshotOperation != null)
            {
                screenshotOperation.Data = data;
                screenshotOperation.IsComplete = true;
            }
        }

        public void TakeScreenshot(object source, int frameNumber, int maximumWidth, int maximumHeight, Action<int, byte[]> callback)
        {
            ScreenshotOperation screenshotOperation = this.GetScreenshotOperation();
            screenshotOperation.FrameNumber = frameNumber;
            screenshotOperation.MaximumWidth = maximumWidth;
            screenshotOperation.MaximumHeight = maximumHeight;
            screenshotOperation.Source = source;
            screenshotOperation.Callback = callback;
        }

        #endregion
    }
}