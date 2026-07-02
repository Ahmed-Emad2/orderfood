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
        // 1. دالة عرض صفحة التعديل (تأخذ المعرّف وتجلب البيانات)
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var foodItem = await _context.FoodItems.FindAsync(id);
            if (foodItem == null)
            {
                return NotFound();
            }
            return View(foodItem);
        }

        // 2. دالة استقبال البيانات المعدلة وحفظها (أضفنا فيها RestaurantName)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ItemId,RestaurantName,Name,Components,Price,Discount")] FoodItem foodItem)
        {
            if (id != foodItem.ItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(foodItem);
                    await _context.SaveChangesAsync();
                }
                catch (Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException)
                {
                    if (!_context.FoodItems.Any(e => e.ItemId == foodItem.ItemId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                // العودة للوحة التحكم بعد نجاح التعديل
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
     
        // 1. شاشة إنشاء الحساب
        [HttpGet]
        public IActionResult Register()
        {
            return View("~/Views/FoodItems/Register.cshtml");
        }

        [HttpPost]
        public IActionResult Register(string username, string email, string password, string role)
        {
            return RedirectToAction("Login");
        }

        // 2. شاشة تسجيل الدخول
        [HttpGet]
        public IActionResult Login()
        {
            return View("~/Views/FoodItems/Login.cshtml");
        }

        [HttpPost]
        public IActionResult Login(string username, string password, string role)
        {
            // التحقق الصارم من الحساب المنشود
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password) && username.Trim() == "admin" && password == "123456")
            {
                return RedirectToAction("Index", new { role = role });
            }

            // إرسال رسالة الخطأ في حال كانت البيانات غير مسجلة
            ViewData["ErrorMessage"] = "Wrong username or password";
            return View("~/Views/FoodItems/Login.cshtml");
        }

    } 
}