using UnityEngine;
using SystemHeat;
public class ModuleThermoelectricGenerator : PartModule
{
    [KSPField]
    public string hotLoopID = "TEGHotLoop";

    [KSPField]
    public string coldLoopID = "TEGColdLoop";

    private ModuleSystemHeat hotLoop;
    private ModuleSystemHeat coldLoop;

    [KSPField(isPersistant = false)]
    public double ratedPower = 10;
    [KSPField(isPersistant = false)]
    public double ratedHotTemperature = 900;
    [KSPField(isPersistant = false)]
    public double ratedColdTemperature = 300;

    [KSPField(isPersistant = true)]
    public double conversionEfficiency = 0.2;

    [KSPField(isPersistant = false, guiActive = true, guiName = "Electric Power", guiUnits = " EC/s")]
    public double electricPower = 0.0;

    [KSPField(isPersistant = false, guiActive = true, guiName = "Heat Flux")]
    public double heatFlux = 0.0;

    [KSPField(isPersistant = false, guiActive = true, guiName = "Carnot Efficiency", guiUnits = " %")]
    public double carnotEfficiency;

    [KSPField(isPersistant = false, guiActive = true, guiName = "Actual Efficiency", guiUnits = " %")]
    public double actualEfficiency;

    public double conductance;

    private const string EC = "ElectricCharge";

    private ThermoelectricCalculator model;

    public override void OnStart(StartState state)
    {
        base.OnStart(state);

        model = new ThermoelectricCalculator();

        conductance = model.Conductance(ratedPower, ratedHotTemperature, ratedColdTemperature, conversionEfficiency);

        hotLoop = null;
        coldLoop = null;

        foreach (ModuleSystemHeat loop in GetComponents<ModuleSystemHeat>())
        {
            if (loop.moduleID == hotLoopID)
                hotLoop = loop;

            if (loop.moduleID == coldLoopID)
                coldLoop = loop;
        }

        if (hotLoop == null || coldLoop == null)
        {
            Debug.LogError("[Fusion] Failed to find SystemHeat modules.");
            return;
        } else{
            Debug.Log($"[Fusion] Connected TEG: Hot={hotLoop.moduleID}, Cold={coldLoop.moduleID}");
        }

        hotLoop.ignoreTemperature = true;
        coldLoop.ignoreTemperature = false;
    }

    public void FixedUpdate()
    {
        if (hotLoop == null || coldLoop == null)
            return;

        ThermoelectricResult result = model.Compute(
            hotLoop.LoopTemperature,
            coldLoop.LoopTemperature,
            conductance,
            conversionEfficiency);

        electricPower = result.ElectricPower;
        heatFlux = result.HeatIntake;
        carnotEfficiency = result.CarnotEfficiency;
        actualEfficiency = result.ActualEfficiency;

        part.RequestResource(EC, -electricPower * TimeWarp.fixedDeltaTime);

        hotLoop.AddFlux(hotLoopID,0f,-(float)result.HeatIntake,false);

        coldLoop.AddFlux(coldLoopID,(float)coldLoop.LoopTemperature,(float)result.HeatOutput,true);
        // TODO:
        // add SystemHeat integration to pull temperatures from the loops and push heat flux back into the loops
    }
}
