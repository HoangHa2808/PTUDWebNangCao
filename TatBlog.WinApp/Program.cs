using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;

namespace TatBlog.WinApp
{
	public class Program
	{
		static void Main(string[] args)
		{
			// Tạo đối tượng context để quản lý phiên làm việc
			var context = new BlogDbContext();


			InitDB(context);

			XuatDanhSachTacGia(context);
			XuatDanhSachBaiViet(context);
			// TimCacBaiVietDuocXemNhieuNhat(context, 3);
			// XuatDanhSachDanhMuc(context);
			// XuatDanhSachTheTheoPhanTrang(context);

			// Wait
			Console.ReadKey();
		}

		static void InitDB(BlogDbContext context)
		{
			// Tạo đối tượng khởi tạo dữ liệu mẫu
			var seeder = new DataSeeder(context);
			// Gọi hàm nhập dữ liệu mẫu
			seeder.Initialize();
		}

		static void XuatDanhSachTacGia(BlogDbContext context)
		{
			// Đọc danh sách tác giả từ CSDL
			var authors = context.Authors.ToList();

			// Xuất danh sách tác giả ra màn hình
			Console.WriteLine("{0,-4}{1,-30}{2,-30}{3,12}", "ID", "Full Name", "Email", "Joined Date");
			foreach (var author in authors)
			{
				Console.WriteLine("{0,-4}{1,-30}{2,-30}{3,12:MM/dd/yyyy}",
					author.Id, author.FullName, author.Email, author.JoinedDate);
			}
		}

		static void XuatDanhSachBaiViet(BlogDbContext context)
		{
			var posts = context.Posts.Where(p => p.Published).OrderBy(p => p.Title).Select(p => new
			{
				Id = p.Id,
				Title = p.Title,
				ViewCount = p.ViewCount,
				PostedDate = p.PostedDate,
				Author = p.Author.FullName,
				Category = p.Category.Name
			}).ToList();

			foreach (var post in posts)
			{
				Console.WriteLine("ID      : {0}", post.Id);
				Console.WriteLine("Title   : {0}", post.Title);
				Console.WriteLine("View    : {0}", post.ViewCount);
				Console.WriteLine("Date    : {0:MM/dd/yyyy}", post.PostedDate);
				Console.WriteLine("Author  : {0}", post.Author);
				Console.WriteLine("Category: {0}", post.Category);
				Console.WriteLine("".PadRight(80, '-'));
			}
		}

		static async void TimCacBaiVietDuocXemNhieuNhat(BlogDbContext context, int numPost)
		{
			// Tạo đối tượng BlogRepository
			IBlogRepository blogRepo = new BlogRepository(context);

			var posts = await blogRepo.GetPopularArticlesAsync(numPost);

			foreach (var post in posts)
			{
				Console.WriteLine("ID      : {0}", post.Id);
				Console.WriteLine("Title   : {0}", post.Title);
				Console.WriteLine("View    : {0}", post.ViewCount);
				Console.WriteLine("Date    : {0:MM/dd/yyyy}", post.PostedDate);
				Console.WriteLine("Author  : {0}", post.Author);
				Console.WriteLine("Category: {0}", post.Category);
				Console.WriteLine("".PadRight(80, '-'));
			}
		}

		static async void XuatDanhSachDanhMuc(BlogDbContext context)
		{
			// Tạo đối tượng BlogRepository
			IBlogRepository blogRepo = new BlogRepository(context);

			var categories = await blogRepo.GetCategoriesAsync();

			Console.WriteLine("{0,-5}{1,-50}{2,10}", "ID", "Name", "Count");

			foreach (var category in categories)
			{
				Console.WriteLine("{0,-5}{1,-50}{2,10}", category.Id, category.Name, category.PostCount);
			}
		}

		static async void XuatDanhSachTheTheoPhanTrang(BlogDbContext context)
		{
			// Tạo đối tượng BlogRepository
			IBlogRepository blogRepo = new BlogRepository(context);

			// Tạo đối tượng chứa tham số phân trang
			var pagingParams = new PagingParams()
			{
				PageNumber = 1,
				PageSize = 5,
				SortColumn = "Name",
				SortOrder = "DESC"
			};

			// Lấy danh sách từ khóa
			var tagsList = await blogRepo.GetPagedTagsAsync(pagingParams);

			// Xuất ra màn hình
			Console.WriteLine("{0,-5}{1,-50}{2,10}", "ID", "Name", "Count");

			foreach (var tag in tagsList)
			{
				Console.WriteLine("{0,-5}{1,-50}{2,10}", tag.Id, tag.Name, tag.PostCount);
			}
		}
	}
}