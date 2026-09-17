using System.Net.Http.Json;

namespace MiniSupermarket.WinForms
{
    public partial class FormCategoryManagement : Form
    {
        // =========================================================
        // ĐỊA CHỈ WEB API
        // =========================================================

        private static readonly HttpClient _client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7123/api/")
        };

        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public FormCategoryManagement()
        {
            InitializeComponent();

            // Hiển thị URL API ở thanh trạng thái
            lblStatus.Text =
                "Ready    " +
                _client.BaseAddress + "categories";
        }

        // =========================================================
        // FORM LOAD
        // =========================================================

        private async void FormCategoryManagement_Load(
            object sender,
            EventArgs e)
        {
            await LoadDataAsync();
        }

        // =========================================================
        // LOAD DANH SÁCH NHÓM HÀNG
        // GET: api/categories
        // =========================================================

        private async Task LoadDataAsync()
        {
            try
            {
                lblStatus.Text = "Đang tải dữ liệu...";

                var categories =
                    await _client.GetFromJsonAsync<List<CategoryDto>>(
                        "categories");

                dgvCategories.DataSource = categories;

                // Đổi tên tiêu đề cột cho giống đề bài
                if (dgvCategories.Columns.Contains("CategoryId"))
                {
                    dgvCategories.Columns["CategoryId"].HeaderText =
                        "Mã ID";
                }

                if (dgvCategories.Columns.Contains("CategoryName"))
                {
                    dgvCategories.Columns["CategoryName"].HeaderText =
                        "Tên Nhóm hàng";
                }

                if (dgvCategories.Columns.Contains("Description"))
                {
                    dgvCategories.Columns["Description"].HeaderText =
                        "Mô Tả";
                }

                lblStatus.Text =
                    "Ready    " +
                    _client.BaseAddress + "categories";
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Lỗi kết nối Server";

                MessageBox.Show(
                    "Không thể kết nối đến Server.\n\n" +
                    "Chi tiết lỗi:\n" +
                    ex.Message,
                    "Lỗi kết nối",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // =========================================================
        // CLICK VÀO DÒNG TRONG DATAGRIDVIEW
        // =========================================================

        private void dgvCategories_CellClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            try
            {
                DataGridViewRow row =
                    dgvCategories.Rows[e.RowIndex];

                // CategoryId
                if (row.Cells["CategoryId"].Value != null)
                {
                    txtId.Text =
                        row.Cells["CategoryId"].Value.ToString();
                }

                // CategoryName
                if (row.Cells["CategoryName"].Value != null)
                {
                    txtCategoryName.Text =
                        row.Cells["CategoryName"].Value.ToString();
                }
                else
                {
                    txtCategoryName.Text = "";
                }

                // Description
                if (row.Cells["Description"].Value != null)
                {
                    txtDescription.Text =
                        row.Cells["Description"].Value.ToString();
                }
                else
                {
                    txtDescription.Text = "";
                }
            }
            catch
            {
                // Không làm gì nếu dòng không hợp lệ
            }
        }

        // =========================================================
        // NÚT TẢI LẠI
        // =========================================================

        private async void btnLoad_Click(
            object sender,
            EventArgs e)
        {
            await LoadDataAsync();

            ClearInputs();
        }

        // =========================================================
        // NÚT THÊM MỚI
        // POST: api/categories
        // =========================================================

        private async void btnAdd_Click(
            object sender,
            EventArgs e)
        {
            // Kiểm tra tên nhóm hàng
            if (string.IsNullOrWhiteSpace(
                txtCategoryName.Text))
            {
                MessageBox.Show(
                    "Vui lòng nhập Tên Nhóm hàng!",
                    "Cảnh báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtCategoryName.Focus();

                return;
            }

            try
            {
                var newCategory = new
                {
                    CategoryName =
                        txtCategoryName.Text.Trim(),

                    Description =
                        txtDescription.Text.Trim()
                };

                var response =
                    await _client.PostAsJsonAsync(
                        "categories",
                        newCategory);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        "Thêm mới nhóm hàng thành công!",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    await LoadDataAsync();

                    ClearInputs();
                }
                else
                {
                    MessageBox.Show(
                        "Thêm mới thất bại!\n\n" +
                        "Mã lỗi: " +
                        response.StatusCode,
                        "Lỗi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi khi thêm nhóm hàng:\n\n" +
                    ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // =========================================================
        // NÚT CẬP NHẬT
        // PUT: api/categories/{id}
        // =========================================================

        private async void btnUpdate_Click(
            object sender,
            EventArgs e)
        {
            // Kiểm tra ID
            if (string.IsNullOrWhiteSpace(txtId.Text))
            {
                MessageBox.Show(
                    "Vui lòng chọn nhóm hàng cần cập nhật!",
                    "Cảnh báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            // Kiểm tra ID
            if (!int.TryParse(
                txtId.Text,
                out int id))
            {
                MessageBox.Show(
                    "Mã ID không hợp lệ!",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            // Kiểm tra tên
            if (string.IsNullOrWhiteSpace(
                txtCategoryName.Text))
            {
                MessageBox.Show(
                    "Vui lòng nhập Tên Nhóm hàng!",
                    "Cảnh báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtCategoryName.Focus();

                return;
            }

            try
            {
                var updateCategory = new
                {
                    CategoryId = id,

                    CategoryName =
                        txtCategoryName.Text.Trim(),

                    Description =
                        txtDescription.Text.Trim()
                };

                var response =
                    await _client.PutAsJsonAsync(
                        $"categories/{id}",
                        updateCategory);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        "Cập nhật nhóm hàng thành công!",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    await LoadDataAsync();

                    ClearInputs();
                }
                else
                {
                    MessageBox.Show(
                        "Cập nhật thất bại!\n\n" +
                        "Mã lỗi: " +
                        response.StatusCode,
                        "Lỗi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi khi cập nhật:\n\n" +
                    ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // =========================================================
        // NÚT XÓA
        // DELETE: api/categories/{id}
        // =========================================================

        private async void btnDelete_Click(
            object sender,
            EventArgs e)
        {
            // Kiểm tra ID
            if (string.IsNullOrWhiteSpace(txtId.Text))
            {
                MessageBox.Show(
                    "Vui lòng chọn nhóm hàng cần xóa!",
                    "Cảnh báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            // Kiểm tra ID
            if (!int.TryParse(
                txtId.Text,
                out int id))
            {
                MessageBox.Show(
                    "Mã ID không hợp lệ!",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            // Xác nhận xóa
            DialogResult result =
                MessageBox.Show(
                    $"Bạn có chắc muốn xóa nhóm hàng ID = {id}?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            try
            {
                var response =
                    await _client.DeleteAsync(
                        $"categories/{id}");

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        "Xóa nhóm hàng thành công!",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    await LoadDataAsync();

                    ClearInputs();
                }
                else
                {
                    MessageBox.Show(
                        "Xóa thất bại!\n\n" +
                        "Mã lỗi: " +
                        response.StatusCode,
                        "Lỗi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi khi xóa:\n\n" +
                    ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // =========================================================
        // NÚT TÌM KIẾM
        // GET: api/categories/search?keyword=
        // =========================================================

        private async void btnSearch_Click(
            object sender,
            EventArgs e)
        {
            string keyword =
                txtKeyword.Text.Trim();

            // Không nhập từ khóa
            // => tải toàn bộ danh sách
            if (string.IsNullOrWhiteSpace(keyword))
            {
                await LoadDataAsync();

                return;
            }

            try
            {
                lblStatus.Text = "Đang tìm kiếm...";

                string encodedKeyword =
                    Uri.EscapeDataString(keyword);

                var result =
                    await _client.GetFromJsonAsync<
                        List<CategoryDto>>(
                        $"categories/search?keyword={encodedKeyword}");

                dgvCategories.DataSource = result;

                // Đặt lại tên cột
                if (dgvCategories.Columns.Contains("CategoryId"))
                {
                    dgvCategories.Columns["CategoryId"].HeaderText =
                        "Mã ID";
                }

                if (dgvCategories.Columns.Contains("CategoryName"))
                {
                    dgvCategories.Columns["CategoryName"].HeaderText =
                        "Tên Nhóm hàng";
                }

                if (dgvCategories.Columns.Contains("Description"))
                {
                    dgvCategories.Columns["Description"].HeaderText =
                        "Mô Tả";
                }

                lblStatus.Text =
                    "Ready    " +
                    _client.BaseAddress +
                    "categories/search?keyword=" +
                    keyword;

                if (result == null || result.Count == 0)
                {
                    MessageBox.Show(
                        "Không tìm thấy nhóm hàng phù hợp!",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Lỗi tìm kiếm";

                MessageBox.Show(
                    "Lỗi tìm kiếm:\n\n" +
                    ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // =========================================================
        // XÓA DỮ LIỆU NHẬP
        // =========================================================

        private void ClearInputs()
        {
            txtId.Clear();
            txtCategoryName.Clear();
            txtDescription.Clear();

            txtCategoryName.Focus();
        }
    }

    // =============================================================
    // DTO NHÓM HÀNG
    // =============================================================

    public class CategoryDto
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }
            = string.Empty;

        public string? Description { get; set; }
    }
}