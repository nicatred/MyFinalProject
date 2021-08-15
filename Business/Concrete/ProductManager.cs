using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Business;
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
        private ICategoryService _categoryService;
        public ProductManager(IProductDal productDal,ICategoryService categoryService)
        {
            _productDal = productDal;
            _categoryService = categoryService;
        }
        [ValidationAspect(typeof(ProductValidator))]
        public IResult Add(Product product)
        {
            var result = BusinessRules.Run(CheckIfProductNameExist(product.ProductName),
                CheckIfProductCountOfCategoryCorrect(product.CategoryId),CheckIfCategoryLimitExceed());
            if(result != null)
            {
                return result;
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

        public IResult Update(Product product)
        {
            if (CheckIfProductCountOfCategoryCorrect(product.CategoryId).Success)
            {
                _productDal.Update(product);
                return new SuccessResult();
            }
            else
            {
                return new ErrorResult();
            }
        }
        private IResult CheckIfProductCountOfCategoryCorrect(int categoryId)
        {
            var result = _productDal.GetAll(x => x.CategoryId == categoryId).Count;
            if (result > 10)
            {
                return new ErrorResult(Messages.ProductOfCategory);
            }
            return new SuccessResult();
        }
        private IResult CheckIfProductNameExist(string productName)
        {
           
            var result = _productDal.GetAll(p => p.ProductName == productName).Any();
            if (result)
            {
                return new ErrorResult(Messages.ProductNameAlreadyExist);
            }
            return new SuccessResult();
        }
        private IResult CheckIfCategoryLimitExceed()
        {

            var result = _categoryService.GetAll().Data.Count;
            if (result>15)
            {
                return new ErrorResult(Messages.CategoryLimitExceed);
            }
            return new SuccessResult();
        }
    }
}
