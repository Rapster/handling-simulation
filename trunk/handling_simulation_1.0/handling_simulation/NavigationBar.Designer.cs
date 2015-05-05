namespace Nth.Eindhoven.Fontys
{
    partial class NavigationBar
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
            this.grpBoxActions = new System.Windows.Forms.GroupBox( );
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel( );
            this.btnStop = new System.Windows.Forms.Button( );
            this.btnPlay = new System.Windows.Forms.Button( );
            this.button3 = new System.Windows.Forms.Button( );
            this.grpBoxActions.SuspendLayout( );
            this.tableLayoutPanel.SuspendLayout( );
            this.SuspendLayout( );
            // 
            // grpBoxActions
            // 
            this.grpBoxActions.Controls.Add( this.tableLayoutPanel );
            this.grpBoxActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpBoxActions.Location = new System.Drawing.Point( 0, 0 );
            this.grpBoxActions.Name = "grpBoxActions";
            this.grpBoxActions.Size = new System.Drawing.Size( 748, 61 );
            this.grpBoxActions.TabIndex = 2;
            this.grpBoxActions.TabStop = false;
            this.grpBoxActions.Text = "Actions";
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.AutoSize = true;
            this.tableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel.ColumnCount = 5;
            this.tableLayoutPanel.ColumnStyles.Add( new System.Windows.Forms.ColumnStyle( System.Windows.Forms.SizeType.Percent, 50F ) );
            this.tableLayoutPanel.ColumnStyles.Add( new System.Windows.Forms.ColumnStyle( System.Windows.Forms.SizeType.Absolute, 75F ) );
            this.tableLayoutPanel.ColumnStyles.Add( new System.Windows.Forms.ColumnStyle( System.Windows.Forms.SizeType.Absolute, 75F ) );
            this.tableLayoutPanel.ColumnStyles.Add( new System.Windows.Forms.ColumnStyle( System.Windows.Forms.SizeType.Absolute, 75F ) );
            this.tableLayoutPanel.ColumnStyles.Add( new System.Windows.Forms.ColumnStyle( System.Windows.Forms.SizeType.Percent, 50F ) );
            this.tableLayoutPanel.ColumnStyles.Add( new System.Windows.Forms.ColumnStyle( System.Windows.Forms.SizeType.Absolute, 20F ) );
            this.tableLayoutPanel.Controls.Add( this.btnStop, 1, 0 );
            this.tableLayoutPanel.Controls.Add( this.btnPlay, 2, 0 );
            this.tableLayoutPanel.Controls.Add( this.button3, 3, 0 );
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tableLayoutPanel.Location = new System.Drawing.Point( 3, 16 );
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 1;
            this.tableLayoutPanel.RowStyles.Add( new System.Windows.Forms.RowStyle( System.Windows.Forms.SizeType.Percent, 100F ) );
            this.tableLayoutPanel.Size = new System.Drawing.Size( 742, 42 );
            this.tableLayoutPanel.TabIndex = 1;
            // 
            // btnStop
            // 
            this.btnStop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnStop.Image = global::Nth.Eindhoven.Fontys.Properties.Resources.stop;
            this.btnStop.Location = new System.Drawing.Point( 261, 3 );
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size( 69, 36 );
            this.btnStop.TabIndex = 0;
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler( this.Stop );
            // 
            // btnPlay
            // 
            this.btnPlay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPlay.Image = global::Nth.Eindhoven.Fontys.Properties.Resources.play;
            this.btnPlay.Location = new System.Drawing.Point( 336, 3 );
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size( 69, 36 );
            this.btnPlay.TabIndex = 1;
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler( this.Play );
            // 
            // button3
            // 
            this.button3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button3.Image = global::Nth.Eindhoven.Fontys.Properties.Resources._switch;
            this.button3.Location = new System.Drawing.Point( 411, 3 );
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size( 69, 36 );
            this.button3.TabIndex = 2;
            this.button3.UseVisualStyleBackColor = true;
            // 
            // NavigationBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add( this.grpBoxActions );
            this.Name = "NavigationBar";
            this.Size = new System.Drawing.Size( 748, 61 );
            this.grpBoxActions.ResumeLayout( false );
            this.grpBoxActions.PerformLayout( );
            this.tableLayoutPanel.ResumeLayout( false );
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.GroupBox grpBoxActions;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.Button button3;


    }
}
