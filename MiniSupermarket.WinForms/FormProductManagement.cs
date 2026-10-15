
using System; // Các kiểu dữ liệu cơ bản
using System.Collections.Generic; // Danh sách List
using System.Drawing; // Màu sắc, kích thước
using System.Net; // Trạng thái HTTP
using System.Net.Http; // Gọi HTTP API
using System.Net.Http.Json; // Đọc và gửi JSON
using System.Text.Json.Serialization; // Ánh xạ JSON
using System.Threading.Tasks; // Xử lý bất đồng bộ
using System.Windows.Forms; // Giao diện WinForms

namespace MiniSupermarket.WinForms // Namespace WinForms
{
    public partial class FormProductManagement : Form // Form quản lý sản phẩm
    {
        // ================= CẤU HÌNH API =================

        private const string ProductEndpoint = "products"; // API sản phẩm
        private const string CategoryEndpoint = "categories"; // API danh mục

        // ================= ĐIỀU KHIỂN NHẬP LIỆU =================

        private readonly TextBox txtProductId = new(); // Mã sản phẩm
        private readonly TextBox txtBarcode = new(); // Mã vạch
        private readonly TextBox txtProductName = new(); // Tên sản phẩm
        private readonly TextBox txtSearch = new(); // Từ khóa tìm kiếm

        private readonly NumericUpDown nudPrice = new(); // Giá bán
        private readonly NumericUpDown nudStock = new(); // Số lượng tồn
        private readonly ComboBox cboCategory = new(); // Danh mục sản phẩm
        private readonly ComboBox cboFilterCategory = new(); // Lọc danh mục

        // ================= CÁC NÚT CHỨC NĂNG =================

        private readonly Button btnAdd = new(); // Thêm sản phẩm
        private readonly Button btnEdit = new(); // Sửa sản phẩm
        private readonly Button btnDelete = new(); // Xóa sản phẩm
        private readonly Button btnRefresh = new(); // Làm mới
        private readonly Button btnSearch = new(); // Tìm kiếm

        // ================= BẢNG DỮ LIỆU =================

        private readonly DataGridView dgvProducts = new(); // Danh sách sản phẩm

        // ================= TRẠNG THÁI =================

        private bool _isLoading = false; // Tránh tải trùng dữ liệu

        // ================= KHỞI TẠO FORM =================

        public FormProductManagement() // Constructor
        {
            InitializeComponent(); // Khởi tạo Designer

            InitializeProductInterface(); // Tạo giao diện

            RegisterEvents(); // Đăng ký sự kiện

            this.Load += async (sender, e) => // Khi Form được tải
            {
                await InitializeDataAsync(); // Tải danh mục và sản phẩm
            };
        }

        // ================= TẠO GIAO DIỆN =================

