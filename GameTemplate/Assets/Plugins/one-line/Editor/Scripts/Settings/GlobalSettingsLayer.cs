using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneLine.Settings {
    [Serializable]
    public class GlobalSettingsLayer : ISettings {
        [SerializeField]
        private TernaryBoolean enabled = TernaryBoolean.NULL;
        public TernaryBoolean Enabled { 
            get { return enabled; } 
        }

        [SerializeField]
        private TernaryBoolean drawVerticalSeparator = TernaryBoolean.NULL;
        public TernaryBoolean DrawVerticalSeparator {
            get { return drawVerticalSeparator; }
        }

        [SerializeField]
        private TernaryBoolean drawHorizontalSeparator = TernaryBoolean.NULL;
        public TernaryBoolean DrawHorizontalSeparator {
            get { return drawHorizontalSeparator; }
        }

        [SerializeField]
        private TernaryBoolean expandable = TernaryBoolean.NULL;
        public TernaryBoolean Expandable {
            get { return expandable; }
        }

        [SerializeField]
        private TernaryBoolean customDrawer = TernaryBoolean.NULL;
        public TernaryBoolean CustomDrawer {
            get { return customDrawer; }
        }

        [SerializeField]
        private TernaryBoolean cullingOptimization = TernaryBoolean.NULL;
        public TernaryBoolean CullingOptimization {
            get { return cullingOptimization; }
        }

        [SerializeField]
        private TernaryBoolean cacheOptimization = TernaryBoolean.NULL;
        public TernaryBoolean CacheOptimization {
            get { return cacheOptimization; }
        }
    }
}