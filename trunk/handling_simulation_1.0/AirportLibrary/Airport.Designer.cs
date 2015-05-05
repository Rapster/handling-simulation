namespace Airport.Forms
{
    partial class AirportArea
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
            this.paintPanel = new Airport.Forms.PaintPanel( );
            this.SuspendLayout( );
            // 
            // paintPanel
            // 
            this.paintPanel.BackColor = System.Drawing.SystemColors.Control;
            this.paintPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paintPanel.Draw = false;
            this.paintPanel.Enabled = false;
            this.paintPanel.Location = new System.Drawing.Point( 3, 3 );
            this.paintPanel.Name = "paintPanel";
            this.paintPanel.Size = new System.Drawing.Size( 732, 361 );
            this.paintPanel.TabIndex = 0;
            this.paintPanel.MouseDown += new System.Windows.Forms.MouseEventHandler( this.PaintPanelMouseDown );
            this.paintPanel.MouseMove += new System.Windows.Forms.MouseEventHandler( this.PaintPanelMouseMove );
            this.paintPanel.MouseUp += new System.Windows.Forms.MouseEventHandler( this.PaintPanelMouseUp );
            // 
            // AirportArea
            // 
            this.AllowDrop = true;
            this.AutoSize = true;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add( this.paintPanel );
            this.Padding = new System.Windows.Forms.Padding( 3 );
            this.Size = new System.Drawing.Size( 740, 369 );
            this.ResumeLayout( false );

        }

        #endregion

        private PaintPanel paintPanel;
    }
}
