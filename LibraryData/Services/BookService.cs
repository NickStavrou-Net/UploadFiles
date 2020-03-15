using LibraryData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraryData.Services
{
    public class BookService : IBookService
    {
        private readonly LibraryContext _context;

        public BookService(LibraryContext libraryContext) => _context = libraryContext;
        public void Add(Book newBook)
        {
            _context.Add(newBook);
            _context.SaveChanges();
        }

        public IEnumerable<Book> GetAllBooks() => _context.Books.ToList();

        public Book GetBook(int id) => _context.Books.FirstOrDefault(book => book.Id == id);

        //public IEnumerable<Book> GetByAuthor(string author) => _context.Books.Where(a => a.Author.Contains(author));

        public IEnumerable<Book> GetByTitle(string title) => _context.Books.Where(t => t.Title.Contains(title));
    }
}
