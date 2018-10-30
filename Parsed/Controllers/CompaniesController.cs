using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Microsoft.EntityFrameworkCore;
using Parsed.Data;
using Parsed.Models;

namespace Parsed.Controllers
{
    
    public class CompaniesController : Controller
    {
        private readonly IStringLocalizer<CompaniesController> _localizer;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CompaniesController(ApplicationDbContext context, 
            UserManager<ApplicationUser> userManager,
            IStringLocalizer<CompaniesController> localizer)
        {
            _context = context;
            _userManager = userManager;
            _localizer = localizer;
        }

        // GET: Companies
        public async Task<IActionResult> Index()
        {
            //Lista somente as empresas do usuário logado.
            ApplicationUser appUser = await _userManager.GetUserAsync(User);
            List<Company> companies = await _context.Company.Include(c => c.Users).ToListAsync();
            companies = companies.Where(c => c.Users.Where(cu => cu.User == appUser).Any()).ToList();

            return View(companies);
        }

        // GET: Companies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Company
                .SingleOrDefaultAsync(m => m.ID == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // GET: Companies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Title,CNPJ,DigitalCertificate")] Company company)
        {
            if (ModelState.IsValid)
            {
                //Verifica se existe alguma empresa com o mesmo CNPJ
                if (_context.Company.Where(c => c.CNPJ == company.CNPJ).Any())
                {
                    ModelState.AddModelError(string.Empty, _localizer["CNPJAlreadyExists"].Value);
                    return View(company);
                }

                //Salva a Empresa
                _context.Add(company);
                await _context.SaveChangesAsync();

                //Salva o Usuário logado como Administrador da Empresa.
                CompanyUser CompanyPermission = new CompanyUser();
                CompanyPermission.CompanyID = company.ID;
                CompanyPermission.UserID = _userManager.GetUserId(User);
                CompanyPermission.Role = CompanyUserRole.Administrator;
                _context.Add(CompanyPermission);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        // GET: Companies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ApplicationUser appUser = await _userManager.GetUserAsync(User);

            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Company.Include(c => c.Users).SingleOrDefaultAsync(m => m.ID == id);
            if (company == null)
            {
                return NotFound();
            }

            //Se o usuário não tiver permissão para acessar a Empresa
            if (!company.Users.Where(c => c.UserID == appUser.Id).Any())
            {
                return NotFound();
            }

            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,CNPJ,DigitalCertificate")] Company company)
        {
            if (id != company.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(company);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyExists(company.ID))
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
            return View(company);
        }

        // GET: Companies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Company
                .SingleOrDefaultAsync(m => m.ID == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var company = await _context.Company.SingleOrDefaultAsync(m => m.ID == id);
            _context.Company.Remove(company);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompanyExists(int id)
        {
            return _context.Company.Any(e => e.ID == id);
        }
    }

}
