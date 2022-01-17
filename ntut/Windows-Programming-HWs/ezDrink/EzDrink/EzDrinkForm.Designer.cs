namespace EzDrink
{
    partial class EzDrinkForm
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this._menu = new System.Windows.Forms.MenuStrip();
            this._fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._exitMenu = new System.Windows.Forms.ToolStripMenuItem();
            this._helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._aboutMenu = new System.Windows.Forms.ToolStripMenuItem();
            this._ezDrinkTabs = new System.Windows.Forms.TabControl();
            this._orderTab = new System.Windows.Forms.TabPage();
            this._iceContainer = new System.Windows.Forms.GroupBox();
            this._noIce = new System.Windows.Forms.Button();
            this._fewIce = new System.Windows.Forms.Button();
            this._halfIce = new System.Windows.Forms.Button();
            this._normalIce = new System.Windows.Forms.Button();
            this._sweetContainer = new System.Windows.Forms.GroupBox();
            this._noSweet = new System.Windows.Forms.Button();
            this._fewSweet = new System.Windows.Forms.Button();
            this._halfSweet = new System.Windows.Forms.Button();
            this._normalSweet = new System.Windows.Forms.Button();
            this._ingredientContainer = new System.Windows.Forms.GroupBox();
            this._ingredientDataGridView = new System.Windows.Forms.DataGridView();
            this._nameIngredientColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._priceIngredientColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._orderContainer = new System.Windows.Forms.GroupBox();
            this._orderDataGridView = new System.Windows.Forms.DataGridView();
            this._drinkNameOfOrderColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._drinkPriceOfOrderColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._drinkSweetOfOrderColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._drinkIceOfOrderColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._drinkIngredientOfOrderColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._sumPanel = new System.Windows.Forms.Panel();
            this._sumButton = new System.Windows.Forms.Button();
            this._sumLabel = new System.Windows.Forms.Label();
            this._drinkContainer = new System.Windows.Forms.GroupBox();
            this._drinkDataGridView = new System.Windows.Forms.DataGridView();
            this._drinkNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._priceDrinkColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._backEndTab = new System.Windows.Forms.TabPage();
            this._ingredientOperationContainer = new System.Windows.Forms.GroupBox();
            this._addingIngredientPriceTextBox = new System.Windows.Forms.TextBox();
            this._ingredientFromFileButton = new System.Windows.Forms.Button();
            this._ingredientPriceLabel = new System.Windows.Forms.Label();
            this._addingIngredientNameTextBox = new System.Windows.Forms.TextBox();
            this._ingredientNameLabel = new System.Windows.Forms.Label();
            this._ingredientFromBoxButton = new System.Windows.Forms.Button();
            this._drinkOperationContainer = new System.Windows.Forms.GroupBox();
            this._addingDrinkPriceTextBox = new System.Windows.Forms.TextBox();
            this._drinkFromFileButton = new System.Windows.Forms.Button();
            this._addingPriceLabel = new System.Windows.Forms.Label();
            this._addingDrinkNameTextBox = new System.Windows.Forms.TextBox();
            this._addingNameLabel = new System.Windows.Forms.Label();
            this._drinkFromBoxButton = new System.Windows.Forms.Button();
            this._ingredientBackContainer = new System.Windows.Forms.GroupBox();
            this._ingredientBackGrid = new System.Windows.Forms.DataGridView();
            this._ingredientNameBackColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._ingredientPriceBackColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._drinkManageContainer = new System.Windows.Forms.GroupBox();
            this._drinkBackGrid = new System.Windows.Forms.DataGridView();
            this._drinkNameBackColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._drinkPriceBackColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._historyTab = new System.Windows.Forms.TabPage();
            this._detailGroupBox = new System.Windows.Forms.GroupBox();
            this._detailDataGridView = new System.Windows.Forms.DataGridView();
            this._drinkTitleColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._sumDrinkColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._sweetColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._iceColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._ingredientColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._amountPanel = new System.Windows.Forms.Panel();
            this._amountLabel = new System.Windows.Forms.Label();
            this._historyGroupBox = new System.Windows.Forms.GroupBox();
            this._historyDataGridView = new System.Windows.Forms.DataGridView();
            this._timeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._priceColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            //this.dataGridViewDisableButtonColumn1 = new EzDrink.DataGridViewDisableButtonColumn();
            ///this.dataGridViewButtonColumn1 = new System.Windows.Forms.DataGridViewButtonColumn();
            //this.dataGridViewButtonColumn2 = new System.Windows.Forms.DataGridViewButtonColumn();
            //this.dataGridViewButtonColumn3 = new System.Windows.Forms.DataGridViewButtonColumn();
            //this.dataGridViewButtonColumn4 = new System.Windows.Forms.DataGridViewButtonColumn();
            this._addIngredientColumn = new EzDrink.DataGridViewDisableButtonColumn();
            this._deleteOrder = new System.Windows.Forms.DataGridViewButtonColumn();
            this._addDrinkColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            this._deleteIngredientColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            this._deleteDrinkButtonColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            this._menu.SuspendLayout();
            this._ezDrinkTabs.SuspendLayout();
            this._orderTab.SuspendLayout();
            this._iceContainer.SuspendLayout();
            this._sweetContainer.SuspendLayout();
            this._ingredientContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._ingredientDataGridView)).BeginInit();
            this._orderContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._orderDataGridView)).BeginInit();
            this._sumPanel.SuspendLayout();
            this._drinkContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._drinkDataGridView)).BeginInit();
            this._backEndTab.SuspendLayout();
            this._ingredientOperationContainer.SuspendLayout();
            this._drinkOperationContainer.SuspendLayout();
            this._ingredientBackContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._ingredientBackGrid)).BeginInit();
            this._drinkManageContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._drinkBackGrid)).BeginInit();
            this._historyTab.SuspendLayout();
            this._detailGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._detailDataGridView)).BeginInit();
            this._amountPanel.SuspendLayout();
            this._historyGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._historyDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // _menu
            // 
            this._menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._fileToolStripMenuItem,
            this._helpToolStripMenuItem});
            this._menu.Location = new System.Drawing.Point(0, 0);
            this._menu.Name = "_menu";
            this._menu.Size = new System.Drawing.Size(1092, 24);
            this._menu.TabIndex = 0;
            this._menu.Text = "menuStrip1";
            // 
            // _fileToolStripMenuItem
            // 
            this._fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._exitMenu});
            this._fileToolStripMenuItem.Name = "_fileToolStripMenuItem";
            this._fileToolStripMenuItem.Size = new System.Drawing.Size(38, 20);
            this._fileToolStripMenuItem.Text = "File";
            // 
            // _exitMenu
            // 
            this._exitMenu.Name = "_exitMenu";
            this._exitMenu.Size = new System.Drawing.Size(94, 22);
            this._exitMenu.Text = "Exit";
            this._exitMenu.Click += new System.EventHandler(this.ClickExit);
            // 
            // _helpToolStripMenuItem
            // 
            this._helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._aboutMenu});
            this._helpToolStripMenuItem.Name = "_helpToolStripMenuItem";
            this._helpToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this._helpToolStripMenuItem.Text = "Help";
            // 
            // _aboutMenu
            // 
            this._aboutMenu.Name = "_aboutMenu";
            this._aboutMenu.Size = new System.Drawing.Size(109, 22);
            this._aboutMenu.Text = "About";
            this._aboutMenu.Click += new System.EventHandler(this.ClickAbout);
            // 
            // _ezDrinkTabs
            // 
            this._ezDrinkTabs.Controls.Add(this._orderTab);
            this._ezDrinkTabs.Controls.Add(this._backEndTab);
            this._ezDrinkTabs.Controls.Add(this._historyTab);
            this._ezDrinkTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this._ezDrinkTabs.Location = new System.Drawing.Point(0, 24);
            this._ezDrinkTabs.Name = "_ezDrinkTabs";
            this._ezDrinkTabs.SelectedIndex = 0;
            this._ezDrinkTabs.Size = new System.Drawing.Size(1092, 571);
            this._ezDrinkTabs.TabIndex = 1;
            // 
            // _orderTab
            // 
            this._orderTab.Controls.Add(this._iceContainer);
            this._orderTab.Controls.Add(this._sweetContainer);
            this._orderTab.Controls.Add(this._ingredientContainer);
            this._orderTab.Controls.Add(this._orderContainer);
            this._orderTab.Controls.Add(this._drinkContainer);
            this._orderTab.Location = new System.Drawing.Point(4, 22);
            this._orderTab.Name = "_orderTab";
            this._orderTab.Padding = new System.Windows.Forms.Padding(3);
            this._orderTab.Size = new System.Drawing.Size(1084, 545);
            this._orderTab.TabIndex = 0;
            this._orderTab.Text = "點餐系統";
            this._orderTab.UseVisualStyleBackColor = true;
            // 
            // _iceContainer
            // 
            this._iceContainer.Controls.Add(this._noIce);
            this._iceContainer.Controls.Add(this._fewIce);
            this._iceContainer.Controls.Add(this._halfIce);
            this._iceContainer.Controls.Add(this._normalIce);
            this._iceContainer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._iceContainer.Location = new System.Drawing.Point(273, 387);
            this._iceContainer.Name = "_iceContainer";
            this._iceContainer.Size = new System.Drawing.Size(344, 155);
            this._iceContainer.TabIndex = 5;
            this._iceContainer.TabStop = false;
            this._iceContainer.Text = "溫度";
            // 
            // _noIce
            // 
            this._noIce.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._noIce.Location = new System.Drawing.Point(204, 86);
            this._noIce.Name = "_noIce";
            this._noIce.Size = new System.Drawing.Size(110, 38);
            this._noIce.TabIndex = 7;
            this._noIce.Text = "去冰";
            this._noIce.UseVisualStyleBackColor = true;
            this._noIce.Click += new System.EventHandler(this.ClickNoIce);
            // 
            // _fewIce
            // 
            this._fewIce.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._fewIce.Location = new System.Drawing.Point(29, 86);
            this._fewIce.Name = "_fewIce";
            this._fewIce.Size = new System.Drawing.Size(110, 38);
            this._fewIce.TabIndex = 6;
            this._fewIce.Text = "少冰";
            this._fewIce.UseVisualStyleBackColor = true;
            this._fewIce.Click += new System.EventHandler(this.ClickFewIce);
            // 
            // _halfIce
            // 
            this._halfIce.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._halfIce.Location = new System.Drawing.Point(204, 30);
            this._halfIce.Name = "_halfIce";
            this._halfIce.Size = new System.Drawing.Size(110, 38);
            this._halfIce.TabIndex = 5;
            this._halfIce.Text = "溫熱";
            this._halfIce.UseVisualStyleBackColor = true;
            this._halfIce.Click += new System.EventHandler(this.ClickHalfIce);
            // 
            // _normalIce
            // 
            this._normalIce.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._normalIce.Location = new System.Drawing.Point(29, 30);
            this._normalIce.Name = "_normalIce";
            this._normalIce.Size = new System.Drawing.Size(110, 38);
            this._normalIce.TabIndex = 4;
            this._normalIce.Text = "正常";
            this._normalIce.UseVisualStyleBackColor = true;
            this._normalIce.Click += new System.EventHandler(this.ClickNormalIce);
            // 
            // _sweetContainer
            // 
            this._sweetContainer.Controls.Add(this._noSweet);
            this._sweetContainer.Controls.Add(this._fewSweet);
            this._sweetContainer.Controls.Add(this._halfSweet);
            this._sweetContainer.Controls.Add(this._normalSweet);
            this._sweetContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._sweetContainer.Location = new System.Drawing.Point(273, 208);
            this._sweetContainer.Name = "_sweetContainer";
            this._sweetContainer.Size = new System.Drawing.Size(344, 334);
            this._sweetContainer.TabIndex = 4;
            this._sweetContainer.TabStop = false;
            this._sweetContainer.Text = "甜度";
            // 
            // _noSweet
            // 
            this._noSweet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._noSweet.Location = new System.Drawing.Point(205, 126);
            this._noSweet.Name = "_noSweet";
            this._noSweet.Size = new System.Drawing.Size(110, 38);
            this._noSweet.TabIndex = 3;
            this._noSweet.Text = "無糖";
            this._noSweet.UseVisualStyleBackColor = true;
            this._noSweet.Click += new System.EventHandler(this.ClickNoSweet);
            // 
            // _fewSweet
            // 
            this._fewSweet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._fewSweet.Location = new System.Drawing.Point(30, 126);
            this._fewSweet.Name = "_fewSweet";
            this._fewSweet.Size = new System.Drawing.Size(110, 38);
            this._fewSweet.TabIndex = 2;
            this._fewSweet.Text = "微糖";
            this._fewSweet.UseVisualStyleBackColor = true;
            this._fewSweet.Click += new System.EventHandler(this.ClickFewSweet);
            // 
            // _halfSweet
            // 
            this._halfSweet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._halfSweet.Location = new System.Drawing.Point(205, 32);
            this._halfSweet.Name = "_halfSweet";
            this._halfSweet.Size = new System.Drawing.Size(110, 38);
            this._halfSweet.TabIndex = 1;
            this._halfSweet.Text = "半糖";
            this._halfSweet.UseVisualStyleBackColor = true;
            this._halfSweet.Click += new System.EventHandler(this.ClickHalfSweet);
            // 
            // _normalSweet
            // 
            this._normalSweet.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._normalSweet.Location = new System.Drawing.Point(30, 32);
            this._normalSweet.Name = "_normalSweet";
            this._normalSweet.Size = new System.Drawing.Size(110, 38);
            this._normalSweet.TabIndex = 0;
            this._normalSweet.Text = "正常";
            this._normalSweet.UseVisualStyleBackColor = true;
            this._normalSweet.Click += new System.EventHandler(this.ClickNormalSweet);
            // 
            // _ingredientContainer
            // 
            this._ingredientContainer.Controls.Add(this._ingredientDataGridView);
            this._ingredientContainer.Dock = System.Windows.Forms.DockStyle.Top;
            this._ingredientContainer.Location = new System.Drawing.Point(273, 3);
            this._ingredientContainer.Name = "_ingredientContainer";
            this._ingredientContainer.Size = new System.Drawing.Size(344, 205);
            this._ingredientContainer.TabIndex = 3;
            this._ingredientContainer.TabStop = false;
            this._ingredientContainer.Text = "配料";
            // 
            // _ingredientDataGridView
            // 
            this._ingredientDataGridView.AllowUserToAddRows = false;
            this._ingredientDataGridView.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            this._ingredientDataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._ingredientDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this._ingredientDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._ingredientDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._addIngredientColumn,
            this._nameIngredientColumn,
            this._priceIngredientColumn});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this._ingredientDataGridView.DefaultCellStyle = dataGridViewCellStyle3;
            this._ingredientDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._ingredientDataGridView.Location = new System.Drawing.Point(3, 18);
            this._ingredientDataGridView.Name = "_ingredientDataGridView";
            this._ingredientDataGridView.ReadOnly = true;
            this._ingredientDataGridView.RowHeadersVisible = false;
            this._ingredientDataGridView.RowTemplate.Height = 24;
            this._ingredientDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._ingredientDataGridView.Size = new System.Drawing.Size(338, 184);
            this._ingredientDataGridView.TabIndex = 0;
            this._ingredientDataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.SelectIngredient);
            // 
            // _nameIngredientColumn
            // 
            this._nameIngredientColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._nameIngredientColumn.HeaderText = "名稱";
            this._nameIngredientColumn.Name = "_nameIngredientColumn";
            this._nameIngredientColumn.ReadOnly = true;
            // 
            // _priceIngredientColumn
            // 
            this._priceIngredientColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._priceIngredientColumn.HeaderText = "價格";
            this._priceIngredientColumn.Name = "_priceIngredientColumn";
            this._priceIngredientColumn.ReadOnly = true;
            // 
            // _orderContainer
            // 
            this._orderContainer.Controls.Add(this._orderDataGridView);
            this._orderContainer.Controls.Add(this._sumPanel);
            this._orderContainer.Dock = System.Windows.Forms.DockStyle.Right;
            this._orderContainer.Location = new System.Drawing.Point(617, 3);
            this._orderContainer.Name = "_orderContainer";
            this._orderContainer.Size = new System.Drawing.Size(464, 539);
            this._orderContainer.TabIndex = 2;
            this._orderContainer.TabStop = false;
            this._orderContainer.Text = "點單";
            // 
            // _orderDataGridView
            // 
            this._orderDataGridView.AllowUserToAddRows = false;
            this._orderDataGridView.AllowUserToDeleteRows = false;
            this._orderDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._orderDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._drinkNameOfOrderColumn,
            this._drinkPriceOfOrderColumn,
            this._drinkSweetOfOrderColumn,
            this._drinkIceOfOrderColumn,
            this._drinkIngredientOfOrderColumn,
            this._deleteOrder});
            this._orderDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._orderDataGridView.Location = new System.Drawing.Point(3, 18);
            this._orderDataGridView.Name = "_orderDataGridView";
            this._orderDataGridView.ReadOnly = true;
            this._orderDataGridView.RowHeadersVisible = false;
            this._orderDataGridView.RowTemplate.Height = 24;
            this._orderDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._orderDataGridView.Size = new System.Drawing.Size(458, 418);
            this._orderDataGridView.TabIndex = 1;
            this._orderDataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.RemoveOrderClick);
            this._orderDataGridView.RowStateChanged += new System.Windows.Forms.DataGridViewRowStateChangedEventHandler(this.OrderSelectChange);
            // 
            // _drinkNameOfOrderColumn
            // 
            this._drinkNameOfOrderColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this._drinkNameOfOrderColumn.HeaderText = "飲料名稱";
            this._drinkNameOfOrderColumn.Name = "_drinkNameOfOrderColumn";
            this._drinkNameOfOrderColumn.ReadOnly = true;
            this._drinkNameOfOrderColumn.Width = 78;
            // 
            // _drinkPriceOfOrderColumn
            // 
            this._drinkPriceOfOrderColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this._drinkPriceOfOrderColumn.HeaderText = "價格";
            this._drinkPriceOfOrderColumn.Name = "_drinkPriceOfOrderColumn";
            this._drinkPriceOfOrderColumn.ReadOnly = true;
            this._drinkPriceOfOrderColumn.Width = 54;
            // 
            // _drinkSweetOfOrderColumn
            // 
            this._drinkSweetOfOrderColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this._drinkSweetOfOrderColumn.HeaderText = "甜度";
            this._drinkSweetOfOrderColumn.Name = "_drinkSweetOfOrderColumn";
            this._drinkSweetOfOrderColumn.ReadOnly = true;
            this._drinkSweetOfOrderColumn.Width = 54;
            // 
            // _drinkIceOfOrderColumn
            // 
            this._drinkIceOfOrderColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this._drinkIceOfOrderColumn.HeaderText = "溫度";
            this._drinkIceOfOrderColumn.Name = "_drinkIceOfOrderColumn";
            this._drinkIceOfOrderColumn.ReadOnly = true;
            this._drinkIceOfOrderColumn.Width = 54;
            // 
            // _drinkIngredientOfOrderColumn
            // 
            this._drinkIngredientOfOrderColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._drinkIngredientOfOrderColumn.HeaderText = "加料";
            this._drinkIngredientOfOrderColumn.Name = "_drinkIngredientOfOrderColumn";
            this._drinkIngredientOfOrderColumn.ReadOnly = true;
            // 
            // _sumPanel
            // 
            this._sumPanel.Controls.Add(this._sumButton);
            this._sumPanel.Controls.Add(this._sumLabel);
            this._sumPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._sumPanel.Location = new System.Drawing.Point(3, 436);
            this._sumPanel.Name = "_sumPanel";
            this._sumPanel.Size = new System.Drawing.Size(458, 100);
            this._sumPanel.TabIndex = 0;
            // 
            // _sumButton
            // 
            this._sumButton.Location = new System.Drawing.Point(244, 35);
            this._sumButton.Name = "_sumButton";
            this._sumButton.Size = new System.Drawing.Size(75, 23);
            this._sumButton.TabIndex = 1;
            this._sumButton.Text = "結帳";
            this._sumButton.UseVisualStyleBackColor = true;
            this._sumButton.Click += new System.EventHandler(this.SaveOrderToModel);
            // 
            // _sumLabel
            // 
            this._sumLabel.AutoSize = true;
            this._sumLabel.Location = new System.Drawing.Point(64, 40);
            this._sumLabel.Name = "_sumLabel";
            this._sumLabel.Size = new System.Drawing.Size(32, 12);
            this._sumLabel.TabIndex = 0;
            this._sumLabel.Text = "總價:";
            // 
            // _drinkContainer
            // 
            this._drinkContainer.Controls.Add(this._drinkDataGridView);
            this._drinkContainer.Dock = System.Windows.Forms.DockStyle.Left;
            this._drinkContainer.Location = new System.Drawing.Point(3, 3);
            this._drinkContainer.Name = "_drinkContainer";
            this._drinkContainer.Size = new System.Drawing.Size(270, 539);
            this._drinkContainer.TabIndex = 0;
            this._drinkContainer.TabStop = false;
            this._drinkContainer.Text = "飲料";
            // 
            // _drinkDataGridView
            // 
            this._drinkDataGridView.AllowUserToAddRows = false;
            this._drinkDataGridView.AllowUserToDeleteRows = false;
            this._drinkDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._drinkDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._addDrinkColumn,
            this._drinkNameColumn,
            this._priceDrinkColumn});
            this._drinkDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._drinkDataGridView.Location = new System.Drawing.Point(3, 18);
            this._drinkDataGridView.Name = "_drinkDataGridView";
            this._drinkDataGridView.ReadOnly = true;
            this._drinkDataGridView.RowHeadersVisible = false;
            this._drinkDataGridView.RowTemplate.Height = 24;
            this._drinkDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._drinkDataGridView.Size = new System.Drawing.Size(264, 518);
            this._drinkDataGridView.TabIndex = 0;
            this._drinkDataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.SelectDrink);
            // 
            // _drinkNameColumn
            // 
            this._drinkNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._drinkNameColumn.HeaderText = "名稱";
            this._drinkNameColumn.Name = "_drinkNameColumn";
            this._drinkNameColumn.ReadOnly = true;
            this._drinkNameColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this._drinkNameColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _priceDrinkColumn
            // 
            this._priceDrinkColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._priceDrinkColumn.HeaderText = "價格";
            this._priceDrinkColumn.Name = "_priceDrinkColumn";
            this._priceDrinkColumn.ReadOnly = true;
            this._priceDrinkColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this._priceDrinkColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _backEndTab
            // 
            this._backEndTab.Controls.Add(this._ingredientOperationContainer);
            this._backEndTab.Controls.Add(this._drinkOperationContainer);
            this._backEndTab.Controls.Add(this._ingredientBackContainer);
            this._backEndTab.Controls.Add(this._drinkManageContainer);
            this._backEndTab.Location = new System.Drawing.Point(4, 22);
            this._backEndTab.Name = "_backEndTab";
            this._backEndTab.Padding = new System.Windows.Forms.Padding(3);
            this._backEndTab.Size = new System.Drawing.Size(1084, 545);
            this._backEndTab.TabIndex = 1;
            this._backEndTab.Text = "後台管理";
            this._backEndTab.UseVisualStyleBackColor = true;
            // 
            // _ingredientOperationContainer
            // 
            this._ingredientOperationContainer.Controls.Add(this._addingIngredientPriceTextBox);
            this._ingredientOperationContainer.Controls.Add(this._ingredientFromFileButton);
            this._ingredientOperationContainer.Controls.Add(this._ingredientPriceLabel);
            this._ingredientOperationContainer.Controls.Add(this._addingIngredientNameTextBox);
            this._ingredientOperationContainer.Controls.Add(this._ingredientNameLabel);
            this._ingredientOperationContainer.Controls.Add(this._ingredientFromBoxButton);
            this._ingredientOperationContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._ingredientOperationContainer.Location = new System.Drawing.Point(321, 263);
            this._ingredientOperationContainer.Name = "_ingredientOperationContainer";
            this._ingredientOperationContainer.Size = new System.Drawing.Size(417, 279);
            this._ingredientOperationContainer.TabIndex = 3;
            this._ingredientOperationContainer.TabStop = false;
            this._ingredientOperationContainer.Text = "加料清單操作";
            // 
            // _addingIngredientPriceTextBox
            // 
            this._addingIngredientPriceTextBox.Enabled = false;
            this._addingIngredientPriceTextBox.Font = new System.Drawing.Font("新細明體", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this._addingIngredientPriceTextBox.Location = new System.Drawing.Point(248, 202);
            this._addingIngredientPriceTextBox.Name = "_addingIngredientPriceTextBox";
            this._addingIngredientPriceTextBox.Size = new System.Drawing.Size(124, 39);
            this._addingIngredientPriceTextBox.TabIndex = 8;
            // 
            // _ingredientFromFileButton
            // 
            this._ingredientFromFileButton.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this._ingredientFromFileButton.Location = new System.Drawing.Point(251, 37);
            this._ingredientFromFileButton.Name = "_ingredientFromFileButton";
            this._ingredientFromFileButton.Size = new System.Drawing.Size(126, 56);
            this._ingredientFromFileButton.TabIndex = 9;
            this._ingredientFromFileButton.Text = "從檔案匯入";
            this._ingredientFromFileButton.UseVisualStyleBackColor = true;
            this._ingredientFromFileButton.Click += new System.EventHandler(this.ClickIngredientFileButton);
            // 
            // _ingredientPriceLabel
            // 
            this._ingredientPriceLabel.AutoSize = true;
            this._ingredientPriceLabel.Font = new System.Drawing.Font("新細明體", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this._ingredientPriceLabel.Location = new System.Drawing.Point(93, 202);
            this._ingredientPriceLabel.Name = "_ingredientPriceLabel";
            this._ingredientPriceLabel.Size = new System.Drawing.Size(73, 27);
            this._ingredientPriceLabel.TabIndex = 5;
            this._ingredientPriceLabel.Text = "價格:";
            // 
            // _addingIngredientNameTextBox
            // 
            this._addingIngredientNameTextBox.Enabled = false;
            this._addingIngredientNameTextBox.Font = new System.Drawing.Font("新細明體", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this._addingIngredientNameTextBox.Location = new System.Drawing.Point(248, 154);
            this._addingIngredientNameTextBox.Name = "_addingIngredientNameTextBox";
            this._addingIngredientNameTextBox.Size = new System.Drawing.Size(124, 39);
            this._addingIngredientNameTextBox.TabIndex = 6;
            // 
            // _ingredientNameLabel
            // 
            this._ingredientNameLabel.AutoSize = true;
            this._ingredientNameLabel.Font = new System.Drawing.Font("新細明體", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this._ingredientNameLabel.Location = new System.Drawing.Point(39, 157);
            this._ingredientNameLabel.Name = "_ingredientNameLabel";
            this._ingredientNameLabel.Size = new System.Drawing.Size(127, 27);
            this._ingredientNameLabel.TabIndex = 4;
            this._ingredientNameLabel.Text = "加料名稱:";
            // 
            // _ingredientFromBoxButton
            // 
            this._ingredientFromBoxButton.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this._ingredientFromBoxButton.Location = new System.Drawing.Point(43, 39);
            this._ingredientFromBoxButton.Name = "_ingredientFromBoxButton";
            this._ingredientFromBoxButton.Size = new System.Drawing.Size(126, 54);
            this._ingredientFromBoxButton.TabIndex = 7;
            this._ingredientFromBoxButton.Text = "新增";
            this._ingredientFromBoxButton.UseVisualStyleBackColor = true;
            this._ingredientFromBoxButton.Click += new System.EventHandler(this.ClickIngredientTextButton);
            // 
            // _drinkOperationContainer
            // 
            this._drinkOperationContainer.Controls.Add(this._addingDrinkPriceTextBox);
            this._drinkOperationContainer.Controls.Add(this._drinkFromFileButton);
            this._drinkOperationContainer.Controls.Add(this._addingPriceLabel);
            this._drinkOperationContainer.Controls.Add(this._addingDrinkNameTextBox);
            this._drinkOperationContainer.Controls.Add(this._addingNameLabel);
            this._drinkOperationContainer.Controls.Add(this._drinkFromBoxButton);
            this._drinkOperationContainer.Dock = System.Windows.Forms.DockStyle.Top;
            this._drinkOperationContainer.Location = new System.Drawing.Point(321, 3);
            this._drinkOperationContainer.Name = "_drinkOperationContainer";
            this._drinkOperationContainer.Size = new System.Drawing.Size(417, 260);
            this._drinkOperationContainer.TabIndex = 2;
            this._drinkOperationContainer.TabStop = false;
            this._drinkOperationContainer.Text = "飲料清單動作";
            // 
            // _addingDrinkPriceTextBox
            // 
            this._addingDrinkPriceTextBox.Enabled = false;
            this._addingDrinkPriceTextBox.Font = new System.Drawing.Font("新細明體", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this._addingDrinkPriceTextBox.Location = new System.Drawing.Point(257, 197);
            this._addingDrinkPriceTextBox.Name = "_addingDrinkPriceTextBox";
            this._addingDrinkPriceTextBox.Size = new System.Drawing.Size(124, 39);
            this._addingDrinkPriceTextBox.TabIndex = 3;
            // 
            // _drinkFromFileButton
            // 
            this._drinkFromFileButton.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this._drinkFromFileButton.Location = new System.Drawing.Point(260, 32);
            this._drinkFromFileButton.Name = "_drinkFromFileButton";
            this._drinkFromFileButton.Size = new System.Drawing.Size(126, 56);
            this._drinkFromFileButton.TabIndex = 3;
            this._drinkFromFileButton.Text = "從檔案匯入";
            this._drinkFromFileButton.UseVisualStyleBackColor = true;
            this._drinkFromFileButton.Click += new System.EventHandler(this.ClickDrinkFileButton);
            // 
            // _addingPriceLabel
            // 
            this._addingPriceLabel.AutoSize = true;
            this._addingPriceLabel.Font = new System.Drawing.Font("新細明體", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this._addingPriceLabel.Location = new System.Drawing.Point(102, 197);
            this._addingPriceLabel.Name = "_addingPriceLabel";
            this._addingPriceLabel.Size = new System.Drawing.Size(73, 27);
            this._addingPriceLabel.TabIndex = 1;
            this._addingPriceLabel.Text = "價格:";
            // 
            // _addingDrinkNameTextBox
            // 
            this._addingDrinkNameTextBox.Enabled = false;
            this._addingDrinkNameTextBox.Font = new System.Drawing.Font("新細明體", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this._addingDrinkNameTextBox.Location = new System.Drawing.Point(257, 149);
            this._addingDrinkNameTextBox.Name = "_addingDrinkNameTextBox";
            this._addingDrinkNameTextBox.Size = new System.Drawing.Size(124, 39);
            this._addingDrinkNameTextBox.TabIndex = 2;
            // 
            // _addingNameLabel
            // 
            this._addingNameLabel.AutoSize = true;
            this._addingNameLabel.Font = new System.Drawing.Font("新細明體", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this._addingNameLabel.Location = new System.Drawing.Point(48, 152);
            this._addingNameLabel.Name = "_addingNameLabel";
            this._addingNameLabel.Size = new System.Drawing.Size(127, 27);
            this._addingNameLabel.TabIndex = 0;
            this._addingNameLabel.Text = "飲料名稱:";
            // 
            // _drinkFromBoxButton
            // 
            this._drinkFromBoxButton.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this._drinkFromBoxButton.Location = new System.Drawing.Point(52, 34);
            this._drinkFromBoxButton.Name = "_drinkFromBoxButton";
            this._drinkFromBoxButton.Size = new System.Drawing.Size(126, 54);
            this._drinkFromBoxButton.TabIndex = 2;
            this._drinkFromBoxButton.Text = "新增";
            this._drinkFromBoxButton.UseVisualStyleBackColor = true;
            this._drinkFromBoxButton.Click += new System.EventHandler(this.ClickDrinkTextButton);
            // 
            // _ingredientBackContainer
            // 
            this._ingredientBackContainer.Controls.Add(this._ingredientBackGrid);
            this._ingredientBackContainer.Dock = System.Windows.Forms.DockStyle.Right;
            this._ingredientBackContainer.Location = new System.Drawing.Point(738, 3);
            this._ingredientBackContainer.Name = "_ingredientBackContainer";
            this._ingredientBackContainer.Size = new System.Drawing.Size(343, 539);
            this._ingredientBackContainer.TabIndex = 1;
            this._ingredientBackContainer.TabStop = false;
            this._ingredientBackContainer.Text = "加料清單";
            // 
            // _ingredientBackGrid
            // 
            this._ingredientBackGrid.AllowUserToAddRows = false;
            this._ingredientBackGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._ingredientBackGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._deleteIngredientColumn,
            this._ingredientNameBackColumn,
            this._ingredientPriceBackColumn});
            this._ingredientBackGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._ingredientBackGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this._ingredientBackGrid.Location = new System.Drawing.Point(3, 18);
            this._ingredientBackGrid.MultiSelect = false;
            this._ingredientBackGrid.Name = "_ingredientBackGrid";
            this._ingredientBackGrid.RowHeadersVisible = false;
            this._ingredientBackGrid.RowTemplate.Height = 24;
            this._ingredientBackGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this._ingredientBackGrid.Size = new System.Drawing.Size(337, 518);
            this._ingredientBackGrid.TabIndex = 0;
            this._ingredientBackGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ClickDeleteIngredient);
            this._ingredientBackGrid.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.ChangeCellValueOfIngredientGrid);
            // 
            // _ingredientNameBackColumn
            // 
            this._ingredientNameBackColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._ingredientNameBackColumn.HeaderText = "配料名稱";
            this._ingredientNameBackColumn.Name = "_ingredientNameBackColumn";
            this._ingredientNameBackColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this._ingredientNameBackColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _ingredientPriceBackColumn
            // 
            this._ingredientPriceBackColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._ingredientPriceBackColumn.HeaderText = "價格";
            this._ingredientPriceBackColumn.Name = "_ingredientPriceBackColumn";
            // 
            // _drinkManageContainer
            // 
            this._drinkManageContainer.Controls.Add(this._drinkBackGrid);
            this._drinkManageContainer.Dock = System.Windows.Forms.DockStyle.Left;
            this._drinkManageContainer.Location = new System.Drawing.Point(3, 3);
            this._drinkManageContainer.Name = "_drinkManageContainer";
            this._drinkManageContainer.Size = new System.Drawing.Size(318, 539);
            this._drinkManageContainer.TabIndex = 0;
            this._drinkManageContainer.TabStop = false;
            this._drinkManageContainer.Text = "飲料清單";
            // 
            // _drinkBackGrid
            // 
            this._drinkBackGrid.AllowUserToAddRows = false;
            this._drinkBackGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._drinkBackGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._deleteDrinkButtonColumn,
            this._drinkNameBackColumn,
            this._drinkPriceBackColumn});
            this._drinkBackGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._drinkBackGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this._drinkBackGrid.Location = new System.Drawing.Point(3, 18);
            this._drinkBackGrid.MultiSelect = false;
            this._drinkBackGrid.Name = "_drinkBackGrid";
            this._drinkBackGrid.RowHeadersVisible = false;
            this._drinkBackGrid.RowTemplate.Height = 24;
            this._drinkBackGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this._drinkBackGrid.Size = new System.Drawing.Size(312, 518);
            this._drinkBackGrid.TabIndex = 0;
            this._drinkBackGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ClickDeleteDrink);
            this._drinkBackGrid.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.ChangeCellValueOfDrinkGrid);
            // 
            // _drinkNameBackColumn
            // 
            this._drinkNameBackColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._drinkNameBackColumn.HeaderText = "飲料名稱";
            this._drinkNameBackColumn.Name = "_drinkNameBackColumn";
            // 
            // _drinkPriceBackColumn
            // 
            this._drinkPriceBackColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._drinkPriceBackColumn.HeaderText = "價格";
            this._drinkPriceBackColumn.Name = "_drinkPriceBackColumn";
            // 
            // _historyTab
            // 
            this._historyTab.Controls.Add(this._detailGroupBox);
            this._historyTab.Controls.Add(this._historyGroupBox);
            this._historyTab.Location = new System.Drawing.Point(4, 22);
            this._historyTab.Name = "_historyTab";
            this._historyTab.Padding = new System.Windows.Forms.Padding(3);
            this._historyTab.Size = new System.Drawing.Size(1084, 545);
            this._historyTab.TabIndex = 2;
            this._historyTab.Text = "歷史紀錄";
            this._historyTab.UseVisualStyleBackColor = true;
            // 
            // _detailGroupBox
            // 
            this._detailGroupBox.Controls.Add(this._detailDataGridView);
            this._detailGroupBox.Controls.Add(this._amountPanel);
            this._detailGroupBox.Dock = System.Windows.Forms.DockStyle.Right;
            this._detailGroupBox.Location = new System.Drawing.Point(439, 3);
            this._detailGroupBox.Name = "_detailGroupBox";
            this._detailGroupBox.Size = new System.Drawing.Size(642, 539);
            this._detailGroupBox.TabIndex = 3;
            this._detailGroupBox.TabStop = false;
            this._detailGroupBox.Text = "詳細";
            // 
            // _detailDataGridView
            // 
            this._detailDataGridView.AllowUserToAddRows = false;
            this._detailDataGridView.AllowUserToDeleteRows = false;
            this._detailDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._detailDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._drinkTitleColumn,
            this._sumDrinkColumn,
            this._sweetColumn,
            this._iceColumn,
            this._ingredientColumn});
            this._detailDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._detailDataGridView.Location = new System.Drawing.Point(3, 18);
            this._detailDataGridView.Name = "_detailDataGridView";
            this._detailDataGridView.ReadOnly = true;
            this._detailDataGridView.RowHeadersVisible = false;
            this._detailDataGridView.RowTemplate.Height = 24;
            this._detailDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._detailDataGridView.Size = new System.Drawing.Size(636, 418);
            this._detailDataGridView.TabIndex = 1;
            // 
            // _drinkTitleColumn
            // 
            this._drinkTitleColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this._drinkTitleColumn.HeaderText = "飲料名稱";
            this._drinkTitleColumn.Name = "_drinkTitleColumn";
            this._drinkTitleColumn.ReadOnly = true;
            this._drinkTitleColumn.Width = 78;
            // 
            // _sumDrinkColumn
            // 
            this._sumDrinkColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this._sumDrinkColumn.HeaderText = "價格";
            this._sumDrinkColumn.Name = "_sumDrinkColumn";
            this._sumDrinkColumn.ReadOnly = true;
            this._sumDrinkColumn.Width = 54;
            // 
            // _sweetColumn
            // 
            this._sweetColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this._sweetColumn.HeaderText = "甜度";
            this._sweetColumn.Name = "_sweetColumn";
            this._sweetColumn.ReadOnly = true;
            this._sweetColumn.Width = 54;
            // 
            // _iceColumn
            // 
            this._iceColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this._iceColumn.HeaderText = "溫度";
            this._iceColumn.Name = "_iceColumn";
            this._iceColumn.ReadOnly = true;
            this._iceColumn.Width = 54;
            // 
            // _ingredientColumn
            // 
            this._ingredientColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._ingredientColumn.HeaderText = "加料";
            this._ingredientColumn.Name = "_ingredientColumn";
            this._ingredientColumn.ReadOnly = true;
            // 
            // _amountPanel
            // 
            this._amountPanel.Controls.Add(this._amountLabel);
            this._amountPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._amountPanel.Location = new System.Drawing.Point(3, 436);
            this._amountPanel.Name = "_amountPanel";
            this._amountPanel.Size = new System.Drawing.Size(636, 100);
            this._amountPanel.TabIndex = 0;
            // 
            // _amountLabel
            // 
            this._amountLabel.AutoSize = true;
            this._amountLabel.Location = new System.Drawing.Point(64, 40);
            this._amountLabel.Name = "_amountLabel";
            this._amountLabel.Size = new System.Drawing.Size(32, 12);
            this._amountLabel.TabIndex = 0;
            this._amountLabel.Text = "總價:";
            // 
            // _historyGroupBox
            // 
            this._historyGroupBox.Controls.Add(this._historyDataGridView);
            this._historyGroupBox.Dock = System.Windows.Forms.DockStyle.Left;
            this._historyGroupBox.Location = new System.Drawing.Point(3, 3);
            this._historyGroupBox.Name = "_historyGroupBox";
            this._historyGroupBox.Size = new System.Drawing.Size(433, 539);
            this._historyGroupBox.TabIndex = 1;
            this._historyGroupBox.TabStop = false;
            this._historyGroupBox.Text = "歷史";
            // 
            // _historyDataGridView
            // 
            this._historyDataGridView.AllowUserToAddRows = false;
            this._historyDataGridView.AllowUserToDeleteRows = false;
            this._historyDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._historyDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._timeColumn,
            this._priceColumn});
            this._historyDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._historyDataGridView.Location = new System.Drawing.Point(3, 18);
            this._historyDataGridView.Name = "_historyDataGridView";
            this._historyDataGridView.ReadOnly = true;
            this._historyDataGridView.RowHeadersVisible = false;
            this._historyDataGridView.RowTemplate.Height = 24;
            this._historyDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._historyDataGridView.Size = new System.Drawing.Size(427, 518);
            this._historyDataGridView.TabIndex = 0;
            this._historyDataGridView.RowStateChanged += new System.Windows.Forms.DataGridViewRowStateChangedEventHandler(this.SelectHistoryItem);
            // 
            // _timeColumn
            // 
            this._timeColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._timeColumn.HeaderText = "時間";
            this._timeColumn.Name = "_timeColumn";
            this._timeColumn.ReadOnly = true;
            this._timeColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this._timeColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _priceColumn
            // 
            this._priceColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._priceColumn.HeaderText = "價格";
            this._priceColumn.Name = "_priceColumn";
            this._priceColumn.ReadOnly = true;
            this._priceColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this._priceColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewDisableButtonColumn1
            // 
            //this.dataGridViewDisableButtonColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            //this.dataGridViewDisableButtonColumn1.HeaderText = "";
            //this.dataGridViewDisableButtonColumn1.Name = "dataGridViewDisableButtonColumn1";
            //this.dataGridViewDisableButtonColumn1.Text = "選擇";
            //this.dataGridViewDisableButtonColumn1.UseColumnTextForButtonValue = true;
            //this.dataGridViewDisableButtonColumn1.Width = 5;
            // 
            // dataGridViewButtonColumn1
            // 
            //this.dataGridViewButtonColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            //this.dataGridViewButtonColumn1.HeaderText = "";
            //this.dataGridViewButtonColumn1.Name = "dataGridViewButtonColumn1";
            //this.dataGridViewButtonColumn1.Text = "刪除";
            //this.dataGridViewButtonColumn1.UseColumnTextForButtonValue = true;
            //this.dataGridViewButtonColumn1.Width = 5;
            // 
            // dataGridViewButtonColumn2
            // 
            //this.dataGridViewButtonColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            //this.dataGridViewButtonColumn2.HeaderText = "";
            //this.dataGridViewButtonColumn2.Name = "dataGridViewButtonColumn2";
            //this.dataGridViewButtonColumn2.Text = "選擇";
            //this.dataGridViewButtonColumn2.UseColumnTextForButtonValue = true;
            //this.dataGridViewButtonColumn2.Width = 5;
            // 
            // dataGridViewButtonColumn3
            // 
            //this.dataGridViewButtonColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            //this.dataGridViewButtonColumn3.HeaderText = "";
            //this.dataGridViewButtonColumn3.Name = "dataGridViewButtonColumn3";
            //this.dataGridViewButtonColumn3.Text = "刪除";
            //this.dataGridViewButtonColumn3.UseColumnTextForButtonValue = true;
            //this.dataGridViewButtonColumn3.Width = 5;
            // 
            // dataGridViewButtonColumn4
            // 
            //this.dataGridViewButtonColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            //this.dataGridViewButtonColumn4.HeaderText = "";
            //this.dataGridViewButtonColumn4.Name = "dataGridViewButtonColumn4";
            //this.dataGridViewButtonColumn4.Text = "刪除";
            //this.dataGridViewButtonColumn4.UseColumnTextForButtonValue = true;
            //this.dataGridViewButtonColumn4.Width = 5;
            // 
            // _addIngredientColumn
            // 
            this._addIngredientColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this._addIngredientColumn.HeaderText = "";
            this._addIngredientColumn.Name = "_addIngredientColumn";
            this._addIngredientColumn.ReadOnly = true;
            this._addIngredientColumn.Text = "選擇";
            this._addIngredientColumn.UseColumnTextForButtonValue = true;
            this._addIngredientColumn.Width = 5;
            // 
            // _deleteOrder
            // 
            this._deleteOrder.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this._deleteOrder.HeaderText = "";
            this._deleteOrder.Name = "_deleteOrder";
            this._deleteOrder.ReadOnly = true;
            this._deleteOrder.Text = "刪除";
            this._deleteOrder.UseColumnTextForButtonValue = true;
            this._deleteOrder.Width = 5;
            // 
            // _addDrinkColumn
            // 
            this._addDrinkColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this._addDrinkColumn.HeaderText = "";
            this._addDrinkColumn.Name = "_addDrinkColumn";
            this._addDrinkColumn.ReadOnly = true;
            this._addDrinkColumn.Text = "選擇";
            this._addDrinkColumn.UseColumnTextForButtonValue = true;
            this._addDrinkColumn.Width = 5;
            // 
            // _deleteIngredientColumn
            // 
            this._deleteIngredientColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this._deleteIngredientColumn.HeaderText = "";
            this._deleteIngredientColumn.Name = "_deleteIngredientColumn";
            this._deleteIngredientColumn.Text = "刪除";
            this._deleteIngredientColumn.UseColumnTextForButtonValue = true;
            this._deleteIngredientColumn.Width = 5;
            // 
            // _deleteDrinkButtonColumn
            // 
            this._deleteDrinkButtonColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this._deleteDrinkButtonColumn.HeaderText = "";
            this._deleteDrinkButtonColumn.Name = "_deleteDrinkButtonColumn";
            this._deleteDrinkButtonColumn.Text = "刪除";
            this._deleteDrinkButtonColumn.UseColumnTextForButtonValue = true;
            this._deleteDrinkButtonColumn.Width = 5;
            // 
            // EzDrinkForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1092, 595);
            this.Controls.Add(this._ezDrinkTabs);
            this.Controls.Add(this._menu);
            this.MainMenuStrip = this._menu;
            this.MinimumSize = new System.Drawing.Size(1108, 601);
            this.Name = "EzDrinkForm";
            this.Text = "EzDrink";
            this._menu.ResumeLayout(false);
            this._menu.PerformLayout();
            this._ezDrinkTabs.ResumeLayout(false);
            this._orderTab.ResumeLayout(false);
            this._iceContainer.ResumeLayout(false);
            this._sweetContainer.ResumeLayout(false);
            this._ingredientContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._ingredientDataGridView)).EndInit();
            this._orderContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._orderDataGridView)).EndInit();
            this._sumPanel.ResumeLayout(false);
            this._sumPanel.PerformLayout();
            this._drinkContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._drinkDataGridView)).EndInit();
            this._backEndTab.ResumeLayout(false);
            this._ingredientOperationContainer.ResumeLayout(false);
            this._ingredientOperationContainer.PerformLayout();
            this._drinkOperationContainer.ResumeLayout(false);
            this._drinkOperationContainer.PerformLayout();
            this._ingredientBackContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._ingredientBackGrid)).EndInit();
            this._drinkManageContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._drinkBackGrid)).EndInit();
            this._historyTab.ResumeLayout(false);
            this._detailGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._detailDataGridView)).EndInit();
            this._amountPanel.ResumeLayout(false);
            this._amountPanel.PerformLayout();
            this._historyGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._historyDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip _menu;
        private System.Windows.Forms.ToolStripMenuItem _fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _exitMenu;
        private System.Windows.Forms.TabControl _ezDrinkTabs;
        private System.Windows.Forms.TabPage _orderTab;
        private System.Windows.Forms.GroupBox _iceContainer;
        private System.Windows.Forms.GroupBox _sweetContainer;
        private System.Windows.Forms.Button _noSweet;
        private System.Windows.Forms.Button _fewSweet;
        private System.Windows.Forms.Button _halfSweet;
        private System.Windows.Forms.Button _normalSweet;
        private System.Windows.Forms.GroupBox _ingredientContainer;
        private System.Windows.Forms.DataGridView _ingredientDataGridView;
        private System.Windows.Forms.GroupBox _orderContainer;
        private System.Windows.Forms.DataGridView _orderDataGridView;
        private System.Windows.Forms.Panel _sumPanel;
        private System.Windows.Forms.Button _sumButton;
        private System.Windows.Forms.Label _sumLabel;
        private System.Windows.Forms.GroupBox _drinkContainer;
        private System.Windows.Forms.DataGridView _drinkDataGridView;
        //private DataGridViewDisableButtonColumn dataGridViewDisableButtonColumn1;
        //private System.Windows.Forms.DataGridViewButtonColumn dataGridViewButtonColumn1;
        //private System.Windows.Forms.DataGridViewButtonColumn dataGridViewButtonColumn2;
        private System.Windows.Forms.Button _noIce;
        private System.Windows.Forms.Button _fewIce;
        private System.Windows.Forms.Button _halfIce;
        private System.Windows.Forms.Button _normalIce;
        private System.Windows.Forms.DataGridViewButtonColumn _addDrinkColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn _drinkNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn _priceDrinkColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn _drinkNameOfOrderColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn _drinkPriceOfOrderColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn _drinkSweetOfOrderColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn _drinkIceOfOrderColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn _drinkIngredientOfOrderColumn;
        private System.Windows.Forms.DataGridViewButtonColumn _deleteOrder;
        private DataGridViewDisableButtonColumn _addIngredientColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn _nameIngredientColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn _priceIngredientColumn;
        //private DataGridViewDisableButtonColumn dataGridViewDisableButtonColumn1;
        //private System.Windows.Forms.DataGridViewButtonColumn dataGridViewButtonColumn1;
        //private System.Windows.Forms.DataGridViewButtonColumn dataGridViewButtonColumn2;
        private System.Windows.Forms.TabPage _backEndTab;
        private System.Windows.Forms.GroupBox _ingredientBackContainer;
        private System.Windows.Forms.GroupBox _drinkManageContainer;
        private System.Windows.Forms.GroupBox _drinkOperationContainer;
        private System.Windows.Forms.GroupBox _ingredientOperationContainer;
        private System.Windows.Forms.TextBox _addingDrinkPriceTextBox;
        private System.Windows.Forms.Button _drinkFromFileButton;
        private System.Windows.Forms.Label _addingPriceLabel;
        private System.Windows.Forms.TextBox _addingDrinkNameTextBox;
        private System.Windows.Forms.Label _addingNameLabel;
        private System.Windows.Forms.Button _drinkFromBoxButton;
        private System.Windows.Forms.DataGridView _ingredientBackGrid;
        private System.Windows.Forms.DataGridView _drinkBackGrid;
        private System.Windows.Forms.TextBox _addingIngredientPriceTextBox;
        private System.Windows.Forms.Button _ingredientFromFileButton;
        private System.Windows.Forms.Label _ingredientPriceLabel;
        private System.Windows.Forms.TextBox _addingIngredientNameTextBox;
        private System.Windows.Forms.Label _ingredientNameLabel;
        private System.Windows.Forms.Button _ingredientFromBoxButton;
        private System.Windows.Forms.ToolStripMenuItem _helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _aboutMenu;
        private System.Windows.Forms.DataGridViewButtonColumn _deleteIngredientColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn _ingredientNameBackColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn _ingredientPriceBackColumn;
        private System.Windows.Forms.DataGridViewButtonColumn _deleteDrinkButtonColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn _drinkNameBackColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn _drinkPriceBackColumn;
        private System.Windows.Forms.TabPage _historyTab;
        private System.Windows.Forms.GroupBox _detailGroupBox;
        private System.Windows.Forms.DataGridView _detailDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn _drinkTitleColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn _sumDrinkColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn _sweetColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn _iceColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn _ingredientColumn;
        private System.Windows.Forms.Panel _amountPanel;
        private System.Windows.Forms.Label _amountLabel;
        private System.Windows.Forms.GroupBox _historyGroupBox;
        private System.Windows.Forms.DataGridView _historyDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn _timeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn _priceColumn;
        //private DataGridViewDisableButtonColumn dataGridViewDisableButtonColumn1;
        //private System.Windows.Forms.DataGridViewButtonColumn dataGridViewButtonColumn1;
        //private System.Windows.Forms.DataGridViewButtonColumn dataGridViewButtonColumn2;
        //private System.Windows.Forms.DataGridViewButtonColumn dataGridViewButtonColumn3;
        //private System.Windows.Forms.DataGridViewButtonColumn dataGridViewButtonColumn4;
    }
}

