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

			//XuatDanhSachTacGia(context);
			//XuatDanhSachBaiViet(context);
			//BaiVietDuocXemNhieuNhat(context, 3);
			//XuatDanhSachDanhMuc(context);
			//XuatDanhSachTheTheoPhanTrang(context);
			//TimMotTheChoTruoc(context,"");
			XoaTags(context, 2);

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

		static async void BaiVietDuocXemNhieuNhat(BlogDbContext context, int numPost)
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

		static async void TimMotTheChoTruoc(BlogDbContext context, string slug)
		{
			// Tạo đối tượng BlogRepository
			IBlogRepository blogRepo = new BlogRepository(context);

			var tags = await blogRepo.GetTagsItemsAsync(slug);

			foreach (var tag in tags)
			{
				Console.WriteLine("ID      : {0}", tag.Id);
				Console.WriteLine("Name   : {0}", tag.Name);
				Console.WriteLine("Url slug    : {0}", tag.UrlSlug);
				Console.WriteLine("Descriptin    : {0}", tag.Description);
				Console.WriteLine("Post count  : {0}", tag.PostCount);
				Console.WriteLine("".PadRight(60, '-'));
			}
		}

		#region C.Thuc hanh
		// Cau 1. a: Tìm một thẻ(Tag) theo tên định danh(slug)

		// Cau 1. c: Lấy danh sách tất cả các thẻ (Tag) kèm theo số bài viết chứa thẻ đó


		// Cau 1.d: Xóa một thẻ theo mã cho trước
		static async void XoaTags(BlogDbContext context, int id)
		{
			IBlogRepository blogRepo = new BlogRepository(context);
			await blogRepo.DeleteTagWithIdAsync(id);

		}


		// Câu 1. e: Tìm một chuyên mục (Category) theo tên định danh(slug)

		// 1.F: Tìm 1 chuyên mục theo mã số

		// 1.G: Thêm hoặc cập nhật một chuyên mục

		// 1.H: Xóa một chuyên mục theo mã số

		// 1.I: Kiểm tra tên định danh (slug)

		// 1.J: Lấy và phân trang danh sách chuyên mục

		// 1.K: Đếm số lượng bài viết trong N tháng gần nhất. N là tham số đầu vào.
		// Kết quả là một danh sách các đối tượng chứa các thông tin sau: Năm, Tháng, Số bài viết


		// 1.L: Tìm một bài viết theo mã số

		// 1.M: Thêm hay cập nhật một bài viết

		// 1.N: Chuyển đổi trạng thái Published của bài viết

		// 1.O: Chuyển đổi trạng thái Published của bài viết

		// 1.P: Tạo lớp PostQuery để lưu trữ các điều kiện tìm kiếm bài viết. Chẳng hạn:mã tác giả,
		// mã chuyên mục, tên ký hiệu chuyên mục, năm/tháng đăng bài, từ khóa, … 

		// 1.Q: Tìm tất cả bài viết thỏa mãn điều kiện tìm kiếm được cho trong đối tượng
		// PostQuery(kết quả trả về kiểu IList<Post>)

		// 1.R: Đếm số lượng bài viết thỏa mãn điều kiện tìm kiếm được cho trong đối tượng PostQuery

		// 1.S: Tìm và phân trang các bài viết thỏa mãn điều kiện tìm kiếm được cho trong
		// đối tượng PostQuery(kết quả trả về kiểu IPagedList<Post>)

		// 1.T: Tương tự câu trên nhưng yêu cầu trả về kiểu IPagedList<T>. Trong đó T
		// là kiểu dữ liệu của đối tượng mới được tạo từ đối tượng Post.Hàm này có
		// thêm một đầu vào là Func<IQueryable<Post>, IQueryable<T>> mapper
		// để ánh xạ các đối tượng Post thành các đối tượng T theo yêu cầu
		#endregion
	}
}