using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace MiniSupermarket.WinForms
{
    public partial class FormLogin : Form
    {
        private readonly HttpClient _client;

        public FormLogin()
        {
            InitializeComponent();

            _client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7123/api/")
            };

            txtPassword.UseSystemPasswordChar = true;
        }

        private void txtPassword_Click(object sender, EventArgs e)
        {

        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show(
                    "Vui lòng nhập tài khoản và mật khẩu!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            try
            {
                var loginData = new
                {
                    Username = username,
                    Password = password
                };

                var response = await _client.PostAsJsonAsync(
                    "auth/login",
                    loginData);

                if (response.IsSuccessStatusCode)
                {
                    string jsonString =
                        await response.Content.ReadAsStringAsync();

                    using var doc = JsonDocument.Parse(jsonString);

                    SessionManager.JwtToken =
                        doc.RootElement
                            .GetProperty("token")
                            .GetString() ?? string.Empty;

                    SessionManager.CurrentRole =
                        doc.RootElement
                            .GetProperty("role")
                            .GetString() ?? string.Empty;

                    MessageBox.Show(
                        $"Đăng nhập thành công với quyền: {SessionManager.CurrentRole}",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    FormCategoryManagement mainForm =
                        new FormCategoryManagement();

                    this.Hide();

                    mainForm.ShowDialog();

                    this.Close();
                }
                else
                {
                    MessageBox.Show(
                        "Sai tài khoản hoặc mật khẩu!",
                        "Đăng nhập thất bại",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể kết nối đến Server.\n\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}