using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using orderfood.Models;
using System.Linq;
using System.Threading.Tasks;

namespace orderfood.Controllers
{
    public class FoodItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FoodItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. شاشة العرض الرئيسية + ميزة البحث (Search)


        public async Task<IActionResult> Index(string searchString, string role = "Admin")
        {
            ViewData["CurrentFilter"] = searchString;
            // نرسل الصلاحية الحالية للواجهة لكي تتحكم في ظهور الأزرار
            ViewData["CurrentRole"] = role;

            var foodItems = from f in _context.FoodItems select f;

            if (!String.IsNullOrEmpty(searchString))
            {
                foodItems = foodItems.Where(s => s.Name.Contains(searchString) || s.RestaurantName.Contains(searchString));
            }

            return View("~/Views/FoodItems/Index.cshtml", await foodItems.ToListAsync());
        }


        // 2. شاشة إضافة وجبة جديدة (GET)
        public IActionResult Create()
        {
            return View();
        }

        // شاشة إضافة وجبة جديدة (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemId,RestaurantName,Name,Components,Price,Discount")] FoodItem foodItem)
        {
            if (ModelState.IsValid)
            {
                // إضافة الوجبة الجديدة إلى قاعدة البيانات
                _context.Add(foodItem);
                await _context.SaveChangesAsync();

                // إعادة التوجيه لصفحة الجدول الرئيسية لرؤية النتيجة فوراً
                return RedirectToAction(nameof(Index));
            }
            return View(foodItem);
        }
        // 3. شاشة التعديل (GET)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var foodItem = await _context.FoodItems.FindAsync(id);
            if (foodItem == null) return NotFound();
            return View(foodItem);
        }

        // شاشة التعديل (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ItemId,Name,Components,Price,Discount")] FoodItem foodItem)
        {
            if (id != foodItem.ItemId) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(foodItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(foodItem);
        }

        // 4. حذف وجبة )
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var foodItem = await _context.FoodItems.FindAsync(id);
            if (foodItem != null)
            {
                _context.FoodItems.Remove(foodItem);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}