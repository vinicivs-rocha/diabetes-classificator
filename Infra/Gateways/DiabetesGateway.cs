using Domain.Entities;
using Domain.Extensions;
using Domain.Gateways;
using Microsoft.VisualBasic.FileIO;

namespace Infra.Gateways;

public class DiabetesGateway : IDiabetesGateway
{
    private IEnumerable<string[]> GetRawData(string filename)
    {
        var csvReader = new TextFieldParser($"../Domain/Data/{filename}");
        csvReader.SetDelimiters(",");
        csvReader.ReadLine();

        while (!csvReader.EndOfData)
        {
            var fields = csvReader.ReadFields();
            if (fields is null)
            {
                continue;
            }

            yield return fields;
        }
    }

    private static EvaluatedPatient ParseRawPatient(string[] rawPatient) => new EvaluatedPatient(
        pregnancies: double.Parse(rawPatient[0]),
        glucose: double.Parse(rawPatient[1]),
        bloodPressure: double.Parse(rawPatient[2]),
        skinThickness: double.Parse(rawPatient[3]), insulin: double.Parse(rawPatient[4]),
        bmi: double.Parse(rawPatient[5]),
        diabetesPedigreeFunction: double.Parse(rawPatient[6]),
        age: double.Parse(rawPatient[7]), outcome: int.Parse(rawPatient[8]));

    public Patient GetScaledPatient(Patient patient)
    {
        var parsedData = GetRawData("cleaned_diabetes.csv").Select(ParseRawPatient).ToArray();
        
        return new Patient(
            Pregnancies: (patient.Pregnancies -
                          parsedData.Select(p => p.Pregnancies).Average()) /
                         parsedData.StandardDeviation(p => p.Pregnancies),
            Glucose:
            (patient.Glucose - parsedData.Select(p => p.Glucose).Average()) /
            parsedData.StandardDeviation(p => p.Glucose),
            BloodPressure: (patient.BloodPressure -
                            parsedData.Select(p => p.BloodPressure).Average()) /
                           parsedData.StandardDeviation(p => p.BloodPressure),
            Insulin: (patient.Insulin - parsedData.Select(p => p.Insulin).Average()) /
                     parsedData.StandardDeviation(p => p.Insulin),
            Bmi: (patient.Bmi - parsedData.Select(p => p.Bmi).Average()) /
                 parsedData.StandardDeviation(p => p.Bmi),
            Age: (patient.Age - parsedData.Select(p => p.Age).Average()) / parsedData.StandardDeviation(p => p.Age)
        );
    }

    public IEnumerable<EvaluatedPatient> GetTrainingData()
    {
        var rawData = GetRawData("scaled_diabetes.csv").ToArray();
        return rawData.Take((int)double.Ceiling(rawData.Length * 0.8))
            .Select(ParseRawPatient);
    }

    public IEnumerable<EvaluatedPatient> GetTestData()
    {
        var rawData = GetRawData("cleaned_diabetes.csv").ToArray();
        return rawData.Skip((int)double.Ceiling(rawData.Length * 0.8))
            .Select(rawPatient => new EvaluatedPatient(pregnancies: int.Parse(rawPatient[0]),
                glucose: double.Parse(rawPatient[1]),
                bloodPressure: double.Parse(rawPatient[2]),
                skinThickness: double.Parse(rawPatient[3]), insulin: double.Parse(rawPatient[4]),
                bmi: double.Parse(rawPatient[5]),
                diabetesPedigreeFunction: double.Parse(rawPatient[6]),
                age: int.Parse(rawPatient[7]), outcome: int.Parse(rawPatient[8])));
    }
}