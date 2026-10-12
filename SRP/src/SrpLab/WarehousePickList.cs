namespace SrpLab;

/// <summary>
/// Warehouse pick list: naive path order, stock allocation, and picker instruction prose.
/// </summary>
public sealed class WarehousePickList
{
    private readonly List<(string Sku, string Aisle, int Bin, int QtyNeeded, int QtyOnHand)> _lines = new();
    private readonly StockAllocator _allocator=new(); private readonly WarehouseWalkingRoutePlanner _routePlanner=new(); private readonly PickerScriptComposer _scriptComposer=new(); private readonly WmsBatchXmlFormatter _xmlFormatter=new();

    public void AddNeed(string sku, string aisle, int bin, int qtyNeeded, int qtyOnHand)
    {
        _lines.Add((sku, aisle, bin, qtyNeeded, qtyOnHand));
    }

    public IReadOnlyList<(string Sku, int Allocated)> Allocate()
    {
        return _allocator.Allocate(_lines.Select(x=>(x.Sku,x.Aisle,x.Bin,x.QtyNeeded,x.QtyOnHand)));
    }

    public IReadOnlyList<(string Aisle, int Bin, string Sku, int Qty)> WalkingOrder()
    {
        return _routePlanner.Plan(_lines.Select(x=>(x.Sku,x.Aisle,x.Bin,x.QtyNeeded,x.QtyOnHand)));
    }

    public string PickerScript()
    {
        return _scriptComposer.Compose(WalkingOrder(),Allocate(),_lines.Select(x=>(x.Sku,x.Aisle,x.Bin,x.QtyNeeded,x.QtyOnHand)));
    }

    public string WmsXmlBatch(string batchId)
    {
        return _xmlFormatter.Format(batchId,Allocate());
    }
}
