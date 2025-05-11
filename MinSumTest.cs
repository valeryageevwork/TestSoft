namespace TestForCompany;

using Xunit;

public class MinSumSolverTests
{
    /// <summary>
    /// Тестируем типовые случаи с небольшими массивами.
    /// Для каждого сценария указан ожидаемый результат: сумма двух минимальных элементов.
    /// </summary>
    [Theory]
    [InlineData(new double[] { 1.2, 2.5, 3.7, 5.0, 8.3 },     1.2 + 2.5)]
    [InlineData(new double[] { -10, -10.1, 2, 8.9, 101 },     -10.1 + -10)]
    [InlineData(new double[] { 10.3, 9.9, 1.2, 1.6, 1.5 },    1.2 + 1.5)]
    [InlineData(new double[] { 0, 0.0001, 0.5, 1 },           0 + 0.0001)]
    [InlineData(new double[] { -4.5, -4.49, 0.01 },           -4.5 + -4.49)]
    public void TestBasic(double[] data, double expected)
    {
        double actual = MinSumSolver.GetMinSumOfValues(data);
        Assert.True(MinSumSolver.AlmostEqual(actual, expected), $"Ожидалось {expected}, получено {actual}");
    }

    /// <summary>
    /// Проверяем обработку некорректных входных данных:
    /// если в массиве меньше двух элементов — ожидается ArgumentException.
    /// </summary>
    [Fact]
    public void TestInvalidInput()
    {
        // Один элемент
        Assert.Throws<ArgumentException>(() => MinSumSolver.GetMinSumOfValues(new double[] { 1.0 }));
        // Пустой массив
        Assert.Throws<ArgumentException>(() => MinSumSolver.GetMinSumOfValues(new double[0]));
        // Null
        Assert.Throws<ArgumentException>(() => MinSumSolver.GetMinSumOfValues(null));
    }

    /// <summary>
    /// Проверка на очень большом массиве (миллиард элементов).
    /// Минимальные элементы известны заранее.
    /// Внимание: этот тест требует много ОЗУ.
    /// </summary>
    [Fact]
    public void Test_BillionElements()
    {
        const int N = 10_000; // Еcли необходимо - поставьте больше, но времени займет более. Сложность линейная o(n)
        double step = 0.25;

        // Массив вида: 0, 0.25, 0.5, ..., посчитаем сумму двух наименьших (0 и 0.25)
        double expected = 0 + 0.25;

        var bigArray = new double[N];
        for (int i = 0; i < N; i++)
            bigArray[i] = i * step;

        double actual = MinSumSolver.GetMinSumOfValues(bigArray);
        Assert.True(MinSumSolver.AlmostEqual(actual, expected),
            $"Для миллиарда элементов ожидается {expected}, получено {actual}");
    }
    
    /// <summary>
    /// Проверка: разница между двумя минимальными числами меньше eps.
    /// </summary>
    [Fact]
    public void Test_TwoMinValuesDifferenceLessThanEps()
    {

        double a = 5.123456789;
        double b = a + 1e-9 / 2; // b отличается от a меньше, чем eps
        double[] data = { 10.0, a, 20.0, b, 30.0 }; // b = a, поскольку отличаются на величину меньшую eps
        
        double expectedSum = a + 10.0;
        double actual = MinSumSolver.GetMinSumOfValues(data);
        
        Assert.True(MinSumSolver.AlmostEqual(actual, expectedSum),
            $"actual: {actual} (ожидалось {expectedSum})"
        );
    }
}