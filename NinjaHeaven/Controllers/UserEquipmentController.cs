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
    public class UserEquipmentController : Controller
    {
        public User currentUser;

        private readonly NinjaHeavenDbContext _context;

        public UserEquipmentController(NinjaHeavenDbContext context)
        {
            _context = context;
        }

        // GET: /UserEquipment/
        public async Task<IActionResult> Index()
        {
            var permission = await CheckUserPermission();
            if (!permission)
            {
                return RedirectToAction("Login", "User");
            }

            var userEquipmentList = await _context.UserEquipment
                .Where(w => w.UserId == HttpContext.Session.GetInt32("UserId"))
                .OrderByDescending(o => o.UpdatedDate)
                .ToListAsync();

            foreach (var item in userEquipmentList)
            {
                var equipment = _context.Equipment.FirstOrDefault(m => m.Id == item.EquipmentId);
                item.Equipment = equipment;
            }
            return View(userEquipmentList);
        }

        // GET: /UserEquipment/Shop/
        public async Task<IActionResult> Shop()
        {
            var permission = await CheckUserPermission();
            if (!permission)
            {
                return RedirectToAction("Login", "User");
            }

            return View(await _context.Equipment.ToListAsync());
        }

        // GET: /UserEquipment/Buy/
        public async Task<IActionResult> Buy(int? id)
        {
            var permission = await CheckUserPermission();
            if (!permission)
            {
                return RedirectToAction("Login", "User");
            }

            var equipment = await _context.Equipment.FindAsync(id);
            if (equipment == null)
            {
                TempData["Error"] = "Equipment not found!";
                return RedirectToAction("Shop");
            }

            UserEquipment model = new UserEquipment
            {
                UserId = currentUser.Id,
                EquipmentId = (int)id,
                User = currentUser,
                Equipment = equipment,
                Amount = 1
            };
            return View(model);
        }

        // POST: /UserEquipment/Buy
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Buy(int id, UserEquipment vm)
        {
            var permission = await CheckUserPermission(vm.UserId);
            if (!permission)
            {
                return RedirectToAction("Index", "Home");
            }

            var equipment = await _context.Equipment.FindAsync(id);
            if (equipment == null)
            {
                TempData["Error"] = "Equipment not found!";
                return RedirectToAction("Shop");
            }


            if (ModelState.IsValid)
            {
                var user = _context.User.FirstOrDefault(m => m.Id == vm.UserId);

                var cost = equipment.Price * vm.Amount;
                var balance = user.Wallet - cost;
                if (balance < 0)
                {
                    ModelState.AddModelError("Amount", "餘額不足");
                }
                else
                {
                    var userEquipment = _context.UserEquipment
                    .FirstOrDefault(m => m.UserId == vm.UserId && m.EquipmentId == id);

                    try
                    {
                        vm.User = user;
                        vm.Equipment = equipment;

                        // 初次購買
                        if (userEquipment == null)
                        {
                            vm.CreatedDate = DateTime.UtcNow.AddHours(8);
                            vm.UpdatedDate = DateTime.UtcNow.AddHours(8);
                            _context.Add(vm);
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            userEquipment.Amount = userEquipment.Amount + vm.Amount;
                            _context.Update(userEquipment);
                            await _context.SaveChangesAsync();
                        }

                        user.Wallet = balance;
                        _context.Update(user);
                        await _context.SaveChangesAsync();

                        TempData["Message"] = "Your purchase was successful!";
                        return RedirectToAction("Shop");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        TempData["Message"] = "Purchase failed";
                    }

                }
            }
            return View(vm);
        }

        // GET: /UserEquipment/Sell/
        public async Task<IActionResult> Sell(int? id)
        {
            var permission = await CheckUserPermission();
            if (!permission)
            {
                return RedirectToAction("Login", "User");
            }

            var equipment = await _context.Equipment.FindAsync(id);
            var userEquipment = await _context.UserEquipment.FirstOrDefaultAsync(m => m.UserId == currentUser.Id && m.EquipmentId == id);
            if (equipment == null || userEquipment == null)
            {
                TempData["Error"] = "Equipment not found!";
                return RedirectToAction("Info", "User");
            }

            UserEquipment model = new UserEquipment
            {
                UserId = currentUser.Id,
                EquipmentId = (int)id,
                User = currentUser,
                Equipment = equipment,
                Amount = userEquipment.Amount
            };
            return View(model);
        }

        // POST: /UserEquipment/Sell
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sell(int id, UserEquipment vm)
        {
            var permission = await CheckUserPermission();
            if (!permission)
            {
                return RedirectToAction("Login", "User");
            }

            var equipment = await _context.Equipment.FindAsync(id);
            var userEquipment = await _context.UserEquipment.FirstOrDefaultAsync(m => m.UserId == currentUser.Id && m.EquipmentId == id);
            if (equipment == null || userEquipment == null)
            {
                TempData["Error"] = "Equipment not found!";
                return RedirectToAction("Shop");
            }

            if (ModelState.IsValid)
            {
                var user = _context.User.FirstOrDefault(m => m.Id == currentUser.Id);

                if (vm.Amount > userEquipment.Amount)
                {
                    ModelState.AddModelError("Amount", "Out of stock");
                    vm.Amount = userEquipment.Amount;
                }
                else
                {
                    var cost = equipment.Price * vm.Amount;

                    userEquipment.Amount = userEquipment.Amount - vm.Amount;
                    _context.Update(userEquipment);
                    await _context.SaveChangesAsync();

                    user.Wallet = user.Wallet + cost;
                    _context.Update(user);
                    await _context.SaveChangesAsync();

                    TempData["Message"] = "Your equipment has been sold.";
                    return RedirectToAction("Info", "User");
                }
            }
            return View(vm);

        }

        // 檢核 User 權限
        public async Task<bool> CheckUserPermission(int? userId = 0)
        {
            var currentId = HttpContext.Session.GetInt32("UserId");
            if (currentId == null)
            {
                TempData["Error"] = "Please login";
                return false;
            }
            else
            {
                currentUser = await _context.User.FirstOrDefaultAsync(m => m.Id == currentId);
                TempData["Role"] = currentUser.Role;

                // 判斷是否同一人
                if (userId != 0 && userId != currentUser.Id)
                {
                    TempData["Error"] = "Permission denied!";
                    return false;
                }
            }

            return true;
        }
    }
}
