namespace Hospisim.Business.Common
{
    public static class CpfHelper
    {
        public static bool IsValid(string cpf)
        {
            cpf = new string(cpf.Where(char.IsDigit).ToArray());
            return cpf.Length == 11 && cpf.Distinct().Count() > 1;
        }
    }
}
