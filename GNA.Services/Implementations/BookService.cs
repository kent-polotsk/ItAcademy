using GNA.Core;
using GNA.Services.Abstractions;

namespace GNA.Services.Implementations

{
    public class BookService : IBookService
    {
        private readonly Dictionary<int, BookModel> _data;
        private readonly IAuthorsService _authorsService;
        private const string Description = 
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent ligula nibh, vulputate id accumsan a, tristique id est. Pellentesque volutpat dolor eros, vitae dictum lorem dignissim non. Ut sodales lacus ligula, id sagittis massa dapibus at. Vivamus tincidunt tristique arcu, vulputate fermentum augue porttitor in. Quisque consectetur neque in purus volutpat, nec condimentum magna hendrerit. Fusce vel metus in augue auctor lobortis. Nunc eu tristique lacus, quis egestas enim. Mauris consequat et turpis vel aliquam. In sed dui odio. Nullam vel dictum arcu.\r\n\r\nCras ac arcu mauris. Vestibulum tristique, tortor sit amet fringilla mollis, massa nulla iaculis orci, feugiat tempor ante tellus convallis quam. Suspendisse velit elit, egestas id massa ac, elementum congue erat. Donec neque arcu, mattis malesuada viverra in, dapibus eget nulla. Suspendisse dignissim sit amet est at congue. Morbi id felis in mi pulvinar sodales ullamcorper et mauris. Nunc malesuada, lorem eget aliquam sagittis, sapien augue viverra ligula, a interdum orci tortor ut purus. Suspendisse at lectus eget libero venenatis fermentum nec ac velit. Interdum et malesuada fames ac ante ipsum primis in faucibus. Fusce at rhoncus erat. Donec vitae ullamcorper metus. Morbi ut facilisis diam. Proin volutpat mollis rhoncus. Praesent ut dignissim sapien.\r\n\r\nDuis gravida bibendum maximus. Praesent et molestie odio. Aenean sit amet blandit erat. Etiam semper malesuada libero eu dignissim. Fusce malesuada a mi eget pulvinar. Donec venenatis eget tortor eget commodo. Nulla facilisi. Vivamus nec felis risus.\r\n\r\nQuisque volutpat, ipsum ac placerat cursus, augue purus iaculis massa, quis euismod felis nunc et velit. In a ligula lacus. Nulla eros mi, eleifend non quam vel, scelerisque tempus libero. Pellentesque et egestas arcu, et egestas eros. Phasellus posuere tempor viverra. Curabitur a turpis at enim mollis dignissim eget pharetra dui. Duis sed laoreet massa. Vestibulum nec malesuada diam. Nulla facilisi.\r\n\r\nAliquam quam ante, pharetra sed quam at, imperdiet cursus nisi. Aenean laoreet pharetra mi. Vestibulum arcu velit, placerat at consectetur id, facilisis nec odio. Suspendisse nec pretium libero. Curabitur vel enim nec mi pharetra tincidunt eget at diam. Integer sit amet placerat neque. Praesent malesuada efficitur dui, sed eleifend urna congue eu. Nulla vitae tellus egestas, auctor lorem vitae, ultrices ex. Interdum et malesuada fames ac ante ipsum primis in faucibus. Mauris vestibulum urna non felis feugiat, sit amet pellentesque neque eleifend. Quisque tellus sapien, varius a finibus ut, viverra eget dolor. Donec vitae nisi eget velit scelerisque eleifend. Morbi et risus ex.";

        public BookService(IAuthorsService authorsService)
        {
            _authorsService = authorsService;
            _data = new Dictionary<int, BookModel>
            {
                { 1, new BookModel {Id=1,Title = "SampleTitle1", Description = Description, Author = "Author1" } },
                { 2, new BookModel {Id=2,Title = "SampleTitle2", Description = Description, Author = "Author2" } },
                { 3, new BookModel {Id=3,Title = "SampleTitle3", Description = Description, Author = "Author3" } },
                { 4, new BookModel {Id=4,Title = "SampleTitle4", Description = Description, Author = "Author4" } },
                { 5, new BookModel {Id=5,Title = "SampleTitle5", Description = Description, Author = "Author5" } },
                { 6, new BookModel {Id=6,Title = "SampleTitle6", Description = Description, Author = "Author6" } },
                { 7, new BookModel {Id=7,Title = "SampleTitle7", Description = Description, Author = "Author8" } },
                { 8, new BookModel {Id=8,Title = "SampleTitle8", Description = Description, Author = "Author8" } },
                { 9, new BookModel {Id=9,Title = "SampleTitle9", Description = Description, Author = "Author9" } },
            }; ;
        }

        public BookModel? GetBookById(int id)
        {
            return _data.ContainsKey(id)
                ?_data[id]
                :null;
        }

        public BookModel[] GetBooks()
        {
            return _data.Values.ToArray();
        }
    }
}