        private void InitializeProductInterface() // Thiết kế giao diện
        {
            this.SuspendLayout(); // Tạm dừng bố trí

            this.Text = "Quản lý sản phẩm"; // Tiêu đề Form
            this.Size = new Size(1150, 800); // Kích thước Form
            this.MinimumSize = new Size(900, 650); // Kích thước tối thiểu
            this.BackColor = Color.WhiteSmoke; // Màu nền
            this.Font = new Font("Segoe UI", 10); // Font chữ

            // ================= PANEL TIÊU ĐỀ =================

            Panel panelTitle = new(); // Tạo Panel tiêu đề
            panelTitle.Dock = DockStyle.Top; // Gắn trên cùng
            panelTitle.Height = 60; // Chiều cao
            panelTitle.BackColor = Color.FromArgb(35, 80, 145); // Màu xanh

            Label lblTitle = new(); // Tạo nhãn tiêu đề
            lblTitle.Text = "QUẢN LÝ SẢN PHẨM"; // Nội dung
            lblTitle.ForeColor = Color.White; // Màu chữ
            lblTitle.Font = new Font("Segoe UI", 18, FontStyle.Bold); // Font
            lblTitle.TextAlign = ContentAlignment.MiddleCenter; // Căn giữa
            lblTitle.Dock = DockStyle.Fill; // Chiếm Panel

            panelTitle.Controls.Add(lblTitle); // Thêm tiêu đề

            // ================= PANEL TÌM KIẾM =================

            Panel panelSearch = new(); // Tạo Panel tìm kiếm
            panelSearch.Dock = DockStyle.Top; // Gắn phía trên
            panelSearch.Height = 65; // Chiều cao
            panelSearch.BackColor = Color.White; // Màu nền

            panelSearch.Controls.Add(
                CreateLabel("Tìm sản phẩm:", 20, 22)
            ); // Thêm nhãn tìm kiếm

            txtSearch.Location = new Point(150, 18); // Vị trí
            txtSearch.Size = new Size(250, 30); // Kích thước
            txtSearch.PlaceholderText = "Nhập tên hoặc mã vạch"; // Gợi ý

            panelSearch.Controls.Add(txtSearch); // Thêm ô tìm kiếm

            panelSearch.Controls.Add(
                CreateLabel("Danh mục:", 420, 22)
            ); // Nhãn lọc danh mục

            cboFilterCategory.Location = new Point(515, 18);
            cboFilterCategory.Size = new Size(220, 30);
            cboFilterCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cboFilterCategory.DisplayMember = "CategoryName";
            cboFilterCategory.ValueMember = "CategoryId";
            panelSearch.Controls.Add(cboFilterCategory);

            SetupButton(
                btnSearch, "Tìm kiếm", 755, 16, Color.SteelBlue
            ); // Thiết lập nút tìm kiếm

            panelSearch.Controls.Add(btnSearch); // Thêm nút tìm kiếm

            // ================= PANEL NHẬP LIỆU =================

            Panel panelInput = new(); // Tạo Panel nhập liệu
            panelInput.Dock = DockStyle.Top; // Gắn phía trên
            panelInput.Height = 200; // Chiều cao
            panelInput.BackColor = Color.White; // Màu nền

            // ---------- MÃ SẢN PHẨM ----------

            panelInput.Controls.Add(
                CreateLabel("Mã sản phẩm:", 20, 25)
            ); // Nhãn mã sản phẩm

            SetupTextBox(txtProductId, 150, 20); // Vị trí ô mã

            txtProductId.ReadOnly = true; // Không cho sửa ID
            txtProductId.BackColor = Color.Gainsboro; // Màu xám

            panelInput.Controls.Add(txtProductId); // Thêm ô ID

            // ---------- MÃ VẠCH ----------

            panelInput.Controls.Add(
                CreateLabel("Mã vạch:", 20, 75)
            ); // Nhãn mã vạch

            SetupTextBox(txtBarcode, 150, 70); // Thiết lập ô mã vạch

            panelInput.Controls.Add(txtBarcode); // Thêm ô mã vạch

            // ---------- TÊN SẢN PHẨM ----------

            panelInput.Controls.Add(
                CreateLabel("Tên sản phẩm:", 20, 125)
            ); // Nhãn tên sản phẩm

            SetupTextBox(txtProductName, 150, 120); // Thiết lập ô tên

            panelInput.Controls.Add(txtProductName); // Thêm ô tên

            // ---------- GIÁ BÁN NUMERICUPDOWN ----------

            panelInput.Controls.Add(
                CreateLabel("Giá bán:", 500, 25)
            ); // Nhãn giá bán

            nudPrice.Location = new Point(640, 20); // Vị trí
            nudPrice.Size = new Size(300, 30); // Kích thước
            nudPrice.Minimum = 0; // Giá tối thiểu
            nudPrice.Maximum = 1000000000000M; // Giới hạn nhập
            nudPrice.DecimalPlaces = 2; // Hai chữ số thập phân
            nudPrice.ThousandsSeparator = true; // Phân cách hàng nghìn
            nudPrice.Increment = 1000; // Mức tăng khi nhấn mũi tên

            panelInput.Controls.Add(nudPrice); // Thêm NumericUpDown giá

            // ---------- SỐ LƯỢNG TỒN NUMERICUPDOWN ----------

            panelInput.Controls.Add(
                CreateLabel("Số lượng tồn:", 500, 75)
            ); // Nhãn tồn kho

            nudStock.Location = new Point(640, 70); // Vị trí
            nudStock.Size = new Size(300, 30); // Kích thước
            nudStock.Minimum = 0; // Tồn kho tối thiểu
            nudStock.Maximum = int.MaxValue; // Tồn kho tối đa
            nudStock.DecimalPlaces = 0; // Chỉ nhận số nguyên

            panelInput.Controls.Add(nudStock); // Thêm NumericUpDown tồn kho

            // ---------- DANH MỤC COMBOBOX ----------

            panelInput.Controls.Add(
                CreateLabel("Danh mục:", 500, 125)
            ); // Nhãn danh mục

            cboCategory.Location = new Point(640, 120); // Vị trí
            cboCategory.Size = new Size(300, 30); // Kích thước

            cboCategory.DropDownStyle =
                ComboBoxStyle.DropDownList; // Chỉ được chọn từ danh sách

            cboCategory.DisplayMember = "CategoryName"; // Hiển thị tên
            cboCategory.ValueMember = "CategoryId"; // Lấy ID

            panelInput.Controls.Add(cboCategory); // Thêm ComboBox

            // ================= PANEL NÚT CHỨC NĂNG =================

            Panel panelButtons = new(); // Tạo Panel nút
            panelButtons.Dock = DockStyle.Top; // Gắn phía trên
            panelButtons.Height = 65; // Chiều cao
            panelButtons.BackColor = Color.White; // Màu nền

            SetupButton(
                btnAdd, "Thêm", 150, 15, Color.SeaGreen
            ); // Nút Thêm

            SetupButton(
                btnEdit, "Sửa", 290, 15, Color.DarkOrange
            ); // Nút Sửa

            SetupButton(
                btnDelete, "Xóa", 430, 15, Color.Firebrick
            ); // Nút Xóa

            SetupButton(
                btnRefresh, "Làm mới", 570, 15, Color.SteelBlue
            ); // Nút Làm mới

            panelButtons.Controls.Add(btnAdd); // Thêm nút
            panelButtons.Controls.Add(btnEdit); // Sửa nút
            panelButtons.Controls.Add(btnDelete); // Xóa nút
            panelButtons.Controls.Add(btnRefresh); // Làm mới nút

            // ================= PANEL BẢNG =================

            Panel panelGrid = new(); // Tạo Panel bảng
            panelGrid.Dock = DockStyle.Fill; // Chiếm vùng còn lại
            panelGrid.Padding = new Padding(15); // Khoảng cách
            panelGrid.BackColor = Color.WhiteSmoke; // Màu nền

            dgvProducts.Dock = DockStyle.Fill; // Chiếm Panel
            dgvProducts.BackgroundColor = Color.White; // Màu nền bảng
            dgvProducts.BorderStyle = BorderStyle.None; // Bỏ viền

            dgvProducts.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill; // Tự giãn cột

            dgvProducts.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect; // Chọn cả dòng

            dgvProducts.MultiSelect = false; // Chỉ chọn một dòng
            dgvProducts.ReadOnly = true; // Không sửa trên bảng
            dgvProducts.AllowUserToAddRows = false; // Không thêm dòng trống
            dgvProducts.RowHeadersVisible = false; // Ẩn tiêu đề dòng
            dgvProducts.RowTemplate.Height = 35; // Chiều cao dòng
            dgvProducts.ColumnHeadersHeight = 40; // Chiều cao tiêu đề
            dgvProducts.EnableHeadersVisualStyles = false; // Tùy chỉnh màu

            dgvProducts.ColumnHeadersDefaultCellStyle.BackColor =
                Color.SteelBlue; // Màu nền tiêu đề

            dgvProducts.ColumnHeadersDefaultCellStyle.ForeColor =
                Color.White; // Màu chữ tiêu đề

            dgvProducts.Columns.Add("ProductId", "Mã SP"); // Cột ID
            dgvProducts.Columns.Add("Barcode", "Mã vạch"); // Cột mã vạch
            dgvProducts.Columns.Add("ProductName", "Tên sản phẩm"); // Cột tên
            dgvProducts.Columns.Add("Price", "Giá bán"); // Cột giá
            dgvProducts.Columns.Add("StockQuantity", "Tồn kho"); // Cột tồn
            dgvProducts.Columns.Add("CategoryId", "Mã danh mục"); // Cột ID danh mục
            dgvProducts.Columns.Add("CategoryName", "Tên danh mục"); // Cột tên danh mục

            panelGrid.Controls.Add(dgvProducts); // Thêm bảng

            // ================= THỨ TỰ DOCK =================

            this.Controls.Add(panelGrid); // Bảng ở dưới
            this.Controls.Add(panelButtons); // Nút chức năng
            this.Controls.Add(panelInput); // Nhập liệu
            this.Controls.Add(panelSearch); // Tìm kiếm
            this.Controls.Add(panelTitle); // Tiêu đề trên cùng

            this.ResumeLayout(true); // Hoàn tất bố trí
        }

