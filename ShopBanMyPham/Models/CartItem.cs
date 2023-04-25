namespace ShopBanMyPham.Models
{
    [Serializable]
    public class CartItem
    {
        public Product Product { get; set; }
        public int SellQuantity { get; set; }

        public int ShippingCost { get; set; }
    }
}
