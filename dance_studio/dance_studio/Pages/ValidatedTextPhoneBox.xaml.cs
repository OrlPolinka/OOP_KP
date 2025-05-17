using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace dance_studio.Pages
{
    public partial class ValidatedTextPhoneBox : UserControl
    {
        public ValidatedTextPhoneBox()
        {
            InitializeComponent();

        }

        // DependencyProperty Text с валидацией и коррекцией
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
    nameof(Text),
    typeof(string),
    typeof(ValidatedTextPhoneBox),
    new FrameworkPropertyMetadata("",
        FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
        OnTextChanged,
        CoerceText));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }


        // DependencyProperty IsValid (только для чтения)
        private static readonly DependencyPropertyKey IsValidPropertyKey = DependencyProperty.RegisterReadOnly(
            nameof(IsValid),
            typeof(bool),
            typeof(ValidatedTextPhoneBox),
            new PropertyMetadata(false));

        public static readonly DependencyProperty IsValidProperty = IsValidPropertyKey.DependencyProperty;

        public bool IsValid
        {
            get => (bool)GetValue(IsValidProperty);
            private set => SetValue(IsValidPropertyKey, value);
        }


        // ValidateValueCallback: очень простая проверка
        private static bool ValidatePhoneStatic(object value)
        {
            if (value == null) return true;
            if (value is string str)
            {
                if (string.IsNullOrWhiteSpace(str)) return true; // пустое разрешено
                return Regex.IsMatch(str, @"^\+375\s\((29|33)\)\s\d{3}-\d{2}-\d{2}$");
            }
            return false;
        }

        // CoerceValueCallback: только форматируем, иначе пустая строка
        private static object CoerceText(DependencyObject d, object baseValue)
        {
            if (baseValue is string input)
            {
                string digits = Regex.Replace(input, @"\D", "");
                if (digits.StartsWith("375") && digits.Length == 12)
                {
                    string code = digits.Substring(3, 2);
                    if (code == "29" || code == "33")
                    {
                        return $"+375 ({code}) {digits.Substring(5, 3)}-{digits.Substring(8, 2)}-{digits.Substring(10, 2)}";
                    }
                }
            }
            return baseValue;
        }

        // Убери прямое присваивание в OnTextChanged, если есть Binding
        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (ValidatedTextPhoneBox)d;
            string newVal = (string)e.NewValue;

            // Обновляем TextBox, только если текст реально отличается (чтобы избежать рекурсии)
            if (ctrl.PhoneTextBox.Text != newVal)
                ctrl.PhoneTextBox.Text = newVal;

            // Проверяем валидность и обновляем свойство IsValid
            ctrl.IsValid = ctrl.ValidatePhone(newVal);
        }

        public bool ValidatePhone(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return true; // разрешаем пустое

            return Regex.IsMatch(input, @"^\+375\s\((29|33)\)\s\d{3}-\d{2}-\d{2}$");
        }

        // --- Routed Events ---

        // Direct event — только в текущем элементе
        public static readonly RoutedEvent PhoneDirectEvent = EventManager.RegisterRoutedEvent(
            nameof(PhoneDirect),
            RoutingStrategy.Direct,
            typeof(RoutedEventHandler),
            typeof(ValidatedTextPhoneBox));

        public event RoutedEventHandler PhoneDirect
        {
            add => AddHandler(PhoneDirectEvent, value);
            remove => RemoveHandler(PhoneDirectEvent, value);
        }

        // Tunneling event (Preview)
        public static readonly RoutedEvent PhoneValidatedPreviewEvent = EventManager.RegisterRoutedEvent(
            nameof(PhoneValidatedPreview),
            RoutingStrategy.Tunnel,
            typeof(RoutedEventHandler),
            typeof(ValidatedTextPhoneBox));

        public event RoutedEventHandler PhoneValidatedPreview
        {
            add => AddHandler(PhoneValidatedPreviewEvent, value);
            remove => RemoveHandler(PhoneValidatedPreviewEvent, value);
        }

        // Bubbling event
        public static readonly RoutedEvent PhoneValidatedEvent = EventManager.RegisterRoutedEvent(
            nameof(PhoneValidated),
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(ValidatedTextPhoneBox));

        public event RoutedEventHandler PhoneValidated
        {
            add => AddHandler(PhoneValidatedEvent, value);
            remove => RemoveHandler(PhoneValidatedEvent, value);
        }


        // Обработчик LostFocus
        private void PhoneTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            // При потере фокуса вызываем коррекцию и присваиваем Text (вызывает OnTextChanged и обновляет IsValid)
            Text = CoerceText(this, PhoneTextBox.Text) as string ?? PhoneTextBox.Text;

            if (!ValidatePhone(Text))
            {
                MessageBox.Show("Неверный формат номера телефона. Ожидается: +375 (29) 123-45-67 или +375 (33) 123-45-67",
                    "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Вспомогательный метод коррекции, чтобы не дублировать код
        private string CoercePhone(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            string digits = Regex.Replace(input, @"\D", "");

            if (digits.StartsWith("375") && digits.Length == 12)
            {
                string operatorCode = digits.Substring(3, 2);
                if (operatorCode == "29" || operatorCode == "33")
                {
                    return $"+375 ({operatorCode}) {digits.Substring(5, 3)}-{digits.Substring(8, 2)}-{digits.Substring(10, 2)}";
                }
            }
            return input;
        }

        // В синтаксисе XAML должен быть TextBox с именем PhoneTextBox, привязанный к LostFocus:
        // <TextBox x:Name="PhoneTextBox" LostFocus="PhoneTextBox_LostFocus"/>
    }
}




