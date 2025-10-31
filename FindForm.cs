using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace текстовый_редактор1
{
    public partial class FindForm : Form

    {
        public string FindText { get; private set; }
        Form1 form1;
        private List<int> searchHighlightPositions = new List<int>();

        public FindForm(Form1 owner)
        {
            InitializeComponent();
            form1 = owner;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
         
        }

        //найти далее
        private void button2_Click(object sender, EventArgs e)
        {
            FindText = textBox1.Text;            
            if (!string.IsNullOrEmpty(FindText))
            {
                int cursorIndex = form1.richTextBox1.SelectionStart;

                // Находим следующее вхождение искомого текста, начиная с текущей позиции курсора.
                int index = form1.richTextBox1.Find(FindText, cursorIndex, form1.richTextBox1.TextLength, RichTextBoxFinds.None);

                if (index >= 0 && index < form1.richTextBox1.TextLength)
                {
                    form1.richTextBox1.Select(index, FindText.Length);
                    form1.richTextBox1.SelectionBackColor = Color.Yellow; // Подсветка найденного текста
                    searchHighlightPositions.Add(index);
                    form1.richTextBox1.SelectionStart = index + FindText.Length;
                    form1.richTextBox1.SelectionLength = 0;
                }
                else { MessageBox.Show("Текст не найден.");}
            }
            else
            { // Выводим сообщение если ввод пуст.
                MessageBox.Show("Пожалуйста, введите действительный текст для поиска.");
            }
        }
        //найти все
        private void button3_Click(object sender, EventArgs e)
        {
            FindText = textBox1.Text;
           
            if (!string.IsNullOrEmpty(FindText))
            {
                int index = 0;
                bool textFound = false;

                while (index < form1.richTextBox1.TextLength)
                {
                    // Находим следующее вхождение искомого текста, начиная с текущей позиции.
                    index = form1.richTextBox1.Find(FindText, index, form1.richTextBox1.TextLength, RichTextBoxFinds.None);

                    // Если текст найден, выделяем его и прокручиваем к нему.
                    if (index >= 0)
                    {
                        form1.richTextBox1.Select(index, FindText.Length);
                     
                        form1.richTextBox1.SelectionBackColor = Color.Yellow; // Подсветка найденного текста
                        searchHighlightPositions.Add(index);
                        index += FindText.Length;
                        textFound = true;
                    }
                    else break;

                }
                if (!textFound) MessageBox.Show("Текст не найден.");
            }
            else
            {
                // Выводим сообщение если ввод пуст.
                MessageBox.Show("Пожалуйста, введите действительный текст для поиска.");
            }

            
        }
        private void ClearSearchHighlights()
        {
            foreach (int position in searchHighlightPositions)
            {
                form1.richTextBox1.Select(position, FindText.Length);
                form1.richTextBox1.SelectionBackColor = Color.White;
            }
        }

        private void FindForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ClearSearchHighlights();
        }

        private void FindForm_Load(object sender, EventArgs e)
        {

        }
    }
}

