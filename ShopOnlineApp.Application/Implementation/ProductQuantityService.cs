using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Data.IRepositories;

namespace ShopOnlineApp.Application.Implementation
{
    public class ProductQuantityService : IProductQuantityService
    {
        private readonly IProductQuantityRepository _quantityRepository;
        public ProductQuantityService(IProductQuantityRepository quantityRepository)
        {
            _quantityRepository = quantityRepository;
        }
        public async Task<bool> SellProduct(int productId, int quantity)
        {
            var productQuantiry = await _quantityRepository.GetByProductId(productId);
            if (productQuantiry != null)
            {
                return productQuantiry.Quantity >= quantity;
            }
            return false;
        }

        public async Task UpdateQuantityProduct(int productId, int quantity)
        {
            var productQuantity = await _quantityRepository.GetByProductId(productId);
            if (productQuantity != null)
            {
                productQuantity.Quantity -= quantity;
                await _quantityRepository.SaveChanges();
            }

        }
    }
}
