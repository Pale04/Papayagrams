﻿using System.Collections.Generic;

namespace BussinessLogic
{
    public static class VerificationCodesPool
    {
        private static Dictionary<string, string> _accountVerificationCodes = new Dictionary<string, string>();
        private static Dictionary<string, string> _passwordRecoveryPINes = new Dictionary<string, string>();

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

        public static string GeneratePasswordRecoveryPIN(string username)
        {
            if (_passwordRecoveryPINes.ContainsKey(username))
            {
                _passwordRecoveryPINes.Remove(username);
            }
            
            string pin;
            do
            {
                pin = CodeGenerator.GenerateCode();
            } while (_passwordRecoveryPINes.ContainsValue(pin));

            _passwordRecoveryPINes.Add(username, pin);
            return pin;
        }

        public static bool PasswordRecoveryPINCorrect(string email, string pin)
        {
            bool pinCorrect = false;
            if (_passwordRecoveryPINes.ContainsKey(email))
            {
                pinCorrect = pin.Equals(_passwordRecoveryPINes[email]);
            }
            return pinCorrect;
        }

        public static void RemovePasswordRecoveryPIN(string email)
        {
            _passwordRecoveryPINes.Remove(email);
        }
    }
}
