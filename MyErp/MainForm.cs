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
    public partial class MainForm : Form
    {
        private Calculator calculator; // Δημιουργία μεταβλητής τύπου Calculator
        public MainForm()
        {
            InitializeComponent();
            calculator = new Calculator();
            tabControl1.DrawMode = TabDrawMode.OwnerDrawFixed; // Ενεργοποίηση custom drawing
            tabControl1.DrawItem += tabControl1_DrawItem; // Σύνδεση του event
            tabControl1.MouseDown += tabControl1_MouseDown; // Σύνδεση του mouse down event
        }

        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            TabPage tabPage = tabControl1.TabPages[e.Index];
            e.DrawBackground();

            // Σχεδίαση του ονόματος του tab
            e.Graphics.DrawString(tabPage.Text, e.Font, Brushes.Black, e.Bounds);

            // Σχεδίαση του κουμπιού κλεισίματος (Χ)
            Rectangle closeButtonRect = new Rectangle(e.Bounds.Right - 20, e.Bounds.Top, 20, e.Bounds.Height);
            e.Graphics.FillRectangle(Brushes.Red, closeButtonRect); // Φόντο του κουμπιού
            e.Graphics.DrawString("X", e.Font, Brushes.White, closeButtonRect); // Χρώμα του κειμένου

            e.DrawFocusRectangle();
        }

        private void tabControl1_MouseDown(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < tabControl1.TabCount; i++)
            {
                Rectangle tabRect = tabControl1.GetTabRect(i);
                Rectangle closeButtonRect = new Rectangle(tabRect.Right - 20, tabRect.Top, 20, tabRect.Height);

                if (closeButtonRect.Contains(e.Location))
                {
                    tabControl1.TabPages.RemoveAt(i); // Κλείσιμο του tab
                    break; // Έξοδος από τον βρόχο
                }
            }
        }

        private void CalculatorMenuItem_Click(object sender, EventArgs e)
        {
            // Δημιουργία νέου tab
            TabPage newTab = new TabPage("Calculator"); // Δώσε όνομα στο tab
            tabControl1.TabPages.Add(newTab); // Προσθήκη του νέου tab στο TabControl

            // Δημιουργία της αριθμομηχανής και προσθήκη της στο νέο tab
            ShowCalculator(newTab);
            // Προσθήκη κουμπιού κλεισίματος
            AddCloseButton(newTab);
        }
        private void ShowCalculator(TabPage tabPage)
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

            // Προσθήκη του TextBox στο tabPage
            tabPage.Controls.Add(resultBox);

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

                    // Προσθήκη του κουμπιού στο tabPage
                    tabPage.Controls.Add(button);
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

        private void CalendarMenuItem_Click(object sender, EventArgs e)
        {
            // Δημιουργία νέου tab για το ημερολόγιο
            TabPage newTab = new TabPage("Calendar"); // Δώσε όνομα στο tab
            tabControl1.TabPages.Add(newTab); // Προσθήκη του νέου tab στο TabControl

            // Δημιουργία του ημερολογίου και προσθήκη του στο νέο tab
            ShowCalendar(newTab);
            // Προσθήκη κουμπιού κλεισίματος
            AddCloseButton(newTab);
        }
        private void ShowCalendar(TabPage tabPage)
        {
            // Δημιουργία MonthCalendar
            MonthCalendar monthCalendar = new MonthCalendar
            {
                Dock = DockStyle.Fill, // Κάνει το ημερολόγιο να γεμίζει το tab
                ShowToday = true,
                MaxSelectionCount = 1 // Επιτρέπει την επιλογή μόνο μιας ημερομηνίας
            };

            // Προσθήκη του MonthCalendar στο tabPage
            tabPage.Controls.Add(monthCalendar);

            // Προσθήκη Label για εμφάνιση της τρέχουσας ημερομηνίας
            Label dateLabel = new Label
            {
                Text = $"Selected Date: {DateTime.Now.ToShortDateString()}",
                Dock = DockStyle.Bottom,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 14)
            };

            // Προσθήκη του Label στο tabPage
            tabPage.Controls.Add(dateLabel);

            // Προσθήκη event handler για την επιλογή ημερομηνίας
            monthCalendar.DateChanged += (s, args) =>
            {
                dateLabel.Text = $"Selected Date: {monthCalendar.SelectionStart.ToShortDateString()}";
            };
        }
        private void AddCloseButton(TabPage tabPage)
        {
            Button closeButton = new Button
            {
                Text = "X",
                Width = 30,
                Height = 30,
                Dock = DockStyle.Right,
                BackColor = Color.Red,
                ForeColor = Color.White
            };

            closeButton.Click += (s, e) =>
            {
                tabControl1.TabPages.Remove(tabPage); // Αφαίρεση του tab από το TabControl
            };

            tabPage.Controls.Add(closeButton); // Προσθήκη του κουμπιού κλεισίματος στο tab
        }
    }
}
