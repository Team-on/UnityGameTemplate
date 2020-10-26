namespace PumpEditor
{
    public static class CodeCompilationDefines
    {
        public static bool Csharp73OrNewerDefined
        {
            get
            {
#if CSHARP_7_3_OR_NEWER
                return true;
#else
                return false;
#endif
            }
        }

        public static bool EnableMonoDefined
        {
            get
            {
#if ENABLE_MONO
                return true;
#else
                return false;
#endif
            }
        }

        public static bool EnableIl2cppDefined
        {
            get
            {
#if ENABLE_IL2CPP
                return true;
#else
                return false;
#endif
            }
        }

        public static bool Net20Defined
        {
            get
            {
#if NET_2_0
                return true;
#else
                return false;
#endif
            }
        }

        public static bool Net20SubsetDefined
        {
            get
            {
#if NET_2_0_SUBSET
                return true;
#else
                return false;
#endif
            }
        }

        public static bool NetLegacyDefined
        {
            get
            {
#if NET_LEGACY
                return true;
#else
                return false;
#endif
            }
        }

        public static bool Net46Defined
        {
            get
            {
#if NET_4_6
                return true;
#else
                return false;
#endif
            }
        }

        public static bool NetStandard20Defined
        {
            get
            {
#if NET_4_6
                return true;
#else
                return false;
#endif
            }
        }

        public static bool EnableWinmdSupportDefined
        {
            get
            {
#if ENABLE_WINMD_SUPPORT
                return true;
#else
                return false;
#endif
            }
        }

        public static bool EnableInputSystemDefined
        {
            get
            {
#if ENABLE_INPUT_SYSTEM
                return true;
#else
                return false;
#endif
            }
        }

        public static bool EnableLegacyInputManagerDefined
        {
            get
            {
#if ENABLE_LEGACY_INPUT_MANAGER
                return true;
#else
                return false;
#endif
            }
        }
    }
}
