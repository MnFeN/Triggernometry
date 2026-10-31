namespace Triggernometry.UI.Forms
{
    partial class AuraDesignForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AuraDesignForm));
            this.tmrSelectionBlink = new System.Windows.Forms.Timer(this.components);
            this.ctxOverlayDesign = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ctxImage = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxWidthToImageWidth = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxHeightToImageHeight = new System.Windows.Forms.ToolStripMenuItem();
            this.sepImageDisplay = new System.Windows.Forms.ToolStripSeparator();
            this.ctxDisplayMethod = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxDisplayNormal = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxDisplayStretch = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxDisplayCentered = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxDisplayZoomed = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxText = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxTextAlignment = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxTextAlignmentTopLeft = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxTextAlignmentTopCenter = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxTextAlignmentTopRight = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxTextAlignmentMiddleLeft = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxTextAlignmentMiddleCenter = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxTextAlignmentMiddleRight = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxTextAlignmentBottomLeft = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxTextAlignmentBottomCenter = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxTextAlignmentBottomRight = new System.Windows.Forms.ToolStripMenuItem();
            this.sepContextSave = new System.Windows.Forms.ToolStripSeparator();
            this.ctxSaveChanges = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxDiscardChanges = new System.Windows.Forms.ToolStripMenuItem();
            this.picOverlayPreview = new Triggernometry.UI.CustomControls.ClickthroughPictureBox();
            this.ctxOverlayDesign.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picOverlayPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // tmrSelectionBlink
            // 
            this.tmrSelectionBlink.Enabled = true;
            this.tmrSelectionBlink.Interval = 500;
            this.tmrSelectionBlink.Tick += new System.EventHandler(this.tmrSelectionBlink_Tick);
            // 
            // ctxOverlayDesign
            // 
            this.ctxOverlayDesign.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctxImage,
            this.ctxText,
            this.sepContextSave,
            this.ctxSaveChanges,
            this.ctxDiscardChanges});
            this.ctxOverlayDesign.Name = "ctxOverlayDesign";
            this.ctxOverlayDesign.Size = new System.Drawing.Size(161, 98);
            this.ctxOverlayDesign.Opening += new System.ComponentModel.CancelEventHandler(this.ctxOverlayDesign_Opening);
            // 
            // ctxImage
            // 
            this.ctxImage.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctxWidthToImageWidth,
            this.ctxHeightToImageHeight,
            this.sepImageDisplay,
            this.ctxDisplayMethod});
            this.ctxImage.Image = ((System.Drawing.Image)(resources.GetObject("ctxImage.Image")));
            this.ctxImage.Name = "ctxImage";
            this.ctxImage.Size = new System.Drawing.Size(160, 22);
            this.ctxImage.Text = "Image ";
            // 
            // ctxWidthToImageWidth
            // 
            this.ctxWidthToImageWidth.Name = "ctxWidthToImageWidth";
            this.ctxWidthToImageWidth.Size = new System.Drawing.Size(214, 22);
            this.ctxWidthToImageWidth.Text = "Set width to image width";
            this.ctxWidthToImageWidth.Click += new System.EventHandler(this.setWidthToImageWidthToolStripMenuItem_Click);
            // 
            // ctxHeightToImageHeight
            // 
            this.ctxHeightToImageHeight.Name = "ctxHeightToImageHeight";
            this.ctxHeightToImageHeight.Size = new System.Drawing.Size(214, 22);
            this.ctxHeightToImageHeight.Text = "Set height to image height";
            this.ctxHeightToImageHeight.Click += new System.EventHandler(this.setHeightToImageHeightToolStripMenuItem_Click);
            // 
            // sepImageDisplay
            // 
            this.sepImageDisplay.Name = "sepImageDisplay";
            this.sepImageDisplay.Size = new System.Drawing.Size(211, 6);
            // 
            // ctxDisplayMethod
            // 
            this.ctxDisplayMethod.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctxDisplayNormal,
            this.ctxDisplayStretch,
            this.ctxDisplayCentered,
            this.ctxDisplayZoomed});
            this.ctxDisplayMethod.Name = "ctxDisplayMethod";
            this.ctxDisplayMethod.Size = new System.Drawing.Size(214, 22);
            this.ctxDisplayMethod.Text = "Display method";
            // 
            // ctxDisplayNormal
            // 
            this.ctxDisplayNormal.CheckOnClick = true;
            this.ctxDisplayNormal.Name = "ctxDisplayNormal";
            this.ctxDisplayNormal.Size = new System.Drawing.Size(276, 22);
            this.ctxDisplayNormal.Text = "Normal";
            this.ctxDisplayNormal.CheckedChanged += new System.EventHandler(this.onceToolStripMenuItem_CheckedChanged);
            this.ctxDisplayNormal.Click += new System.EventHandler(this.onceToolStripMenuItem_Click);
            // 
            // ctxDisplayStretch
            // 
            this.ctxDisplayStretch.CheckOnClick = true;
            this.ctxDisplayStretch.Name = "ctxDisplayStretch";
            this.ctxDisplayStretch.Size = new System.Drawing.Size(276, 22);
            this.ctxDisplayStretch.Text = "Stretched to fill";
            this.ctxDisplayStretch.Click += new System.EventHandler(this.stretchedToolStripMenuItem_Click);
            // 
            // ctxDisplayCentered
            // 
            this.ctxDisplayCentered.CheckOnClick = true;
            this.ctxDisplayCentered.Name = "ctxDisplayCentered";
            this.ctxDisplayCentered.Size = new System.Drawing.Size(276, 22);
            this.ctxDisplayCentered.Text = "Centered";
            this.ctxDisplayCentered.Click += new System.EventHandler(this.centeredToolStripMenuItem_Click);
            // 
            // ctxDisplayZoomed
            // 
            this.ctxDisplayZoomed.CheckOnClick = true;
            this.ctxDisplayZoomed.Name = "ctxDisplayZoomed";
            this.ctxDisplayZoomed.Size = new System.Drawing.Size(276, 22);
            this.ctxDisplayZoomed.Text = "Zoomed to fill respecting Aspect Ratio";
            this.ctxDisplayZoomed.Click += new System.EventHandler(this.stretchedWithAspectRatioToolStripMenuItem_Click);
            // 
            // ctxText
            // 
            this.ctxText.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctxTextAlignment});
            this.ctxText.Image = ((System.Drawing.Image)(resources.GetObject("ctxText.Image")));
            this.ctxText.Name = "ctxText";
            this.ctxText.Size = new System.Drawing.Size(160, 22);
            this.ctxText.Text = "Text";
            this.ctxText.DropDownOpening += new System.EventHandler(this.textToolStripMenuItem_DropDownOpening);
            // 
            // ctxTextAlignment
            // 
            this.ctxTextAlignment.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctxTextAlignmentTopLeft,
            this.ctxTextAlignmentTopCenter,
            this.ctxTextAlignmentTopRight,
            this.ctxTextAlignmentMiddleLeft,
            this.ctxTextAlignmentMiddleCenter,
            this.ctxTextAlignmentMiddleRight,
            this.ctxTextAlignmentBottomLeft,
            this.ctxTextAlignmentBottomCenter,
            this.ctxTextAlignmentBottomRight});
            this.ctxTextAlignment.Name = "ctxTextAlignment";
            this.ctxTextAlignment.Size = new System.Drawing.Size(130, 22);
            this.ctxTextAlignment.Text = "Alignment";
            // 
            // ctxTextAlignmentTopLeft
            // 
            this.ctxTextAlignmentTopLeft.CheckOnClick = true;
            this.ctxTextAlignmentTopLeft.Name = "ctxTextAlignmentTopLeft";
            this.ctxTextAlignmentTopLeft.Size = new System.Drawing.Size(150, 22);
            this.ctxTextAlignmentTopLeft.Text = "Top left";
            this.ctxTextAlignmentTopLeft.Click += new System.EventHandler(this.ctxTextAlignmentTopLeft_Click);
            // 
            // ctxTextAlignmentTopCenter
            // 
            this.ctxTextAlignmentTopCenter.CheckOnClick = true;
            this.ctxTextAlignmentTopCenter.Name = "ctxTextAlignmentTopCenter";
            this.ctxTextAlignmentTopCenter.Size = new System.Drawing.Size(150, 22);
            this.ctxTextAlignmentTopCenter.Text = "Top center";
            this.ctxTextAlignmentTopCenter.Click += new System.EventHandler(this.ctxTextAlignmentTopCenter_Click);
            // 
            // ctxTextAlignmentTopRight
            // 
            this.ctxTextAlignmentTopRight.CheckOnClick = true;
            this.ctxTextAlignmentTopRight.Name = "ctxTextAlignmentTopRight";
            this.ctxTextAlignmentTopRight.Size = new System.Drawing.Size(150, 22);
            this.ctxTextAlignmentTopRight.Text = "Top right";
            this.ctxTextAlignmentTopRight.Click += new System.EventHandler(this.ctxTextAlignmentTopRight_Click);
            // 
            // ctxTextAlignmentMiddleLeft
            // 
            this.ctxTextAlignmentMiddleLeft.CheckOnClick = true;
            this.ctxTextAlignmentMiddleLeft.Name = "ctxTextAlignmentMiddleLeft";
            this.ctxTextAlignmentMiddleLeft.Size = new System.Drawing.Size(150, 22);
            this.ctxTextAlignmentMiddleLeft.Text = "Middle left";
            this.ctxTextAlignmentMiddleLeft.Click += new System.EventHandler(this.ctxTextAlignmentMiddleLeft_Click);
            // 
            // ctxTextAlignmentMiddleCenter
            // 
            this.ctxTextAlignmentMiddleCenter.CheckOnClick = true;
            this.ctxTextAlignmentMiddleCenter.Name = "ctxTextAlignmentMiddleCenter";
            this.ctxTextAlignmentMiddleCenter.Size = new System.Drawing.Size(150, 22);
            this.ctxTextAlignmentMiddleCenter.Text = "Middle center";
            this.ctxTextAlignmentMiddleCenter.Click += new System.EventHandler(this.ctxTextAlignmentMiddleCenter_Click);
            // 
            // ctxTextAlignmentMiddleRight
            // 
            this.ctxTextAlignmentMiddleRight.CheckOnClick = true;
            this.ctxTextAlignmentMiddleRight.Name = "ctxTextAlignmentMiddleRight";
            this.ctxTextAlignmentMiddleRight.Size = new System.Drawing.Size(150, 22);
            this.ctxTextAlignmentMiddleRight.Text = "Middle right";
            this.ctxTextAlignmentMiddleRight.Click += new System.EventHandler(this.ctxTextAlignmentMiddleRight_Click);
            // 
            // ctxTextAlignmentBottomLeft
            // 
            this.ctxTextAlignmentBottomLeft.CheckOnClick = true;
            this.ctxTextAlignmentBottomLeft.Name = "ctxTextAlignmentBottomLeft";
            this.ctxTextAlignmentBottomLeft.Size = new System.Drawing.Size(150, 22);
            this.ctxTextAlignmentBottomLeft.Text = "Bottom left";
            this.ctxTextAlignmentBottomLeft.Click += new System.EventHandler(this.ctxTextAlignmentBottomLeft_Click);
            // 
            // ctxTextAlignmentBottomCenter
            // 
            this.ctxTextAlignmentBottomCenter.CheckOnClick = true;
            this.ctxTextAlignmentBottomCenter.Name = "ctxTextAlignmentBottomCenter";
            this.ctxTextAlignmentBottomCenter.Size = new System.Drawing.Size(150, 22);
            this.ctxTextAlignmentBottomCenter.Text = "Bottom center";
            this.ctxTextAlignmentBottomCenter.Click += new System.EventHandler(this.ctxTextAlignmentBottomCenter_Click);
            // 
            // ctxTextAlignmentBottomRight
            // 
            this.ctxTextAlignmentBottomRight.CheckOnClick = true;
            this.ctxTextAlignmentBottomRight.Name = "ctxTextAlignmentBottomRight";
            this.ctxTextAlignmentBottomRight.Size = new System.Drawing.Size(150, 22);
            this.ctxTextAlignmentBottomRight.Text = "Bottom right";
            this.ctxTextAlignmentBottomRight.Click += new System.EventHandler(this.ctxTextAlignmentBottomRight_Click);
            // 
            // sepContextSave
            // 
            this.sepContextSave.Name = "sepContextSave";
            this.sepContextSave.Size = new System.Drawing.Size(157, 6);
            // 
            // ctxSaveChanges
            // 
            this.ctxSaveChanges.Image = ((System.Drawing.Image)(resources.GetObject("ctxSaveChanges.Image")));
            this.ctxSaveChanges.Name = "ctxSaveChanges";
            this.ctxSaveChanges.Size = new System.Drawing.Size(160, 22);
            this.ctxSaveChanges.Text = "Save changes";
            this.ctxSaveChanges.Click += new System.EventHandler(this.applyToolStripMenuItem_Click);
            // 
            // ctxDiscardChanges
            // 
            this.ctxDiscardChanges.Image = ((System.Drawing.Image)(resources.GetObject("ctxDiscardChanges.Image")));
            this.ctxDiscardChanges.Name = "ctxDiscardChanges";
            this.ctxDiscardChanges.Size = new System.Drawing.Size(160, 22);
            this.ctxDiscardChanges.Text = "Discard changes";
            this.ctxDiscardChanges.Click += new System.EventHandler(this.discardChangesToolStripMenuItem_Click);
            // 
            // picOverlayPreview
            // 
            this.picOverlayPreview.BackColor = System.Drawing.Color.Transparent;
            this.picOverlayPreview.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.picOverlayPreview.ContextMenuStrip = this.ctxOverlayDesign;
            this.picOverlayPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picOverlayPreview.Location = new System.Drawing.Point(0, 0);
            this.picOverlayPreview.Name = "picOverlayPreview";
            this.picOverlayPreview.Size = new System.Drawing.Size(270, 91);
            this.picOverlayPreview.TabIndex = 0;
            this.picOverlayPreview.TabStop = false;
            // 
            // AuraDesignForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Fuchsia;
            this.ClientSize = new System.Drawing.Size(270, 91);
            this.ContextMenuStrip = this.ctxOverlayDesign;
            this.ControlBox = false;
            this.Controls.Add(this.picOverlayPreview);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(30, 30);
            this.Name = "AuraDesignForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.TopMost = true;
            this.ctxOverlayDesign.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picOverlayPreview)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer tmrSelectionBlink;
        private CustomControls.ClickthroughPictureBox picOverlayPreview;
        private System.Windows.Forms.ContextMenuStrip ctxOverlayDesign;
        private System.Windows.Forms.ToolStripMenuItem ctxImage;
        private System.Windows.Forms.ToolStripSeparator sepContextSave;
        private System.Windows.Forms.ToolStripMenuItem ctxSaveChanges;
        private System.Windows.Forms.ToolStripMenuItem ctxWidthToImageWidth;
        private System.Windows.Forms.ToolStripMenuItem ctxHeightToImageHeight;
        private System.Windows.Forms.ToolStripSeparator sepImageDisplay;
        private System.Windows.Forms.ToolStripMenuItem ctxDiscardChanges;
        private System.Windows.Forms.ToolStripMenuItem ctxDisplayMethod;
        private System.Windows.Forms.ToolStripMenuItem ctxDisplayNormal;
        private System.Windows.Forms.ToolStripMenuItem ctxDisplayCentered;
        private System.Windows.Forms.ToolStripMenuItem ctxDisplayStretch;
        private System.Windows.Forms.ToolStripMenuItem ctxDisplayZoomed;
        private System.Windows.Forms.ToolStripMenuItem ctxText;
        private System.Windows.Forms.ToolStripMenuItem ctxTextAlignment;
        private System.Windows.Forms.ToolStripMenuItem ctxTextAlignmentTopLeft;
        private System.Windows.Forms.ToolStripMenuItem ctxTextAlignmentTopCenter;
        private System.Windows.Forms.ToolStripMenuItem ctxTextAlignmentTopRight;
        private System.Windows.Forms.ToolStripMenuItem ctxTextAlignmentMiddleLeft;
        private System.Windows.Forms.ToolStripMenuItem ctxTextAlignmentMiddleCenter;
        private System.Windows.Forms.ToolStripMenuItem ctxTextAlignmentMiddleRight;
        private System.Windows.Forms.ToolStripMenuItem ctxTextAlignmentBottomLeft;
        private System.Windows.Forms.ToolStripMenuItem ctxTextAlignmentBottomCenter;
        private System.Windows.Forms.ToolStripMenuItem ctxTextAlignmentBottomRight;
    }
}