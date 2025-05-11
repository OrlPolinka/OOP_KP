using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace dance_studio
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    /// 
    public class Lang
    {
        public static string currentLanguageCode = "ru";
    }

    public partial class App : Application
    {
        public void ChangeLanguage(string languageCode)
        {
            ResourceDictionary dict = new ResourceDictionary();

            if (languageCode == "en")
            {
                Lang.currentLanguageCode = "en";
                dict.Source = new Uri("Resources.en.xaml", UriKind.Relative);
            }
            else if (languageCode == "ru")
            {
                Lang.currentLanguageCode = "ru";
                dict.Source = new Uri("Resources.ru.xaml", UriKind.Relative);
            }

            // Удаляем предыдущий словарь (русский/английский)
            var oldDict = Resources.MergedDictionaries.FirstOrDefault(
                d => d.Source != null && (d.Source.OriginalString.StartsWith("Resources"))
            );

            if (oldDict != null)
            {
                Resources.MergedDictionaries.Remove(oldDict);
            }

            // Добавляем новый словарь
            Resources.MergedDictionaries.Add(dict);
        }
    }
}
