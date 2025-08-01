using HarmonyLib;
using Assets.Scripts.Objects.Electrical;
using UnityEngine;
using Assets.Scripts.Util;
using System;


namespace MediumDishCorrection
{
    //[HarmonyPatch(typeof(SatelliteDish))]
    //[HarmonyPatch(nameof(SatelliteDish.Vertical), MethodType.Getter)]
    //public class VerticalGetPatch
    //{
    //    static void Postfix(SatelliteDish __instance, ref double __result, ref double ____vertical, ref double ____horizontal)
    //    {
    //        Debug.LogWarning("DishTransform.up: " + __instance.DishTransform.up.ToString());
    //        Debug.LogWarning("DishTransform.up angle: " + Mathf.Acos(Vector3.Dot(__instance.DishTransform.up, Vector3.up)) * 57.29578f);
    //        Debug.LogWarning("DishForward vector: " + __instance.DishForward.ToString());
    //        Debug.LogWarning("DishForward vector angle: " + Mathf.Acos(Vector3.Dot(__instance.DishForward, Vector3.up)) * 57.29578f);
    //        Debug.LogWarning("Get result: " + __result * 90.0);
    //    }
    //}

    [HarmonyPatch(typeof(SatelliteDish))]
    [HarmonyPatch(nameof(SatelliteDish.Vertical), MethodType.Setter)]
    public class VerticalSetPatch
    {
        static bool Prefix(SatelliteDish __instance, double value, ref double ____vertical)
        {
            if (____vertical != value)
            {
                ____vertical = value;
                __instance.BaseAnimator.SetFloat(Defines.Animator.Vertical, (float)____vertical);

                // Apply correction to DishTransform.up vertical component
                double max_vertical_rad = __instance.MaximumVertical * Math.PI / 180.0;
                double current_angle = Mathf.Acos(Vector3.Dot(__instance.DishTransform.up, Vector3.up));
                __instance.DishTransform.up = Vector3.RotateTowards(__instance.DishTransform.up, Vector3.up, (float)((current_angle - (____vertical * max_vertical_rad))), 0.0f);

                __instance.DishForward = __instance.DishTransform.up;
                __instance._isDirty = true;
            }
            return false;
        }
    }

    [HarmonyPatch(typeof(SatelliteDish))]
    [HarmonyPatch(nameof(SatelliteDish.Horizontal), MethodType.Setter)]
    public class HorizontalSetPatch
    {
        static bool Prefix(SatelliteDish __instance, double value, ref double ____vertical, ref double ____horizontal)
        {
            if (____horizontal != value)
            {
                ____horizontal = value;
                __instance.BaseAnimator.SetFloat(Defines.Animator.Horizontal, (float)____horizontal);

                // Apply correction to DishTransform.up vertical component
                double max_vertical_rad = __instance.MaximumVertical * Math.PI / 180.0;
                double current_angle = Mathf.Acos(Vector3.Dot(__instance.DishTransform.up, Vector3.up));
                __instance.DishTransform.up = Vector3.RotateTowards(__instance.DishTransform.up, Vector3.up, (float)((current_angle - (____vertical * max_vertical_rad))), 0.0f);

                __instance.DishForward = __instance.DishTransform.up;
                __instance._isDirty = true;
            }
            return false;
        }
    }
}
