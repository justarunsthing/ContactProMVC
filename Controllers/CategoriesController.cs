﻿using ContactProMVC.Data;
using ContactProMVC.Models;
using Microsoft.AspNetCore.Mvc;
using ContactProMVC.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace ContactProMVC.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailSender _emailSender;

        public CategoriesController(ApplicationDbContext context, UserManager<AppUser> userManager, IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        // GET: Categories
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var appUserId = _userManager.GetUserId(User);
            var categories = await _context.Categories
                                     .Where(c => c.AppUserId == appUserId)
                                     .Include(c => c.AppUser)
                                     .ToListAsync();

            return View(categories);
        }

        [Authorize]
        public async Task<IActionResult> EmailCategory(int id)
        {
            var appUserId = _userManager.GetUserId(User);
            var category = await _context.Categories
                                         .Include(c => c.Contacts)
                                         .FirstOrDefaultAsync(c => c.Id == id && c.AppUserId == appUserId);

            List<string> emails = category.Contacts.Select(c => c.Email).ToList();

            var emailData = new EmailData
            {
                GroupName = category.Name,
                EmailAddress = string.Join(";", emails),
                Subject = $"Group Message: {category.Name}"
            };

            var model = new EmailCategoryViewModel
            {
                Contacts = category.Contacts.ToList(),
                EmailData = emailData
            };

            return View(model);
        }

        // GET: Categories/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.AppUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AppUserId,Name")] Category category)
        {
            ModelState.Remove("AppUserId");

            if (ModelState.IsValid)
            {
                var appUserId = _userManager.GetUserId(User);
                category.AppUserId = appUserId;

                _context.Add(category);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        // GET: Categories/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appUserId = _userManager.GetUserId(User);
            var category = await _context.Categories.Where(c => c.Id == id && c.AppUserId == appUserId)
                                                    .FirstOrDefaultAsync();

            if (category == null)
            {
                return NotFound();
            }

            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", category.AppUserId);
            
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AppUserId,Name")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var appUserId = _userManager.GetUserId(User);
                    category.AppUserId = appUserId;

                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
            
            return View(category);
        }

        // GET: Categories/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.AppUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
