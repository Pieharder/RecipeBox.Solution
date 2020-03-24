using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RecipeBox.Models;
using System.Collections.Generic;
using System.Linq;

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

    public ActionResult Create()
    {
      ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name");
      return View();
    }

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

    public ActionResult Edit(int id)
    {
      Category thisCategory = _db.Categories.FirstOrDefault(category => category.CategoryId == id);
      ViewBag.CategoryId = new SelectList(_db.Recipes, "RecipeId", "Name");
      return View(thisCategory);
    }

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

    public ActionResult AddRecipe(int id)
    {
      Category thisCategory = _db.Categories.FirstOrDefault(categories => categories.CategoryId == id);
      ViewBag.RecipeId = new SelectList(_db.Recipes, "RecipeId", "Name");
      return View(thisCategory);
    }

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

    // public ActionResult Delete(int id)
    // {
    //   Category thisCategory = _db.Categories.FirstOrDefault(patient => patient.CategoryId == id);
    //   return View(thisCategory);
    // }
    public ActionResult Delete(int id)
    {
      var thisCategory = _db.Categories.FirstOrDefault(category => category.CategoryId == id);
      return View(thisCategory);
      // var joinEntry = _db.CategoryRecipe.FirstOrDefault(entry => entry.CategoryRecipeId == id);
      // _db.CategoryRecipe.Remove(joinEntry);
      // _db.SaveChanges();
      // return RedirectToAction("Index");
    }
    
    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      var thisCategory = _db.Categories.FirstOrDefault(category => category.CategoryId == id);
      _db.Categories.Remove(thisCategory);
      _db.SaveChanges();
      return RedirectToAction("Index");
      // Category thisCategory = _db.Categories.FirstOrDefault(patients => patients.CategoryId == id);
      // List<CategoryRecipe> categoryRecipes = _db.CategoryRecipe.Where(recipeCategory => recipeCategory.CategoryId == id).ToList();
      // foreach(CategoryRecipe dp in categoryRecipes)
      // {
      //   _db.CategoryRecipe.Remove(dp);
      // }
      // _db.Categories.Remove(thisCategory);
      // _db.SaveChanges();
      // return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult Search(string search)
    {
      List<Category> model = _db.Categories.Where(category => (category.Name.Contains(search))).ToList();
      return View(model);
    }


  }
}