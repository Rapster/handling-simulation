namespace Nth.Eindhoven.Fontys
{
    partial class ComponentsBar
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
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel( );
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel( );
            this.checkInCtrl = new Airport.Forms.CheckIn( );
            this.destinationCtrl = new Airport.Forms.DestinationGate( );
            this.sorterCtrl = new Airport.Forms.Sorter( );
            this.grpComponents = new System.Windows.Forms.GroupBox( );
            this.checkBoxConveyor = new System.Windows.Forms.CheckBox( );
            this.tableLayoutPanel.SuspendLayout( );
            this.tableLayoutPanel2.SuspendLayout( );
            this.grpComponents.SuspendLayout( );
            this.SuspendLayout( );
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 3;
            this.tableLayoutPanel.ColumnStyles.Add( new System.Windows.Forms.ColumnStyle( System.Windows.Forms.SizeType.Percent, 25F ) );
            this.tableLayoutPanel.ColumnStyles.Add( new System.Windows.Forms.ColumnStyle( System.Windows.Forms.SizeType.Percent, 50F ) );
            this.tableLayoutPanel.ColumnStyles.Add( new System.Windows.Forms.ColumnStyle( System.Windows.Forms.SizeType.Percent, 25F ) );
            this.tableLayoutPanel.Controls.Add( this.tableLayoutPanel2, 1, 0 );
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point( 3, 16 );
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add( new System.Windows.Forms.RowStyle( System.Windows.Forms.SizeType.Percent, 50F ) );
            this.tableLayoutPanel.RowStyles.Add( new System.Windows.Forms.RowStyle( System.Windows.Forms.SizeType.Percent, 50F ) );
            this.tableLayoutPanel.Size = new System.Drawing.Size( 340, 328 );
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add( new System.Windows.Forms.ColumnStyle( System.Windows.Forms.SizeType.Percent, 100F ) );
            this.tableLayoutPanel2.Controls.Add( this.checkInCtrl, 0, 2 );
            this.tableLayoutPanel2.Controls.Add( this.destinationCtrl, 0, 3 );
            this.tableLayoutPanel2.Controls.Add( this.sorterCtrl, 0, 4 );
            this.tableLayoutPanel2.Controls.Add( this.checkBoxConveyor, 0, 1 );
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point( 88, 3 );
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 6;
            this.tableLayoutPanel.SetRowSpan( this.tableLayoutPanel2, 2 );
            this.tableLayoutPanel2.RowStyles.Add( new System.Windows.Forms.RowStyle( System.Windows.Forms.SizeType.Percent, 16.66667F ) );
            this.tableLayoutPanel2.RowStyles.Add( new System.Windows.Forms.RowStyle( System.Windows.Forms.SizeType.Percent, 16.66667F ) );
            this.tableLayoutPanel2.RowStyles.Add( new System.Windows.Forms.RowStyle( System.Windows.Forms.SizeType.Percent, 16.66667F ) );
            this.tableLayoutPanel2.RowStyles.Add( new System.Windows.Forms.RowStyle( System.Windows.Forms.SizeType.Percent, 16.66667F ) );
            this.tableLayoutPanel2.RowStyles.Add( new System.Windows.Forms.RowStyle( System.Windows.Forms.SizeType.Percent, 16.66667F ) );
            this.tableLayoutPanel2.RowStyles.Add( new System.Windows.Forms.RowStyle( System.Windows.Forms.SizeType.Percent, 16.66667F ) );
            this.tableLayoutPanel2.RowStyles.Add( new System.Windows.Forms.RowStyle( System.Windows.Forms.SizeType.Absolute, 20F ) );
            this.tableLayoutPanel2.Size = new System.Drawing.Size( 164, 322 );
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // checkInCtrl
            // 
            this.checkInCtrl.AllowDrop = true;
            this.checkInCtrl.BackgroundImage = global::Nth.Eindhoven.Fontys.Properties.Resources.check_in;
            this.checkInCtrl.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkInCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkInCtrl.DrawGrid = false;
            this.checkInCtrl.Id = new System.Guid( "115d1d7c-ef97-47ce-859e-a14c36bae30b" );
            this.checkInCtrl.IsMarked = false;
            this.checkInCtrl.Location = new System.Drawing.Point( 3, 109 );
            this.checkInCtrl.Name = "checkInCtrl";
            this.checkInCtrl.Position = new System.Drawing.Point( 0, 0 );
            this.checkInCtrl.Size = new System.Drawing.Size( 158, 47 );
            this.checkInCtrl.TabIndex = 2;
            this.checkInCtrl.Text = "airportControl3";
            this.checkInCtrl.MouseEnter += new System.EventHandler( this.componentMouseEnter );
            this.checkInCtrl.MouseMove += new System.Windows.Forms.MouseEventHandler( this.componentMouseMove );
            // 
            // destinationCtrl
            // 
            this.destinationCtrl.AllowDrop = true;
            this.destinationCtrl.BackgroundImage = global::Nth.Eindhoven.Fontys.Properties.Resources.check_out;
            this.destinationCtrl.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.destinationCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.destinationCtrl.DrawGrid = false;
            this.destinationCtrl.Id = new System.Guid( "95ef4623-a67b-4a7e-9fe5-a5b8a4ee8737" );
            this.destinationCtrl.IsMarked = false;
            this.destinationCtrl.Location = new System.Drawing.Point( 3, 162 );
            this.destinationCtrl.Name = "destinationCtrl";
            this.destinationCtrl.Position = new System.Drawing.Point( 0, 0 );
            this.destinationCtrl.Size = new System.Drawing.Size( 158, 47 );
            this.destinationCtrl.TabIndex = 3;
            this.destinationCtrl.Text = "airportControl4";
            this.destinationCtrl.MouseEnter += new System.EventHandler( this.componentMouseEnter );
            this.destinationCtrl.MouseMove += new System.Windows.Forms.MouseEventHandler( this.componentMouseMove );
            // 
            // sorterCtrl
            // 
            this.sorterCtrl.AllowDrop = true;
            this.sorterCtrl.BackgroundImage = global::Nth.Eindhoven.Fontys.Properties.Resources.sorter;
            this.sorterCtrl.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.sorterCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sorterCtrl.DrawGrid = false;
            this.sorterCtrl.Id = new System.Guid( "c76308e2-c1d7-414a-8253-37401920bdef" );
            this.sorterCtrl.IsMarked = false;
            this.sorterCtrl.Location = new System.Drawing.Point( 3, 215 );
            this.sorterCtrl.Name = "sorterCtrl";
            this.sorterCtrl.Position = new System.Drawing.Point( 0, 0 );
            this.sorterCtrl.Size = new System.Drawing.Size( 158, 47 );
            this.sorterCtrl.TabIndex = 4;
            this.sorterCtrl.Text = "airportControl5";
            this.sorterCtrl.MouseEnter += new System.EventHandler( this.componentMouseEnter );
            this.sorterCtrl.MouseMove += new System.Windows.Forms.MouseEventHandler( this.componentMouseMove );
            // 
            // grpComponents
            // 
            this.grpComponents.Controls.Add( this.tableLayoutPanel );
            this.grpComponents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpComponents.Location = new System.Drawing.Point( 0, 0 );
            this.grpComponents.Name = "grpComponents";
            this.grpComponents.Size = new System.Drawing.Size( 346, 347 );
            this.grpComponents.TabIndex = 1;
            this.grpComponents.TabStop = false;
            this.grpComponents.Text = "Components";
            // 
            // checkBoxConveyor
            // 
            this.checkBoxConveyor.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxConveyor.AutoSize = true;
            this.checkBoxConveyor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxConveyor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxConveyor.Image = global::Nth.Eindhoven.Fontys.Properties.Resources.conveyor;
            this.checkBoxConveyor.Location = new System.Drawing.Point( 3, 56 );
            this.checkBoxConveyor.Name = "checkBoxConveyor";
            this.checkBoxConveyor.Size = new System.Drawing.Size( 158, 47 );
            this.checkBoxConveyor.TabIndex = 5;
            this.checkBoxConveyor.UseVisualStyleBackColor = true;
            this.checkBoxConveyor.CheckedChanged += new System.EventHandler( this.checkBoxConveyor_CheckedChanged );
            // 
            // ComponentsBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add( this.grpComponents );
            this.Name = "ComponentsBar";
            this.Size = new System.Drawing.Size( 346, 347 );
            this.tableLayoutPanel.ResumeLayout( false );
            this.tableLayoutPanel2.ResumeLayout( false );
            this.tableLayoutPanel2.PerformLayout( );
            this.grpComponents.ResumeLayout( false );
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.GroupBox grpComponents;
        private Airport.Forms.CheckIn checkInCtrl;
        private Airport.Forms.DestinationGate destinationCtrl;
        private Airport.Forms.Sorter sorterCtrl;
        private System.Windows.Forms.CheckBox checkBoxConveyor;
    }
}
