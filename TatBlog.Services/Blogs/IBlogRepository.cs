using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs
{
	public interface IBlogRepository
	{
		// Tìm bài viết có tên định danh là 'slug'
		// và được đăng vào tháng 'month' năm 'year'
		public Task<Post> GetPostAsync(
			int year, 
			int month, 
			string slug, 
			CancellationToken cancellationToken = default);

		// Tìm top N bài viết phổ biến được nhiều người xem nhất
		public Task<IList<Post>> GetPopularArticlesAsync(
			int numPosts, 
			CancellationToken cancellationToken = default);

		// Kiểm tra xem tên định danh của bài viết đã có hay chưa
		public Task<bool> IsPostSlugExistedAsync(
			int postId, string slug, 
			CancellationToken cancellationToken = default);

		// Tăng số lượt xem của một bài viết
		public Task IncreaseViewCountAsync(
			int postId, 
			CancellationToken cancellationToken = default);

		// Lấy danh sách chuyên mục và số lượng bài viết
		// thuộc từng chuyên mục/chủ đề
		public Task<IList<CategoryItem>> GetCategoriesAsync(
			bool showOnMenu = false, 
			CancellationToken cancellationToken = default);

		// Lấy danh sách từ khóa/thẻ và phân trang theo
		// các tham số pagingParams
		public Task<IPagedList<TagItem>> GetPagedTagsAsync(
			IPagingParams pagingParams, CancellationToken cancellationToken = default);
	}
}
