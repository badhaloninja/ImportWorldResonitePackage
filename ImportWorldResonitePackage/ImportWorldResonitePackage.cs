using FrooxEngine;
using HarmonyLib;
using ResoniteModLoader;
using Elements.Core;

namespace ImportWorldResonitePackage
{
    public class ImportWorldResonitePackage : ResoniteMod
    {
        public override string Name => "Import World ResonitePackage";
        public override string Author => "badhaloninja";
        public override string Version => "1.0.0";
        public override string Link => "https://github.com/badhaloninja/ImportWorldResonitePackage";

        static readonly Harmony harmony = new("ninja.badhalo.ImportWorldResonitePackage");

        public override void OnEngineInit()
        {
            harmony.PatchAll();
        }

        [HarmonyPatch]
        class Patch
        {
            [HarmonyPrefix]
            [HarmonyPatch(typeof(Slot), nameof(Slot.LoadObject))]
            public static bool LoadObjectWorld(Slot __instance, DataTreeDictionary node)
            {
                if (node.TryGetNode("Slots") != null)
                {
                    // Not destroying immediately because some places call the slot after loading object
                    __instance.PersistentSelf = false;
                    __instance.DestroyWhenUserLeaves(__instance.LocalUser);


                    Msg("Loading raw world from object");
                    Userspace.StartLocal(null, node, true);

                    return false;
                }
                return true;
            }
        }
    }
}