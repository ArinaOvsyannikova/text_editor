using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Printing;
using static System.Net.WebRequestMethods;

  
namespace текстовый_редактор1
{
    public partial class Form1 : Form
    {
        private string openFile;
        private Stack<string> undoStack = new Stack<string>();   
        

        public Form1()
        {
            InitializeComponent();
            saveFileDialog1.Filter = "Text File(*.txt)|*.txt|world (*.rtf)|*.rtf";
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(openFile))
            {
                // Если файл не открыт, открываем форму сохранения
                сохранитьКакToolStripMenuItem_Click(sender, e);
            }
            else
            {
                // Проверяем, существует ли файл
                if (System.IO.File.Exists(openFile))
                {
                    // Файл существует, обновляем существующий файл
                    try
                    {
                        if (Path.GetExtension(openFile).ToLower() == ".txt")
                        {
                            System.IO.File.WriteAllText(openFile, richTextBox1.Text);
                        }
                        else if (Path.GetExtension(openFile).ToLower() == ".rtf")
                        {
                            richTextBox1.SaveFile(openFile, RichTextBoxStreamType.RichText);
                        }
                        MessageBox.Show("Файл успешно обновлен");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при обновлении файла: " + ex.Message);
                    }
                }
                else
                {
                    // Файл не существует, открываем форму сохранения
                    сохранитьКакToolStripMenuItem_Click(sender, e);
                }
            }
            richTextBox1.Focus();
        }
     
        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Rich Text Format (*.rtf)|*.rtf|Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFile = openFileDialog.FileName;
                if (Path.GetExtension(selectedFile).ToLower() == ".rtf")
                {
                    richTextBox1.LoadFile(selectedFile, RichTextBoxStreamType.RichText);
                }
                else if (Path.GetExtension(selectedFile).ToLower() == ".txt")
                {
                    string text = System.IO.File.ReadAllText(selectedFile);
                    richTextBox1.Text = text;
                }
                openFile = selectedFile;
                undoStack.Clear();
            }
            richTextBox1.Focus();
        }        

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filename = saveFileDialog1.FileName;
                if (Path.GetExtension(filename).ToLower() == ".txt")
                {
                    System.IO.File.WriteAllText(filename, richTextBox1.Text);
                }
                else if (Path.GetExtension(filename).ToLower() == ".rtf")
                {
                    richTextBox1.SaveFile(filename, RichTextBoxStreamType.RichText);
                }
                MessageBox.Show("Saved successfully");
            }
            richTextBox1.Focus();
        }

        private void создатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Хотите сохранить изменения в файле?", "Предупреждение", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                SaveFileDialog Sdialog = new SaveFileDialog();
                Sdialog.Filter = "Text File(*.txt)|*.txt|world (*.rtf)|*.rtf";
                if (Sdialog.ShowDialog() == DialogResult.OK)
                {
                    System.IO.File.WriteAllText(Sdialog.FileName, richTextBox1.Text);
                    openFile = Sdialog.FileName;
                    richTextBox1.Text = "";
                    openFile = "";
                }
              

            }
            else if (result == DialogResult.No)
            {
                richTextBox1.Text = "";
                openFile = "";
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
            richTextBox1.Focus();

        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void копироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text.Length > 0) richTextBox1.Copy();
            richTextBox1.Focus();

        }

        private void вставитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            richTextBox1.Paste();
            richTextBox1.Focus();
        }

        private void вырезатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text.Length > 0) richTextBox1.Cut();
            richTextBox1.Focus();
        }

        private void выделитьВсеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text.Length > 0) richTextBox1.SelectAll();
        }

        private void отменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
           richTextBox1.Undo();
            richTextBox1.Focus();

        }
   
        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectedText = "";
            richTextBox1.Focus();
        }

        private void найтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FindForm(this).Show();        
        }
       
        private void richTextBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) richTextBox1.ContextMenuStrip = contextMenuStrip1;

        }

        private void заменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {

            new ReplaseForm(this).Show();
        }

        private void перейтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int lineNumber;
            string input = Microsoft.VisualBasic.Interaction.InputBox("Введите номер строки:", "Перейти к строке");

            if (int.TryParse(input, out lineNumber))
            {

                if (lineNumber > 0)
                {
                    int lineIndex = richTextBox1.GetFirstCharIndexFromLine(lineNumber - 1);
                    if (lineIndex >= 0)
                    {
                        richTextBox1.SelectionStart = lineIndex;
                        richTextBox1.ScrollToCaret();
                    }
                    else
                    {
                        MessageBox.Show("Указанная строка не существует.");
                    }
                }
                else { MessageBox.Show("Пожалуйста, введите действительный номер строки."); }
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите действительный номер строки.");
            }
            richTextBox1.Focus();
        }
        //кнопка шрифт
        private void button1_Click(object sender, EventArgs e)
        {
            FontDialog myFont = new FontDialog();
            if (myFont.ShowDialog() == DialogResult.OK)
            { richTextBox1.SelectionFont = myFont.Font; 
            
            }
            richTextBox1.Focus();
        }
        //кнопка цвет
        private void button2_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.SelectionColor = colorDialog.Color;
            }
            richTextBox1.Focus();
        }
        //кнопка выделение
        private void button3_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.SelectionBackColor = colorDialog.Color;
            }
            richTextBox1.Focus();
        }
        //кнопка фон 
        private void button4_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.BackColor = colorDialog.Color;
            }
            richTextBox1.Focus();
        }

        private void переносПоСловамToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!переносПоСловамToolStripMenuItem.Checked)
            {
                richTextBox1.WordWrap = true;
                переносПоСловамToolStripMenuItem.Checked = true;
              
            }
            else
            {
                richTextBox1.WordWrap = false;
                переносПоСловамToolStripMenuItem.Checked = false;
            }
            richTextBox1.Focus();
        }

        private void richTextBox1_SelectionChanged(object sender, EventArgs e)
        {
            if (richTextBox1.SelectedText == "")
            {
                копироватьToolStripMenuItem.Enabled = false;
                копироватьToolStripMenuItem1.Enabled = false;
                вырезатьToolStripMenuItem.Enabled = false;
                вырезатьToolStripMenuItem1.Enabled = false;
               
               
            }
            else
            {
                копироватьToolStripMenuItem.Enabled = true;
                копироватьToolStripMenuItem1.Enabled = true;
                вырезатьToolStripMenuItem.Enabled = true;
                вырезатьToolStripMenuItem1.Enabled = true;
               
            }
            if (Clipboard.ContainsText() == true)
            {
                вставитьToolStripMenuItem.Enabled = true;
                вставитьToolStripMenuItem1.Enabled = true;

            }
            else
            {
                вставитьToolStripMenuItem.Enabled = false;
                вставитьToolStripMenuItem1.Enabled = false;
            }
            if (richTextBox1.Text == null)
            {
                удалитьToolStripMenuItem.Enabled = false;
                выделитьВсеToolStripMenuItem.Enabled = false;
                отменитьToolStripMenuItem.Enabled = false;
            }
            else
            {
                удалитьToolStripMenuItem.Enabled = true;
                отменитьToolStripMenuItem.Enabled = true;
                выделитьВсеToolStripMenuItem.Enabled = true;
            }
        }
                 
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = $"Количество строк {richTextBox1.Lines.Length}";
            Font activeFont = richTextBox1.SelectionFont;
            toolStripStatusLabel2.Text = "Название шрифта: " + activeFont.Name;
        }
    }
}