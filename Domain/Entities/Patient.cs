namespace Domain.Entities;

public record Patient(double Pregnancies, double Glucose, double BloodPressure, double Insulin, double Bmi, double Age)
{
    public int Classify(IEnumerable<EvaluatedPatient> trainingData, int k = 1) =>
        trainingData
            .OrderBy(Distance)
            .Take(k)
            .GroupBy(neighbour => neighbour.Outcome)
            .OrderByDescending(group => group.Count()).First().Key;

    private double Distance(EvaluatedPatient patient)
    {
        return Math.Sqrt(
            Math.Pow(Pregnancies - patient.Pregnancies, 2) +
            Math.Pow(Glucose - patient.Glucose, 2) +
            Math.Pow(BloodPressure - patient.BloodPressure, 2) +
            Math.Pow(Insulin - patient.Insulin, 2) +
            Math.Pow(Bmi - patient.Bmi, 2) +
            Math.Pow(Age - patient.Age, 2)
        );
    }
};