//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
//using System.Windows.Shapes;

//namespace dance_studio.Pages
//{
//    /// <summary>
//    /// Логика взаимодействия для ValidatedTextPhoneBox.xaml
//    /// </summary>
//    public partial class ValidatedTextPhoneBox : UserControl
//    {
//        public ValidatedTextPhoneBox()
//        {
//            InitializeComponent();
//        }

//        //    // DependencyProperty с валидацией и коррекцией
//        //    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
//        //        "Text",
//        //        typeof(string),
//        //        typeof(ValidatedTextPhoneBox),
//        //        new FrameworkPropertyMetadata("+375 (29) 000-00-00",  // здесь не пустая строка, а валидный номер
//        //    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
//        //    null,
//        //    CoerceText,
//        //    true,
//        //    UpdateSourceTrigger.PropertyChanged),
//        //ValidatePhone);

//        //    public string Text
//        //    {
//        //        get => (string)GetValue(TextProperty);
//        //        set => SetValue(TextProperty, value);
//        //    }

//        //    private static bool ValidatePhone(object value)
//        //    {
//        //        try
//        //        {
//        //            if (value is string str)
//        //            {
//        //                return Regex.IsMatch(str, @"^\+375\s\((29|33)\)\s\d{3}-\d{2}-\d{2}$");
//        //            }
//        //        }
//        //        catch { }
//        //        return false;
//        //    }

//        //    private static object CoerceText(DependencyObject d, object baseValue)
//        //    {
//        //        try
//        //        {
//        //            if (baseValue is string input)
//        //            {
//        //                string digits = Regex.Replace(input, @"\D", "");
//        //                if (digits.Length == 12 && digits.StartsWith("375"))
//        //                {
//        //                    string code = digits.Substring(3, 2);
//        //                    if (code == "29" || code == "33")
//        //                    {
//        //                        return $"+375 ({code}) {digits.Substring(5, 3)}-{digits.Substring(8, 2)}-{digits.Substring(10, 2)}";
//        //                    }
//        //                }
//        //            }
//        //        }
//        //        catch { }
//        //        return baseValue;
//        //    }


//        //    // RoutedEvent Tunneling (Preview)
//        //    public static readonly RoutedEvent PhoneValidatedPreviewEvent = EventManager.RegisterRoutedEvent(
//        //        "PhoneValidatedPreview",
//        //        RoutingStrategy.Tunnel,
//        //        typeof(RoutedEventHandler),
//        //        typeof(ValidatedTextPhoneBox));

//        //    public event RoutedEventHandler PhoneValidatedPreview
//        //    {
//        //        add => AddHandler(PhoneValidatedPreviewEvent, value);
//        //        remove => RemoveHandler(PhoneValidatedPreviewEvent, value);
//        //    }

