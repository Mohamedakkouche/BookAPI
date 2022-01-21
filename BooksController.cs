using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookAPI.Models;
using BookAPI.Repositories;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : Controller
    {
        private readonly IBookRepository _bookRepository;
        // GET: /<controller>/
        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IEnumerable<Bookie>> GetBooks()
        {
            return await _bookRepository.Get();
        }

        [HttpGet("id")]
        public async Task<ActionResult<Bookie>> GetBooks(int id)
        {
            return await _bookRepository.Get(id);
        }
        [HttpPost]
        public async Task<ActionResult<Bookie>> PostBooks([FromBody] Bookie book)
        {
            var newBook = await _bookRepository.Create(book);
            return CreatedAtAction(nameof(GetBooks), new { id = newBook.ID }, newBook);

        }
        [HttpPut]
        public async Task<ActionResult> updateBook(int id, [FromBody] Bookie book)
        {
            if(id != book.ID)
            {
                return BadRequest();
            }
            await _bookRepository.Update(book);
            return NoContent();
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            var bookToDelete = await _bookRepository.Get(id);
            if(bookToDelete == null)
            {
                return NotFound();
            }
            await _bookRepository.Delete(bookToDelete.ID);
            return NoContent();
        }
    }
}
