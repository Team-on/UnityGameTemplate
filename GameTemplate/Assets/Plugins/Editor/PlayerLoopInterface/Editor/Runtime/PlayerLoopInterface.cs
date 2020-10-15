using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;



/// <summary>
/// Unity exposes the PlayerLoop to allow you to insert your own "systems" to be run in similar ways to eg. Update or FixedUpate.
/// The interface for that is a bit hairy, and there are bugs that needs workarounds, so this is a nice interface for interacting with that system.
///
/// In essence, use PlayerLoopInterface.InsertSystemBefore/After to have a callback be executed every frame, before or after some built-in system.
/// The built-in systems live in UnityEngine.Experimental.PlayerLoop, so if you want to insert a system to run just before Update, call:
///
/// PlayerLoopInterface.InsertSystemBefore(typeof(MyType), MyMethod, typeof(UnityEngine.Experimental.PlayerLoop.Update);
/// </summary>
public static class PlayerLoopInterface {

    private static bool hasFetchedSystem;
    private static IntPtr fixedUpdateLoopCondition;

    private static UnityEngine.LowLevel.PlayerLoopSystem defaultSystem;
    internal static UnityEngine.LowLevel.PlayerLoopSystem system;

    [RuntimeInitializeOnLoadMethod]
    private static void Initialize() {
        EnsureSystemFetched();
    }

    private static void EnsureSystemFetched() {
        if (hasFetchedSystem)
            return;

        defaultSystem = UnityEngine.LowLevel.PlayerLoop.GetDefaultPlayerLoop();
        system = CopySystem(defaultSystem);
        hasFetchedSystem = true;

        if (!TryFetchFixedUpdatePointer(system))
            Debug.LogError("Couldn't find FixedUpdate in the built-in player loop systems! This means that setting runInFixedUpdate to true in " +
                           "InsertSystemBefore or After won't work!");

        // if the Entities package is not installed, any systems registered keeps running after we leave play mode.
        // This is "intended behaviour". Not joking. https://fogbugz.unity3d.com/default.asp?1089518_lub560iemcggi1c9
        PlayerLoopQuitChecker.GameQuitCallback += () => {
            UnityEngine.LowLevel.PlayerLoop.SetPlayerLoop(defaultSystem);
        };
    }

    private enum InsertType {
        Before,
        After
    }

    /// <summary>
    /// Inserts a new player loop system in the player loop, just after another system.
    /// </summary>
    /// <param name="newSystemMarker">Type marker for the new system.</param>
    /// <param name="newSystemUpdate">Callback that will be called each frame after insertAfter.</param>
    /// <param name="insertAfter">The subsystem to insert the system after.</param>
    /// <param name="runInFixedUpdate">Set the loop condition function to the same as the built-in FixedUpdate. NOTE: This doesn't work!</param>
    public static void InsertSystemAfter(Type newSystemMarker, UnityEngine.LowLevel.PlayerLoopSystem.UpdateFunction newSystemUpdate, Type insertAfter, bool runInFixedUpdate = false) {
        var playerLoopSystem = new UnityEngine.LowLevel.PlayerLoopSystem {type = newSystemMarker, updateDelegate = newSystemUpdate};
        if (runInFixedUpdate)
            playerLoopSystem.loopConditionFunction = fixedUpdateLoopCondition;

        InsertSystemAfter(playerLoopSystem, insertAfter);
    }

    /// <summary>
    /// Inserts a new player loop system in the player loop, just before another system.
    /// </summary>
    /// <param name="newSystemMarker">Type marker for the new system.</param>
    /// <param name="newSystemUpdate">Callback that will be called each frame before insertBefore.</param>
    /// <param name="insertBefore">The subsystem to insert the system before.</param>
    /// <param name="runInFixedUpdate">Set the loop condition function to the same as the built-in FixedUpdate. NOTE: This doesn't work!</param>
    public static void InsertSystemBefore(Type newSystemMarker, UnityEngine.LowLevel.PlayerLoopSystem.UpdateFunction newSystemUpdate, Type insertBefore, bool runInFixedUpdate = false) {
        var playerLoopSystem = new UnityEngine.LowLevel.PlayerLoopSystem {type = newSystemMarker, updateDelegate = newSystemUpdate};
        if (runInFixedUpdate)
            playerLoopSystem.loopConditionFunction = fixedUpdateLoopCondition;
        InsertSystemBefore(playerLoopSystem, insertBefore);
    }