//        //    // RoutedEvent Bubbling
//        //    public static readonly RoutedEvent PhoneValidatedEvent = EventManager.RegisterRoutedEvent(
//        //        "PhoneValidated",
//        //        RoutingStrategy.Bubble,
//        //        typeof(RoutedEventHandler),
//        //        typeof(ValidatedTextPhoneBox));

//        //    public event RoutedEventHandler PhoneValidated
//        //    {
//        //        add => AddHandler(PhoneValidatedEvent, value);
//        //        remove => RemoveHandler(PhoneValidatedEvent, value);
//        //    }

//        //    private void PhoneTextBox_LostFocus(object sender, RoutedEventArgs e)
//        //    {
//        //        // Обновляем значение свойства Text из TextBox
//        //        Text = PhoneTextBox.Text;

//        //        // Применяем коррекцию формата
//        //        CoerceValue(TextProperty);

//        //        // Проверяем валидность
//        //        if (!ValidatePhone(Text))
//        //        {
//        //            MessageBox.Show("Неверный формат номера телефона. Ожидается: +375 (29) 123-45-67 или +375 (33) 123-45-67",
//        //                "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
//        //        }
//        //        else
//        //        {
//        //            // Если валиден, можно поднять событие валидации
//        //            RaiseEvent(new RoutedEventArgs(PhoneValidatedEvent, this));
//        //        }

//        //        //// Коррекция
//        //        //CoerceValue(TextProperty);

//        //        //// Генерация Preview (Tunnel)
//        //        //RaiseEvent(new RoutedEventArgs(PhoneValidatedPreviewEvent, this));

//        //        //if (ValidatePhone(Text))
//        //        //{
//        //        //    // Генерация Bubbling
//        //        //    RaiseEvent(new RoutedEventArgs(PhoneValidatedEvent, this));
//        //        //}
//        //    }


//        public string Text
//        {
//            get => PhoneTextBox.Text;
//            set => PhoneTextBox.Text = value;
//        }

//        // Метод проверки валидности белорусского номера
//        private bool ValidatePhone(string input)
//        {
//            if (string.IsNullOrWhiteSpace(input))
//                return true; // пустое значение разрешено

//            // Регулярное выражение для +375 (29) 123-45-67 или +375 (33) 123-45-67
//            string pattern = @"^\+375\s\((29|33)\)\s\d{3}-\d{2}-\d{2}$";
//            return Regex.IsMatch(input, pattern);
//        }

//        // Метод форматирования/коррекции введённого текста
//        private string CoercePhone(string input)
//        {
//            if (string.IsNullOrWhiteSpace(input))
//                return string.Empty;

//            // Убираем всё, кроме цифр
//            string digits = Regex.Replace(input, @"\D", "");

//            // Проверяем, что номер начинается с 375 и далее 9 цифр
//            if (digits.StartsWith("375") && digits.Length == 12)
//            {
//                // Берём код оператора - 2 цифры после 375
//                string operatorCode = digits.Substring(3, 2);
//                if (operatorCode == "29" || operatorCode == "33")
//                {
//                    // Формируем номер в виде +375 (XX) XXX-XX-XX
//                    string formatted = $"+375 ({operatorCode}) {digits.Substring(5, 3)}-{digits.Substring(8, 2)}-{digits.Substring(10, 2)}";
//                    return formatted;
//                }
//            }

//            // Если формат не подходит, возвращаем исходное значение без изменений
//            return input;
//        }

//        // Обработка события потери фокуса TextBox
//        private void PhoneTextBox_LostFocus(object sender, RoutedEventArgs e)
//        {
//            // Корректируем текст
//            string coerced = CoercePhone(PhoneTextBox.Text);
//            PhoneTextBox.Text = coerced;

//            // Проверяем валидность
//            if (!ValidatePhone(coerced))
//            {
//                MessageBox.Show("Неверный формат номера телефона. Ожидается: +375 (29) 123-45-67 или +375 (33) 123-45-67",
//                    "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
//            }
//        }
//    }
//}
