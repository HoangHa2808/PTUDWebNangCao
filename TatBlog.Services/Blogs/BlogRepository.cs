using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;
using TatBlog.Core.DTO;
using TatBlog.Core.Contracts;
using TatBlog.Data.Contexts;
using TatBlog.Services.Extensions;

namespace TatBlog.Services.Blogs;

public class BlogRepository : IBlogRepository
{
	private readonly BlogDbContext _context;

	public BlogRepository(BlogDbContext context)
	{
		_context = context;
	}

	// Tìm bài viết có tên định danh là 'slug'
	// và được đăng vào tháng 'month' năm 'year'
	public async Task<Post> GetPostAsync(
		int year, int month,
		string slug, CancellationToken cancellationToken = default)
	{
		IQueryable<Post> postsQuery = _context.Set<Post>()
			.Include(x => x.Category)
			.Include(x => x.Author);

		if (year > 0)
		{
			postsQuery = postsQuery.Where(x => x.PostedDate.Year == year);
		}

		if (month > 0)
		{
			postsQuery = postsQuery.Where(x => x.PostedDate.Month == month);
		}

		if (!string.IsNullOrWhiteSpace(slug))
		{
			postsQuery = postsQuery.Where(x => x.UrlSlug == slug);
		}

		return await postsQuery.FirstOrDefaultAsync(cancellationToken);
	}

	// Tìm top N bài viết phổ biến được nhiều người xem nhất
	public async Task<IList<Post>> GetPopularArticlesAsync(
		int numPosts, CancellationToken cancellationToken = default)
	{
		return await _context.Set<Post>()
			.Include(x => x.Author)
			.Include(x => x.Category)
			.OrderByDescending(p => p.ViewCount)
			.Take(numPosts)
			.ToListAsync(cancellationToken);
	}

	// Kiểm tra xem tên định danh của bài viết đã có hay chưa
	public async Task<bool> IsPostSlugExistedAsync(
		int postId, string slug, CancellationToken cancellationToken = default)
	{
		return await _context.Set<Post>()
			.AnyAsync(x => x.Id != postId && x.UrlSlug == slug,
			cancellationToken);
	}

	// Tăng số lượt xem của một bài viết
	public async Task IncreaseViewCountAsync(
		int postId, CancellationToken cancellationToken = default)
	{
		await _context.Set<Post>()
			.Where(x => x.Id == postId)
			.ExecuteUpdateAsync(p => p.SetProperty(
				x => x.ViewCount, x => x.ViewCount + 1),
				cancellationToken);
	}

	// Lấy danh sách chuyên mục và số lượng bài viết
	// thuộc từng chuyên mục/chủ đề
	public async Task<IList<CategoryItem>> GetCategoriesAsync(bool showOnMenu = false, CancellationToken cancellationToken = default)
	{
		IQueryable<Category> categories = _context.Set<Category>();

		if (showOnMenu)
		{
			categories = categories.Where(x => x.ShowOnMenu);
		}

		return await categories
			.OrderBy(x => x.Name)
			.Select(x => new CategoryItem()
			{
				Id = x.Id,
				Name = x.Name,
				UrlSlug = x.UrlSlug,
				Description = x.Description,
				ShowOnMenu = x.ShowOnMenu,
				PostCount = x.Posts.Count(p => p.Published)
			})
			.ToListAsync(cancellationToken);
	}

	// Lấy danh sách từ khóa/thẻ và phân trang theo
	// các tham số pagingParams
	public async Task<IPagedList<TagItem>> GetPagedTagsAsync(
		IPagingParams pagingParams, CancellationToken cancellationToken = default)
	{
		var tagQuery = _context.Set<Tag>()
			.Select(x => new TagItem()
			{
				Id = x.Id,
				Name = x.Name,
				UrlSlug = x.UrlSlug,
				Description = x.Description,
				PostCount = x.Posts.Count(p => p.Published)
			});

		return await tagQuery.ToPagedListAsync(pagingParams, cancellationToken);
	}

	//public Task<IList<TagItem>> GetTagsItemsAsync(string slug, CancellationToken cancellationToken = default)
	//{
	//	IQueryable<Tag> tagItemsQuery = _context.Set<Tag>()
	//		.Include(x => x.Id)
	//		.Include(x => x.Name);
	//	return tagItemsQuery;
	//	.OrderBy(x => x.Name)
	//	.Select(x => new TagItem()
	//	{
	//		Id = x.Id,
	//		Name = x.Name,
	//		UrlSlug = x.UrlSlug,
	//		Description = x.Description,
	//		PostCount = x.Posts.Count(p => p.Published)
	//	})
	//.ToListAsync(cancellationToken);
	//}

	#region C.Thuc hanh

	// Cau 1. a: Tìm một thẻ(Tag) theo tên định danh(slug)
	public async Task<Tag> GetTagByUrlSlugAsync(string slug, CancellationToken cancellationToken = default)
	{
		return await _context.Set<Tag>()
			.Where(x => x.UrlSlug == slug)
			.FirstOrDefaultAsync(cancellationToken);
	}

	// Cau 1. c: Lấy danh sách tất cả các thẻ (Tag) kèm theo số bài viết chứa thẻ đó


	// Cau 1.d: Xóa một thẻ theo mã cho trước

	public async Task DeleteTagWithIdAsync(int id, CancellationToken cancellationToken = default)
	{
		await _context.Database
				.ExecuteSqlRawAsync("DELETE FROM PostTags WHERE TagId = " + id, cancellationToken);

		await _context.Set<Tag>()
			.Where(t => t.Id == id)
			.ExecuteDeleteAsync(cancellationToken);
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
