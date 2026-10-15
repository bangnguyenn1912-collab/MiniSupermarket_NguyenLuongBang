namespace MiniSupermarket.WinForms
{
    partial class FormMainShell
    {
        /// <summary>
        /// Biến chứa các component do WinForms Designer quản lý.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Giải phóng tài nguyên đang được Form sử dụng.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Phương thức do WinForms Designer quản lý.
        /// Không đặt logic nghiệp vụ, đọc Session hoặc gọi API trong phương thức này.
        /// </summary>
        private void InitializeComponent()
        {
            panelSidebar = new Panel();
            tableSidebarLayout = new TableLayoutPanel();
            lblBrand = new Label();
            panelMenu = new FlowLayoutPanel();
            btnPOS = new Button();
            btnCategory = new Button();
            btnProduct = new Button();
            btnCustomer = new Button();
            btnReports = new Button();
            btnUserManage = new Button();
            btnRoles = new Button();
            panelSidebarFooter = new Panel();
            btnLogout = new Button();
            panelWorkspace = new Panel();
            panelMainContent = new Panel();
            panelTopHeader = new Panel();
            tableHeaderLayout = new TableLayoutPanel();
            lblTitle = new Label();
            lblUserInfo = new Label();
            panelSidebar.SuspendLayout();
            tableSidebarLayout.SuspendLayout();
            panelMenu.SuspendLayout();
            panelSidebarFooter.SuspendLayout();
            panelWorkspace.SuspendLayout();
            panelTopHeader.SuspendLayout();
            tableHeaderLayout.SuspendLayout();
            SuspendLayout();
            // 
            // panelSidebar
            // 
            panelSidebar.BackColor = Color.FromArgb(24, 30, 48);
            panelSidebar.Controls.Add(tableSidebarLayout);
            panelSidebar.Dock = DockStyle.Left;
            panelSidebar.Location = new Point(0, 0);
            panelSidebar.Margin = new Padding(3, 4, 3, 4);
            panelSidebar.Name = "panelSidebar";
            panelSidebar.Size = new Size(263, 737);
            panelSidebar.TabIndex = 0;
            // 
            // tableSidebarLayout
            // 
            tableSidebarLayout.BackColor = Color.FromArgb(24, 30, 48);
            tableSidebarLayout.ColumnCount = 1;
            tableSidebarLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableSidebarLayout.Controls.Add(lblBrand, 0, 0);
            tableSidebarLayout.Controls.Add(panelMenu, 0, 1);
            tableSidebarLayout.Controls.Add(panelSidebarFooter, 0, 2);
            tableSidebarLayout.Dock = DockStyle.Fill;
            tableSidebarLayout.Location = new Point(0, 0);
            tableSidebarLayout.Margin = new Padding(0);
            tableSidebarLayout.Name = "tableSidebarLayout";
            tableSidebarLayout.RowCount = 3;
            tableSidebarLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 115F));
            tableSidebarLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableSidebarLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 83F));
            tableSidebarLayout.Size = new Size(263, 737);
            tableSidebarLayout.TabIndex = 0;
            // 
            // lblBrand
            // 
            lblBrand.BackColor = Color.FromArgb(24, 30, 48);
            lblBrand.Dock = DockStyle.Fill;
            lblBrand.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblBrand.ForeColor = Color.White;
            lblBrand.Location = new Point(0, 0);
            lblBrand.Margin = new Padding(0);
            lblBrand.Name = "lblBrand";
            lblBrand.Size = new Size(263, 115);
            lblBrand.TabIndex = 0;
            lblBrand.Text = "CIRCLE K EXPRESS\r\nQUẢN LÝ BÁN LẺ";
            lblBrand.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panelMenu
            // 
            panelMenu.AutoScroll = true;
            panelMenu.BackColor = Color.FromArgb(24, 30, 48);
            panelMenu.Controls.Add(btnPOS);
            panelMenu.Controls.Add(btnCategory);
            panelMenu.Controls.Add(btnProduct);
            panelMenu.Controls.Add(btnCustomer);
            panelMenu.Controls.Add(btnReports);
            panelMenu.Controls.Add(btnUserManage);
            panelMenu.Controls.Add(btnRoles);
            panelMenu.Dock = DockStyle.Fill;
            panelMenu.FlowDirection = FlowDirection.TopDown;
            panelMenu.Location = new Point(0, 115);
            panelMenu.Margin = new Padding(0);
            panelMenu.Name = "panelMenu";
            panelMenu.Padding = new Padding(0, 11, 0, 0);
            panelMenu.Size = new Size(263, 539);
            panelMenu.TabIndex = 1;
            panelMenu.WrapContents = false;
            // 
            // btnPOS
            // 
            btnPOS.BackColor = Color.FromArgb(24, 30, 48);
            btnPOS.Cursor = Cursors.Hand;
            btnPOS.FlatAppearance.BorderSize = 0;
            btnPOS.FlatStyle = FlatStyle.Flat;
            btnPOS.Font = new Font("Segoe UI", 10F);
            btnPOS.ForeColor = Color.White;
            btnPOS.Location = new Point(0, 11);
            btnPOS.Margin = new Padding(0, 0, 0, 5);
            btnPOS.Name = "btnPOS";
            btnPOS.Size = new Size(249, 60);
            btnPOS.TabIndex = 0;
            btnPOS.Text = "   Bán hàng (POS)";
            btnPOS.TextAlign = ContentAlignment.MiddleLeft;
            btnPOS.UseVisualStyleBackColor = false;
            btnPOS.Click += btnPOS_Click;
            // 
            // btnCategory
            // 
            btnCategory.BackColor = Color.FromArgb(24, 30, 48);
            btnCategory.Cursor = Cursors.Hand;
            btnCategory.FlatAppearance.BorderSize = 0;
            btnCategory.FlatStyle = FlatStyle.Flat;
            btnCategory.Font = new Font("Segoe UI", 10F);
            btnCategory.ForeColor = Color.White;
            btnCategory.Location = new Point(0, 76);
            btnCategory.Margin = new Padding(0, 0, 0, 5);
            btnCategory.Name = "btnCategory";
            btnCategory.Size = new Size(249, 60);
            btnCategory.TabIndex = 1;
            btnCategory.Text = "   Quản lý danh mục";
            btnCategory.TextAlign = ContentAlignment.MiddleLeft;
            btnCategory.UseVisualStyleBackColor = false;
            btnCategory.Click += btnCategory_Click;
            // 
            // btnProduct
            // 
            btnProduct.BackColor = Color.FromArgb(24, 30, 48);
            btnProduct.Cursor = Cursors.Hand;
            btnProduct.FlatAppearance.BorderSize = 0;
            btnProduct.FlatStyle = FlatStyle.Flat;
            btnProduct.Font = new Font("Segoe UI", 10F);
            btnProduct.ForeColor = Color.White;
            btnProduct.Location = new Point(0, 141);
            btnProduct.Margin = new Padding(0, 0, 0, 5);
            btnProduct.Name = "btnProduct";
            btnProduct.Size = new Size(249, 60);
            btnProduct.TabIndex = 2;
            btnProduct.Text = "   Quản lý sản phẩm";
            btnProduct.TextAlign = ContentAlignment.MiddleLeft;
            btnProduct.UseVisualStyleBackColor = false;
            btnProduct.Click += btnProduct_Click;
            // 
            // btnCustomer
            // 
            btnCustomer.BackColor = Color.FromArgb(24, 30, 48);
            btnCustomer.Cursor = Cursors.Hand;
            btnCustomer.FlatAppearance.BorderSize = 0;
            btnCustomer.FlatStyle = FlatStyle.Flat;
            btnCustomer.Font = new Font("Segoe UI", 10F);
            btnCustomer.ForeColor = Color.White;
            btnCustomer.Location = new Point(0, 206);
            btnCustomer.Margin = new Padding(0, 0, 0, 5);
            btnCustomer.Name = "btnCustomer";
            btnCustomer.Size = new Size(249, 60);
            btnCustomer.TabIndex = 3;
            btnCustomer.Text = "   Quản lý khách hàng";
            btnCustomer.TextAlign = ContentAlignment.MiddleLeft;
            btnCustomer.UseVisualStyleBackColor = false;
            btnCustomer.Click += btnCustomer_Click;
            // 
            // btnReports
            // 
            btnReports.BackColor = Color.FromArgb(24, 30, 48);
            btnReports.Cursor = Cursors.Hand;
            btnReports.FlatAppearance.BorderSize = 0;
            btnReports.FlatStyle = FlatStyle.Flat;
            btnReports.Font = new Font("Segoe UI", 10F);
            btnReports.ForeColor = Color.White;
            btnReports.Location = new Point(0, 271);
            btnReports.Margin = new Padding(0, 0, 0, 5);
            btnReports.Name = "btnReports";
            btnReports.Size = new Size(249, 60);
            btnReports.TabIndex = 4;
            btnReports.Text = "   Báo cáo doanh thu";
            btnReports.TextAlign = ContentAlignment.MiddleLeft;
            btnReports.UseVisualStyleBackColor = false;
            btnReports.Click += btnReports_Click;
            // 
            // btnUserManage
            // 
            btnUserManage.BackColor = Color.FromArgb(24, 30, 48);
            btnUserManage.Cursor = Cursors.Hand;
            btnUserManage.FlatAppearance.BorderSize = 0;
            btnUserManage.FlatStyle = FlatStyle.Flat;
            btnUserManage.Font = new Font("Segoe UI", 10F);
            btnUserManage.ForeColor = Color.White;
            btnUserManage.Location = new Point(0, 336);
            btnUserManage.Margin = new Padding(0, 0, 0, 5);
            btnUserManage.Name = "btnUserManage";
            btnUserManage.Size = new Size(249, 60);
            btnUserManage.TabIndex = 5;
            btnUserManage.Text = "   Quản lý tài khoản";
            btnUserManage.TextAlign = ContentAlignment.MiddleLeft;
            btnUserManage.UseVisualStyleBackColor = false;
            btnUserManage.Click += btnUserManage_Click;
            // 
            // btnRoles
            // 
            btnRoles.BackColor = Color.FromArgb(24, 30, 48);
            btnRoles.Cursor = Cursors.Hand;
            btnRoles.FlatAppearance.BorderSize = 0;
            btnRoles.FlatStyle = FlatStyle.Flat;
            btnRoles.Font = new Font("Segoe UI", 10F);
            btnRoles.ForeColor = Color.White;
            btnRoles.Location = new Point(0, 401);
            btnRoles.Margin = new Padding(0, 0, 0, 5);
            btnRoles.Name = "btnRoles";
            btnRoles.Size = new Size(249, 60);
            btnRoles.TabIndex = 6;
            btnRoles.Text = "   Quản lý vai trò";
            btnRoles.TextAlign = ContentAlignment.MiddleLeft;
            btnRoles.UseVisualStyleBackColor = false;
            btnRoles.Click += btnRoles_Click;
            // 
            // panelSidebarFooter
            // 
            panelSidebarFooter.BackColor = Color.FromArgb(24, 30, 48);
            panelSidebarFooter.Controls.Add(btnLogout);
            panelSidebarFooter.Dock = DockStyle.Fill;
            panelSidebarFooter.Location = new Point(0, 654);
            panelSidebarFooter.Margin = new Padding(0);
            panelSidebarFooter.Name = "panelSidebarFooter";
            panelSidebarFooter.Padding = new Padding(9, 11, 9, 11);
            panelSidebarFooter.Size = new Size(263, 83);
            panelSidebarFooter.TabIndex = 2;
            // 
            // btnLogout
            // 
            btnLogout.BackColor = Color.FromArgb(24, 30, 48);
            btnLogout.Cursor = Cursors.Hand;
            btnLogout.Dock = DockStyle.Fill;
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.Font = new Font("Segoe UI", 10F);
            btnLogout.ForeColor = Color.White;
            btnLogout.Location = new Point(9, 11);
            btnLogout.Margin = new Padding(0);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(245, 61);
            btnLogout.TabIndex = 0;
            btnLogout.Text = "   Đăng xuất";
            btnLogout.TextAlign = ContentAlignment.MiddleLeft;
            btnLogout.UseVisualStyleBackColor = false;
            btnLogout.Click += btnLogout_Click;
            // 
            // panelWorkspace
            // 
            panelWorkspace.BackColor = Color.FromArgb(244, 245, 247);
            panelWorkspace.Controls.Add(panelMainContent);
            panelWorkspace.Controls.Add(panelTopHeader);
            panelWorkspace.Dock = DockStyle.Fill;
            panelWorkspace.Location = new Point(263, 0);
            panelWorkspace.Margin = new Padding(3, 4, 3, 4);
            panelWorkspace.Name = "panelWorkspace";
            panelWorkspace.Size = new Size(1008, 737);
            panelWorkspace.TabIndex = 1;
            // 
            // panelMainContent
            // 
            panelMainContent.BackColor = Color.FromArgb(244, 245, 247);
            panelMainContent.Dock = DockStyle.Fill;
            panelMainContent.Location = new Point(0, 80);
            panelMainContent.Margin = new Padding(3, 4, 3, 4);
            panelMainContent.Name = "panelMainContent";
            panelMainContent.Padding = new Padding(14, 16, 14, 16);
            panelMainContent.Size = new Size(1008, 657);
            panelMainContent.TabIndex = 1;
            // 
            // panelTopHeader
            // 
            panelTopHeader.BackColor = Color.White;
            panelTopHeader.Controls.Add(tableHeaderLayout);
            panelTopHeader.Dock = DockStyle.Top;
            panelTopHeader.Location = new Point(0, 0);
            panelTopHeader.Margin = new Padding(3, 4, 3, 4);
            panelTopHeader.Name = "panelTopHeader";
            panelTopHeader.Padding = new Padding(23, 0, 23, 0);
            panelTopHeader.Size = new Size(1008, 80);
            panelTopHeader.TabIndex = 0;
            // 
            // tableHeaderLayout
            // 
            tableHeaderLayout.BackColor = Color.White;
            tableHeaderLayout.ColumnCount = 2;
            tableHeaderLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 58F));
            tableHeaderLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42F));
            tableHeaderLayout.Controls.Add(lblTitle, 0, 0);
            tableHeaderLayout.Controls.Add(lblUserInfo, 1, 0);
            tableHeaderLayout.Dock = DockStyle.Fill;
            tableHeaderLayout.Location = new Point(23, 0);
            tableHeaderLayout.Margin = new Padding(0);
            tableHeaderLayout.Name = "tableHeaderLayout";
            tableHeaderLayout.RowCount = 1;
            tableHeaderLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableHeaderLayout.Size = new Size(962, 80);
            tableHeaderLayout.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoEllipsis = true;
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(31, 41, 55);
            lblTitle.Location = new Point(0, 0);
            lblTitle.Margin = new Padding(0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(557, 80);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "BÀN LÀM VIỆC";
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblUserInfo
            // 
            lblUserInfo.AutoEllipsis = true;
            lblUserInfo.Dock = DockStyle.Fill;
            lblUserInfo.Font = new Font("Segoe UI", 9.5F);
            lblUserInfo.ForeColor = Color.FromArgb(55, 65, 81);
            lblUserInfo.Location = new Point(557, 0);
            lblUserInfo.Margin = new Padding(0);
            lblUserInfo.Name = "lblUserInfo";
            lblUserInfo.Size = new Size(405, 80);
            lblUserInfo.TabIndex = 1;
            lblUserInfo.Text = "Nhân viên: ... | Vai trò: ...";
            lblUserInfo.TextAlign = ContentAlignment.MiddleRight;
            // 
            // FormMainShell
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(244, 245, 247);
            ClientSize = new Size(1271, 737);
            Controls.Add(panelWorkspace);
            Controls.Add(panelSidebar);
            Margin = new Padding(3, 4, 3, 4);
            MinimumSize = new Size(1168, 784);
            Name = "FormMainShell";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Hệ thống Quản lý Bán lẻ & Tồn kho Siêu thị Mini";
            Load += FormMainShell_Load;
            Shown += FormMainShell_Shown;
            panelSidebar.ResumeLayout(false);
            tableSidebarLayout.ResumeLayout(false);
            panelMenu.ResumeLayout(false);
            panelSidebarFooter.ResumeLayout(false);
            panelWorkspace.ResumeLayout(false);
            panelTopHeader.ResumeLayout(false);
            tableHeaderLayout.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panelSidebar;
        private TableLayoutPanel tableSidebarLayout;
        private Label lblBrand;
        private FlowLayoutPanel panelMenu;
        private Button btnPOS;
        private Button btnCategory;
        private Button btnProduct;
        private Button btnCustomer;
        private Button btnReports;
        private Button btnUserManage;
        private Button btnRoles;
        private Panel panelSidebarFooter;
        private Button btnLogout;
        private Panel panelWorkspace;
        private Panel panelMainContent;
        private Panel panelTopHeader;
        private TableLayoutPanel tableHeaderLayout;
        private Label lblTitle;
        private Label lblUserInfo;
    }
}