    /// <summary>
    /// Inserts a new player loop system in the player loop, just after another system.
    /// </summary>
    /// <param name="toInsert">System to insert. Needs to have updateDelegate and Type set.</param>
    /// <param name="insertAfter">The subsystem to insert the system after</param>
    public static void InsertSystemAfter(UnityEngine.LowLevel.PlayerLoopSystem toInsert, Type insertAfter) {
        if (toInsert.type == null)
            throw new ArgumentException("The inserted player loop system must have a marker type!", nameof(toInsert.type));
        if (toInsert.updateDelegate == null)
            throw new ArgumentException("The inserted player loop system must have an update delegate!", nameof(toInsert.updateDelegate));
        if (insertAfter == null)
            throw new ArgumentNullException(nameof(insertAfter));

        EnsureSystemFetched();

        InsertSystem(ref system, toInsert, insertAfter, InsertType.After, out var couldInsert);
        if (!couldInsert) {
            throw new ArgumentException($"When trying to insert the type {toInsert.type.Name} into the player loop after {insertAfter.Name}, " +
                                        $"{insertAfter.Name} could not be found in the current player loop!");
        }

        UnityEngine.LowLevel.PlayerLoop.SetPlayerLoop(system);
    }

    /// <summary>
    /// Inserts a new player loop system in the player loop, just before another system.
    /// </summary>
    /// <param name="toInsert">System to insert. Needs to have updateDelegate and Type set.</param>
    /// <param name="insertBefore">The subsystem to insert the system before</param>
    public static void InsertSystemBefore(UnityEngine.LowLevel.PlayerLoopSystem toInsert, Type insertBefore) {
        if (toInsert.type == null)
            throw new ArgumentException("The inserted player loop system must have a marker type!", nameof(toInsert.type));
        if (toInsert.updateDelegate == null)
            throw new ArgumentException("The inserted player loop system must have an update delegate!", nameof(toInsert.updateDelegate));
        if (insertBefore == null)
            throw new ArgumentNullException(nameof(insertBefore));

        EnsureSystemFetched();

        InsertSystem(ref system, toInsert, insertBefore, InsertType.Before, out var couldInsert);
        if (!couldInsert) {
            throw new ArgumentException($"When trying to insert the type {toInsert.type.Name} into the player loop before {insertBefore.Name}, " +
                                        $"{insertBefore.Name} could not be found in the current player loop!");
        }

        UnityEngine.LowLevel.PlayerLoop.SetPlayerLoop(system);
    }

    /// <summary>
    /// Utility to get a string representation of the current player loop.
    /// Note that this is the current player loop as the PlayerLoopInterface believes it to be, if a different system changes the underlying player loop system,
    /// there's no way to get any info about that.
    /// </summary>
    /// <returns>String representation of the current player loop system.</returns>
    public static string CurrentLoopToString() {
        var stringBuilder = new StringBuilder();

        AppendSystemRecursive(system, stringBuilder, 0);
        return stringBuilder.ToString();

        void AppendSystemRecursive(UnityEngine.LowLevel.PlayerLoopSystem pls, StringBuilder sb, int indentLevel) {
            for (int i = 0; i < indentLevel * 2; i++) {
                sb.Append(' ');
            }

            if (pls.type == null) {
                sb.Append("null");
                sb.Append('\n');
            }
            else {
                sb.Append(pls.type.Name);
                sb.Append('\n');
            }

            if(pls.subSystemList != null) {
                indentLevel++;
                foreach (var subSystem in pls.subSystemList) {
                    AppendSystemRecursive(subSystem, sb, indentLevel);
                }
            }
        }
    }

    public static UnityEngine.LowLevel.PlayerLoopSystem CopySystem(UnityEngine.LowLevel.PlayerLoopSystem system) {
        // PlayerLoopSystem is a struct.
        var copy = system;

        // but the sub system list is an array.
        if (system.subSystemList != null) {
            copy.subSystemList = new UnityEngine.LowLevel.PlayerLoopSystem[system.subSystemList.Length];
            for (int i = 0; i < copy.subSystemList.Length; i++) {
                copy.subSystemList[i] = CopySystem(system.subSystemList[i]);
            }
        }

        return copy;
    }

