![main branch status](https://github.com/bigtlb/WinFormMarkup/actions/workflows/dotnet.yml/badge.svg) 
![Nuget Link](https://img.shields.io/nuget/v/WinFormMarkup?logo=nuget)  <!-- https://shields.io/category/version -->

[The latest Nuget package is available here.](https://www.nuget.org/packages/WinFormMarkup/)

# WinFormMarkup

A more concise, flexible, and extensible syntax for working with .Net Core Windows Forms.

Inspired by [TornadoFX][1] and [Xamarin Community Toolkit C# Markup][2].  WinFormMarkup is a set of extension methods that make it easier to create Windows Forms in a cleaner, more declarative manner.

Don't be dependent on an IDE designer.  Make cleaner forms.  Not every UI control needs a member variable.

## Problem
In designer initialization, every control is created and assigned a member variable, then styled and applied to the parent.  Although this makes it easier for tooling to parse the code, it is incredible redundant and harder to read through.  The generated block must be maintained by the designer otherwise you run the risk of breaking the parsing logic when a designer tries to load it.  This makes projects harder to work on in editors that don't support a designer (e.g., VS Code).

If you hand code Winforms Form and Control classes by hand you either end up relying on the same laborious manner of initialization that the designer uses, or you end up using C# constructor initialization.  But Contructor initialization doesn't give you the option of assigning arbitrary elements to variables (you would have to do that in advance).  There are other optimizations that are more convenient when implementaed as an extension method.  Also using a fluent syntax like this does not EXCLUDE the use of initializer wherever desired.

### With WinForms Designer
Designer is verbose, and should only be maintained in a designer.  <code>#DoesNotSparkJoy</code>
```csharp
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._newFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._openFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {this._newFileMenuItem, this._openFileMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // _newFileMenuItem
            // 
            this._newFileMenuItem.Name = "_newFileMenuItem";
            this._newFileMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys) ((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this._newFileMenuItem.Size = new System.Drawing.Size(152, 22);
            this._newFileMenuItem.Text = "&New";
            // 
            // _openFileMenuItem
            // 
            this._openFileMenuItem.Name = "_openFileMenuItem";
            this._openFileMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys) ((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this._openFileMenuItem.Size = new System.Drawing.Size(152, 22);
            this._openFileMenuItem.Text = "&Open";
            // 
            // Test
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Test";
            this.Text = "Test";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.ToolStripMenuItem _openFileMenuItem;

        private System.Windows.Forms.ToolStripMenuItem _newFileMenuItem;

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;

        #endregion
```

### With C# Initialzers
Occasional need for variable references means having to break apart the initialization flow, wherever assignment is needed, potentially at multiple levels

```csharp
// earlier in the initialization process
 _newFileMenuItem = new ToolStripMenuItem("&New") {ShortcutKeys = Keys.Control | Keys.N};
 _openFileMenuItem = new ToolStripMenuItem("&Open") {ShortcutKeys = Keys.Control | Keys.O};
 
 ...

this.Controls.AddRange(new [] {
    new MenuStrip
    {
        Items = {
            new ToolStripMenuItem("&File")
            {
                  DropDownItems = { _newFileMenuItem, _openFileMenuItem }
            }
        }
    }
});
```

### With WinFormsMarkup

Everything can be declared in a single flow.  Binding helpers (see samples) simplify field and event binding as well.

```csharp
this.Controls(
    new MenuStrip().Items(
        new ToolStripMenuItem("&File").DropDownItems(
            _newFileMenuItem = new ToolStripMenuItem("&New").Keys(Keys.Control | Keys.N),
            _openFileMenuItem = new ToolStripMenuItem("&Open").Keys(Keys.Control | Keys.O)
        )));
```

**NOTE:**  Or mix and match either of the two formats.

[1]: https://github.com/edvin/tornadofx
[2]: https://docs.microsoft.com/en-us/xamarin/community-toolkit/markup