        // ================= HÀM TẠO LABEL =================

        private Label CreateLabel(string text, int x, int y) // Tạo Label
        {
            Label label = new(); // Khởi tạo nhãn
            label.Text = text; // Gán nội dung
            label.Location = new Point(x, y); // Vị trí
            label.AutoSize = true; // Tự động kích thước
            return label; // Trả về nhãn
        }

        // ================= HÀM TẠO TEXTBOX =================

        private void SetupTextBox(TextBox textBox, int x, int y) // Thiết lập TextBox
        {
            textBox.Location = new Point(x, y); // Vị trí
            textBox.Size = new Size(300, 30); // Kích thước
        }

        // ================= HÀM TẠO BUTTON =================

        private void SetupButton(
            Button button,
            string text,
            int x,
            int y,
            Color color) // Thiết lập Button
        {
            button.Text = text; // Nội dung
            button.Location = new Point(x, y); // Vị trí
            button.Size = new Size(120, 35); // Kích thước
            button.BackColor = color; // Màu nền
            button.ForeColor = Color.White; // Màu chữ
            button.FlatStyle = FlatStyle.Flat; // Nút phẳng
            button.Cursor = Cursors.Hand; // Con trỏ chuột
        }

        // ================= ĐĂNG KÝ SỰ KIỆN =================

