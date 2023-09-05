namespace InjectorSharp
{
    partial class InjectorSharpForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            RunAndInjectButton = new Button();
            ApplicationButton = new Button();
            DllButton = new Button();
            ProcessPathTextBox = new TextBox();
            DllPathTextBox = new TextBox();
            ParametersTextBox = new TextBox();
            ApplicationLabel = new Label();
            DllLabel = new Label();
            ParametersLabel = new Label();
            ProcessInjectButton = new Button();
            SuspendLayout();
            // 
            // RunAndInjectButton
            // 
            RunAndInjectButton.Location = new Point(297, 204);
            RunAndInjectButton.Name = "RunAndInjectButton";
            RunAndInjectButton.Size = new Size(155, 40);
            RunAndInjectButton.TabIndex = 3;
            RunAndInjectButton.Text = "Run && Inject";
            RunAndInjectButton.UseVisualStyleBackColor = true;
            RunAndInjectButton.Click += RunAndInjectButton_Click;
            // 
            // ApplicationButton
            // 
            ApplicationButton.Location = new Point(753, 27);
            ApplicationButton.Name = "ApplicationButton";
            ApplicationButton.Size = new Size(131, 40);
            ApplicationButton.TabIndex = 0;
            ApplicationButton.Text = "...";
            ApplicationButton.UseVisualStyleBackColor = true;
            ApplicationButton.Click += ProcessButton_Click;
            // 
            // DllButton
            // 
            DllButton.Location = new Point(753, 89);
            DllButton.Name = "DllButton";
            DllButton.Size = new Size(131, 40);
            DllButton.TabIndex = 1;
            DllButton.Text = "...";
            DllButton.UseVisualStyleBackColor = true;
            DllButton.Click += DllButton_Click;
            // 
            // ProcessPathTextBox
            // 
            ProcessPathTextBox.Location = new Point(149, 27);
            ProcessPathTextBox.Name = "ProcessPathTextBox";
            ProcessPathTextBox.ReadOnly = true;
            ProcessPathTextBox.Size = new Size(598, 35);
            ProcessPathTextBox.TabIndex = 0;
            ProcessPathTextBox.TabStop = false;
            // 
            // DllPathTextBox
            // 
            DllPathTextBox.Location = new Point(149, 89);
            DllPathTextBox.Name = "DllPathTextBox";
            DllPathTextBox.ReadOnly = true;
            DllPathTextBox.Size = new Size(598, 35);
            DllPathTextBox.TabIndex = 0;
            DllPathTextBox.TabStop = false;
            // 
            // ParametersTextBox
            // 
            ParametersTextBox.Location = new Point(149, 150);
            ParametersTextBox.Name = "ParametersTextBox";
            ParametersTextBox.Size = new Size(598, 35);
            ParametersTextBox.TabIndex = 2;
            // 
            // ApplicationLabel
            // 
            ApplicationLabel.AutoSize = true;
            ApplicationLabel.Location = new Point(25, 27);
            ApplicationLabel.Name = "ApplicationLabel";
            ApplicationLabel.Size = new Size(121, 30);
            ApplicationLabel.TabIndex = 5;
            ApplicationLabel.Text = "Application";
            // 
            // DllLabel
            // 
            DllLabel.AutoSize = true;
            DllLabel.Location = new Point(25, 89);
            DllLabel.Name = "DllLabel";
            DllLabel.Size = new Size(105, 30);
            DllLabel.TabIndex = 6;
            DllLabel.Text = "Input DLL";
            // 
            // ParametersLabel
            // 
            ParametersLabel.AutoSize = true;
            ParametersLabel.Location = new Point(25, 150);
            ParametersLabel.Name = "ParametersLabel";
            ParametersLabel.Size = new Size(117, 30);
            ParametersLabel.TabIndex = 7;
            ParametersLabel.Text = "Parameters";
            // 
            // ProcessInjectButton
            // 
            ProcessInjectButton.Location = new Point(458, 204);
            ProcessInjectButton.Name = "ProcessInjectButton";
            ProcessInjectButton.Size = new Size(171, 40);
            ProcessInjectButton.TabIndex = 4;
            ProcessInjectButton.Text = "Process Inject";
            ProcessInjectButton.UseVisualStyleBackColor = true;
            ProcessInjectButton.Click += ProcessInjectButton_Click;
            // 
            // InjectorSharpForm
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(913, 258);
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Controls.Add(ParametersLabel);
            Controls.Add(DllLabel);
            Controls.Add(ApplicationLabel);
            Controls.Add(ParametersTextBox);
            Controls.Add(DllPathTextBox);
            Controls.Add(ProcessPathTextBox);
            Controls.Add(DllButton);
            Controls.Add(ApplicationButton);
            Controls.Add(ProcessInjectButton);
            Controls.Add(RunAndInjectButton);
            Name = "InjectorSharpForm";
            Text = "InjectorSharp";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label ApplicationLabel;
        private Label DllLabel;
        private Label ParametersLabel;
        private TextBox ProcessPathTextBox;
        private TextBox DllPathTextBox;
        private TextBox ParametersTextBox;
        private Button ApplicationButton;
        private Button DllButton;
        private Button RunAndInjectButton;
        private Button ProcessInjectButton;
    }
}