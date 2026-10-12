namespace SrpLab;

/// <summary>
/// Online checkout basket: totals, coupon linguistics, gift-wrap copy, and a faux payment auth code.
/// </summary>
public sealed class CheckoutBasket
{
    private readonly List<(string Sku, decimal Price, int Qty)> _lines = new();
    private string? _couponRaw;
    private bool _giftWrap;
    private readonly CouponDiscountPolicy _couponPolicy = new();
    private readonly GiftMessageComposer _giftMessageComposer = new();
    private readonly PaymentAuthorizationFormatter _paymentAuthorization = new();

    public void AddLine(string sku, decimal price, int qty)
    {
        if (qty <= 0) throw new ArgumentOutOfRangeException(nameof(qty));
        _lines.Add((sku, price, qty));
    }

    public void ApplyCouponText(string? couponText) => _couponRaw = couponText;
    public void EnableGiftWrap() => _giftWrap = true;

    public decimal SubTotal() => _lines.Sum(l => l.Price * l.Qty);

    public decimal DiscountAmount()
    {
        return _couponPolicy.Calculate(_couponRaw, SubTotal());
    }

    public decimal GrandTotal()
    {
        var total = SubTotal() - DiscountAmount();
        if (_giftWrap) total += 4.99m; // packaging fee policy ≠ cart math
        return Math.Max(0m, total);
    }

    public string GiftMessageCard(string fromName)
    {
        // Customer-facing copy will change with marketing, not with totals.
        return _giftMessageComposer.Compose(fromName, _lines.Select(l => l.Sku), GrandTotal());
    }

    public string AuthorizePaymentStub(string cardLast4)
    {
        // Pretends to talk to a gateway — auth scheme changes independently of cart rules.
        return _paymentAuthorization.Authorize(GrandTotal(), cardLast4, _lines.Count);
    }
}
