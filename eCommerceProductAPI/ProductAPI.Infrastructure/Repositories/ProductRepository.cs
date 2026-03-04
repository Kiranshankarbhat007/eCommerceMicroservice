using eCommerce.SharedLibrary.Logs;
using eCommerce.SharedLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ProductAPI.App.Interfaces;
using ProductAPI.Domain.Entities;
using ProductAPI.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ProductAPI.Infrastructure.Repositories
{
    public class ProductRepository(ProductDbContext context) : IProduct
    {
        public async Task<Response> CreateAsync(Product entity)
        {
            try
            {
                var getProduct = await GetByAsync(p => p.Name.ToLower() == entity.Name.ToLower());
                if(getProduct != null)
                {
                    return new Response(false, $"Product with the same name - {getProduct.Name} already exists.");
                }
                else
                {
                    var newProduct = context.Products.Add(entity).Entity;
                    await context.SaveChangesAsync();
                    if(newProduct != null && newProduct.Id > 0)
                    {
                        return new Response(true, $"{newProduct.Name} product added successfully");
                    }
                    else
                    {
                        return new Response(false, $"Error occurred while adding {newProduct.Name} to database");
                    }
                }
            }
            catch (Exception ex)
            {
                //Log exception
                LogExceptions.LogException(ex);
                return new Response(false, $"An error occurred while creating the product - {entity.Name}.");
            }
        }

        public async Task<Response> DeleteAsync(Product entity)
        {
            try
            {
                var product = await FindByIdAsync(entity.Id);
                if (product == null)
                {
                    return new Response(false, $"This product - {product.Name} does not exists.");
                }
                else
                {
                    context.Products.Remove(product);
                    await context.SaveChangesAsync();
                    return new Response(true, $"{product.Name} product deleted successfully");
                }
            }
            catch (Exception ex)
            {
                //Log exception
                LogExceptions.LogException(ex);
                return new Response(false, $"An error occurred while deleting product - {entity.Name}.");
            }
        }

        public async Task<Product?> FindByIdAsync(long id)
        {
            try
            {
                Product? product = await context.Products.FindAsync(id);
                return product != null ? product : null;
            }
            catch (Exception ex)
            {
                //Log exception
                LogExceptions.LogException(ex);
                throw new Exception("An error occurred while finding the product by ID.", ex);
            }
        }

        public async Task<IEnumerable<Product>?> GetAllAsync()
        {
            try
            {
                IEnumerable<Product>? products = await context.Products.AsNoTracking().ToListAsync();
                return products != null ? products : null;
            }
            catch (Exception ex)
            {
                //Log exception
                LogExceptions.LogException(ex);
                throw new Exception("An error occurred while fetching products", ex);
            }
        }

        public async Task<Product?> GetByAsync(Expression<Func<Product, bool>> predicate)
        {
            try
            {
                var product = context.Products.Where(predicate).FirstOrDefault();
                return product != null ? product : null;
            }
            catch(Exception ex)
            {
                LogExceptions.LogException(ex);
                throw new Exception("An error occurred while fetching products", ex);
            }
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            try
            {
                Product? product = await context.Products.FindAsync(id);
                return product != null ? product : null;
            }
            catch (Exception ex)
            {
                //Log exception
                LogExceptions.LogException(ex);
                throw new Exception("An error occurred while finding the product by ID.", ex);
            }
        }

        public async Task<Response> UpdateAsync(Product entity)
        {
            try
            {
                var product = await FindByIdAsync(entity.Id);
                if (product == null)
                {
                    return new Response(false, $"This product - {product.Name} not found.");
                }
                else
                {
                    context.Entry(product).State = EntityState.Detached;
                    context.Products.Update(entity);
                    await context.SaveChangesAsync();
                    return new Response(true, $"{entity.Name} product updated successfully");
                }
            }
            catch (Exception ex)
            {
                //Log exception
                LogExceptions.LogException(ex);
                return new Response(false, $"An error occurred while updating product - {entity.Name}.");
            }
        }
    }
}
