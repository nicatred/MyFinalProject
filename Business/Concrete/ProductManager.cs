using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        private IProductDal _productDal;
        public ProductManager(IProductDal productDal)
        {
            _productDal = productDal;
        }

        public IResult Add(Product product)
        {
            var context = new ValidationContext<Product>(product);
            ProductValidator productValidator= new ProductValidator();
            var result = productValidator.Validate(context);

            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
            _productDal.Add(product);
            return new SuccessResult(Messages.ProductAdded);
        }


        public IDataResult<List<Product>> GetAll()
        {
            if (DateTime.Now.Hour == 7)
            {
                return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
            }
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(),Messages.ProductListed);
        }

        public IDataResult<List<Product>> GetAllByCategoryId(int id)
        {
            if (DateTime.Now.Hour == 7)
            {
                return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
            }
            return new  SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.CategoryId == id),Messages.ProductListed);   
        }

        public IDataResult<List<Product>> GetAllByUnitPrice(decimal min, decimal max)
        {
            if (DateTime.Now.Hour == 7)
            {
                return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
            }
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.UnitPrice >= min && p.UnitPrice <= max),Messages.ProductListed);
        }

        public IDataResult<Product> GetById(int id)
        {
            if (DateTime.Now.Hour == 8)
            {
                return new ErrorDataResult<Product>(Messages.MaintenanceTime);
            }
            return new SuccessDataResult<Product>(_productDal.Get(p => p.ProductId == id),Messages.ProductReturned);
        }

        public IDataResult<List<ProductDetailDto>> GetProductDetails()
        {
            if (DateTime.Now.Hour ==9) {
                return new ErrorDataResult<List<ProductDetailDto>>(Messages.MaintenanceTime);
            }
            return new SuccessDataResult<List<ProductDetailDto>>(_productDal.GetProductDetails(),Messages.ProductListed);
        }
    }
}
