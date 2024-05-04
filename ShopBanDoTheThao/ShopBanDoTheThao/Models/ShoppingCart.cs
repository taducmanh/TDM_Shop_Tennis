using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopBanDoTheThao.Models
{
    public class ShoppingCart
    {
        public ShoppingCart()
        {
            ListItem = new List<ShoppingCartItem>();
        }
        public List<ShoppingCartItem> ListItem { get; set; }
        public void AddToCart(ShoppingCartItem item)
        {
            if (ListItem.Where(s => s.ProductName.Equals(item.ProductName)).Any())
            {
                var myItem = ListItem.Single(s => s.ProductName.Equals(item.ProductName));
                myItem.Quanlity += item.Quanlity;
                myItem.Total += item.Quanlity * Convert.ToDouble(item.Price.Trim().Replace(",", string.Empty).Replace(".", string.Empty));
            }
            else
            {
                ListItem.Add(item);
            }
        }
        public bool RemoveFromCart(int lngProductSellID)
        {
            ShoppingCartItem existsItem = ListItem.Where(x => x.ProductID == lngProductSellID).SingleOrDefault();
            if (existsItem != null)
            {
                ListItem.Remove(existsItem);
            }
            return true;
        }
        public bool UpdateQuantity(int lngProductSellID, int intQuantity)
        {
            ShoppingCartItem existsItem = ListItem.Where(x => x.ProductID == lngProductSellID).SingleOrDefault();
            if (existsItem != null)
            {
                existsItem.Quanlity = intQuantity;
                existsItem.Total = existsItem.Quanlity * Convert.ToDouble(existsItem.Price.Replace(",", string.Empty).Replace(".", string.Empty));
            }
            return true;
        }
        public bool EmptyCart()
        {
            ListItem.Clear();
            return true;
        }

        public class ShoppingCartItem
        {
            public string Image { get; set; }
            public int ProductID { get; set; }
            public string ProductName { get; set; }
            public string Price { get; set; }
            public int Quanlity { get; set; }
            public double Total { get; set; }
        }
    }
}