namespace Domain.Entities;

public readonly struct EvaluatedPatient(
    double pregnancies,
    double glucose,
    double skinThickness,
    double bloodPressure,
    double insulin,
    double diabetesPedigreeFunction,
    double bmi,
    double age,
    int outcome)
{
    public double Pregnancies { get; init; } = pregnancies;
    public double Glucose { get; init; } = glucose;
    public double SkinThickness { get; init; } = skinThickness;
    public double BloodPressure { get; init; } = bloodPressure;
    public double Insulin { get; init; } = insulin;
    public double DiabetesPedigreeFunction { get; init; } = diabetesPedigreeFunction;
    public double Bmi { get; init; } = bmi;
    public double Age { get; init; } = age;
    public int Outcome { get; init; } = outcome;
}