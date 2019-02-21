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
using Parsed.Services;
using System.IO;
using Parsed.Models.CompanyViewModel;

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
        public async Task<IActionResult> Create([Bind("ID,Title,CNPJ,DigitalCertificate")] EditViewModel newCompany)
        {
            if (ModelState.IsValid)
            {
                //Verifica se o arquivo é maior que 28,6 MB
                if (newCompany.DigitalCertificate != null && newCompany.DigitalCertificate.Length > 30000000)
                {
                    ModelState.AddModelError(string.Empty, _localizer["FileMaxLength"].Value);
                    return View(newCompany);
                }

                //Verifica se o CNPJ é válido
                if (!ValidationService.ValidaCnpj(newCompany.CNPJ))
                {
                    ModelState.AddModelError(string.Empty, _localizer["CNPJInvalid"].Value);
                    return View(newCompany);
                }

                //Verifica se existe alguma empresa com o mesmo CNPJ
                if (_context.Company.Where(c => c.CNPJ == newCompany.CNPJ).Any())
                {
                    ModelState.AddModelError(string.Empty, _localizer["CNPJAlreadyExists"].Value);
                    return View(newCompany);
                }

                var UserID = _userManager.GetUserId(User);

                //Cria objeto Empresa do Entity Framework
                Company company = new Company() {
                    CNPJ = newCompany.CNPJ,
                    Title = newCompany.Title,
                    CreationDate = DateTime.Now,
                    CreatedByID = UserID
                };

                if (newCompany.DigitalCertificate != null && !string.IsNullOrEmpty(newCompany.DigitalCertificate.FileName))
                {
                    //copia o arquivo enviado para o objeto Empresa
                    using (var memoryStream = new MemoryStream())
                    {
                        await newCompany.DigitalCertificate.CopyToAsync(memoryStream);
                        company.DigitalCertificate = memoryStream.ToArray();
                        company.DigitalCertificateFileName = newCompany.DigitalCertificate.FileName;
                    }
                }

                //Salva a Empresa
                _context.Add(company);
                await _context.SaveChangesAsync();

                //Salva o Usuário logado como Administrador da Empresa.
                CompanyUser CompanyPermission = new CompanyUser();
                CompanyPermission.CompanyID = company.ID;
                CompanyPermission.UserID = UserID;
                CompanyPermission.Role = CompanyUserRole.Administrator;
                _context.Add(CompanyPermission);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(newCompany);
        }

        // GET: Companies/Edit/5
        public async Task<IActionResult> Edit(long? id)
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

            //Se o usuário não tiver permissão para acessar a Empresa em modo de edição
            if (!company.Users.Where(c => c.UserID == appUser.Id && c.Role == CompanyUserRole.Administrator).Any())
            {
                return NotFound();
            }

            EditViewModel edit = new EditViewModel()
            {
                ID = company.ID,
                Title = company.Title,
                CNPJ = company.CNPJ
            };

            return View(edit);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Bind("ID,Title,CNPJ,DigitalCertificate")]
        public async Task<IActionResult> Edit(long id, EditViewModel newCompany)
        {
            if (id != newCompany.ID)
            {
                return NotFound();
            }

            ApplicationUser appUser = await _userManager.GetUserAsync(User);
            var currentCompany = await _context.Company.Include(c => c.Users).SingleOrDefaultAsync(m => m.ID == id);

            //Se o usuário não tiver permissão para editar a Empresa
            if (!currentCompany.Users.Where(c => c.UserID == appUser.Id && c.Role == CompanyUserRole.Administrator).Any())
            {
                return NotFound();
            }

            //Verifica se o arquivo é maior que 28,6 MB
            if (newCompany.DigitalCertificate != null && newCompany.DigitalCertificate.Length > 30000000) 
            {
                ModelState.AddModelError(string.Empty, _localizer["FileMaxLength"].Value);
                return View(newCompany);
            }

            //Se o CNPJ atual for diferente do que está atualmente no banco de dados.
                if (currentCompany.CNPJ != newCompany.CNPJ)
            {
                //Verifica se o CNPJ é válido
                if (!ValidationService.ValidaCnpj(newCompany.CNPJ))
                {
                    ModelState.AddModelError(string.Empty, _localizer["CNPJInvalid"].Value);
                    return View(newCompany);
                }

                //Verifica se existe alguma empresa com o mesmo CNPJ
                if (_context.Company.Where(c => c.CNPJ == newCompany.CNPJ).Any())
                {
                    ModelState.AddModelError(string.Empty, _localizer["CNPJAlreadyExists"].Value);
                    return View(newCompany);
                }
            }

            //passa os valores para o item baixados pra nãr erro de tracking
            currentCompany.Title = newCompany.Title;
            currentCompany.CNPJ = newCompany.CNPJ;

            if (ModelState.IsValid)
            {
                try
                {
                    if (newCompany.DigitalCertificate != null && !string.IsNullOrEmpty(newCompany.DigitalCertificate.FileName))
                    {
                        //copia o arquivo enviado para o objeto Empresa
                        using (var memoryStream = new MemoryStream())
                        {
                            await newCompany.DigitalCertificate.CopyToAsync(memoryStream);
                            currentCompany.DigitalCertificate = memoryStream.ToArray();
                            currentCompany.DigitalCertificateFileName = newCompany.DigitalCertificate.FileName;
                        }
                    }

                    _context.Update(currentCompany);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyExists(currentCompany.ID))
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
            return View(currentCompany);
        }

        // GET: Companies/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ApplicationUser appUser = await _userManager.GetUserAsync(User);
            var company = await _context.Company.Include(c => c.Users).SingleOrDefaultAsync(m => m.ID == id);

            if (company == null)
            {
                return NotFound();
            }

            //Se o usuário não tiver permissão para deletar a Empresa
            if (!company.Users.Where(c => c.UserID == appUser.Id && c.Role == CompanyUserRole.Administrator).Any())
            {
                return NotFound();
            }

            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            ApplicationUser appUser = await _userManager.GetUserAsync(User);
            var company = await _context.Company.Include(c => c.Users).SingleOrDefaultAsync(m => m.ID == id);

            //Se o usuário não tiver permissão para deletar a Empresa
            if (!company.Users.Where(c => c.UserID == appUser.Id && c.Role == CompanyUserRole.Administrator).Any())
            {
                return NotFound();
            }

            _context.Company.Remove(company);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> DownloadDigitalCertificate(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ApplicationUser appUser = await _userManager.GetUserAsync(User);
            var company = await _context.Company.Include(c => c.Users).SingleOrDefaultAsync(m => m.ID == id);

            if (company == null)
            {
                return NotFound();
            }

            //Se o usuário não tiver permissão para administrar a Empresa
            if (!company.Users.Where(c => c.UserID == appUser.Id && c.Role == CompanyUserRole.Administrator).Any())
            {
                return NotFound();
            }

            byte[] fileBytes = company.DigitalCertificate;
            string fileName = company.DigitalCertificateFileName;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        private bool CompanyExists(long id)
        {
            return _context.Company.Any(e => e.ID == id);
        }
    }

}
