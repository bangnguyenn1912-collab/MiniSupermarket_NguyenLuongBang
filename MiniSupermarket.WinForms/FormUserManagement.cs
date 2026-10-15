
using System; // Kiểu dữ liệu cơ bản
using System.Drawing; // Màu sắc giao diện
using System.Linq; // Tìm dữ liệu
using System.Net.Mail; // Kiểm tra định dạng email
using System.Net.Http.Json; // Gửi nhận JSON
using System.Text.Json; // Đọc lỗi API
using System.Threading.Tasks; // Xử lý bất đồng bộ
using System.Windows.Forms; // Windows Forms

namespace MiniSupermarket.WinForms
{
    public partial class FormUserManagement : Form
    {
        // Các điều khiển giao diện
        private readonly TextBox txtUsername = new();
        private readonly TextBox txtFullName = new();
        private readonly TextBox txtEmail = new();
        private readonly TextBox txtPassword = new();
        private readonly ComboBox cboRole = new();
        private readonly DataGridView dgvUsers = new();
        private readonly Label lblStatus = new();

        // Các nút chức năng
        private readonly Button btnAdd = new();
        private readonly Button btnUpdate = new();
        private readonly Button btnReset = new();
        private readonly Button btnLock = new();
        private readonly Button btnRefresh = new();

        // ID tài khoản đang chọn
        private int? selectedUserId;

        // Trạng thái tài khoản đang chọn
        private bool selectedIsActive;

        // Danh sách tài khoản
        private List<UserManagementDto> users = new();

        public FormUserManagement()
        {
            InitializeComponent(); // Khởi tạo Designer
            BuildInterface(); // Tạo giao diện

            // Tự động tải tài khoản khi Form hiển thị
            Load += async (s, e) => await LoadUsersAsync();
        }

        // ==================== TẠO GIAO DIỆN ====================

