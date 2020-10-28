
namespace OneLine.Settings {
    public interface ISettings {
        TernaryBoolean Enabled { get; }
        TernaryBoolean DrawVerticalSeparator { get; }
        TernaryBoolean DrawHorizontalSeparator { get; }
        TernaryBoolean Expandable { get; }
        TernaryBoolean CustomDrawer { get; }

        TernaryBoolean CullingOptimization { get; }
        TernaryBoolean CacheOptimization { get; }
    }
}