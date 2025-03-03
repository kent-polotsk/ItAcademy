using GNA.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GNA.Services.Abstractions
{
    public interface IBookService
    {
        BookModel[] GetBooks();
        BookModel? GetBookById(int id);
    }
}
