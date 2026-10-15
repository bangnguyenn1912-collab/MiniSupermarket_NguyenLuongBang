using System; // Kiểu dữ liệu
using System.Collections.Generic; // Danh sách hóa đơn
using System.Drawing; // Giao diện
using System.Net.Http.Json; // Đọc JSON từ API
using System.Text.Json; // Đọc nội dung lỗi API
using System.Threading.Tasks; // Xử lý bất đồng bộ
using System.Windows.Forms; // Windows Forms

namespace MiniSupermarket.WinForms
{
    public partial class FormQuickReport : Form
    {
        private readonly Label lblOrders = new(); // Số hóa đơn
        private readonly Label lblRevenue = new(); // Doanh thu
        private readonly Label lblBestSeller = new(); // Sản phẩm bán chạy
        private readonly DataGridView dgvReport = new(); // Bảng hóa đơn
        private readonly DateTimePicker dtpReportDate = new(); // Ngày báo cáo
        private readonly Button btnRunReport = new(); // Chạy báo cáo

        public FormQuickReport()
        {
            InitializeComponent(); // Khởi tạo Form từ Designer
            BuildInterface(); // Tạo giao diện báo cáo
            Load += FormQuickReport_Load;
        }

        private void BuildInterface()
        {
            Text = "Báo cáo doanh thu";
            BackColor = Color.FromArgb(245, 247, 250);
            Font = new Font("Segoe UI", 10);
            MinimumSize = new Size(680, 470);

            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(16),
                RowCount = 4,
                ColumnCount = 1
            };

            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 55));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 125));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 55));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            Controls.Add(root);

            root.Controls.Add(new Label
            {
                Text = "BÁO CÁO DOANH THU & HIỆU SUẤT",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 17, FontStyle.Bold),
                ForeColor = Color.FromArgb(35, 70, 120)
            }, 0, 0);

            var cards = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3
            };

            for (int i = 0; i < 3; i++)
            {
                cards.ColumnStyles.Add(
                    new ColumnStyle(SizeType.Percent, 33.33F));
            }

            cards.Controls.Add(
                MakeCard("TỔNG HÓA ĐƠN", lblOrders), 0, 0);
            cards.Controls.Add(
                MakeCard("DOANH THU", lblRevenue), 1, 0);
            cards.Controls.Add(
                MakeCard("SẢN PHẨM BÁN CHẠY", lblBestSeller), 2, 0);
            root.Controls.Add(cards, 0, 1);

            var toolbar = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                WrapContents = false,
                Padding = new Padding(0, 6, 0, 0)
            };

            toolbar.Controls.Add(new Label
            {
                Text = "Ngày báo cáo:",
                AutoSize = true,
                Margin = new Padding(0, 8, 8, 0)
            });

            dtpReportDate.Format = DateTimePickerFormat.Short;
            dtpReportDate.Value = DateTime.Today;
            dtpReportDate.Width = 150;
            toolbar.Controls.Add(dtpReportDate);

            btnRunReport.Text = "Chạy báo cáo";
            btnRunReport.Width = 150;
            btnRunReport.Height = 34;
            btnRunReport.BackColor = Color.SteelBlue;
            btnRunReport.ForeColor = Color.White;
            btnRunReport.FlatStyle = FlatStyle.Flat;
            btnRunReport.Click += async (s, e) =>
                await LoadReportAsync();
            toolbar.Controls.Add(btnRunReport);
            root.Controls.Add(toolbar, 0, 2);

            dgvReport.Dock = DockStyle.Fill;
            dgvReport.ReadOnly = true;
            dgvReport.AllowUserToAddRows = false;
            dgvReport.AllowUserToDeleteRows = false;
            dgvReport.RowHeadersVisible = false;
            dgvReport.AutoGenerateColumns = false;
            dgvReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvReport.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            AddColumn("Mã hóa đơn", "OrderId", "N0", 18);
            AddColumn("Thời gian", "OrderDate", "dd/MM/yyyy HH:mm", 27);
            AddColumn("Thu ngân", "CashierUsername", "", 25);
            AddColumn("Tổng tiền", "TotalAmount", "N0", 30);

            root.Controls.Add(dgvReport, 0, 3);
        }

        // Kiểm tra quyền ngay trong Form, không chỉ dựa vào nút Sidebar.
        private void FormQuickReport_Load(object? sender, EventArgs e)
        {
            bool isAdmin = SessionManager.IsLoggedIn &&
                SessionManager.CurrentRole.Equals(
                    "Admin",
                    StringComparison.OrdinalIgnoreCase);

            btnRunReport.Enabled = isAdmin;
            dtpReportDate.Enabled = isAdmin;

            if (!isAdmin)
            {
                MessageBox.Show(
                    "Chỉ Admin được phép xem báo cáo doanh thu.",
                    "Từ chối truy cập",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        // Gọi API báo cáo thật và chỉ cập nhật giao diện khi HTTP thành công.
        private async Task LoadReportAsync()
        {
            if (!SessionManager.IsLoggedIn ||
                !SessionManager.CurrentRole.Equals(
                    "Admin",
                    StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Chỉ Admin được phép chạy báo cáo.");
                return;
            }

            btnRunReport.Enabled = false;
            btnRunReport.Text = "Đang tải...";

            try
            {
                string date = dtpReportDate.Value.ToString("yyyy-MM-dd");
                using var response = await ApiClientService.Client.GetAsync(
                    $"Orders/quick-report?date={date}");

                if (!response.IsSuccessStatusCode)
                {
                    await ShowApiErrorAsync(response);
                    return;
                }

                QuickReportDto? report = await response.Content
                    .ReadFromJsonAsync<QuickReportDto>();

                if (report == null)
                {
                    MessageBox.Show(
                        "API không trả dữ liệu báo cáo hợp lệ.",
                        "Lỗi báo cáo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                lblOrders.Text = report.TotalOrders.ToString("N0");
                lblRevenue.Text = $"{report.TotalRevenue:N0} đ";
                lblBestSeller.Text = report.BestSellerQuantity > 0
                    ? $"{report.BestSeller}\n({report.BestSellerQuantity:N0} sản phẩm)"
                    : "Chưa có dữ liệu";

                dgvReport.DataSource = null;
                dgvReport.DataSource = report.Orders;
                dgvReport.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể kết nối API báo cáo: " + ex.Message,
                    "Lỗi báo cáo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                btnRunReport.Enabled = true;
                btnRunReport.Text = "Chạy báo cáo";
            }
        }

        private static async Task ShowApiErrorAsync(
            System.Net.Http.HttpResponseMessage response)
        {
            string message = await response.Content.ReadAsStringAsync();

            try
            {
                using var json = JsonDocument.Parse(message);
                if (json.RootElement.ValueKind == JsonValueKind.Object &&
                    json.RootElement.TryGetProperty(
                        "message", out var property))
                {
                    message = property.ToString();
                }
            }
            catch (JsonException)
            {
                // Giữ nguyên lỗi dạng văn bản nếu API không trả JSON.
            }

            MessageBox.Show(
                $"HTTP {(int)response.StatusCode}: {message}",
                "Lỗi báo cáo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        private void AddColumn(
            string title,
            string property,
            string format,
            float fillWeight)
        {
            dgvReport.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = title,
                DataPropertyName = property,
                FillWeight = fillWeight,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Format = format
                }
            });
        }

        // Tạo thẻ thống kê.
        private static Panel MakeCard(string title, Label valueLabel)
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Margin = new Padding(6)
            };

            var titleLabel = new Label
            {
                Text = title,
                Dock = DockStyle.Top,
                Height = 40,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            valueLabel.Text = "Chưa có dữ liệu";
            valueLabel.Dock = DockStyle.Fill;
            valueLabel.TextAlign = ContentAlignment.MiddleCenter;
            valueLabel.ForeColor = Color.DarkOrange;
            valueLabel.Font = new Font("Segoe UI", 12, FontStyle.Bold);

            panel.Controls.Add(valueLabel);
            panel.Controls.Add(titleLabel);
            return panel;
        }
    }

    public class QuickReportDto
    {
        public DateTime ReportDate { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public string? BestSeller { get; set; }
        public int BestSellerQuantity { get; set; }
        public List<QuickReportOrderDto> Orders { get; set; } = new();
    }

    public class QuickReportOrderDto
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string CashierUsername { get; set; } = "";
        public decimal TotalAmount { get; set; }
    }
}
