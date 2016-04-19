using System.IO;

namespace BankOcrProblem
{
    class DataSource
    {
        public TextReader Open(string name)
        {
            return new StreamReader("C:\\stash\\lmg\\dojo\\BankOcrProblem\\BankOcrProblem\\Data\\" + name + ".txt");
        }
    }
}
