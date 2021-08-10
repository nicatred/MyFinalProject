using Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ValidationRules.FluentValidation
{
    public class ProductValidator:AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(e => e.ProductName).NotEmpty();
            RuleFor(e => e.ProductName).MinimumLength(2);
            RuleFor(e => e.UnitPrice).NotEmpty();
            RuleFor(e => e.UnitPrice).GreaterThan(0);
            RuleFor(e => e.UnitPrice).GreaterThan(5).When(e => e.CategoryId == 1);
            RuleFor(e => e.ProductName).Must(StartWithA).WithMessage("Məhsulun adı A hərfi ilə başlamlıdır");
        }

        private bool StartWithA(string arg)
        {
          return  arg.StartsWith('A');
        }
    }
}
