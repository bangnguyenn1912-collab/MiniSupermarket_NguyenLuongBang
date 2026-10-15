
using System; // Sử dụng các kiểu dữ liệu và sự kiện cơ bản
using System.Drawing; // Sử dụng màu sắc giao diện
using System.Windows.Forms; // Sử dụng Windows Forms

namespace MiniSupermarket.WinForms // Namespace ứng dụng WinForms
{
    // Form chính quản lý Sidebar, Header và các Form con
    public partial class FormMainShell : Form
    {
        // Màu nền mặc định của Sidebar
        private static readonly Color SidebarColor =
            Color.FromArgb(24, 30, 48);

        // Màu nút đang được chọn
        private static readonly Color ActiveMenuColor =
            Color.FromArgb(41, 100, 180);

        // Danh sách tất cả các nút Sidebar
        private readonly Button[] _menuButtons;

        // Lưu Form con đang hiển thị
        private Form? _activeForm;

        // ==================== KHỞI TẠO FORM ====================

        public FormMainShell()
        {
            // Khởi tạo các thành phần từ Designer
            InitializeComponent();

            // Đưa tất cả nút Sidebar vào danh sách
            _menuButtons = new[]
            {
                btnPOS, // Bán hàng POS
                btnCategory, // Quản lý danh mục
                btnProduct, // Quản lý sản phẩm
                btnCustomer, // Quản lý khách hàng
                btnReports, // Báo cáo doanh thu
                btnUserManage, // Quản lý tài khoản
                btnRoles // Quản lý vai trò
            };
        }

        // ==================== SỰ KIỆN LOAD ====================

        private void FormMainShell_Load(object? sender, EventArgs e)
        {
            // Không chạy logic đăng nhập khi mở Designer
            if (IsDesignerActive())
                return;

            // Hiển thị thông tin nhân viên
            UpdateHeaderUserInfo();

            // Áp dụng phân quyền theo vai trò
            ApplyRolePermissions(SessionManager.CurrentRole);
        }

        // ==================== SỰ KIỆN SHOWN ====================

        private void FormMainShell_Shown(object? sender, EventArgs e)
        {
            // Không mở Form con khi đang ở Designer
            if (IsDesignerActive())
                return;

            // Mở màn hình mặc định theo vai trò
            OpenDefaultScreenByRole(SessionManager.CurrentRole);
        }

        // ==================== KIỂM TRA DESIGNER ====================

        private bool IsDesignerActive()
        {
            // Kiểm tra Visual Studio có đang mở Designer không
            return System.ComponentModel.LicenseManager.UsageMode ==
                       System.ComponentModel.LicenseUsageMode.Designtime ||
                   DesignMode ||
                   Site?.DesignMode == true;
        }

        // ==================== HIỂN THỊ THÔNG TIN NHÂN VIÊN ====================

        private void UpdateHeaderUserInfo()
        {
            // Ưu tiên tên đầy đủ, nếu không có thì dùng Username
            string displayName =
                string.IsNullOrWhiteSpace(SessionManager.CurrentFullName)
                    ? SessionManager.CurrentUsername
                    : SessionManager.CurrentFullName;

            // Hiển thị tên và vai trò trên Header
            lblUserInfo.Text =
                $"Nhân viên: {displayName} | Vai trò: {SessionManager.CurrentRole}";
        }

        // ==================== NÚT BÁN HÀNG POS ====================

        private void btnPOS_Click(object? sender, EventArgs e)
        {
            // Mở Form bán hàng tại quầy
            OpenChildForm(
                new FormPOS(),
                "BÁN HÀNG TẠI QUẦY (POS)",
                btnPOS);
        }

        // ==================== NÚT QUẢN LÝ DANH MỤC ====================

        private void btnCategory_Click(object? sender, EventArgs e)
        {
            // Mở Form quản lý danh mục
            OpenChildForm(
                new FormCategoryManagement(),
                "QUẢN LÝ DANH MỤC",
                btnCategory);
        }

        // ==================== NÚT QUẢN LÝ SẢN PHẨM ====================

        private void btnProduct_Click(object? sender, EventArgs e)
        {
            // Mở Form quản lý sản phẩm
            OpenChildForm(
                new FormProductManagement(),
                "QUẢN LÝ SẢN PHẨM",
                btnProduct);
        }

        // ==================== NÚT QUẢN LÝ KHÁCH HÀNG ====================

        private void btnCustomer_Click(object? sender, EventArgs e)
        {
            // Mở Form quản lý khách hàng
            OpenChildForm(
                new FormCustomerManagement(),
                "QUẢN LÝ KHÁCH HÀNG & TÍCH ĐIỂM",
                btnCustomer);
        }

        // ==================== NÚT BÁO CÁO DOANH THU ====================

