using ProductsManagement.Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductsManagement.Repository.Interfaces
{
    public interface IProductsManagementRepository
    {
        List<Product> GetActiveProductsList(string connectionString);

        List<Product> GetActiveProductsListByCategory(int categoryCode, string connectionString);

        Product GetProductById(int productCode, string connectionString);

        bool AddOrUpdateProduct(Product product, string action, int productCode, string connectionString);

        List<Category> GetAllCategories(string connectionString);

        void DeleteProductById(int productCode, string connectionString);
    }
}
