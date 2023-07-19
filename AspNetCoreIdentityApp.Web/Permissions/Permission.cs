namespace AspNetCoreIdentityApp.Web.Permissions;

// 
public class Permission
{
    public static class Stock
    {
        public const string ListStocks = "Permissions.Stock.ListStocks";
        public const string CreateStock = "Permissions.Stock.CreateStock";
        public const string EditStock = "Permissions.Stock.EditStock";
        public const string DeleteStock = "Permissions.Stock.DeleteStock";
    }

    public static class Order
    {
        public const string ListOrders = "Permissions.Order.ListOrders";
        public const string CreateOrder = "Permissions.Order.CreateOrder";
        public const string EditOrder = "Permissions.Order.EditOrder";
        public const string DeleteOrder = "Permissions.Order.DeleteOrder";
    }

    public static class Catalog
    {
        public const string ListCatalogs = "Permissions.Catalog.ListCatalogs";
        public const string CreateCatalog = "Permissions.Catalog.CreateCatalog";
        public const string EditCatalog = "Permissions.Catalog.EditCatalog";
        public const string DeleteCatalog = "Permissions.Catalog.DeleteCatalog";
    }
}

