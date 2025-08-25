using BreakInfinity;

public static class Utils
{
    public static string FormatNum(BigDouble num, bool canFloat = false)
    {
        if (num < 0.01 && num != 0 && canFloat)
        {
            return num.ToString("F5");
        }
        else if (num < 100 && canFloat)
        {
            return num.ToString("F2");
        }
        else if (num < 100000)
        {
            return num.ToString("F0");
        }
        else
        {
            return num.ToString("E1");
        }
    }
}
