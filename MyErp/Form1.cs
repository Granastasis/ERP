using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyErp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void CalendarMenuItem_Click(object sender, EventArgs e)
        {
            // Καθαρισμός του panel1 (αν περιέχει ήδη κάτι)

            // Δημιουργία του MonthCalendar
            MonthCalendar calendar = new MonthCalendar();

            // Ορισμός της τρέχουσας ημερομηνίας
            calendar.TodayDate = DateTime.Today;
            calendar.SelectionStart = DateTime.Today;
            calendar.SelectionEnd = DateTime.Today;

            // Προσθήκη του calendar στο panel1
            
        }

        private void CalculatorMenuItem_Click(object sender, EventArgs e)
        {
            // Καθαρισμός του panel1 (αν περιέχει ήδη κάτι)

            // Δημιουργία της αριθμομηχανής
            ShowCalculator();
        }
        private void ShowCalculator()
        {
            // Δημιουργία ενός TextBox για εμφάνιση των αποτελεσμάτων
            TextBox resultBox = new TextBox
            {
                ReadOnly = true,
                Font = new Font("Arial", 20),
                Dock = DockStyle.Top,
                TextAlign = HorizontalAlignment.Right,
                Height = 50
            };

            // Προσθήκη του TextBox στο panel1

            // Δημιουργία των κουμπιών της αριθμομηχανής
            string[,] buttons = {
        { "7", "8", "9", "/" },
        { "4", "5", "6", "*" },
        { "1", "2", "3", "-" },
        { "0", "C", "=", "+" },
        { "√", "^", "(", ")" }
    };

            int buttonWidth = 60, buttonHeight = 60;  // Μέγεθος κουμπιών
            for (int i = 0; i < buttons.GetLength(0); i++)
            {
                for (int j = 0; j < buttons.GetLength(1); j++)
                {
                    Button button = new Button
                    {
                        Text = buttons[i, j],
                        Width = buttonWidth,
                        Height = buttonHeight,
                        Left = j * buttonWidth,
                        Top = i * buttonHeight + 60  // Ύψος για το TextBox
                    };

                    // Προσθήκη event handler για κάθε κουμπί
                    button.Click += (s, args) => Button_Click(resultBox, button.Text);

                    // Προσθήκη του κουμπιού στο panel1
                }
            }
        }

        private void Button_Click(TextBox resultBox, string buttonValue)
        {
            if (buttonValue == "C")
            {
                resultBox.Text = "";  // Καθαρισμός της οθόνης
            }
            else if (buttonValue == "=")
            {
                try
                {
                    // Υπολογισμός του αποτελέσματος της έκφρασης
                    resultBox.Text = Calculate(resultBox.Text).ToString();
                }
                catch
                {
                    resultBox.Text = "Error";  // Αν υπάρχει λάθος, εμφάνιση μηνύματος λάθους
                }
            }
            else
            {
                resultBox.Text += buttonValue;  // Προσθήκη του πατημένου κουμπιού στο αποτέλεσμα
            }
        }

        private double Calculate(string expression)
        {
            // Αντικατάσταση της ρίζας και του εκθέτη
            expression = expression.Replace("√", "Math.Sqrt(");
            expression = expression.Replace("^", "Math.Pow(");

            // Προσθήκη κλεισίματος παρενθέσεων για ρίζες και εκθέτες
            expression = expression.Replace("Math.Sqrt(", "Math.Sqrt(");
            expression += new string(')', expression.Count(c => c == '(') - expression.Count(c => c == ')')); // Διόρθωση παρενθέσεων

            // Χρήση DataTable για την αξιολόγηση της έκφρασης
            var dataTable = new DataTable();
            return Convert.ToDouble(dataTable.Compute(expression, null));
        }


    }
}
