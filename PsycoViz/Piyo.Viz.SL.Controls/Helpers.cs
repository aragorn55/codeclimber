#region Disclaimer

// Copyright © Piyosailing.com 2008.
// This source is subject to the Microsoft Public License (Ms-PL).
// See the included License.txt file.
// All other rights reserved.

#endregion

using System;
using System.Windows.Media;

namespace Piyo.Viz.SL.Controls
{
    public class Helpers
    {
        public static Color HexToColor(String hexString)
            // Translates a html hexadecimal definition of a color into a .NET Framework Color.
            // The input string must start with a '#' character and be followed by 6 hexadecimal
            // digits. The digits A-F are not case sensitive. If the conversion was not successfull
            // the color white will be returned.
        {
            Color actColor;

            if ((hexString.StartsWith("#")) && (hexString.Length == 7))
            {
                byte r, g, b;
                r = HexToInt(hexString.Substring(1, 2));
                g = HexToInt(hexString.Substring(3, 2));
                b = HexToInt(hexString.Substring(5, 2));
                actColor = Color.FromRgb(r, g, b);
            }
            else
            {
                actColor = Colors.White;
            }
            return actColor;
        }

        private static byte HexToInt(string hexString)
        {
            return byte.Parse(hexString,
                              System.Globalization.NumberStyles.HexNumber, null);
        }
    }
}