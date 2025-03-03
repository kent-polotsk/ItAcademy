using GNA.Core;
using GNA.Services.Abstractions;
using GNA.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using WebAppGNAggregator.Models;

namespace WebAppGNAggregator.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        public IActionResult Index()
        {
            var books = _bookService.GetBooks();
            return View(books);
        }

        public IActionResult BookPreview(BookModel bookModel)
        { 
            return PartialView(bookModel);
        }

        public IActionResult Details(int id)
        {
            var book = _bookService.GetBookById(id);
            if (book != null)
            {
                return View(book);
            }
            return NotFound();
        }
        public IActionResult AddNew(int id)
        {
            return View();
        }

    }
}
