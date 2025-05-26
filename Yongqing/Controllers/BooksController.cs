using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebApplication1.Interfaces;
using Yongqing.Database;
using Yongqing.ViewModels;

namespace WebApplication1.Controllers; 
public class BooksController : Controller
{
    private readonly IBooksService _bookService;

    public BooksController(IBooksService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> IndexAsync()
    {
        IReadOnlyList<BooksViewModel>? books = default;

        try
        {
            var data = await _bookService.GetAllAsync();
            books = data.Select(m => new BooksViewModel
            {
                Id = m.Id,
                Author = m.Author,
                PublishDate = m.PublishDate,
                Title = m.Title,
                Price = m.Price,
            }).ToList();

            if (TempData["ViewResult"] is string viewResult)
            {
                var parts = viewResult.Split('|');
                ViewData["ViewResult"] = (Convert.ToInt32(parts[0]), parts[1]);
            }

            return View(books);
        }
        catch
        {
            ViewData["ViewResult"] = (1, "Index 發生例外狀況");
            return View(books);
        }
    }

    [HttpGet]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> DetailsAsync(int id)
    {
        BooksViewModel? book = default;

        try
        {
            var data = await _bookService.GetByIdAsync(id);
            if (data == null)
            {
                ViewData["ViewResult"] = (1, "找不到資料");
                return View(book);
            }
            book = new BooksViewModel
            {
                Id = data.Id,
                Author = data.Author,
                PublishDate = data.PublishDate,
                Title = data.Title,
                Price = data.Price,
            };

            return View(book);
        }
        catch
        {
            ViewData["ViewResult"] = (1, "Details 發生例外狀況");
            return View(book);
        }
    }

    [HttpGet]
    [AutoValidateAntiforgeryToken]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAsync(BooksViewModel book)
    {
        try
        {
            if (ModelState.IsValid)
            {
                if(await _bookService.CreateAsync(new Books
                {
                    Author = book.Author,
                    PublishDate = book.PublishDate,
                    Title = book.Title,
                    Price = book.Price,
                }))
                {
                    TempData["ViewResult"] = "0|新增成功！";
                    return RedirectToAction("Index");
                }
            }

            return View(book);
        }
        catch
        {
            ViewData["ViewResult"] = (1, "Create 發生例外狀況");
            return View(book);
        }
    }

    [HttpGet]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> EditAsync(int id)
    {
        BooksViewModel? book = null;

        try
        {
            var data = await _bookService.GetByIdAsync(id);
            if (data == null)
            {
                ViewData["ViewResult"] = (1, "找不到資料");
                return View(book);
            }
            book = new BooksViewModel
            {
                Id = data.Id,
                Author = data.Author,
                PublishDate = data.PublishDate,
                Title = data.Title,
                Price = data.Price,
            };
            HttpContext.Session.SetString("EditId", book.Id.ToString());

            return View(book);
        }
        catch
        {
            ViewData["ViewResult"] = (1, "Edit 發生例外狀況");
            return View(book);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditAsync(BooksViewModel book)
    {
        try
        {
            if (int.TryParse(HttpContext.Session.GetString("EditId"), out int id) && id != book.Id)
            {
                ViewData["ViewResult"] = (1, "找不到資料");
                return View(book);
            }

            if (ModelState.IsValid)
            {
                var data = await _bookService.GetByIdAsync(book.Id);
                if (data == null)
                {
                    ViewData["ViewResult"] = (1, "找不到資料");
                    return View(book);
                }

                if (
                    data.Author == book.Author &&
                    data.PublishDate == book.PublishDate &&
                    data.Title == book.Title &&
                    data.Price == book.Price
                )
                {
                    ViewData["ViewResult"] = (0, "資料無異動");
                    return View(book);
                }

                if (await _bookService.UpdateAsync(new Books
                {
                    Id = book.Id,
                    Author = book.Author,
                    PublishDate = book.PublishDate,
                    Title = book.Title,
                    Price = book.Price,
                }))
                {
                    ViewData["ViewResult"] = (0, "修改成功！");
                    return View(book);
                }

                ViewData["ViewResult"] = (1, "修改失敗！");
            }

            return View(book);
        }
        catch
        {
            ViewData["ViewResult"] = (1, "Edit 發生例外狀況");
            return View(book);
        }
        finally
        {
            HttpContext.Session.SetString("EditId", string.Empty);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        try
        {
            if (await _bookService.DeleteAsync(id))
                return Json(new { Code = 0, Message = "刪除成功！" });

            return Json(new { Code = 1, Message = "刪除失敗！" });
        }
        catch
        {
            return Json(new { Code = 1, Message = "Delete 發生例外狀況" });
        }
    }
}
