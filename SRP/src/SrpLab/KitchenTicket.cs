namespace SrpLab;

/// <summary>
/// Kitchen ticket: allergen scan from ingredients, cook ETA heuristics, and printed ticket layout.
/// </summary>
public sealed class KitchenTicket
{
    private readonly List<(string Item, List<string> Ingredients, int PrepMinutes)> _items = new();
    private readonly AllergenScanner _scanner=new(); private readonly KitchenReadyTimeEstimator _estimator=new(); private readonly ThermalTicketRenderer _renderer=new(); private readonly ExpoLaneSelector _laneSelector=new();

    public void AddItem(string item, IEnumerable<string> ingredients, int prepMinutes)
    {
        _items.Add((item, ingredients.Select(i => i.Trim().ToLowerInvariant()).ToList(), prepMinutes));
    }

    public IReadOnlyList<string> DetectAllergens()
    {
        return _scanner.Scan(_items.Select(x=>x.Ingredients));
    }

    public int EstimatedReadyMinutes(int openStations)
    {
        return _estimator.Estimate(_items.Select(x=>x.PrepMinutes),openStations,DetectAllergens().Count>0);
    }

    public string RenderThermalTicket(int orderNumber)
    {
        var allergens=DetectAllergens();return _renderer.Render(orderNumber,_items.Select(x=>(x.Item,x.PrepMinutes)),EstimatedReadyMinutes(2),allergens);
    }

    public string ExpoLaneHint()
    {
        return _laneSelector.Select(DetectAllergens(),EstimatedReadyMinutes(2));
    }
}
