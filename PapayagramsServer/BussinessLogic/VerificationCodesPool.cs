using System.Collections.Generic;

namespace BussinessLogic
{
    public class VerificationCodesPool
    {
        private static Dictionary<string, string> _accountVerificationCodes = new Dictionary<string, string>();

        /// <summary>
        /// Generate a new account verification code for the specified username and remove any previous code
        /// </summary>
        /// <param name="username">Username of the player</param>
        /// <returns>The verification code</returns>
        public static string GenerateAccountVerificationCode(string username)
        {
            if (_accountVerificationCodes.ContainsKey(username))
            {
                _accountVerificationCodes.Remove(username);
            }
            string code;

            do
            {
                code = CodeGenerator.GenerateCode();
            } while (_accountVerificationCodes.ContainsValue(code));

            _accountVerificationCodes.Add(username, code);

            return code;
        }

        public static bool AccountVerificationCodeCorrect(string username, string code)
        {
            bool codeCorrect = false;
            if (_accountVerificationCodes.ContainsKey(username))
            {
                string codeStored = _accountVerificationCodes[username];
                codeCorrect = codeStored != null && codeStored.Equals(code);
            }
            return codeCorrect;
        }

        public static void RemoveAccountVerificationCode(string code)
        {
            _accountVerificationCodes.Remove(code);
        }
    }
}
