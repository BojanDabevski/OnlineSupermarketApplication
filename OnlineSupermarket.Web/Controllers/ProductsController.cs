using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineSupermarket.Domain.DomainModels;
using OnlineSupermarket.Domain.DTO;
using OnlineSupermarket.Domain.Identity;
using OnlineSupermarket.Repository;
using OnlineSupermarket.Services.Interface;

namespace OnlineSupermarket.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: Products
        public IActionResult Index()
        {
            var allProducts = this._productService.GetAllProducts();
            return View(allProducts);
        }
        public IActionResult AddProductToCard(Guid? id)
        {
            var model = this._productService.GetShoppingCartInfo(id);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddProductToCard([Bind("ProductId", "Quantity")] AddToShoppingCardDTO item)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = this._productService.AddToShoppingCart(item, userId);
            if (result)
            {
                return RedirectToAction("Index", "Products");
            }

            return View(item);
        }

        // GET: Products/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = this._productService.GetDetailsForProduct(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,ProductName,ProductImage,ProductDescription,ProductPrice,Rating,ExpirationDate,Category")] Product product)
        {
            if (ModelState.IsValid)
            {
                
                this._productService.CreateNewProduct(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = this._productService.GetDetailsForProduct(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Id,ProductName,ProductImage,ProductDescription,ProductPrice,Rating,ExpirationDate,Category")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    this._productService.UpdeteExistingProduct(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = this._productService.GetDetailsForProduct(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            this._productService.DeleteProduct(id);
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(Guid id)
        {
            return this._productService.GetDetailsForProduct(id) != null;
        }
        public IActionResult ProductByCategory()
        {
            var allProducts = this._productService.GetAllProducts();
            return View(allProducts); ;
        }
        [HttpGet]
        public FileContentResult ExportAllTickets()
        {

            string fileName = "Products.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("All Products");
                worksheet.Cell(1, 1).Value = "TicketName";
                worksheet.Cell(1, 2).Value = "Category";
                worksheet.Cell(1, 3).Value = "ExpirationDate";
                worksheet.Cell(1, 4).Value = "ProductDescription";
                worksheet.Cell(1, 5).Value = "Price";
                worksheet.Cell(1, 6).Value = "Rating";

                //string zanr =  MyAction();
                List<Product> result = _productService.GetAllProducts();
                //var s = MyAction();
                for (int i = 1; i <= result.Count(); i++)
                {
                    var item = result[i - 1];

                    worksheet.Cell(i + 1, 1).Value = item.ProductName.ToString();
                    worksheet.Cell(i + 1, 2).Value = item.Category.ToString();
                    worksheet.Cell(i + 1, 3).Value = item.ExpirationDate.ToString();
                    worksheet.Cell(i + 1, 4).Value = item.ProductDescription.ToString();
                    worksheet.Cell(i + 1, 5).Value = item.ProductPrice.ToString();
                    worksheet.Cell(i + 1, 6).Value = item.Rating.ToString();


                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(content, contentType, fileName);
                }

            }
            return null;
        }
        public FileContentResult ExportCertainCategory(string valueINeed)
        {
            string fileName = valueINeed.ToString()+"Products.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("All Products");
                worksheet.Cell(1, 1).Value = "TicketName";
                worksheet.Cell(1, 2).Value = "Category";
                worksheet.Cell(1, 3).Value = "ExpirationDate";
                worksheet.Cell(1, 4).Value = "ProductDescription";
                worksheet.Cell(1, 5).Value = "Price";
                worksheet.Cell(1, 6).Value = "Rating";

                //string zanr =  MyAction();
                List<Product> result = _productService.GetAllProducts();
                //var s = MyAction();
                for (int i = 1; i <= result.Count(); i++)
                {
                    var item = result[i - 1];
                    if (valueINeed == item.Category.ToString())
                    {
                        worksheet.Cell(i + 1, 1).Value = item.ProductName.ToString();
                        worksheet.Cell(i + 1, 2).Value = item.Category.ToString();
                        worksheet.Cell(i + 1, 3).Value = item.ExpirationDate.ToString();
                        worksheet.Cell(i + 1, 4).Value = item.ProductDescription.ToString();
                        worksheet.Cell(i + 1, 5).Value = item.ProductPrice.ToString();
                        worksheet.Cell(i + 1, 6).Value = item.Rating.ToString();
                    }

                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(content, contentType, fileName);
                }

            }
            return null;
        }
    }
}
