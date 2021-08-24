using HarmonyLib;
using NeosModLoader;
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using FrooxEngine;
using FrooxEngine.LogiX;

namespace ExposeAllImpulses
{
    public class ExposeAllImpulses : NeosMod
    {
        public override string Name => "ExposeAllImpulses";
        public override string Author => "eia485";
        public override string Version => "1.0.0";
        public override string Link => "https://github.com/EIA485/NeosExposeAllImpulses/";
        public override void OnEngineInit()
        {
            Harmony harmony = new Harmony("net.eia485.ExposeAllImpulses");
            harmony.PatchAll();

        }
        [HarmonyPatch(typeof(LogixHelper), "GetImpulseTargets")]
        class ExposeAllImpulsesPatch
        {
            public static bool Prefix(ref MethodInfo[] __result, Type nodeType, ref Dictionary<System.Type, MethodInfo[]> ___impulseTargets)
            {

            MethodInfo[] ValidTargets;

            if (!___impulseTargets.TryGetValue(nodeType, out ValidTargets))
			{

				ValidTargets = (from m in nodeType.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
				where m.ReturnType == typeof(void) && m.GetParameters().Length == 0 && m.GetCustomAttributes(typeof(SyncMethod), false).Any<object>()
				select m).ToArray<MethodInfo>();

                ___impulseTargets.Add(nodeType, ValidTargets);

			}

            __result = ValidTargets;

            return false;

            }


        }
    }
}
