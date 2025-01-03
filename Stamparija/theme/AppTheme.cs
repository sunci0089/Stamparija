using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Stamparija.theme
{
    class AppTheme
    {
        //internacionalizacija todo
       private static void ClearResources() {
            Application.Current.Resources.MergedDictionaries.Clear();
       }
       public static void LoadDefaultTheme()
       {
            Settings.Default.FontSize = "medium";
            Settings.Default.Theme= "light";
            Settings.Default.Theme = "eng";
            LoadTheme();
        } 
       public static void ChangeTheme(Uri themeuri)
        {
            ResourceDictionary Theme = new ResourceDictionary() { Source = themeuri };
            ClearResources();
            Application.Current.Resources.MergedDictionaries.Add(Theme);
        }

        public static void ChangeFontSize(double small, double medium, double large) {
            Application.Current.Resources["FontSizeSmall"] = small;
            Application.Current.Resources["FontSizeMedium"] = medium;
            Application.Current.Resources["FontSizeLarge"] = large;
        }

        public static void LoadFontSize()
        {
            switch (Settings.Default.FontSize)
            {
                case "small":
                    ChangeFontSize(12, 14, 16);
                    break;
                case "medium":
                    ChangeFontSize(14, 16, 18);
                    break;
                case "large":
                    ChangeFontSize(16, 18, 20);
                    break;
                default:
                    ChangeFontSize(12, 14, 16);
                    break;
            }
        }
        public static void LoadThemeColor()
        {
            switch (Settings.Default.Theme)
            {
                case "light":
                    ChangeTheme(new Uri("theme/LightTheme.xaml", UriKind.Relative));
                    break;
                case "dark":
                    ChangeTheme(new Uri("theme/DarkTheme.xaml", UriKind.Relative));
                    break;
                default:
                    ChangeTheme(new Uri("theme/LightTheme.xaml", UriKind.Relative));
                    break;
            }
        }
        public static void LoadTheme() {
            LoadThemeColor();
            LoadFontSize();
        }
    }
}
