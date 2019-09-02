using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;

namespace ЛБ2
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// Поле, хранящее предыдущее значение текстбокса.
        /// </summary>
        string textPred = string.Empty;
        /// <summary>
        /// Поле, хранящее символ нажатой кнопки-действия.
        /// </summary>
        char ActChr;
        /// <summary>
        /// Поле, хранящее изначальное значение формы.
        /// </summary>
        ushort thWidth;
        /// <summary>
        /// Конструктор класса.
        /// </summary>
        public Form1()
        {
            InitializeComponent(); // Инициализация дочерних компонентов управления класса.
        }

        /// <summary>
        /// Метод нажания всех числовых кнопок, а также запятой.
        /// </summary>
        /// <param name="sender">Объект, вызвавший метод.</param>
        /// <param name="e">Событие</param>
        private void ButtonsClick(object sender, EventArgs e)
        {
            textBox1.Text += (Char.IsDigit(((Button)sender).Text.Last()) ? ((Button)sender).Text : (textBox1.Text.Length == 0 ? "0"
                : string.Empty) + (textBox1.Text.IndexOf(',') == -1 ? ((Button)sender).Text : string.Empty)); // Присвоение значения тексбоксу.
        }
        /// <summary>
        /// Метод, анимирующий движение формы.
        /// </summary>
        /// <param name="v">Значение одного сдвига.</param>
        /// <param name="sender">Объект, вызвавший метод.</param>
        private void AnimateThis(int v, object sender)
        {
            this.Width += v; // Инкрементирование ширины формы.
            textBox1.Width += v; // Инкрементирование ширины тексбокса.
            this.Update(); // Перерисовка формы.
            Thread.Sleep(5); // Асинхронное ожидание.
            if (!(this.Width <= ((Button)sender).Location.X + ((Button)sender).Width + 20 || this.Width >= thWidth)) AnimateThis(v, sender); // Рекурсивный вызов анимирующего метода.
            else textBox1.Width = v > 0 ? textBox1.Width : ((Button)sender).Location.X + ((Button)sender).Width - textBox1.Location.X; // корректировка ширины текстбокса.
        }
        /// <summary>
        /// Метод нажания по кнопке анимирования.
        /// </summary>
        /// <param name="sender">Объект, вызвавший метод.</param>
        /// <param name="e">Событие.</param>
        private void buttonChange_Click(object sender, EventArgs e)
        {
            AnimateThis(((Button)sender).Text == "<" ? -1 : 1, sender); // Вызов метода анимировния.
            ((Button)sender).Text = ((Button)sender).Text == "<" ? ">" : "<"; // Изменение направления стрелочки!

        }
        /// <summary>
        /// Метод нажания по кнопкам действия.
        /// </summary>
        /// <param name="sender">Объект, вызвавший метод.</param>
        /// <param name="e">Событие.</param>
        private void ButtonsAction(object sender, EventArgs e)
        {
            textPred = textBox1.Text; // Сохранение поля текстбокса в предназначенное для это поле.
            textBox1.Text = string.Empty; // Обнуление поля текстбокса.
            ActChr = ((Button)sender).Text.Last(); // Сохранение символа нажатой кнопки.
            butPer.BackColor = ButPlus.BackColor = ButMulti.BackColor = ButMinus.BackColor = ButDivision.BackColor = SystemColors.Control; // Сбос цвета кнопок-действия.
            ((Button)sender).BackColor = Color.Blue; // Выделение нажатой кнопки.
        }
        /// <summary>
        /// Метод выполнения вычислительных действий.
        /// </summary>
        /// <param name="textPred">Поле с предыдущем значением поля текстбокса.</param>
        /// <param name="textBox1">Поле значения текстбокса.</param>
        /// <param name="text">Символ нажатой кнопки.</param>
        /// <returns></returns>
        private string ExecuteAction(string textPred, string textBox1, char text)
        {
            switch (text) // Обработка действий в соответсвии с символом нажатой кнопки.
            {
                case '+': return (Convert.ToDouble(textPred) + Convert.ToDouble(textBox1)).ToString();
                case '-': return (Convert.ToDouble(textPred) - Convert.ToDouble(textBox1)).ToString();
                case '*': return (Convert.ToDouble(textPred) * Convert.ToDouble(textBox1)).ToString();
                case '/': return (Convert.ToDouble(textPred) / Convert.ToDouble(textBox1)).ToString();
                case '%': return (Convert.ToDouble(textPred) % Convert.ToDouble(textBox1)).ToString();
                case '^': return Math.Pow(Convert.ToDouble(textPred), Convert.ToDouble(textBox1)).ToString();
                default: return null;
            }
        }
        /// <summary>
        /// Метод результирующий значение.
        /// </summary>
        /// <param name="sender">Объект, вызвавший метод.</param>
        /// <param name="e">Событие.</param>
        private void ButtonsRes(object sender, EventArgs e)
        {
            switch (((Button)sender).Text) // Проверка значения нажатой кнопки.
            {
                case "C": if (textBox1.Text.Length > 0) textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1); break; // Удаление последнего символа поля текстбокса.
                case "=":
                    if (textBox1.Text == string.Empty || textPred == string.Empty) return; // При хотя бы одном пустом поле, метод завершается.
                    textBox1.Text = ExecuteAction(textPred, textBox1.Text, ActChr); textPred = string.Empty;
                    butPer.BackColor = ButPlus.BackColor = ButMulti.BackColor = ButMinus.BackColor = ButDivision.BackColor = SystemColors.Control;
                    break; // результат.
            }
        }
        /// <summary>
        /// Загрузка формы.
        /// </summary>
        /// <param name="sender">Объект, вызвавший метод.</param>
        /// <param name="e">Событие.</param>
        private void Form1_Load(object sender, EventArgs e)
        {
            thWidth = (ushort)this.Width; // Сохранение исходной ширины формы в поле.
            buttonChange_Click(buttonChange, e); // Вызов метода сдвига формы.
            textBox1_TextChanged(sender, e); // Вызов метода изменения значения поля текстбокса.
        }
        /// <summary>
        /// Метод, привязанный к действию изменения значения поля текстбокса.
        /// </summary>
        /// <param name="sender">Объект, вызвавший метод.</param>
        /// <param name="e">Событие.</param>
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.IndexOf('∞') != -1) // Действие при появления бесконечности!!! ᕦ(ˇò_ó)ᕤ
            {
                MessageBox.Show("Действия привели к бесконечности, мои поздравления! ᕦ(ò_óˇ)ᕤ "); // Вывод сообщения.
                textBox1.Text = textBox1.Text.Remove(textBox1.Text.IndexOf('∞'), 1); // Удаление бесконечности!
            }
            ButDot.Enabled = textBox1.Text.IndexOf(',') == -1; // Изменение статуса кнопки-точки.
            ButC.Enabled = textBox1.Text.Length > 0; // Изменение статуса кнопки-удаления.
            butRaw.Enabled = textPred != string.Empty && textBox1.Text.Length > 0 && textBox1.Text.Last() != ',' && textBox1.Text.Last() != '-';
            butPer.Enabled = ButPlus.Enabled = ButMinus.Enabled = ButMulti.Enabled = ButDivision.Enabled = textBox1.Text.Length > 0 && textBox1.Text.Last() != ',' && textBox1.Text.Last() != '-';
            for (byte i = 19; i <= 30; i++) (this.Controls.Find(String.Format("Button{0}", i), true)[0] as Button).Enabled = ButPlus.Enabled;
        }
        /// <summary>
        /// Метод, привязанный на нажатие кнопки удаления!
        /// </summary>
        /// <param name="sender">Объект, вызвавший метод.</param>
        /// <param name="e">Событие.</param>
        private void butNeg_Click(object sender, EventArgs e)
        {
            try
            {
                textBox1.Text = (-Convert.ToDouble(textBox1.Text)).ToString();
            }
            catch { textBox1.Text = textBox1.Text.IndexOf('-') == -1 ? textBox1.Text.Insert(0, "-") : textBox1.Text.Remove(textBox1.Text.IndexOf('-'), 1); }
        }
        /// <summary>
        /// Метод нажатия кнопки на форме.
        /// </summary>
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar)) textBox1.Text += e.KeyChar; // Вывод нажатой цифры.
            switch (e.KeyChar) // Собственная обработка нажатий, мы независимы! 
            {
                case '_': butNeg_Click(sender, e); break;
                case (char)8: ButtonsRes(ButC, e); break;
                case '=': ButtonsRes(butRaw, e); break;
                case ',': ButtonsClick(ButDot, e); break;
                case '-': ButtonsAction(ButMinus, e); break;
                case '+': ButtonsAction(ButPlus, e); break;
                case '/': ButtonsAction(ButDivision, e); break;
                case '%': ButtonsAction(butPer, e); break;
                case '*': ButtonsAction(ButMulti, e); break;
            }
            e.Handled = false; // Отказ обработки нажатой кнопки!
        }
        /// <summary>
        /// Метод обработки кнопок математической активности в перспективе. 
        /// </summary>
        private void MathButtons(object sender, EventArgs e)
        {
            switch (((Button)sender).Text)
            {
                case "sin(x)": textBox1.Text = Math.Sin(Convert.ToDouble(textBox1.Text)).ToString(); break;
                case "cos(x)": textBox1.Text = Math.Cos(Convert.ToDouble(textBox1.Text)).ToString(); break;
                case "tg(x)": textBox1.Text = Math.Tan(Convert.ToDouble(textBox1.Text)).ToString(); break;
                case "ctg(x)": textBox1.Text = (1 / Math.Tan(Convert.ToDouble(textBox1.Text))).ToString(); break;
                case "|x|": textBox1.Text = Math.Abs(Convert.ToDouble(textBox1.Text)).ToString(); break;
                case "ln(x)": textBox1.Text = Math.Log(Convert.ToDouble(textBox1.Text)).ToString(); break;
                case "exp(x)": textBox1.Text = Math.Exp(Convert.ToDouble(textBox1.Text)).ToString(); break;
                case "sqrt(x)": textBox1.Text = Math.Sqrt(Convert.ToDouble(textBox1.Text)).ToString(); break;
                case "x^2": textBox1.Text = Math.Pow(Convert.ToDouble(textBox1.Text), 2).ToString(); break;
                case "1/x": textBox1.Text = (1 / Convert.ToDouble(textBox1.Text)).ToString(); break;
                case "n!": textBox1.Text = Math.Abs(Convert.ToDouble(textBox1.Text)) >= 100 ? "0" : Factorial(Convert.ToUInt64(textBox1.Text)).ToString(); break;
                case "x^y": textPred = textBox1.Text; textBox1.Text = string.Empty; ActChr = '^'; break;
            }
        }
        /// <summary>
        /// Факториал!!
        /// </summary>
        /// <param name="n">Введенное значение!</param>
        /// <returns>Факториал введенного значения!</returns>
        static ulong Factorial(ulong n)
        {
            return (n <= 0 ? 1 : n * Factorial(--n));
        }

    }
}