    private static void InsertSystem(ref UnityEngine.LowLevel.PlayerLoopSystem currentLoopRecursive, UnityEngine.LowLevel.PlayerLoopSystem toInsert, Type insertTarget, InsertType insertType,
                                     out bool couldInsert) {
        var currentSubSystems = currentLoopRecursive.subSystemList;
        if (currentSubSystems == null) {
            couldInsert = false;
            return;
        }

        int indexOfTarget = -1;
        for (int i = 0; i < currentSubSystems.Length; i++) {
            if (currentSubSystems[i].type == insertTarget) {
                indexOfTarget = i;
                break;
            }
        }

        if (indexOfTarget != -1) {
            var newSubSystems = new UnityEngine.LowLevel.PlayerLoopSystem[currentSubSystems.Length + 1];

            var insertIndex = insertType == InsertType.Before ? indexOfTarget : indexOfTarget + 1;

            for (int i = 0; i < newSubSystems.Length; i++) {
                if (i < insertIndex)
                    newSubSystems[i] = currentSubSystems[i];
                else if (i == insertIndex) {
                    newSubSystems[i] = toInsert;
                }
                else {
                    newSubSystems[i] = currentSubSystems[i - 1];
                }
            }

            couldInsert = true;
            currentLoopRecursive.subSystemList = newSubSystems;
        }
        else {
            for (var i = 0; i < currentSubSystems.Length; i++) {
                var subSystem = currentSubSystems[i];
                InsertSystem(ref subSystem, toInsert, insertTarget, insertType, out var couldInsertInInner);
                if (couldInsertInInner) {
                    currentSubSystems[i] = subSystem;
                    couldInsert = true;
                    return;
                }
            }

            couldInsert = false;
        }
    }

    private static bool TryFetchFixedUpdatePointer(UnityEngine.LowLevel.PlayerLoopSystem loopSystem) {
        if (loopSystem.type == typeof(UnityEngine.PlayerLoop.FixedUpdate)) {
            fixedUpdateLoopCondition = loopSystem.loopConditionFunction;
            return true;
        }

        var subSystems = loopSystem.subSystemList;
        if (subSystems != null) {
            foreach (var subSystem in subSystems) {
                if (TryFetchFixedUpdatePointer(subSystem))
                    return true;
            }
        }

        return false;
    }

    // Methods for listing sub system data. Kept around as this stuff is experimental, so being able to quickly check the info is convenient.

    // The loop functions are somewhat weird - a bunch of functions have the same one (like FixedUpdate and Update and LateUpdate), so I assume that dispatch
    // to the correct update loop happens inside of that one.
    private static void FindAllNativeLoopFunctions() {
        Dictionary<IntPtr, List<Type>> loopFunctionToSystems = new Dictionary<IntPtr, List<Type>>();
        IntPtr GrabNativeLoops(UnityEngine.LowLevel.PlayerLoopSystem pls) => pls.updateFunction;

        FindNativeStuff(system, loopFunctionToSystems, GrabNativeLoops);

        string text = "Loop functions\n";
        foreach (var kvp in loopFunctionToSystems) {
            var loopFunc   = kvp.Key;
            var systemList = kvp.Value;

            text += $"  function {loopFunc} used by:\n    {string.Join(", ", systemList.Select(sl => sl.Name))}\n";
        }
        Debug.Log(text);
    }

    // As of 2018.1.6f1, every single default system has the value 0 for their loop condition, except for FixedUpdate.
    private static void FindAllNativeLoopConditions() {
        Dictionary<IntPtr, List<Type>> conditionFunctionToSystems = new Dictionary<IntPtr, List<Type>>();
        IntPtr GrabNativeConditions(UnityEngine.LowLevel.PlayerLoopSystem pls) => pls.loopConditionFunction;

        FindNativeStuff(system, conditionFunctionToSystems, GrabNativeConditions);

        string text = "Condition functions\n";
        foreach (var kvp in conditionFunctionToSystems) {
            var loopFunc   = kvp.Key;
            var systemList = kvp.Value;
            text += $"  function {loopFunc} used by:\n    {string.Join(", ", systemList.Select(sl => sl.Name))}\n";
        }
        Debug.Log(text);
    }

    private static void FindNativeStuff(UnityEngine.LowLevel.PlayerLoopSystem playerLoopSystem, Dictionary<IntPtr, List<Type>> ptrToSystems, Func<UnityEngine.LowLevel.PlayerLoopSystem, IntPtr> grabPtr) {
        var ptr = grabPtr(playerLoopSystem);

        if (!ptrToSystems.ContainsKey(ptr))
            ptrToSystems[ptr] = new List<Type>();

        ptrToSystems[ptr].Add(playerLoopSystem.type);

        if(playerLoopSystem.subSystemList != null) {
            foreach (var subSystem in playerLoopSystem.subSystemList) {
                FindNativeStuff(subSystem, ptrToSystems, grabPtr);
            }
        }
    }
}
