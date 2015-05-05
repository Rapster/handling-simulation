namespace Nth.Eindhoven.Fontys
{
    partial class OutputControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose( );
            }
            base.Dispose( disposing );
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent( )
        {
            this.gridOuput = new System.Windows.Forms.DataGridView( );
            this.lblDescription = new System.Windows.Forms.DataGridViewLinkColumn( );
            this.lblAdvice = new System.Windows.Forms.DataGridViewTextBoxColumn( );
            this.linkDescription = new System.Windows.Forms.DataGridViewLinkColumn( );
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel( );
            this.lblErrors = new System.Windows.Forms.Label( );
            ( (System.ComponentModel.ISupportInitialize)( this.gridOuput ) ).BeginInit( );
            this.tableLayoutPanel1.SuspendLayout( );
            this.SuspendLayout( );
            // 
            // gridOuput
            // 
            this.gridOuput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridOuput.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridOuput.Columns.AddRange( new System.Windows.Forms.DataGridViewColumn[ ] {
            this.lblDescription,
            this.lblAdvice} );
            this.gridOuput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridOuput.Location = new System.Drawing.Point( 3, 23 );
            this.gridOuput.MultiSelect = false;
            this.gridOuput.Name = "gridOuput";
            this.gridOuput.RowHeadersVisible = false;
            this.gridOuput.Size = new System.Drawing.Size( 535, 165 );
            this.gridOuput.TabIndex = 0;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.lblDescription.HeaderText = "Description";
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.ReadOnly = true;
            this.lblDescription.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.lblDescription.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // lblAdvice
            // 
            this.lblAdvice.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.lblAdvice.HeaderText = "Advice";
            this.lblAdvice.Name = "lblAdvice";
            this.lblAdvice.ReadOnly = true;
            this.lblAdvice.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.lblAdvice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // linkDescription
            // 
            this.linkDescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.linkDescription.HeaderText = "Description";
            this.linkDescription.Name = "linkDescription";
            this.linkDescription.ReadOnly = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add( new System.Windows.Forms.ColumnStyle( System.Windows.Forms.SizeType.Percent, 100F ) );
            this.tableLayoutPanel1.ColumnStyles.Add( new System.Windows.Forms.ColumnStyle( System.Windows.Forms.SizeType.Absolute, 20F ) );
            this.tableLayoutPanel1.Controls.Add( this.gridOuput, 0, 1 );
            this.tableLayoutPanel1.Controls.Add( this.lblErrors, 0, 0 );
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point( 3, 3 );
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add( new System.Windows.Forms.RowStyle( System.Windows.Forms.SizeType.Absolute, 20F ) );
            this.tableLayoutPanel1.RowStyles.Add( new System.Windows.Forms.RowStyle( System.Windows.Forms.SizeType.Percent, 100F ) );
            this.tableLayoutPanel1.Size = new System.Drawing.Size( 541, 191 );
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // lblErrors
            // 
            this.lblErrors.AutoSize = true;
            this.lblErrors.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblErrors.Location = new System.Drawing.Point( 3, 3 );
            this.lblErrors.Margin = new System.Windows.Forms.Padding( 3, 3, 3, 0 );
            this.lblErrors.Name = "lblErrors";
            this.lblErrors.Size = new System.Drawing.Size( 48, 13 );
            this.lblErrors.TabIndex = 1;
            this.lblErrors.Text = "Error List";
            // 
            // OutputControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add( this.tableLayoutPanel1 );
            this.Name = "OutputControl";
            this.Padding = new System.Windows.Forms.Padding( 3 );
            this.Size = new System.Drawing.Size( 547, 197 );
            ( (System.ComponentModel.ISupportInitialize)( this.gridOuput ) ).EndInit( );
            this.tableLayoutPanel1.ResumeLayout( false );
            this.tableLayoutPanel1.PerformLayout( );
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.DataGridView gridOuput;
        private System.Windows.Forms.DataGridViewLinkColumn linkDescription;
        private System.Windows.Forms.DataGridViewLinkColumn lblDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn lblAdvice;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblErrors;
    }
}