        private void btnReports_Click(object? sender, EventArgs e)
        {
            // Mở Form báo cáo doanh thu
            OpenChildForm(
                new FormQuickReport(),
                "BÁO CÁO DOANH THU",
                btnReports);
        }

        // ==================== NÚT QUẢN LÝ TÀI KHOẢN ====================

        private void btnUserManage_Click(object? sender, EventArgs e)
        {
            // Mở Form quản lý tài khoản nhân viên
            OpenChildForm(
                new FormUserManagement(),
                "QUẢN LÝ TÀI KHOẢN & PHÂN QUYỀN",
                btnUserManage);
        }

        // ==================== NÚT QUẢN LÝ VAI TRÒ ====================

        private void btnRoles_Click(object? sender, EventArgs e)
        {
            // Mở Form quản lý vai trò
            OpenChildForm(
                new FormRoleManagement(),
                "QUẢN LÝ VAI TRÒ",
                btnRoles);
        }

        // ==================== PHÂN QUYỀN SIDEBAR ====================

        private void ApplyRolePermissions(string role)
        {
            // Ẩn tất cả menu trước khi cấp quyền
            HideAllMenus();

            // Kiểm tra vai trò, không phân biệt chữ hoa chữ thường
            switch ((role ?? "").Trim().ToUpperInvariant())
            {
                case "ADMIN":
                    // Admin được sử dụng tất cả menu
                    ConfigureAdminMenu();
                    break;

                case "CASHIER":
                    // Cashier chỉ được bán hàng và quản lý khách hàng
                    ConfigureCashierMenu();
                    break;

                case "WAREHOUSE":
                    // Warehouse được quản lý danh mục và sản phẩm
                    ConfigureWarehouseMenu();
                    break;

                default:
                    // Vai trò không hợp lệ sẽ không có menu
                    break;
            }
        }

        // ==================== MENU ADMIN ====================

        private void ConfigureAdminMenu()
        {
            btnPOS.Visible = true; // Bán hàng
            btnCategory.Visible = true; // Danh mục
            btnProduct.Visible = true; // Sản phẩm
            btnCustomer.Visible = true; // Khách hàng
            btnReports.Visible = true; // Báo cáo
            btnUserManage.Visible = true; // Tài khoản
            btnRoles.Visible = true; // Vai trò
        }

        // ==================== MENU CASHIER ====================

        private void ConfigureCashierMenu()
        {
            btnPOS.Visible = true; // Được bán hàng
            btnCustomer.Visible = true; // Được quản lý khách hàng
        }

        // ==================== MENU WAREHOUSE ====================

        private void ConfigureWarehouseMenu()
        {
            btnCategory.Visible = true; // Được quản lý danh mục
            btnProduct.Visible = true; // Được quản lý sản phẩm
        }

        // ==================== ẨN TẤT CẢ MENU ====================

        private void HideAllMenus()
        {
            // Duyệt tất cả nút Sidebar
            foreach (Button button in _menuButtons)
            {
                button.Visible = false; // Ẩn nút
                button.BackColor = SidebarColor; // Khôi phục màu mặc định
            }
        }

        // ==================== KIỂM TRA QUYỀN MỞ FORM ====================

        private bool CanOpenMenu(Button button)
        {
            // Chưa đăng nhập thì không được mở Form
            if (!SessionManager.IsLoggedIn)
                return false;

            // Lấy vai trò hiện tại
            string role = (SessionManager.CurrentRole ?? "").Trim();

            // Xác định quyền Admin
            bool isAdmin = role.Equals(
                "Admin",
                StringComparison.OrdinalIgnoreCase);

            // Xác định quyền Cashier
            bool isCashier = role.Equals(
                "Cashier",
                StringComparison.OrdinalIgnoreCase);

            // Xác định quyền Warehouse
            bool isWarehouse = role.Equals(
                "Warehouse",
                StringComparison.OrdinalIgnoreCase);

            // POS chỉ dành cho Admin và Cashier
            if (ReferenceEquals(button, btnPOS))
                return isAdmin || isCashier;

            // Danh mục dành cho Admin và Warehouse
            if (ReferenceEquals(button, btnCategory))
                return isAdmin || isWarehouse;

            // Sản phẩm dành cho Admin và Warehouse
            if (ReferenceEquals(button, btnProduct))
                return isAdmin || isWarehouse;

            // Khách hàng dành cho Admin và Cashier
            if (ReferenceEquals(button, btnCustomer))
                return isAdmin || isCashier;

            // Báo cáo, tài khoản và vai trò chỉ dành cho Admin
            if (ReferenceEquals(button, btnReports) ||
                ReferenceEquals(button, btnUserManage) ||
                ReferenceEquals(button, btnRoles))
            {
                return isAdmin;
            }

            // Không thuộc menu hợp lệ thì từ chối
            return false;
        }

