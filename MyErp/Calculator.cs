using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyErp
{
    public class Calculator
    {
        private double currentResult; // Αποθήκευση του τρέχοντος αποτελέσματος
        private string lastOperation; // Αποθήκευση της τελευταίας πράξης

        public Calculator()
        {
            currentResult = 0; // Αρχικοποίηση του αποτελέσματος
            lastOperation = string.Empty; // Αρχικοποίηση της τελευταίας πράξης
        }

        public double Add(double number)
        {
            currentResult += number; // Προσθήκη
            lastOperation = "+"; // Καταγραφή της τελευταίας πράξης
            return currentResult;
        }

        public double Subtract(double number)
        {
            currentResult -= number; // Αφαίρεση
            lastOperation = "-"; // Καταγραφή της τελευταίας πράξης
            return currentResult;
        }

        public double Multiply(double number)
        {
            currentResult *= number; // Πολλαπλασιασμός
            lastOperation = "*"; // Καταγραφή της τελευταίας πράξης
            return currentResult;
        }

        public double Divide(double number)
        {
            if (number != 0)
            {
                currentResult /= number; // Διαίρεση
            }
            else
            {
                throw new DivideByZeroException("Cannot divide by zero."); // Έλεγχος διαίρεσης με το μηδέν
            }
            lastOperation = "/"; // Καταγραφή της τελευταίας πράξης
            return currentResult;
        }

        public double GetResult()
        {
            return currentResult; // Επιστροφή του τρέχοντος αποτελέσματος
        }

        public void Clear()
        {
            currentResult = 0; // Εκκαθάριση του αποτελέσματος
            lastOperation = string.Empty; // Εκκαθάριση της τελευταίας πράξης
        }
    }
}
