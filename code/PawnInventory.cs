namespace Kira;

public class PawnInventory
{
    public List<InventorySlot> Items;

    public PawnInventory()
    {
        Items = new List<InventorySlot>();

        for (int i = 0; i < 4; i++)
        {
            Items.Add(new InventorySlot());
        }

        Items[0].GiveItem(ItemsDb.IceCream);
        Items[1].GiveItem(ItemsDb.Shield);
    }
}

public class InventorySlot
{
    public bool HasItem { get; set; }
    public PawnItem Item { get; set; }

    public void GiveItem(PawnItem item)
    {
        Item = item;
        HasItem = true;
    }

    public void RemoveItem()
    {
        HasItem = false;
    }

    public bool TryRemoveItem(out PawnItem item)
    {
        if (!HasItem)
        {
            item = null;
            return false;
        }

        item = Item;
        HasItem = false;
        return true;
    }
}

public static class ItemsDb
{
    public static PawnItem IceCream = new PawnItem(1, "Icecream", "icecream");
    public static PawnItem Shield = new PawnItem(2, "Shield", "shield");
}

public class PawnItem
{
    public string Name { get; set; }
    public int Id { get; set; }
    public string Icon { get; set; }

    public PawnItem(int id, string name, string icon)
    {
        this.Id = id;
        this.Name = name;
        this.Icon = icon;
    }
}