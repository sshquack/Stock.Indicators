using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skender.Stock.Indicators;

namespace Internal.Tests;

[TestClass]
public class Awesome : TestBase
{
    [TestMethod]
    public void Standard()
    {
        List<AwesomeResult> results = quotes
            .GetAwesome(5, 34)
            .ToList();

        // proper quantities
        Assert.AreEqual(502, results.Count);
        Assert.AreEqual(469, results.Count(x => x.Oscillator != null));

        // sample values
        AwesomeResult r1 = results[32];
        Assert.AreEqual(null, r1.Oscillator);
        Assert.AreEqual(null, r1.Normalized);

        AwesomeResult r2 = results[33];
        Assert.AreEqual(5.4756, NullMath.Round(r2.Oscillator, 4));
        Assert.AreEqual(2.4548, NullMath.Round(r2.Normalized, 4));

        AwesomeResult r3 = results[249];
        Assert.AreEqual(5.0618, NullMath.Round(r3.Oscillator, 4));
        Assert.AreEqual(1.9634, NullMath.Round(r3.Normalized, 4));

        AwesomeResult r4 = results[501];
        Assert.AreEqual(-17.7692, NullMath.Round(r4.Oscillator, 4));
        Assert.AreEqual(-7.2763, NullMath.Round(r4.Normalized, 4));
    }

    [TestMethod]
    public void UseTuple()
    {
        List<AwesomeResult> results = quotes
            .Use(CandlePart.Close)
            .GetAwesome()
            .ToList();

        Assert.AreEqual(502, results.Count);
        Assert.AreEqual(469, results.Count(x => x.Oscillator != null));
    }

    [TestMethod]
    public void TupleNaN()
    {
        List<AwesomeResult> r = tupleNanny
            .GetAwesome()
            .ToList();

        Assert.AreEqual(200, r.Count);
        Assert.AreEqual(0, r.Count(x => x.Oscillator is double and double.NaN));
    }

    [TestMethod]
    public void Chainee()
    {
        List<AwesomeResult> results = quotes
            .GetSma(2)
            .GetAwesome()
            .ToList();

        Assert.AreEqual(502, results.Count);
        Assert.AreEqual(468, results.Count(x => x.Oscillator != null));
    }

    [TestMethod]
    public void Chainor()
    {
        List<SmaResult> results = quotes
            .GetAwesome()
            .GetSma(10)
            .ToList();

        Assert.AreEqual(502, results.Count);
        Assert.AreEqual(460, results.Count(x => x.Sma != null));
    }

    [TestMethod]
    public void BadData()
    {
        List<AwesomeResult> r = badQuotes
            .GetAwesome()
            .ToList();

        Assert.AreEqual(502, r.Count);
        Assert.AreEqual(0, r.Count(x => x.Oscillator is double and double.NaN));
    }

    [TestMethod]
    public void NoQuotes()
    {
        List<AwesomeResult> r0 = noquotes
            .GetAwesome()
            .ToList();

        Assert.AreEqual(0, r0.Count);

        List<AwesomeResult> r1 = onequote
            .GetAwesome()
            .ToList();

        Assert.AreEqual(1, r1.Count);
    }

    [TestMethod]
    public void Removed()
    {
        List<AwesomeResult> results = quotes
            .GetAwesome(5, 34)
            .RemoveWarmupPeriods()
            .ToList();

        // assertions
        Assert.AreEqual(502 - 33, results.Count);

        AwesomeResult last = results.LastOrDefault();
        Assert.AreEqual(-17.7692, NullMath.Round(last.Oscillator, 4));
        Assert.AreEqual(-7.2763, NullMath.Round(last.Normalized, 4));
    }

    [TestMethod]
    public void Exceptions()
    {
        // bad fast period
        Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            quotes.GetAwesome(0, 34));

        // bad slow period
        Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            quotes.GetAwesome(25, 25));
    }
}
