using System;
using UnityEngine;
using Unity.VisualScripting;
[System.Serializable]
public struct TextElementsContent
{
    [SerializeField]
    public string _defaultTextVersion;
    [SerializeField]
    string _portugueseVersion;
    [SerializeField]
    string _englishVersion;
    [SerializeField]
    string _spanishVersion;
    [SerializeField]
    string _frenchVersion;

    public string DefaultTextVersion => _defaultTextVersion;
    public string PortugueseVersion => _portugueseVersion;
    public string EnglishVersion => _englishVersion;
    public string SpanishVersion => _spanishVersion;
    public string FrenchVersion => _frenchVersion;

    public TextElementsContent(
        string portugueseText,
        string englishText,
        string spanishText,
        string frenchText,
        string defaultText)
    {
        _portugueseVersion = portugueseText;
        _englishVersion = englishText;
        _spanishVersion = spanishText;
        _frenchVersion = frenchText;
        _defaultTextVersion = defaultText;
    }

    public static string GetTextByLanguage(TextElementsContent textContent, GameLanguages language)
    {
        string text = language switch
        {
            GameLanguages.Portuguese => textContent._portugueseVersion,
            GameLanguages.English => textContent._englishVersion,
            GameLanguages.Spanish => textContent._spanishVersion,
            GameLanguages.French => textContent._frenchVersion,
            _ => textContent._defaultTextVersion
        };

        return string.IsNullOrEmpty(text) ? textContent._defaultTextVersion : text;
    }
}