        // ==================== MÀN HÌNH MẶC ĐỊNH ====================

        private void OpenDefaultScreenByRole(string role)
        {
            // Chọn màn hình mặc định theo vai trò
            switch ((role ?? "").Trim().ToUpperInvariant())
            {
                case "ADMIN":
                    // Admin mở quản lý danh mục mặc định
                    OpenChildForm(
                        new FormCategoryManagement(),
                        "QUẢN LÝ DANH MỤC",
                        btnCategory);
                    break;

                case "WAREHOUSE":
                    // Warehouse mở quản lý sản phẩm mặc định
                    OpenChildForm(
                        new FormProductManagement(),
                        "QUẢN LÝ SẢN PHẨM",
                        btnProduct);
                    break;

                case "CASHIER":
                    // Cashier mở bán hàng POS mặc định
                    OpenChildForm(
                        new FormPOS(),
                        "BÁN HÀNG TẠI QUẦY (POS)",
                        btnPOS);
                    break;

                default:
                    // Từ chối vai trò không hợp lệ
                    MessageBox.Show(
                        "Vai trò hiện tại không được phép sử dụng hệ thống.",
                        "Từ chối truy cập",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    // Xóa thông tin đăng nhập
                    SessionManager.Clear();

                    // Đóng Form chính
                    Close();
                    break;
            }
        }

        // ==================== MỞ FORM CON ====================

        private void OpenChildForm(
            Form childForm,
            string screenTitle,
            Button senderButton)
        {
            // Kiểm tra quyền trước khi mở Form
            if (!CanOpenMenu(senderButton))
            {
                // Giải phóng Form vừa tạo nhưng không được phép mở
                childForm.Dispose();

                // Hiển thị thông báo từ chối
                MessageBox.Show(
                    "Bạn không có quyền mở màn hình này.",
                    "Từ chối truy cập",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            // Đóng Form con cũ nếu đang tồn tại
            if (_activeForm != null)
            {
                _activeForm.Close(); // Đóng Form cũ
                _activeForm = null; // Xóa tham chiếu
            }

            // Xóa các điều khiển cũ trong vùng nội dung
            panelMainContent.Controls.Clear();

            // Lưu Form mới đang hoạt động
            _activeForm = childForm;

            // Không hiển thị Form con như cửa sổ độc lập
            childForm.TopLevel = false;

            // Ẩn viền cửa sổ của Form con
            childForm.FormBorderStyle = FormBorderStyle.None;

            // Cho Form con lấp đầy vùng nội dung
            childForm.Dock = DockStyle.Fill;

            // Theo dõi khi Form con đóng
            childForm.FormClosed += (_, _) =>
            {
                // Nếu đúng Form đang hoạt động thì xóa tham chiếu
                if (ReferenceEquals(_activeForm, childForm))
                {
                    _activeForm = null;
                }
            };

            // Đưa Form con vào vùng Main Content
            panelMainContent.Controls.Add(childForm);

            // Đưa Form con lên phía trước
            childForm.BringToFront();

            // Hiển thị Form con
            childForm.Show();

            // Cập nhật tiêu đề Header
            lblTitle.Text = screenTitle.ToUpperInvariant();

            // Đổi màu nút đang chọn
            HighlightActiveButton(senderButton);
        }

        // ==================== ĐÁNH DẤU MENU ĐANG CHỌN ====================

        private void HighlightActiveButton(Button activeButton)
        {
            // Khôi phục màu mặc định của tất cả nút
            foreach (Button button in _menuButtons)
            {
                button.BackColor = SidebarColor;
            }

            // Đổi màu nút đang được chọn
            activeButton.BackColor = ActiveMenuColor;
        }

        // ==================== ĐĂNG XUẤT ====================

        private void btnLogout_Click(object? sender, EventArgs e)
        {
            // Yêu cầu xác nhận trước khi đăng xuất
            DialogResult confirm = MessageBox.Show(
                "Bạn có chắc chắn muốn đăng xuất khỏi hệ thống?",
                "Xác nhận đăng xuất",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            // Nếu người dùng không đồng ý thì hủy
            if (confirm != DialogResult.Yes)
                return;

            // Đóng Form con đang hoạt động
            _activeForm?.Close();

            // Xóa tham chiếu Form con
            _activeForm = null;

            // Xóa giao diện vùng nội dung
            panelMainContent.Controls.Clear();

            // Xóa JWT và thông tin phiên đăng nhập
            SessionManager.Clear();

            // Đóng Form chính
            Close();
        }

        // ==================== KHI ĐÓNG FORM CHÍNH ====================

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            // Đóng Form con nếu còn tồn tại
            if (_activeForm != null)
            {
                _activeForm.Close();
                _activeForm = null;
            }

            // Gọi xử lý đóng Form của lớp cha
            base.OnFormClosed(e);
        }
    }
}
