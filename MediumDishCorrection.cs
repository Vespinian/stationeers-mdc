using HarmonyLib;
using StationeersMods.Interface;


namespace MediumDishCorrection
{
    class MediumDishCorrection : ModBehaviour
    {
        public override void OnLoaded(ContentHandler contentHandler)
        {
            Harmony harmony = new Harmony("MediumDishCorrection");
            harmony.PatchAll();
            UnityEngine.Debug.Log("Medium Dish Correction Loaded!");
        }
    }
}