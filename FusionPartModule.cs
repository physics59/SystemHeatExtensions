using UnityEngine;

public class FusionPartModule : PartModule
{
    [KSPField]
    public float baseFuelConsumption = 0.2f;

    [KSPField(guiActive = true, guiName = "Fusion Reactor Status")]
    public string status = "Offline";

    [KSPField(isPersistant = true, guiActive = true, guiName = "Reactor Enabled")]
    public bool enabledReactor = false;

    [KSPEvent(guiActive = true, guiName = "Toggle Reactor")]
    public void ToggleReactor()
    {
        enabledReactor = !enabledReactor;
        status = enabledReactor ? "Online" : "Offline";
    }

    [KSPField(isPersistant = true, guiActive = true, guiName = "Power Level")]
    [UI_FloatRange(minValue = 10f, maxValue = 100f, stepIncrement = 5f)]
    public float powerPercent = 100f;

    [KSPField(guiActive = true, guiName = "Fuel Flow", guiUnits = " u/s")]
    public float fuelFlow = 0.0f;

    [KSPField(guiActive = true, guiName = "Heat")]
    public double heat = 0;

    [KSPField(guiActive = true, guiName = "Ticks")]
    public int ticks = 0;

    public override void OnUpdate()
    {
        fuelFlow = enabledReactor ? baseFuelConsumption * (powerPercent / 100f) : 0f;

        if (!HighLogic.LoadedSceneIsFlight || !enabledReactor) return;

        ticks++;

        heat += fuelFlow * 10f * Time.deltaTime;

        double fuelNeeded = fuelFlow * Time.deltaTime;

        double fuelConsumed = part.RequestResource(
        "LqdDeuterium",
        fuelNeeded,
        ResourceFlowMode.ALL_VESSEL
        );

        if (fuelConsumed < fuelNeeded * 0.99f)
        {
            enabledReactor = false;
            status = "Insufficient Fuel";
        }
    }
}