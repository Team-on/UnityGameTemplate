namespace PumpEditor
{
    public static class PlatformDefines
    {
        public static bool UnityEditorDefined
        {
            get
            {
#if UNITY_EDITOR
                return true;
#else
                return false;
#endif
            }
        }

        public static bool UnityEditorWinDefined
        {
            get
            {
#if UNITY_EDITOR_WIN
                return true;
#else
                return false;
#endif
            }
        }

        public static bool UnityEditorOsxDefined
        {
            get
            {
#if UNITY_EDITOR_OSX
                return true;
#else
                return false;
#endif
            }
        }

        public static bool UnityEditorLinuxDefined
        {
            get
            {
#if UNITY_EDITOR_LINUX
                return true;
#else
                return false;
#endif
            }
        }

        public static bool UnityStandaloneOsxDefined
        {
            get
            {
#if UNITY_STANDALONE_OSX
                return true;
#else
                return false;
#endif
            }
        }

        public static bool UnityStandaloneWinDefined
        {
            get
            {
#if UNITY_STANDALONE_WIN
                return true;
#else
                return false;
#endif
            }
        }

        public static bool UnityStandaloneLinuxDefined
        {
            get
            {
#if UNITY_STANDALONE_LINUX
                return true;
#else
                return false;
#endif
            }
        }

        public static bool UnityStandaloneDefined
        {
            get
            {
#if UNITY_STANDALONE
                return true;
#else
                return false;
#endif
            }
        }

        public static bool UnityWiiDefined
        {
            get
            {
#if UNITY_WII
                return true;
#else
                return false;
#endif
            }
        }

        public static bool UnityIosDefined
        {
            get
            {
#if UNITY_IOS
                return true;
#else
                return false;
#endif
            }
        }

        public static bool UnityIphoneDefined
        {
            get
            {
#if UNITY_IPHONE
                return true;
#else
                return false;
#endif
            }
        }

        public static bool UnityAndroidDefined
        {
            get
            {
#if UNITY_ANDROID
                return true;
#else
                return false;
#endif
            }
        }

        public static bool UnityPs4Defined
        {
            get
            {
#if UNITY_PS4
                return true;
#else
                return false;
#endif
            }
        }

        public static bool UnityXboxoneDefined
        {
            get
            {
#if UNITY_XBOXONE
                return true;
#else
                return false;
#endif
            }
        }

        public static bool UnityLuminDefined
        {
            get
            {
#if UNITY_LUMIN
                return true;
#else
                return false;
#endif
            }
        }

        public static bool UnityTizenDefined
        {
            get
            {
#if UNITY_TIZEN
                return true;
#else
                return false;
#endif
            }
        }

        public static bool UnityTvosDefined
        {
            get
            {
#if UNITY_TVOS
                return true;
#else
                return false;
#endif
            }
        }

        public static bool UnityWsaDefined
        {
            get
            {
#if UNITY_WSA
                return true;
#else
                return false;
#endif
            }
        }

        public static bool UnityWsa10Defined
        {
            get
            {
#if UNITY_WSA_10_0
                return true;
#else
                return false;
#endif
            }
        }

        public static bool UnityWinrtDefined
        {
            get
            {
#if UNITY_WINRT
                return true;
#else
                return false;
#endif
            }
        }

        public static bool UnityWinrt10Defined
        {
            get
            {
#if UNITY_WINRT_10_0
                return true;
#else
                return false;
#endif
            }
        }

        public static bool UnityWebglDefined
        {
            get
            {
#if UNITY_WEBGL
                return true;
#else
                return false;
#endif
            }
        }

        public static bool UnityFacebookDefined
        {
            get
            {
#if UNITY_FACEBOOK
                return true;
#else
                return false;
#endif
            }
        }

        public static bool UnityAdsDefined
        {
            get
            {
#if UNITY_ADS
                return true;
#else
                return false;
#endif
            }
        }

        public static bool UnityAnalyticsDefined
        {
            get
            {
#if UNITY_ANALYTICS
                return true;
#else
                return false;
#endif
            }
        }

        public static bool UnityAssertionsDefined
        {
            get
            {
#if UNITY_ASSERTIONS
                return true;
#else
                return false;
#endif
            }
        }

        public static bool Unity64Defined
        {
            get
            {
#if UNITY_64
                return true;
#else
                return false;
#endif
            }
        }
    }
}
