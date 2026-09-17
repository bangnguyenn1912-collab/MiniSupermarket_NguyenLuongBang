namespace MiniSupermarket.WinForms
{
    partial class FormCategoryManagement
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">
        /// true if managed resources should be disposed;
        /// otherwise, false.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            lblId = new Label();
            lblCategoryName = new Label();
            lblDescription = new Label();
            lblKeyword = new Label();
            txtId = new TextBox();
            txtCategoryName = new TextBox();
            txtDescription = new TextBox();
            txtKeyword = new TextBox();
            btnLoad = new Button();
            btnAdd = new Button();
            btnUpdate = new Button();
            btnDelete = new Button();
            btnSearch = new Button();
            grpSearch = new GroupBox();
            grpList = new GroupBox();
            dgvCategories = new DataGridView();
            grpInfo = new GroupBox();
            statusStrip1 = new StatusStrip();
            lblStatus = new ToolStripStatusLabel();
            grpSearch.SuspendLayout();
            grpList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCategories).BeginInit();
            grpInfo.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // lblId
            // 
            lblId.AutoSize = true;
            lblId.Location = new Point(15, 30);
            lblId.Name = "lblId";
            lblId.Size = new Size(49, 20);
            lblId.TabIndex = 0;
            lblId.Text = "Mã ID";
            // 
            // lblCategoryName
            // 
            lblCategoryName.AutoSize = true;
            lblCategoryName.Location = new Point(15, 90);
            lblCategoryName.Name = "lblCategoryName";
            lblCategoryName.Size = new Size(114, 20);
            lblCategoryName.TabIndex = 2;
            lblCategoryName.Text = "Tên Nhóm hàng";
            // 
            // lblDescription
            // 
            lblDescription.AutoSize = true;
            lblDescription.Location = new Point(15, 152);
            lblDescription.Name = "lblDescription";
            lblDescription.Size = new Size(51, 20);
            lblDescription.TabIndex = 4;
            lblDescription.Text = "Mô Tả";
            // 
            // lblKeyword
            // 
            lblKeyword.Location = new Point(0, 0);
            lblKeyword.Name = "lblKeyword";
            lblKeyword.Size = new Size(100, 23);
            lblKeyword.TabIndex = 0;
            // 
            // txtId
            // 
            txtId.Location = new Point(15, 50);
            txtId.Name = "txtId";
            txtId.ReadOnly = true;
            txtId.Size = new Size(285, 27);
            txtId.TabIndex = 1;
            // 
            // txtCategoryName
            // 
            txtCategoryName.Location = new Point(15, 112);
            txtCategoryName.Name = "txtCategoryName";
            txtCategoryName.Size = new Size(285, 27);
            txtCategoryName.TabIndex = 3;
            // 
            // txtDescription
            // 
            txtDescription.Location = new Point(15, 175);
            txtDescription.Multiline = true;
            txtDescription.Name = "txtDescription";
            txtDescription.ScrollBars = ScrollBars.Vertical;
            txtDescription.Size = new Size(285, 75);
            txtDescription.TabIndex = 5;
            // 
            // txtKeyword
            // 
            txtKeyword.Location = new Point(15, 25);
            txtKeyword.Name = "txtKeyword";
            txtKeyword.PlaceholderText = "Nhập từ khóa...";
            txtKeyword.Size = new Size(320, 27);
            txtKeyword.TabIndex = 0;
            txtKeyword.Text = "Nhập từ khóa...";
            // 
            // btnLoad
            // 
            btnLoad.Location = new Point(445, 24);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new Size(95, 30);
            btnLoad.TabIndex = 2;
            btnLoad.Text = "Tải lại";
            btnLoad.UseVisualStyleBackColor = true;
            btnLoad.Click += btnLoad_Click;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(15, 275);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(85, 35);
            btnAdd.TabIndex = 6;
            btnAdd.Text = "Thêm mới";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnUpdate
            // 
            btnUpdate.Location = new Point(110, 275);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(90, 35);
            btnUpdate.TabIndex = 7;
            btnUpdate.Text = "Cập nhật";
            btnUpdate.UseVisualStyleBackColor = true;
            btnUpdate.Click += btnUpdate_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(210, 275);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(90, 35);
            btnDelete.TabIndex = 8;
            btnDelete.Text = "Xóa";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnSearch
            // 
            btnSearch.Location = new Point(345, 24);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(95, 30);
            btnSearch.TabIndex = 1;
            btnSearch.Text = "Tìm kiếm";
            btnSearch.UseVisualStyleBackColor = true;
            btnSearch.Click += btnSearch_Click;
            // 
            // grpSearch
            // 
            grpSearch.Controls.Add(txtKeyword);
            grpSearch.Controls.Add(btnSearch);
            grpSearch.Controls.Add(btnLoad);
            grpSearch.Location = new Point(15, 15);
            grpSearch.Name = "grpSearch";
            grpSearch.Size = new Size(560, 65);
            grpSearch.TabIndex = 0;
            grpSearch.TabStop = false;
            grpSearch.Text = "Tìm kiếm";
            // 
            // grpList
            // 
            grpList.Controls.Add(dgvCategories);
            grpList.Location = new Point(15, 95);
            grpList.Name = "grpList";
            grpList.Size = new Size(560, 375);
            grpList.TabIndex = 3;
            grpList.TabStop = false;
            grpList.Text = "Danh sách Nhóm hàng";
            // 
            // dgvCategories
            // 
            dgvCategories.AllowUserToAddRows = false;
            dgvCategories.AllowUserToDeleteRows = false;
            dgvCategories.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCategories.BackgroundColor = SystemColors.Window;
            dgvCategories.BorderStyle = BorderStyle.Fixed3D;
            dgvCategories.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCategories.Location = new Point(10, 25);
            dgvCategories.MultiSelect = false;
            dgvCategories.Name = "dgvCategories";
            dgvCategories.ReadOnly = true;
            dgvCategories.RowHeadersWidth = 51;
            dgvCategories.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCategories.Size = new Size(540, 335);
            dgvCategories.TabIndex = 0;
            dgvCategories.CellClick += dgvCategories_CellClick;
            // 
            // grpInfo
            // 
            grpInfo.Controls.Add(lblId);
            grpInfo.Controls.Add(txtId);
            grpInfo.Controls.Add(lblCategoryName);
            grpInfo.Controls.Add(txtCategoryName);
            grpInfo.Controls.Add(lblDescription);
            grpInfo.Controls.Add(txtDescription);
            grpInfo.Controls.Add(btnAdd);
            grpInfo.Controls.Add(btnUpdate);
            grpInfo.Controls.Add(btnDelete);
            grpInfo.Location = new Point(590, 95);
            grpInfo.Name = "grpInfo";
            grpInfo.Size = new Size(315, 375);
            grpInfo.TabIndex = 4;
            grpInfo.TabStop = false;
            grpInfo.Text = "Thông tin Nhóm hàng";
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { lblStatus });
            statusStrip1.Location = new Point(0, 489);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(920, 26);
            statusStrip1.TabIndex = 9;
            // 
            // lblStatus
            // 
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(50, 20);
            lblStatus.Text = "Ready";
            // 
            // FormCategoryManagement
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(920, 515);
            Controls.Add(statusStrip1);
            Controls.Add(grpInfo);
            Controls.Add(grpList);
            Controls.Add(grpSearch);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FormCategoryManagement";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Quản lý Danh mục Nhóm hàng - FormCategoryManagement";
            Load += FormCategoryManagement_Load;
            grpSearch.ResumeLayout(false);
            grpSearch.PerformLayout();
            grpList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvCategories).EndInit();
            grpInfo.ResumeLayout(false);
            grpInfo.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        // =========================================================
        // LABEL
        // =========================================================

        private Label lblId;
        private Label lblCategoryName;
        private Label lblDescription;
        private Label lblKeyword;

        // =========================================================
        // TEXTBOX
        // =========================================================

        private TextBox txtId;
        private TextBox txtCategoryName;
        private TextBox txtDescription;
        private TextBox txtKeyword;

        // =========================================================
        // BUTTON
        // =========================================================

        private Button btnLoad;
        private Button btnAdd;
        private Button btnUpdate;
        private Button btnDelete;
        private Button btnSearch;

        // =========================================================
        // GROUPBOX
        // =========================================================

        private GroupBox grpSearch;
        private GroupBox grpList;
        private GroupBox grpInfo;

        // =========================================================
        // DATAGRIDVIEW
        // =========================================================

        private DataGridView dgvCategories;

        // =========================================================
        // STATUS STRIP
        // =========================================================

        private StatusStrip statusStrip1;
        private ToolStripStatusLabel lblStatus;
    }
}