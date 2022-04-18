using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NinjaHeaven.Models;
using NinjaHeaven.Services;

namespace NinjaHeaven.Controllers
{
    public class UserController : Controller
    {
        public User currentUser;

        private readonly NinjaHeavenDbContext _context;

        public UserController(NinjaHeavenDbContext context)
        {
            _context = context;
        }

        // GET: /User/
        public async Task<IActionResult> Index()
        {
            var permission = await CheckUserPermission(admin: true);
            if (!permission)
            {
                return RedirectToAction("Index", "Home");
            }

            var userList = await _context.User.ToListAsync();
            foreach (var item in userList)
            {
                item.Password = EncryptionService.Decrypt(item.Password);
            }

            return View(userList);
        }

        // GET: /User/Info/1
        public async Task<IActionResult> Info(int? id)
        {
            var permission = await CheckUserPermission(userId: id);
            if (!permission)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = currentUser;
            if (id != null) {
                user = await _context.User.FirstOrDefaultAsync(m => m.Id == id);
            }

            if (user == null)
            {
                TempData["Error"] = "User not found!";
                return RedirectToAction("Index", "Home");
            }

            return View(user);
        }

        // GET: /User/Login/
        public IActionResult Login()
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                SetCurrentRole();
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: /User/Login/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.User.FirstOrDefaultAsync(m => m.Email == vm.Email);

                if (user == null)
                {
                    return RedirectToAction("SignUp", new { email = vm.Email });
                }
                else
                {
                    // 密碼錯誤
                    if (EncryptionService.Decrypt(user.Password) != vm.Password)
                    {
                        ModelState.AddModelError("Password", "Wrong password, please try again!");
                    }
                    else
                    {
                        HttpContext.Session.SetInt32("UserId", user.Id);
                        TempData["Role"] = user.Role;
                        TempData["Message"] = $"Hello, { user.Name }";
                        return RedirectToAction("Info", new { id = user.Id });
                    }
                }
            }
            return View(vm);
        }

        // GET: /User/SignUp
        public ActionResult SignUp(string email)
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                SetCurrentRole();
                return RedirectToAction("Index", "Home");
            }

            SignUpVM user = new SignUpVM
            {
                Email = email
            };
            return View(user);
        }

        // POST: /User/SignUp
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SignUpVM vm)
        {
            if (ModelState.IsValid)
            {
                var valid = true;

                var name = await _context.User.FirstOrDefaultAsync(m => m.Name == vm.Name);
                var email = await _context.User.FirstOrDefaultAsync(m => m.Email == vm.Email);

                if (name != null)
                {
                    valid = false;
                    ModelState.AddModelError("Name", $"A user named { vm.Name } already exists.");
                }
                if (email != null)
                {
                    valid = false;
                    ModelState.AddModelError("Email", $"Email { vm.Email } is already in use.");
                }
                if (vm.Password != vm.ConfirmPassword)
                {
                    valid = false;
                    ModelState.AddModelError("ConfirmPassword", "Please make sure your passwords match.");
                }

                if (valid)
                {
                    try
                    {
                        User user = new User
                        {
                            Name = vm.Name,
                            Email = vm.Email,
                            Password = EncryptionService.Encrypt(vm.Password),
                            Gender = vm.Gender,
                            Role = "Player",
                            Wallet = 1000,
                            CreatedDate = DateTime.UtcNow.AddHours(8),
                            UpdatedDate = DateTime.UtcNow.AddHours(8)
                        };
                        _context.Add(user);
                        await _context.SaveChangesAsync();

                        HttpContext.Session.SetInt32("UserId", user.Id);
                        TempData["Message"] = $"Welcome, { user.Name }";
                        TempData["Role"] = user.Role;

                        return RedirectToAction("Info", new { id = user.Id });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        TempData["Error"] = "Sign up failed";
                    }
                }
            }
            return View(vm);
        }

        // GET: /User/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permission = await CheckUserPermission(userId: id);
            if (!permission)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                TempData["Error"] = "User not found!";
                return RedirectToAction("Index", "Home");
            }

            EditInfoVM vm = new EditInfoVM
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Gender = user.Gender
            };

            TempData["Role"] = currentUser.Role;
            return View(vm);
        }

        // POST: /User/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditInfoVM vm)
        {
            if (id != vm.Id)
            {
                return NotFound();
            }

            var permission = await CheckUserPermission(userId: id);
            if (!permission)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await _context.User.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                TempData["Error"] = "User not found!";
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                var valid = true;

                var name = await _context.User.FirstOrDefaultAsync(m => m.Name == vm.Name && m.Id != id);
                var email = await _context.User.FirstOrDefaultAsync(m => m.Email == vm.Email && m.Id != id);

                if (name != null)
                {
                    valid = false;
                    ModelState.AddModelError("Name", $"A user named { vm.Name } already exists.");
                }
                if (email != null)
                {
                    valid = false;
                    ModelState.AddModelError("Email", $"Email { vm.Email } is already in use.");
                }

                // 判斷是否更新密碼
                if (!string.IsNullOrEmpty(vm.Password))
                {
                    if (vm.Password != vm.ConfirmPassword)
                    {
                        valid = false;
                        ModelState.AddModelError("ConfirmPassword", "Please make sure your passwords match.");
                    }
                    else
                    {
                        vm.Password = EncryptionService.Encrypt(vm.Password);
                    }
                }
                else
                {
                    vm.Password = user.Password;
                }
                

                if (valid)
                {
                    try
                    {
                        user.Name = vm.Name;
                        user.Email = vm.Email;
                        user.Password = vm.Password;
                        user.Gender = vm.Gender;

                        user.UpdatedDate = DateTime.UtcNow.AddHours(8);
                        _context.Update(user);
                        await _context.SaveChangesAsync();
                        TempData["Message"] = "Info has been edited.";
                        return RedirectToAction("Info", new { id = id });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        TempData["Error"] = "Edit failed";
                    }
                }
            }
            TempData["Role"] = user.Role;
            return View(vm);
        }

        // GET: /User/LogOut
        public ActionResult LogOut()
        {
            HttpContext.Session.Remove("UserId");
            TempData["Role"] = null;
            TempData["Message"] = "Bye Bye";
            return RedirectToAction("Index", "Home");
        }

        // 檢核 User 權限
        public async Task<bool> CheckUserPermission(int? userId = 0, bool admin = false)
        {
            var currentId = HttpContext.Session.GetInt32("UserId");
            if (currentId == null)
            {
                return false;
            }

            currentUser = await _context.User.FirstOrDefaultAsync(m => m.Id == currentId);
            TempData["Role"] = currentUser.Role;

            var permissionDenied = false;
            if (userId != 0 && userId != null)
            {
                // 判斷是否同一人
                if (userId != currentUser.Id && currentUser.Role != "Admin")
                {
                    permissionDenied = true;
                }
            }
            else if (admin && currentUser.Role != "Admin")
            {
                permissionDenied = true;
                
            }

            if (permissionDenied)
            {
                TempData["Error"] = "Permission denied!";
                return false;
            }

            return true;
        }

        // 換頁時重新設定 TempData
        public void SetCurrentRole()
        {
            var user = _context.User.FirstOrDefault(m => m.Id == HttpContext.Session.GetInt32("UserId"));
            TempData["Role"] = user.Role;
        }
    }
}