        private void BuildInterface()
        {
            Text = "Quản lý tài khoản";
            BackColor = Color.FromArgb(245, 247, 250);
            Font = new Font("Segoe UI", 10);
            MinimumSize = new Size(750, 520);

            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(14),
                ColumnCount = 1,
                RowCount = 4
            };

            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 225));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 90));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            Controls.Add(root);

            root.Controls.Add(new Label
            {
                Text = "QUẢN TRỊ TÀI KHOẢN NHÂN VIÊN",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 17, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 65, 110)
            }, 0, 0);

            // Bảng nhập liệu
            var fields = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 5
            };

            fields.ColumnStyles.Add(
                new ColumnStyle(SizeType.Absolute, 155));

            fields.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 100));

            AddField(fields, 0, "Tên đăng nhập:", txtUsername);
            AddField(fields, 1, "Họ và tên:", txtFullName);
            AddField(fields, 2, "Email:", txtEmail);

            txtPassword.UseSystemPasswordChar = true;
            txtPassword.PlaceholderText = "Ít nhất 8 ký tự";
            AddField(fields, 3, "Mật khẩu:", txtPassword);

            cboRole.DropDownStyle = ComboBoxStyle.DropDownList;
            cboRole.Items.AddRange(new object[]
            {
                "Admin",
                "Cashier",
                "Warehouse"
            });
            cboRole.SelectedIndex = 1;

            fields.Controls.Add(new Label
            {
                Text = "Vai trò:",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            }, 0, 4);

            cboRole.Dock = DockStyle.Fill;
            fields.Controls.Add(cboRole, 1, 4);

            root.Controls.Add(fields, 0, 1);

            // Khu vực nút chức năng
            var actions = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true
            };

            ConfigureButton(btnAdd, "Thêm tài khoản", Color.SeaGreen);
            ConfigureButton(btnUpdate, "Cập nhật", Color.SteelBlue);
            ConfigureButton(btnReset, "Đặt lại mật khẩu", Color.DarkOrange);
            ConfigureButton(btnLock, "Khóa / Mở khóa", Color.Firebrick);
            ConfigureButton(btnRefresh, "Làm mới", Color.SlateGray);

            actions.Controls.AddRange(new Control[]
            {
                btnAdd, btnUpdate, btnReset, btnLock, btnRefresh
            });

            root.Controls.Add(actions, 0, 2);

            // Bảng danh sách tài khoản
            var bottom = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2
            };

            bottom.RowStyles.Add(new RowStyle(SizeType.Absolute, 35));
            bottom.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            lblStatus.Dock = DockStyle.Fill;
            lblStatus.Text = "Đang chờ tải danh sách tài khoản...";
            lblStatus.ForeColor = Color.SteelBlue;

            bottom.Controls.Add(lblStatus, 0, 0);

            dgvUsers.Dock = DockStyle.Fill;
            dgvUsers.ReadOnly = true;
            dgvUsers.AllowUserToAddRows = false;
            dgvUsers.AllowUserToDeleteRows = false;
            dgvUsers.AutoGenerateColumns = false;
            dgvUsers.RowHeadersVisible = false;
            dgvUsers.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgvUsers.MultiSelect = false;
            dgvUsers.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            AddColumn("ID", "Id", 10);
            AddColumn("Tài khoản", "Username", 20);
            AddColumn("Họ tên", "FullName", 25);
            AddColumn("Email", "Email", 25);
            AddColumn("Vai trò", "Role", 15);
            AddColumn("Trạng thái", "StatusText", 15);

            dgvUsers.SelectionChanged += DgvUsers_SelectionChanged;

            bottom.Controls.Add(dgvUsers, 0, 1);
            root.Controls.Add(bottom, 0, 3);

            // Đăng ký sự kiện cho các nút
            btnAdd.Click += async (s, e) => await AddUserAsync();
            btnUpdate.Click += async (s, e) => await UpdateUserAsync();
            btnReset.Click += async (s, e) => await ResetPasswordAsync();
            btnLock.Click += async (s, e) => await ChangeStatusAsync();
            btnRefresh.Click += async (s, e) => await LoadUsersAsync();
        }

        // Tạo một hàng nhập liệu
        private static void AddField(
            TableLayoutPanel panel,
            int row,
            string title,
            TextBox textbox)
        {
            panel.Controls.Add(new Label
            {
                Text = title,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            }, 0, row);

            textbox.Dock = DockStyle.Fill;
            panel.Controls.Add(textbox, 1, row);
        }

        // Thiết lập nút
        private static void ConfigureButton(
            Button button,
            string title,
            Color color)
        {
            button.Text = title;
            button.Width = 145;
            button.Height = 38;
            button.BackColor = color;
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
        }

        // Tạo cột cho DataGridView
        private void AddColumn(string title, string property, float width)
        {
            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = title,
                DataPropertyName = property,
                FillWeight = width
            });
        }

        // ==================== TẢI DANH SÁCH ====================

        private async Task LoadUsersAsync()
        {
            try
            {
                lblStatus.Text = "Đang tải dữ liệu...";
                lblStatus.ForeColor = Color.SteelBlue;

                using var response =
                    await ApiClientService.Client.GetAsync("Users");

                if (!response.IsSuccessStatusCode)
                {
                    await ShowApiErrorAsync(response);
                    return;
                }

                users = await response.Content
                    .ReadFromJsonAsync<List<UserManagementDto>>()
                    ?? new List<UserManagementDto>();

                // Làm mới bảng
                dgvUsers.DataSource = null;
                dgvUsers.DataSource = users;

                // Bỏ lựa chọn mặc định
                dgvUsers.ClearSelection();
                selectedUserId = null;
                txtUsername.ReadOnly = false;

                lblStatus.Text =
                    $"Đã tải {users.Count} tài khoản từ API.";

                lblStatus.ForeColor = Color.DarkGreen;
            }
            catch (Exception ex)
            {
                ShowError("Không thể kết nối API: " + ex.Message);
            }
        }

        // ==================== CHỌN TÀI KHOẢN ====================

        private void DgvUsers_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0)
                return;

            if (dgvUsers.SelectedRows[0].DataBoundItem
                is not UserManagementDto user)
                return;

            selectedUserId = user.Id;
            selectedIsActive = user.IsActive;

            txtUsername.Text = user.Username;
            txtUsername.ReadOnly = true;

            txtFullName.Text = user.FullName;
            txtEmail.Text = user.Email;
            txtPassword.Clear();

            cboRole.SelectedItem = user.Role;

            btnLock.Text = user.IsActive
                ? "Khóa tài khoản"
                : "Mở khóa";
        }

        // ==================== THÊM TÀI KHOẢN ====================

        private async Task AddUserAsync()
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtFullName.Text) ||
                !IsValidEmail(txtEmail.Text) ||
                txtPassword.Text.Length < 8)
            {
                MessageBox.Show(
                    "Vui lòng nhập tài khoản, họ tên, email hợp lệ " +
                    "và mật khẩu ít nhất 8 ký tự.");
                return;
            }

            try
            {
                var request = new
                {
                    username = txtUsername.Text.Trim(),
                    fullName = txtFullName.Text.Trim(),
                    email = txtEmail.Text.Trim(),
                    password = txtPassword.Text,
                    role = cboRole.Text
                };

                using var response =
                    await ApiClientService.Client.PostAsJsonAsync(
                        "Users", request);

                if (!response.IsSuccessStatusCode)
                {
                    await ShowApiErrorAsync(response);
                    return;
                }

                MessageBox.Show("Thêm tài khoản thành công.");

                ClearInputs();
                await LoadUsersAsync();
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        // ==================== CẬP NHẬT TÀI KHOẢN ====================

        private async Task UpdateUserAsync()
        {
            if (selectedUserId == null)
            {
                MessageBox.Show("Vui lòng chọn tài khoản cần cập nhật.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtFullName.Text) ||
                !IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Họ tên và email hợp lệ không được để trống.");
                return;
            }

            try
            {
                var request = new
                {
                    fullName = txtFullName.Text.Trim(),
                    email = txtEmail.Text.Trim(),
                    role = cboRole.Text
                };

                using var response =
                    await ApiClientService.Client.PutAsJsonAsync(
                        $"Users/{selectedUserId.Value}", request);

                if (!response.IsSuccessStatusCode)
                {
                    await ShowApiErrorAsync(response);
                    return;
                }

                MessageBox.Show("Cập nhật tài khoản thành công.");
                await LoadUsersAsync();
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        // ==================== ĐẶT LẠI MẬT KHẨU ====================

        private async Task ResetPasswordAsync()
        {
            if (selectedUserId == null)
            {
                MessageBox.Show("Vui lòng chọn tài khoản.");
                return;
            }

            if (txtPassword.Text.Length < 8)
            {
                MessageBox.Show("Mật khẩu mới phải có ít nhất 8 ký tự.");
                return;
            }

            if (MessageBox.Show(
                "Bạn có chắc chắn muốn đặt lại mật khẩu?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                var request = new
                {
                    newPassword = txtPassword.Text
                };

                using var response =
                    await ApiClientService.Client.PutAsJsonAsync(
                        $"Users/{selectedUserId.Value}/password",
                        request);

                if (!response.IsSuccessStatusCode)
                {
                    await ShowApiErrorAsync(response);
                    return;
                }

                txtPassword.Clear();

                MessageBox.Show("Đặt lại mật khẩu thành công.");
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        // ==================== KHÓA / MỞ KHÓA ====================

        private async Task ChangeStatusAsync()
        {
            if (selectedUserId == null)
            {
                MessageBox.Show("Vui lòng chọn tài khoản.");
                return;
            }

            bool newStatus = !selectedIsActive;

            string action = newStatus ? "mở khóa" : "khóa";

            if (MessageBox.Show(
                $"Bạn có chắc chắn muốn {action} tài khoản này?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                var request = new
                {
                    isActive = newStatus
                };

                using var response =
                    await ApiClientService.Client.PutAsJsonAsync(
                        $"Users/{selectedUserId.Value}/status",
                        request);

                if (!response.IsSuccessStatusCode)
                {
                    await ShowApiErrorAsync(response);
                    return;
                }

                MessageBox.Show(
                    newStatus
                        ? "Mở khóa tài khoản thành công."
                        : "Khóa tài khoản thành công.");

                await LoadUsersAsync();
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        // ==================== XÓA NỘI DUNG NHẬP ====================

        private void ClearInputs()
        {
            selectedUserId = null;

            dgvUsers.ClearSelection();

            txtUsername.ReadOnly = false;
            txtUsername.Clear();
            txtFullName.Clear();
            txtEmail.Clear();
            txtPassword.Clear();

            cboRole.SelectedIndex = 1;
            btnLock.Text = "Khóa / Mở khóa";
        }

        // Kiểm tra ở giao diện để báo lỗi sớm; API vẫn kiểm tra lại lần nữa.
        private static bool IsValidEmail(string value)
        {
            string email = value.Trim();

            return email.Length <= 150 &&
                   MailAddress.TryCreate(email, out var parsed) &&
                   parsed.Address.Equals(
                       email,
                       StringComparison.OrdinalIgnoreCase);
        }

        // ==================== XỬ LÝ LỖI API ====================

        private async Task ShowApiErrorAsync(
            System.Net.Http.HttpResponseMessage response)
        {
            string message = await response.Content.ReadAsStringAsync();

            // Chỉ lấy nội dung thông báo khi API trả JSON
            try
            {
                using var json = JsonDocument.Parse(message);

                if (json.RootElement.ValueKind == JsonValueKind.Object)
                {
                    if (json.RootElement.TryGetProperty(
                        "message", out var property))
                    {
                        message = property.ToString();
                    }
                    else if (json.RootElement.TryGetProperty(
                        "title", out var title))
                    {
                        message = title.ToString();
                    }
                }
            }
            catch (JsonException)
            {
                // Giữ nguyên thông báo dạng văn bản
            }

            ShowError(
                $"HTTP {(int)response.StatusCode}: {message}");
        }

        private void ShowError(string message)
        {
            lblStatus.Text = "Lỗi: " + message;
            lblStatus.ForeColor = Color.Firebrick;

            MessageBox.Show(
                message,
                "Lỗi quản lý tài khoản",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    // DTO hiển thị tài khoản, không chứa mật khẩu
    public class UserManagementDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = "";
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Role { get; set; } = "";
        public bool IsActive { get; set; }

        // Hiển thị trạng thái trong DataGridView
        public string StatusText =>
            IsActive ? "Hoạt động" : "Đã khóa";
    }
}
