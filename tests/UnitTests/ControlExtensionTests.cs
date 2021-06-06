using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using WinFormMarkup.Extensions;
using Xunit;

namespace UnitTests
{
    public class ControlExtensionTests
    {
        [Fact]
        private void CanSet_Anchor()
        {
            var ctl = new Control();
            ctl.Anchor = AnchorStyles.None;

            ctl.Anchor(AnchorStyles.Top | AnchorStyles.Left);

            Assert.Equal(AnchorStyles.Top | AnchorStyles.Left, ctl.Anchor);
        }

        [Fact]
        private void CanSet_AutoSize()
        {
            var ctl = new Control();
            ctl.AutoSize = false;

            ctl.AutoSize(true);

            Assert.Equal(true, ctl.AutoSize);
        }

        [Fact]
        private void CanSet_BackColor()
        {
            var ctl = new Control();
            ctl.BackColor = Color.Black;

            ctl.BackColor(Color.White);

            Assert.Equal(Color.White, ctl.BackColor);
        }

        [Fact]
        private void CanSet_BoundsFull()
        {
            var ctl = new Control();
            ctl.Bounds = new Rectangle(10, 20, 30, 40);

            ctl.Bounds(11, 22, 33, 44);

            Assert.StrictEqual(new Rectangle(11, 22, 33, 44), ctl.Bounds);
        }

        [Fact]
        private void CanSet_BoundsSize()
        {
            var ctl = new Control();
            ctl.Bounds = new Rectangle(10, 20, 0, 0);

            ctl.Bounds(30, 40);

            Assert.StrictEqual(new Rectangle(10, 20, 30, 40), ctl.Bounds);
        }

        [Fact]
        private void CanSet_Clicked()
        {
            var ctl = new ClickControl();
            var callCount = 0;
            ctl.Clicked(c => callCount++);

            using (new ControlTester(ctl))
            {
                ctl.PerformClick(EventArgs.Empty);
            }

            Assert.Equal(1, callCount);
        }


        [Fact]
        private void CanSet_Controls()
        {
            var ctl = new Control();
            ctl.Controls(
                new MenuStrip(),
                new Panel());

            Assert.Equal(2, ctl.Controls.Count);
            Assert.Contains(ctl.Controls
                .Cast<Control>(), c => c is MenuStrip);
        }

        [Fact]
        private void CanSet_ControlsAndBringToFrontDocked()
        {
            var ctl = new Control();
            ctl.Controls(
                new MenuStrip(),
                new Panel().Dock(DockStyle.Fill));

            using (new ControlTester(ctl))
            {
                Assert.Equal(2, ctl.Controls.Count);
                Assert.Contains(ctl.Controls
                    .Cast<Control>(), c => c is Panel && c.Top > 0);
            }
        }

        [Fact]
        private void CanSet_DefaultTextBinding()
        {
            var source = new INPCSource();
            var target = new Control();
            target.Binding(source, s => s.SourceString);
            target.Text = "Initial Value";
            source.SourceString = "It Works!";

            using (new ControlTester(target))
            {
                Assert.Equal("It Works!", target.Text);
            }
        }

        [Fact]
        private void CanSet_GeneralBinding()
        {
            var source = new INPCSource();
            var target = new Control();
            target.Binding(source, s => s.SourceString, t => t.Text);
            target.Text = "Initial Value";
            source.SourceString = "It Works!";

            using (new ControlTester(target))
            {
                Assert.Equal("It Works!", target.Text);
            }
        }
        
        [Fact]
        private void CanSet_GeneralBinding_WithConvert()
        {
            var source = new INPCSource();
            var target = new Control();
            target.Binding(source, s => s.SourceNumber, t => t.Text, 
                n => $"This is {n}");
            target.Text = "Initial Value";
            source.SourceNumber = 5;

            using (new ControlTester(target))
            {
                Assert.Equal("This is 5", target.Text);
            }
        }
        
        
        [Fact]
        private void CanSet_GeneralBinding_WithConvertBack()
        {
            var source = new INPCSource();
            var target = new Control();
            target.Binding(source, 
                s => s.SourceNumber, t => t.Text, 
                n => $"This is {n}", s=>int.Parse(s[^1].ToString()));
            target.Text = "Initial Value";
            source.SourceNumber = 5;

            using (new ControlTester(target))
            {
                Assert.Equal("This is 5", target.Text);
                target.Text = "Setting to 6";
                Assert.Equal(6, source.SourceNumber);
            }
        }        
        
