namespace DMCorp.Framework.Basics.Utils;

/// <summary>
/// Генератор случайных текстов
/// </summary>
public static class ShortStringGenerator
{
    private const string possibleChars = "abcdefghijklmnopqrstuvwxyz";

    private static readonly Random random = new();

    /// <summary>
    /// Генерирует случайный набор букв заданной длины
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public static string Generate(short num = 6)
    {
        char[] possibleCharsArray = possibleChars.ToCharArray();
        int possibleCharsAvailable = possibleChars.Length;
        char[] result = new char[num];
        Parallel.For(0, num, index =>
        {
            result[index] = possibleCharsArray[random.Next(possibleCharsAvailable)];
        });
        return new string(result);
    }
}