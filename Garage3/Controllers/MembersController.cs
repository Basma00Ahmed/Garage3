using AutoMapper;
using Garage3.Data;
using Garage3.Models.Entities;
using Garage3.Models.ViewModels.Members;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Controllers
{
    public class MembersController : Controller
    {

        private readonly IMapper _mapper;
        private readonly Garage3Context _context;

        public MembersController(Garage3Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string PersonalNo, string FirstName, string LastName)
        {
            var member = _context.Member
                       .Where(member => (string.IsNullOrWhiteSpace(PersonalNo) || member.PersonalNo.Contains(PersonalNo)) &&
                      (string.IsNullOrWhiteSpace(FirstName) || member.FirstName.StartsWith(FirstName)) &&
                      (string.IsNullOrWhiteSpace(LastName) || member.LastName.StartsWith(LastName)));

            var model = _mapper.ProjectTo<MembersVehiclesViewModel>(member);
            if (model == null)
            {
                return NotFound();
            }
            var sortModel = await model.ToListAsync();

            return View(sortModel.OrderBy(m => m.FirstName.Substring(0, 2), StringComparer.Ordinal));
        }

        // GET: Members/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var member = await _mapper.ProjectTo<MembersVehiclesViewModel>(_context.Member)
                                       .FirstOrDefaultAsync(m => m.Id == id);
            if (member == null)
            {
                return NotFound();
            }
            return View(member);
        }

        // GET: Members/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PersonalNo,FirstName,LastName")] Member member)
        {
            if (ModelState.IsValid)
            {
                _context.Add(member);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        // GET: Members/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Member.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PersonalNo,FirstName,LastName")] Member member)
        {
            if (id != member.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(member);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(member.Id))
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
            return View(member);
        }

        // GET: Members/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Member.FirstOrDefaultAsync(m => m.Id == id);
            
            if (member == null)
            {
                return NotFound();
            }
            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var member = await _context.Member.FindAsync(id);
            
            _context.Member.Remove(member);
            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }

        private bool MemberExists(int id)
        {
            return _context.Member.Any(e => e.Id == id);
        }

        public IActionResult VerifyPersonalNo(string personalNo)
        {
            bool PersonalNoExists = _context.Member.Any(m => m.PersonalNo == personalNo);
            if (PersonalNoExists)
            {
                return Json($"A Member with personl number {personalNo} already exists.");
            }
            return Json(true);
        }

    }
}
