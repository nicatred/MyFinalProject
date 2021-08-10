using Business.Concrete;
using DataAccess.Concrete.EntityFrameWork;
using System;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
             ProuctTest();
            //CategoryTest();
            Console.ReadKey();
        }

        private static void CategoryTest()
        {
            CategoryManager categoryManager = new CategoryManager(new EfCategoryDal());
            foreach (var category in categoryManager.GetAll())
            {
                Console.WriteLine(category.CategoryName);
            }
        }

        private static void ProuctTest()
        {
            ProductManager productManager = new ProductManager(new EfProductDal());
            var result = productManager.GetProductDetails();
            if (result.Success)
            {
                foreach (var product in productManager.GetProductDetails().Data)
                {
                    Console.WriteLine((product.ProductName + "/" + product.Categoryname));
                }
            }
            else
            {
                Console.WriteLine(result.Message);
            }
          
           
        }
    }
}
