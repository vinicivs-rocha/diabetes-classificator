using Domain.Entities;
using Domain.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace diabetes_classificator.Controllers;

public class DiabetesController(EvaluatePatient evaluatePatient, CalculateAccuracy calculateAccuracy) : Controller
{
    private readonly string _accuracy = (calculateAccuracy.Execute() * 100).ToString("F2");
    
    public IActionResult Index()
    {
        ViewData["Accuracy"] = _accuracy;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Evaluate([Bind("Pregnancies,Glucose,BloodPressure,Insulin,Bmi,Age")] Patient patient)
    {
        ViewData["Accuracy"] = _accuracy;
        ViewData["Result"] = (DiabetesOutcome)evaluatePatient.Execute(patient);
        return View("Result");
    }
}