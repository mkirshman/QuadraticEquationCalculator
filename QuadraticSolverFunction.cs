using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

public static class QuadraticSolverFunction
{
    [FunctionName("SolveQuadraticEquation")]
    public static IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
        ILogger log)
    {
        try
        {
            // Get the values of 'a', 'b', and 'c' from query parameters
            if (req.Query.TryGetValue("a", out var aValues) &&
                req.Query.TryGetValue("b", out var bValues) &&
                req.Query.TryGetValue("c", out var cValues) &&
                double.TryParse(aValues, out double a) &&
                double.TryParse(bValues, out double b) &&
                double.TryParse(cValues, out double c))
            {
                // Calculate the discriminant
                double discriminant = (b * b) - (4 * a * c);

                // Check the discriminant value
                if (discriminant < 0)
                {
                    return new OkObjectResult("No real roots");
                }
                else if (discriminant == 0)
                {
                    double root = -b / (2 * a);
                    return new OkObjectResult($"Single real root: {root}");
                }
                else
                {
                    double root1 = (-b + Math.Sqrt(discriminant)) / (2 * a);
                    double root2 = (-b - Math.Sqrt(discriminant)) / (2 * a);
                    return new OkObjectResult($"Two real roots: {root1}, {root2}");
                }
            }

            return new BadRequestObjectResult("Invalid or missing query parameters.");
        }
        catch (Exception ex)
        {
            log.LogError($"An error occurred: {ex.Message}");
            return new BadRequestObjectResult("An error occurred");
        }
    }
}
