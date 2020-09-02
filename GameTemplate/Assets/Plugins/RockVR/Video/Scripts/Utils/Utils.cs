using System;
using System.Linq;
using UnityEngine;
using Polyglot;

namespace RockVR.Video
{
    public class StringUtils
    {
        public static string GetMp4FileName(string rootFolder)
        {
            Vector2Int screenSize = new Vector2Int(Screen.currentResolution.width, Screen.currentResolution.height);
            return ScreenshotTaker.GetUniqueFilePath(screenSize.x, screenSize.y, false, true, Localization.Instance.SelectedLanguage.ToString(), rootFolder, "mp4");
        }

        public static string GetH264FileName(string rootFolder)
        {
            Vector2Int screenSize = new Vector2Int(Screen.currentResolution.width, Screen.currentResolution.height);
            return ScreenshotTaker.GetUniqueFilePath(screenSize.x, screenSize.y, false, true, Localization.Instance.SelectedLanguage.ToString(), rootFolder, "h264");
        }

        public static string GetWavFileName(string rootFolder)
        {
            Vector2Int screenSize = new Vector2Int(Screen.currentResolution.width, Screen.currentResolution.height);
            return ScreenshotTaker.GetUniqueFilePath(screenSize.x, screenSize.y, false, true, Localization.Instance.SelectedLanguage.ToString(), rootFolder, "wav");
        }

        public static string GetPngFileName(string rootFolder)
        {
            Vector2Int screenSize = new Vector2Int(Screen.currentResolution.width, Screen.currentResolution.height);
            return ScreenshotTaker.GetUniqueFilePath(screenSize.x, screenSize.y, false, true, Localization.Instance.SelectedLanguage.ToString(), rootFolder, "png");
        }

        public static string GetJpgFileName(string rootFolder)
        {
            Vector2Int screenSize = new Vector2Int(Screen.currentResolution.width, Screen.currentResolution.height);
            return ScreenshotTaker.GetUniqueFilePath(screenSize.x, screenSize.y, false, true, Localization.Instance.SelectedLanguage.ToString(), rootFolder, "jpg");
        }

        public static bool IsRtmpAddress(string str)
        {
            return (str != null && str.StartsWith("rtmp"));
        }
    }

    public class MathUtils
    {
        public static bool CheckPowerOfTwo(int input)
        {
            return (input & (input - 1)) == 0;
        }
    }
}