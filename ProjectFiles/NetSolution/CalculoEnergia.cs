#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.UI;
using FTOptix.HMIProject;
using FTOptix.NativeUI;
using FTOptix.Retentivity;
using FTOptix.CoreBase;
using FTOptix.Core;
using FTOptix.NetLogic;
using FTOptix.OPCUAServer;
using FTOptix.SQLiteStore;
using FTOptix.Store;
using FTOptix.DataLogger;
using FTOptix.WebUI;
#endregion

public class CalculoEnergia : BaseNetLogic
{
    private PeriodicTask tareaIntegracion;
    private IUAVariable varPotencia;
    private IUAVariable varAcumulado;
    private IUAVariable varDemandaMax;

    public override void Start()
    {
        // 1. Buscamos las variables en el modelo (Asegúrate que estos nombres coincidan con tu Model)
        varPotencia = Project.Current.GetVariable("Model/Consumo_Instantaneo");
        varAcumulado = Project.Current.GetVariable("Model/Consumo_Acumulado");
        varDemandaMax = Project.Current.GetVariable("Model/Demanda_Maxima");

        // 2. Creamos una tarea que se ejecute cada 1000 milisegundos (1 segundo)
        tareaIntegracion = new PeriodicTask(IntegrarEnergia, 1000, LogicObject);
        tareaIntegracion.Start();
    }

    public override void Stop()
    {
        // Limpiamos la tarea al detener el proyecto
        tareaIntegracion?.Dispose();
    }

    /*private void IntegrarEnergia()
    {
        // Verificamos que las variables existan para evitar errores
        if (varPotencia == null || varAcumulado == null) return;

        // Obtenemos la potencia actual en kW
        double kwActual = ToDouble(varPotencia.Value.Value,0.000);

        // Delta Energía (kWh) = Potencia (kW) * (1 segundo / 3600 segundos/hora)
        double deltaEnergia = kwActual / 3600.0;

        // Sumamos al acumulado actual
        double totalAnterior = ToDouble(varAcumulado.Value.Value,0.000);
        varAcumulado.Value = totalAnterior + deltaEnergia;
    }*/

    private void IntegrarEnergia()
    {
        if (varPotencia == null || varAcumulado == null || varDemandaMax == null) return;

        // Tu código existente: Obtenemos la potencia actual en kW
        double kwActual = ToDouble(varPotencia.Value.Value, 0.000);

        // --- NUEVA LÓGICA: DEMANDA MÁXIMA ---
        double demandaRegistrada = ToDouble(varDemandaMax.Value.Value, 0.000);
        
        // Si el consumo de este segundo es mayor al récord, actualizamos el récord
        if (kwActual > demandaRegistrada)
        {
            varDemandaMax.Value = kwActual;
        }
        // ------------------------------------

        // Tu código existente: Delta Energía y suma...
        double deltaEnergia = kwActual / 3600.0;
        double totalAnterior = ToDouble(varAcumulado.Value.Value, 0.000);
        varAcumulado.Value = totalAnterior + deltaEnergia;
    }


    private static double ToDouble(object v, double fallback)
    {
        if (v == null) return fallback;
        try { return Convert.ToDouble(v); }
        catch { return fallback; }
    }
}