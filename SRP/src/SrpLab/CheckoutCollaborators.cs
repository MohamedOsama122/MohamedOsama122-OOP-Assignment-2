namespace SrpLab;

public sealed class CouponDiscountPolicy
{
    public decimal Calculate(string? couponText, decimal subtotal)
    {
        if (string.IsNullOrWhiteSpace(couponText)) return 0m;
        var text = couponText.Trim().ToUpperInvariant();
        if (text.StartsWith("SAVE") && int.TryParse(text[4..], out var percentage) && percentage is > 0 and <= 50) return Math.Round(subtotal * percentage / 100m, 2);
        if (text == "WELCOME10") return Math.Min(10m, subtotal);
        return 0m;
    }
}

public sealed class GiftMessageComposer
{
    public string Compose(string fromName, IEnumerable<string> skus, decimal total) { return $"Dear friend,\nA gift from {fromName} awaits ({string.Join(", ", skus)}).\nTotal surprise value: {total:C}\n"; }
}

public sealed class PaymentAuthorizationFormatter
{
    public string Authorize(decimal total, string cardLast4, int lineCount) { var hash = ($"{total:0.00}|{cardLast4}|{lineCount}").GetHashCode(); return $"AUTH-{Math.Abs(hash):X8}"; }
}
