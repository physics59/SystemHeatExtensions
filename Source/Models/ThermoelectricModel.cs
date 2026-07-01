public class ThermoelectricResult
{
    public double CarnotEfficiency;

    public double ActualEfficiency;

    public double HeatIntake;

    public double HeatOutput;

    public double ElectricPower;

    public double OutletTemperature;
}

public class ThermoelectricCalculator
{
    
    public double Conductance(double ratedPower, double ratedHotTemp, double ratedColdTemp, double conversionEfficiency)
    {
        double ratedDeltaTemp = ratedHotTemp - ratedColdTemp;
        return ratedPower / (conversionEfficiency * ratedDeltaTemp * ratedDeltaTemp / ratedHotTemp);
    }

    public double ThermalEfficiency(float hotTemp, float coldTemp)
    {
        return 1 - (coldTemp / hotTemp);
    }

    public double HeatFlux(float hotTemp, float coldTemp, double flowParameter)
    {
        return flowParameter * (hotTemp - coldTemp);
    }

    public double ThermalPower(float hotTemp, float coldTemp, double flowParameter)
    {
        return ThermalEfficiency(hotTemp, coldTemp) * HeatFlux(hotTemp, coldTemp, flowParameter);
    }

    public double ElectricPower(float hotTemp, float coldTemp, double flowParameter, double conversionEfficiency)
    {
        return ThermalPower(hotTemp, coldTemp, flowParameter) * conversionEfficiency;
    }

    public ThermoelectricResult Compute(
        double hotTemp,
        double coldTemp,
        double conductance,
        double conversionEfficiency)
    {
        var result = new ThermoelectricResult();

        if (hotTemp <= coldTemp)
            return result;

        double deltaT = hotTemp - coldTemp;

        // Heat transferred (kW)
        result.HeatIntake = conductance * deltaT;

        // Carnot limit
        result.CarnotEfficiency =
            1.0 - coldTemp / hotTemp;

        // Real efficiency
        result.ActualEfficiency =
            result.CarnotEfficiency * conversionEfficiency;

        // Electricity
        result.ElectricPower =
            result.HeatIntake * result.ActualEfficiency;

        // Remaining heat
        result.HeatOutput =
            result.HeatIntake - result.ElectricPower;

        result.OutletTemperature = hotTemp;

        return result;
    }
}
