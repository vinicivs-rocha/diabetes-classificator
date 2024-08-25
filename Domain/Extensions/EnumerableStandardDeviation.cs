namespace Domain.Extensions;

public static class EnumerableStandardDeviation
{
    public static double StandardDeviation<TItem>(this IEnumerable<TItem> enumerable, Func<TItem, double> selector)
    {
        var values = enumerable.Select(selector).ToList();
        var average = values.Average();
        return Math.Sqrt(values.Select(item => Math.Pow((item - average), 2)).Sum() / values.Count);
    }
}