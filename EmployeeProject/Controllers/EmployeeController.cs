using EmployeeProject.Data;
using EmployeeProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeProject.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext Context;
        private readonly IWebHostEnvironment WebHostEnvironment;
        private readonly ILogger<EmployeeController> Logger;

        public EmployeeController(ApplicationDbContext _context, IWebHostEnvironment _webHostEnvironment, ILogger<EmployeeController> _logger)
        {
            Context = _context;
            WebHostEnvironment = _webHostEnvironment;
            Logger = _logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginModel model)
        {
            if (IsValidUser(model))
            {
                var user = GetEmployeeByUsername(model.Username);

                if (user != null)
                {
                    TempData["SuccessMessage"] = "Login successful. Welcome, " + model.Username + "!";
                    return RedirectToAction("EmployeeDetails", new { id = user.Id });
                }
            }
            ViewBag.ErrorMessage = "Invalid username or password.";
            return View();
        }
        public async Task<IActionResult> List()
        {
            var users = await Context.Employees.ToListAsync();
            return View(users);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(EmployeeModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        var uploadsFolder = Path.Combine(WebHostEnvironment.WebRootPath, "images");

                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageFile.FileName);

                        var filePath = Path.Combine(uploadsFolder, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            model.ImageFile.CopyTo(stream);
                        }
                        model.ImagePath = "images/" + fileName;
                        Context.Employees.Add(model);
                        await Context.SaveChangesAsync();
                        return RedirectToAction("EmployeeDetails", new { id = model.Id });
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError($"Error registering employee: {ex.Message}");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Bad register request.");
            }           
            return View();            
        }
        [HttpGet]
        public async Task<IActionResult> EditDetails(int id)
        {
            var user = await Context.Employees.FirstOrDefaultAsync(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDetails(int id, EmployeeModel model)
        {
            if (id != model.Id)
            {
                return NotFound("User doesnt exists");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingMember = await Context.Employees.FirstOrDefaultAsync(m => m.Id == id);

                    if (existingMember == null)
                    {
                        return NotFound("User doesnt exists");
                    }
                    var webRootPath = WebHostEnvironment.WebRootPath;
                    var existingImagePath = Path.Combine(webRootPath, existingMember.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(existingImagePath))
                    {
                        System.IO.File.Delete(existingImagePath);
                    }   
                    existingMember.UserName = model.UserName;
                    existingMember.Password = model.Password;
                    existingMember.DateOfBirth = model.DateOfBirth;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        var uploadsFolder = Path.Combine(WebHostEnvironment.WebRootPath, "images");
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageFile.FileName);
                        var filePath = Path.Combine(uploadsFolder, fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            model.ImageFile.CopyTo(stream);
                        }
                        existingMember.ImagePath = "images/" + fileName;
                        await Context.SaveChangesAsync();
                        return RedirectToAction("EmployeeDetails", new { id = id });
                    }
                    return View(model);

                }
                catch(Exception ex)
                {
                    Logger.LogError($"Error updating employee: {ex.Message}");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "User not found.");
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> EmployeeDetails(int id)
        {
            var user = await Context.Employees.FirstOrDefaultAsync(s => s.Id == id);
            if (user != null)
            {
                return View(user);
            }
            return RedirectToAction("Login");
        }
        public bool IsValidUser(LoginModel model)     // To verify the user is valid or not
        {
            var user = Context.Employees.FirstOrDefault(u => u.UserName == model.Username && u.Password == model.Password);
            return user != null;
        }
        public EmployeeModel GetEmployeeByUsername(string username)
         {
            return Context.Employees.SingleOrDefault(u => u.UserName == username);
        }

    }
}
