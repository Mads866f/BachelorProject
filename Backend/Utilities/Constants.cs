namespace Backend.Utilities;

public class Constants
{
    public static string PbEngine = "PbEngine";

    public static Dictionary<string, string> sat_map = new Dictionary<string, string>()
    {
        { "1", "CC  Sat" },
        { "2", "Cost  Sqrt  Sat" },
        {"3", "CostLog  Sat"},
        { "4", "AdditiveSatisfaction" },
        { "5", "Relative  Cost  Sat" },
        { "6", "Relative  Cost  Approx  Normaliser  Sat" },
        { "7", "Additive  Cost  Sqrt  Sat" },
        { "8", "Cost  Log  Sat" },
        { "9", "Additive  Cost  Log  Sat" },
        { "10", "Cardinality  Sat " },
        { "11", "Relative  Cardinality  Sat " },
        { "12", "Effort  Sat " },
        { "13", "Additive  Cardinal  Sat" },
        { "14", "Relative  Cardinality  Sat" },
        { "15", "Additive  Borda  Sat " }
    };
}