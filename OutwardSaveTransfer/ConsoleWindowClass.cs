using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace OutwardSaveTransfer
{
    public class ConsoleWindowClass
    {
        private RichTextBox consoleWindow;
        private Color successColor = Color.Green;
        private Color errorColor = Color.Red;
        private Color normalColor = Color.Black;

        delegate void SetTextCallback(string text);

        public ConsoleWindowClass(RichTextBox consoleWindowBox)
        {
            this.consoleWindow = consoleWindowBox;
        }

        public async void Print_text(string text)
        {
            await Task.Run(() =>
            {
                Console_print_text(text);
            });
        }

        public async void Print_text_new_Line(string text)
        {
            await Task.Run(() =>
            {
                Console_print_text_new_line(text);
            });
        }

        public async void Print_success_text(string text)
        {
            await Task.Run(() =>
            {
                Console_print_success_text(text);
            });
        }

        public async void Print_error_text(string text)
        {
            await Task.Run(() =>
            {
                Console_print_error_text(text);
            });
        }

        public void Clear_all_text()
        {
            consoleWindow.Text = "";
        }

        private void Console_print_text(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.consoleWindow.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(Console_print_text);
                consoleWindow.Invoke(d, new object[] { text });
            }
            else
            {
                consoleWindow.SelectionColor = normalColor;
                consoleWindow.SelectedText += text;

                Scroll_down();
            }
        }

        private void Console_print_text_new_line(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.consoleWindow.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(Console_print_text_new_line);
                consoleWindow.Invoke(d, new object[] { text });
            }
            else
            {
                consoleWindow.SelectionColor = normalColor;
                consoleWindow.SelectedText += "\n" + text;

                Scroll_down();
            }
        }

        private void Console_print_success_text(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.consoleWindow.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(Console_print_success_text);
                consoleWindow.Invoke(d, new object[] { text });
            }
            else
            {
                consoleWindow.SelectionColor = successColor;
                consoleWindow.SelectedText = text;

                Scroll_down();
            }
        }

        private void Console_print_error_text(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.consoleWindow.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(Console_print_error_text);
                consoleWindow.Invoke(d, new object[] { text });
            }
            else
            {
                consoleWindow.SelectionColor = errorColor;
                consoleWindow.SelectedText += text;

                Scroll_down();
            }
        }

        private void Scroll_down()
        {
            // set the current caret position to the end
            //consoleWindow.SelectionStart = consoleWindow.Text.Length;
            // scroll it automatically
            //consoleWindow.ScrollToCaret();
        }
    }
}
