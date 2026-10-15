
using System; // Kiểu dữ liệu cơ bản
using System.Collections.Generic; // Danh sách sản phẩm
using System.Drawing; // Màu sắc giao diện
using System.Globalization; // Chuyển đổi số tiền theo thiết lập Windows
using System.Linq; // Tính tổng và tìm sản phẩm
using System.Net; // Mã trạng thái HTTP
using System.Net.Http.Json; // Đọc dữ liệu JSON
using System.Threading.Tasks; // Xử lý bất đồng bộ
using System.Windows.Forms; // Windows Forms
using MiniSupermarket.WinForms.Services; // Service gọi API thanh toán

namespace MiniSupermarket.WinForms
{
    public partial class FormPOS : Form
    {
        // Danh sách sản phẩm đang nằm trong giỏ hàng
        private readonly List<POSCartItem> _cart = new();

        // Service dùng HttpClient và JWT chung của ứng dụng
        private readonly PosCheckoutService _checkoutService = new();

        // Chặn Click/F9 liên tiếp trong lúc API đang xử lý thanh toán
        private bool _isCheckoutInProgress;

        // Chặn gọi trùng API khi người dùng vừa nhấn Enter vừa rời ô điện thoại.
        private bool _isCustomerLookupInProgress;

        // Khai báo các điều khiển giao diện
        private readonly TextBox txtBarcode = new();
        private readonly TextBox txtCustomerPhone = new();
        private readonly TextBox txtCashReceived = new();
        private readonly Label lblCustomerName = new();
        private readonly Label lblTotalAmount = new();
        private readonly Label lblChange = new();
        private readonly DataGridView dgvCart = new();
        private readonly Button btnCheckout = new();
        private readonly Button btnClearCart = new();

        public FormPOS()
        {
            InitializeComponent(); // Khởi tạo Form từ Designer
            BuildInterface(); // Tạo giao diện bán hàng
            KeyPreview = true; // Cho Form nhận phím F9 trước Control con
            KeyDown += FormPOS_KeyDown;
            UpdateCartDisplay(); // Cập nhật giỏ hàng ban đầu
        }

