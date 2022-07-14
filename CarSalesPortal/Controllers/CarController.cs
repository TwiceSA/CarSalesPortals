using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarSalesPortal.AppDbControl;
using CarSalesPortal.Models;
using CarSalesPortal.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.AspNetCore.Hosting.Internal;
using cloudscribe.Pagination.Models;
using CarSalesPortal.Helpers;
using System.Diagnostics;

namespace CarSalesPortal.Controllers
{
    [Authorize(Roles = Roles.Admin + "," + Roles.Executive)]
    public class CarController : Controller
    {
        private readonly CarSalesPortalDbContext _db;
        private readonly HostingEnvironment _hostingEnvironment;


        [BindProperty]
        public CarViewModel CarVM { get; set; }

        public CarController(CarSalesPortalDbContext db, HostingEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
            CarVM = new CarViewModel()
            {
                Makes = _db.Makes.ToList(),
                Models = _db.Models.ToList(),
                Car = new Models.Car()
            };

        }

        [AllowAnonymous]
        public IActionResult Index(string searchString, string sortOrder, int pageNumber=1, int pageSize=2)
        {
            ViewBag.CurrentSortOrder = sortOrder;
            ViewBag.CurrentFilter = searchString;
            ViewBag.PriceSortParam = String.IsNullOrEmpty(sortOrder)?"price_desc" : "";
            int ExcludeRecords = (pageSize * pageNumber) - pageSize;



            var Cars = from b in _db.Cars.Include(m => m.Make).Include(m => m.Model)
                       select b;

            var CarCount = Cars.Count();

            if (!String.IsNullOrEmpty(searchString))
            {
                Cars = Cars.Where(b => b.Make.Name.Contains(searchString));
                CarCount = Cars.Count();
            }

            //sorting logic
            switch (sortOrder)
            {
                case "price_desc" :
                    Cars = Cars.OrderByDescending(b => b.Price);
                    break;
                default:
                    Cars = Cars.OrderBy(b => b.Price);
                    break;
            }


                Cars=Cars
                .Skip(ExcludeRecords)
                .Take(pageSize);

            var result = new PagedResult<Car>
            {
                Data = Cars.AsNoTracking().ToList(),
                TotalItems = CarCount,
                PageNumber = pageNumber,
                PageSize = pageSize

            };

            return View(result);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            CarVM.Car = _db.Cars.SingleOrDefault(b => b.Id == id);

            CarVM.Models = _db.Models.Where(m => m.MakeID == CarVM.Car.MakeID);

            if (CarVM == null)
            {
                return NotFound();
            }
            return View(CarVM);
        }
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public IActionResult EditPost()
        {
            if (!ModelState.IsValid)
            {
                CarVM.Makes = _db.Makes.ToList();
                CarVM.Models = _db.Models.ToList();
                return View(CarVM);
            }
            _db.Cars.Update(CarVM.Car);
            UploadImageIfAvailable();
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));

        }


        //Get Method
        public IActionResult Create()
        {
            return View(CarVM);
        }

        //Post Method
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePost()
        {
            if (!ModelState.IsValid)
            {
                CarVM.Makes = _db.Makes.ToList();
                CarVM.Models = _db.Models.ToList();
                return View(CarVM);
            }
            _db.Cars.Add(CarVM.Car);
            UploadImageIfAvailable();
                _db.SaveChanges();
            
            return RedirectToAction(nameof(Index));

        }

        private void UploadImageIfAvailable()
        {
            //Get Car ID saved in database
            var CarID = CarVM.Car.Id;

            //get wwwwrootpath to save the file on server
            string wwwrootPath = _hostingEnvironment.WebRootPath;

            //get the uploaded files
            var files = HttpContext.Request.Form.Files;

            //Get the refrence of DBset for the Car we just saved in the database
            var SavedCar = _db.Cars.Find(CarID);

            //Upload the files on a server and save the image path of uploaded file
            if (files.Count != 0)
            {
                var ImagePath = @"C:\Users\tshep\source\repos\CarSalesPortal\CarSalesPortal\wwwroot\images\car";
                var Extension = Path.GetExtension(files[0].FileName);
                var RelativeImagePath = ImagePath + CarID + Extension;
                var AbsImagePath = Path.Combine(wwwrootPath, RelativeImagePath);

                // Upload file on Server
                using (var fileStream = new FileStream(AbsImagePath, FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }

                //Set the iamge path on database
                SavedCar.ImagePath = RelativeImagePath;
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            Car car = _db.Cars.Find(id);
            if (car == null)
            {
                return NotFound();
            }
            _db.Cars.Remove(car);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult View(int id)
        {
            CarVM.Car = _db.Cars.SingleOrDefault(b => b.Id == id);

            if (CarVM.Car == null)
            {
                return NotFound();
            }
            return View(CarVM);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}