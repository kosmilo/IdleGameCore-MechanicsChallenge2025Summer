using BreakInfinity;

public static class Utils
{
    public static string FormatNum(BigDouble num, bool canFloat = true)
    {
        if (num < 10 && canFloat)
        {
            return num.ToString("F2");
        }
        else if (num < 100000)
        {
            return num.ToString("F0");
        }
        else
        {
            return num.ToString("G4");
        }
    }
}
