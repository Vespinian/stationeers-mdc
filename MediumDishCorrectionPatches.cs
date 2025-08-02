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
    //        Debug.Log("DishTransform.up: " + __instance.DishTransform.up.ToString());
    //        Debug.Log("DishTransform.up angle: " + Mathf.Acos(Vector3.Dot(__instance.DishTransform.up, Vector3.up)) * 57.29578f);
    //        Debug.Log("DishForward vector: " + __instance.DishForward.ToString());
    //        Debug.Log("DishForward vector angle: " + Mathf.Acos(Vector3.Dot(__instance.DishForward, Vector3.up)) * 57.29578f);
    //        Debug.Log("Get result: " + __result * 90.0);
    //    }
    //}

    [HarmonyPatch(typeof(SatelliteDish))]
    [HarmonyPatch(nameof(SatelliteDish.DeserializeSave))]
    public class DeserializeSavePatch
    {
        static void Postfix(SatelliteDish __instance, ref double ____vertical, ref double ____horizontal)
        {
            double max_vertical_rad = __instance.MaximumVertical * Math.PI / 180.0;
            // Here we basically construct the DishForwards vector since the setters in the deserialize save function always sets it to a vector that points upward.
            // For a reason I don't know (world rotation?), you have to apply the thing rotation to Vector3.back and not a Vector3.forward,
            // maybe there's a way to know what vector you should apply the rotation to
            //Debug.Log("__instance.DishForward init: " + __instance.DishForward);
            __instance.DishForward = __instance.ThingTransformLocalRotation * Vector3.back;
            //Debug.Log("__instance.DishForward 1: " + __instance.DishForward);
            __instance.DishForward = Quaternion.AngleAxis((Mathf.Lerp(0f, (float)__instance.MaximumHorizontal, (float)____horizontal)), Vector3.up) * __instance.DishForward;
            //Debug.Log("__instance.DishForward 2: " + __instance.DishForward);
            __instance.DishForward = Vector3.RotateTowards(Vector3.up, __instance.DishForward, Mathf.Lerp(0f, (float)max_vertical_rad, (float)____vertical), 0.0f);
            //Debug.Log("__instance.DishForward 3: " + __instance.DishForward);
        }
    }

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
                float current_angle = Mathf.Acos(Vector3.Dot(__instance.DishTransform.up, Vector3.up));
                float desire_angle = Mathf.Lerp(0f, (float)max_vertical_rad, (float)____vertical);
                //Debug.Log("Vertical Setter current_angle: " + current_angle * 180.0 / Math.PI);
                //Debug.Log("Vertical Setter desired angle: " + desire_angle * 180.0 / Math.PI);
                __instance.DishTransform.up = Vector3.RotateTowards(__instance.DishTransform.up, Vector3.up, current_angle - desire_angle, 0.0f);

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
                float current_angle = Mathf.Acos(Vector3.Dot(__instance.DishTransform.up, Vector3.up));
                float desire_angle = Mathf.Lerp(0f, (float)max_vertical_rad, (float)____vertical);
                //Debug.Log("Horizontal Setter current_angle: " + current_angle * 180.0 / Math.PI);
                //Debug.Log("Horizontal Setter desired angle: " + desire_angle * 180.0 / Math.PI);
                __instance.DishTransform.up = Vector3.RotateTowards(__instance.DishTransform.up, Vector3.up, current_angle - desire_angle, 0.0f);

                __instance.DishForward = __instance.DishTransform.up;
                __instance._isDirty = true;
            }
            return false;
        }
    }
}
