using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skender.Stock.Indicators;

namespace Internal.Tests;

[TestClass]
public class T3 : TestBase
{
    [TestMethod]
    public void Standard()
    {
        List<T3Result> results = quotes
            .GetT3(5, 0.7)
            .ToList();

        // proper quantities
        Assert.AreEqual(502, results.Count);
        Assert.AreEqual(478, results.Count(x => x.T3 != null));

        // sample values
        T3Result r1 = results[23];
        Assert.IsNull(r1.T3);

        T3Result r2 = results[24];
        Assert.AreEqual(215.9343, NullMath.Round(r2.T3, 4));

        T3Result r3 = results[44];
        Assert.AreEqual(224.9412, NullMath.Round(r3.T3, 4));

        T3Result r4 = results[149];
        Assert.AreEqual(235.8851, NullMath.Round(r4.T3, 4));

        T3Result r5 = results[249];
        Assert.AreEqual(257.8735, NullMath.Round(r5.T3, 4));

        T3Result r6 = results[501];
        Assert.AreEqual(238.9308, NullMath.Round(r6.T3, 4));
    }

    [TestMethod]
    public void UseTuple()
    {
        List<T3Result> results = quotes
            .Use(CandlePart.Close)
            .GetT3()
            .ToList();

        Assert.AreEqual(502, results.Count);
        Assert.AreEqual(478, results.Count(x => x.T3 != null));
    }

    [TestMethod]
    public void TupleNaN()
    {
        List<T3Result> r = tupleNanny
            .GetT3()
            .ToList();

        Assert.AreEqual(200, r.Count);
        Assert.AreEqual(0, r.Count(x => x.T3 is double and double.NaN));
    }

    [TestMethod]
    public void Chainee()
    {
        List<T3Result> results = quotes
            .GetSma(2)
            .GetT3()
            .ToList();

        Assert.AreEqual(502, results.Count);
        Assert.AreEqual(477, results.Count(x => x.T3 != null));
    }

    [TestMethod]
    public void Chainor()
    {
        List<SmaResult> results = quotes
            .GetT3()
            .GetSma(10)
            .ToList();

        Assert.AreEqual(502, results.Count);
        Assert.AreEqual(469, results.Count(x => x.Sma != null));
    }

    [TestMethod]
    public void BadData()
    {
        List<T3Result> r = badQuotes
            .GetT3()
            .ToList();

        Assert.AreEqual(502, r.Count);
        Assert.AreEqual(0, r.Count(x => x.T3 is double and double.NaN));
    }

    [TestMethod]
    public void NoQuotes()
    {
        List<T3Result> r0 = noquotes
            .GetT3()
            .ToList();

        Assert.AreEqual(0, r0.Count);

        List<T3Result> r1 = onequote
            .GetT3()
            .ToList();

        Assert.AreEqual(1, r1.Count);
    }

    [TestMethod]
    public void Removed()
    {
        List<T3Result> results = quotes
            .GetT3(5, 0.7)
            .RemoveWarmupPeriods()
            .ToList();

        // assertions
        Assert.AreEqual(502 - ((6 * (5 - 1)) + 250), results.Count);

        T3Result last = results.LastOrDefault();
        Assert.AreEqual(238.9308, NullMath.Round(last.T3, 4));
    }

    [TestMethod]
    public void Exceptions()
    {
        // bad lookback period
        Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            quotes.GetT3(0));

        // bad volume factor
        Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            quotes.GetT3(25, 0));
    }
}
