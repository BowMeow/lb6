using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ЛР1
{
    public partial class Form1 : Form
    {
        private int fileCounter = 1;
        private string openedFilePath;
        public Form1()
        {
            InitializeComponent();
            resultBox.ReadOnly = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string folderPath = @"C:\Users\kaban\source\repos\ЛР1\ЛР1\bin\Debug";
            try
            {
                string fileName = $"File{fileCounter}.txt";
                string filePath = Path.Combine(folderPath, fileName);
                // Создание файла
                File.WriteAllText(filePath, $"Содержимое файла {fileCounter}");
                MessageBox.Show($"Файл {fileName} успешно создан.", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);

                fileCounter++;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (!сheckForChanges())
            {
                return;
            }
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;

                    try
                    {
                        // Считываем содержимое файла
                        string fileContent = File.ReadAllText(filePath);

                        // Устанавливаем текст в RichTextBox
                        editingTextBox.Text = fileContent;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при открытии файла: {ex.Message}", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private bool сheckForChanges()
        {
            if (editingTextBox.Modified)
            {
                DialogResult result = MessageBox.Show("Хотите сохранить изменения?", "Предупреждение", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes)
                {
                    saveChanges();
                }
                else if (result == DialogResult.Cancel)
                {
                    return false;
                }
            }
            return true;
        }
        private void saveChanges()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Текстовые файлы|*.txt|Все файлы|*.*";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                editingTextBox.SaveFile(saveFileDialog.FileName, RichTextBoxStreamType.PlainText);
                editingTextBox.Modified = false;
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(openedFilePath))
            {
                // Если файл не был открыт ранее, используйте диалог сохранения
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        openedFilePath = saveFileDialog.FileName;
                    }
                    else
                    {
                        return; // Пользователь отменил сохранение
                    }
                }
            }

            try
            {
                // Сохраняем содержимое RichTextBox в тот же файл
                File.WriteAllText(openedFilePath, editingTextBox.Text);
                MessageBox.Show("Файл успешно сохранен", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            editingTextBox.Modified = false;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsRichTextBoxModified())
            {
                checkingFileSaving();
            }
            else
            {
                Close();
            }
        }

        private bool IsRichTextBoxModified()
        {
            return editingTextBox.Modified;
        }

        private void SaveChanges()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Текстовые файлы|*.txt|Все файлы|*.*";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                editingTextBox.SaveFile(saveFileDialog.FileName, RichTextBoxStreamType.PlainText);
                editingTextBox.Modified = false;
            }
        }
        private void checkingFileSaving()
        {
            if (IsRichTextBoxModified())
            {
                DialogResult result = MessageBox.Show("Сохранить изменения?", "Предупреждение", MessageBoxButtons.YesNoCancel);

                if (result == DialogResult.Yes)
                {
                    SaveChanges();
                    Close();
                }
                else if (result == DialogResult.Cancel)
                {
                    return;
                }
                else if (result == DialogResult.No)
                {
                    Close();
                }
            }
        }

        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (editingTextBox.CanUndo)
            {
                editingTextBox.Undo();
            }
        }

        private void repeatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (editingTextBox.CanRedo)
            {
                editingTextBox.Redo();
            }
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (editingTextBox.SelectionLength > 0) 
            {
                Clipboard.SetText(editingTextBox.SelectedText);
                editingTextBox.SelectedText = "";
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (editingTextBox.SelectionLength > 0)
                Clipboard.SetText(editingTextBox.SelectedText);
        }

        private void insertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                editingTextBox.SelectedText = Clipboard.GetText();
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (editingTextBox.SelectionLength > 0)
            {
                editingTextBox.SelectedText = "";
            }
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editingTextBox.SelectAll();
        }

        private void callHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            const string filePath = @"C:\Users\kaban\source\repos\ЛР1\Help.html";
            if (System.IO.File.Exists(filePath))
            {
                Process p = new Process();
                p.StartInfo = new ProcessStartInfo(filePath) { UseShellExecute = true };
                p.Start();
            }
        }

        private void aboutProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            const string filePath = @"C:\Users\kaban\source\repos\ЛР1\Info.html";
            if (System.IO.File.Exists(filePath))
            {
                Process p = new Process();
                p.StartInfo = new ProcessStartInfo(filePath) { UseShellExecute = true };
                p.Start();
            }
        }

       private void runParserButton_Click(object sender, EventArgs e)
    {
            string inputText = editingTextBox.Text;
            //1 задание
            FindHtmlTags(inputText);
            //2 задание
            //FindNames(inputText);
            //3 задание
            //isplayHSLColors(inputText);
        }
        //1 задание
        private void FindHtmlTags(string text)
        {
           
            string htmlTagsRegex = @"<(p|li|h3)\b[^>]*>";

            
            MatchCollection htmlMatches = Regex.Matches(text, htmlTagsRegex);
            foreach (Match match in htmlMatches)
            {
                HighlightText(match.Index, match.Length);
            }
        }

        private void HighlightText(int startIndex, int length)
        {
            string foundTag = editingTextBox.Text.Substring(startIndex, length);

            resultBox.AppendText("Открывающий тег: " + foundTag + Environment.NewLine);
        }

        //2 задание
        /*
         private void FindNames(string text)
        {
            resultBox.Clear();
            string nameRegex = @"\b[А-ЯЁ][а-яё]+\s+[А-ЯЁ][а-яё]+\s+[А-ЯЁ][а-яё]+";
            MatchCollection nameMatches = Regex.Matches(text, nameRegex);
            foreach (Match match in nameMatches)
            {
                DisplayMatch(match);
            }
        }

        private void DisplayMatch(Match match)
        {
            resultBox.AppendText(match.Value + Environment.NewLine);
        }
         */
        //3 задание
        /*
         private void DisplayHSLColors(string inputText)
        {
            string pattern = @"hsl\(\s*(\d{1,3}),\s*(\d{1,3}%),\s*(\d{1,3}%)\s*\)";    
            resultBox.Clear();

            MatchCollection matches = Regex.Matches(inputText, pattern);

            foreach (Match match in matches)
            {
                resultBox.AppendText(match.Value + Environment.NewLine);
            }
        }
        */
    }


}
