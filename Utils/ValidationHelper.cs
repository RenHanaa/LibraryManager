using System;

namespace LibraryManager.Utils
{
    public static class ValidationHelper
    {
        public static bool ValidateBook(string title, string author, int year, string isbn, int quantity, string genre, out string errorMessage)
        {
            if (IsEmptyString(title))
            {
                errorMessage = "Название не может быть пустым";
                return false;
            }

            if (IsEmptyString(author))
            {
                errorMessage = "Автор не может быть пустым";
                return false;
            }

            if (year < 1000 || year > DateTime.Now.Year)
            {
                errorMessage = "Неверный год издания";
                return false;
            }

            if (!IsEmptyString(isbn) && !ValidateIsbn(isbn))
            {
                errorMessage = "Неверный формат ISBN";
                return false;
            }

            if (quantity < 0)
            {
                errorMessage = "Количество не может быть отрицательным";
                return false;
            }

            if (IsEmptyString(genre))
            {
                errorMessage = "Жанр не может быть пустым";
                return false;
            }

            errorMessage = "OK";
            return true;
        }

        public static bool ValidateIsbn(string isbn)
        {
            if (IsEmptyString(isbn))
                return false;

            string normalizedIsbn = isbn.Replace("-", "");

            if (normalizedIsbn.Length != 13)
                return false;

            return IsDigitsOnly(normalizedIsbn);
        }

        public static bool ValidateReader(string lastName, string firstName, string middleName, string phone, string email, out string errorMessage)
        {
            if (IsEmptyString(lastName))
            {
                errorMessage = "Фамилия не может быть пустой";
                return false;
            }

            if (IsEmptyString(firstName))
            {
                errorMessage = "Имя не может быть пустым";
                return false;
            }

            if (IsEmptyString(middleName))
            {
                errorMessage = "Отчество не может быть пустым";
                return false;
            }

            if (!ValidatePhone(phone))
            {
                errorMessage = "Неверный формат телефона";
                return false;
            }

            if (!ValidateEmail(email))
            {
                errorMessage = "Неверный формат email";
                return false;
            }

            errorMessage = "OK";
            return true;
        }

        public static bool ValidatePhone(string phone)
        {
            if (IsEmptyString(phone))
                return false;

            if (!phone.StartsWith("+7-"))
                return false;

            string normalizedPhone = phone.Replace("+7-", "").Replace("-", "");

            if (normalizedPhone.Length != 10)
                return false;

            return IsDigitsOnly(normalizedPhone);
        }

        public static bool ValidateEmail(string email)
        {
            if (IsEmptyString(email))
                return false;

            int atIndex = email.IndexOf('@');
            int dotIndex = email.LastIndexOf('.');

            if (atIndex <= 0)
                return false;

            if (dotIndex < atIndex + 2)
                return false;

            if (dotIndex >= email.Length - 1)
                return false;

            return true;
        }

        public static bool IsEmptyString(string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static bool IsDigitsOnly(string value)
        {
            if (IsEmptyString(value))
                return false;

            foreach (char symbol in value)
            {
                if (!char.IsDigit(symbol))
                    return false;
            }

            return true;
        }
    }
}