using Domain.Entities;

namespace Domain.Gateways;

public interface IDiabetesGateway
{
    public Patient GetScaledPatient(Patient patient);
    public IEnumerable<EvaluatedPatient> GetTrainingData();
    public IEnumerable<EvaluatedPatient> GetTestData();
}