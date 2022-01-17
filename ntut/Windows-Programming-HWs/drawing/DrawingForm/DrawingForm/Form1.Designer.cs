namespace DrawingForm
{
    partial class Form1
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
            this._tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this._undoButton = new System.Windows.Forms.Button();
            this._redoButton = new System.Windows.Forms.Button();
            this._rectangleButton = new System.Windows.Forms.Button();
            this._circleButton = new System.Windows.Forms.Button();
            this._triangleButton = new System.Windows.Forms.Button();
            this._saveButton = new System.Windows.Forms.Button();
            this._canvas = new DrawingForm.DoubleBufferedPanel();
            this._tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _tableLayoutPanel1
            // 
            this._tableLayoutPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this._tableLayoutPanel1.ColumnCount = 7;
            this._tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 88F));
            this._tableLayoutPanel1.Controls.Add(this._undoButton, 0, 0);
            this._tableLayoutPanel1.Controls.Add(this._redoButton, 1, 0);
            this._tableLayoutPanel1.Controls.Add(this._rectangleButton, 2, 0);
            this._tableLayoutPanel1.Controls.Add(this._circleButton, 5, 0);
            this._tableLayoutPanel1.Controls.Add(this._triangleButton, 4, 0);
            this._tableLayoutPanel1.Controls.Add(this._saveButton, 6, 0);
            this._tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this._tableLayoutPanel1.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.AddColumns;
            this._tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this._tableLayoutPanel1.Name = "_tableLayoutPanel1";
            this._tableLayoutPanel1.RowCount = 1;
            this._tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this._tableLayoutPanel1.Size = new System.Drawing.Size(493, 30);
            this._tableLayoutPanel1.TabIndex = 0;
            // 
            // _undoButton
            // 
            this._undoButton.Location = new System.Drawing.Point(3, 3);
            this._undoButton.Name = "_undoButton";
            this._undoButton.Size = new System.Drawing.Size(75, 23);
            this._undoButton.TabIndex = 0;
            this._undoButton.Text = "Undo";
            this._undoButton.UseVisualStyleBackColor = true;
            this._undoButton.Click += new System.EventHandler(this.ClickUndo);
            // 
            // _redoButton
            // 
            this._redoButton.Location = new System.Drawing.Point(84, 3);
            this._redoButton.Name = "_redoButton";
            this._redoButton.Size = new System.Drawing.Size(75, 23);
            this._redoButton.TabIndex = 1;
            this._redoButton.Text = "Redo";
            this._redoButton.UseVisualStyleBackColor = true;
            this._redoButton.Click += new System.EventHandler(this.ClickRedo);
            // 
            // _rectangleButton
            // 
            this._rectangleButton.Location = new System.Drawing.Point(165, 3);
            this._rectangleButton.Name = "_rectangleButton";
            this._rectangleButton.Size = new System.Drawing.Size(75, 23);
            this._rectangleButton.TabIndex = 2;
            this._rectangleButton.Text = "Rectangle";
            this._rectangleButton.UseVisualStyleBackColor = true;
            this._rectangleButton.Click += new System.EventHandler(this.ClickRectangle);
            // 
            // _circleButton
            // 
            this._circleButton.Location = new System.Drawing.Point(327, 3);
            this._circleButton.Name = "_circleButton";
            this._circleButton.Size = new System.Drawing.Size(75, 23);
            this._circleButton.TabIndex = 3;
            this._circleButton.Text = "Circle";
            this._circleButton.UseVisualStyleBackColor = true;
            this._circleButton.Click += new System.EventHandler(this.ClickCircle);
            // 
            // _triangleButton
            // 
            this._triangleButton.Location = new System.Drawing.Point(246, 3);
            this._triangleButton.Name = "_triangleButton";
            this._triangleButton.Size = new System.Drawing.Size(75, 23);
            this._triangleButton.TabIndex = 4;
            this._triangleButton.Text = "Triangle";
            this._triangleButton.UseVisualStyleBackColor = true;
            this._triangleButton.Click += new System.EventHandler(this.ClickTriangle);
            // 
            // _saveButton
            // 
            this._saveButton.Location = new System.Drawing.Point(408, 3);
            this._saveButton.Name = "_saveButton";
            this._saveButton.Size = new System.Drawing.Size(75, 23);
            this._saveButton.TabIndex = 5;
            this._saveButton.Text = "Save in Bmp";
            this._saveButton.UseVisualStyleBackColor = true;
            this._saveButton.Click += new System.EventHandler(this.ClickSave);
            // 
            // _canvas
            // 
            this._canvas.BackColor = System.Drawing.Color.Khaki;
            this._canvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this._canvas.Location = new System.Drawing.Point(0, 30);
            this._canvas.Name = "_canvas";
            this._canvas.Size = new System.Drawing.Size(493, 362);
            this._canvas.TabIndex = 1;
            this._canvas.Paint += new System.Windows.Forms.PaintEventHandler(this.HandleCanvasPaint);
            this._canvas.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleCanvasPressed);
            this._canvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.HandleCanvasMoved);
            this._canvas.MouseUp += new System.Windows.Forms.MouseEventHandler(this.HandleCanvasReleased);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(493, 392);
            this.Controls.Add(this._canvas);
            this.Controls.Add(this._tableLayoutPanel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this._tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel1;
        private System.Windows.Forms.Button _undoButton;
        private System.Windows.Forms.Button _redoButton;
        private System.Windows.Forms.Button _rectangleButton;
        private System.Windows.Forms.Button _circleButton;
        private System.Windows.Forms.Button _triangleButton;
        private System.Windows.Forms.Button _saveButton;
        private DoubleBufferedPanel _canvas;
    }
}

