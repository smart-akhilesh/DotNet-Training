using System;
using System.Collections.Generic;
using System.Web.UI;

namespace Asp_Assignment_1
{
    public partial class Products : Page
    {
        private class Product
        {
            public string Name { get; set; }
            public decimal Price { get; set; }
            public string ImageUrl { get; set; }
        }

        private static readonly List<Product> productList = new List<Product>
        {
            new Product { Name = "Laptop", Price = 50000, ImageUrl = "~/Images/laptop.jpg" },
            new Product { Name = "Mobile Phone", Price = 30000, ImageUrl = "~/Images/phone.jpg" },
            new Product { Name = "Headphones", Price = 2000, ImageUrl = "~/Images/headphone.jpg" },
            new Product { Name = "Smart Watch", Price = 1000, ImageUrl = "~/Images/watch.jpg" },
            new Product { Name = "Tablet", Price = 20000, ImageUrl = "~/Images/tablet.jpg" }
        };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlProducts.DataSource = productList;
                ddlProducts.DataTextField = "Name";
                ddlProducts.DataValueField = "Name";
                ddlProducts.DataBind();
                ddlProducts.Items.Insert(0, "-- Select Product --");
            }
        }

        protected void ddlProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlProducts.SelectedIndex > 0)
            {
                Product selected = productList.Find(p => p.Name == ddlProducts.SelectedValue);
                imgProduct.ImageUrl = selected.ImageUrl;
            }
            else
            {
                imgProduct.ImageUrl = "";
            }
        }

        protected void btnGetPrice_Click(object sender, EventArgs e)
        {
            if (ddlProducts.SelectedIndex > 0)
            {
                Product selected = productList.Find(p => p.Name == ddlProducts.SelectedValue);
                lblPrice.Text = "Price: Rs " + selected.Price.ToString("F2");
            }
            else
            {
                lblPrice.Text = "Please select a product.";
            }
        }
    }
}
