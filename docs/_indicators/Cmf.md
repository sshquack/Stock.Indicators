---
title: Chaikin Money Flow (CMF)
description: Created by Marc Chaikin, Chaikin Money Flow is the simple moving average of the directional Money Flow Volume.
permalink: /indicators/Cmf/
image: /assets/charts/Cmf.png
type: volume-based
layout: indicator
---

# {{ page.title }}

Created by Marc Chaikin, [Chaikin Money Flow](https://en.wikipedia.org/wiki/Chaikin_Analytics#Chaikin_Money_Flow) is the simple moving average of the directional Money Flow Volume.
[[Discuss] :speech_balloon:]({{site.github.repository_url}}/discussions/261 "Community discussion about this indicator")

![image]({{site.baseurl}}{{page.image}})

```csharp
// usage
IEnumerable<CmfResult> results =
  quotes.GetCmf(lookbackPeriods);
```

## Parameters

| name | type | notes
| -- |-- |--
| `lookbackPeriods` | int | Number of periods (`N`) in the moving average.  Must be greater than 0.  Default is 20.

### Historical quotes requirements

You must have at least `N+1` periods of `quotes` to cover the warmup periods.

`quotes` is a collection of generic `TQuote` historical price quotes.  It should have a consistent frequency (day, hour, minute, etc).  See [the Guide]({{site.baseurl}}/guide/#historical-quotes) for more information.

## Response

```csharp
IEnumerable<CmfResult>
```

- This method returns a time series of all available indicator values for the `quotes` provided.
- It always returns the same number of elements as there are in the historical quotes.
- It does not return a single incremental indicator value.
- The first `N-1` periods will have `null` values since there's not enough data to calculate.

### CmfResult

| name | type | notes
| -- |-- |--
| `Date` | DateTime | Date
| `MoneyFlowMultiplier` | double | Money Flow Multiplier
| `MoneyFlowVolume` | double | Money Flow Volume
| `Cmf` | double | Chaikin Money Flow = SMA of MFV

> :warning: **Warning**: absolute values in MFV and CMF are somewhat meaningless.  Use with caution.

### Utilities

- [.Condense()]({{site.baseurl}}/utilities#condense)
- [.Find(lookupDate)]({{site.baseurl}}/utilities#find-indicator-result-by-date)
- [.RemoveWarmupPeriods()]({{site.baseurl}}/utilities#remove-warmup-periods)
- [.RemoveWarmupPeriods(qty)]({{site.baseurl}}/utilities#remove-warmup-periods)

See [Utilities and helpers]({{site.baseurl}}/utilities#utilities-for-indicator-results) for more information.

## Chaining

Results can be further processed on `Cmf` with additional chain-enabled indicators.

```csharp
// example
var results = quotes
    .GetCmf(..)
    .GetSlope(..);
```

This indicator must be generated from `quotes` and **cannot** be generated from results of another chain-enabled indicator or method.
