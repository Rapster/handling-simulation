namespace Nth.Eindhoven.Fontys
{
    partial class PropertiesControl
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
            this.tbCtrlProperties = new System.Windows.Forms.TabControl( );
            this.Properties = new System.Windows.Forms.TabPage( );
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel( );
            this.cmboComponents = new System.Windows.Forms.ComboBox( );
            this.propertyGrid = new System.Windows.Forms.PropertyGrid( );
            this.tbCtrlProperties.SuspendLayout( );
            this.Properties.SuspendLayout( );
            this.tableLayoutPanel.SuspendLayout( );
            this.SuspendLayout( );
            // 
            // tbCtrlProperties
            // 
            this.tbCtrlProperties.Controls.Add( this.Properties );
            this.tbCtrlProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbCtrlProperties.Location = new System.Drawing.Point( 0, 0 );
            this.tbCtrlProperties.Name = "tbCtrlProperties";
            this.tbCtrlProperties.SelectedIndex = 0;
            this.tbCtrlProperties.Size = new System.Drawing.Size( 369, 304 );
            this.tbCtrlProperties.TabIndex = 0;
            // 
            // Properties
            // 
            this.Properties.Controls.Add( this.tableLayoutPanel );
            this.Properties.Location = new System.Drawing.Point( 4, 22 );
            this.Properties.Name = "Properties";
            this.Properties.Padding = new System.Windows.Forms.Padding( 3 );
            this.Properties.Size = new System.Drawing.Size( 361, 278 );
            this.Properties.TabIndex = 0;
            this.Properties.Text = "Properties";
            this.Properties.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add( new System.Windows.Forms.ColumnStyle( System.Windows.Forms.SizeType.Percent, 100F ) );
            this.tableLayoutPanel.Controls.Add( this.cmboComponents, 0, 0 );
            this.tableLayoutPanel.Controls.Add( this.propertyGrid, 0, 1 );
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point( 3, 3 );
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add( new System.Windows.Forms.RowStyle( System.Windows.Forms.SizeType.Absolute, 30F ) );
            this.tableLayoutPanel.RowStyles.Add( new System.Windows.Forms.RowStyle( System.Windows.Forms.SizeType.Percent, 100F ) );
            this.tableLayoutPanel.Size = new System.Drawing.Size( 355, 272 );
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // cmboComponents
            // 
            this.cmboComponents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmboComponents.FormattingEnabled = true;
            this.cmboComponents.Location = new System.Drawing.Point( 3, 3 );
            this.cmboComponents.Name = "cmboComponents";
            this.cmboComponents.Size = new System.Drawing.Size( 349, 21 );
            this.cmboComponents.TabIndex = 0;
            this.cmboComponents.SelectedIndexChanged += new System.EventHandler( this.cmboComponents_SelectedIndexChanged );
            // 
            // propertyGrid
            // 
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.Location = new System.Drawing.Point( 3, 33 );
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size( 349, 236 );
            this.propertyGrid.TabIndex = 1;
            // 
            // PropertiesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add( this.tbCtrlProperties );
            this.Name = "PropertiesControl";
            this.Size = new System.Drawing.Size( 369, 304 );
            this.tbCtrlProperties.ResumeLayout( false );
            this.Properties.ResumeLayout( false );
            this.tableLayoutPanel.ResumeLayout( false );
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.TabControl tbCtrlProperties;
        private System.Windows.Forms.TabPage Properties;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.ComboBox cmboComponents;
        private System.Windows.Forms.PropertyGrid propertyGrid;
    }
}
