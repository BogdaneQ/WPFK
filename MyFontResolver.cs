using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharp.Fonts;
using System.IO;


namespace WPFK
{
    public class MyFontResolver : IFontResolver
    {
        public byte[] GetFont(string faceName)
        {
            if (faceName == "Arial#Regular")
            {
                // Załaduj plik czcionki i zwróć bajty
                return File.ReadAllBytes("fonts/arial.ttf");
            }
            else if (faceName == "Arial#Bold")
            {
                return File.ReadAllBytes("fonts/arialbd.ttf");
            }
            // Dodaj inne warunki jeśli masz

            // Jeśli nie znaleziono czcionki, zwróć null lub rzuć wyjątek:
            throw new ArgumentException($"Font not found: {faceName}");
        }


        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            // Przykładowa implementacja - mapowanie nazw
            if (familyName.Equals("Arial", StringComparison.OrdinalIgnoreCase))
            {
                if (isBold) return new FontResolverInfo("Arial#Bold");
                return new FontResolverInfo("Arial#Regular");
            }
            return new FontResolverInfo("Arial#Regular");
        }
    }
}
