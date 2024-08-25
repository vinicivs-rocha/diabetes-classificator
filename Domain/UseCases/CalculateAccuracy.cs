using Domain.Entities;
using Domain.Gateways;

namespace Domain.UseCases;

public class CalculateAccuracy(IDiabetesGateway diabetesGateway, int k = 73)
{
    public double Execute()
    {
        var trainingData = diabetesGateway.GetTrainingData();
        var testingData = diabetesGateway.GetTestData().ToArray();
        var testingPatients = testingData.Select(evaluatedPatient => new Patient(
            Pregnancies: evaluatedPatient.Pregnancies,
            Glucose: evaluatedPatient.Glucose,
            BloodPressure: evaluatedPatient.BloodPressure,
            Insulin: evaluatedPatient.Insulin,
            Bmi: evaluatedPatient.Bmi, Age: evaluatedPatient.Age));
        var scaledTestingPatients = testingPatients.Select(diabetesGateway.GetScaledPatient);
        
        var outcomes = testingData.Zip(scaledTestingPatients, (first, second) => new { expectedOutcome = first.Outcome, predictedOutcome = second.Classify(trainingData, k) });
        
        return outcomes.Count(outcome => outcome.expectedOutcome == outcome.predictedOutcome) / (double)testingData.Length;
    }
}