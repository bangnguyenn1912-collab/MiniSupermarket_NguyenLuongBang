namespace MiniSupermarket.WinForms
{
    public partial class FormLogin : Form
    {
        private FormMainShell? _mainShell;

        public FormLogin()
        {
            InitializeComponent();
        }

        // Đăng nhập xong chỉ mở Shell; các màn hình nghiệp vụ sẽ nằm bên trong Shell.
        private async void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUser.Text.Trim();
            string password = txtPass.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show(
                    "Vui lòng nhập đầy đủ tài khoản và mật khẩu!",
                    "Cảnh báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            btnLogin.Enabled = false;
            try
            {
                if (!await ApiClientService.LoginAsync(username, password))
                {
                    MessageBox.Show(
                        "Sai tài khoản hoặc mật khẩu!",
                        "Đăng nhập thất bại",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                _mainShell = new FormMainShell();
                _mainShell.FormClosed += MainShell_FormClosed;

                Hide();
                _mainShell.Show();
            }
            catch (Exception ex)
            {
                SessionManager.Clear();
                MessageBox.Show(
                    "Không thể đăng nhập vào máy chủ API.\n\n" + ex.Message,
                    "Lỗi kết nối",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                if (!IsDisposed)
                    btnLogin.Enabled = true;
            }
        }

        private void MainShell_FormClosed(object? sender, FormClosedEventArgs e)
        {
            _mainShell = null;

            if (SessionManager.IsLoggedIn)
            {
                // Đóng Shell bằng nút X kết thúc ứng dụng; nút Đăng xuất đã xóa phiên trước đó.
                SessionManager.Clear();
                Close();
                return;
            }

            txtPass.Clear();
            Show();
            Activate();
        }
    }
}
