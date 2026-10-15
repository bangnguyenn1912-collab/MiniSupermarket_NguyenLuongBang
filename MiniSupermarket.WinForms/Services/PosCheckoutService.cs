using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace MiniSupermarket.WinForms.Services
{
    /// <summary>Dòng sản phẩm được gửi lên API thanh toán.</summary>
    public sealed class PosCheckoutItem
    {
        public int ProductId { get; set; }

        public int Quantity { get; set; }
    }

    /// <summary>
    /// Dữ liệu gửi tới POST /api/Orders/checkout.
    /// Không gửi đơn giá hoặc tổng tiền vì API phải tính lại từ SQL Server.
    /// </summary>
    public sealed class PosCheckoutRequest
    {
        public string? CustomerPhone { get; set; }

        public decimal CashReceived { get; set; }

        public List<PosCheckoutItem> Items { get; set; } = new();
    }

    /// <summary>Dòng chi tiết được API trả về sau khi thanh toán thành công.</summary>
    public sealed class PosCheckoutResultItem
    {
        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal TotalPrice { get; set; }
    }

    /// <summary>Kết quả do OrdersController trả về.</summary>
    public sealed class PosCheckoutResult
    {
        public bool Success { get; set; }

        public string Message { get; set; } = string.Empty;

        public int OrderId { get; set; }

        public DateTime OrderDate { get; set; }

        public string CashierUsername { get; set; } = string.Empty;

        public string? CustomerPhone { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal CashReceived { get; set; }

        public decimal ChangeAmount { get; set; }

        public List<PosCheckoutResultItem> Items { get; set; } = new();
    }

    /// <summary>
    /// Lỗi thanh toán có kèm trạng thái HTTP và cờ cho biết kết quả giao dịch
    /// có thể chưa xác định (ví dụ mất kết nối sau khi đã gửi yêu cầu).
    /// </summary>
    public sealed class PosCheckoutException : Exception
    {
        public PosCheckoutException(
            string message,
            HttpStatusCode? statusCode = null,
            bool isOutcomeUnknown = false,
            Exception? innerException = null)
            : base(message, innerException)
        {
            StatusCode = statusCode;
            IsOutcomeUnknown = isOutcomeUnknown;
        }

        public HttpStatusCode? StatusCode { get; }

        public bool IsOutcomeUnknown { get; }
    }

    /// <summary>Gửi yêu cầu thanh toán bằng HttpClient và JWT dùng chung.</summary>
    public sealed class PosCheckoutService
    {
        private static readonly TimeSpan CheckoutTimeout =
            TimeSpan.FromSeconds(30);

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private readonly HttpClient _client;

        public PosCheckoutService()
        {
            // Không tạo hoặc Dispose HttpClient tại đây. ApiClientService quản lý
            // BaseAddress và Bearer Token của toàn bộ phiên đăng nhập.
            _client = ApiClientService.Client;
        }

        public async Task<PosCheckoutResult> CheckoutAsync(
            PosCheckoutRequest request,
            CancellationToken cancellationToken = default)
        {
            ValidateRequest(request);

            using var timeoutSource =
                CancellationTokenSource.CreateLinkedTokenSource(
                    cancellationToken);
            timeoutSource.CancelAfter(CheckoutTimeout);

            try
            {
                // Tuyệt đối không tự gửi lại request. Nếu mất kết nối, hóa đơn có
                // thể đã được server tạo nên người dùng phải kiểm tra lịch sử trước.
                using HttpResponseMessage response =
                    await _client.PostAsJsonAsync(
                        "Orders/checkout",
                        request,
                        timeoutSource.Token);

                string responseContent =
                    await response.Content.ReadAsStringAsync(
                        timeoutSource.Token);

                if (!response.IsSuccessStatusCode)
                {
                    throw CreateApiException(
                        response.StatusCode,
                        ExtractErrorMessage(responseContent));
                }

                PosCheckoutResult? result;

                try
                {
                    result = JsonSerializer.Deserialize<PosCheckoutResult>(
                        responseContent,
                        JsonOptions);
                }
                catch (JsonException ex)
                {
                    throw new PosCheckoutException(
                        "API đã phản hồi nhưng dữ liệu kết quả không hợp lệ. " +
                        "Hãy kiểm tra lịch sử hóa đơn trước khi thử lại.",
                        response.StatusCode,
                        isOutcomeUnknown: true,
                        ex);
                }

                if (result == null || !result.Success || result.OrderId <= 0)
                {
                    throw new PosCheckoutException(
                        "Không xác nhận được mã hóa đơn từ API. " +
                        "Hãy kiểm tra lịch sử hóa đơn trước khi thử lại.",
                        response.StatusCode,
                        isOutcomeUnknown: true);
                }

                return result;
            }
            catch (PosCheckoutException)
            {
                throw;
            }
            catch (OperationCanceledException ex)
                when (!cancellationToken.IsCancellationRequested)
            {
                throw new PosCheckoutException(
                    "Yêu cầu thanh toán đã quá thời gian chờ. Kết quả giao dịch " +
                    "chưa xác định; không thanh toán lại trước khi kiểm tra lịch sử hóa đơn.",
                    isOutcomeUnknown: true,
                    innerException: ex);
            }
            catch (HttpRequestException ex)
            {
                throw new PosCheckoutException(
                    "Mất kết nối với API trong lúc thanh toán. Kết quả giao dịch " +
                    "chưa xác định; không thanh toán lại trước khi kiểm tra lịch sử hóa đơn.",
                    isOutcomeUnknown: true,
                    innerException: ex);
            }
        }

        private static void ValidateRequest(PosCheckoutRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            if (request.Items == null || request.Items.Count == 0)
            {
                throw new PosCheckoutException(
                    "Giỏ hàng đang trống. Vui lòng thêm sản phẩm.");
            }

            if (request.Items.Any(item =>
                    item.ProductId <= 0 || item.Quantity <= 0))
            {
                throw new PosCheckoutException(
                    "Giỏ hàng có sản phẩm hoặc số lượng không hợp lệ.");
            }

            if (request.CashReceived < 0)
            {
                throw new PosCheckoutException(
                    "Số tiền khách đưa không được âm.");
            }

            if (!string.IsNullOrWhiteSpace(request.CustomerPhone) &&
                request.CustomerPhone.Trim().Length > 20)
            {
                throw new PosCheckoutException(
                    "Số điện thoại khách hàng tối đa 20 ký tự.");
            }
        }

        private static PosCheckoutException CreateApiException(
            HttpStatusCode statusCode,
            string apiMessage)
        {
            string message = statusCode switch
            {
                HttpStatusCode.BadRequest =>
                    "Dữ liệu thanh toán không hợp lệ: " + apiMessage,
                HttpStatusCode.Unauthorized =>
                    "Phiên đăng nhập đã hết hạn hoặc không hợp lệ. Vui lòng đăng nhập lại.",
                HttpStatusCode.Forbidden =>
                    "Tài khoản hiện tại không có quyền thanh toán.",
                HttpStatusCode.NotFound =>
                    "Không tìm thấy dữ liệu cần thanh toán: " + apiMessage,
                HttpStatusCode.Conflict =>
                    "Không thể hoàn tất thanh toán do xung đột dữ liệu: " + apiMessage,
                HttpStatusCode.InternalServerError =>
                    "Máy chủ không thể hoàn tất thanh toán: " + apiMessage,
                _ =>
                    $"Thanh toán thất bại (HTTP {(int)statusCode}): {apiMessage}"
            };

            return new PosCheckoutException(message, statusCode);
        }

        private static string ExtractErrorMessage(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return "Máy chủ không trả về nội dung lỗi.";

            try
            {
                using JsonDocument document = JsonDocument.Parse(content);
                JsonElement root = document.RootElement;

                foreach (string propertyName in
                         new[] { "message", "error", "detail", "title" })
                {
                    if (root.TryGetProperty(
                            propertyName,
                            out JsonElement value) &&
                        value.ValueKind == JsonValueKind.String)
                    {
                        return value.GetString()
                            ?? "Máy chủ trả về lỗi không xác định.";
                    }
                }
            }
            catch (JsonException)
            {
                // Nếu API trả text thuần, dùng chính nội dung đó nhưng giới hạn độ dài.
            }

            return content.Length > 300
                ? content[..300]
                : content;
        }
    }
}