        // Tạo giao diện POS bằng C#
        private void BuildInterface()
        {
            Text = "Bán hàng tại quầy - POS"; // Tiêu đề Form
            BackColor = Color.FromArgb(245, 247, 250); // Màu nền
            Font = new Font("Segoe UI", 10); // Font mặc định
            MinimumSize = new Size(780, 520); // Kích thước tối thiểu

            // Chia Form thành hai khu vực
            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                Padding = new Padding(12),
                BackColor = BackColor
            };

            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65)); // Trái 65%
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35)); // Phải 35%

            Controls.Add(layout); // Đưa bố cục vào Form

            // Khu vực giỏ hàng bên trái
            var left = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 3,
                Padding = new Padding(8)
            };

            left.RowStyles.Add(new RowStyle(SizeType.Absolute, 45)); // Tiêu đề
            left.RowStyles.Add(new RowStyle(SizeType.Absolute, 55)); // Mã vạch
            left.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); // Bảng giỏ hàng

            layout.Controls.Add(left, 0, 0);

            left.Controls.Add(new Label
            {
                Text = "GIỎ HÀNG BÁN LẺ",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 65, 110),
                TextAlign = ContentAlignment.MiddleLeft
            }, 0, 0);

            txtBarcode.Dock = DockStyle.Fill; // Ô nhập mã vạch
            txtBarcode.PlaceholderText = "Nhập mã vạch rồi nhấn Enter...";
            txtBarcode.Font = new Font("Segoe UI", 13);
            txtBarcode.KeyDown += TxtBarcode_KeyDown; // Bắt phím Enter
            left.Controls.Add(txtBarcode, 0, 1);

            dgvCart.Dock = DockStyle.Fill; // Bảng giỏ hàng
            dgvCart.ReadOnly = true; // Không sửa trực tiếp trong bảng
            dgvCart.AllowUserToAddRows = false; // Không cho thêm dòng thủ công
            dgvCart.AutoGenerateColumns = false; // Tự khai báo cột
            dgvCart.RowHeadersVisible = false; // Ẩn cột đầu
            dgvCart.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCart.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvCart.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Mã SP",
                DataPropertyName = "ProductId",
                FillWeight = 13
            });

            dgvCart.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Tên sản phẩm",
                DataPropertyName = "ProductName",
                FillWeight = 35
            });

            dgvCart.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Đơn giá",
                DataPropertyName = "UnitPrice",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" },
                FillWeight = 18
            });

            dgvCart.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "SL",
                DataPropertyName = "Quantity",
                FillWeight = 10
            });

            dgvCart.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Thành tiền",
                DataPropertyName = "TotalPrice",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" },
                FillWeight = 24
            });

            left.Controls.Add(dgvCart, 0, 2); // Hiển thị bảng

            // Khu vực thanh toán bên phải
            var right = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(12),
                RowCount = 11
            };

            right.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            for (int i = 0; i < 9; i++)
                right.RowStyles.Add(new RowStyle(SizeType.Absolute, 47));
            right.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            layout.Controls.Add(right, 1, 0);

            right.Controls.Add(new Label
            {
                Text = "THANH TOÁN",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            }, 0, 0);

            right.Controls.Add(MakeLabel("Số điện thoại khách hàng:"), 0, 1);

            txtCustomerPhone.Dock = DockStyle.Fill;
            txtCustomerPhone.PlaceholderText = "Khách vãng lai";
            txtCustomerPhone.KeyDown += TxtCustomerPhone_KeyDown;
            txtCustomerPhone.Leave += async (s, e) =>
                await LookupCustomerAsync();
            right.Controls.Add(txtCustomerPhone, 0, 2);

            lblCustomerName.Text = "Khách vãng lai";
            lblCustomerName.Dock = DockStyle.Fill;
            lblCustomerName.AutoEllipsis = true;
            right.Controls.Add(lblCustomerName, 0, 3);

            right.Controls.Add(MakeLabel("TỔNG TIỀN:"), 0, 4);

            lblTotalAmount.Dock = DockStyle.Fill;
            lblTotalAmount.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            lblTotalAmount.ForeColor = Color.DarkRed;
            right.Controls.Add(lblTotalAmount, 0, 5);

            right.Controls.Add(MakeLabel("Tiền khách đưa:"), 0, 6);

            txtCashReceived.Dock = DockStyle.Fill;
            txtCashReceived.PlaceholderText = "Nhập số tiền...";
            txtCashReceived.TextChanged += (s, e) => CalculateChange();
            right.Controls.Add(txtCashReceived, 0, 7);

            lblChange.Dock = DockStyle.Fill;
            lblChange.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            right.Controls.Add(lblChange, 0, 8);

            btnCheckout.Text = "THANH TOÁN (F9)";
            btnCheckout.Dock = DockStyle.Fill;
            btnCheckout.BackColor = Color.SeaGreen;
            btnCheckout.ForeColor = Color.White;
            btnCheckout.Enabled = CanCurrentUserCheckout();
            btnCheckout.Click += BtnCheckout_Click;
            right.Controls.Add(btnCheckout, 0, 9);

            btnClearCart.Text = "HỦY GIỎ HÀNG";
            btnClearCart.Dock = DockStyle.Top;
            btnClearCart.Height = 48;
            btnClearCart.BackColor = Color.Firebrick;
            btnClearCart.ForeColor = Color.White;
            btnClearCart.Click += (s, e) =>
            {
                if (_isCheckoutInProgress) return; // Không thay đổi giỏ khi đang gửi API
                if (_cart.Count == 0) return; // Không xử lý giỏ trống

                if (MessageBox.Show(
                    "Bạn có muốn xóa toàn bộ giỏ hàng?",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                _cart.Clear(); // Xóa sản phẩm khỏi giỏ trên giao diện
                UpdateCartDisplay(); // Cập nhật tổng tiền
            };

            right.Controls.Add(btnClearCart, 0, 10);
        }

        // Chỉ Admin và Cashier được gọi chức năng thanh toán.
        private static bool CanCurrentUserCheckout()
        {
            if (!SessionManager.IsLoggedIn)
                return false;

            string role = SessionManager.CurrentRole.Trim();

            return role.Equals("Admin", StringComparison.OrdinalIgnoreCase) ||
                   role.Equals("Cashier", StringComparison.OrdinalIgnoreCase);
        }

        // Nút Thanh toán và phím F9 đều đi qua cùng một luồng xử lý.
        private async void BtnCheckout_Click(object? sender, EventArgs e)
        {
            await CheckoutAsync();
        }

        private async void FormPOS_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.F9)
                return;

            e.Handled = true;
            e.SuppressKeyPress = true;

            await CheckoutAsync();
        }

        // Kiểm tra dữ liệu, gửi hóa đơn và chỉ xóa giỏ khi API xác nhận thành công.
        private async Task CheckoutAsync()
        {
            if (_isCheckoutInProgress)
                return;

            if (!CanCurrentUserCheckout())
            {
                MessageBox.Show(
                    "Chỉ Admin hoặc Cashier được phép thanh toán.",
                    "Từ chối truy cập",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (_cart.Count == 0)
            {
                MessageBox.Show(
                    "Giỏ hàng đang trống. Vui lòng thêm sản phẩm.",
                    "Chưa thể thanh toán",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtBarcode.Focus();
                return;
            }

            if (_cart.Any(item =>
                    item.ProductId <= 0 || item.Quantity <= 0))
            {
                MessageBox.Show(
                    "Giỏ hàng có sản phẩm hoặc số lượng không hợp lệ.",
                    "Dữ liệu không hợp lệ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            string customerPhone = txtCustomerPhone.Text.Trim();

            if (customerPhone.Length > 20)
            {
                MessageBox.Show(
                    "Số điện thoại khách hàng tối đa 20 ký tự.",
                    "Dữ liệu không hợp lệ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtCustomerPhone.Focus();
                return;
            }

            if (!TryReadCashReceived(out decimal cashReceived))
            {
                MessageBox.Show(
                    "Vui lòng nhập số tiền khách đưa hợp lệ.",
                    "Dữ liệu không hợp lệ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtCashReceived.Focus();
                return;
            }

            decimal displayedTotal = _cart.Sum(item => item.TotalPrice);

            if (cashReceived < displayedTotal)
            {
                MessageBox.Show(
                    $"Tiền khách đưa còn thiếu " +
                    $"{displayedTotal - cashReceived:N0} đ.",
                    "Chưa đủ tiền",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtCashReceived.Focus();
                return;
            }

            DialogResult confirm = MessageBox.Show(
                $"Tổng tiền: {displayedTotal:N0} đ\n" +
                $"Khách đưa: {cashReceived:N0} đ\n" +
                $"Tiền thừa dự kiến: {cashReceived - displayedTotal:N0} đ\n\n" +
                "Xác nhận thanh toán?",
                "Xác nhận thanh toán",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

            var request = new PosCheckoutRequest
            {
                CustomerPhone = string.IsNullOrWhiteSpace(customerPhone)
                    ? null
                    : customerPhone,
                CashReceived = cashReceived,
                Items = _cart.Select(item => new PosCheckoutItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                }).ToList()
            };

            _isCheckoutInProgress = true;
            SetCheckoutControlsEnabled(false);

            try
            {
                PosCheckoutResult result =
                    await _checkoutService.CheckoutAsync(request);

                MessageBox.Show(
                    $"Thanh toán thành công!\n\n" +
                    $"Mã hóa đơn: {result.OrderId}\n" +
                    $"Tổng tiền: {result.TotalAmount:N0} đ\n" +
                    $"Khách đưa: {result.CashReceived:N0} đ\n" +
                    $"Tiền thừa: {result.ChangeAmount:N0} đ",
                    "Thanh toán thành công",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                // Chỉ xóa dữ liệu giao diện sau khi API trả về hóa đơn hợp lệ.
                _cart.Clear();
                txtCustomerPhone.Clear();
                lblCustomerName.Text = "Khách vãng lai";
                lblCustomerName.ForeColor = SystemColors.ControlText;
                txtCashReceived.Clear();
                UpdateCartDisplay();
            }
            catch (PosCheckoutException ex)
            {
                string guidance = ex.IsOutcomeUnknown
                    ? "\n\nGiỏ hàng được giữ nguyên. Không nhấn thanh toán lại " +
                      "trước khi kiểm tra lịch sử hóa đơn."
                    : "\n\nGiỏ hàng được giữ nguyên để bạn kiểm tra lại.";

                MessageBox.Show(
                    ex.Message + guidance,
                    "Thanh toán chưa hoàn tất",
                    MessageBoxButtons.OK,
                    ex.IsOutcomeUnknown
                        ? MessageBoxIcon.Warning
                        : MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // Lỗi không dự kiến cũng không được làm mất giỏ hàng.
                MessageBox.Show(
                    "Có lỗi khi thanh toán: " + ex.Message +
                    "\n\nGiỏ hàng được giữ nguyên.",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                _isCheckoutInProgress = false;
                SetCheckoutControlsEnabled(true);
                txtBarcode.Focus();
            }
        }

        // Khóa cả thao tác quét, hủy giỏ và thanh toán để không thay đổi request đang gửi.
        private void SetCheckoutControlsEnabled(bool enabled)
        {
            txtBarcode.Enabled = enabled;
            txtCustomerPhone.Enabled = enabled;
            txtCashReceived.Enabled = enabled;
            btnClearCart.Enabled = enabled;
            btnCheckout.Enabled = enabled && CanCurrentUserCheckout();
            btnCheckout.Text = enabled
                ? "THANH TOÁN (F9)"
                : "ĐANG XỬ LÝ...";
        }

        private bool TryReadCashReceived(out decimal cashReceived)
        {
            string value = txtCashReceived.Text.Trim();

            return decimal.TryParse(
                       value,
                       NumberStyles.Number,
                       CultureInfo.CurrentCulture,
                       out cashReceived) &&
                   cashReceived >= 0;
        }

        // Nhấn Enter để tra cứu thành viên theo số điện thoại.
        private async void TxtCustomerPhone_KeyDown(
            object? sender,
            KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            e.Handled = true;
            e.SuppressKeyPress = true;
            await LookupCustomerAsync();
        }

        // Tra cứu khách hàng thật từ API; không tạo dữ liệu giả khi không tìm thấy.
        private async Task LookupCustomerAsync()
        {
            if (_isCustomerLookupInProgress)
                return;

            string phone = txtCustomerPhone.Text.Trim();

            if (string.IsNullOrWhiteSpace(phone))
            {
                lblCustomerName.Text = "Khách vãng lai";
                lblCustomerName.ForeColor = SystemColors.ControlText;
                return;
            }

            if (phone.Length > 15)
            {
                lblCustomerName.Text = "Số điện thoại tối đa 15 ký tự";
                lblCustomerName.ForeColor = Color.Firebrick;
                return;
            }

            _isCustomerLookupInProgress = true;
            string requestedPhone = phone;
            lblCustomerName.Text = "Đang tra cứu khách hàng...";
            lblCustomerName.ForeColor = Color.SteelBlue;

            try
            {
                string url = "Customers/by-phone/" +
                    Uri.EscapeDataString(requestedPhone);

                using var response =
                    await ApiClientService.Client.GetAsync(url);

                // Không ghi đè nếu người dùng đã đổi số trong lúc chờ API.
                if (!txtCustomerPhone.Text.Trim().Equals(
                        requestedPhone,
                        StringComparison.Ordinal))
                {
                    return;
                }

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    lblCustomerName.Text =
                        "Không tìm thấy thành viên - xử lý như khách vãng lai";
                    lblCustomerName.ForeColor = Color.DarkOrange;
                    return;
                }

                if (!response.IsSuccessStatusCode)
                {
                    lblCustomerName.Text =
                        $"Không thể tra cứu (HTTP {(int)response.StatusCode})";
                    lblCustomerName.ForeColor = Color.Firebrick;
                    return;
                }

                POSCustomerDto? customer = await response.Content
                    .ReadFromJsonAsync<POSCustomerDto>();

                if (customer == null)
                {
                    lblCustomerName.Text = "API không trả dữ liệu khách hàng";
                    lblCustomerName.ForeColor = Color.Firebrick;
                    return;
                }

                lblCustomerName.Text =
                    $"{customer.CustomerName} | " +
                    $"Hạng: {customer.MembershipRank} | " +
                    $"Điểm: {customer.RewardPoints}";
                lblCustomerName.ForeColor = Color.DarkGreen;
            }
            catch (Exception ex)
            {
                lblCustomerName.Text =
                    "Không thể kết nối API khách hàng: " + ex.Message;
                lblCustomerName.ForeColor = Color.Firebrick;
            }
            finally
            {
                _isCustomerLookupInProgress = false;
            }
        }

        // Tạo Label thông thường
        private static Label MakeLabel(string text)
        {
            return new Label
            {
                Text = text,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
        }

        // Bắt sự kiện Enter sau khi nhập mã vạch
        private async void TxtBarcode_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return; // Chỉ xử lý Enter

            e.SuppressKeyPress = true; // Không phát tiếng bíp

            string barcode = txtBarcode.Text.Trim(); // Lấy mã vạch

            if (string.IsNullOrWhiteSpace(barcode)) return; // Bỏ qua mã trống

            txtBarcode.Enabled = false; // Tránh quét lặp khi đang tải

            try
            {
                await AddProductByBarcodeAsync(barcode); // Gọi API tìm sản phẩm
                txtBarcode.Clear(); // Xóa ô nhập khi xử lý xong
            }
            finally
            {
                txtBarcode.Enabled = true; // Cho phép nhập lại
                txtBarcode.Focus(); // Đưa con trỏ về ô mã vạch
            }
        }

        // Tìm sản phẩm và thêm vào giỏ hàng
        private async Task AddProductByBarcodeAsync(string barcode)
        {
            try
            {
                string url = "Products/barcode/" + Uri.EscapeDataString(barcode);

                using var response = await ApiClientService.Client.GetAsync(url);

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    MessageBox.Show("Không tìm thấy sản phẩm có mã vạch này.");
                    return;
                }

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        "API trả về lỗi: " + (int)response.StatusCode +
                        "\nVui lòng kiểm tra JWT và quyền truy cập.");
                    return;
                }

                var product = await response.Content.ReadFromJsonAsync<POSProductDto>();

                if (product == null)
                {
                    MessageBox.Show("Dữ liệu sản phẩm không hợp lệ.");
                    return;
                }

                var existing = _cart.FirstOrDefault(
                    x => x.ProductId == product.ProductId);

                int newQuantity = (existing?.Quantity ?? 0) + 1;

                if (newQuantity > product.StockQuantity)
                {
                    MessageBox.Show("Không đủ số lượng tồn kho.");
                    return;
                }

                if (existing != null)
                {
                    existing.Quantity++; // Tăng số lượng trong giỏ
                }
                else
                {
                    _cart.Add(new POSCartItem
                    {
                        ProductId = product.ProductId,
                        ProductName = product.ProductName,
                        UnitPrice = product.Price,
                        Quantity = 1
                    });
                }

                UpdateCartDisplay(); // Cập nhật bảng và tổng tiền
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tìm sản phẩm: " + ex.Message);
            }
        }

        // Hiển thị lại giỏ hàng
        private void UpdateCartDisplay()
        {
            dgvCart.DataSource = null; // Làm mới nguồn dữ liệu
            dgvCart.DataSource = _cart.ToList(); // Hiển thị danh sách mới

            decimal total = _cart.Sum(x => x.TotalPrice); // Tính tổng tiền
            lblTotalAmount.Text = $"{total:N0} đ"; // Hiển thị tổng
            CalculateChange(); // Tính lại tiền thừa
        }

        // Tính tiền thừa
        private void CalculateChange()
        {
            decimal total = _cart.Sum(x => x.TotalPrice); // Tổng tiền

            if (!TryReadCashReceived(out decimal cash))
            {
                lblChange.Text = "Tiền thừa: 0 đ";
                lblChange.ForeColor = Color.Black;
                return;
            }

            decimal change = cash - total; // Tiền khách đưa trừ tổng tiền

            lblChange.Text = change >= 0
                ? $"Tiền thừa: {change:N0} đ"
                : "Khách đưa chưa đủ tiền!";

            lblChange.ForeColor = change >= 0 ? Color.DarkGreen : Color.Red;
        }
    }

    // DTO nhận dữ liệu sản phẩm từ API Barcode
    public class POSProductDto
    {
        public int ProductId { get; set; }
        public string Barcode { get; set; } = "";
        public string ProductName { get; set; } = "";
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    }

    // DTO khách hàng dùng riêng cho thao tác tra cứu tại quầy.
    public class POSCustomerDto
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public int RewardPoints { get; set; }
        public string MembershipRank { get; set; } = "";
    }

    // DTO sản phẩm trong giỏ hàng
    public class POSCartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = "";
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice => UnitPrice * Quantity;
    }
}
