using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RecipeBox.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace RecipeBox.Controllers
{
  public class CategoriesController : Controller
  {
    private readonly RecipeBoxContext _db;

    public CategoriesController(RecipeBoxContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      return View(_db.Categories.ToList());
    }

    [Authorize]
    public ActionResult Create()
    {
      ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name");
      return View();
    }

    [Authorize]
    [HttpPost]
    public ActionResult Create(Category category, int RecipeId)
    {
      _db.Categories.Add(category);
      if(RecipeId != 0)
      {
        _db.CategoryRecipe.Add(new CategoryRecipe() { RecipeId = RecipeId, CategoryId = category.CategoryId});
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      Category thisCategory = _db.Categories
        .Include(category => category.Recipes)
        .ThenInclude(join => join.Recipe)
        .FirstOrDefault(category => category.CategoryId == id);
      return View(thisCategory);
    }

    [Authorize]
    public ActionResult Edit(int id)
    {
      Category thisCategory = _db.Categories.FirstOrDefault(category => category.CategoryId == id);
      ViewBag.CategoryId = new SelectList(_db.Recipes, "RecipeId", "Name");
      return View(thisCategory);
    }

    [Authorize]
    [HttpPost]
    public ActionResult Edit(Category category, int RecipeId)
    {
      if(RecipeId != 0)
      {
        _db.CategoryRecipe.Add(new CategoryRecipe() { RecipeId = RecipeId, CategoryId = category.CategoryId });
      }
      _db.Entry(category).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [Authorize]
    public ActionResult AddRecipe(int id)
    {
      Category thisCategory = _db.Categories.FirstOrDefault(categories => categories.CategoryId == id);
      ViewBag.RecipeId = new SelectList(_db.Recipes, "RecipeId", "Name");
      return View(thisCategory);
    }

    [Authorize]
    [HttpPost]
    public ActionResult AddRecipe(Category category, int RecipeId)
    {
      if(RecipeId != 0)
      {
        _db.CategoryRecipe.Add(new CategoryRecipe() { RecipeId = RecipeId, CategoryId = category.CategoryId });
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [Authorize]
    public ActionResult Delete(int id)
    {
      var thisCategory = _db.Categories.FirstOrDefault(category => category.CategoryId == id);
      return View(thisCategory);
    }
    
    [Authorize]
    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      var thisCategory = _db.Categories.FirstOrDefault(category => category.CategoryId == id);
      _db.Categories.Remove(thisCategory);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [Authorize]
    [HttpPost]
    public ActionResult Search(string search)
    {
      List<Category> model = _db.Categories.Where(category => (category.Name.Contains(search))).ToList();
      return View(model);
    }


  }
}