        private void RegisterEvents() // Đăng ký các sự kiện
        {
            btnRefresh.Click += async (sender, e) =>
                await InitializeDataAsync(); // Tải lại danh mục và sản phẩm

            btnSearch.Click += async (sender, e) =>
                await SearchProductsAsync(); // Tìm kiếm

            btnAdd.Click += async (sender, e) =>
                await AddProductAsync(); // POST

            btnEdit.Click += async (sender, e) =>
                await UpdateProductAsync(); // PUT

            btnDelete.Click += async (sender, e) =>
                await DeleteProductAsync(); // DELETE

            dgvProducts.CellClick += DgvProducts_CellClick; // Chọn dòng
        }

        // ================= KIỂM TRA JWT VÀ ROLE =================

        private bool PrepareAuthorization() // Kiểm tra quyền
        {
            if (!SessionManager.IsLoggedIn) // Chưa đăng nhập
            {
                MessageBox.Show("Bạn chưa đăng nhập."); // Thông báo
                return false; // Dừng
            }

            string role = SessionManager.CurrentRole; // Lấy role

            if (!string.Equals(
                    role, "Admin", StringComparison.OrdinalIgnoreCase)
                &&
                !string.Equals(
                    role, "Warehouse", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(
                    "Bạn không có quyền quản lý sản phẩm."
                ); // Thông báo không đủ quyền

                return false; // Dừng
            }

            ApiClientService.SetBearerToken(
                SessionManager.JwtToken
            ); // Gắn JWT vào HttpClient

            return true; // Đủ quyền
        }

        // ================= TẢI DỮ LIỆU BAN ĐẦU =================

        private async Task InitializeDataAsync() // Tải dữ liệu
        {
            if (_isLoading) return; // Tránh tải trùng

            _isLoading = true; // Đánh dấu đang tải

            btnRefresh.Enabled = false; // Khóa làm mới

            try
            {
                await LoadCategoriesAsync(); // Tải danh mục trước

                await LoadProductsAsync(); // Tải sản phẩm sau
            }
            finally
            {
                btnRefresh.Enabled = true; // Bật lại nút

                _isLoading = false; // Kết thúc tải
            }
        }

        // ================= GET DANH MỤC - BƯỚC 15.5 =================

        private async Task LoadCategoriesAsync() // Tải danh mục từ API
        {
            if (!PrepareAuthorization()) return; // Kiểm tra quyền

            cboCategory.DataSource = null; // Xóa dữ liệu danh mục cũ
            cboFilterCategory.DataSource = null; // Xóa bộ lọc cũ

            try
            {
                using HttpResponseMessage response =
                    await ApiClientService.Client.GetAsync(
                        CategoryEndpoint
                    ); // GET /api/categories

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    MessageBox.Show(
                        "Phiên đăng nhập đã hết hạn hoặc token không hợp lệ."
                    ); // Thông báo lỗi 401

                    return; // Dừng
                }

                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    MessageBox.Show(
                        "Tài khoản không có quyền xem danh mục."
                    ); // Thông báo lỗi 403

                    return; // Dừng
                }

                response.EnsureSuccessStatusCode(); // Kiểm tra HTTP

                List<CategoryDto>? categories =
                    await response.Content.ReadFromJsonAsync<
                        List<CategoryDto>
                    >(); // Đọc JSON danh mục

                cboCategory.DisplayMember = "CategoryName"; // Tên hiển thị
                cboCategory.ValueMember = "CategoryId"; // ID được chọn

                List<CategoryDto> availableCategories =
                    categories ?? new List<CategoryDto>();

                cboCategory.DataSource =
                    new List<CategoryDto>(availableCategories); // Gán dữ liệu

                cboCategory.SelectedIndex = -1; // Không chọn mặc định

                // Bộ lọc có thêm lựa chọn xem toàn bộ danh mục.
                var filterCategories = new List<CategoryDto>
                {
                    new CategoryDto
                    {
                        CategoryId = 0,
                        CategoryName = "Tất cả danh mục"
                    }
                };

                filterCategories.AddRange(availableCategories);
                cboFilterCategory.DataSource = filterCategories;
                cboFilterCategory.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải danh mục: " + ex.Message,
                    "Lỗi API",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                ); // Hiển thị lỗi
            }
        }

        // ================= GET DANH SÁCH SẢN PHẨM =================

        private async Task LoadProductsAsync() // Tải sản phẩm
        {
            if (!PrepareAuthorization()) return; // Kiểm tra quyền

            try
            {
                using HttpResponseMessage response =
                    await ApiClientService.Client.GetAsync(
                        ProductEndpoint
                    ); // GET /api/products

                if (!await CheckReadResponseAsync(response))
                    return; // Kiểm tra lỗi HTTP

                List<ProductViewModel>? products =
                    await response.Content.ReadFromJsonAsync<
                        List<ProductViewModel>
                    >(); // Đọc JSON

                DisplayProducts(
                    products ?? new List<ProductViewModel>()
                ); // Hiển thị sản phẩm
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải sản phẩm: " + ex.Message
                ); // Hiển thị lỗi
            }
        }

        // ================= GET TÌM KIẾM =================

        private async Task SearchProductsAsync() // Tìm kiếm sản phẩm
        {
            if (!PrepareAuthorization()) return; // Kiểm tra quyền

            string keyword = txtSearch.Text.Trim(); // Lấy từ khóa
            int categoryId = cboFilterCategory.SelectedValue is int value
                ? value
                : 0;

            if (string.IsNullOrWhiteSpace(keyword) && categoryId <= 0)
            {
                await LoadProductsAsync(); // Từ khóa trống thì tải tất cả
                return; // Kết thúc
            }

            btnSearch.Enabled = false; // Khóa nút

            try
            {
                var query = new List<string>();

                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    query.Add(
                        "keyword=" + Uri.EscapeDataString(keyword));
                }

                if (categoryId > 0)
                    query.Add("categoryId=" + categoryId);

                string url = ProductEndpoint +
                    "/search?" + string.Join("&", query); // Tạo URL tìm kiếm

                using HttpResponseMessage response =
                    await ApiClientService.Client.GetAsync(
                        url
                    ); // GET /api/products/search

                if (!await CheckReadResponseAsync(response))
                    return; // Kiểm tra HTTP

                List<ProductViewModel>? products =
                    await response.Content.ReadFromJsonAsync<
                        List<ProductViewModel>
                    >(); // Đọc kết quả

                DisplayProducts(
                    products ?? new List<ProductViewModel>()
                ); // Hiển thị kết quả
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tìm kiếm: " + ex.Message
                ); // Thông báo lỗi
            }
            finally
            {
                btnSearch.Enabled = true; // Bật lại nút
            }
        }

        // ================= KIỂM TRA PHẢN HỒI GET =================

        private async Task<bool> CheckReadResponseAsync(
            HttpResponseMessage response) // Kiểm tra HTTP GET
        {
            if (response.IsSuccessStatusCode)
                return true; // GET thành công

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                MessageBox.Show(
                    "Token không hợp lệ hoặc đã hết hạn.",
                    "Lỗi 401"
                ); // Lỗi xác thực

                return false; // Không đọc JSON
            }

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                MessageBox.Show(
                    "Bạn không có quyền thực hiện thao tác này.",
                    "Lỗi 403"
                ); // Lỗi phân quyền

                return false; // Không đọc JSON
            }

            string error = await response.Content.ReadAsStringAsync(); // Đọc lỗi

            MessageBox.Show(
                $"API trả lỗi HTTP {(int)response.StatusCode}:\n{error}"
            ); // Hiển thị lỗi

            return false; // Thất bại
        }

        // ================= KIỂM TRA DỮ LIỆU NHẬP =================

        private bool TryGetProductInput(
            out string barcode,
            out string productName,
            out decimal price,
            out int stock,
            out int categoryId) // Lấy và kiểm tra dữ liệu
        {
            barcode = txtBarcode.Text.Trim(); // Lấy mã vạch
            productName = txtProductName.Text.Trim(); // Lấy tên
            price = nudPrice.Value; // Lấy giá bán
            stock = (int)nudStock.Value; // Lấy tồn kho
            categoryId = 0; // Khởi tạo ID danh mục

            if (string.IsNullOrWhiteSpace(barcode))
            {
                MessageBox.Show("Vui lòng nhập mã vạch."); // Thiếu mã vạch
                txtBarcode.Focus(); // Đưa con trỏ vào ô
                return false; // Không hợp lệ
            }

            if (string.IsNullOrWhiteSpace(productName))
            {
                MessageBox.Show("Vui lòng nhập tên sản phẩm."); // Thiếu tên
                txtProductName.Focus(); // Đưa con trỏ vào ô
                return false; // Không hợp lệ
            }

            if (cboCategory.SelectedValue is not int selectedId ||
                selectedId <= 0)
            {
                MessageBox.Show(
                    "Vui lòng chọn danh mục hợp lệ."
                ); // Chưa chọn danh mục

                cboCategory.Focus(); // Đưa con trỏ vào ComboBox
                return false; // Không hợp lệ
            }

            categoryId = selectedId; // Lấy ID danh mục

            return true; // Dữ liệu hợp lệ
        }

        // ================= POST THÊM SẢN PHẨM =================

        private async Task AddProductAsync() // Thêm sản phẩm
        {
            if (!PrepareAuthorization()) return; // Kiểm tra quyền

            if (!TryGetProductInput(
                out string barcode,
                out string productName,
                out decimal price,
                out int stock,
                out int categoryId))
                return; // Kiểm tra dữ liệu nhập

            var newProduct = new // Tạo đối tượng POST
            {
                Barcode = barcode, // Mã vạch
                ProductName = productName, // Tên sản phẩm
                Price = price, // Giá bán
                StockQuantity = stock, // Số lượng tồn
                CategoryId = categoryId // ID danh mục từ ComboBox
            };

            btnAdd.Enabled = false; // Khóa nút Thêm

            try
            {
                using HttpResponseMessage response =
                    await ApiClientService.Client.PostAsJsonAsync(
                        ProductEndpoint,
                        newProduct
                    ); // POST /api/products

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        "Thêm sản phẩm thành công!"
                    ); // Thông báo thành công

                    await LoadProductsAsync(); // Tải lại danh sách
                    return; // Kết thúc
                }

                await ShowWriteErrorAsync(response, "thêm"); // Hiển thị lỗi
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể thêm sản phẩm: " + ex.Message
                ); // Thông báo lỗi
            }
            finally
            {
                btnAdd.Enabled = true; // Bật lại nút
            }
        }

        // ================= PUT SỬA SẢN PHẨM =================

        private async Task UpdateProductAsync() // Sửa sản phẩm
        {
            if (!PrepareAuthorization()) return; // Kiểm tra quyền

            if (!int.TryParse(
                txtProductId.Text.Trim(),
                out int productId) || productId <= 0)
            {
                MessageBox.Show(
                    "Vui lòng chọn sản phẩm cần sửa trong bảng."
                ); // Chưa chọn sản phẩm

                return; // Dừng
            }

            if (!TryGetProductInput(
                out string barcode,
                out string productName,
                out decimal price,
                out int stock,
                out int categoryId))
                return; // Kiểm tra dữ liệu

            DialogResult confirm = MessageBox.Show(
                $"Bạn có chắc muốn sửa sản phẩm ID {productId}?",
                "Xác nhận sửa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            ); // Hỏi xác nhận

            if (confirm != DialogResult.Yes)
                return; // Người dùng hủy

            var updatedProduct = new // Tạo đối tượng PUT
            {
                ProductId = productId, // ID sản phẩm
                Barcode = barcode, // Mã vạch
                ProductName = productName, // Tên sản phẩm
                Price = price, // Giá bán
                StockQuantity = stock, // Số lượng tồn
                CategoryId = categoryId // ID danh mục từ ComboBox
            };

            btnEdit.Enabled = false; // Khóa nút Sửa

            try
            {
                string url = $"{ProductEndpoint}/{productId}"; // URL PUT

                using HttpResponseMessage response =
                    await ApiClientService.Client.PutAsJsonAsync(
                        url,
                        updatedProduct
                    ); // PUT /api/products/{id}

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        "Cập nhật sản phẩm thành công!"
                    ); // Thông báo thành công

                    await LoadProductsAsync(); // Tải lại danh sách
                    return; // Kết thúc
                }

                await ShowWriteErrorAsync(response, "sửa"); // Hiển thị lỗi
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể sửa sản phẩm: " + ex.Message
                ); // Thông báo lỗi
            }
            finally
            {
                btnEdit.Enabled = true; // Bật lại nút
            }
        }

        // ================= DELETE XÓA SẢN PHẨM =================

        private async Task DeleteProductAsync() // Xóa sản phẩm
        {
            if (!PrepareAuthorization()) return; // Kiểm tra quyền

            if (!int.TryParse(
                txtProductId.Text.Trim(),
                out int productId) || productId <= 0)
            {
                MessageBox.Show(
                    "Vui lòng chọn sản phẩm cần xóa trong bảng."
                ); // Chưa chọn sản phẩm

                return; // Dừng
            }

            string productName = txtProductName.Text.Trim(); // Tên sản phẩm

            DialogResult confirm = MessageBox.Show(
                $"Bạn có chắc muốn xóa sản phẩm?\n\n" +
                $"Mã: {productId}\n" +
                $"Tên: {productName}",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            ); // Hỏi xác nhận

            if (confirm != DialogResult.Yes)
                return; // Người dùng hủy

            btnDelete.Enabled = false; // Khóa nút Xóa

            try
            {
                string url = $"{ProductEndpoint}/{productId}"; // URL DELETE

                using HttpResponseMessage response =
                    await ApiClientService.Client.DeleteAsync(
                        url
                    ); // DELETE /api/products/{id}

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        "Xóa sản phẩm thành công!"
                    ); // Thông báo thành công

                    await LoadProductsAsync(); // Tải lại danh sách
                    return; // Kết thúc
                }

                await ShowWriteErrorAsync(response, "xóa"); // Hiển thị lỗi
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể xóa sản phẩm: " + ex.Message
                ); // Thông báo lỗi
            }
            finally
            {
                btnDelete.Enabled = true; // Bật lại nút
            }
        }

        // ================= HIỂN THỊ LỖI POST/PUT/DELETE =================

        private async Task ShowWriteErrorAsync(
            HttpResponseMessage response,
            string action) // Hiển thị lỗi thao tác ghi
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                MessageBox.Show(
                    "Token không hợp lệ hoặc đã hết hạn.",
                    "Lỗi 401"
                ); // Lỗi JWT

                return; // Kết thúc
            }

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                MessageBox.Show(
                    $"Bạn không có quyền {action} sản phẩm.",
                    "Lỗi 403"
                ); // Lỗi phân quyền

                return; // Kết thúc
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                MessageBox.Show(
                    "Không tìm thấy sản phẩm.",
                    "Lỗi 404"
                ); // Không tồn tại

                return; // Kết thúc
            }

            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                MessageBox.Show(
                    "Thao tác bị từ chối do xung đột dữ liệu.",
                    "Lỗi 409"
                ); // Xung đột dữ liệu

                return; // Kết thúc
            }

            string error =
                await response.Content.ReadAsStringAsync(); // Đọc lỗi API

            MessageBox.Show(
                $"Không thể {action} sản phẩm.\n" +
                $"HTTP {(int)response.StatusCode}\n{error}",
                "Lỗi API",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            ); // Hiển thị lỗi chi tiết
        }

        // ================= HIỂN THỊ SẢN PHẨM =================

        private void DisplayProducts(
            List<ProductViewModel> products) // Hiển thị danh sách
        {
            dgvProducts.Rows.Clear(); // Xóa dòng cũ

            ClearProductInputs(); // Xóa thông tin đang chọn

            foreach (ProductViewModel product in products)
            {
                dgvProducts.Rows.Add(
                    product.ProductId, // ID
                    product.Barcode, // Mã vạch
                    product.ProductName, // Tên
                    product.Price, // Giá
                    product.StockQuantity, // Tồn kho
                    product.CategoryId, // ID danh mục
                    product.CategoryName // Tên danh mục
                ); // Thêm một dòng
            }

            dgvProducts.ClearSelection(); // Bỏ chọn dòng
        }

        // ================= CHỌN SẢN PHẨM TRONG BẢNG =================

        private void DgvProducts_CellClick(
            object? sender,
            DataGridViewCellEventArgs e) // Khi nhấn vào dòng
        {
            if (e.RowIndex < 0) return; // Bỏ qua tiêu đề

            DataGridViewRow row =
                dgvProducts.Rows[e.RowIndex]; // Lấy dòng

            txtProductId.Text =
                Convert.ToString(
                    row.Cells["ProductId"].Value
                ) ?? ""; // Hiển thị ID

            txtBarcode.Text =
                Convert.ToString(
                    row.Cells["Barcode"].Value
                ) ?? ""; // Hiển thị mã vạch

            txtProductName.Text =
                Convert.ToString(
                    row.Cells["ProductName"].Value
                ) ?? ""; // Hiển thị tên

            // ---------- GIÁ BÁN ----------

            if (decimal.TryParse(
                Convert.ToString(row.Cells["Price"].Value),
                out decimal price))
            {
                nudPrice.Value = Math.Min(
                    nudPrice.Maximum,
                    Math.Max(nudPrice.Minimum, price)
                ); // Hiển thị giá trong giới hạn
            }

            // ---------- TỒN KHO ----------

            if (decimal.TryParse(
                Convert.ToString(row.Cells["StockQuantity"].Value),
                out decimal stock))
            {
                nudStock.Value = Math.Min(
                    nudStock.Maximum,
                    Math.Max(nudStock.Minimum, stock)
                ); // Hiển thị số lượng
            }

            // ---------- CHỌN DANH MỤC ----------

            if (int.TryParse(
                Convert.ToString(row.Cells["CategoryId"].Value),
                out int categoryId))
            {
                cboCategory.SelectedValue = categoryId; // Chọn theo ID

                if (cboCategory.SelectedIndex < 0)
                {
                    MessageBox.Show(
                        "Danh mục của sản phẩm chưa có trong danh sách."
                    ); // Thông báo danh mục không tồn tại
                }
            }
            else
            {
                cboCategory.SelectedIndex = -1; // Bỏ chọn
            }
        }

        // ================= XÓA THÔNG TIN NHẬP =================

        private void ClearProductInputs() // Làm trống dữ liệu
        {
            txtProductId.Clear(); // Xóa ID
            txtBarcode.Clear(); // Xóa mã vạch
            txtProductName.Clear(); // Xóa tên
            nudPrice.Value = 0; // Đưa giá về 0
            nudStock.Value = 0; // Đưa tồn kho về 0
            cboCategory.SelectedIndex = -1; // Bỏ chọn danh mục
        }

        // ================= MODEL SẢN PHẨM =================

        private class ProductViewModel // Model JSON sản phẩm
        {
            [JsonPropertyName("productId")] // Thuộc tính JSON
            public int ProductId { get; set; } // ID sản phẩm

            [JsonPropertyName("barcode")] // Thuộc tính JSON
            public string Barcode { get; set; } = ""; // Mã vạch

            [JsonPropertyName("productName")] // Thuộc tính JSON
            public string ProductName { get; set; } = ""; // Tên sản phẩm

            [JsonPropertyName("price")] // Thuộc tính JSON
            public decimal Price { get; set; } // Giá bán

            [JsonPropertyName("stockQuantity")] // Thuộc tính JSON
            public int StockQuantity { get; set; } // Tồn kho

            [JsonPropertyName("categoryId")] // Thuộc tính JSON
            public int CategoryId { get; set; } // ID danh mục

            [JsonPropertyName("categoryName")] // Thuộc tính JSON
            public string? CategoryName { get; set; } // Tên danh mục
        }

        // ================= MODEL DANH MỤC =================

        private class CategoryDto // Model JSON danh mục
        {
            [JsonPropertyName("categoryId")] // Thuộc tính JSON
            public int CategoryId { get; set; } // ID danh mục

            [JsonPropertyName("categoryName")] // Thuộc tính JSON
            public string CategoryName { get; set; } = ""; // Tên danh mục
        }

    } // Kết thúc lớp FormProductManagement
} // Kết thúc namespace
