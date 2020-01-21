﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using IssueTrackerV1.Models;
using IssueTrackerV1.ViewModels;

namespace IssueTrackerV1.Controllers
{
    public class IssueController : Controller
    {
        private ApplicationDbContext _context;

        public IssueController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }


        public ViewResult Index()
        {
            var issues = _context.Issues.Include(m => m.Genre).ToList();


            return View(issues);
        }

        public ViewResult New()
        {
            var genres = _context.Genres.ToList();

            var viewModel = new IssueFormViewModel
            {
                Genres = genres
            };

            return View("IssueForm", viewModel);
        }

        public ActionResult Edit(int id)
        {
            var issue = _context.Issues.SingleOrDefault(c => c.Id == id);

            if (issue == null)
                return HttpNotFound();

            var viewModel = new IssueFormViewModel
            {
                Issue = issue,
                Genres = _context.Genres.ToList()
            };

            return View("IssueForm", viewModel);
        }

        [HttpPost]
        public ActionResult Save(Issue issue)
        {
            if (issue.Id == 0)
            {
                issue.DateAdded = DateTime.Now;
                _context.Issues.Add(issue);
            }
            else
            {
                var issueInDb = _context.Issues.Single(m => m.Id == issue.Id);
                issueInDb.Name = issue.Name;
                issueInDb.GenreId = issue.GenreId;
                issueInDb.NumberInStack = issue.NumberInStack;
                issueInDb.ReleaseDate = issue.ReleaseDate;
            }

            _context.SaveChanges();

            return RedirectToAction("Index", "Issue");
        }

        public ActionResult Details(int id)
        {
            var issue = _context.Issues.Include(m => m.Genre).SingleOrDefault(m => m.Id == id);

            if (issue == null)
            {
                return HttpNotFound();
            }

            return View(issue);

        }
    }
}