namespace TestForCompany;

/// <summary>
/// Статический класс для поиска минимальной суммы двух различных чисел в массиве вещественных чисел с учётом приблизительного равенства.
/// </summary>
public static class MinSumSolver
{
    private const double relEps = 1e-9; // точность сравнения
    
    /// <summary>
    /// Проверяет, равны ли два вещественных числа с заданной относительной точностью relEps.
    /// </summary>
    /// <param name="a">Первое число.</param>
    /// <param name="b">Второе число.</param>
    /// <param name="relEps">Относительная точность сравнения (по умолчанию 1e-9).</param>
    /// <returns>True, если |a-b| не превышает relEps*максимум из |a|, |b| и 1.0.</returns>
    public static bool AlmostEqual(double a, double b)
    {
        double diff = Math.Abs(a - b);
        double max = Math.Max(Math.Max(Math.Abs(a), Math.Abs(b)), 1.0);
        
        // Проверка на переполнение relEps * max
        double threshold = relEps * max;
        if (double.IsInfinity(threshold))
        {
            // Только если diff тоже бесконечность, значит числа непредставимо большие, и условно равны
            return diff == double.PositiveInfinity;
        }
        
        return diff <= threshold;
    }

    /// <summary>
    /// Возвращает список уникальных (с учётом почти-равенства) значений из массива.
    /// Выбрасывает исключение, если обнаружит NaN или бесконечность, или если найдется меньше двух уникальных элементов.
    /// </summary>
    /// <param name="arr">Входной массив.</param>
    /// <returns>Список уникальных чисел.</returns>
    /// <exception cref="InvalidOperationException">
    /// Бросается, если найден NaN или Infinity, или уникальных значений меньше двух.
    /// </exception>
    private static LinkedList<double> GetUniques(double[] arr)
    {
        var uniques = new LinkedList<double>();

        foreach (double x in arr)
        {
            // Проверка на недопустимые значения (NaN, Infinity)
            if (double.IsNaN(x) || double.IsInfinity(x))
                throw new ArgumentException("Array contains NaN or Infinity.");

            // Проверка числа на уникальность с учетом AlmostEqual
            bool isUnique = true;
            foreach (double y in uniques)
                if (AlmostEqual(x, y))
                {
                    isUnique = false;
                    break;
                }
            if (isUnique)
                uniques.AddLast(x);
        }
        // Должно быть как минимум два уникальных значения
        if (uniques.Count < 2)
            throw new ArgumentException("Array must contain at least two unique elements (with respect to precision).");

        return uniques;
    }

    /// <summary>
    /// Ищет минимально возможную сумму двух различных уникальных чисел в массиве (с учетом погрешности сравнения).
    /// Выполняется за O(n), не использует сортировку.
    /// </summary>
    /// <param name="arr">Массив входных значений.</param>
    /// <returns>Минимальная сумма двух уникальных чисел.</returns>
    /// <exception cref="InvalidOperationException">
    /// Бросается, если в массиве меньше двух элементов, либо все элементы практически неотличимы, либо есть NaN/Infinity.
    /// </exception>
    public static double GetMinSumOfValues(double[]? arr)
    {
        // Проверка: массив должен содержать хотя бы два элемента
        if (arr?.Length < 2 || arr == null)
            throw new ArgumentException("Array must contain at least two elements.");

        // Получить список уникальных значений
        var uniques = GetUniques(arr);

        // Найти два минимальных из этих уникальных
        double min1 = double.MaxValue;
        double min2 = double.MaxValue;

        foreach (double x in uniques)
        {
            if (x < min1)
            {
                min2 = min1; // было минимумом, смещаем на второе место
                min1 = x;    // найден новый минимум
            }
            else if (x < min2)
            {
                min2 = x;    // новый второй минимум
            }
        }
        // Суммируем два самых маленьких уникальных значения
        return min1 + min2;
    }
}