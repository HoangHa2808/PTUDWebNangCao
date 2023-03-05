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


		public Task<IList<TagItem>> GetTagsItemsAsync(
			string slug,
			CancellationToken cancellationToken = default);

		// 1.D : Xóa 1 thẻ theo mã cho trước
		Task DeleteTagWithIdAsync(int id,CancellationToken cancellationToken = default);

		//1.E: Tìm một chuyên mục(Category) theo tên định danh (slug)
		Task<Category> GetCategoryByUrlAsync(string slug, CancellationToken cancellationToken = default);

		// 1.F: Tìm 1 chuyên mục theo mã số
		Task<Category> GetCategoryByIDAsync(int id, CancellationToken cancellationToken = default);

		// 1.G: Thêm hoặc cập nhật một chuyên mục
		Task AddCategoryAsync(string CategoriesName, CancellationToken cancellationToken = default);

		// 1.H: Xóa một chuyên mục theo mã số
		Task DeleteCategoryByIdAsync(int CategoryId, CancellationToken cancellationToken = default);

		// 1.I: Kiểm tra tên định danh (slug)
		Task<bool> IsPostSlugByCategoryAsync(int categoryId,string slug , CancellationToken cancellationToken = default);
	
		// 1.J: Lấy và phân trang danh sách chuyên mục
		Task<IPagedList<CategoryItem>> GetPagedCategoryAsync(IPagingParams pagingParams, CancellationToken cancellationToken = default);

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
	}
}
