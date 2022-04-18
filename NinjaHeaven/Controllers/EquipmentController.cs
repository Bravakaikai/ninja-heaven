using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NinjaHeaven.Models;

namespace NinjaHeaven.Controllers
{
    public class EquipmentController : Controller
    {
        private readonly NinjaHeavenDbContext _context;

        public EquipmentController(NinjaHeavenDbContext context)
        {
            _context = context;
        }

        // GET: /Equipment/
        public async Task<IActionResult> Index()
        {
            var permission = await CheckUserPermission();
            if (!permission)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(await _context.Equipment.ToListAsync());
        }

        // GET: /Equipment/Create
        public async Task<IActionResult> Create()
        {
            var permission = await CheckUserPermission();
            if (!permission)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: /Equipment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Equipment eq)
        {
            var permission = await CheckUserPermission();
            if (!permission)
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                var valid = true;

                var name = await _context.Equipment.FirstOrDefaultAsync(m => m.Name == eq.Name);
                var imgUrl = await _context.Equipment.FirstOrDefaultAsync(m => m.ImgUrl == eq.ImgUrl);

                if (name != null)
                {
                    valid = false;
                    ModelState.AddModelError("Name", $"An equipment named { eq.Name } already exists.");
                }
                if (imgUrl != null)
                {
                    valid = false;
                    ModelState.AddModelError("ImgUrl", $"Image url is already in use.");
                }

                if (valid)
                {
                    try
                    {
                        eq.CreatedDate = DateTime.UtcNow.AddHours(8);
                        eq.UpdatedDate = DateTime.UtcNow.AddHours(8);
                        _context.Add(eq);
                        await _context.SaveChangesAsync();
                        TempData["Message"] = "Your equipment has been created.";
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        TempData["Error"] = "Create failed!";
                    }
                    
                    return RedirectToAction("Index");
                }
            }
            return View(eq);
        }

        // GET: /Equipment/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            var equipment = await _context.Equipment.FindAsync(id);
            if (equipment == null)
            {
                TempData["Error"] = "Equipment not found!";
                return RedirectToAction("Index");
            }

            var permission = await CheckUserPermission();
            if (!permission)
            {
                return RedirectToAction("Index", "Home");
            }

            Equipment model = new Equipment
            {
                Id = equipment.Id,
                Name = equipment.Name,
                Description = equipment.Description,
                Price = equipment.Price,
                ImgUrl = equipment.ImgUrl,
                CreatedDate = equipment.CreatedDate
            };
            return View(model);
        }

        // POST: /Equipment/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Equipment eq)
        {
            if (id != eq.Id)
            {
                TempData["Error"] = "Equipment not found!";
                return RedirectToAction("Index");
            }

            var permission = await CheckUserPermission();
            if (!permission)
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                var valid = true;

                var name = await _context.Equipment.FirstOrDefaultAsync(m => m.Name == eq.Name && m.Id != eq.Id);
                var imgUrl = await _context.Equipment.FirstOrDefaultAsync(m => m.ImgUrl == eq.ImgUrl && m.Id != eq.Id);

                if (name != null)
                {
                    valid = false;
                    ModelState.AddModelError("Name", $"An equipment named { eq.Name } already exists.");
                }
                if (imgUrl != null)
                {
                    valid = false;
                    ModelState.AddModelError("ImgUrl", $"Image url is already in use.");
                }

                if (valid)
                {
                    try
                    {
                        eq.UpdatedDate = DateTime.UtcNow.AddHours(8);
                        _context.Update(eq);
                        await _context.SaveChangesAsync();
                        TempData["Message"] = "Your equipment has been edited.";
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        TempData["Error"] = "Edit failed!";
                    }
                    return RedirectToAction("Index");
                }
            }

            return View(eq);
        }

        // GET: /Equipment/Delete
        public async Task<IActionResult> Delete(int? id)
        {
            var permission = await CheckUserPermission();
            if (!permission)
            {
                return RedirectToAction("Index", "Home");
            }

            var equipment = await _context.Equipment.FirstOrDefaultAsync(m => m.Id == id);
            if (equipment == null)
            {
                TempData["Error"] = "Equipment not found!";
                return RedirectToAction("Index");
            }

            return View(equipment);
        }

        // Post: /Equipment/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var permission = await CheckUserPermission();
            if (!permission)
            {
                return RedirectToAction("Index", "Home");
            }

            var equipment = await _context.Equipment.FirstOrDefaultAsync(m => m.Id == id);
            if (equipment == null)
            {
                TempData["Error"] = "Equipment not found!";
            }

            try
            {
                _context.Remove(equipment);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Your equipment has been removed.";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                TempData["Error"] = "Delete failed";
                return View(equipment);
            }
            return RedirectToAction("Index");

        }

        // 檢核 User 權限
        public async Task<bool> CheckUserPermission()
        {
            var currentId = HttpContext.Session.GetInt32("UserId");
            if (currentId == null)
            {
                return false;
            }

            var currentUser = await _context.User.FirstOrDefaultAsync(m => m.Id == currentId);
            TempData["Role"] = currentUser.Role;

            if (currentUser.Role != "Admin")
            {
                TempData["Error"] = "Permission denied!";
                return false;
            }

            return true;
        }
    }
}
