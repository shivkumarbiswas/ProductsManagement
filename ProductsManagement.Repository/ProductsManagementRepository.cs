using ProductsManagement.Repository.Interfaces;
using ProductsManagement.Repository.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ProductsManagement.Repository
{
    public class ProductsManagementRepository : IProductsManagementRepository
    {
        public ProductsManagementRepository()
        {
        }

        public List<Product> GetActiveProductsList(string connectionString)
        {
            DataTable dtProducts = new DataTable();

            List<Product> products = new List<Product>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand objSqlCommand = new SqlCommand("GetActiveProductsList", con);
                objSqlCommand.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter objSqlDataAdapter = new SqlDataAdapter(objSqlCommand);
                try
                {
                    objSqlDataAdapter.Fill(dtProducts);

                    foreach (DataRow dr in dtProducts.Rows)
                    {

                        products.Add(
                            new Product
                            {
                                ProductCode = Convert.ToInt32(dr["ProductCode"]),
                                Name =Convert.ToString(dr["Name"]),
                                Category = new Category() { Name = Convert.ToString(dr["CategoryName"]) },
                                UnitPrice = Convert.ToDecimal(dr["UnitPrice"]),
                                ManufactureDate = Convert.ToDateTime(dr["ManufactureDate"]),
                                Active = Convert.ToBoolean(dr["Active"]),
                                AddedDate = Convert.ToDateTime(dr["AddedDate"]),
                                LastEditedDate = dr["LastEditedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastEditedDate"]): (DateTime?)null,
                            }
                        );
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return products;
        }

        public List<Product> GetActiveProductsListByCategory(int categoryCode, string connectionString)
        {
            DataTable dtProducts = new DataTable();

            List<Product> products = new List<Product>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand com = new SqlCommand("GetActiveProductsListByCategory", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@categoryCode", categoryCode);
                SqlDataAdapter objSqlDataAdapter = new SqlDataAdapter(com);
                try
                {
                    objSqlDataAdapter.Fill(dtProducts);

                    foreach (DataRow dr in dtProducts.Rows)
                    {

                        products.Add(
                            new Product
                            {
                                ProductCode = Convert.ToInt32(dr["ProductCode"]),
                                Name =Convert.ToString(dr["Name"]),
                                Category = new Category() { Name = Convert.ToString(dr["CategoryName"]) },
                                UnitPrice = Convert.ToDecimal(dr["UnitPrice"]),
                                ManufactureDate = Convert.ToDateTime(dr["ManufactureDate"]),
                                Active = Convert.ToBoolean(dr["Active"]),
                                AddedDate = Convert.ToDateTime(dr["AddedDate"]),
                                LastEditedDate = dr["LastEditedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastEditedDate"]) : (DateTime?)null,
                            }
                        );
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return products;
        }

        public Product GetProductById(int productCode, string connectionString)
        {
            DataTable dtProducts = new DataTable();

            Product product = null;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand com = new SqlCommand("GetProductById", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@productCode", productCode);
                SqlDataAdapter objSqlDataAdapter = new SqlDataAdapter(com);
                try
                {
                    objSqlDataAdapter.Fill(dtProducts);

                    foreach (DataRow dr in dtProducts.Rows)
                    {

                        product =
                            new Product
                            {
                                ProductCode = Convert.ToInt32(dr["ProductCode"]),
                                Name =Convert.ToString(dr["Name"]),
                                Category = new Category() { Name = Convert.ToString(dr["CategoryName"]), CategoryCode = Convert.ToInt32(dr["CategoryCode"]) },
                                UnitPrice = Convert.ToDecimal(dr["UnitPrice"]),
                                ManufactureDate = Convert.ToDateTime(dr["ManufactureDate"]),
                                Active = Convert.ToBoolean(dr["Active"]),
                                AddedDate = Convert.ToDateTime(dr["AddedDate"]),
                                LastEditedDate = dr["LastEditedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastEditedDate"]) : (DateTime?)null,
                            };

                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return product;
        }

        //For insert and update
        public bool AddOrUpdateProduct(Product product, string action, int productCode, string connectionString)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand com;
                if (action.Equals("Add"))
                {
                    com = new SqlCommand("AddNewProduct", con);
                }
                else
                {
                    com = new SqlCommand("UpdateExistingProduct", con);
                    com.Parameters.AddWithValue("@productCode", productCode);
                }

                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Name", product.Name);
                com.Parameters.AddWithValue("@CategoryCode", product.Category.CategoryCode);
                com.Parameters.AddWithValue("@UnitPrice", product.UnitPrice);
                com.Parameters.AddWithValue("@ManufactureDate", product.ManufactureDate);
                com.Parameters.AddWithValue("@Active", product.Active);
                com.Parameters.AddWithValue("@AddedDate", product.AddedDate);
                com.Parameters.AddWithValue("@LastEditedDate", product.LastEditedDate);


                con.Open();
                int i = com.ExecuteNonQuery();
                con.Close();
                if (i >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public List<Category> GetAllCategories(string connectionString)
        {
            DataTable dtCategories = new DataTable();

            List<Category> categories = new List<Category>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand objSqlCommand = new SqlCommand("GetAllCategories", con);
                objSqlCommand.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter objSqlDataAdapter = new SqlDataAdapter(objSqlCommand);

                try
                {
                    objSqlDataAdapter.Fill(dtCategories);

                    foreach (DataRow dr in dtCategories.Rows)
                    {

                        categories.Add(
                            new Category
                            {
                                CategoryCode = Convert.ToInt32(dr["CategoryCode"]),
                                Name =Convert.ToString(dr["Name"]),
                                Description = Convert.ToString(dr["Description"]),
                                AddedDate = Convert.ToDateTime(dr["AddedDate"]),
                                LastEditedDate = dr["LastEditedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastEditedDate"]) : (DateTime?)null,
                            }
                        );
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return categories;
        }

        public void DeleteProductById(int productCode, string connectionString)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand com = new SqlCommand("DeleteProductById", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@productCode", productCode);
                SqlDataAdapter objSqlDataAdapter = new SqlDataAdapter(com);
                try
                {
                    con.Open();
                    int i = com.ExecuteNonQuery();
                    con.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
