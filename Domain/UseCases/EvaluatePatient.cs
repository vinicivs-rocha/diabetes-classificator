using Domain.Entities;
using Domain.Gateways;

namespace Domain.UseCases;

public class EvaluatePatient(IDiabetesGateway diabetesGateway, int k = 73)
{
    public int Execute(Patient patient)
    {
        var trainingData = diabetesGateway.GetTrainingData();
        var scaledPatient = diabetesGateway.GetScaledPatient(patient);
        Console.WriteLine(scaledPatient);
        return scaledPatient.Classify(trainingData, k);
    }
}