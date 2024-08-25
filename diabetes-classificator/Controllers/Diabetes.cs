using Domain.Entities;
using Domain.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace diabetes_classificator.Controllers;

public class DiabetesController(EvaluatePatient evaluatePatient, CalculateAccuracy calculateAccuracy) : Controller
{
    // GET: Index
    public IActionResult Index()
    {
        ViewData["Accuracy"] = (calculateAccuracy.Execute() * 100).ToString("F2");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public string Evaluate([Bind("Pregnancies,Glucose,BloodPressure,Insulin,Bmi,Age")] Patient patient)
    {
        return ((DiabetesOutcome)evaluatePatient.Execute(patient)).ToString();
    }
}