
namespace Scan2Dicom
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tbLogging = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnDelAll = new System.Windows.Forms.Button();
            this.btnDelPage = new System.Windows.Forms.Button();
            this.lblPageCount = new System.Windows.Forms.Label();
            this.btnPrevImage = new System.Windows.Forms.Button();
            this.btnNextImage = new System.Windows.Forms.Button();
            this.btnSendToPacs = new System.Windows.Forms.Button();
            this.pbPreview = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnScanStart = new System.Windows.Forms.Button();
            this.cbScannerList = new System.Windows.Forms.ComboBox();
            this.tbDokTitle = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnImportPDF = new System.Windows.Forms.Button();
            this.tbPatGebDat = new System.Windows.Forms.TextBox();
            this.tbDokCreator = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.dpDokDate = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tbPatName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbPatId = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lvResults = new System.Windows.Forms.ListView();
            this.PatId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.PatName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.PatBD = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.PatSex = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.tbSearchPatId = new System.Windows.Forms.TextBox();
            this.lblPatientName = new System.Windows.Forms.Label();
            this.tbSearchPatName = new System.Windows.Forms.TextBox();
            this.btnTestSearch = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lblDICOMEchoStorageResult = new System.Windows.Forms.Label();
            this.lblDICOMEchoQueryResult = new System.Windows.Forms.Label();
            this.btnDICOMEcho = new System.Windows.Forms.Button();
            this.btnSaveSettings = new System.Windows.Forms.Button();
            this.dgConfig = new System.Windows.Forms.DataGridView();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgConfig)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(0, -1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1264, 1039);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tbLogging);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.lvResults);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.tbSearchPatId);
            this.tabPage1.Controls.Add(this.lblPatientName);
            this.tabPage1.Controls.Add(this.tbSearchPatName);
            this.tabPage1.Controls.Add(this.btnTestSearch);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1256, 1013);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Scan2DICOM";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tbLogging
            // 
            this.tbLogging.Location = new System.Drawing.Point(7, 347);
            this.tbLogging.Multiline = true;
            this.tbLogging.Name = "tbLogging";
            this.tbLogging.ReadOnly = true;
            this.tbLogging.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbLogging.Size = new System.Drawing.Size(421, 489);
            this.tbLogging.TabIndex = 2;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnDelAll);
            this.groupBox2.Controls.Add(this.btnDelPage);
            this.groupBox2.Controls.Add(this.lblPageCount);
            this.groupBox2.Controls.Add(this.btnPrevImage);
            this.groupBox2.Controls.Add(this.btnNextImage);
            this.groupBox2.Controls.Add(this.btnSendToPacs);
            this.groupBox2.Controls.Add(this.pbPreview);
            this.groupBox2.Location = new System.Drawing.Point(440, 195);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(809, 809);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            // 
            // btnDelAll
            // 
            this.btnDelAll.Location = new System.Drawing.Point(128, 139);
            this.btnDelAll.Name = "btnDelAll";
            this.btnDelAll.Size = new System.Drawing.Size(79, 23);
            this.btnDelAll.TabIndex = 6;
            this.btnDelAll.Text = "Alle löschen";
            this.btnDelAll.UseVisualStyleBackColor = true;
            this.btnDelAll.Click += new System.EventHandler(this.btnDelAll_Click);
            // 
            // btnDelPage
            // 
            this.btnDelPage.Location = new System.Drawing.Point(128, 110);
            this.btnDelPage.Name = "btnDelPage";
            this.btnDelPage.Size = new System.Drawing.Size(79, 23);
            this.btnDelPage.TabIndex = 5;
            this.btnDelPage.Text = "Seite löschen";
            this.btnDelPage.UseVisualStyleBackColor = true;
            this.btnDelPage.Click += new System.EventHandler(this.btnDelPage_Click);
            // 
            // lblPageCount
            // 
            this.lblPageCount.AutoSize = true;
            this.lblPageCount.Location = new System.Drawing.Point(176, 25);
            this.lblPageCount.Name = "lblPageCount";
            this.lblPageCount.Size = new System.Drawing.Size(37, 13);
            this.lblPageCount.TabIndex = 4;
            this.lblPageCount.Text = "Seiten";
            // 
            // btnPrevImage
            // 
            this.btnPrevImage.Location = new System.Drawing.Point(176, 81);
            this.btnPrevImage.Name = "btnPrevImage";
            this.btnPrevImage.Size = new System.Drawing.Size(31, 23);
            this.btnPrevImage.TabIndex = 3;
            this.btnPrevImage.Text = "<";
            this.btnPrevImage.UseVisualStyleBackColor = true;
            this.btnPrevImage.Click += new System.EventHandler(this.btnPrevImage_Click);
            // 
            // btnNextImage
            // 
            this.btnNextImage.Location = new System.Drawing.Point(176, 50);
            this.btnNextImage.Name = "btnNextImage";
            this.btnNextImage.Size = new System.Drawing.Size(31, 23);
            this.btnNextImage.TabIndex = 2;
            this.btnNextImage.Text = ">";
            this.btnNextImage.UseVisualStyleBackColor = true;
            this.btnNextImage.Click += new System.EventHandler(this.btnNextImage_Click);
            // 
            // btnSendToPacs
            // 
            this.btnSendToPacs.Location = new System.Drawing.Point(15, 19);
            this.btnSendToPacs.Name = "btnSendToPacs";
            this.btnSendToPacs.Size = new System.Drawing.Size(142, 85);
            this.btnSendToPacs.TabIndex = 1;
            this.btnSendToPacs.Text = "Sende an PACS";
            this.btnSendToPacs.UseVisualStyleBackColor = true;
            this.btnSendToPacs.Click += new System.EventHandler(this.btnSendToPacs_Click);
            // 
            // pbPreview
            // 
            this.pbPreview.Location = new System.Drawing.Point(213, 19);
            this.pbPreview.Name = "pbPreview";
            this.pbPreview.Size = new System.Drawing.Size(596, 622);
            this.pbPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbPreview.TabIndex = 0;
            this.pbPreview.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnScanStart);
            this.groupBox1.Controls.Add(this.cbScannerList);
            this.groupBox1.Controls.Add(this.tbDokTitle);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.btnImportPDF);
            this.groupBox1.Controls.Add(this.tbPatGebDat);
            this.groupBox1.Controls.Add(this.tbDokCreator);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.dpDokDate);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.tbPatName);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.tbPatId);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(3, 69);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(431, 272);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            // 
            // btnScanStart
            // 
            this.btnScanStart.Location = new System.Drawing.Point(9, 219);
            this.btnScanStart.Name = "btnScanStart";
            this.btnScanStart.Size = new System.Drawing.Size(153, 40);
            this.btnScanStart.TabIndex = 20;
            this.btnScanStart.Text = "ScanStart";
            this.btnScanStart.UseVisualStyleBackColor = true;
            this.btnScanStart.Click += new System.EventHandler(this.btnScanStart_Click);
            // 
            // cbScannerList
            // 
            this.cbScannerList.FormattingEnabled = true;
            this.cbScannerList.Location = new System.Drawing.Point(9, 187);
            this.cbScannerList.Name = "cbScannerList";
            this.cbScannerList.Size = new System.Drawing.Size(153, 21);
            this.cbScannerList.TabIndex = 19;
            this.cbScannerList.DropDown += new System.EventHandler(this.cbScannerList_DropDown);
            // 
            // tbDokTitle
            // 
            this.tbDokTitle.Location = new System.Drawing.Point(132, 152);
            this.tbDokTitle.Name = "tbDokTitle";
            this.tbDokTitle.Size = new System.Drawing.Size(293, 20);
            this.tbDokTitle.TabIndex = 14;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 152);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(79, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "Dokument Titel";
            // 
            // btnImportPDF
            // 
            this.btnImportPDF.Location = new System.Drawing.Point(303, 219);
            this.btnImportPDF.Name = "btnImportPDF";
            this.btnImportPDF.Size = new System.Drawing.Size(122, 47);
            this.btnImportPDF.TabIndex = 16;
            this.btnImportPDF.Text = "Datei laden";
            this.btnImportPDF.UseVisualStyleBackColor = true;
            this.btnImportPDF.Click += new System.EventHandler(this.btnImportPDF_Click);
            // 
            // tbPatGebDat
            // 
            this.tbPatGebDat.Location = new System.Drawing.Point(132, 74);
            this.tbPatGebDat.Name = "tbPatGebDat";
            this.tbPatGebDat.ReadOnly = true;
            this.tbPatGebDat.Size = new System.Drawing.Size(293, 20);
            this.tbPatGebDat.TabIndex = 15;
            // 
            // tbDokCreator
            // 
            this.tbDokCreator.Location = new System.Drawing.Point(132, 126);
            this.tbDokCreator.Name = "tbDokCreator";
            this.tbDokCreator.Size = new System.Drawing.Size(293, 20);
            this.tbDokCreator.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 126);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(96, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Dokument Ersteller";
            // 
            // dpDokDate
            // 
            this.dpDokDate.Location = new System.Drawing.Point(132, 100);
            this.dpDokDate.Name = "dpDokDate";
            this.dpDokDate.Size = new System.Drawing.Size(293, 20);
            this.dpDokDate.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 100);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Dokument Datum";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 74);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(109, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Patient Geburtsdatum";
            // 
            // tbPatName
            // 
            this.tbPatName.Location = new System.Drawing.Point(132, 48);
            this.tbPatName.Name = "tbPatName";
            this.tbPatName.ReadOnly = true;
            this.tbPatName.Size = new System.Drawing.Size(293, 20);
            this.tbPatName.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Patient Name";
            // 
            // tbPatId
            // 
            this.tbPatId.Location = new System.Drawing.Point(132, 22);
            this.tbPatId.Name = "tbPatId";
            this.tbPatId.ReadOnly = true;
            this.tbPatId.Size = new System.Drawing.Size(293, 20);
            this.tbPatId.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Patient ID";
            // 
            // lvResults
            // 
            this.lvResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.PatId,
            this.PatName,
            this.PatBD,
            this.PatSex});
            this.lvResults.HideSelection = false;
            this.lvResults.Location = new System.Drawing.Point(440, 9);
            this.lvResults.MultiSelect = false;
            this.lvResults.Name = "lvResults";
            this.lvResults.Size = new System.Drawing.Size(809, 176);
            this.lvResults.TabIndex = 4;
            this.lvResults.UseCompatibleStateImageBehavior = false;
            this.lvResults.View = System.Windows.Forms.View.Details;
            this.lvResults.SelectedIndexChanged += new System.EventHandler(this.lvResults_SelectedIndexChanged);
            // 
            // PatId
            // 
            this.PatId.Tag = "1";
            this.PatId.Text = "PatientID";
            this.PatId.Width = 118;
            // 
            // PatName
            // 
            this.PatName.Tag = "2";
            this.PatName.Text = "Name";
            this.PatName.Width = 135;
            // 
            // PatBD
            // 
            this.PatBD.Tag = "3";
            this.PatBD.Text = "Geburtsdatum";
            this.PatBD.Width = 123;
            // 
            // PatSex
            // 
            this.PatSex.Text = "Geschlecht";
            this.PatSex.Width = 97;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "PatentenID";
            // 
            // tbSearchPatId
            // 
            this.tbSearchPatId.Location = new System.Drawing.Point(71, 3);
            this.tbSearchPatId.Name = "tbSearchPatId";
            this.tbSearchPatId.Size = new System.Drawing.Size(141, 20);
            this.tbSearchPatId.TabIndex = 1;
            this.tbSearchPatId.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SearchBox_KeyDown);
            // 
            // lblPatientName
            // 
            this.lblPatientName.AutoSize = true;
            this.lblPatientName.Location = new System.Drawing.Point(4, 35);
            this.lblPatientName.Name = "lblPatientName";
            this.lblPatientName.Size = new System.Drawing.Size(35, 13);
            this.lblPatientName.TabIndex = 2;
            this.lblPatientName.Text = "Name";
            // 
            // tbSearchPatName
            // 
            this.tbSearchPatName.Location = new System.Drawing.Point(71, 32);
            this.tbSearchPatName.Name = "tbSearchPatName";
            this.tbSearchPatName.Size = new System.Drawing.Size(141, 20);
            this.tbSearchPatName.TabIndex = 2;
            this.tbSearchPatName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SearchBox_KeyDown);
            // 
            // btnTestSearch
            // 
            this.btnTestSearch.Location = new System.Drawing.Point(232, 29);
            this.btnTestSearch.Name = "btnTestSearch";
            this.btnTestSearch.Size = new System.Drawing.Size(75, 23);
            this.btnTestSearch.TabIndex = 3;
            this.btnTestSearch.Text = "TestSuche";
            this.btnTestSearch.UseVisualStyleBackColor = true;
            this.btnTestSearch.Click += new System.EventHandler(this.btnTestSearch_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.lblDICOMEchoStorageResult);
            this.tabPage2.Controls.Add(this.lblDICOMEchoQueryResult);
            this.tabPage2.Controls.Add(this.btnDICOMEcho);
            this.tabPage2.Controls.Add(this.btnSaveSettings);
            this.tabPage2.Controls.Add(this.dgConfig);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1256, 1013);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Konfiguration";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lblDICOMEchoStorageResult
            // 
            this.lblDICOMEchoStorageResult.AutoSize = true;
            this.lblDICOMEchoStorageResult.Location = new System.Drawing.Point(102, 32);
            this.lblDICOMEchoStorageResult.Name = "lblDICOMEchoStorageResult";
            this.lblDICOMEchoStorageResult.Size = new System.Drawing.Size(35, 13);
            this.lblDICOMEchoStorageResult.TabIndex = 3;
            this.lblDICOMEchoStorageResult.Text = "Echo:";
            // 
            // lblDICOMEchoQueryResult
            // 
            this.lblDICOMEchoQueryResult.AutoSize = true;
            this.lblDICOMEchoQueryResult.Location = new System.Drawing.Point(102, 19);
            this.lblDICOMEchoQueryResult.Name = "lblDICOMEchoQueryResult";
            this.lblDICOMEchoQueryResult.Size = new System.Drawing.Size(35, 13);
            this.lblDICOMEchoQueryResult.TabIndex = 3;
            this.lblDICOMEchoQueryResult.Text = "Echo:";
            // 
            // btnDICOMEcho
            // 
            this.btnDICOMEcho.Location = new System.Drawing.Point(8, 19);
            this.btnDICOMEcho.Name = "btnDICOMEcho";
            this.btnDICOMEcho.Size = new System.Drawing.Size(88, 23);
            this.btnDICOMEcho.TabIndex = 2;
            this.btnDICOMEcho.Text = "DICOM-Echo";
            this.btnDICOMEcho.UseVisualStyleBackColor = true;
            this.btnDICOMEcho.Click += new System.EventHandler(this.btnDICOMEcho_Click);
            // 
            // btnSaveSettings
            // 
            this.btnSaveSettings.Location = new System.Drawing.Point(18, 757);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new System.Drawing.Size(114, 35);
            this.btnSaveSettings.TabIndex = 1;
            this.btnSaveSettings.Text = "Speichern";
            this.btnSaveSettings.UseVisualStyleBackColor = true;
            this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
            // 
            // dgConfig
            // 
            this.dgConfig.AllowUserToAddRows = false;
            this.dgConfig.AllowUserToDeleteRows = false;
            this.dgConfig.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgConfig.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgConfig.BackgroundColor = System.Drawing.SystemColors.ActiveBorder;
            this.dgConfig.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgConfig.Location = new System.Drawing.Point(6, 198);
            this.dgConfig.Name = "dgConfig";
            this.dgConfig.Size = new System.Drawing.Size(1241, 521);
            this.dgConfig.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1265, 869);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Scan2DICOM";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgConfig)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dgConfig;
        private System.Windows.Forms.Button btnSaveSettings;
        private System.Windows.Forms.Label lblDICOMEchoQueryResult;
        private System.Windows.Forms.Button btnDICOMEcho;
        private System.Windows.Forms.Button btnTestSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbSearchPatId;
        private System.Windows.Forms.Label lblPatientName;
        private System.Windows.Forms.TextBox tbSearchPatName;
        private System.Windows.Forms.ListView lvResults;
        private System.Windows.Forms.ColumnHeader PatId;
        private System.Windows.Forms.ColumnHeader PatName;
        private System.Windows.Forms.ColumnHeader PatBD;
        private System.Windows.Forms.Label lblDICOMEchoStorageResult;
        private System.Windows.Forms.ColumnHeader PatSex;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbDokCreator;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dpDokDate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbPatName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbPatId;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbPatGebDat;
        private System.Windows.Forms.Button btnImportPDF;
        private System.Windows.Forms.TextBox tbDokTitle;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.PictureBox pbPreview;
        private System.Windows.Forms.Button btnSendToPacs;
        private System.Windows.Forms.TextBox tbLogging;
        private System.Windows.Forms.ComboBox cbScannerList;
        private System.Windows.Forms.Button btnScanStart;
        private System.Windows.Forms.Button btnNextImage;
        private System.Windows.Forms.Button btnPrevImage;
        private System.Windows.Forms.Label lblPageCount;
        private System.Windows.Forms.Button btnDelAll;
        private System.Windows.Forms.Button btnDelPage;
    }
}

