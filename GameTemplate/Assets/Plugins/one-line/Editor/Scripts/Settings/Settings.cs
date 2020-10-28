using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace OneLine.Settings {
    public class Settings : ScriptableObject, ISettings {
        public ISettings Defaults { get; private set; }

        private LocalSettingsLayer local = new LocalSettingsLayer();
        public ISettings Local { get { return local; } }

        [SerializeField]
        private GlobalSettingsLayer layer = new GlobalSettingsLayer();
        public ISettings Layer { get { return layer; } }

        private void OnEnable() {
            Defaults = new DefaultSettingsLayer();

            SettingsMenu.ApplyDirectivesInOrderToCurrentSettings(this);
        }

        public TernaryBoolean Enabled { get { return GetBoolean(x => x.Enabled); } }
        public TernaryBoolean DrawVerticalSeparator { get { return GetBoolean(x => x.DrawVerticalSeparator); } }
        public TernaryBoolean DrawHorizontalSeparator { get { return GetBoolean(x => x.DrawHorizontalSeparator); } }
        public TernaryBoolean Expandable { get { return GetBoolean(x => x.Expandable); } }
        public TernaryBoolean CustomDrawer { get { return GetBoolean(x => x.CustomDrawer); } }
        public TernaryBoolean CullingOptimization { get { return GetBoolean(x => x.CullingOptimization); } }
        public TernaryBoolean CacheOptimization { get { return GetBoolean(x => x.CacheOptimization); } }

        private TernaryBoolean GetBoolean(Func<ISettings, TernaryBoolean> get) {
                var result = get(Defaults);
                if (get(layer).HasValue) {
                    result = get(layer);
                }
                if (get(Local).HasValue) {
                    result = get(Local);
                }
                return result;
        }

        public void SaveAndApply() {
            local.Save();
            EditorUtility.SetDirty(this);
            SettingsMenu.ApplyDirectivesInOrderToCurrentSettings(this);
        }


    }
}