        [Fact]
        private void CanSet_Dock()
        {
            var ctl = new Control();
            ctl.Dock = DockStyle.None;

            ctl.Dock(DockStyle.Top | DockStyle.Left);

            Assert.Equal(DockStyle.Top | DockStyle.Left, ctl.Dock);
        }

        
        [Fact]
        private void CanSet_MarginAll()
        {
            var ctl = new Control();
            ctl.Margin = new Padding(10, 20, 30, 40);

            ctl.Margin(5);

            Assert.StrictEqual(new Padding(5, 5, 5, 5), ctl.Margin);
        }
        
        [Fact]
        private void CanSet_MarginHorizontalVertical()
        {
            var ctl = new Control();
            ctl.Margin = new Padding(10, 20, 30, 40);

            ctl.Margin(5,6);

            Assert.StrictEqual(new Padding(5, 6, 5, 6), ctl.Margin);
        }
        
        [Fact]
        private void CanSet_MarginLeftTopRightBottom()
        {
            var ctl = new Control();
            ctl.Margin = new Padding(10, 20, 30, 40);

            ctl.Margin(1,2,3,4);

            Assert.StrictEqual(new Padding(1,2,3,4), ctl.Margin);
        }
        
        
        [Fact]
        private void CanSet_PaddingAll()
        {
            var ctl = new Control();
            ctl.Padding = new Padding(10, 20, 30, 40);

            ctl.Padding(5);

            Assert.StrictEqual(new Padding(5, 5, 5, 5), ctl.Padding);
        }
        
        [Fact]
        private void CanSet_PaddingHorizontalVertical()
        {
            var ctl = new Control();
            ctl.Padding = new Padding(10, 20, 30, 40);

            ctl.Padding(5,6);

            Assert.StrictEqual(new Padding(5, 6, 5, 6), ctl.Padding);
        }
        
        [Fact]
        private void CanSet_PaddingLeftTopRightBottom()
        {
            var ctl = new Control();
            ctl.Padding = new Padding(10, 20, 30, 40);

            ctl.Padding(1,2,3,4);

            Assert.StrictEqual(new Padding(1,2,3,4), ctl.Padding);
        }
        
        [Fact]
        private void CanSet_Position()
        {
            var ctl = new Control();
            ctl.Left = 2;
            ctl.Top = 3;

            ctl.Position(10, 20);

            Assert.Equal(10, ctl.Left);
            Assert.Equal(20, ctl.Top);
        }
        
        [Fact]
        private void CanSet_Text()
        {
            var ctl = new Control();

            ctl.Text("Hello");

            Assert.Equal("Hello", ctl.Text);
        }
        
        [Fact]
        private void CanBringToFront_AfterParented()
        {
            using (var form = new Form())
            {
                Panel _yellow;
                form.Controls(
                    new Panel().BackColor(Color.Blue),
                    _yellow = new Panel().BackColor(Color.Yellow),
                    new Panel().BackColor(Color.Red));
                form.Show();
                Assert.NotEqual(_yellow, form.Controls[0]);
                _yellow.ToFront();
                EventHandler doAssert = (s,e) => Assert.Equal(_yellow, form.Controls[0]);
                _yellow.BeginInvoke(doAssert);
            }
        }
        
        
        [Fact]
        private void CanBringToFront_BeforeParented()
        {
            using (var form = new Form())
            {
                Panel _yellow = new Panel().BackColor(Color.Yellow).ToFront();
                form.Controls(
                    new Panel().BackColor(Color.Blue),
                    _yellow,
                    new Panel().BackColor(Color.Red));
                form.Show();
                EventHandler doAssert = (s,e) => Assert.Equal(_yellow, form.Controls[0]);
                _yellow.BeginInvoke(doAssert);
            }
        }

        private class ClickControl : Control
        {
            public void PerformClick(EventArgs e)
            {
                base.OnClick(e);
            }
        }

        private class ControlTester : IDisposable
        {
            private readonly Form _form = new();

            public ControlTester(Control control)
            {
                _form = new Form();
                _form.Controls.Add(control);
                _form.Show();
            }

            public void Dispose()
            {
                _form.Close();
            }
        }

        private class INPCSource : INotifyPropertyChanged
        {
            private string _sourceString;
            private int _sourceNumber;

            public string SourceString
            {
                get => _sourceString;
                set
                {
                    _sourceString = value;
                    OnPropertyChanged(nameof(SourceString));
                }
            }

            public int SourceNumber
            {
                get => _sourceNumber;
                set
                {
                    _sourceNumber = value;
                    OnPropertyChanged(nameof(SourceNumber));
                }
            }

            public event PropertyChangedEventHandler? PropertyChanged;

            protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}